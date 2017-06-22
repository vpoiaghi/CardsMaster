using System;
using System.Drawing;

namespace CardMasterImageClipping
{
    class SelectRectangle
    {
        private double sourceWidth = 0;
        private double sourceHeight = 0;

        private double targetWidth = 0;
        private double targetHeight = 0;
        private double targetRatioW = 0;
        private double targetRatioH = 0;

        private double anchorX = -1;
        private double anchorY = -1;

        private double oldLeftX = -1;
        private double oldTopY = -1;
        private double oldWidth = -1;
        private double oldHeight = -1;

        public SelectRectangle(double sourceWidth, double sourceHeight, double targetWidth, double targetHeight)
        {
            this.sourceWidth = sourceWidth;
            this.sourceHeight = sourceHeight;

            this.targetWidth = targetWidth;
            this.targetHeight = targetHeight;
            targetRatioW = targetWidth / targetHeight;
            targetRatioH = targetHeight / targetWidth;
        }

        public void StartSelection(double mouseX, double mouseY)
        {
            this.anchorX = mouseX;
            this.anchorY = mouseY;
            this.oldLeftX = mouseX;
            this.oldTopY = mouseY;
            this.oldWidth = 0;
            this.oldHeight = 0;
        }

        public Rectangle GetSelection(double mouseX, double mouseY)
        {
            double x = mouseX;
            double y = mouseY;
            double w = Math.Abs(x - anchorX);
            double h = Math.Abs(y - anchorY);

            if ((h * targetRatioW) <= w)
            {
                w = h * targetRatioW;
            }
            else
            {
                h = w * targetRatioH;
            }

            x = x < anchorX ? anchorX - w : anchorX + w;
            y = y < anchorY ? anchorY - h : anchorY + h;

            double leftX = Math.Min(x, anchorX);
            double topY = Math.Min(y, anchorY);

            if (leftX < 0 || topY < 0 || ((leftX + w) > sourceWidth) || ((topY + h) > sourceHeight))
            {
                leftX = oldLeftX;
                topY = oldTopY; ;
                w = oldWidth;
                h = oldHeight;
            }
            else
            {
                oldLeftX = leftX;
                oldTopY = topY;
                oldWidth = w;
                oldHeight = h;
            }

            return new Rectangle((int)leftX, (int)topY, (int)w, (int)h);
        }
    }
}
