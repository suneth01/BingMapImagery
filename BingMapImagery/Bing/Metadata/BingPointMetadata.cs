
namespace BingMapImagery.Bing.Metadata
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Bing point metadata.
    /// </summary>
    [DataContract]
    public class BingPointMetadata : IExtensibleDataObject
    {
        /// <summary>
        /// Gets or sets the coordinates.
        /// </summary>
        [DataMember(Name = "coordinates")]
        public double[] Coordinates { get; set; }

        /// <summary>
        /// Gets or sets the type of point metadata.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <inheritdoc />
        public ExtensionDataObject ExtensionData { get; set; }
    }
}