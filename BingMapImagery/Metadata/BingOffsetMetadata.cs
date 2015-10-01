
namespace BingMapImagery.Metadata
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Offset metadata.
    /// </summary>
    [DataContract]
    public class BingOffsetMetadata : IExtensibleDataObject
    {
        /// <summary>
        /// Gets or sets the X component of the offset metadata.
        /// </summary>
        [DataMember(Name = "x")]
        public double X { get; set; }

        /// <summary>
        /// Gets or sets the Y component of the offset metadata.
        /// </summary>
        [DataMember(Name = "y")]
        public double Y { get; set; }

        /// <inheritdoc />
        public ExtensionDataObject ExtensionData { get; set; }
    }
}