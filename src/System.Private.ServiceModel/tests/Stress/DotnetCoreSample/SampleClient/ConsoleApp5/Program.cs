using System;

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
           var result= client.HelloAsync("helloworld");
            Console.WriteLine(result.Result);
        }
    }
}
