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
            string file = HttpContext.Current.Server.MapPath("~/SVGS/Test.svg");

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
                //TextLayoutInfo tlf = DoSizing(w, h, "Once upon a time in a land faraway there lived a pig.  The pig was not very bright as is often the way with such animals. A bit more text");
                //TextLayoutInfo tlf = DoSizing(w, h, "A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A A ");
                string theString = new StringBuilder().Insert(0, "A Ham and Cheese sandwich on brown bread ", 10).ToString();
                TextLayoutInfo tlf = DoSizing(w, h, theString);

                XElement elem = new XElement(svg + "text");
                XAttribute attrib = new XAttribute("font-size", tlf.FontSize.ToString() + "in");
                elem.Add(attrib);
                float linePos = 0;
                XAttribute step2 = new XAttribute("x", "0in");
                foreach (string s in tlf.Lines)
                {
                    XElement e2 = new XElement(svg + "tspan", s);
                    linePos += tlf.VerticalStep;
                    XAttribute step = new XAttribute("y", linePos.ToString() + "in");
                    e2.Add(step);
                    e2.Add(step2);

                    elem.Add(e2);
                }


                x.Add(elem);
            }

            return xdoc.ToString();

            //using (StreamReader sr = new StreamReader(file))
            //{
            //    return sr.ReadToEnd();
            //}
        }


        TextLayoutInfo DoSizing(float boxWidth, float boxHeight, string s)
        {
            float[,] l;
            float h;

            TextLayoutInfo tlf = new TextLayoutInfo();

            float initialBoxWidth, initialBoxHeight;

            float startFontSize = 1;
            using (Bitmap bm = new Bitmap(10, 10))
            {
                using (Graphics g = Graphics.FromImage(bm))
                {
                    g.PageUnit = GraphicsUnit.Inch;

                    initialBoxWidth = boxWidth;
                    initialBoxHeight = boxHeight;

                    Font f = new Font("Arial", startFontSize, GraphicsUnit.Inch);

                    string[] words = s.Split(' ');
                    l = new float[words.Length, 2];

                    for (int i = 0; i < words.GetLength(0); i++)
                    {
                        l[i, 0] = g.MeasureString(words[i], f).Width;
                        l[i, 1] = g.MeasureString(" " + words[i], f).Width;
                    }



                    ////Start with a box scaled to exact number of lines:
                    //boxHeight = h = g.MeasureString(s, f).Height;
                    //float factor = boxHeight / initialBoxHeight;
                    //boxWidth = boxWidth * factor;                    

                    h = g.MeasureString(s, f).Height;

                    while (!TestBox(l, h, boxWidth, boxHeight, words))
                    {
                        {
                            //For testing just make assumption that box is too small

                            //float increase = boxHeight + h;
                            //float factor = boxHeight / increase;
                            //boxWidth /= factor;
                            //boxHeight = increase;

                            boxHeight += h;
                            float factor = boxHeight / initialBoxHeight;
                            boxWidth = initialBoxWidth*factor;

                        }
                    }
                    tlf.FontSize = startFontSize / boxWidth * initialBoxWidth;

                    Font f2 = new Font("Arial", tlf.FontSize, GraphicsUnit.Inch);
                    tlf.VerticalStep = g.MeasureString("M", f2).Height;
                    float length = 0;
                    string line = String.Empty;

                    for (int i = 0; i < words.GetLength(0); i++)
                    {
                        if (length == 0)
                            length += g.MeasureString(words[i], f2).Width;
                        else
                            length += g.MeasureString(" " + words[i], f2).Width;
                        if (length < initialBoxWidth)
                            if (i == 0)
                                line += words[i];
                            else
                                line += " " + words[i];
                        else
                        {
                            length = 0;
                            i--;
                            tlf.Lines.Add(line);
                            line = string.Empty;
                        }
                    }
                    if (line != string.Empty)
                        tlf.Lines.Add(line);

                }
            }
            return tlf;
        }


        bool TestBox(float[,] list, float height, float boxwidth, float boxheight, string[] words)
        {
            System.Diagnostics.Debug.WriteLine("Trying: " + boxheight.ToString());

            int availableLines = (int)(boxheight / height)-1;


            System.Diagnostics.Debug.WriteLine("Available lines: " + availableLines.ToString());
            System.Diagnostics.Debug.WriteLine("-----------");

            int currentLine = 1;
            float currentLineLength = 0;
            // lineString just added for debugging purposes
            string lineString = string.Empty;
            for (int i = 0; i < list.GetLength(0); i++)
            {
                if (currentLineLength == 0)
                    currentLineLength += list[i,0];
                else
                    currentLineLength += list[i,1];
                if (currentLineLength > boxwidth)
                {
                    if (currentLine++ > availableLines)
                        return false;
                    currentLineLength = 0;
                    System.Diagnostics.Debug.WriteLine(lineString);
                    lineString = string.Empty;
                    i--;
                }
                else
                {
                    lineString += words[i] + " ";
                }
            }
            System.Diagnostics.Debug.WriteLine(lineString);
            return true;
        }

     }
}