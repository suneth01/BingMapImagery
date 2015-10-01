
namespace BingMapImagery
{
    using System.Drawing.Imaging;

    /// <summary>
    /// Desctription of the requesting map image.
    /// </summary>
    public class MapDescription
    {
        private const int DefaultRentComparableLocationsMapWidth = 840;
        private const int DefaultRentComparableLocationsMapHeight = 484;
        private const double DefaultMapImageScaleFactor = 1;
        private const int DefaultZoomLevel = 12;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapDescription" /> class.
        /// </summary>
        public MapDescription()
        {
            this.MapWidth = DefaultRentComparableLocationsMapWidth;
            this.MapHeight = DefaultRentComparableLocationsMapHeight;         
            this.MapImageScaleFactor = DefaultMapImageScaleFactor;
            this.ZoomLevel = DefaultZoomLevel;
            this.ImageFormat = ImageFormat.Png;
        }        

        /// <summary>
        /// Gets or sets the Width of the requesting map image.
        /// </summary>
        public int MapWidth { get; set; }

        /// <summary>
        /// Gets or sets the Height of the requesting map image.
        /// </summary>
        public int MapHeight { get; set; }        

        /// <summary>
        /// Gets or sets the zoom level when using center point based map bounding box.
        /// </summary>
        public int ZoomLevel { get; set; }

        /// <summary>
        /// Gets or sets the format of the returned image.
        /// </summary>
        public ImageFormat ImageFormat { get; set; }

        /// <summary>
        /// Gets or sets the format of the returned image.
        /// </summary>
        public ImagerySet ImagerySet { get; set; }

        /// <summary>
        /// Gets or sets the final image resolution factor if additional image enhancements are necessary.
        /// </summary>
        public double MapImageScaleFactor { get; set; }

        public bool DisplayPushpins { get; set; }

        public bool DisplayPushpinLabel { get; set; }

        public bool DeclutterPins { get; set; }
    }
}
