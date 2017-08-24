using GeoJSON.Net.Geometry;
using System.Data.Entity.Spatial;
using Xunit;

namespace GeoJSON.Net.Contrib.EntityFramework.Test
{
    public class EntityFrameworkConvertTests
    {
        [Theory]
        [InlineData(4326, "POINT(30 10)")]
        [InlineData(4326, "LINESTRING (30 10, 10 30, 40 40)")]
        [InlineData(4326, "POLYGON ((30 10, 40 40, 20 40, 10 20, 30 10))")]
        [InlineData(4326, "POLYGON ((35 10, 45 45, 15 40, 10 20, 35 10),(20 30, 35 35, 30 20, 20 30))")]
        [InlineData(4326, "MULTIPOINT ((10 40), (40 30), (20 20), (30 10))")]
        [InlineData(4326, "MULTIPOINT (10 40, 40 30, 20 20, 30 10)")]
        [InlineData(4326, "MULTILINESTRING ((10 10, 20 20, 10 40),(40 40, 30 30, 40 20, 30 10))")]
        [InlineData(4326, "MULTIPOLYGON (((30 20, 45 40, 10 40, 30 20)),((15 5, 40 10, 10 20, 5 10, 15 5)))")]
        [InlineData(4326, "MULTIPOLYGON (((40 40, 20 45, 45 30, 40 40)),((20 35, 10 30, 10 10, 30 5, 45 20, 20 35),(30 20, 20 15, 20 25, 30 20)))")]
        [InlineData(4326, "GEOMETRYCOLLECTION(POINT(4 6),LINESTRING(4 6,7 10))")]
        public void FromDbGeography_ToGeoJSONs(int srid, string wkb)
        {
            //Arrange
            var dbGeography = DbGeography.FromText(wkb, srid);

            // Act & Assert
            var geometryObject = dbGeography.ToGeoJSONGeometry();

            switch (geometryObject.Type)
            {
                case GeoJSONObjectType.Point:
                    var point = dbGeography.ToGeoJSONObject<Point>();
                    Assert.NotNull(point);
                    break;
                case GeoJSONObjectType.MultiPoint:
                    var multiPoint = dbGeography.ToGeoJSONObject<MultiPoint>();
                    Assert.NotNull(multiPoint);
                    break;
                case GeoJSONObjectType.LineString:
                    var lineString = dbGeography.ToGeoJSONObject<LineString>();
                    Assert.NotNull(lineString);
                    break;
                case GeoJSONObjectType.MultiLineString:
                    var multiLineString = dbGeography.ToGeoJSONObject<MultiLineString>();
                    Assert.NotNull(multiLineString);
                    break;
                case GeoJSONObjectType.Polygon:
                    var polygon = dbGeography.ToGeoJSONObject<Polygon>();
                    Assert.NotNull(polygon);
                    break;
                case GeoJSONObjectType.MultiPolygon:
                    var multiPolygon = dbGeography.ToGeoJSONObject<MultiPolygon>();
                    Assert.NotNull(multiPolygon);
                    break;
                case GeoJSONObjectType.GeometryCollection:
                    var geometryCollection = dbGeography.ToGeoJSONObject<GeometryCollection>();
                    Assert.NotNull(geometryCollection);
                    break;
            }
        }
    }
}

