using System;
using System.ServiceModel;
using WcfServices.Service.Interface;
namespace Artech.WcfServices.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string protocal = args[0];
            if (protocal == "nettcp")
            {
                using (ChannelFactory<ICalculator> channelFactory = new ChannelFactory<ICalculator>("nettcp"))
                {
                    ICalculator proxy = channelFactory.CreateChannel();
                    for (int i = 0; i <= 10; i++)
                    {

                        Console.WriteLine("x + y = {2} when x = {0} and y = {1}", 1, 2, proxy.Add(1, 2));
                        Console.WriteLine("x - y = {2} when x = {0} and y = {1}", 1, 2, proxy.Subtract(1, 2));
                        Console.WriteLine("x * y = {2} when x = {0} and y = {1}", 1, 2, proxy.Multiply(1, 2));
                        Console.WriteLine("x / y = {2} when x = {0} and y = {1}", 1, 2, proxy.Divide(1, 2));
                        System.Threading.Thread.Sleep(5000);
                    }
                }
            }
            else if (protocal == "http")
            {
                using (ChannelFactory<ICalculator> channelFactory = new ChannelFactory<ICalculator>("http"))
                {
                    ICalculator proxy = channelFactory.CreateChannel();
                    for (int i = 0; i <= 10; i++)
                    {

                        Console.WriteLine("x + y = {2} when x = {0} and y = {1}", 1, 2, proxy.Add(1, 2));
                        Console.WriteLine("x - y = {2} when x = {0} and y = {1}", 1, 2, proxy.Subtract(1, 2));
                        Console.WriteLine("x * y = {2} when x = {0} and y = {1}", 1, 2, proxy.Multiply(1, 2));
                        Console.WriteLine("x / y = {2} when x = {0} and y = {1}", 1, 2, proxy.Divide(1, 2));
                        System.Threading.Thread.Sleep(5000);
                    }
                }
            }
            Console.Read();
        }
    }
}
