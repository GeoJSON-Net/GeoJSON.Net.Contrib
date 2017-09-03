using GeoJSON.Net.Contrib.EntityFramework.WkbConversions;
using GeoJSON.Net.Geometry;

namespace GeoJSON.Net.Contrib.Wkb
{
    /// <summary>
    /// GeoJSON.Net / Wkb converter.
    /// All methods here are static and extensions to GeoJSON.Net types and Db geography and geometry types.
    /// </summary>
    public static partial class WkbConverter
    {
        public static IGeometryObject ToGeoJSONGeometry(this byte[] wkb)
        {
            return WkbDecode.Decode(wkb);
        }
    }
}
