namespace GeoJSON.Net.Contrib.Wkb.Conversions
{
    public enum WkbGeometryType : uint
    {
        Point = 1,
        LineString = 2,
        Polygon = 3,
        MultiPoint = 4,
        MultiLineString = 5,
        MultiPolygon = 6,
        GeometryCollection = 7
    }
}
