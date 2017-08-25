using GeoJSON.Net.Contrib.EntityFramework.WkbConversions;
using System.Linq;
using Xunit;

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
            //Assert.Equal(dbPoint.SpatialTypeName, WkbGeometryType.WkbPoint.ToString());
            Assert.Equal(dbPoint.PointCount, 1);
        }

        [Fact]
        public void ToDbGeographyValidMultiPointTest()
        {
            var dbMultiPoint = multiPoint.ToDbGeography();

            Assert.NotNull(multiPoint);
            Assert.NotNull(dbMultiPoint);
            //Assert.Equal(dbMultiPoint.SpatialTypeName, WkbGeometryType.WkbMultiPoint.ToString());
            Assert.Equal(dbMultiPoint.PointCount, multiPoint.Coordinates.Count);
        }

        [Fact]
        public void ToDbGeographyValidLineStringTest()
        {
            var dbLineString = lineString.ToDbGeography();

            Assert.NotNull(lineString);
            Assert.NotNull(dbLineString);
            //Assert.Equal(dbLineString.SpatialTypeName, WkbGeometryType.WkbMultiLineString.ToString());
            Assert.Equal(dbLineString.PointCount, lineString.Coordinates.Count);
        }

        [Fact]
        public void ToDbGeographyValidMultiLineStringTest()
        {
            var dbMultiLineString = multiLineString.ToDbGeography();

            Assert.NotNull(multiLineString);
            Assert.NotNull(dbMultiLineString);
            //Assert.Equal(dbMultiLineString.SpatialTypeName, WkbGeometryType.WkbMultiLineString.ToString());
            Assert.Equal(dbMultiLineString.ElementCount, multiLineString.Coordinates.Count);
            Assert.Equal(dbMultiLineString.PointCount, multiLineString.Coordinates.SelectMany(ls => ls.Coordinates).Count());
        }

        [Fact]
        public void ToDbGeographyValidPolygonTest()
        {
            var dbPolygon = polygon.ToDbGeography();

            Assert.NotNull(polygon);
            Assert.NotNull(dbPolygon);
            //Assert.Equal(dbPolygon.SpatialTypeName, WkbGeometryType.WkbPolygon.ToString());
            Assert.Equal(dbPolygon.PointCount, polygon.Coordinates.SelectMany(ls => ls.Coordinates).Count());
        }

        [Fact]
        public void ToDbGeographyValidPolygonWithHoleTest()
        {
            var dbPolygon = polygonWithHole.ToDbGeography();

            Assert.NotNull(polygonWithHole);
            Assert.NotNull(dbPolygon);
            //Assert.Equal(dbPolygon.SpatialTypeName, WkbGeometryType.WkbPolygon.ToString());
            Assert.Equal(dbPolygon.PointCount, polygonWithHole.Coordinates.SelectMany(ls => ls.Coordinates).Count());
        }

        [Fact]
        public void ToDbGeographyValidPolygonWithHoleReverseWindingTest()
        {
            var dbPolygon = polygonWithHoleReverseWinding.ToDbGeography();

            Assert.NotNull(polygonWithHoleReverseWinding);
            Assert.NotNull(dbPolygon);
            //Assert.Equal(dbPolygon.SpatialTypeName, WkbGeometryType.WkbPolygon.ToString());
            Assert.Equal(dbPolygon.PointCount, polygonWithHoleReverseWinding.Coordinates.SelectMany(ls => ls.Coordinates).Count());
        }

        [Fact]
        public void ToDbGeographyValidMultiPolygonTest()
        {
            var dbMultiPolygon = multiPolygon.ToDbGeography();

            Assert.NotNull(multiPolygon);
            Assert.NotNull(dbMultiPolygon);
            //Assert.Equal(dbMultiPolygon.SpatialTypeName, WkbGeometryType.WkbMultiPolygon.ToString());
            Assert.Equal(dbMultiPolygon.ElementCount, multiPolygon.Coordinates.Count);
            Assert.Equal(dbMultiPolygon.PointCount, multiPolygon.Coordinates.SelectMany(p => p.Coordinates).SelectMany(ls => ls.Coordinates).Count());
        }

        [Fact]
        public void ToDbGeographyValidGeometryCollectionTest()
        {
            var dbGeomCol = geomCollection.ToDbGeography();

            Assert.NotNull(geomCollection);
            Assert.NotNull(dbGeomCol);
            //Assert.Equal(dbGeomCol.SpatialTypeName, WkbGeometryType.WkbGeometryCollection.ToString());
            Assert.Equal(dbGeomCol.ElementCount, geomCollection.Geometries.Count);
        }
    }
}
