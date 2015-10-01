
namespace BingMapImagery.Interfaces
{
    using System.Collections.Generic;

    internal interface IMapCalculations
    {
        MapBoundingBox CalculateBoundingBox(List<MapPoint> pushpins);        
    }
}