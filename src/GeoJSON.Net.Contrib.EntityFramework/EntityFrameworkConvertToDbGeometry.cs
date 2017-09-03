using System.Data.Entity.Spatial;
using GeoJSON.Net.Contrib.EntityFramework.WkbConversions;
using GeoJSON.Net.Geometry;

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
