
namespace BingMapImagery
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using BingMapImagery.Bing;
    using BingMapImagery.Bing.Metadata;
    using BingMapImagery.Interfaces;

    /// <inheritdoc />
    internal class BingMapImageryClient : IBingMapImageryClient
    {
        private readonly IMapImageryProvider imageryProvider;

        private readonly IMapCalculations mapCalculations;

        private readonly IMapLayerDrawing mapLayerDrawing;

        /// <inheritdoc />
        public BingMapImageryClient(
            string bingAPIKey
            //IAnalyticMapImageryProvider imageryProvider,
            //IAnalyticsMapCalculations mapCalculations
            //IAnalyticMapLayerDrawing mapLayerDrawing
            )
        {
            if (bingAPIKey == null)
            {
                throw new ArgumentNullException("bingAPIKey");
            }

            //if (imageryProvider == null)
            //{
            //    throw new ArgumentNullException("imageryProvider");
            //}

            //if (mapCalculations == null)
            //{
            //    throw new ArgumentNullException("mapCalculations");
            //}

            //if (mapLayerDrawing == null)
            //{
            //    throw new ArgumentNullException("mapLayerDrawing");
            //}

            this.imageryProvider = new BingMapsRESTImageryProvider(new MapRESTApiClient<BingMapMetadataResponse>(), bingAPIKey);

            this.mapCalculations = new MapCalculations();

            this.mapLayerDrawing = new BingMapPushpinLayerDrawing(new MapPushpinBuilder());
        }                

        /// <inheritdoc />
        public async Task<byte[]> GenerateMap(
            MapRequest mapDescriptionRequest)
        {            
            byte[] mapImageBytes;
            
            using (var mapImage = await this.GenerateMapImage(mapDescriptionRequest))
            {
                mapImageBytes = mapImage == null ? null : mapImage.ToByteArray(mapDescriptionRequest.MapDescription.ImageFormat);
            }

            return mapImageBytes;
        }

        public async Task<Stream> GenerateMapStream(MapRequest mapDescriptionRequest)
        {            
            var stream = new MemoryStream();

            using (var mapImage = await this.GenerateMapImage(mapDescriptionRequest))
            {
                mapImage.Save(stream, mapDescriptionRequest.MapDescription.ImageFormat);    
            }
            
            return stream;
        }

        /// <inheritdoc />
        public async Task<Image> GenerateMapImage(
            MapRequest mapDescriptionRequest)
        {
            if (mapDescriptionRequest == null)
            {
                throw new ArgumentNullException("mapDescriptionRequest");
            }

            if (mapDescriptionRequest.MapDescription == null)
            {
                throw new NoNullAllowedException("MapDescription");
            }

            List<MapPoint> pushpinCoordinates = null;
            MapBoundingBox mapAreaBoundingBox = null;
            Image mapImage = null;
            if (mapDescriptionRequest.MapBoundingBoxType == MapBoundingBoxType.BestFitPoints)
            {
                if (mapDescriptionRequest.Pushpins == null || !mapDescriptionRequest.Pushpins.Any())
                {
                    throw new ArgumentNullException("MapBoundingBoxType.BestFitPushPin doesnt allow mapDescriptionRequest.Pushpins to be null.");
                }

                if (mapDescriptionRequest.Pushpins.Count() == 1)
                {
                    throw new NotSupportedException("MapBoundingBoxType.BestFitPoints must have more than or equal to two points. Change to MapBoundingBoxType.CenterPushPin if you want center point and zoom level based besetfit view.");
                }

                var pushpinList = mapDescriptionRequest.Pushpins.ToList();
                pushpinCoordinates = pushpinList.Select(pp => pp.Coordinate).ToList();

                mapAreaBoundingBox = this.mapCalculations.CalculateBoundingBox(pushpinCoordinates);

                mapImage = await this.imageryProvider.GetStaticMapImage(
                mapAreaBoundingBox,
                mapDescriptionRequest);

                var customPushpins = mapDescriptionRequest.Pushpins.Where(pp => pp.CustomPushpinIcon != null);
                if (customPushpins.Any())
                {
                    var mapMetadata =
                        await this.imageryProvider.GetStaticMapMetaData(mapAreaBoundingBox, mapDescriptionRequest);

                    var pushpinMapLayer = this.mapLayerDrawing.GetMapLayer(mapMetadata, mapDescriptionRequest);

                    var mapLayers = new List<Image>()
                                        {                                
                                            pushpinMapLayer
                                        };

                    mapImage = this.MergeMapLayers(mapImage, mapLayers);
                }                
            }
            else if (mapDescriptionRequest.MapBoundingBoxType == MapBoundingBoxType.ExplicitBoundingBox)
            {
                if (mapDescriptionRequest.MapExplicitBoundingBox == null)
                {
                    throw new ArgumentNullException("MapExplicitBoundingBox cant be null when using MapBoundingBoxType.ExplicitBoundingBox mode.");                    
                }

                mapAreaBoundingBox = mapDescriptionRequest.MapExplicitBoundingBox;
                mapImage = await this.imageryProvider.GetStaticMapImage(
                mapAreaBoundingBox,
                mapDescriptionRequest);

            }
            else if (mapDescriptionRequest.MapBoundingBoxType == MapBoundingBoxType.CenterPushPin)
            {
                if (mapDescriptionRequest.CenterPushpin == null)
                {
                    throw new ArgumentNullException("Center Pushpin cant be null when using MapBoundingBoxType.CenterPushPin mode.");
                }

                mapImage = await this.imageryProvider.GetStaticMapImageForCenterPushPin(mapDescriptionRequest);
            }
            else
            {
                throw new NotSupportedException("Unsupported MapBoundingBoxType");
            }                        

            mapImage.Save(@"D:\Temp\MapImages\" + Guid.NewGuid() + "." + mapDescriptionRequest.MapDescription.ImageFormat, mapDescriptionRequest.MapDescription.ImageFormat);
            
            return mapImage;            
        }
        
        private Image MergeMapLayers(Image mapImage, IEnumerable<Image> mapLayers)
        {
            if (mapImage == null)
            {
                throw new ArgumentNullException("mapImage");
            }

            if (mapLayers == null || !mapLayers.Any())
            {
                return mapImage;
            }

            var mergePushPinLayers = new Bitmap(mapImage.Width, mapImage.Height);

            using (var g = Graphics.FromImage(mergePushPinLayers))
            {
                g.Clear(Color.Transparent);

                using (mapImage)
                {
                    g.DrawImage(mapImage, 0, 0);
                }

                foreach (var mapLayer in mapLayers)
                {
                    using (mapLayer)
                    {
                        g.DrawImage(mapLayer, 0, 0);
                    }
                }

                g.Flush();
            }

            return mergePushPinLayers;
        }
    }
}