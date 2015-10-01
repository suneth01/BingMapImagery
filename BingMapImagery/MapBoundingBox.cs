
namespace BingMapImagery
{
    using System;    

    /// <summary>
    /// A rectangle defined by two geographic coordinates.
    /// </summary>    
    [Serializable]
    public class MapBoundingBox
    {        
        /// <summary>
        /// Initializes a new instance of the <see cref="MapBoundingBox"/> class.
        /// </summary>
        /// <param name="northEast">The coordinate of the north east of the bounding box.</param>
        /// <param name="southWest">The coordinate of the south west of the bounding box.</param>
        /// <exception cref="ArgumentNullException">When either <paramref name="northEast"/>
        /// or <paramref name="southWest"/> are <b>null</b>.</exception>
        public MapBoundingBox(MapPoint northEast, MapPoint southWest)
        {
            if (northEast == null)
            {
                throw new ArgumentNullException("northEast");
            }

            if (southWest == null)
            {
                throw new ArgumentNullException("southWest");
            }
            
            this.NorthEast = northEast;
            this.SouthWest = southWest;
        }

        /// <summary>
        /// Gets or sets the coordinate of the upper left of the rectangle.
        /// </summary>
        public MapPoint NorthEast
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the coordinate of the lower right of the rectangle.
        /// </summary>
        public MapPoint SouthWest
        {
            get;
            set;
        }
    }
}