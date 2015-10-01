namespace BingMapImagery.Tests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using BingMapImagery.Interfaces;

    using NUnit.Framework;    

    [TestFixture]
    public class BingMapImageryClientIngerationTests
    {
        private IBingMapImageryClient sut;


        [SetUp]
        public void Initialize()
        {            
            this.sut = new BingMapImageryClient("BING_API_KEY");
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
    }
}
