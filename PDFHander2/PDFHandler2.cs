using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LLL.PDFHander2
{
    public class PdfDictionary : Dictionary<string, object>
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("<<");
            foreach (KeyValuePair<string, object> item in this)
            {
                if (item.Value !=null)
                sb.AppendFormat("/{0} {1} ", item.Key, item.Value.ToString());
            }
            sb.Append(">>");
            return sb.ToString();

        }
    }

    public class PdfArray : List<object>
    {
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("[");
            foreach (object o in this)
            {
                sb.AppendFormat(" {0}", o.ToString());
            }
            sb.Append("]");
            return sb.ToString();

        }
    }

    public class PdfString
    {
        private string content;
        public PdfString(string s)
        {
            content = s;
        }

        public override string ToString()
        {
            return "(" + content + ") ";
        }
    }

    public class PdfIndirectReference
    {
        private int id;
        public PdfIndirectReference(int i)
        {
            id = i;
        }

        public override string ToString()
        {
            return string.Format("{0} 0 R", id);
        }
    }

    public class PdfDictionaryObject : PdfDictionary
    {
        public PdfIndirectReference id;

        public PdfDictionaryObject(int i)
        {
            id = new PdfIndirectReference(i);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("obj {0}", id));
            sb.AppendLine(base.ToString());
            sb.AppendLine("endobj");
            return sb.ToString();
        }
    }

    public class PdfPage : PdfDictionaryObject
    {

        public PdfDictionary Resources = new PdfDictionary();
        public PdfRectangle MediaBox = new PdfRectangle(0, 0, 0, 0);
        public PdfIndirectReference Parent;

        public PdfPage(int id): base(id)
        {
            this.Add("Type", null);
            this.Add("MediaBox", null);
            this.Add("Resources",null);
            this.Add("Parent", null);
        }

        public override string ToString()
        {
            this["MediaBox"] = MediaBox;
            this["Resources"] = Resources;
            this["Parent"] = Parent;
            return base.ToString();
        }
    }

    public class PdfRectangle
    {
        private int x1, y1, w1, h1;

        public PdfRectangle(int x, int y, int w, int h)
        {
            x1 = x; y1 = y; w1 = w; h1 = h;
        }
        public override string ToString()
        {
            return string.Format("[{0} {1} {2} {3}]", x1, y1, w1, h1);
        }

    }

  
   

}
