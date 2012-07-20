using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace WebApplication1
{

 


    public partial class WebForm1 : System.Web.UI.Page
    {

        public string LoadSVG()
        {
            string file = HttpContext.Current.Server.MapPath("~/SVGS/test2.svg");

            LLL.TextHandling.PageBuilder builder = new LLL.TextHandling.PageBuilder();

            string result = builder.BuildPage(file);

            return result;
        }


 
    }
}