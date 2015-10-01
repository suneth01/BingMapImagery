
namespace BingMapImagery.Bing
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using BingMapImagery.Bing.Metadata;
    using BingMapImagery.Interfaces;

    internal class BingMapsRESTImageryProvider : IMapImageryProvider
    {
        private const int MaxPushPinCountForeSingleGETRequest = 50;
        
        private const string BingRestImageryRoadmapsEndpoint = "http://dev.virtualearth.net/REST/v1/Imagery/Map";

        public string BingAPIKey
        {
            get;
            set;
        }

        private readonly IMapRESTApiClient<BingMapMetadataResponse> restClient;

        public BingMapsRESTImageryProvider(IMapRESTApiClient<BingMapMetadataResponse> restClient, string bingAPIKey)
        {
            if (restClient == null)
            {
                throw new ArgumentNullException("restClient");
            }

            if (bingAPIKey == null)
            {
                throw new ArgumentNullException("bingAPIKey");
            }

            this.restClient = restClient;
            this.BingAPIKey = bingAPIKey;
            //this.restClient.Endpoint = this.GetBingRESTRoadMapsEndpoint();
        }               

        public async Task<Image> GetStaticMapImage(MapBoundingBox mapAreaBoundingBox, MapRequest mapRequest)
        {
            var mapDescription = mapRequest.MapDescription;

            var mapPathParams = new SortedList<int, string> { { 1, mapDescription.ImagerySet.ToString() } };

            var mapImageQueryArgs = new List<KeyValuePair<string, string>>();
            
            AddMapSizeQueryArguments(mapDescription, mapImageQueryArgs);
            
            AddMapAreaQueryArguments(mapAreaBoundingBox, mapImageQueryArgs);

            AddDisplayPushpins(mapRequest, mapImageQueryArgs);

            AddMapKeyQueryArguments(this.BingAPIKey, mapImageQueryArgs);

            var mapImage = await this.restClient.DownloadImage(BingRestImageryRoadmapsEndpoint, mapPathParams, mapImageQueryArgs);

            return mapImage.ReSize(mapDescription.MapImageScaleFactor);
        }

        private static void AddDisplayPushpins(MapRequest mapRequest, List<KeyValuePair<string, string>> mapImageQueryArgs)
        {
            //If pushping presented and want them to be printed. what do do for map area?
            IEnumerable<MapPushpin> displayPushpins = null;
            if (mapRequest.Pushpins != null)
            {
                displayPushpins = mapRequest.Pushpins.Where(pp => !pp.HidePushpin);                
            }

            if (displayPushpins != null || mapRequest.CenterPushpin != null)
            {
                AddPushPinQueryArgumentsWithLabels(displayPushpins, mapRequest.CenterPushpin, mapImageQueryArgs);
            }            
        }

        public async Task<Image> GetStaticMapImageForCenterPushPin(MapRequest mapRequest)
        {
            var mapCenterPushPin = mapRequest.CenterPushpin;
            var mapCenterPoint = mapCenterPushPin.Coordinate;
            var mapDescription = mapRequest.MapDescription;
            var centerPoint = string.Format("{0},{1}", mapCenterPoint.Latitude, mapCenterPoint.Longitude);
            var mapPathParams = new SortedList<int, string>
                                    {
                                        { 1, mapDescription.ImagerySet.ToString() },
                                        { 2, centerPoint },
                                        { 3, mapDescription.ZoomLevel.ToString() }
                                    };

            var mapImageQueryArgs = new List<KeyValuePair<string, string>>();

            AddDisplayPushpins(mapRequest, mapImageQueryArgs);

            AddMapSizeQueryArguments(mapDescription, mapImageQueryArgs);

            AddDeclutterPinsQueryArguments(mapDescription.DeclutterPins, mapImageQueryArgs);

            AddMapKeyQueryArguments(this.BingAPIKey, mapImageQueryArgs);

            var mapImage = await this.restClient.DownloadImage(BingRestImageryRoadmapsEndpoint, mapPathParams, mapImageQueryArgs);

            return mapImage.ReSize(mapDescription.MapImageScaleFactor);
        }

        public async Task<IMapMetadata> GetStaticMapMetaData(
            MapBoundingBox mapAreaBoundingBox,            
            MapRequest mapRequest)
        {
            var mapDescription = mapRequest.MapDescription;
            IEnumerable<MapPoint> mapPushPinCoordinates = mapRequest.Pushpins.Where(pp=>pp.CustomPushpinIcon != null).Select(p=>p.Coordinate);

            if (mapAreaBoundingBox == null)
            {
                throw new ArgumentNullException("mapAreaBoundingBox");
            }

            var pushPinCoordinates = mapPushPinCoordinates as IList<MapPoint> ?? mapPushPinCoordinates.ToList();
            if (mapPushPinCoordinates == null || !pushPinCoordinates.Any())
            {
                throw new ArgumentNullException("mapPushPinCoordinates");
            }

            var mapPathParams = new SortedList<int, string> { { 1, mapDescription.ImagerySet.ToString() } };

            var metaDataQueryArgs = new List<KeyValuePair<string, string>>
                                        {
                                            new KeyValuePair<string, string>(
                                                "o",
                                                "json"),
                                            new KeyValuePair<string, string>(
                                                "mapmetadata",
                                                "1"),
                                        };

            AddMapSizeQueryArguments(mapDescription, metaDataQueryArgs);

            AddMapAreaQueryArguments(mapAreaBoundingBox, metaDataQueryArgs);            

            AddMapKeyQueryArguments(this.BingAPIKey, metaDataQueryArgs);

            var bingMetaData = await this.GetBingMapMetadata(pushPinCoordinates, mapPathParams, metaDataQueryArgs);

            if (bingMetaData == null)
            {
                return null;
            }

            // NOTE: do this in try catch incase ResourceSets or MetadataResources returned from BING are null.
            try
            {
                return bingMetaData.ResourceSets.FirstOrDefault().MetadataResources.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new InvalidDataException("invalid MetaData from Bing REST api", ex);
            }
        }

        private static BingMapMetadataResponse MergerMetadataResponse(List<BingMapMetadataResponse> bingMetaData)
        {
            if (bingMetaData == null)
            {
                return new BingMapMetadataResponse();
            }

            if (bingMetaData.Any() == false)
            {
                return new BingMapMetadataResponse();
            }

            var bingMapMetadata = new BingMapMetadata
            {
                BoundingBox = bingMetaData.First().ResourceSets.First().MetadataResources.First().BoundingBox,
                MapCenter = bingMetaData.First().ResourceSets.First().MetadataResources.First().MapCenter,
                MapHeight = bingMetaData.First().ResourceSets.First().MetadataResources.First().MapHeight,
                MapWidth = bingMetaData.First().ResourceSets.First().MetadataResources.First().MapWidth,
                MapZoom = DetermineMapZoom(bingMetaData)
            };

            var metadataResourceSet = new BingMapMetadataResourceSet
            {
                EstimatedTotal = bingMetaData.First().ResourceSets.First().EstimatedTotal,
                MetadataResources = new[] { bingMapMetadata },
            };

            metadataResourceSet.MetadataResources.First().Pushpins = bingMetaData.SelectMany(metaData => metaData.ResourceSets.First()
                                                                        .MetadataResources.First()
                                                                        .Pushpins).ToList();

            var results = new BingMapMetadataResponse
            {
                StatusCode = bingMetaData.First().StatusCode,
                TraceId = bingMetaData.First().TraceId,
                ResourceSets = new[] { metadataResourceSet }
            };

            return results;
        }

        private static int DetermineMapZoom(IEnumerable<BingMapMetadataResponse> bingMetadata)
        {
            var mapZoom = 0;
            foreach (var metadataResource in 
                from bingMapMetadataResponse in bingMetadata 
                from bingMapMetadataResourceSet in bingMapMetadataResponse.ResourceSets 
                from metadataResource in bingMapMetadataResourceSet.MetadataResources 
                select metadataResource)
            {
                if (mapZoom == 0)
                {
                    mapZoom = metadataResource.MapZoom;
                    continue;
                }

                if (mapZoom > metadataResource.MapZoom)
                {
                    mapZoom = metadataResource.MapZoom;
                }
            }

            return mapZoom;
        }
        
        private static IEnumerable<MapPoint> GetBatch(IEnumerable<MapPoint> pushPinCoordinates, int pageNumber)
        {
            return pushPinCoordinates.Skip(pageNumber * MaxPushPinCountForeSingleGETRequest).Take(MaxPushPinCountForeSingleGETRequest);
        }

        private static void AddMapKeyQueryArguments(string key, List<KeyValuePair<string, string>> queryArguments)
        {
            // key=xxiiddiiddfdf
            queryArguments.Add(new KeyValuePair<string, string>("key", key));
        }
        

        private static void AddMapAreaQueryArguments(
            MapBoundingBox boundingBox,
            List<KeyValuePair<string, string>> queryArguments)
        {            
            // Bing needs: South Latitude, West Longitude, North Latitude, East Longitude
            // ?mapArea=37.317227,-122.318439,37.939081,-122.194565&
            var mapArea = string.Format(
                "{0},{1},{2},{3}",
                boundingBox.SouthWest.Latitude,
                boundingBox.SouthWest.Longitude,
                boundingBox.NorthEast.Latitude,
                boundingBox.NorthEast.Longitude);
            queryArguments.Add(new KeyValuePair<string, string>("ma", mapArea));
        }

        private static void AddMapSizeQueryArguments(
            MapDescription mapDescription,
            List<KeyValuePair<string, string>> queryArguments)
        {
            queryArguments.Add(
                new KeyValuePair<string, string>(
                    "ms",
                    string.Format("{0},{1}", mapDescription.MapWidth, mapDescription.MapHeight)));
        }

        private static void AddDeclutterPinsQueryArguments(bool dcl, List<KeyValuePair<string, string>> queryArguments)
        {
            if (dcl)
            {
                queryArguments.Add(new KeyValuePair<string, string>("dcl", "1"));
            }
        }

        private static void AddPushPinQueryArguments(
            IEnumerable<MapPoint> pushPinLocations,
            List<KeyValuePair<string, string>> queryArguments)
        {
            // add points query part
            // foreach point +="&pp=37.317227,-122.318439;IconType;Label"                        
            foreach (var pushPin in pushPinLocations)
            {
                var pushPinQuery = string.Format("{0},{1}", pushPin.Latitude, pushPin.Longitude);
                queryArguments.Add(new KeyValuePair<string, string>("pp", pushPinQuery));
            }
        }

        private static void AddPushPinQueryArgumentsWithLabels(
            IEnumerable<MapPushpin> pushPinLocations,
            MapPushpin centerPushpin,
            List<KeyValuePair<string, string>> queryArguments)
        {
            if (centerPushpin != null)
            {
                AddPushpin(queryArguments, centerPushpin);
            }

            // add points query part
            // foreach point +="&pp=37.317227,-122.318439;IconType;Label"                        
            if (pushPinLocations != null)
            {
                foreach (var pushPin in pushPinLocations)
                {
                    AddPushpin(queryArguments, pushPin);
                }
            }

        }

        private static void AddPushpin(List<KeyValuePair<string, string>> queryArguments, MapPushpin pushPin)
        {
            // Note: if custom pushpins are defined they shouldnt go in the image image request's pushpin list
            if (pushPin.CustomPushpinIcon != null)
            {
                return;
            }

            var point = pushPin.Coordinate;
            var label = pushPin.HideLabel ? string.Empty : pushPin.Label;
            var type = pushPin.PushpinIconStyle;

            // pushpin=latitude,longitude;iconStyle;label
            var pushPinQuery = string.Format("{0},{1};{2};{3}", point.Latitude, point.Longitude, type, label);
            queryArguments.Add(new KeyValuePair<string, string>("pp", pushPinQuery));
        }


        private async Task<BingMapMetadataResponse> GetBingMapMetadata(
            IEnumerable<MapPoint> mapPushPinCoordinates,
            SortedList<int, string> pathParams,
            List<KeyValuePair<string, string>> metaDataQueryArgs)
        {
            bool stop;
            var x = 0;
            var bingMetaData = new List<BingMapMetadataResponse>();

            if (mapPushPinCoordinates == null || metaDataQueryArgs == null)
            {
                return new BingMapMetadataResponse();
            }

            var pushPinCoordinates = mapPushPinCoordinates as IList<MapPoint> ??
                                     mapPushPinCoordinates.ToList();

            do
            {
                var result = GetBatch(pushPinCoordinates, x);
                x++;

                var pushPinLocations = result as MapPoint[] ?? result.ToArray();
                stop = pushPinLocations.Any() == false;

                if (stop)
                {
                    continue;
                }

                // TODO: refactor to do deep copy
                var copyMetaDataQueryArgs = new List<KeyValuePair<string, string>>
                {
                    metaDataQueryArgs[0],
                    metaDataQueryArgs[1],
                    metaDataQueryArgs[2],
                    metaDataQueryArgs[3],
                    metaDataQueryArgs[4]
                };                

                AddPushPinQueryArguments(pushPinLocations, copyMetaDataQueryArgs);
                var mapMetadataResponse = await this.restClient.GETResponse(BingRestImageryRoadmapsEndpoint, pathParams, copyMetaDataQueryArgs);
                if (mapMetadataResponse == null)
                {
                    return null;
                }

                bingMetaData.Add(mapMetadataResponse);
            } 
            while (stop == false);

            return bingMetaData.Count == 1 ? bingMetaData.First() : MergerMetadataResponse(bingMetaData);
        }
    }
}