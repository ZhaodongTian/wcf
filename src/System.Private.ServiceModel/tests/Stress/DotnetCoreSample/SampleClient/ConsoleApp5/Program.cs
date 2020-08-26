using System;
using System.Diagnostics;
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
        }
    }
}
