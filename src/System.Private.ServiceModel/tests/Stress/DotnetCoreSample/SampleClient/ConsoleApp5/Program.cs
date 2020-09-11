using System;
using System.Diagnostics;
using System.ServiceModel;
using Benchmarks;

namespace ConsoleApp5
{
    public class Parameters
    {
        public const string Binding = "binding";
        public const string ServiceUrl = "serviceurl";
        public const string TransferMode = "transfermode";
        public const string ReportingUrl = "reportingurl";
        public const string PerfMeasurementDuration = "perfmeasurementduration";
    }
    public enum TestBinding { BasicHttp, WSHttp, NetTcp }

    class Program
    {
        // Parameters and default values

        private TestBinding _paramBinding = TestBinding.BasicHttp;
        private TimeSpan _paramPerfMeasurementDuration = s_defaultPerfMeasurementDuration;
        private string _paramServiceUrl = "";
        private readonly static TimeSpan s_defaultPerfMeasurementDuration = TimeSpan.FromSeconds(10);
        private string _paramTransferMode = "Buffered";

        static void Main(string[] args)
        {
            Program test = new Program();
            if (test._paramBinding == TestBinding.NetTcp)
            {
                BenchmarksEventSource.Log.Metadata("NetTcp/Channel Open", "max", "max", "Channel Open Time (ms)", "Time to Open Channel in ms", "n0");
                BenchmarksEventSource.Log.Metadata("NetTcp/firstrequest", "max", "max", "First Request (ms)", "Time to first request in ms", "n0");
            }
            else
            {
                BenchmarksEventSource.Log.Metadata("http/Channel Open", "max", "max", "Channel Open Time (ms)", "Time to Open Channel in ms", "n0");
                BenchmarksEventSource.Log.Metadata("http/firstrequest", "max", "max", "First Request (ms)", "Time to first request in ms", "n0");
            }
            
            BenchmarksEventSource.Log.Metadata("bombardier/requests", "max", "sum", "Requests", "Total number of requests", "n0");
            BenchmarksEventSource.Log.Metadata("bombardier/rps/max", "max", "sum", "Requests/sec (max)", "Max requests per second", "n0");
           
            if (test.ProcessRunOptions(args))
            {
                var startTime = DateTime.Now;
                int request = 0;
                switch (test._paramBinding)
                {
                    case TestBinding.BasicHttp:
                        BasicHttpBinding binding = new BasicHttpBinding();
                        switch (test._paramTransferMode.ToLower())
                        {
                            case "buffered":
                                binding.TransferMode = TransferMode.Buffered;
                                break;
                            case "streamed":
                                binding.TransferMode = TransferMode.Streamed;
                                break;
                            case "streamedrequest":
                                binding.TransferMode = TransferMode.StreamedRequest;
                                break;
                            case "streamedresponse":
                                binding.TransferMode = TransferMode.StreamedResponse;
                                break;
                            default:
                                break;
                        }
                        Console.WriteLine($"Testing TransferMode: {binding.TransferMode}");
                        ChannelFactory<ISayHello> factory = new ChannelFactory<ISayHello>(binding, new EndpointAddress(test._paramServiceUrl));
                        var stopwatchChannelOpen = new Stopwatch();
                        stopwatchChannelOpen.Start();
                        factory.Open();
                        BenchmarksEventSource.Measure("http/Channel Open", stopwatchChannelOpen.ElapsedMilliseconds);

                        var client = factory.CreateChannel();
                        var stopwatchFirstReq = new Stopwatch();
                        stopwatchFirstReq.Start();
                        var result = client.HelloAsync("helloworld").Result;
                        BenchmarksEventSource.Measure("http/firstrequest", stopwatchFirstReq.ElapsedMilliseconds);
                        
                        while (DateTime.Now <= startTime.Add(test._paramPerfMeasurementDuration))
                        {
                            var rtnResult=client.HelloAsync("helloworld").Result;
                            request++;
                        }

                        BenchmarksEventSource.Measure("bombardier/requests", request);
                        BenchmarksEventSource.Measure("bombardier/rps/max", request / test._paramPerfMeasurementDuration.TotalSeconds);
                        break;
                    case TestBinding.WSHttp:
                        WSHttpBinding wsHttpBinding = new WSHttpBinding(SecurityMode.TransportWithMessageCredential);
                        ChannelFactory<ISayHello> wsHttpFactory = new ChannelFactory<ISayHello>(wsHttpBinding, new EndpointAddress(test._paramServiceUrl));
                        wsHttpFactory.Credentials.UserName.UserName = "abc";
                        wsHttpFactory.Credentials.UserName.Password = "abc";
                        var stopwatchWSHttpChannelOpen = new Stopwatch();
                        stopwatchWSHttpChannelOpen.Start();
                        BenchmarksEventSource.Measure("http/Channel Open", stopwatchWSHttpChannelOpen.ElapsedMilliseconds);
                        var clientWSHttp = wsHttpFactory.CreateChannel();
                        var stopwatchWSHttpFirstReq = new Stopwatch();
                        stopwatchWSHttpFirstReq.Start();
                        Console.WriteLine(clientWSHttp.HelloAsync("helloworld").Result);
                        BenchmarksEventSource.Measure("http/firstrequest", stopwatchWSHttpFirstReq.ElapsedMilliseconds);

                        while (DateTime.Now <= startTime.Add(test._paramPerfMeasurementDuration))
                        {
                            var rtnResult = clientWSHttp.HelloAsync("helloworld").Result;
                            request++;
                        }

                        BenchmarksEventSource.Measure("bombardier/requests", request);
                        BenchmarksEventSource.Measure("bombardier/rps/max", request / test._paramPerfMeasurementDuration.TotalSeconds);
                        break;
                    case TestBinding.NetTcp:                        
                        NetTcpBinding netTcpBinding = new NetTcpBinding() ;
                        ChannelFactory<IService1> netTcpFactory = new ChannelFactory<IService1>(netTcpBinding, new EndpointAddress(test._paramServiceUrl));
                        var stopwatchNetTcpChannelOpen = new Stopwatch();
                        stopwatchNetTcpChannelOpen.Start();
                        BenchmarksEventSource.Measure("NetTcp/Channel Open", stopwatchNetTcpChannelOpen.ElapsedMilliseconds);

                        var clientNetTcp = netTcpFactory.CreateChannel();

                        var stopwatchNetTcpFirstReq = new Stopwatch();
                        stopwatchNetTcpFirstReq.Start();
                        var netTcpResult = clientNetTcp.GetDataAsync(1).Result;
                        BenchmarksEventSource.Measure("http/firstrequest", stopwatchNetTcpFirstReq.ElapsedMilliseconds);
                       
                        while (DateTime.Now <= startTime.Add(test._paramPerfMeasurementDuration))
                        {
                            var rtnResult = clientNetTcp.GetDataAsync(1).Result;
                            request++;
                        }

                        BenchmarksEventSource.Measure("bombardier/requests", request);
                        BenchmarksEventSource.Measure("bombardier/rps/max", request / test._paramPerfMeasurementDuration.TotalSeconds);
                        break;
                }
            }


            //var address = new System.ServiceModel.EndpointAddress(args[0]);
            //string transferMode = args[1];

            //System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
            //binding.MaxBufferSize = int.MaxValue;
            //binding.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
            //binding.MaxReceivedMessageSize = int.MaxValue;
            //binding.AllowCookies = true;
            //switch (transferMode.ToLower())
            //{
            //    case "buffered":
            //        binding.TransferMode = System.ServiceModel.TransferMode.Buffered;
            //        break;
            //    case "streamed":
            //        binding.TransferMode = System.ServiceModel.TransferMode.Streamed;
            //        break;
            //    case "streamedrequest":
            //        binding.TransferMode = System.ServiceModel.TransferMode.StreamedRequest;
            //        break;
            //    case "streamedresponse":
            //        binding.TransferMode = System.ServiceModel.TransferMode.StreamedResponse;
            //        break;
            //    default:
            //        break;
            //}
            //Console.WriteLine($"Testing TransferMode: {binding.TransferMode}");

            // ServiceReference1.SayHelloClient client = new ServiceReference1.SayHelloClient(binding, address);
            //var stopwatch = new Stopwatch();
            //stopwatch.Start();
            //var result = client.HelloAsync("helloworld");
            //var elapsed = stopwatch.ElapsedMilliseconds;
            //Console.WriteLine($"{elapsed} ms");

            //BenchmarksEventSource.Log.Metadata("http/firstrequest", "max", "max", "First Request (ms)", "Time to first request in ms", "n0");
            //BenchmarksEventSource.Measure("http/firstrequest", elapsed);
            //Console.WriteLine(result.Result);

            //var startTime = DateTime.Now;
            //int request = 0;
            //while (DateTime.Now <= startTime.AddMinutes(2))
            //{
            //    client.HelloAsync("helloworld");
            //    request++;
            //}
            //Console.WriteLine(request);
            //BenchmarksEventSource.Log.Metadata("bombardier/requests", "max", "sum", "Requests", "Total number of requests", "n0");
            //BenchmarksEventSource.Measure("bombardier/requests", request);

            //BenchmarksEventSource.Log.Metadata("bombardier/rps/max", "max", "sum", "Requests/sec (max)", "Max requests per second", "n0");

            //BenchmarksEventSource.Measure("bombardier/rps/max", request / 120);
        }

        private bool ProcessRunOptions(string[] args)
        {
            foreach (string s in args)
            {
                Console.WriteLine(s);
                string[] p = s.Split(new char[] { ':' }, count: 2);
                if (p.Length != 2)
                {
                    continue;
                }

                switch (p[0].ToLower())
                {

                    case Parameters.Binding:
                        if (!Enum.TryParse<TestBinding>(p[1], ignoreCase: true, result: out _paramBinding))
                        {
                            return ReportWrongArgument(s);
                        }
                        break;

                    case Parameters.PerfMeasurementDuration:
                        int perfPerfMeasurementDurationSeconds = 0;
                        if (!Int32.TryParse(p[1], out perfPerfMeasurementDurationSeconds))
                        {
                            return ReportWrongArgument(s);
                        }
                        _paramPerfMeasurementDuration = TimeSpan.FromSeconds(perfPerfMeasurementDurationSeconds);
                        break;

                    case Parameters.ServiceUrl:
                        _paramServiceUrl = p[1];
                        break;

                    case Parameters.TransferMode:
                        _paramTransferMode = p[1];
                        break;
                    default:
                        Console.WriteLine("unknown argument: " + s);
                        continue;
                }
            }

            return true;
        }

        private bool ReportWrongArgument(string arg)
        {
            Console.WriteLine("Wrong parameter: " + arg);
            return false;
        }
    }
}
