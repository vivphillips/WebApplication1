using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

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


        [OperationContract]
        public string GetDocument(int pageCount)
        {
            try
            {
                LLL.TextHandling.PageBuilder pb = new LLL.TextHandling.PageBuilder();
                string s2 = HttpContext.Current.Server.MapPath("~/SVGS/test2.svg");
                string preparedSVGPage = pb.BuildPage(s2);
                for (int i = 0; i < pageCount; i++)
                {
                    //Convert the page to PDF and store
                }
                //Concat the pages
                string result = "Nothing to send";
                //Return final file as string.....
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }


    }


    public class Car
    {
        public string Make { get; set; }
        public string Model { get; set; }
    }
}
