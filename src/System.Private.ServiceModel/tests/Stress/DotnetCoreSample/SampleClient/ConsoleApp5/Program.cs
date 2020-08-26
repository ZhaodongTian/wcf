using System;
using System.Diagnostics;
using System.Xml.Schema;
using Benchmarks;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
            binding.MaxBufferSize = int.MaxValue;
            binding.ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max;
            binding.MaxReceivedMessageSize = int.MaxValue;
            binding.AllowCookies = true;
           var address= new System.ServiceModel.EndpointAddress(args[0]);
            ServiceReference1.SayHelloClient client = new ServiceReference1.SayHelloClient(binding,address);
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result= client.HelloAsync("helloworld");
            var elapsed = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"{elapsed} ms");

            BenchmarksEventSource.Log.Metadata("http/firstrequest", "max", "max", "First Request (ms)", "Time to first request in ms", "n0");
            BenchmarksEventSource.Measure("http/firstrequest", elapsed);
            Console.WriteLine(result.Result);

            var startTime = DateTime.Now;
            int request = 0;
            while (DateTime.Now <= startTime.AddMinutes(2))
            {
                client.HelloAsync("helloworld");
                request++;
            }
            Console.WriteLine(request);
            BenchmarksEventSource.Log.Metadata("bombardier/requests", "max", "sum", "Requests", "Total number of requests", "n0");
            BenchmarksEventSource.Measure("bombardier/requests", request);

            BenchmarksEventSource.Log.Metadata("bombardier/rps/max", "max", "sum", "Requests/sec (max)", "Max requests per second", "n0");

            BenchmarksEventSource.Measure("bombardier/rps/max", request/120);
        }
    }
}
