using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;


namespace LLL.TextHandling
{
    public class PageBuilder
    {
        public string BuildPage(string file)
        {

            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            XNamespace svg = "http://www.w3.org/2000/svg";
            XNamespace pg = "http://www.planglow.com";

            XDocument xdoc = XDocument.Load(file);
            var v = xdoc.Descendants(svg + "g").Where(x => x.Attributes("id") != null);

            var v2 = v.Where(x => (string)x.Attribute("id") == "InsertPoint");



            foreach (XElement x in v2)
            {
                bool showBorder = false;


                //Check for metadata
                XElement meta = x.Descendants(svg + "metadata").FirstOrDefault();
                if (meta != null)
                {
                    //Check for the planglow display element
                    XElement display = meta.Descendants(pg + "display").FirstOrDefault();
                    if (display != null)
                    {
                        //Process the attributes
                        showBorder = display.Attribute("showborder") != null;
                        //etc


                        // Remove this from the output
                        display.Remove();
                    }
                    if (!meta.HasElements)
                        // Remove this as well if it's empty
                        meta.Remove();
                }



                float w = Single.Parse(x.Attribute("width").Value);
                float h = Single.Parse(x.Attribute("height").Value);

                string units = string.Empty;
                if (x.Attribute("units") != null)
                    units = x.Attribute("units").Value;
                var fontAttrib = x.Attribute("font-family");
                string fontFamily = fontAttrib != null ? fontAttrib.Value : "Arial";


                float aligner = .5f;
                if (x.Attribute("text-anchor") != null)
                {
                    string alignment = x.Attribute("text-anchor").Value;
                    if (alignment == "middle")
                        aligner = w / 2;
                    else if (alignment == "end")
                        aligner = w - .5f;
                }


                FontStyle FStyle = FontStyle.Regular;
                if (x.Attribute("font-style") != null)
                {
                    string fontStyle = x.Attribute("font-style").Value;
                    FStyle = fontStyle == "italic" ? FontStyle.Italic : FontStyle.Regular;
                }

                if (x.Attribute("font-weight") != null)
                {
                    string fontWeight = x.Attribute("font-weight").Value;
                    if (fontWeight == "bold")
                        FStyle = FStyle | FontStyle.Bold;
                }

                StringBuilder sb = new StringBuilder().Insert(0, "A Ham and Cheese sandwich on brown bread with dill pickle. (Yum, Yum) ", 10);
                string theString = sb.Append(" The end.").ToString();

                // string theString = "Ham and Cheese Sandwich on white bread with margarine and mustard";

                bool oldMethod = false;


                if (oldMethod)
                {

                    TextLayoutInfo tlf = DoSizing2(w, h, theString, units, fontFamily);
                    //Draw a rectangle
                    if (showBorder)
                    {
                        XElement theBorder = new XElement(svg + "rect");

                        theBorder.Add(new XAttribute("width", w.ToString() + units));
                        theBorder.Add(new XAttribute("height", h.ToString() + units));
                        theBorder.Add(new XAttribute("fill", "none"));
                        theBorder.Add(new XAttribute("stroke", "black"));
                        theBorder.Add(new XAttribute("stroke-width", "0.2"));
                        x.Add(theBorder);
                    }

                    XElement textElement = new XElement(svg + "text");
                    XAttribute attrib = new XAttribute("font-size", tlf.FontSize.ToString() + units);
                    textElement.Add(attrib);


                    float linePos = -.02f; ;
                    XAttribute xAttrib = new XAttribute("x", aligner.ToString() + units);
                    foreach (string s in tlf.Lines)
                    {
                        XElement tSpan = new XElement(svg + "tspan", s);
                        linePos += tlf.VerticalStep;
                        XAttribute yAttrib = new XAttribute("y", linePos.ToString() + units);
                        tSpan.Add(yAttrib);
                        tSpan.Add(xAttrib);
                        textElement.Add(tSpan);
                    }
                    x.Add(textElement);
                }

                else
                {

                    LLL.TextHandling.FontSizer fs = new LLL.TextHandling.FontSizer();
                    fs.FontFamilyName = fontFamily;
                    fs.Bounds = new LLL.TextHandling.Bounds { Width = w, Height = h };
                    fs.FormatMode = LLL.TextHandling.FormatMode.VaryFont;
                    fs.FontStyle = FStyle;
                    fs.Text = theString;
                    LLL.TextHandling.LayoutInfo layout = fs.Format();

                    if (showBorder)
                    {
                        //Draw a rectangle
                        XElement theBorder = new XElement(svg + "rect");
                        theBorder.Add(new XAttribute("width", w.ToString() + units));
                        theBorder.Add(new XAttribute("height", h.ToString() + units));
                        theBorder.Add(new XAttribute("fill", "none"));
                        theBorder.Add(new XAttribute("stroke", "black"));
                        theBorder.Add(new XAttribute("stroke-width", "0.5"));
                        x.Add(theBorder);
                    }

                    XElement textElement = new XElement(svg + "text");
                    //elem.Add(new XAttribute("text-anchor", "middle"));

                    textElement.Add(new XAttribute("font-size", layout.Font.Size.ToString() + units));
                    // textElement.Add(new XAttribute("font-family", layout.Font.FontFamily.ToString()));
                    // textElement.Add(new XAttribute("font-stretch", "normal"));
                    float linePos = 0 - layout.Headroom;
                    XAttribute step2 = new XAttribute("x", aligner.ToString() + units);
                    foreach (string s in layout.Lines)
                    {
                        XElement tSpan = new XElement(svg + "tspan", s);
                        linePos += layout.VerticalStep;
                        tSpan.Add(new XAttribute("y", linePos.ToString() + units));
                        tSpan.Add(step2);
                        textElement.Add(tSpan);
                    }
                    x.Add(textElement);
                }
            }

            string result = xdoc.ToString();

            return result;
        }

        bool TestSize(string[] words, float boxHeight, float boxWidth, Font f, Graphics g, float h, TextLayoutInfo tlf)
        {
            int availableLines = ((int)(boxHeight / h)) - 1;
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

    }

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





}
