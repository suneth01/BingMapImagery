
namespace BingMapImagery.Bing.Metadata
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The metadata response for Bing maps.
    /// </summary>
    [DataContract]
    public class BingMapMetadataResponse : IExtensibleDataObject
    {
        /// <summary>
        /// Gets or sets the resource sets.
        /// </summary>
        [DataMember(Name = "resourceSets")]
        public IEnumerable<BingMapMetadataResourceSet> ResourceSets { get; set; }

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        [DataMember(Name = "statusCode")]
        public string StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the trace ID.
        /// </summary>
        [DataMember(Name = "traceId")]
        public string TraceId { get; set; }

        /// <inheritdoc />
        public ExtensionDataObject ExtensionData { get; set; }
    }
}