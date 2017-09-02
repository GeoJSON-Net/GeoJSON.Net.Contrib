using Microsoft.SqlServer.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeoJSON.Net.Geometry;
using GeoJSON.Net.Contrib.MsSqlSpatial;
using System.Data.SqlTypes;
using System;

namespace GeoJSON.Net.MsSqlSpatial.Tests
{
    [TestClass]
    public class ToGeoJSONGeometryTests
    {
        [TestMethod]
        [TestCategory("ToGeoJSONGeometry")]
        public void ValidPointTest()
        {
            IGeometryObject geoJSON = MsSqlSpatialConvert.ToGeoJSONGeometry(simplePoint);
            Point geoJSONobj = MsSqlSpatialConvert.ToGeoJSONObject<Point>(simplePoint);

            Assert.IsNotNull(geoJSON);
            Assert.IsNotNull(geoJSONobj);
            Assert.AreEqual(geoJSON.Type, GeoJSONObjectType.Point);
            Assert.AreEqual(geoJSONobj.Type, GeoJSONObjectType.Point);
            Assert.IsNotNull(geoJSONobj.BoundingBoxes);
            CollectionAssert.AreEqual(geoJSONobj.BoundingBoxes, simplePoint.BoundingBox());

        }

        [TestMethod]
        [TestCategory("ToGeoJSONGeometry")]
        public void ValidMultiPointTest()
        {
            IGeometryObject geoJSON = MsSqlSpatialConvert.ToGeoJSONGeometry(multiPoint);
            var geoJSONobj = MsSqlSpatialConvert.ToGeoJSONObject<MultiPoint>(multiPoint);

            Assert.IsNotNull(geoJSON);
            Assert.IsNotNull(geoJSONobj);
            Assert.AreEqual(geoJSON.Type, GeoJSONObjectType.MultiPoint);
            Assert.AreEqual(geoJSONobj.Type, GeoJSONObjectType.MultiPoint);
            Assert.IsNotNull(geoJSONobj.BoundingBoxes);
            CollectionAssert.AreEqual(geoJSONobj.BoundingBoxes, multiPoint.BoundingBox());
            Assert.AreEqual(multiPoint.STNumGeometries().Value, geoJSONobj.Coordinates.Count);
        }

        [TestMethod]
        [TestCategory("ToGeoJSONGeometry")]
        public void ValidLineStringTest()
        {
            IGeometryObject geoJSON = MsSqlSpatialConvert.ToGeoJSONGeometry(lineString);
            var geoJSONobj = MsSqlSpatialConvert.ToGeoJSONObject<LineString>(lineString);

            Assert.IsNotNull(geoJSON);
            Assert.IsNotNull(geoJSONobj);
            Assert.AreEqual(geoJSON.Type, GeoJSONObjectType.LineString);
            Assert.AreEqual(geoJSONobj.Type, GeoJSONObjectType.LineString);
            Assert.IsNotNull(geoJSONobj.BoundingBoxes);
            CollectionAssert.AreEqual(geoJSONobj.BoundingBoxes, lineString.BoundingBox());
        }

        [TestMethod]
        [TestCategory("ToGeoJSONGeometry")]
        public void ValidMultiLineStringTest()
        {
            IGeometryObject geoJSON = MsSqlSpatialConvert.ToGeoJSONGeometry(multiLineString);
            var geoJSONobj = MsSqlSpatialConvert.ToGeoJSONObject<MultiLineString>(multiLineString);

            Assert.IsNotNull(geoJSON);
            Assert.IsNotNull(geoJSONobj);
            Assert.AreEqual(geoJSON.Type, GeoJSONObjectType.MultiLineString);
            Assert.AreEqual(geoJSONobj.Type, GeoJSONObjectType.MultiLineString);
            Assert.IsNotNull(geoJSONobj.BoundingBoxes);
            CollectionAssert.AreEqual(geoJSONobj.BoundingBoxes, multiLineString.BoundingBox());
        }

        [TestMethod]
        [TestCategory("ToGeoJSONGeometry")]
        public void ValidPolygonTest()
        {
            IGeometryObject geoJSON = MsSqlSpatialConvert.ToGeoJSONGeometry(simplePoly);
            var geoJSONobj = MsSqlSpatialConvert.ToGeoJSONObject<Polygon>(simplePoly);

            Assert.IsNotNull(geoJSON);
            Assert.IsNotNull(geoJSONobj);
            Assert.AreEqual(geoJSON.Type, GeoJSONObjectType.Polygon);
            Assert.AreEqual(geoJSONobj.Type, GeoJSONObjectType.Polygon);
            Assert.IsNotNull(geoJSONobj.BoundingBoxes);
            CollectionAssert.AreEqual(geoJSONobj.BoundingBoxes, simplePoly.BoundingBox());


            geoJSON = MsSqlSpatialConvert.ToGeoJSONGeometry(polyWithHole);
            geoJSONobj = MsSqlSpatialConvert.ToGeoJSONObject<Polygon>(polyWithHole);

            Assert.IsNotNull(geoJSON);
            Assert.IsNotNull(geoJSONobj);
            Assert.AreEqual(geoJSON.Type, GeoJSONObjectType.Polygon);
            Assert.AreEqual(geoJSONobj.Type, GeoJSONObjectType.Polygon);
            CollectionAssert.AreEqual(geoJSONobj.BoundingBoxes, polyWithHole.BoundingBox());
            Assert.AreEqual(geoJSONobj.Coordinates.Count, 2);
        }

        [TestMethod]
        [TestCategory("ToGeoJSONGeometry")]
        public void ValidMultiPolygonTest()
        {
            IGeometryObject geoJSON = MsSqlSpatialConvert.ToGeoJSONGeometry(multiPolygon);
            var geoJSONobj = MsSqlSpatialConvert.ToGeoJSONObject<MultiPolygon>(multiPolygon);

            Assert.IsNotNull(geoJSON);
            Assert.IsNotNull(geoJSONobj);
            Assert.AreEqual(geoJSON.Type, GeoJSONObjectType.MultiPolygon);
            Assert.AreEqual(geoJSONobj.Type, GeoJSONObjectType.MultiPolygon);
            CollectionAssert.AreEqual(geoJSONobj.BoundingBoxes, multiPolygon.BoundingBox());
            Assert.AreEqual(geoJSONobj.Coordinates.Count, 3);
            Assert.AreEqual(geoJSONobj.Coordinates[0].Coordinates.Count, 1);
            Assert.AreEqual(geoJSONobj.Coordinates[1].Coordinates.Count, 2);
            Assert.AreEqual(geoJSONobj.Coordinates[2].Coordinates.Count, 2);

            Assert.AreEqual(geoJSONobj.Coordinates[0].Coordinates[0].Coordinates.Count, 4);
            Assert.AreEqual(geoJSONobj.Coordinates[1].Coordinates[0].Coordinates.Count, 6);
            Assert.AreEqual(geoJSONobj.Coordinates[1].Coordinates[1].Coordinates.Count, 4);
            Assert.AreEqual(geoJSONobj.Coordinates[2].Coordinates[0].Coordinates.Count, 5);
            Assert.AreEqual(geoJSONobj.Coordinates[2].Coordinates[1].Coordinates.Count, 5);
        }

        [TestMethod]
        [TestCategory("ToGeoJSONGeometry")]
        public void ValidGeometryCollectionTest()
        {
            IGeometryObject geoJSON = MsSqlSpatialConvert.ToGeoJSONGeometry(geomCol);
            var geoJSONobj = MsSqlSpatialConvert.ToGeoJSONObject<GeometryCollection>(geomCol);

            Assert.IsNotNull(geoJSON);
            Assert.IsNotNull(geoJSONobj);
            Assert.AreEqual(geoJSON.Type, GeoJSONObjectType.GeometryCollection);
            Assert.AreEqual(geoJSONobj.Type, GeoJSONObjectType.GeometryCollection);
            Assert.IsTrue(geoJSONobj.BoundingBoxes.Length == 4);
            CollectionAssert.AreEqual(geoJSONobj.BoundingBoxes, geomCol.BoundingBox());
            Assert.AreEqual(geoJSONobj.Geometries.Count, 3);
            Assert.AreEqual(geoJSONobj.Geometries[0].Type, GeoJSONObjectType.Polygon);
            Assert.AreEqual(geoJSONobj.Geometries[1].Type, GeoJSONObjectType.Point);
            Assert.AreEqual(geoJSONobj.Geometries[2].Type, GeoJSONObjectType.MultiLineString);

            Assert.AreEqual(((Polygon)geoJSONobj.Geometries[0]).Coordinates[0].Coordinates.Count, 5);
            Assert.AreEqual(((MultiLineString)geoJSONobj.Geometries[2]).Coordinates.Count, 0);
        }

        [TestMethod]
        [TestCategory("ToGeoJSONGeometry")]
        public void TestEmptyMultiPoint()
        {
            SqlGeometryBuilder builder = new SqlGeometryBuilder();
            builder.SetSrid(4326);
            builder.BeginGeometry(OpenGisGeometryType.MultiPoint);
            builder.EndGeometry();
            var multiPoint = builder.ConstructedGeometry;
            var geoJSON = multiPoint.ToGeoJSONGeometry();
            var geoJSONobj = multiPoint.ToGeoJSONObject<MultiPoint>();
            Assert.IsNotNull(geoJSON);
            Assert.IsNotNull(geoJSONobj);
            Assert.AreEqual(geoJSON.Type, GeoJSONObjectType.MultiPoint);
            Assert.AreEqual(geoJSONobj.Type, GeoJSONObjectType.MultiPoint);
            Assert.IsTrue(geoJSONobj.BoundingBoxes.Length == 4);
            CollectionAssert.AreEqual(geoJSONobj.BoundingBoxes, multiPoint.BoundingBox());
            var geom = geoJSONobj.ToSqlGeometry();
            Assert.IsTrue(geom.STGeometryType().Value == "MultiPoint");
            Assert.IsTrue(geom.STIsEmpty().IsTrue);
        }

        [TestMethod]
        [TestCategory("ToGeoJSONGeometry")]
        public void TestEmptyMultiPolygon()
        {
            SqlGeometryBuilder builder = new SqlGeometryBuilder();
            builder.SetSrid(4326);
            builder.BeginGeometry(OpenGisGeometryType.MultiPolygon);
            builder.EndGeometry();
            var multiPoly = builder.ConstructedGeometry;
            var geoJSON = multiPoly.ToGeoJSONGeometry();
            var geoJSONobj = multiPoly.ToGeoJSONObject<MultiPolygon>();
            Assert.IsNotNull(geoJSON);
            Assert.IsNotNull(geoJSONobj);
            Assert.AreEqual(geoJSON.Type, GeoJSONObjectType.MultiPolygon);
            Assert.AreEqual(geoJSONobj.Type, GeoJSONObjectType.MultiPolygon);
            Assert.IsTrue(geoJSONobj.BoundingBoxes.Length == 4);
            CollectionAssert.AreEqual(geoJSONobj.BoundingBoxes, multiPoly.BoundingBox());
            var geom = geoJSONobj.ToSqlGeometry();
            Assert.IsTrue(geom.STGeometryType().Value == "MultiPolygon");
            Assert.IsTrue(geom.STIsEmpty().IsTrue);
        }

        [TestMethod]
        [TestCategory("ToGeoJSONGeometry")]
        public void TestEmptyMultiLineString()
        {
            SqlGeometryBuilder builder = new SqlGeometryBuilder();
            builder.SetSrid(4326);
            builder.BeginGeometry(OpenGisGeometryType.MultiLineString);
            builder.EndGeometry();
            var multiLineString = builder.ConstructedGeometry;
            var geoJSON = multiLineString.ToGeoJSONGeometry();
            var geoJSONobj = multiLineString.ToGeoJSONObject<MultiLineString>();
            Assert.IsNotNull(geoJSON);
            Assert.IsNotNull(geoJSONobj);
            Assert.AreEqual(geoJSON.Type, GeoJSONObjectType.MultiLineString);
            Assert.AreEqual(geoJSONobj.Type, GeoJSONObjectType.MultiLineString);
            Assert.IsTrue(geoJSONobj.BoundingBoxes.Length == 4);
            CollectionAssert.AreEqual(geoJSONobj.BoundingBoxes, multiLineString.BoundingBox());
            var geom = geoJSONobj.ToSqlGeometry();
            Assert.IsTrue(geom.STGeometryType().Value == "MultiLineString");
            Assert.IsTrue(geom.STIsEmpty().IsTrue);
        }

        [TestMethod]
        [TestCategory("ToGeoJSONGeometry")]
        public void TestEmptGeometryCollection()
        {
            SqlGeometryBuilder builder = new SqlGeometryBuilder();
            builder.SetSrid(4326);
            builder.BeginGeometry(OpenGisGeometryType.GeometryCollection);
            builder.EndGeometry();
            var geomCollection = builder.ConstructedGeometry;
            var geoJSON = geomCollection.ToGeoJSONGeometry();
            var geoJSONobj = geomCollection.ToGeoJSONObject<GeometryCollection>();
            Assert.IsNotNull(geoJSON);
            Assert.IsNotNull(geoJSONobj);
            Assert.AreEqual(geoJSON.Type, GeoJSONObjectType.GeometryCollection);
            Assert.AreEqual(geoJSONobj.Type, GeoJSONObjectType.GeometryCollection);
            Assert.IsTrue(geoJSONobj.BoundingBoxes.Length == 4);
            CollectionAssert.AreEqual(geoJSONobj.BoundingBoxes, geomCollection.BoundingBox());
            var geom = geoJSONobj.ToSqlGeometry();
            Assert.IsTrue(geom.STGeometryType().Value == "GeometryCollection");
            Assert.IsTrue(geom.STIsEmpty().IsTrue);

        }

        #region Test geometries

        SqlGeometry simplePoint = SqlGeometry.Parse(new SqlString(WktSamples.POINT));
        SqlGeometry multiPoint = SqlGeometry.Parse(new SqlString(WktSamples.MULTIPOINT));
        SqlGeometry lineString = SqlGeometry.Parse(new SqlString(WktSamples.LINESTRING));
        SqlGeometry multiLineString = SqlGeometry.Parse(new SqlString(WktSamples.MULTILINESTRING));

        SqlGeometry simplePoly = SqlGeometry.Parse(new SqlString(WktSamples.POLYGON_SIMPLE));
        SqlGeometry polyWithHole = SqlGeometry.Parse(new SqlString(WktSamples.POLYGON_WITH_HOLE));
        SqlGeometry multiPolygon = SqlGeometry.Parse(new SqlString(WktSamples.MULTIPOLYGON));

        SqlGeometry geomCol = SqlGeometry.Parse(new SqlString(WktSamples.GEOMETRYCOLLECTION));


        #endregion

    }
}
