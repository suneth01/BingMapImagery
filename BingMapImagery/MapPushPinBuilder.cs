
namespace BingMapImagery
{
    using System;
    using System.Drawing;

    using BingMapImagery.Interfaces;

    /// <inheritdoc />
    internal class MapPushpinBuilder : IMapPushpinBuilder
    {
        private const string PushPinLabelDynamicGraphicText = "9999";

        private const string PushPinLabelFontFamilyName = "Arial";        

        /// <inheritdoc />
        public Image GetLabeledPushpinImage(
            Image originalPushPinImage,
            string pushPinLabelText)
        {
            Image labeledPushPin = new Bitmap(originalPushPinImage);
            using (var g = Graphics.FromImage(labeledPushPin))
            {
                var originalFont = new Font(PushPinLabelFontFamilyName, 7, FontStyle.Bold);
                var labelFont = this.GetAdjustedFont(g, 2, originalFont, labeledPushPin.Width, 20, 5, true);
                var labelAnchorX = 0;
                var labelAnchorY = 0;
                this.GetLabelOffsets(labeledPushPin, pushPinLabelText, labelFont, ref labelAnchorX, ref labelAnchorY);
                g.DrawString(pushPinLabelText, labelFont, Brushes.White, labelAnchorX, labelAnchorY);
                g.Flush();
            }

            ////labeledPushPin.Save(@"D:\Temp\MapImages\" + Guid.NewGuid() + ".png", System.Drawing.Imaging.ImageFormat.Png);

            return labeledPushPin;
        }

        private Font GetAdjustedFont(
            Graphics graphic,
            int graphicCharCount,
            Font originalFont,
            int containerWidth,
            int maxFontSize,
            int minFontSize,
            bool smallestOnFail)
        {
            return this.GetAdjustedFont(
                graphic,
                PushPinLabelDynamicGraphicText.Substring(0, graphicCharCount),
                originalFont,
                containerWidth,
                maxFontSize,
                minFontSize,
                smallestOnFail);
        }

        private Font GetAdjustedFont(
            Graphics graphic,
            string graphicText,
            Font originalFont,
            int containerWidth,
            int maxFontSize,
            int minFontSize,
            bool smallestOnFail)
        {
            // We utilize MeasureString which we get via a control instance           
            for (int adjustedSize = maxFontSize; adjustedSize >= minFontSize; adjustedSize--)
            {
                var testFont = new Font(originalFont.Name, adjustedSize, originalFont.Style);

                // Test the string with the new size
                var adjustedSizeNew = graphic.MeasureString(graphicText, testFont);

                if (containerWidth > Convert.ToInt32(adjustedSizeNew.Width))
                {
                    // Good font, return it
                    return testFont;
                }
            }

            // If you get here there was no fontsize that worked
            // return MinimumSize or Original?
            if (smallestOnFail)
            {
                return new Font(originalFont.Name, minFontSize, originalFont.Style);
            }

            return originalFont;
        }

        private void GetLabelOffsets(Image pushPin, string pushPinLabelText, Font labelFont, ref int x, ref int y)
        {
            // TODO: calculate offset based on icon image size to support any image.
            // This is important since scale factor changes the size of the pushpin image.
            switch (pushPinLabelText.Length)
            {
                case 1:
                    x = 6;
                    break;
                default:
                    x = 0;
                    break;
            }

            switch ((int)labelFont.Size)
            {
                case 10:
                case 9:
                case 8:
                    y = 8;
                    break;
                case 7:
                case 6:
                case 5:
                    y = 10;
                    break;
                default:
                    y = 6;
                    break;
            }
        }        
    }
}