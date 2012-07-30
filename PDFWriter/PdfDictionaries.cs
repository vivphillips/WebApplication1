using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LLL.PDFHandler
{


    public interface IPdfItem
    {
    }

    

    public class PdfDictionary : Dictionary<string, string>,IPdfItem
    {

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<<");
            foreach (KeyValuePair<string, string> entry in this)
            {
                sb.AppendFormat("/{0} {1} ", entry.Key, entry.Value);
            }
            sb.AppendLine(">>");
            return sb.ToString();
        }
    }

    public class IndirectPdfDictionary : PdfDictionary
    {

        public int Ref { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} 0 obj\r\n", Ref.ToString());
            sb.Append(base.ToString());
            sb.AppendLine("endobj");
            return sb.ToString();
        }
    }

    public class PdfPage : IndirectPdfDictionary
    {
        public PdfDictionary Resources { get; set; }

        private List<PdfFont> Fonts = new List<PdfFont>();
        private List<PdfXObjectForm> XObjects = new List<PdfXObjectForm>();

        public PdfStream PageContents
        {
            set
            {
                this["Contents"] = string.Format("{0} 0 R", value.Ref);
            }
        }

        public void AddFont(PdfFont f)
        {
            Fonts.Add(f);
        }

        public void AddXObject(PdfXObjectForm form)
        {
            XObjects.Add(form);
        }

        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();

            if (Fonts.Count > 0)
            {
                Resources.Add("Font", Utilities.BuildFontDictionary(Fonts));
            }
            if (XObjects.Count >0 )
            {
            Resources.Add("XObject", Utilities.BuildXObjectDictionary(XObjects));
            }

            //Expand the Resources dictionary (or get rid of it)
            this.Add("Resources", Resources.Count == 0 ? "<<>>" : Resources.ToString());

            return base.ToString();
        }
    }

    public class PdfPageTreeNode : IndirectPdfDictionary
    {

    }

    public class PdfFont : IndirectPdfDictionary
    {
        public string FontId { get; set; }
    }

    public class PdfCatalog : IndirectPdfDictionary
    {
        public void SetPagesRef(int i)
        {
            this["Pages"] = string.Format("{0} 0 R", i.ToString());
        }

    }
}
