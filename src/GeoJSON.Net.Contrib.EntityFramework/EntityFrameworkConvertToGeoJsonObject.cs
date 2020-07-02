using System.Data.Entity.Spatial;

namespace GeoJSON.Net.Contrib.EntityFramework
{
    public static partial class EntityFrameworkConvert
    {
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
