using GeoJSON.Net.Geometry;
using Xunit;

namespace GeoJSON.Net.Contrib.Wkb.Test
{
    public partial class WkbConversionsTests
    {
        [Fact]
        public void EncodeDecodePointTest()
        {
            var processedPoint = point.ToWkb().ToGeoJSONObject<Point>();

            Assert.Equal(point, processedPoint);
        }

        [Fact]
        public void EncodeDecodePointZTest()
        {
            var processedPointZ = pointZ.ToWkb().ToGeoJSONObject<Point>();

            Assert.Equal(pointZ, processedPointZ);
        }

        [Fact]
        public void EncodeDecodeMultiPointTest()
        {
            var processedMultiPoint = multiPoint.ToWkb().ToGeoJSONObject<MultiPoint>();

            Assert.Equal(multiPoint, processedMultiPoint);
        }

        [Fact]
        public void EncodeDecodeMultiPointZTest()
        {
            var processedMultiPointZ = multiPointZ.ToWkb().ToGeoJSONObject<MultiPoint>();

            Assert.Equal(multiPointZ, processedMultiPointZ);
        }

        [Fact]
        public void EncodeDecodeLineStringTest()
        {
            var processedLineString = lineString.ToWkb().ToGeoJSONObject<LineString>();

            Assert.Equal(lineString, processedLineString);
        }

        [Fact]
        public void EncodeDecodeLineStringZTest()
        {
            var processedLineStringZ = lineStringZ.ToWkb().ToGeoJSONObject<LineString>();

            Assert.Equal(lineStringZ, processedLineStringZ);
        }

        [Fact]
        public void EncodeDecodeMultiLineStringTest()
        {
            var processedMultiLineString = multiLineString.ToWkb().ToGeoJSONObject<MultiLineString>();

            Assert.Equal(multiLineString, processedMultiLineString);
        }

        [Fact]
        public void EncodeDecodeMultiLineStringZTest()
        {
            var processedMultiLineStringZ = multiLineStringZ.ToWkb().ToGeoJSONObject<MultiLineString>();

            Assert.Equal(multiLineStringZ, processedMultiLineStringZ);
        }

        [Fact]
        public void EncodeDecodePolygonTest()
        {
            var processedPolygon = polygon.ToWkb().ToGeoJSONObject<Polygon>();

            Assert.Equal(polygon, processedPolygon);
        }

        [Fact]
        public void EncodeDecodePolygonZTest()
        {
            var processedPolygonZ = polygonZ.ToWkb().ToGeoJSONObject<Polygon>();

            Assert.Equal(polygonZ, processedPolygonZ);
        }

        [Fact]
        public void EncodeDecodePolygonWithHoleTest()
        {
            var processedPolygon = polygonWithHole.ToWkb().ToGeoJSONObject<Polygon>();

            Assert.Equal(polygonWithHole, processedPolygon);
        }

        [Fact]
        public void EncodeDecodePolygonWithHoleReverseWindingTest()
        {
            var processedPolygon = polygonWithHoleReverseWinding.ToWkb().ToGeoJSONObject<Polygon>();

            Assert.Equal(polygonWithHoleReverseWinding, processedPolygon);
        }

        [Fact]
        public void EncodeDecodeMultiPolygonTest()
        {
            var processedMultiPolygon = multiPolygon.ToWkb().ToGeoJSONObject<MultiPolygon>();

            Assert.Equal(multiPolygon, processedMultiPolygon);
        }

        [Fact]
        public void EncodeDecodeMultiPolygonZTest()
        {
            var processedMultiPolygonZ = multiPolygonZ.ToWkb().ToGeoJSONObject<MultiPolygon>();

            Assert.Equal(multiPolygonZ, processedMultiPolygonZ);
        }

        [Fact]
        public void EncodeDecodeGeometryCollectionTest()
        {
            var processedGeomCol = geomCollection.ToWkb().ToGeoJSONObject<GeometryCollection>();

            Assert.Equal(geomCollection, processedGeomCol);
        }

        [Fact]
        public void EncodeDecodeGeometryCollectionZTest()
        {
            var processedGeomColZ = geomCollectionZ.ToWkb().ToGeoJSONObject<GeometryCollection>();

            Assert.Equal(geomCollectionZ, processedGeomColZ);
        }
    }
}
