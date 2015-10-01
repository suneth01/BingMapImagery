
namespace BingMapImagery.Metadata
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The metadata resource set.
    /// </summary>
    [DataContract]
    public class BingMapMetadataResourceSet : IExtensibleDataObject
    {
        /// <summary>
        /// Gets or sets the estimated total.
        /// </summary>
        [DataMember(Name = "estimatedTotal")]
        public string EstimatedTotal { get; set; }

        /// <summary>
        /// Gets or sets the metadata resources.
        /// </summary>
        [DataMember(Name = "resources")]
        public IEnumerable<BingMapMetadata> MetadataResources { get; set; }

        /// <inheritdoc />
        public ExtensionDataObject ExtensionData { get; set; }
    }
}