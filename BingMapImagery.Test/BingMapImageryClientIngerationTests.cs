namespace BingMapImagery.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Drawing.Imaging;
    using System.Threading.Tasks;

    using BingMapImagery.Interfaces;

    using GeoJSON.Net.Geometry;

    using NUnit.Framework;    

    [TestFixture]
    public class BingMapImageryClientIngerationTests
    {
        private IBingMapImageryClient sut;


        [SetUp]
        public void Initialize()
        {
            this.sut = new BingMapImageryClient("YOUR_BING_API_KEY");
        }

        [Test]
        public async Task MapRequestWithPushpinList()
        {
            var actual = await this.sut.GenerateMap(new MapRequest()
                                             {
                                                 MapDescription = new MapDescription()
                                                                      {
                                                                          MapImageScaleFactor = 1,
                                                                          ImagerySet = ImagerySet.Road
                                                                      },
                                                 Pushpins = new List<MapPushpin>
                                                                {
                                                                    new MapPushpin()
                                                                        {
                                                                            Coordinate    = new MapPoint(32.8096983m, -117.0667287m),
                                                                            CustomPushpinIcon = Resources.map_pin
                                                                        },
                                                                        new MapPushpin()
                                                                        {
                                                                            Coordinate    = new MapPoint(32.8074630m, -117.0743790m),
                                                                            CustomPushpinIcon = Resources.map_pin
                                                                        },
                                                                }
                                             });

            Assert.IsNotNull(actual);
        }

        [Test]
        public async void MapRequestWithCenterPoint()
        {            
            var actual =
                await
                this.sut.GenerateMap(
                    new MapRequest()
                        {
                            MapDescription = new MapDescription() { MapImageScaleFactor = 1, ZoomLevel = 15, ImagerySet = ImagerySet.Aerial },
                            MapBoundingBoxType = MapBoundingBoxType.CenterPushPin,
                            CenterPushpin = new MapPushpin()
                                            {
                                                Coordinate = new MapPoint(32.8096983m, -117.0667287m),
                                                Label = "XX",
                                                HideLabel = true,
                                                PushpinIconStyle = 7
                                            }                                    
                        });

            Assert.IsNotNull(actual);
        }

        [Test]
        public async void MapRequestWithExplicitBoundingBox()
        {
            var actual = await this.sut.GenerateMap(new MapRequest()
            {
                MapDescription = new MapDescription()
                {
                    MapImageScaleFactor = 1                    
                },
                Pushpins = new List<MapPushpin>
                                                                {
                                                                    new MapPushpin()
                                                                        {
                                                                            Coordinate    = new MapPoint(32.8096983m, -117.0667287m),                                                                            
                                                                        }
                                                                },
                MapBoundingBoxType = MapBoundingBoxType.ExplicitBoundingBox,
                MapExplicitBoundingBox = new MapBoundingBox(new MapPoint(32.8096983m, -117.0667287m), new MapPoint(32.8074630m, -117.0743790m))
            });

            Assert.IsNotNull(actual);
        }

        [Test]
        public async Task MapRequestWithPushpinListAndPolygon()
        {
            var actual = await this.sut.GenerateMap(new MapRequest()
            {
                MapDescription = new MapDescription()
                {
                    MapImageScaleFactor = 1,
                    ImagerySet = ImagerySet.Road
                },
                Pushpins = new List<MapPushpin>
                                                                {
                                                                    new MapPushpin()
                                                                        {
                                                                            Coordinate    = new MapPoint(41.7969913m, -87.7044997m),
                                                                            CustomPushpinIcon = Resources.map_pin
                                                                        },
                                                                        new MapPushpin()
                                                                        {
                                                                            Coordinate    = new MapPoint(41.7828538m, -87.6986874m),
                                                                            CustomPushpinIcon = Resources.map_pin
                                                                        },
                                                                },
                Geometry = new Polygon(new List<LineString>
            {
                new LineString(new List<GeographicPosition>
                {
                    new GeographicPosition(41.781403745400382, -87.744959889282228),
                    new GeographicPosition(41.775003097688206, -87.692774830688478),
                    new GeographicPosition(41.7995781014328, -87.7047911270752),
                    new GeographicPosition(41.781403745400382, -87.744959889282228),
                })
            })
            });            

            Assert.IsNotNull(actual);
        }


        [Test]
        public async Task MapRequestWithExplicitBoundingBoxAndPolygon()
        {
            var actual = await this.sut.GenerateMap(new MapRequest()
            {
                MapDescription = new MapDescription()
                {
                    MapImageScaleFactor = 1,
                    ImagerySet = ImagerySet.Road                    
                },
                MapBoundingBoxType = MapBoundingBoxType.ExplicitBoundingBox,
                MapExplicitBoundingBox = new MapBoundingBox(new MapPoint(41.813093582091952m, -87.6618543251648m), new MapPoint(41.760807428034141m, -87.780386505950929m)),
                Geometry = new Polygon(new List<LineString>
                                {
                                    new LineString(new List<GeographicPosition>
                                    {
                                        new GeographicPosition(41.781403745400382, -87.744959889282228),
                                        new GeographicPosition(41.775003097688206, -87.692774830688478),
                                        new GeographicPosition(41.7995781014328, -87.7047911270752),
                                        new GeographicPosition(41.781403745400382, -87.744959889282228),
                                    })
                                })
            });

            Assert.IsNotNull(actual);
        }
    }
}
