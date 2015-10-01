# BingMapImagery

Providing Map Static Image Layers via Bing REST Api. Create Map Layers with bing icons as well your custom pushpin icons.

Example Usage: Getting Map Image For Custom Pushpin Images.
```cs
var sut = new MapGenerator("BING_API_KEY");
var actual = await sut.GenerateMap(new MapRequest()
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
```
![alt tag](https://raw.githubusercontent.com/suneth01/BingMapImagery/master/SampleImages/Example_1.Png)


Example Usage: Map CenterPoint and Zoom level based best fit with and Centerpoint pushpin marked as hidden.
```cs
var actual =
    await
    sut.GenerateMap(
        new MapRequest()
            {
                MapDescription = new MapDescription() { MapImageScaleFactor = 1, ZoomLevel = 15, ImagerySet = ImagerySet.Aerial },
                MapBoundingBoxType = MapBoundingBoxType.CenterPushPin,
                CenterPushpin = new MapPushpin()
                                {
                                    Coordinate = new MapPoint(32.8096983m, -117.0667287m),
                                    HideLabel = true,
                                    PushpinIconStyle = 7
                                }                                    
            });
```
![alt tag](https://raw.githubusercontent.com/suneth01/BingMapImagery/master/SampleImages/Example_3.Png)
   
                        
Exaple Usage:  Excplicit Bounding box based best fit with bing's provided default pushpin icon.
```cs
var actual = await sut.GenerateMap(new MapRequest()
{
    MapDescription = new MapDescription()
    {
        MapImageScaleFactor = 1                    
    },
    MapBoundingBoxType = MapBoundingBoxType.ExplicitBoundingBox,
    MapExplicitBoundingBox = new MapBoundingBox(new MapPoint(32.8096983m, -117.0667287m), new MapPoint(32.8074630m, -117.0743790m))
});
```            
![alt tag](https://raw.githubusercontent.com/suneth01/BingMapImagery/master/SampleImages/Example_2.Png)
