using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class GetPdf : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Threading.Thread.Sleep(5000);
            string s2 = HttpContext.Current.Server.MapPath("~/PDFs/"+Request.QueryString);
            byte[] b = System.IO.File.ReadAllBytes(s2);
            Response.Clear();
            Response.ContentType = "application/pdf";

            var output = Response.OutputStream ;
            output.Write(b, 0, b.Length);
            output.Close();
            Response.Flush();
        }
    }
}