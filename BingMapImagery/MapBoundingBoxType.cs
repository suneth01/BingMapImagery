namespace BingMapImagery
{
    public enum MapBoundingBoxType
    {
        BestFitPoints = 0, // Need to provide two or more MapPoints
        ExplicitBoundingBox = 1, // Need to provide boundingbox
        CenterPushPin = 2 // Need to provide zoom level and center point
    }
}
