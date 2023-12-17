
using XeGo.Services.Location.API.Services.IServices;

namespace XeGo.Services.Location.Grpc.Services
{
    public class GeoHashService : IGeoHashService
    {
        public string Geohash(double latitude, double longitude, double geoSquareSideMeters)
        {
            var geohasher = new Geohash.Geohasher();
            string geohash = geohasher.Encode(latitude, longitude, 5);
            return geohash;
        }

        public List<string> GetNeighbors(double latitude, double longitude, double geoHashSquareSideInMeters, double radiusInMeters)
        {
            var geohasher = new Geohash.Geohasher();
            string geohash = geohasher.Encode(latitude, longitude, 5);

            Dictionary<Geohash.Direction, string> neighborsDict = geohasher.GetNeighbors(geohash);
            List<string> neighborsList = neighborsDict.Values.ToList();

            neighborsList.Add(geohash);

            return neighborsList;
        }
        //public string Geohash(double latitude, double longitude, double geoSquareSideMeters)
        //{
        //    double minLat = -90.0, maxLat = 90.0;
        //    double minLon = -180.0, maxLon = 180.0;
        //    double stepSize = geoSquareSideMeters / 111000.0;

        //    int geohash = 0;
        //    bool evenBit = true;
        //    int bit = 0;

        //    while (Math.Max(maxLat - minLat, maxLon - minLon) > stepSize)
        //    {
        //        if (evenBit)
        //        {
        //            // Bisect the longitude range
        //            double mid = (minLon + maxLon) / 2;
        //            if (longitude > mid)
        //            {
        //                geohash |= (1 << bit);
        //                minLon = mid;
        //            }
        //            else
        //            {
        //                maxLon = mid;
        //            }
        //        }
        //        else
        //        {
        //            // Bisect the latitude range
        //            double mid = (minLat + maxLat) / 2;
        //            if (latitude > mid)
        //            {
        //                geohash |= (1 << bit);
        //                minLat = mid;
        //            }
        //            else
        //            {
        //                maxLat = mid;
        //            }
        //        }

        //        evenBit = !evenBit;
        //        bit++;
        //    }

        //    // Reverse the bits
        //    int reversedGeohash = 0;
        //    for (int i = 0; i < bit; i++)
        //    {
        //        reversedGeohash <<= 1;
        //        reversedGeohash |= (geohash >> i) & 1;
        //    }

        //    // Convert to hexadecimal
        //    return reversedGeohash.ToString("X");
        //}

        public bool IsValidCoordinates(double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90)
            {
                return false;
            }
            if (longitude < -180 || longitude > 180)
            {
                return false;
            }
            return true;
        }

        //public List<string> GetNeighbors(double latitude, double longitude, double geoHashSquareSideInMeters, double radiusInMeters)
        //{
        //    double stepSize = geoHashSquareSideInMeters / 111000.0;
        //    int radiusInSquare = (int)Math.Ceiling(radiusInMeters / geoHashSquareSideInMeters);
        //    List<string> neighbors = new List<string>();

        //    for (int i = -radiusInSquare; i <= radiusInSquare; i++)
        //    {
        //        for (int j = -radiusInSquare; j <= radiusInSquare; j++)
        //        {
        //            // Skip the center point
        //            if (i == 0 && j == 0)
        //                continue;

        //            neighbors.Add(Geohash(latitude + i * stepSize, longitude + j * stepSize, geoHashSquareSideInMeters));
        //        }
        //    }

        //    return neighbors;
        //}


    }
}
