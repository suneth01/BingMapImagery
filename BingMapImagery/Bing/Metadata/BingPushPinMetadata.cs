
namespace BingMapImagery.Bing.Metadata
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Pushpin metadata.
    /// </summary>
    [DataContract]
    public class BingPushpinMetadata : IExtensibleDataObject
    {
        /// <summary>
        /// Gets or sets the anchor.
        /// </summary>
        [DataMember(Name = "anchor")]
        public BingOffsetMetadata Anchor { get; set; }

        /// <summary>
        /// Gets or sets the point.
        /// </summary>
        [DataMember(Name = "point")]
        public BingPointMetadata BingPointMetadata { get; set; }

        /// <summary>
        /// Gets or sets the bottom right offset.
        /// </summary>
        [DataMember(Name = "bottomRightOffset")]
        public BingOffsetMetadata BottomRightBingOffsetMetadata { get; set; }

        /// <summary>
        /// Gets or sets the top left offset.
        /// </summary>
        [DataMember(Name = "topLeftOffset")]
        public BingOffsetMetadata TopLeftBingOffsetMetadata { get; set; }

        /// <inheritdoc />
        public ExtensionDataObject ExtensionData { get; set; }
    }
}