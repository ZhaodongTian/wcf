using System;

namespace ConsoleApp5
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceReference1.SayHelloClient client = new ServiceReference1.SayHelloClient();
           var result= client.HelloAsync("helloworld");
            Console.WriteLine(result.Result);
        }
    }
}
