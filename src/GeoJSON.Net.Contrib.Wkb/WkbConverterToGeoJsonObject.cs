namespace GeoJSON.Net.Contrib.Wkb
{
    /// <summary>
    /// GeoJSON.Net / Wkb converter.
    /// All methods here are static and extensions to GeoJSON.Net types and Db geography and geometry types.
    /// </summary>
    public static partial class WkbConverter
    {
        public static T ToGeoJSONObject<T>(this byte[] wkb) where T : GeoJSONObject
        {
            return wkb.ToGeoJSONGeometry() as T;
        }
    }
}
