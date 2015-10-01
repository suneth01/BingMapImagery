
namespace BingMapImagery.Bing
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using BingMapImagery;
    using BingMapImagery.Bing.Metadata;
    using BingMapImagery.Interfaces;

    internal class BingMapPushpinLayerDrawing : IMapLayerDrawing
    {
        private readonly IMapPushpinBuilder pushpinBuilder;

        private readonly Dictionary<string, Image> pushpinCache = new Dictionary<string, Image>();

        public BingMapPushpinLayerDrawing(IMapPushpinBuilder pushpinBuilder)
        {
            if (pushpinBuilder == null)
            {
                throw new ArgumentNullException("pushpinBuilder");
            }

            this.pushpinBuilder = pushpinBuilder;
        }

        public Image GetMapLayer(IMapMetadata analyticMapMetadata, MapRequest mapRequest)        
        {
            MapDescription mapDescription = mapRequest.MapDescription;
            IEnumerable<MapPushpin> pushpinDescriptions = mapRequest.Pushpins;

            if (pushpinDescriptions == null || !pushpinDescriptions.Any())
            {
                throw new ArgumentNullException("pushpinDescriptions");
            }

            if (analyticMapMetadata == null)
            {
                throw new ArgumentNullException("analyticMapMetadata");
            }

            if (mapDescription == null)
            {
                throw new ArgumentNullException("mapDescription");
            }

            var scaleFactor = mapDescription.MapImageScaleFactor;
            var width = (int)(analyticMapMetadata.MapWidth * scaleFactor);
            var height = (int)(analyticMapMetadata.MapHeight * scaleFactor);
            var image = new Bitmap(width, height);

            using (var g = Graphics.FromImage(image))
            {
                g.Clear(Color.Transparent);
                foreach (var pushPinDescription in pushpinDescriptions.OrderBy(pp => pp.Zindex))
                {
                    var pushPinMetadata = this.GetPushpinMetadata(pushPinDescription.Coordinate, analyticMapMetadata);

                    if (pushPinMetadata == null)
                    {
                        continue;
                    }

                    var pushPinImage = this.GetPushPinImage(
                        pushPinDescription.CustomPushpinIcon,
                        pushPinDescription.Label,
                        mapDescription);

                    if (pushPinImage == null)
                    {
                        continue;
                    }

                    var pushPinOffsetX = GetPushPinOffsetX(pushPinMetadata, scaleFactor, pushPinImage);
                    var pushPinOffsetY = GetPushPinOffsetY(
                        pushPinMetadata,
                        scaleFactor,
                        pushPinImage);

                    g.DrawImage(pushPinImage, pushPinOffsetX, pushPinOffsetY, pushPinImage.Width, pushPinImage.Height);
                }

                g.Flush();
            }
            
            return image;
        }

        private static float GetPushPinOffsetX(
            BingPushpinMetadata pushPinMetadata,
            double scaleFactor,
            Image pushPinImage)
        {
            var pushPinOffsetX = (float)((pushPinMetadata.Anchor.X * scaleFactor) - (pushPinImage.Width / 2));
            return pushPinOffsetX;
        }

        private static float GetPushPinOffsetY(
            BingPushpinMetadata pushPinMetadata,
            double scaleFactor,
            Image pushPinImage)
        {
            int pushPinImageOffsetY = pushPinImage.Height;            

            var pushPinOffsetY = (float)((pushPinMetadata.Anchor.Y * scaleFactor) - pushPinImageOffsetY);
            return pushPinOffsetY;
        }

        private Image GetPushPinImage(
            Image customPushpinImage,
            string pushPinLabelText,
            MapDescription mapDescription)
        {            
                var originalPushPinImage = customPushpinImage;
                var resizedPushPinImage = originalPushPinImage.ReSize(mapDescription.MapImageScaleFactor);
                Image pushpinImage = null;
                pushpinImage = string.IsNullOrWhiteSpace(pushPinLabelText)
                                   ? resizedPushPinImage
                                   : this.pushpinBuilder.GetLabeledPushpinImage(resizedPushPinImage, pushPinLabelText);


                return pushpinImage;
        }

        private BingPushpinMetadata GetPushpinMetadata(
            MapPoint coordinate,
            IMapMetadata analyticMapMetadata)
        {
            BingPushpinMetadata selectedPushpin = null;
            foreach (var pin in analyticMapMetadata.Pushpins)
            {
                // index latituide = 0, longitude = 1
                ////if(Math.Abs(pin.point.coordinates[0] - (double)coordinate.Latitude) < TOLERANCE)
                if (pin.BingPointMetadata.Coordinates[0] == (double)coordinate.Latitude
                    && pin.BingPointMetadata.Coordinates[1] == (double)coordinate.Longitude)
                {
                    selectedPushpin = pin;
                    break;
                }
            }

            return selectedPushpin;
        }        
    }
}