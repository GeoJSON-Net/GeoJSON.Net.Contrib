using GeoJSON.Net.Geometry;
using System.Data.Entity.Spatial;
using System.Linq;
using Xunit;

namespace GeoJSON.Net.Contrib.EntityFramework.Test
{
    public partial class EntityFrameworkConvertTests
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
        public void FromDbGeography_ToGeoJSONs(int srid, string wkt)
        {
            //Arrange
            var dbGeography = DbGeography.FromText(wkt, srid);

            // Act & Assert
            var geometryObject = dbGeography.ToGeoJSONGeometry();

            Assert.NotNull(geometryObject);

            switch (geometryObject.Type)
            {
                case GeoJSONObjectType.Point:
                    var point = dbGeography.ToGeoJSONObject<Point>();

                    Assert.NotNull(point);
                    Assert.True(point.Equals(geometryObject));

                    Assert.Equal(point.Coordinates.Latitude, dbGeography.Latitude);
                    Assert.Equal(point.Coordinates.Longitude, dbGeography.Longitude);
                    Assert.Equal(point.Coordinates.Altitude, dbGeography.Elevation);
                    break;
                case GeoJSONObjectType.MultiPoint:
                    var multiPoint = dbGeography.ToGeoJSONObject<MultiPoint>();

                    Assert.NotNull(multiPoint);
                    Assert.True(multiPoint.Equals(geometryObject));
                    Assert.Equal(dbGeography.PointCount, multiPoint.Coordinates.Count);

                    for (int i = 1; i < dbGeography.PointCount; i++)
                    {
                        var geographyPoint = dbGeography.PointAt(i);

                        Assert.NotNull(multiPoint.Coordinates.Single(p => p.Coordinates.Latitude == geographyPoint.Latitude
                            && p.Coordinates.Longitude == geographyPoint.Longitude
                            && p.Coordinates.Altitude == geographyPoint.Elevation));
                    }
                    break;
                case GeoJSONObjectType.LineString:
                    var lineString = dbGeography.ToGeoJSONObject<LineString>();

                    Assert.True(lineString.Equals(geometryObject));
                    Assert.NotNull(lineString);
                    Assert.Equal(dbGeography.PointCount, lineString.Coordinates.Count);

                    for (int i = 1; i < dbGeography.PointCount; i++)
                    {
                        var geographyPoint = dbGeography.PointAt(i);

                        Assert.NotNull(lineString.Coordinates.Single(p => p.Latitude == geographyPoint.Latitude
                            && p.Longitude == geographyPoint.Longitude
                            && p.Altitude == geographyPoint.Elevation));
                    }
                    break;
                case GeoJSONObjectType.MultiLineString:
                    var multiLineString = dbGeography.ToGeoJSONObject<MultiLineString>();

                    Assert.True(multiLineString.Equals(geometryObject));
                    Assert.NotNull(multiLineString);
                    Assert.Equal(dbGeography.PointCount, multiLineString.Coordinates.SelectMany(ls => ls.Coordinates).Count());
                    break;
                case GeoJSONObjectType.Polygon:
                    var polygon = dbGeography.ToGeoJSONObject<Polygon>();

                    Assert.True(polygon.Equals(geometryObject));
                    Assert.NotNull(polygon);
                    Assert.Equal(dbGeography.PointCount, polygon.Coordinates.SelectMany(ls => ls.Coordinates).Count());
                    break;
                case GeoJSONObjectType.MultiPolygon:
                    var multiPolygon = dbGeography.ToGeoJSONObject<MultiPolygon>();

                    Assert.True(multiPolygon.Equals(geometryObject));
                    Assert.NotNull(multiPolygon);
                    Assert.Equal(dbGeography.PointCount, multiPolygon.Coordinates.SelectMany(p => p.Coordinates.SelectMany(ls => ls.Coordinates)).Count());
                    break;
                case GeoJSONObjectType.GeometryCollection:
                    var geometryCollection = dbGeography.ToGeoJSONObject<GeometryCollection>();

                    Assert.True(geometryCollection.Equals(geometryObject));
                    Assert.NotNull(geometryCollection);
                    break;
            }
        }

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
        public void FromDbGeometry_ToGeoJSONs(int srid, string wkt)
        {
            //Arrange
            var dbGeometry = DbGeometry.FromText(wkt, srid);

            // Act & Assert
            var geometryObject = dbGeometry.ToGeoJSONGeometry();

            Assert.NotNull(geometryObject);

            switch (geometryObject.Type)
            {
                case GeoJSONObjectType.Point:
                    var point = dbGeometry.ToGeoJSONObject<Point>();

                    Assert.NotNull(point);
                    Assert.True(point.Equals(geometryObject));

                    Assert.Equal(point.Coordinates.Latitude, dbGeometry.YCoordinate);
                    Assert.Equal(point.Coordinates.Longitude, dbGeometry.XCoordinate);
                    Assert.Equal(point.Coordinates.Altitude, dbGeometry.Elevation);
                    break;
                case GeoJSONObjectType.MultiPoint:
                    var multiPoint = dbGeometry.ToGeoJSONObject<MultiPoint>();

                    Assert.NotNull(multiPoint);
                    Assert.True(multiPoint.Equals(geometryObject));
                    Assert.Equal(dbGeometry.PointCount, multiPoint.Coordinates.Count);

                    for (int i = 1; i < dbGeometry.PointCount; i++)
                    {
                        var geographyPoint = dbGeometry.PointAt(i);

                        Assert.NotNull(multiPoint.Coordinates.Single(p => p.Coordinates.Latitude == geographyPoint.YCoordinate
                            && p.Coordinates.Longitude == geographyPoint.XCoordinate
                            && p.Coordinates.Altitude == geographyPoint.Elevation));
                    }
                    break;
                case GeoJSONObjectType.LineString:
                    var lineString = dbGeometry.ToGeoJSONObject<LineString>();

                    Assert.True(lineString.Equals(geometryObject));
                    Assert.NotNull(lineString);
                    Assert.Equal(dbGeometry.PointCount, lineString.Coordinates.Count);

                    for (int i = 1; i < dbGeometry.PointCount; i++)
                    {
                        var geographyPoint = dbGeometry.PointAt(i);

                        Assert.NotNull(lineString.Coordinates.Single(p => p.Latitude == geographyPoint.YCoordinate
                            && p.Longitude == geographyPoint.XCoordinate
                            && p.Altitude == geographyPoint.Elevation));
                    }
                    break;
                case GeoJSONObjectType.MultiLineString:
                    var multiLineString = dbGeometry.ToGeoJSONObject<MultiLineString>();

                    Assert.True(multiLineString.Equals(geometryObject));
                    Assert.NotNull(multiLineString);
                    Assert.Equal(dbGeometry.PointCount, multiLineString.Coordinates.SelectMany(ls => ls.Coordinates).Count());
                    break;
                case GeoJSONObjectType.Polygon:
                    var polygon = dbGeometry.ToGeoJSONObject<Polygon>();

                    Assert.True(polygon.Equals(geometryObject));
                    Assert.NotNull(polygon);
                    Assert.Equal(dbGeometry.PointCount, polygon.Coordinates.SelectMany(ls => ls.Coordinates).Count());
                    break;
                case GeoJSONObjectType.MultiPolygon:
                    var multiPolygon = dbGeometry.ToGeoJSONObject<MultiPolygon>();

                    Assert.True(multiPolygon.Equals(geometryObject));
                    Assert.NotNull(multiPolygon);
                    Assert.Equal(dbGeometry.PointCount, multiPolygon.Coordinates.SelectMany(p => p.Coordinates.SelectMany(ls => ls.Coordinates)).Count());
                    break;
                case GeoJSONObjectType.GeometryCollection:
                    var geometryCollection = dbGeometry.ToGeoJSONObject<GeometryCollection>();

                    Assert.True(geometryCollection.Equals(geometryObject));
                    Assert.NotNull(geometryCollection);
                    break;
            }
        }
    }
}
