using GeoJSON.Net.Contrib.MsSqlSpatial;
using GeoJSON.Net.Geometry;
using Microsoft.SqlServer.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoJSON.Net.MsSqlSpatial.Tests
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
