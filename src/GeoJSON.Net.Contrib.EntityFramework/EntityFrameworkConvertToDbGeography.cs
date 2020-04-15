using System;
using System.Data.Entity.Spatial;
using GeoJSON.Net.Geometry;
using GeoJSON.Net.Contrib.Wkb.Conversions;

namespace GeoJSON.Net.Contrib.EntityFramework
{
    public static partial class EntityFrameworkConvert
    {
        [Obsolete("This method will be removed in future releases, consider migrating now to the newest signature.", false)]
        public static DbGeography ToDbGeography(this IGeometryObject geometryObject)
        {
            return geometryObject.ToDbGeography(4326);
        }
        
        public static DbGeography ToDbGeography(this IGeometryObject geometryObject, int coordinateSystemId = 4326)
        {
            return DbGeography.FromBinary(WkbEncode.Encode(geometryObject), coordinateSystemId);
        }
    }
}
