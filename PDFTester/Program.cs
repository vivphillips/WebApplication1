using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LLL.PDFHandler;
using System.Drawing;
using System.IO;
using System.IO.Compression;


namespace PDFTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Test4();

        }



        private static void Test1()
        {
            PdfDocument doc = new PdfDocument();

            //Create the fonts
            PdfFont font = doc.GetFont();
            font.Add("BaseFont", "/Helvetica");
            font.Add("Subtype", "/Type1");

            PdfFont font2 = doc.GetFont();
            font2.Add("BaseFont", "/Times-Italic");
            font2.Add("Subtype", "/Type1");

            //Create a Stream which will be the page contents
            PdfStream stream = doc.GetPdfStream();

            //Create two text elements for the stream
            PdfTextElement element = doc.GetTextElement();
            element.Content = "This is a test";
            element.FontId = font2.FontId;
            element.FontSize = 36;
            element.Position = new PointF { X = 200, Y = 200 };
            stream.AddElement(element);

            PdfTextElement element2 = doc.GetTextElement();
            element2.Content = "Some more Text";
            element2.FontId = font.FontId;
            element2.FontSize = 25;
            element2.Position = new PointF { X = 200, Y = 100 };
            stream.AddElement(element2);

            //Create a page - set it's content to the above stream
            PdfPage page = doc.GetPage();
            page.PageContents = stream;
            page.AddFont(font);
            page.AddFont(font2);

            //Add the page to the Pages object of the document
            doc.PageList.Add(page);

            //Output the document
            string final = doc.Render();
            System.IO.File.WriteAllText(@"Junk.pdf", final);

        }


        //Using XObject Form
        private static void Test2()
        {
            PdfDocument doc = new PdfDocument();

            //Set up font
            PdfFont font = doc.GetFont();
            font.Add("BaseFont", "/Times-Italic");
            font.Add("Subtype", "/Type1");

            //Create two Text Elements
            PdfTextElement element = doc.GetTextElement();
            element.Content = "18 Point";
            element.FontId = font.FontId;
            element.FontSize = 18;
            element.Position = new PointF { X = 0, Y = 0 };

            PdfTextElement element2 = doc.GetTextElement();
            element2.Content = "25 Point";
            element2.FontId = font.FontId;
            element2.FontSize = 25;
            element2.Position = new PointF { X = 0, Y = 50 };


            //Create an XObject (Label)
            PdfXObjectForm form = doc.GetPdfXForm();
            form.AddElement(element);
            form.AddElement(element2);
            form.BBox = "[0 0 1000 1000]";
            form.AddFont(font);
            //Add to the XObject collection of the docuemnt
            doc.XForms.Add(form);

            //Create a page and reference the label
            PdfPage page = doc.GetPage();
            page.Add("UserUnit", "0.5");
            page.AddXObject(form);

            //Create the page contents (to show six instances of the XObject (label)
            PdfStream pageStream = doc.GetPdfStream();

            pageStream.AddElement(new PdfLiteralStreamElement(string.Format("1 0 0 1 50 200 cm {0} Do", form.XObjectId)));
            pageStream.AddElement(new PdfLiteralStreamElement(string.Format("1 0 0 1 0   200 cm {0} Do", form.XObjectId)));

            pageStream.AddElement(new PdfLiteralStreamElement(string.Format("1 0 0 1 0   200 cm {0} Do", form.XObjectId)));
            pageStream.AddElement(new PdfLiteralStreamElement(string.Format("1 0 0 1 200 -400 cm {0} Do", form.XObjectId)));
            pageStream.AddElement(new PdfLiteralStreamElement(string.Format("1 0 0 1 0 200 cm {0} Do", form.XObjectId)));
            pageStream.AddElement(new PdfLiteralStreamElement(string.Format("1 0 0 1 0 200 cm {0} Do", form.XObjectId)));
            page.PageContents = pageStream;

            //Add the page to the document
            doc.PageList.Add(page);

            //Note: Although this adds an additional reference to the same page in the Pages list
            //and the Document shows two pages the second page is blank.....  needs investigating....
            doc.PageList.Add(page);
            doc.PageList.Add(page);
            doc.PageList.Add(page);


            //Output the document
            string final = doc.Render();
            System.IO.File.WriteAllText(@"Junk.pdf", final);

        }

        private static void Test4()
        {
            PdfDocument doc = new PdfDocument();

            //Set up font
            PdfFont font = doc.GetFont();
            font.Add("BaseFont", "/Times-Italic");
            font.Add("Subtype", "/Type1");

            //Create two Text Elements
            PdfTextElement element = doc.GetTextElement();
            element.Content = "18 Point";
            element.FontId = font.FontId;
            element.FontSize = 18;
            element.Position = new PointF { X = 28.34F, Y = 85.02F };

            PdfTextElement element2 = doc.GetTextElement();
            element2.Content = "25 Point";
            element2.FontId = font.FontId;
            element2.FontSize = 25;
            element2.Position = new PointF { X = 28.35F, Y = 113.38F };


            //Create an XObject (Label)
            PdfXObjectForm form = doc.GetPdfXForm();
            form.AddElement(element);
            form.AddElement(element2);

            form.AddElement(new PdfLiteralStreamElement("28.74 28.74 m 56.69 28.74 l 56.69 56.69 l 28.74 56.69 l 28.74 28.74 l S"));
            form.BBox = "[0 0 1000 1000]";
            form.AddFont(font);
            //Add to the XObject collection of the docuemnt
            doc.XForms.Add(form);

            //Create a page and reference the label
            for (int i = 0; i < 5; i++)
            {
            PdfPage page = doc.GetPage();
            page.Add("UserUnit", "0.5");
            page.AddXObject(form);

            //Create the page contents (to show six instances of the XObject (label)
      
                PdfStream pageStream = doc.GetPdfStream();

                pageStream.AddElement(new PdfLiteralStreamElement(string.Format("1 0 0 1 0 0 cm {0} Do", form.XObjectId)));
                //pageStream.AddElement(new PdfLiteralStreamElement(string.Format("1 0 0 1 0   200 cm {0} Do", form.XObjectId)));

                //pageStream.AddElement(new PdfLiteralStreamElement(string.Format("1 0 0 1 0   200 cm {0} Do", form.XObjectId)));
                //pageStream.AddElement(new PdfLiteralStreamElement(string.Format("1 0 0 1 200 -400 cm {0} Do", form.XObjectId)));
                //pageStream.AddElement(new PdfLiteralStreamElement(string.Format("1 0 0 1 0 200 cm {0} Do", form.XObjectId)));
                //pageStream.AddElement(new PdfLiteralStreamElement(string.Format("1 0 0 1 0 200 cm {0} Do", form.XObjectId)));
                page.PageContents = pageStream;

                //Add the page to the document
                doc.PageList.Add(page);
            }

            //Note: Although this adds an additional reference to the same page in the Pages list
            //and the Document shows two pages the second page is blank.....  needs investigating....
           // doc.PageList.Add(page);
           // doc.PageList.Add(page);
           // doc.PageList.Add(page);


            //Output the document
            string final = doc.Render();
            System.IO.File.WriteAllText(@"Junk.pdf", final);

        }


        private static void Test3()
        {

            LLL.PDFHander2.PdfPage page = new LLL.PDFHander2.PdfPage(3);
            page.MediaBox = new LLL.PDFHander2.PdfRectangle(0, 0, 500, 500);
            page.Parent = new LLL.PDFHander2.PdfIndirectReference(27);

            LLL.PDFHander2.PdfArray refs = new LLL.PDFHander2.PdfArray();
            refs.Add(new LLL.PDFHander2.PdfIndirectReference(7));
            refs.Add(new LLL.PDFHander2.PdfIndirectReference(8));
            refs.Add(new LLL.PDFHander2.PdfIndirectReference(9));

            page.Resources.Add("Links", refs);

            string s = page.ToString();
  

        }


 
    }
}
