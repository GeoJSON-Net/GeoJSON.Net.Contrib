using System.Linq;
using GeoJSON.Net.Contrib.EntityFramework.WkbConversions;
using Xunit;

namespace GeoJSON.Net.Contrib.EntityFramework.Test
{
    public partial class EntityFrameworkConvertTests
    {
        [Fact]
        public void ToDbGeometryValidPointTest()
        {
            var dbPoint = point.ToDbGeometry();

            Assert.NotNull(point);
            Assert.NotNull(dbPoint);
            Assert.Equal(WkbGeometryType.Point.ToString(), dbPoint.SpatialTypeName);
            Assert.Equal(1, dbPoint.PointCount);
        }

        [Fact]
        public void ToDbGeometryValidMultiPointTest()
        {
            var dbMultiPoint = multiPoint.ToDbGeometry();

            Assert.NotNull(multiPoint);
            Assert.NotNull(dbMultiPoint);
            Assert.Equal(WkbGeometryType.MultiPoint.ToString(), dbMultiPoint.SpatialTypeName);
            Assert.Equal(multiPoint.Coordinates.Count, dbMultiPoint.PointCount);
        }

        [Fact]
        public void ToDbGeometryValidLineStringTest()
        {
            var dbLineString = lineString.ToDbGeometry();

            Assert.NotNull(lineString);
            Assert.NotNull(dbLineString);
            Assert.Equal(WkbGeometryType.LineString.ToString(), dbLineString.SpatialTypeName);
            Assert.Equal(lineString.Coordinates.Count, dbLineString.PointCount);
        }

        [Fact]
        public void ToDbGeometryValidMultiLineStringTest()
        {
            var dbMultiLineString = multiLineString.ToDbGeometry();

            Assert.NotNull(multiLineString);
            Assert.NotNull(dbMultiLineString);
            Assert.Equal(WkbGeometryType.MultiLineString.ToString(), dbMultiLineString.SpatialTypeName);
            Assert.Equal(dbMultiLineString.ElementCount, multiLineString.Coordinates.Count);
            Assert.Equal(multiLineString.Coordinates.SelectMany(ls => ls.Coordinates).Count(), dbMultiLineString.PointCount);
        }

        [Fact]
        public void ToDbGeometryValidPolygonTest()
        {
            var dbPolygon = polygon.ToDbGeometry();

            Assert.NotNull(polygon);
            Assert.NotNull(dbPolygon);
            Assert.Equal(WkbGeometryType.Polygon.ToString(), dbPolygon.SpatialTypeName);
            Assert.Equal(polygon.Coordinates.SelectMany(ls => ls.Coordinates).Count(), dbPolygon.PointCount);
        }

        [Fact]
        public void ToDbGeometryValidPolygonWithHoleTest()
        {
            var dbPolygon = polygonWithHole.ToDbGeometry();

            Assert.NotNull(polygonWithHole);
            Assert.NotNull(dbPolygon);
            Assert.Equal(WkbGeometryType.Polygon.ToString(), dbPolygon.SpatialTypeName);
            Assert.Equal(polygonWithHole.Coordinates.SelectMany(ls => ls.Coordinates).Count(), dbPolygon.PointCount);
        }

        [Fact]
        public void ToDbGeometryValidPolygonWithHoleReverseWindingTest()
        {
            var dbPolygon = polygonWithHoleReverseWinding.ToDbGeometry();

            Assert.NotNull(polygonWithHoleReverseWinding);
            Assert.NotNull(dbPolygon);
            Assert.Equal(WkbGeometryType.Polygon.ToString(), dbPolygon.SpatialTypeName);
            Assert.Equal(polygonWithHoleReverseWinding.Coordinates.SelectMany(ls => ls.Coordinates).Count(), dbPolygon.PointCount);
        }

        [Fact]
        public void ToDbGeometryValidMultiPolygonTest()
        {
            var dbMultiPolygon = multiPolygon.ToDbGeometry();

            Assert.NotNull(multiPolygon);
            Assert.NotNull(dbMultiPolygon);
            Assert.Equal(WkbGeometryType.MultiPolygon.ToString(), dbMultiPolygon.SpatialTypeName);
            Assert.Equal(multiPolygon.Coordinates.Count, dbMultiPolygon.ElementCount);
            Assert.Equal(multiPolygon.Coordinates.SelectMany(p => p.Coordinates).SelectMany(ls => ls.Coordinates).Count(), dbMultiPolygon.PointCount);
        }

        [Fact]
        public void ToDbGeometryValidGeometryCollectionTest()
        {
            var dbGeomCol = geomCollection.ToDbGeometry();

            Assert.NotNull(geomCollection);
            Assert.NotNull(dbGeomCol);
            Assert.Equal(WkbGeometryType.GeometryCollection.ToString(), dbGeomCol.SpatialTypeName);
            Assert.Equal(geomCollection.Geometries.Count, dbGeomCol.ElementCount);
        }
    }
}
