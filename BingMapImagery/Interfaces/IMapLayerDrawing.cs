
namespace BingMapImagery.Interfaces
{
    using System.Drawing;

    using BingMapImagery;

    internal interface IMapLayerDrawing
    {
        Image GetMapLayer(IMapMetadata analyticMapMetadata, MapRequest mapRequest);
    }
}