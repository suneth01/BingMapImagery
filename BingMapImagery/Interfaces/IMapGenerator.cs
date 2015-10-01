
namespace BingMapImagery.Interfaces
{
    using System.Drawing;
    using System.IO;
    using System.Threading.Tasks;

    internal interface IMapGenerator
    {
        Task<byte[]> GenerateMap(MapRequest mapDescriptionRequest);

        Task<Stream> GenerateMapStream(MapRequest mapDescriptionRequest);

        Task<Image> GenerateMapImage(MapRequest mapDescriptionRequest);
    }
}