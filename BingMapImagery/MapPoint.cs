namespace BingMapImagery
{
    using System;

    /// <summary>
    /// A measure of latitude and longitude.    
    /// </summary>
    public class MapPoint
    {
        public Decimal Latitude { get; set; }

        public Decimal Longitude { get; set; }

        public MapPoint(Decimal latitude, Decimal longitude)
        {
            if (latitude < new Decimal(-90) || latitude > new Decimal(90))
                throw new ArgumentOutOfRangeException("latitude");
            if (longitude < new Decimal(-180) || longitude > new Decimal(180))
                throw new ArgumentOutOfRangeException("longitude");
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
    }
}
