using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GeoJSON.Net.Geometry;

namespace GeoJSON.Net.Contrib.EntityFramework.WkbConversions
{
    /// <summary>
    /// Wkb to GeoJson encoder.
    /// Built from https://github.com/gksource/GeoJSON.Net.Contrib.EF
    /// </summary>
    public static class WkbEncode
    {
        private static byte _wKBXDR = 0x00;       // Big Endian
        private static byte _wKBNDR = 0x01;       // Little Endian

        public static byte[] Encode(IGeometryObject geometryObject)
        {
            using (var v_ms = new MemoryStream())
            {
                using (var v_bw = new BinaryWriter(v_ms))
                {
                    Encode(v_bw, geometryObject);

                    var v_length = (int)v_ms.Length;
                    v_bw.Close();
                    var v_buffer = v_ms.GetBuffer();
                    Array.Resize(ref v_buffer, v_length);
                    v_ms.Close();
                    return v_buffer;
                }
            }
        }

        public static byte[] Encode(Feature.Feature feature)
        {
            return Encode(feature.Geometry);
        }

        private static void Point(BinaryWriter binaryWriter, IGeometryObject geometryObject)
        {
            var v_Point = geometryObject as Point;

            binaryWriter.Write(_wKBNDR);
            binaryWriter.Write((int)WkbGeometryType.WkbPoint);
            var position = v_Point.Coordinates as Position;
            binaryWriter.Write(position.Longitude);
            binaryWriter.Write(position.Latitude);
        }

        private static void MultiPoint(BinaryWriter binaryWriter, IGeometryObject geometryObject)
        {
            var multiPoint = geometryObject as MultiPoint;

            binaryWriter.Write(_wKBNDR);
            binaryWriter.Write((int)WkbGeometryType.WkbMultiPoint);
            binaryWriter.Write((int)multiPoint.Coordinates.Count);

            foreach(Point point in multiPoint.Coordinates)
            {
                Point(binaryWriter, point);
            }
        }

        private static void Pointold(BinaryWriter binaryWriter, Position position)
        {
            binaryWriter.Write(_wKBNDR);
            binaryWriter.Write((int)WkbGeometryType.WkbPoint);
            binaryWriter.Write(position.Longitude);
            binaryWriter.Write(position.Latitude);
        }

        private static void Points(BinaryWriter binaryWriter, List<IPosition> positions)
        {
            foreach (IPosition v_Point in positions)
            {
                var position = v_Point as Position;
                binaryWriter.Write(position.Longitude);
                binaryWriter.Write(position.Latitude);
            }
        }

        private static void Polyline(BinaryWriter binaryWriter, IGeometryObject geometryObject)
        {
            var v_Polyline = geometryObject as LineString;

            binaryWriter.Write(_wKBNDR);
            binaryWriter.Write((int)WkbGeometryType.WkbLineString);
            binaryWriter.Write((int)v_Polyline.Coordinates.Count);

            Points(binaryWriter, v_Polyline.Coordinates.ToList());
        }

        private static void MultiPolyline(BinaryWriter binaryWriter, IGeometryObject GeometryObject)
        {
            var multiLineString = GeometryObject as MultiLineString;

            binaryWriter.Write(_wKBNDR);
            binaryWriter.Write((int)WkbGeometryType.WkbMultiLineString);
            binaryWriter.Write((int)multiLineString.Coordinates.Count);

            foreach (LineString lineString in multiLineString.Coordinates)
            {
                Polyline(binaryWriter, lineString);
            }
        }

        private static void Polygon(BinaryWriter binaryWriter, IGeometryObject geometryObject)
        {
            var polygon = geometryObject as Polygon;

            binaryWriter.Write(_wKBNDR);
            binaryWriter.Write((int)WkbGeometryType.WkbPolygon);

            var numberOfRings = polygon.Coordinates.Count;
            binaryWriter.Write(numberOfRings);

            foreach (LineString ring in polygon.Coordinates)
            {
                binaryWriter.Write((int)ring.Coordinates.Count);
                Points(binaryWriter, ring.Coordinates.ToList());
            }
        }

        private static void MultiPolygon(BinaryWriter binaryWriter, IGeometryObject geometryObject)
        {
            var multiPolygon = geometryObject as MultiPolygon;

            binaryWriter.Write(_wKBNDR);
            binaryWriter.Write((int)WkbGeometryType.WkbMultiPolygon);
            binaryWriter.Write((int)multiPolygon.Coordinates.Count);

            foreach(Polygon polygon in multiPolygon.Coordinates)
            {
                Polygon(binaryWriter, polygon);
            }
        }

        private static void GeometryCollection(BinaryWriter binaryWriter, IGeometryObject geometryObject)
        {
            var geometryCollection = geometryObject as GeometryCollection;

            binaryWriter.Write(_wKBNDR);
            binaryWriter.Write((Int32)WkbGeometryType.WkbGeometryCollection);
            binaryWriter.Write((Int32)geometryCollection.Geometries.Count);

            foreach(IGeometryObject geometry in geometryCollection.Geometries)
            {
                Encode(binaryWriter, geometry);
            }
        }        

        private static void Encode(BinaryWriter binaryWriter, IGeometryObject geometryObject)
        {
            switch (geometryObject.Type)
            {
                case GeoJSONObjectType.Point:
                    Point(binaryWriter, geometryObject);
                    break;

                case GeoJSONObjectType.MultiPoint:
                    MultiPoint(binaryWriter, geometryObject);
                    break;

                case GeoJSONObjectType.Polygon:
                    Polygon(binaryWriter, geometryObject);
                    break;

                case GeoJSONObjectType.MultiPolygon:
                    MultiPolygon(binaryWriter, geometryObject);
                    break;

                case GeoJSONObjectType.LineString:
                    Polyline(binaryWriter, geometryObject);
                    break;

                case GeoJSONObjectType.MultiLineString:
                    MultiPolyline(binaryWriter, geometryObject);
                    break;

                case GeoJSONObjectType.GeometryCollection:
                    GeometryCollection(binaryWriter, geometryObject);
                    break;
            }
        }        
    }
}
