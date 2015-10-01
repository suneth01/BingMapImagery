
namespace BingMapImagery.Bing.Metadata
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using BingMapImagery.Interfaces;

    /// <summary>
    /// Bing map metadata.
    /// </summary>
    [DataContract]
    public class BingMapMetadata : IMapMetadata, IExtensibleDataObject
    {
        /// <inheritdoc />
        [DataMember(Name = "bbox")]
        public IEnumerable<double> BoundingBox { get; set; }

        /// <inheritdoc />
        [DataMember(Name = "mapCenter")]
        public BingPointMetadata MapCenter { get; set; }

        /// <inheritdoc />
        [DataMember(Name = "imageHeight")]
        public int MapHeight { get; set; }

        /// <inheritdoc />
        [DataMember(Name = "imageWidth")]
        public int MapWidth { get; set; }

        /// <inheritdoc />
        [DataMember(Name = "zoom")]
        public int MapZoom { get; set; }

        /// <inheritdoc />
        [DataMember(Name = "pushpins")]
        public IEnumerable<BingPushpinMetadata> Pushpins { get; set; }

        /// <inheritdoc />
        public ExtensionDataObject ExtensionData { get; set; }
    }
}