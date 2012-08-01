using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LLL.PDFHandler
{
    public class PdfDocument
    {
        private int nextRef = 0;

        public PdfCatalog Catalog { get; set; }
        public PdfPageTreeNode Pages { get; set; }

        public List<PdfPage> PageList { get; set; }
        public List<PdfStream> Streams { get; set; }
        public List<PdfFont> Fonts { get; set; }
        public List<PdfXObjectForm> XForms { get; set; }

        public int NextRef
        {
            get { return ++nextRef; }
        }

        public PdfDocument()
        {
            PageList = new List<PdfPage>();
            Streams = new List<PdfStream>();
            Fonts = new List<PdfFont>();
            XForms = new List<PdfXObjectForm>();
            Catalog = GetCatalog();
            Pages = GetPageTreeNode();
            Catalog.SetPagesRef(Pages.Ref);
        }

        private string GetPageArray()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            foreach (PdfPage p in PageList)
            {
                sb.AppendFormat("{0} 0 R ", p.Ref.ToString());
            }
            sb.Append("]");
            return sb.ToString();
        }

        public string Render()
        {
            SortedDictionary<int, int> XRefs = new SortedDictionary<int, int>();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("%PDF-1.6");
            foreach (PdfFont f in Fonts)
            {
                XRefs.Add(f.Ref, sb.Length);
                sb.Append(f);
            }

            foreach (PdfStream tstream in Streams)
            {
                XRefs.Add(tstream.Ref,sb.Length);
                sb.Append(tstream);
            }

            foreach (PdfXObjectForm xObject in XForms)
            {
                XRefs.Add(xObject.Ref, sb.Length);
                sb.Append(xObject);
            }



            foreach (PdfPage p in PageList)
            {
                if (! XRefs.ContainsKey(p.Ref))
                {
                XRefs.Add(p.Ref,sb.Length);
                sb.Append(p);
                }
            }
            // Add PageList to Pages
            Pages["Kids"] = GetPageArray();
            Pages["Count"] = PageList.Count.ToString();
            XRefs.Add(Pages.Ref,sb.Length);
            sb.Append(Pages);
            XRefs.Add(Catalog.Ref,sb.Length);
            sb.Append(Catalog);
            int XRefMarker = sb.Length;
            sb.Append(WriteXRefs(XRefs));
            sb.Append(WriteTrailer(Catalog.Ref,XRefs.Count,XRefMarker));
            sb.AppendLine("%%EOF");
            return sb.ToString();
        }


        private string WriteXRefs(SortedDictionary<int, int> entries)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("xref");
            sb.AppendFormat("0 {0}\r\n", entries.Count + 1);
            sb.Append("0000000000 65535 f\r\n");
            foreach (KeyValuePair<int, int> entry in entries)
            {
                sb.AppendFormat("{0} 00000 n\r\n", entry.Value.ToString().PadLeft(10, '0'));
            }
            return sb.ToString();
        }

        private string WriteTrailer(int rootRef, int xRefCount, int xRefMarker)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("trailer\r\n<<");
            sb.AppendFormat("/Root {0} 0 R\r\n", rootRef);
            sb.AppendFormat("/Size {0}\r\n", xRefCount + 1);
            sb.Append(">>\nstartxref\r\n");
            sb.AppendFormat("{0}\r\n", xRefMarker);

            return sb.ToString();
        }

        public PdfCatalog GetCatalog()
        {
            PdfCatalog c = new PdfCatalog();
            c.Ref = NextRef;
            c.Add("Type", "/Catalog");
            c.Add("Pages", "");
            return c;
        }

 

        public PdfPageTreeNode GetPageTreeNode()
        {
            PdfPageTreeNode node = new PdfPageTreeNode();
            node.Ref = NextRef;
            node.Add("Type", "/Pages");
            node.Add("Kids", "[ ]");
            node.Add("Count", "0");

            return node;
        }

        public PdfPage GetPage()
        {
            PdfPage page = new PdfPage();
            page.Resources = new PdfDictionary();
            page.Ref = NextRef;
            page.Add("Type", "/Page");
            page.Add("Parent", string.Format("{0} 0 R", Pages.Ref));
            page.Add("Contents", "");
            //TODO: Hard coded A4 - Do this properly
            //page.Add("MediaBox", "[0 0 595.27 841.88]");
            page.Add("MediaBox", "[0 0 133.22 189.92]");
            
            //PageList.Add(page);
            return page;
        }

        public PdfStream GetPdfStream()
        {
            PdfStream stream = new PdfStream();
            stream.Dictionary.Add("Length", "0");
            stream.Ref = NextRef;
            Streams.Add(stream);
            return stream;
        }

        public PdfXObjectForm GetPdfXForm()
        {
            PdfXObjectForm form = new PdfXObjectForm();
            form.Ref = NextRef;
            form.Dictionary.Add("Type", "/XObject");
            form.Dictionary.Add("Subtype", "/Form");
            form.Dictionary.Add("BBox", "");
            form.XObjectId = NextXObjectId;
            return form;
        }

        public PdfTextElement GetTextElement()
        {
            return new PdfTextElement();
        }

        public PdfFont GetFont()
        {
            PdfFont font = new PdfFont();
            font.Ref = NextRef;
            font.FontId = NextFontId;
            font.Add("Type", "/Font");
            Fonts.Add(font);
            return font;
        }

        private int nextFont = 0;
        public string NextFontId
        {
            get { return "/F" + (++nextFont).ToString(); }
        }

        private int nextXObject = 0;

        public string NextXObjectId
        {
            get { return "/X" + (++nextXObject).ToString(); }
        }
    }
}
