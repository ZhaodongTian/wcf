﻿using System.ServiceModel;
namespace WcfServices.Service.Interface
{
    [ServiceContract(Name = "CalculatorService")]
    public interface ICalculator
    {
        [OperationContract]
        double Add(double x, double y);

        [OperationContract]
        double Subtract(double x, double y);

        [OperationContract]
        double Multiply(double x, double y);

        [OperationContract]
        double Divide(double x, double y);
    }
}
