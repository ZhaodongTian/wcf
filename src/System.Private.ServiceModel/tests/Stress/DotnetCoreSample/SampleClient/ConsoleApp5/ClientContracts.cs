using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp5
{   
    [ServiceContract]
    public interface ISayHello
    {
        [OperationContract]
        Task<string> HelloAsync(string name);
    }

    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        Task<string> GetDataAsync(int value);      
    }
}
