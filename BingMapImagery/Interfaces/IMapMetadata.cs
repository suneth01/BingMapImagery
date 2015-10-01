
namespace BingMapImagery.Interfaces
{
    using System.Collections.Generic;

    using BingMapImagery.Metadata;

    internal interface IMapMetadata
    {
        IEnumerable<double> BoundingBox { get; set; }

        BingPointMetadata MapCenter { get; set; }

        int MapHeight { get; set; }

        int MapWidth { get; set; }

        int MapZoom { get; set; }

        IEnumerable<BingPushpinMetadata> Pushpins { get; set; }
    }
}