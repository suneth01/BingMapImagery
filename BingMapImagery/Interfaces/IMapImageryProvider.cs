
namespace BingMapImagery.Interfaces
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Threading.Tasks;

    internal interface IMapImageryProvider
    {
        Task<Image> GetStaticMapImage(MapBoundingBox mapAreaBoundingBox, MapRequest mapRequest);

        Task<Image> GetStaticMapImageForCenterPushPin(MapRequest mapRequest);

        Task<IMapMetadata> GetStaticMapMetaData(
            MapBoundingBox mapAreaBoundingBox,            
            MapRequest mapRequest);
    }
}