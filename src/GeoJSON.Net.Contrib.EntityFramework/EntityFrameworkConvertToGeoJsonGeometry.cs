using System.Data.Entity.Spatial;
using GeoJSON.Net.Geometry;
using GeoJSON.Net.Contrib.Wkb.Conversions;

namespace GeoJSON.Net.Contrib.EntityFramework
{
    public static partial class EntityFrameworkConvert
    {
        public static IGeometryObject ToGeoJSONGeometry(this DbGeography dbGeography)
        {
            return WkbDecode.Decode(dbGeography.AsBinary());
        }

        public static IGeometryObject ToGeoJSONGeometry(this DbGeometry dbGeometry)
        {
            return WkbDecode.Decode(dbGeometry.AsBinary());
        }
    }
}
