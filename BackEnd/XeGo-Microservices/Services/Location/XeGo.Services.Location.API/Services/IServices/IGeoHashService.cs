namespace XeGo.Services.Location.API.Services.IServices
{
    public interface IGeoHashService
    {
        string Geohash(double latitude, double longitude, double geoSquareSideMeters);
        bool IsValidCoordinates(double latitude, double longitude);
        List<string> GetNeighbors(double latitude, double longitude, double geoSquareSideMeters, double radiusInMeters);
    }
}
