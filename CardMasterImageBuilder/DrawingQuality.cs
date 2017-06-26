using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace CardMasterImageBuilder
{
    public class DrawingQuality
    {
        public SmoothingMode SmoothingMode { get; set; } = SmoothingMode.AntiAlias;
        public TextRenderingHint TextRenderingHint { get; set; } = TextRenderingHint.AntiAlias;
        public CompositingQuality CompositingQuality { get; set; } = CompositingQuality.HighQuality;
        public CompositingMode CompositingMode { get; set; } = CompositingMode.SourceOver;
        public InterpolationMode InterpolationMode { get; set; } = InterpolationMode.HighQualityBicubic;
        public PixelOffsetMode PixelOffsetMode { get; set; } = PixelOffsetMode.HighQuality;
    }
}
