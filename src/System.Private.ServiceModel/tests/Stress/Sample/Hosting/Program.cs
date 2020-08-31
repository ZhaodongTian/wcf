using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using WcfServices.Service.Interface;
using WcfServices.Service;
namespace WcfServices.Hosting
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ServiceHost host = new ServiceHost(typeof(CalculatorService)))
            {
                host.Opened += delegate
                {
                    Console.WriteLine("CalculaorService has started , any key to stop！");
                };

                host.Open();
                Console.Read();
            }
        }
    }
}
