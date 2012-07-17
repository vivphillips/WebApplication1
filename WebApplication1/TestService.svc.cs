using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace WebApplication1
{
    [ServiceContract(Namespace = "WebApplication1")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TestService
    {
 
        [OperationContract]
        public string SayHello(string s)
        {
            return "Hello " + s;
        }

        [OperationContract]
        public string SayGoodbye(string s)
        {
            return "Goodbye " + s;
        }

        [OperationContract]
        public Car GetCar()
        {
            return new Car { Make = "Ford", Model = "Escort" };
        }

        [OperationContract]
        public Car UpgradeCar(Car c)
        {
            c.Model = c.Model + " GT";
            return c;
        }

    }


    public class Car
    {
        public string Make { get; set; }
        public string Model { get; set; }
    }
}
