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

        [Fact(Skip = "Equal method does not seem to be working right")]
        public void EncodeDecodeMultiPointTest()
        {
            var processedMultiPoint = multiPoint.ToWkb().ToGeoJSONObject<MultiPoint>();

            Assert.Equal(multiPoint, processedMultiPoint);            
        }

        [Fact(Skip = "Equal method does not seem to be working right")]
        public void EncodeDecodeLineStringTest()
        {
            var processedLineString = lineString.ToWkb().ToGeoJSONObject<LineString>();

            Assert.Equal(lineString, processedLineString);
        }

        [Fact(Skip = "Equal method does not seem to be working right")]
        public void EncodeDecodeMultiLineStringTest()
        {
            var processedMultiLineString = multiLineString.ToWkb().ToGeoJSONObject<MultiLineString>();

            Assert.Equal(multiLineString, processedMultiLineString);      
        }

        [Fact(Skip = "Equal method does not seem to be working right")]
        public void EncodeDecodePolygonTest()
        {
            var processedPolygon = polygon.ToWkb().ToGeoJSONObject<Polygon>();

            Assert.Equal(polygon, processedPolygon);
        }

        [Fact(Skip = "Equal method does not seem to be working right")]
        public void EncodeDecodePolygonWithHoleTest()
        {
            var processedPolygon = polygonWithHole.ToWkb().ToGeoJSONObject<Polygon>();

            Assert.Equal(polygon, processedPolygon);
        }

        [Fact(Skip = "Equal method does not seem to be working right")]
        public void EncodeDecodePolygonWithHoleReverseWindingTest()
        {
            var processedPolygon = polygonWithHoleReverseWinding.ToWkb().ToGeoJSONObject<Polygon>();

            Assert.Equal(polygon, processedPolygon);
        }

        [Fact]
        public void EncodeDecodeMultiPolygonTest()
        {
            var processedMultiPolygon = multiPolygon.ToWkb().ToGeoJSONObject<MultiPolygon>();

            Assert.Equal(multiPolygon, processedMultiPolygon);
        }

        [Fact(Skip = "Equal method does not seem to be working right")]
        public void EncodeDecodeGeometryCollectionTest()
        {
            var processedGeomCol = geomCollection.ToWkb().ToGeoJSONObject<GeometryCollection>();

            Assert.Equal(geomCollection, processedGeomCol);
        }
    }
}
