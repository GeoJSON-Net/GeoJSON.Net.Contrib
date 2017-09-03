using GeoJSON.Net.Contrib.EntityFramework.WkbConversions;
using GeoJSON.Net.Geometry;
using System;
using System.Data.Entity.Spatial;

namespace GeoJSON.Net.Contrib.EntityFramework
{
    /// <summary>
    /// GeoJSON.Net / EntityFramework types converter.
    /// All methods here are static and extensions to GeoJSON.Net types and Db geography and geometry types.
    /// </summary>
    public static class EntityFrameworkConvert
    {
        public static DbGeography ToDbGeography(this IGeometryObject geometryObject)
        {
            return DbGeography.FromBinary(WkbEncode.Encode(geometryObject));
        }

        public static DbGeometry ToDbGeometry(this IGeometryObject geometryObject)
        {
            return DbGeometry.FromBinary(WkbEncode.Encode(geometryObject));
        }

        public static IGeometryObject ToGeoJSONGeometry(this DbGeography dbGeography)
        {
            return WkbDecode.Decode(dbGeography.AsBinary());
        }

        public static IGeometryObject ToGeoJSONGeometry(this DbGeometry dbGeometry)
        {
            return WkbDecode.Decode(dbGeometry.AsBinary());
        }

        public static T ToGeoJSONObject<T>(this DbGeography dbGeography) where T : GeoJSONObject
        {
            return dbGeography.ToGeoJSONGeometry() as T;
        }

        public static T ToGeoJSONObject<T>(this DbGeometry dbGeometry) where T : GeoJSONObject
        {
            return dbGeometry.ToGeoJSONGeometry() as T;
        }
    }
}
