
namespace BingMapImagery
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using BingMapImagery.Interfaces;

    /// <inheritdoc />
    internal class MapCalculations : IMapCalculations
    {        
        /// <inheritdoc />
        public MapBoundingBox CalculateBoundingBox(List<MapPoint> pushpins)
        {
            if (pushpins == null || !pushpins.Any())
            {
                throw new ArgumentNullException("pushpins");
            }

            // find the bounding box.
            // South Latitude, lowerst latitude
            // West Longitude, lowerst longitude
            // North Latitude, largest latitide
            // East Longitude. largest longitude
            var firstPin = pushpins[0];
            var minLat = firstPin.Latitude;
            var minLon = firstPin.Longitude;
            var maxLat = firstPin.Latitude;
            var maxLon = firstPin.Longitude;

            foreach (var pushPin in pushpins)
            {
                minLat = minLat > pushPin.Latitude ? pushPin.Latitude : minLat;
                minLon = minLon > pushPin.Longitude ? pushPin.Longitude : minLon;
                maxLat = maxLat < pushPin.Latitude ? pushPin.Latitude : maxLat;
                maxLon = maxLon < pushPin.Longitude ? pushPin.Longitude : maxLon;
            }

            var southWest = new MapPoint(minLat, minLon);
            var northEast = new MapPoint(maxLat, maxLon);
                        
            var boundingBox = new MapBoundingBox(northEast, southWest);
            
            return boundingBox;
        }
    }
}