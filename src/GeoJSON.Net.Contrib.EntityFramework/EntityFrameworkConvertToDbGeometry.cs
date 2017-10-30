using System.Data.Entity.Spatial;
using GeoJSON.Net.Geometry;
using GeoJSON.Net.Contrib.Wkb.Conversions;

namespace GeoJSON.Net.Contrib.EntityFramework
{
    public static partial class EntityFrameworkConvert
    {
        public static DbGeometry ToDbGeometry(this IGeometryObject geometryObject)
        {
            return DbGeometry.FromBinary(WkbEncode.Encode(geometryObject));
        }
    }
}
