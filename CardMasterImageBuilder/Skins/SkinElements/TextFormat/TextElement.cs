using System.Drawing;
using System.IO;

namespace CardMasterImageBuilder.Elements.TextFormat
{
    class TextElement
    {
        private const string SYBOL_MARKER_1 = "<<";
        private const string SYBOL_MARKER_2 = ">>";

        private SizeF elementSize = SizeF.Empty;

        public string Text { get; set; } = "";
        public Image Image { get; set; } = null;
        public Rectangle Bounds { get; set; }
        public bool IsText { get { return (this.Image == null); } }
        public bool IsImage { get { return (this.Image != null); } }

        public TextElement(string text)
        {
            this.Text = text;
        }

        public void LoadSymbols(DirectoryInfo SymbolsDirectory)
        {
            string searchPattern = null;

            string txt = this.Text;

            if (txt.StartsWith(SYBOL_MARKER_1) && txt.EndsWith(SYBOL_MARKER_2))
            {
                txt = txt.Substring(2, txt.Length - 4);
                this.Text = txt;
                searchPattern = txt + ".*";

                FileInfo[] files = SymbolsDirectory.GetFiles(searchPattern, SearchOption.AllDirectories);

                if (files.Length > 0)
                {
                    this.Image = Bitmap.FromFile(files[0].FullName);
                }

                files = null;
            }
        }

        public SizeF GetSize(Graphics g, Font TextFont, StringFormat TextFormat, int MaxWidth)
        {
            if (this.elementSize == SizeF.Empty)
            {
                SizeF refSize = SizeF.Empty;
                SizeF initSize = SizeF.Empty;
                float ratio = 0;
                GraphicsUnit graphicsPageUnit = g.PageUnit;

                if (this.IsText)
                {
                    // Ajout d'un mot à une ligne

                    this.elementSize = g.MeasureString(this.Text, TextFont, MaxWidth, TextFormat);
                }
                else
                {
                    // Ajout d'un symbole à une ligne

                    if (refSize == SizeF.Empty)
                    {
                        refSize = g.MeasureString("O", TextFont, MaxWidth, TextFormat);
                    }

                    initSize = this.Image.GetBounds(ref graphicsPageUnit).Size;
                    ratio = initSize.Height / refSize.Height;
                    this.elementSize = new SizeF(initSize.Width / ratio, refSize.Height);
                }
            }

            return elementSize;
        }
    }
}
