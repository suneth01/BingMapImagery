namespace BingMapImagery
{    
    using System.Collections.Generic;

    using GeoJSON.Net.Geometry;

    /// <summary>
    /// Description for map request.
    /// </summary>    
    public class MapRequest
    {        
        /// <summary>
        /// Gets or sets the map description.
        /// </summary>
        public MapDescription MapDescription { get; set; }

        /// <summary>
        /// Gets or sets MapBoundingBoxType wihch defines which BING rest api to call.
        /// </summary>
        public MapBoundingBoxType MapBoundingBoxType { get; set; }

        /// <summary>
        /// Gets or sets the push pins list need more thatn two pushpings if youa re using pushpin bestfit type.
        /// </summary>
        public IEnumerable<MapPushpin> Pushpins { get; set; }

        /// <summary>
        /// Gets explicit bounding box to get the map area.
        /// </summary>
        public MapBoundingBox MapExplicitBoundingBox { get; set; }

        /// <summary>
        /// Gets map center point to use with zoom level center point boundig box type.
        /// </summary>
        public MapPushpin CenterPushpin { get; set; }

        /// <summary>
        /// Gets or sets the geometry.
        /// </summary>
        public IGeometryObject Geometry { get; set; }
    }
}
