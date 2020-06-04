using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GeoJSON.Net.Geometry;

namespace GeoJSON.Net.Contrib.Wkb.Conversions
{
    /// <summary>
    /// Wkb to GeoJson encoder.
    /// Built from https://github.com/gksource/GeoJSON.Net.Contrib.EF
    /// </summary>
    public static class WkbEncode
    {
        //private static byte _wKBXDR = 0x00;       // Big Endian
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
            var type = (int)WkbGeometryType.Point;
            var hasAltitude = HasAltitude(v_Point);
            if (hasAltitude)
            {
                type += 1000;
            }


            binaryWriter.Write(_wKBNDR);
            binaryWriter.Write(type);
            var position = v_Point.Coordinates as Position;
            binaryWriter.Write(position.Longitude);
            binaryWriter.Write(position.Latitude);
            if (hasAltitude)
            {
                binaryWriter.Write((double)position.Altitude);
            }
        }

        private static void MultiPoint(BinaryWriter binaryWriter, IGeometryObject geometryObject)
        {
            var multiPoint = geometryObject as MultiPoint;
            var type = (int)WkbGeometryType.MultiPoint;
            var hasAltitude = HasAltitude(multiPoint);
            if (hasAltitude)
            {
                type += 1000;
            }

            binaryWriter.Write(_wKBNDR);
            binaryWriter.Write(type);
            binaryWriter.Write((int)multiPoint.Coordinates.Count);

            foreach(Point point in multiPoint.Coordinates)
            {
                Point(binaryWriter, point);
            }
        }

        private static void Pointold(BinaryWriter binaryWriter, Position position)
        {
            binaryWriter.Write(_wKBNDR);
            binaryWriter.Write((int)WkbGeometryType.Point);
            binaryWriter.Write(position.Longitude);
            binaryWriter.Write(position.Latitude);
        }

        private static void Points(BinaryWriter binaryWriter, List<IPosition> positions, bool hasAltitude)
        {
            foreach (IPosition v_Point in positions)
            {
                var position = v_Point as Position;
                binaryWriter.Write(position.Longitude);
                binaryWriter.Write(position.Latitude);
                if (hasAltitude)
                {
                    binaryWriter.Write((double)position.Altitude);
                }
            }
        }

        private static void Polyline(BinaryWriter binaryWriter, IGeometryObject geometryObject)
        {
            var v_Polyline = geometryObject as LineString;
            var type = (int)WkbGeometryType.LineString;
            var hasAltitude = HasAltitude(v_Polyline);
            if (hasAltitude)
            {
                type += 1000;
            }

            binaryWriter.Write(_wKBNDR);
            binaryWriter.Write(type);
            binaryWriter.Write((int)v_Polyline.Coordinates.Count);

            Points(binaryWriter, v_Polyline.Coordinates.ToList(), hasAltitude);
        }

        private static void MultiPolyline(BinaryWriter binaryWriter, IGeometryObject GeometryObject)
        {
            var multiLineString = GeometryObject as MultiLineString;
            var type = (int)WkbGeometryType.MultiLineString;
            var hasAltitude = HasAltitude(multiLineString);
            if (hasAltitude)
            {
                type += 1000;
            }

            binaryWriter.Write(_wKBNDR);
            binaryWriter.Write(type);
            binaryWriter.Write((int)multiLineString.Coordinates.Count);

            foreach (LineString lineString in multiLineString.Coordinates)
            {
                Polyline(binaryWriter, lineString);
            }
        }

        private static void Polygon(BinaryWriter binaryWriter, IGeometryObject geometryObject)
        {
            var polygon = geometryObject as Polygon;
            var type = (int)WkbGeometryType.Polygon;
            var hasAltitude = HasAltitude(polygon);
            if (hasAltitude)
            {
                type += 1000;
            }

            binaryWriter.Write(_wKBNDR);
            binaryWriter.Write(type);

            var numberOfRings = polygon.Coordinates.Count;
            binaryWriter.Write(numberOfRings);

            foreach (LineString ring in polygon.Coordinates)
            {
                binaryWriter.Write((int)ring.Coordinates.Count);
                Points(binaryWriter, ring.Coordinates.ToList(), hasAltitude);
            }
        }

        private static void MultiPolygon(BinaryWriter binaryWriter, IGeometryObject geometryObject)
        {
            var multiPolygon = geometryObject as MultiPolygon;
            var type = (int)WkbGeometryType.MultiPolygon;
            var hasAltitude = HasAltitude(multiPolygon);
            if (hasAltitude)
            {
                type += 1000;
            }

            binaryWriter.Write(_wKBNDR);
            binaryWriter.Write(type);
            binaryWriter.Write((int)multiPolygon.Coordinates.Count);

            foreach(Polygon polygon in multiPolygon.Coordinates)
            {
                Polygon(binaryWriter, polygon);
            }
        }

        private static void GeometryCollection(BinaryWriter binaryWriter, IGeometryObject geometryObject)
        {
            var geometryCollection = geometryObject as GeometryCollection;
            var type = (int)WkbGeometryType.GeometryCollection;
            var hasAltitude = HasAltitude(geometryCollection);
            if (hasAltitude)
            {
                type += 1000;
            }

            binaryWriter.Write(_wKBNDR);
            binaryWriter.Write(type);
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

        private static bool HasAltitude(Point point)
        {
            return point.Coordinates.Altitude != null;
        }

        private static bool HasAltitude(MultiPoint multiPoint)
        {
            return multiPoint.Coordinates.FirstOrDefault()?.Coordinates.Altitude != null;
        }

        private static bool HasAltitude(Polygon polygon)
        {
            return polygon.Coordinates.FirstOrDefault()?.Coordinates.FirstOrDefault()?.Altitude != null;
        }

        private static bool HasAltitude(MultiPolygon multiPolygon)
        {
            return multiPolygon.Coordinates.FirstOrDefault()?.Coordinates.FirstOrDefault()?.Coordinates.FirstOrDefault()?.Altitude != null;
        }

        private static bool HasAltitude(LineString lineString)
        {
            return lineString.Coordinates.FirstOrDefault()?.Altitude != null;
        }

        private static bool HasAltitude(MultiLineString multiLineString)
        {
            return multiLineString.Coordinates.FirstOrDefault()?.Coordinates.FirstOrDefault()?.Altitude != null;
        }

        private static bool HasAltitude(GeometryCollection geometryCollection)
        {
            if (geometryCollection.Geometries.Count == 0)
            {
                return false;
            }

            var firstGeometry = geometryCollection.Geometries.First();
            switch (firstGeometry.Type)
            {
                case GeoJSONObjectType.Point:
                    return HasAltitude(firstGeometry as Point);

                case GeoJSONObjectType.MultiPoint:
                    return HasAltitude(firstGeometry as MultiPoint);

                case GeoJSONObjectType.Polygon:
                    return HasAltitude(firstGeometry as Polygon);

                case GeoJSONObjectType.MultiPolygon:
                    return HasAltitude(firstGeometry as MultiPolygon);

                case GeoJSONObjectType.LineString:
                    return HasAltitude(firstGeometry as LineString);

                case GeoJSONObjectType.MultiLineString:
                    return HasAltitude(firstGeometry as MultiLineString);

                default:
                    throw new Exception("Unsupported type");
            }
        }
    }
}
