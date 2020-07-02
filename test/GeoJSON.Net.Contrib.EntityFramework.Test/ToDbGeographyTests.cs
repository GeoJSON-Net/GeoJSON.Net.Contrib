using System;
using System.Linq;
using GeoJSON.Net.Contrib.Wkb.Conversions;
using Xunit;

#pragma warning disable 618

namespace GeoJSON.Net.Contrib.EntityFramework.Test
{
    public partial class EntityFrameworkConvertTests
    {
        [Fact]
        public void ToDbGeographyValidPointTest()
        {
            var dbPoint = point.ToDbGeography();

            Assert.NotNull(point);
            Assert.NotNull(dbPoint);
            Assert.Equal(WkbGeometryType.Point.ToString(), dbPoint.SpatialTypeName);
            Assert.Equal(1, dbPoint.PointCount);
        }

        [Fact]
        public void ToDbGeographyValidMultiPointTest()
        {
            var dbMultiPoint = multiPoint.ToDbGeography();

            Assert.NotNull(multiPoint);
            Assert.NotNull(dbMultiPoint);
            Assert.Equal(WkbGeometryType.MultiPoint.ToString(), dbMultiPoint.SpatialTypeName);
            Assert.Equal(multiPoint.Coordinates.Count, dbMultiPoint.PointCount);
        }

        [Fact]
        public void ToDbGeographyValidLineStringTest()
        {
            var dbLineString = lineString.ToDbGeography();

            Assert.NotNull(lineString);
            Assert.NotNull(dbLineString);
            Assert.Equal(WkbGeometryType.LineString.ToString(), dbLineString.SpatialTypeName);
            Assert.Equal(lineString.Coordinates.Count, dbLineString.PointCount);
        }

        [Fact]
        public void ToDbGeographyValidMultiLineStringTest()
        {
            var dbMultiLineString = multiLineString.ToDbGeography();

            Assert.NotNull(multiLineString);
            Assert.NotNull(dbMultiLineString);
            Assert.Equal(WkbGeometryType.MultiLineString.ToString(), dbMultiLineString.SpatialTypeName);
            Assert.Equal(dbMultiLineString.ElementCount, multiLineString.Coordinates.Count);
            Assert.Equal(multiLineString.Coordinates.SelectMany(ls => ls.Coordinates).Count(), dbMultiLineString.PointCount);
        }

        [Fact]
        public void ToDbGeographyValidPolygonTest()
        {
            var dbPolygon = polygon.ToDbGeography();

            Assert.NotNull(polygon);
            Assert.NotNull(dbPolygon);
            Assert.Equal(WkbGeometryType.Polygon.ToString(), dbPolygon.SpatialTypeName);
            Assert.Equal(polygon.Coordinates.SelectMany(ls => ls.Coordinates).Count(), dbPolygon.PointCount);
        }

        [Fact]
        public void ToDbGeographyValidPolygonWithHoleTest()
        {
            var dbPolygon = polygonWithHole.ToDbGeography();

            Assert.NotNull(polygonWithHole);
            Assert.NotNull(dbPolygon);
            Assert.Equal(WkbGeometryType.Polygon.ToString(), dbPolygon.SpatialTypeName);
            Assert.Equal(polygonWithHole.Coordinates.SelectMany(ls => ls.Coordinates).Count(), dbPolygon.PointCount);
        }

        [Fact(Skip = "Does not throw exception as expected")]
        public void ToDbGeographyValidPolygonWithHoleReverseWindingTest()
        {
            // The reverse should not work due to geographycal restrictions:
            // Message: Microsoft.SqlServer.Types.GLArgumentException : 24205:
            // The specified input does not represent a valid geography instance because
            // it exceeds a single hemisphere. Each geography instance must fit inside a single hemisphere.
            // A common reason for this error is that a polygon has the wrong ring orientation.
            Assert.ThrowsAny<Exception>(() => polygonWithHoleReverseWinding.ToDbGeography());
        }

        [Fact]
        public void ToDbGeographyValidMultiPolygonTest()
        {
            var dbMultiPolygon = multiPolygon.ToDbGeography();

            Assert.NotNull(multiPolygon);
            Assert.NotNull(dbMultiPolygon);
            Assert.Equal(WkbGeometryType.MultiPolygon.ToString(), dbMultiPolygon.SpatialTypeName);
            Assert.Equal(multiPolygon.Coordinates.Count, dbMultiPolygon.ElementCount);
            Assert.Equal(multiPolygon.Coordinates.SelectMany(p => p.Coordinates).SelectMany(ls => ls.Coordinates).Count(), dbMultiPolygon.PointCount);
        }

        [Fact]
        public void ToDbGeographyValidGeometryCollectionTest()
        {
            var dbGeomCol = geomCollection.ToDbGeography();

            Assert.NotNull(geomCollection);
            Assert.NotNull(dbGeomCol);
            Assert.Equal(WkbGeometryType.GeometryCollection.ToString(), dbGeomCol.SpatialTypeName);
            Assert.Equal(geomCollection.Geometries.Count, dbGeomCol.ElementCount);
        }
    }
}
