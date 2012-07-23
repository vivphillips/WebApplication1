using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

                string tempPath = HttpContext.Current.Server.MapPath("~/PDF_Temp/");
                for (int i = 0; i < pageCount; i++)
                {
                    using (StreamWriter sw = File.CreateText(tempPath + "Page_" + i.ToString() + ".svg"))
                    {
                        sw.Write(preparedSVGPage);
                        sw.Close();
                    }

                    //Convert the page to PDF and store
                    Process inkscape = new Process();
                    inkscape.StartInfo.WorkingDirectory = tempPath;
                    inkscape.StartInfo.FileName = "C:\\Program Files (x86)\\Inkscape\\inkscape.exe";
                    inkscape.StartInfo.Arguments = "--file=Page_" + i + ".svg --export-dpi=90 --export-pdf=Page_" + i + ".pdf";
                    inkscape.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    inkscape.Start();
                    inkscape.WaitForExit();
                }

                //Concat the pages
                Process pdftk = new Process();
                pdftk.StartInfo.WorkingDirectory = tempPath;
                pdftk.StartInfo.FileName = HttpContext.Current.Server.MapPath("~/PDF_Merge/pdftk.exe");
                pdftk.StartInfo.Arguments = "*.pdf cat output FinalDoc.pdf";
                pdftk.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                pdftk.Start();
                pdftk.WaitForExit();

                string result = File.ReadAllText(tempPath + "FinalDoc.pdf"); // I don't think this will work, the file will need to be served by IIS, but maybe..
                
                // Clear up after myself
                DirectoryInfo dirInfo = new DirectoryInfo(tempPath);
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in dirInfo.GetDirectories())
                {
                    dir.Delete(true);
                }

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
