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
	/// WktHelper tests
	/// </summary>
    [TestClass]
    public class WktTests
    {
        [TestMethod]
        public void Wkt_ToIGeometryObject()
        {
            IGeometryObject geom = WktHelper.GeoJSONGeometry(WktSamples.LINESTRING);

            Assert.IsNotNull(geom);
            Assert.AreEqual(geom.Type, GeoJSONObjectType.LineString);


            geom = WktHelper.GeoJSONGeometry(WktSamples.MULTILINESTRING_EMPY);
            Assert.IsNotNull(geom);
            Assert.AreEqual(geom.Type, GeoJSONObjectType.MultiLineString);

        }

        [TestMethod]
        public void Wkt_ToGeoJsonObject()
        {
            LineString lineString = WktHelper.GeoJSONObject<LineString>(WktSamples.LINESTRING);

            Assert.IsNotNull(lineString);
            Assert.AreEqual(lineString.Coordinates.Count, 5);

            MultiLineString multiLineString = WktHelper.GeoJSONObject<MultiLineString>(WktSamples.MULTILINESTRING_EMPY);

            Assert.IsNotNull(multiLineString);
            Assert.AreEqual(multiLineString.Coordinates.Count, 0);

        }
    }
}
