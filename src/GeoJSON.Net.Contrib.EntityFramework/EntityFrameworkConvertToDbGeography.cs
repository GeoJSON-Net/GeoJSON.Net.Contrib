using System.Data.Entity.Spatial;
using GeoJSON.Net.Geometry;
using GeoJSON.Net.Contrib.Wkb.Conversions;

namespace GeoJSON.Net.Contrib.EntityFramework
{
    public static partial class EntityFrameworkConvert
    {
        public static DbGeography ToDbGeography(this IGeometryObject geometryObject, int coordinateSystemId = 4326)
        {
            return DbGeography.FromBinary(WkbEncode.Encode(geometryObject), coordinateSystemId);
        }
    }
}
