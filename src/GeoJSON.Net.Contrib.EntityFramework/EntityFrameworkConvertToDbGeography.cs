using System.Data.Entity.Spatial;
using GeoJSON.Net.Geometry;
using GeoJSON.Net.Contrib.Wkb.Conversions;

namespace GeoJSON.Net.Contrib.EntityFramework
{
    public static partial class EntityFrameworkConvert
    {
        public static DbGeography ToDbGeography(this IGeometryObject geometryObject)
        {
            DbGeography value = DbGeography.FromBinary(WkbEncode.Encode(geometryObject));
            return value;
        }
    }
}
