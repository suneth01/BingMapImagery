
namespace BingMapImagery.Interfaces
{
    using System.Drawing;

    internal interface IMapPushpinBuilder
    {
        Image GetLabeledPushpinImage(Image originalPushpinImage, string pushpinLabelText);        
    }
}