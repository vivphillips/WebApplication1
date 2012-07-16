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

    public class TextLayoutInfo
    {
        public TextLayoutInfo()
        {
            Lines = new List<string>();
        }

        public float FontSize;
        public float VerticalStep;
        public List<string> Lines;
    }


    public partial class WebForm1 : System.Web.UI.Page
    {

        public string LoadSVG()
        {
            string file = HttpContext.Current.Server.MapPath("~/SVGS/test2.svg");

            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            XNamespace svg = "http://www.w3.org/2000/svg";

            XDocument xdoc = XDocument.Load(file);
            var v = xdoc.Descendants(svg + "g").Where(x => x.Attributes("id") != null);

            var v2 = v.Where(x => (string)x.Attribute("id") == "InsertPoint");

            foreach (XElement x in v2)
            {

                float w = Single.Parse(x.Attribute("displaywidth").Value);
                float h = Single.Parse(x.Attribute("displayheight").Value);

                string units = string.Empty;
                if (x.Attribute("units") != null)
                    units = x.Attribute("units").Value;
                var fontAttrib = x.Attribute("font-family");

                string fontFamily = fontAttrib != null ? fontAttrib.Value : "Arial";

                //string theString = new StringBuilder().Insert(0, "A Ham and Cheese sandwich on brown bread with dill pickle. (Yum, Yum) ", 10).ToString();
                string theString = "Ham and Cheese Sandwich on white bread with margarine";
                TextLayoutInfo tlf = DoSizing2(w, h, theString, units, fontFamily);

                //Draw a rectangle
                XElement theBorder = new XElement(svg + "rect");

                theBorder.Add(new XAttribute("width", w.ToString() + units));
                theBorder.Add(new XAttribute("height", h.ToString() + units));
                theBorder.Add(new XAttribute("fill", "none"));
                theBorder.Add(new XAttribute("stroke", "black"));
                theBorder.Add(new XAttribute("stroke-width", "0.5"));
                x.Add(theBorder);

                XElement elem = new XElement(svg + "text");
                elem.Add(new XAttribute("text-anchor", "middle"));
                XAttribute attrib = new XAttribute("font-size", tlf.FontSize.ToString() + units);
                elem.Add(attrib);
                XAttribute a4 = new XAttribute("font-family", fontFamily);
                elem.Add(a4);
                elem.Add(new XAttribute("font-stretch", "normal"));
                float linePos = 0.02f;
                XAttribute step2 = new XAttribute("x", (w / 2).ToString() + units);
                foreach (string s in tlf.Lines)
                {
                    XElement e2 = new XElement(svg + "tspan", s);
                    linePos += tlf.VerticalStep;
                    XAttribute step = new XAttribute("y", linePos.ToString() + units);
                    e2.Add(step);
                    e2.Add(step2);

                    elem.Add(e2);
                }
                x.Add(elem);
            }
            return xdoc.ToString();
        }


        TextLayoutInfo DoSizing2(float boxWidth, float boxHeight, string s, string units, string fontFamily)
        {
            GraphicsUnit gUnits = GraphicsUnit.Pixel;
            if (units != string.Empty)
                gUnits = units == "mm" ? GraphicsUnit.Millimeter : GraphicsUnit.Inch;

            TextLayoutInfo tlf = new TextLayoutInfo();

            string[] words = s.Split(' ');


            float initialBoxWidth = boxWidth;
            float initialBoxHeight = boxHeight;
            using (Bitmap bm = new Bitmap(10, 10))
            {
                using (Graphics g = Graphics.FromImage(bm))
                {
                    g.PageUnit = gUnits;

                    float testFontSize = gUnits == GraphicsUnit.Inch ? 1 : 25;

                    Font f = new Font(fontFamily, testFontSize, gUnits);
                    float h = g.MeasureString("M", f).Height;
                    while (!TestSize(words, boxHeight, boxWidth, f, g, h, tlf))
                    {
                        boxHeight += h;
                        float factor = boxHeight / initialBoxHeight;
                        boxWidth = initialBoxWidth * factor;
                    }
                    tlf.FontSize = (testFontSize / boxWidth) * initialBoxWidth;

                    Font f2 = new Font(fontFamily, tlf.FontSize, gUnits);
                    tlf.VerticalStep = g.MeasureString("M", f2).Height;

                }
            }
            return tlf;
        }


        bool TestSize(string[] words, float boxHeight, float boxWidth, Font f, Graphics g, float h, TextLayoutInfo tlf)
        {
            int availableLines = ((int)(boxHeight / h)) -1;
            int currentLine = 0;
            tlf.Lines = new List<string>();
            string tryLine = string.Empty;
            for (int i = 0; i < words.Length; i++)
            {
                string nextWord = (tryLine != string.Empty ? " " : "") + words[i];
                float nextLength = g.MeasureString(tryLine + nextWord, f).Width;
                if (nextLength > boxWidth)
                {
                    i--;
                    tlf.Lines.Add(tryLine);
                    tryLine = string.Empty;
                    // move to next line
                    if (++currentLine > availableLines)
                        return false;
                }
                else
                    tryLine += nextWord;
            }
            if (tryLine != string.Empty)
                tlf.Lines.Add(tryLine);
            return true;
        }
    }
}