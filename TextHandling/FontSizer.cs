using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace LLL.TextHandling
{
    public delegate LayoutInfo FormatDelegate();

    public class FontSizer
    {
        public string FontFamilyName { get; set; }
        public FontStyle FontStyle { get; set; }
        public Bounds Bounds { get; set; }
        public string Text { get; set; }
        public FormatMode FormatMode { get; set; }

        private Graphics Graphics;
        private Font Font;

        public FontSizer()
        {
            using (Bitmap bm = new Bitmap(10, 10))
            {
                Graphics = Graphics.FromImage(bm);
            }
        }

        public LayoutInfo Format()
        {
            FormatDelegate fd;
            if (FormatMode == FormatMode.VaryFont)
                fd = VariableFontFormat;
            else
                fd = VariableBoxFormat;

            return fd();
        }

        public LayoutInfo Format(string ffn, Bounds b, string s, FormatMode fm)
        {
            FontFamilyName = ffn;
            Bounds = b;
            Text = s;
            FormatMode = fm;
            return Format();
        }

        public LayoutInfo VariableFontFormat()
        {
            LayoutInfo layout = new LayoutInfo();
            LayoutInfo best = new LayoutInfo();
            string[] words = Text.Split(' ');

            float topFontSize = 96, bottomFontSize = .1F, lastFit = 0;
            // float sizeToUse = 1; 
            int lineCount;
            while (topFontSize - bottomFontSize > .1)
            {
                lastFit = bottomFontSize + (topFontSize - bottomFontSize) / 2;
                Font = new Font(FontFamilyName, lastFit, FontStyle, GraphicsUnit.Pixel);

                lineCount = (int)(Bounds.Height / Graphics.MeasureString("M", Font).Height);
                if (lineCount == 0)
                {
                    topFontSize = lastFit;
                    continue;
                }

                if (TestSize(words, Bounds.Width, lineCount, ref layout))
                {
                    bottomFontSize = lastFit;
                    //sizeToUse = lastFit;
                    best = layout;
                }
                else
                    topFontSize = lastFit;

            }
            best.Font = new Font(FontFamilyName, best.FontSize, FontStyle, GraphicsUnit.Pixel);
            best.VerticalStep = Graphics.MeasureString("M", best.Font).Height;
            SetMetrics(ref best);
            return best;
        }

        public LayoutInfo VariableBoxFormat()
        {

            string[] words = Text.Split(' ');
            Font = new Font(FontFamilyName, 96, FontStyle, GraphicsUnit.Pixel);
            float h = Graphics.MeasureString("M", Font).Height;
            int lineCount = 1;
            float testWidth = (h / Bounds.Height) * Bounds.Width;
            LayoutInfo lo = new LayoutInfo();
            while (!TestSize(words, testWidth, lineCount, ref lo))
            {
                float testHeight = h * ++lineCount;
                testWidth = (testHeight / Bounds.Height) * Bounds.Width;
            }
            float scale = Bounds.Width / testWidth;

            float fontSize = 96 * scale;
            lo.Font = new Font(FontFamilyName, fontSize, FontStyle, GraphicsUnit.Pixel);
            lo.FontSize = fontSize;
            lo.VerticalStep = Graphics.MeasureString("M", lo.Font).Height;
            SetMetrics(ref lo);
            return lo;
        }


        void SetMetrics(ref LayoutInfo li)
        {
            var emHeight = li.Font.FontFamily.GetEmHeight(FontStyle);

            var ascent = li.Font.FontFamily.GetCellAscent(FontStyle);
            var lineSpacing = li.Font.FontFamily.GetLineSpacing(FontStyle);
            li.Headroom =  li.Font.Size * (lineSpacing-ascent) / emHeight;
        }



        private bool TestSize(string[] words, float testWidth, int lineCount, ref LayoutInfo lo)
        {
            lo.Lines = new List<String>();
            string tryLine = string.Empty;
            int currentLine = 1;
            for (int i = 0; i < words.Length; i++)
            {
                string nextWord = (tryLine != string.Empty ? " " : "") + words[i];
                float nextLength = Graphics.MeasureString(tryLine + nextWord, Font).Width;
                if (nextLength > testWidth)
                {
                    if (++currentLine > lineCount)
                        return false;
                    lo.Lines.Add(tryLine);
                    i--;
                    tryLine = string.Empty;
                }
                else
                    tryLine += nextWord;
            }
            if (tryLine != string.Empty)
                lo.Lines.Add(tryLine);

            lo.FontSize = Font.Size;
            return true;
        }
    }

    public struct Bounds
    {
        public float Width;
        public float Height;
    }

    public struct LayoutInfo
    {
        public List<String> Lines;
        public Font Font;
        public float FontSize;
        public float VerticalStep;
        public float Headroom;
    }

    public enum FormatMode
    {
        VaryBox,
        VaryFont
    }

}
