namespace BingMapImagery.Interfaces
{
    using System.Drawing;
    using System.Threading.Tasks;

    using global::GeoJSON.Net.Geometry;

    interface IMapShapeDrawing
    {
        Task<Image> DrawShapeLayerAsync(
            IGeometryObject geometry,
            MapBoundingBox mapAreaBoundingBox,
            MapDescription mapDescription);
    }
}
