using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;


namespace LLL.PDFHandler
{
    public class PdfStream
    {
        public PdfDictionary Dictionary = new PdfDictionary();
        public int Ref { get; set; }
        List<PdfStreamElement> Elements = new List<PdfStreamElement>();

        public void AddElement(PdfStreamElement element)
        {
            Elements.Add(element);
        }

        public override string ToString()
        {
            StringBuilder stream = new StringBuilder();
            foreach (PdfStreamElement element in Elements)
            {
                stream.Append(element.ToString());
            }
            Dictionary["Length"]=stream.Length.ToString();

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} 0 obj",Ref.ToString());
            sb.AppendLine();
            sb.Append(Dictionary);
            sb.Append("stream\r\n");
            sb.Append(stream);
            sb.Append("endstream\r\n");
            sb.Append("endobj\r\n");
            return sb.ToString();
        }
    }

    public class PdfXObjectForm : PdfStream
    {
        public PdfDictionary Resources = new PdfDictionary();
        private List<PdfFont> Fonts = new List<PdfFont>();
        public string XObjectId;
        public string BBox { get; set; }
   
        public void AddFont(PdfFont f)
        {
            Fonts.Add(f);
        }

        public override string ToString()
        {
            Resources["Font"] = Utilities.BuildFontDictionary(Fonts);
            Dictionary["BBox"]= BBox;
            Dictionary["Resources"] = Resources.ToString();
            return base.ToString();
        }
    }

    public class PdfStreamElement
    {
    }

    public class PdfLiteralStreamElement : PdfStreamElement
    {
        private string content;
        public PdfLiteralStreamElement(string s)
        {
            content = s;
        }

        public override string ToString()
        {
            return content +"\r\n";
        }
    }

    public class PdfTextElement :PdfStreamElement
    {
        public string Content { get; set; }
        public string FontId { get; set; }
        public double FontSize { get; set; }
        public PointF Position { get; set; }

        public override string ToString()
        {
            StringBuilder content = new StringBuilder();
            content.AppendFormat("BT {0} {1} Tf {2} {3} Td ({4}) Tj ET",
                FontId,
                FontSize,
                Position.X,
                Position.Y,
                Content);
            content.Append("\r\n");
            return content.ToString();
        }
    }
}
