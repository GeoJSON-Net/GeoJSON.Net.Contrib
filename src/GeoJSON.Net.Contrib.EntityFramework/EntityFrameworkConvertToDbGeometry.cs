using System;
using System.Data.Entity.Spatial;
using GeoJSON.Net.Geometry;
using GeoJSON.Net.Contrib.Wkb.Conversions;

namespace GeoJSON.Net.Contrib.EntityFramework
{
    public static partial class EntityFrameworkConvert
    {
        [Obsolete("This method will be removed in future releases, consider migrating now to the newest signature.", false)]
        public static DbGeometry ToDbGeometry(this IGeometryObject geometryObject)
        {
            return geometryObject.ToDbGeometry(4326);
        }

        public static DbGeometry ToDbGeometry(this IGeometryObject geometryObject, int coordinateSystemId = 4326)
        {
            return DbGeometry.FromBinary(WkbEncode.Encode(geometryObject), coordinateSystemId);
        }
    }
}
