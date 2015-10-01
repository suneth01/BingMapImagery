
namespace BingMapImagery
{
    using System.Drawing;

    /// <summary>
    /// Analytic Pushpin Description.
    /// </summary>    
    public class MapPushpin
    {
        public MapPushpin()
        {
            this.PushpinIconStyle = 1;
        }

        /// <summary>
        /// Gets or sets the coordinate.
        /// </summary>        
        public MapPoint Coordinate { get; set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>        
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the pushpin type.
        /// </summary>        
        //public AnalyticMapPushpinType PushpinType { get; set; }

        /// <summary>
        /// Gets or sets the zindex.
        /// </summary>
        public int Zindex { get; set; }

        public bool HidePushpin { get; set; }

        public bool HideLabel { get; set; }

        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/ff701719.aspx?f=255&MSPPError=-2147217396
        /// </summary>
        public int PushpinIconStyle { get; set; }

        /// <summary>
        /// Custom pushpin image if you dnt want to use Bing's pushpin icons. 
        /// </summary>
        public Image CustomPushpinIcon { get; set; }
    }
}