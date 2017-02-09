using System;
using System.Linq;
using Microsoft.SqlServer.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeoJSON.Net.Geometry;
using GeoJSON.Net.Contrib.MsSqlSpatial;
using System.Data.SqlTypes;

namespace GeoJSON.Net.MsSqlSpatial.Tests
{
    [TestClass]
    public class ToGeoJSONGeographyTests
    {
        [TestMethod]
        [TestCategory("ToGeoJSONGeography")]
        public void ValidPointTest_Geography()
        {
            IGeometryObject geoJSON = MsSqlSpatialConvert.ToGeoJSONGeometry(simplePoint);
            Point geoJSONobj = MsSqlSpatialConvert.ToGeoJSONObject<Point>(simplePoint);

            Assert.IsNotNull(geoJSON);
            Assert.IsNotNull(geoJSONobj);
            Assert.AreEqual(geoJSON.Type, GeoJSONObjectType.Point);
            Assert.AreEqual(geoJSONobj.Type, GeoJSONObjectType.Point);
            Assert.IsNotNull(geoJSONobj.BoundingBoxes);
            CollectionAssert.AreEqual(geoJSONobj.BoundingBoxes, simplePoint.BoundingBox());
            Assert.AreEqual(((GeographicPosition)geoJSONobj.Coordinates).Latitude, simplePoint.Lat.Value);
            Assert.AreEqual(((GeographicPosition)geoJSONobj.Coordinates).Longitude, simplePoint.Long.Value);

        }

        [TestMethod]
        [TestCategory("ToGeoJSONGeography")]
        public void ValidMultiPointTest_Geography()
        {
            IGeometryObject geoJSON = MsSqlSpatialConvert.ToGeoJSONGeometry(multiPoint);
            MultiPoint geoJSONobj = MsSqlSpatialConvert.ToGeoJSONObject<MultiPoint>(multiPoint);

            Assert.IsNotNull(geoJSON);
            Assert.IsNotNull(geoJSONobj);
            Assert.AreEqual(geoJSON.Type, GeoJSONObjectType.MultiPoint);
            Assert.AreEqual(geoJSONobj.Type, GeoJSONObjectType.MultiPoint);
            Assert.IsNotNull(geoJSONobj.BoundingBoxes);
            CollectionAssert.AreEqual(geoJSONobj.BoundingBoxes, multiPoint.BoundingBox());
        }

        [TestMethod]
        [TestCategory("ToGeoJSONGeography")]
        public void ValidLineStringTest_Geography()
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
        [TestCategory("ToGeoJSONGeography")]
        public void ValidMultiLineStringTest_Geography()
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
        [TestCategory("ToGeoJSONGeography")]
        public void ValidPolygonTest_Geography()
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
        [TestCategory("ToGeoJSONGeography")]
        public void ValidMultiPolygonTest_Geography()
        {
            IGeometryObject geoJSON = MsSqlSpatialConvert.ToGeoJSONGeometry(multiPolygon);
            var geoJSONobj = MsSqlSpatialConvert.ToGeoJSONObject<MultiPolygon>(multiPolygon);

            Assert.IsNotNull(geoJSON);
            Assert.IsNotNull(geoJSONobj);
            Assert.AreEqual(geoJSON.Type, GeoJSONObjectType.MultiPolygon);
            Assert.AreEqual(geoJSONobj.Type, GeoJSONObjectType.MultiPolygon);
            CollectionAssert.AreEqual(geoJSONobj.BoundingBoxes, multiPolygon.BoundingBox());
            Assert.AreEqual(geoJSONobj.Coordinates.Count, 3);
            Assert.AreEqual(geoJSONobj.Coordinates[1].Coordinates.Count, 2);
            Assert.AreEqual(geoJSONobj.Coordinates[1].Coordinates[0].Coordinates.Count, 6);
            Assert.AreEqual(geoJSONobj.Coordinates[1].Coordinates[1].Coordinates.Count, 4);
            Assert.AreEqual(geoJSONobj.Coordinates[2].Coordinates[0].Coordinates.Count, 5);
            Assert.AreEqual(geoJSONobj.Coordinates[2].Coordinates[1].Coordinates.Count, 5);
        }

        [TestMethod]
        [TestCategory("ToGeoJSONGeography")]
        public void ValidGeometryCollectionTest_Geography()
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
        }

        [TestMethod]
        [TestCategory("ToGeoJSONGeography")]
        public void TestEmptyMultiPoint_Geography()
        {
            SqlGeographyBuilder builder = new SqlGeographyBuilder();
            builder.SetSrid(4326);
            builder.BeginGeography(OpenGisGeographyType.MultiPoint);
            builder.EndGeography();
            var multiPoint = builder.ConstructedGeography;
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
        [TestCategory("ToGeoJSONGeography")]
        public void TestEmptyMultiPolygon_Geography()
        {
            SqlGeographyBuilder builder = new SqlGeographyBuilder();
            builder.SetSrid(4326);
            builder.BeginGeography(OpenGisGeographyType.MultiPolygon);
            builder.EndGeography();
            var multiPoly = builder.ConstructedGeography;
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
        [TestCategory("ToGeoJSONGeography")]
        public void TestEmptyMultiLineString_Geography()
        {
            SqlGeographyBuilder builder = new SqlGeographyBuilder();
            builder.SetSrid(4326);
            builder.BeginGeography(OpenGisGeographyType.MultiLineString);
            builder.EndGeography();
            var multiLineString = builder.ConstructedGeography;
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
        [TestCategory("ToGeoJSONGeography")]
        public void TestEmptGeometryCollection_Geography()
        {
            SqlGeographyBuilder builder = new SqlGeographyBuilder();
            builder.SetSrid(4326);
            builder.BeginGeography(OpenGisGeographyType.GeometryCollection);
            builder.EndGeography();
            var geomCollection = builder.ConstructedGeography;
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

        #region Test geographies

        SqlGeography simplePoint = SqlGeography.Parse(new SqlString(WktSamples.POINT)).MakeValidIfInvalid();
        SqlGeography multiPoint = SqlGeography.Parse(new SqlString(WktSamples.MULTIPOINT)).MakeValidIfInvalid();
        SqlGeography lineString = SqlGeography.Parse(new SqlString(WktSamples.LINESTRING)).MakeValidIfInvalid();
        SqlGeography multiLineString = SqlGeography.Parse(new SqlString(WktSamples.MULTILINESTRING)).MakeValidIfInvalid();

        SqlGeography simplePoly = SqlGeography.Parse(new SqlString(WktSamples.POLYGON_SIMPLE)).MakeValidIfInvalid();
        SqlGeography polyWithHole = SqlGeography.Parse(new SqlString(WktSamples.POLYGON_WITH_HOLE)).MakeValidIfInvalid();
        SqlGeography multiPolygon = SqlGeography.Parse(new SqlString(WktSamples.MULTIPOLYGON)).MakeValidIfInvalid();

        SqlGeography geomCol = SqlGeography.Parse(new SqlString(WktSamples.GEOMETRYCOLLECTION)).MakeValidIfInvalid();

        #endregion

    }
}
