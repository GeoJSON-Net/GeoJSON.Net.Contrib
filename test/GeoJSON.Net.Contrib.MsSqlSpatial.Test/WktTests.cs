using GeoJSON.Net.Geometry;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoJSON.Net.Contrib.MsSqlSpatial.Test
{
    /// <summary>
    /// WktConvert tests
    /// </summary>
    [TestClass]
    public class WktTests
    {
        [TestMethod]
        [TestCategory("WKT")]
        public void Wkt_ToIGeometryObject()
        {
            IGeometryObject geom = WktConvert.GeoJSONGeometry(WktSamples.LINESTRING);

            Assert.IsNotNull(geom);
            Assert.AreEqual(geom.Type, GeoJSONObjectType.LineString);


            geom = WktConvert.GeoJSONGeometry(WktSamples.MULTILINESTRING_EMPY);
            Assert.IsNotNull(geom);
            Assert.AreEqual(geom.Type, GeoJSONObjectType.MultiLineString);
        }

        [TestMethod]
        [TestCategory("WKT")]
        public void Wkt_ToGeoJsonObject()
        {
            LineString lineString = WktConvert.GeoJSONObject<LineString>(WktSamples.LINESTRING);

            Assert.IsNotNull(lineString);
            Assert.AreEqual(lineString.Coordinates.Count, 5);

            MultiLineString multiLineString = WktConvert.GeoJSONObject<MultiLineString>(WktSamples.MULTILINESTRING_EMPY);

            Assert.IsNotNull(multiLineString);
            Assert.AreEqual(multiLineString.Coordinates.Count, 0);
        }
    }
}
