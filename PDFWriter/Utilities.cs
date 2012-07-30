using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace LLL.PDFHandler
{
    public static class Utilities
    {
        public static string BuildFontDictionary(List<PdfFont> list)
        {
            StringBuilder sb = new StringBuilder("<<");
            foreach (PdfFont f in list)
            {
                sb.AppendFormat("{0} {1} 0 R", f.FontId, f.Ref);
            }
            sb.Append(">>");
            return sb.ToString();
        }

        public static string BuildXObjectDictionary(List<PdfXObjectForm> list)
        {
            StringBuilder sb = new StringBuilder("<<");
            foreach (PdfXObjectForm x in list)
            {
                sb.AppendFormat("{0} {1} 0 R", x.XObjectId, x.Ref);
            }
            sb.Append(">>");
            return sb.ToString();
        }


 

    }
}
