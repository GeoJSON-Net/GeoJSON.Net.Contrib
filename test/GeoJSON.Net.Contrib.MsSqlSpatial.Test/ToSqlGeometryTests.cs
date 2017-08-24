using System;
using System.Linq;
using Microsoft.SqlServer.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeoJSON.Net.Geometry;
using GeoJSON.Net.Contrib.MsSqlSpatial;
using System.Data.SqlTypes;
using System.Collections.Generic;
using GeoJSON.Net.Feature;

namespace GeoJSON.Net.MsSqlSpatial.Tests
{
    [TestClass]
    public class ToSqlGeometryTests
    {
        Point point;
        MultiPoint multiPoint;
        LineString lineString;
        MultiLineString multiLineString;
        Polygon polygon;
        Polygon polygonWithHole;
        MultiPolygon multiPolygon;
        GeometryCollection geomCollection;
        Feature.Feature feature;
        FeatureCollection featureCollection;
        public ToSqlGeometryTests()
        {
            point = new Point(new Position(53.2455662, 90.65464646));

            multiPoint = new MultiPoint(new List<Point>
                {
                    new Point(new Position(52.379790828551016, 5.3173828125)),
                    new Point(new Position(52.36721467920585, 5.456085205078125)),
                    new Point(new Position(52.303440474272755, 5.386047363281249, 4.23))
                });
            lineString = new LineString(new List<IPosition>
                {
                    new Position(52.379790828551016, 5.3173828125),
                    new Position(52.36721467920585, 5.456085205078125),
                    new Position(52.303440474272755, 5.386047363281249, 4.23)
                });
            multiLineString = new MultiLineString(new List<LineString>
                {
                    new LineString(new List<IPosition>
                    {
                        new Position(52.379790828551016, 5.3173828125),
                        new Position(52.36721467920585, 5.456085205078125),
                        new Position(52.303440474272755, 5.386047363281249, 4.23)
                    }),
                    new LineString(new List<IPosition>
                    {
                        new Position(52.379790828551016, 5.3273828125),
                        new Position(52.36721467920585, 5.486085205078125),
                        new Position(52.303440474272755, 5.426047363281249, 4.23)
                    })
                });

            polygonWithHole = new Polygon(new List<LineString>
                {
                    new LineString(new List<Position>
                    {
                                            new Position(0.516357421875, 47.6415668949958),
                                            new Position(0.516357421875, 47.34463879017405),
                                            new Position(0.977783203125, 47.22539733216678),
                                            new Position(1.175537109375, 47.463611506072866),
                                            new Position(0.516357421875, 47.6415668949958)
                    }),
                                         new LineString(new List<Position>
                                            {
                                                new Position(0.630340576171875, 47.54944962456812),
                                                new Position(0.630340576171875, 47.49380564962583),
                                                new Position(0.729217529296875, 47.482669772098674),
                                                new Position(0.731964111328125, 47.53276262898896),
                                                new Position(0.630340576171875, 47.54944962456812)
                                            })
                                });
            polygon = new Polygon(new List<LineString>
                {
                    new LineString(new List<Position>
                    {
                        new Position(52.379790828551016, 5.3173828125),
                        new Position(52.36721467920585, 5.456085205078125),
                        new Position(52.303440474272755, 5.386047363281249, 4.23),
                        new Position(52.379790828551016, 5.3173828125)
                    })
                });

            multiPolygon = new MultiPolygon(new List<Polygon>
                {
                    new Polygon(new List<LineString>
                    {
                        new LineString(new List<IPosition>
                        {
                            new Position(52.959676831105995, -2.6797102391514338),
                            new Position(52.9608756693609, -2.6769029474483279),
                            new Position(52.908449372833715, -2.6079763270327119),
                            new Position(52.891287242948195, -2.5815104708998668),
                            new Position(52.875476700983896, -2.5851645010668989),
                            new Position(52.882954723868622, -2.6050779098387191),
                            new Position(52.875255907042678, -2.6373482332006359),
                            new Position(52.878791122091066, -2.6932445076063951),
                            new Position(52.89564268523565, -2.6931334629377890),
                            new Position(52.930592009390175, -2.6548779332193022),
                            new Position(52.959676831105995, -2.6797102391514338)
                        })
                    }),
                    new Polygon(new List<LineString>
                    {
                        new LineString(new List<IPosition>
                        {
                            new Position(52.89610842810761, -2.69628632041613),
                            new Position(52.8894641454077, -2.75901233808515),
                            new Position(52.89938894657412, -2.7663172788742449),
                            new Position(52.90253773227807, -2.804554822840895),
                            new Position(52.929801009654575, -2.83848602260174),
                            new Position(52.94013913205788, -2.838979264607087),
                            new Position(52.937353122653533, -2.7978187468478741),
                            new Position(52.920394929466184, -2.772273870352612),
                            new Position(52.926572918779222, -2.6996509024137052),
                            new Position(52.89610842810761, -2.69628632041613)
                        })
                    })
                });

            geomCollection = new GeometryCollection(new List<IGeometryObject>
                {
                    point,
                    multiPoint,
                    lineString,
                    multiLineString,
                    polygon,
                    multiPolygon
                });

            feature = new Feature.Feature(polygon, new Dictionary<string, object>() { { "Key", "Value" } }, "Id");

            featureCollection = new FeatureCollection(new List<Feature.Feature> {
                    feature, new Feature.Feature(multiPolygon, null)
            });

        }

        [TestMethod]
        [TestCategory("ToSqlGeometry")]
        public void ToSqlGeometryValidPointTest()
        {
            SqlGeometry sqlPoint = point.ToSqlGeometry();

            Assert.IsNotNull(point);
            Assert.IsNotNull(sqlPoint);
            Assert.AreEqual(sqlPoint.STGeometryType().Value, OpenGisGeometryType.Point.ToString());
            Assert.AreEqual(sqlPoint.STNumPoints().Value, 1);
        }

        [TestMethod]
        [TestCategory("ToSqlGeometry")]
        public void ToSqlGeometryValidMultiPointTest()
        {
            SqlGeometry sqlMultiPoint = multiPoint.ToSqlGeometry();

            Assert.IsNotNull(multiPoint);
            Assert.IsNotNull(sqlMultiPoint);
            Assert.AreEqual(sqlMultiPoint.STGeometryType().Value, OpenGisGeometryType.MultiPoint.ToString());
            Assert.AreEqual(sqlMultiPoint.STNumPoints().Value, multiPoint.Coordinates.Count);
        }

        [TestMethod]
        [TestCategory("ToSqlGeometry")]
        public void ToSqlGeometryValidLineStringTest()
        {
            SqlGeometry sqlLineString = lineString.ToSqlGeometry();

            Assert.IsNotNull(lineString);
            Assert.IsNotNull(sqlLineString);
            Assert.AreEqual(sqlLineString.STGeometryType().Value, OpenGisGeometryType.LineString.ToString());
            Assert.AreEqual(sqlLineString.STNumPoints().Value, lineString.Coordinates.Count);
        }

        [TestMethod]
        [TestCategory("ToSqlGeometry")]
        public void ToSqlGeometryValidMultiLineStringTest()
        {
            SqlGeometry sqlMultiLineString = multiLineString.ToSqlGeometry();

            Assert.IsNotNull(multiLineString);
            Assert.IsNotNull(sqlMultiLineString);
            Assert.AreEqual(sqlMultiLineString.STGeometryType().Value, OpenGisGeometryType.MultiLineString.ToString());
            Assert.AreEqual(sqlMultiLineString.STNumGeometries().Value, multiLineString.Coordinates.Count);
            Assert.AreEqual(sqlMultiLineString.STNumPoints().Value, multiLineString.Coordinates.SelectMany(ls => ls.Coordinates).Count());
        }

        [TestMethod]
        [TestCategory("ToSqlGeometry")]
        public void ToSqlGeometryValidPolygonTest()
        {
            SqlGeometry sqlPolygon = polygon.ToSqlGeometry();

            Assert.IsNotNull(polygon);
            Assert.IsNotNull(sqlPolygon);
            Assert.AreEqual(sqlPolygon.STGeometryType().Value, OpenGisGeometryType.Polygon.ToString());
            Assert.AreEqual(sqlPolygon.STNumInteriorRing().Value + 1, polygon.Coordinates.Count);
            Assert.AreEqual(sqlPolygon.STNumPoints().Value, polygon.Coordinates.SelectMany(ls => ls.Coordinates).Count());
        }

        [TestMethod]
        [TestCategory("ToSqlGeometry")]
        public void ToSqlGeometryValidPolygonWithHoleTest()
        {
            SqlGeometry sqlPolygon = polygonWithHole.ToSqlGeometry();

            Assert.IsNotNull(polygonWithHole);
            Assert.IsNotNull(sqlPolygon);
            Assert.AreEqual(sqlPolygon.STGeometryType().Value, OpenGisGeometryType.Polygon.ToString());
            Assert.AreEqual(sqlPolygon.STNumInteriorRing().Value + 1, polygonWithHole.Coordinates.Count);
            Assert.AreEqual(sqlPolygon.STNumPoints().Value, polygonWithHole.Coordinates.SelectMany(ls => ls.Coordinates).Count());
        }

        [TestMethod]
        [TestCategory("ToSqlGeometry")]
        public void ToSqlGeometryValidMultiPolygonTest()
        {
            SqlGeometry sqlMultiPolygon = multiPolygon.ToSqlGeometry();

            Assert.IsNotNull(multiPolygon);
            Assert.IsNotNull(sqlMultiPolygon);
            Assert.AreEqual(sqlMultiPolygon.STGeometryType().Value, OpenGisGeometryType.MultiPolygon.ToString());
            Assert.AreEqual(sqlMultiPolygon.STNumGeometries().Value, multiPolygon.Coordinates.Count);
            Assert.AreEqual(sqlMultiPolygon.Geometries().Sum(g => g.STNumInteriorRing().Value + 1), multiPolygon.Coordinates.SelectMany(p => p.Coordinates).Count());
            Assert.AreEqual(sqlMultiPolygon.STNumPoints().Value, multiPolygon.Coordinates.SelectMany(p => p.Coordinates).SelectMany(ls => ls.Coordinates).Count());
        }

        [TestMethod]
        [TestCategory("ToSqlGeometry")]
        public void ToSqlGeometryValidGeometryCollectionTest()
        {
            SqlGeometry sqlGeomCol = geomCollection.ToSqlGeometry();

            Assert.IsNotNull(geomCollection);
            Assert.IsNotNull(sqlGeomCol);
            Assert.AreEqual(sqlGeomCol.STGeometryType().Value, OpenGisGeometryType.GeometryCollection.ToString());
            Assert.AreEqual(sqlGeomCol.STNumGeometries().Value, geomCollection.Geometries.Count);
        }

        [TestMethod]
        [TestCategory("ToSqlGeometry")]
        public void ToSqlGeometryValidFeatureTest()
        {
            SqlGeometry sqlFeature = feature.ToSqlGeometry();

            Assert.IsNotNull(feature);
            Assert.IsNotNull(sqlFeature);
            Assert.AreEqual(sqlFeature.STGeometryType().Value, OpenGisGeometryType.Polygon.ToString());
            Assert.AreEqual(sqlFeature.STNumPoints().Value, polygon.Coordinates.SelectMany(ls => ls.Coordinates).Count());
        }

        [TestMethod]
        [TestCategory("ToSqlGeometry")]
        public void ToSqlGeometryValidFeatureCollectionTest()
        {
            List<SqlGeometry> sqlFeatureCol = featureCollection.ToSqlGeometry();

            Assert.IsNotNull(feature);
            Assert.IsNotNull(sqlFeatureCol);
            Assert.AreEqual(sqlFeatureCol.Count, featureCollection.Features.Count);
            Assert.AreEqual(sqlFeatureCol.Sum(g => g.STNumPoints().Value), polygon.Coordinates.SelectMany(ls => ls.Coordinates).Count() + multiPolygon.Coordinates.SelectMany(p => p.Coordinates).SelectMany(ls => ls.Coordinates).Count());
        }



    }
}
