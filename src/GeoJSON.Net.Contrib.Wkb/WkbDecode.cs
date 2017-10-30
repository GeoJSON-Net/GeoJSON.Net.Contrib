using System;
using System.Collections.Generic;
using GeoJSON.Net.Geometry;

namespace GeoJSON.Net.Contrib.Wkb.Conversions
{
    /// <summary>
    /// Wkb to GeoJson decoder.
    /// Built from https://github.com/gksource/GeoJSON.Net.Contrib.EF
    /// </summary>
    public static class WkbDecode
    {
        //private static byte _WKBXDR = 0x00; // Big Endian
        private static byte _wKBNDR = 0x01; // Little Endian

        public static IGeometryObject Decode(byte[] wkb)
        {
            var v_pos = 0;
            return ParseShape(wkb, ref v_pos);
        }        

        private static Point ParsePoint(byte[] wkb, ref int wkbPosition)
        {
            var wkbGeometryType = GetType(wkb, ref wkbPosition);

            CheckType(WkbGeometryType.Point, wkbGeometryType);

            Position geographicalPosition = GetGeographicPosition(wkb, ref wkbPosition);

            return new Point(geographicalPosition);
        }

        private static LineString ParseLineString(byte[] wkb, ref int wkbPosition)
        {
            var wkbGeometryType = GetType(wkb, ref wkbPosition);

            CheckType(WkbGeometryType.LineString, wkbGeometryType);

            Position[] positions = ParsePositions(wkb, ref wkbPosition);

            return new LineString(positions);
        }        

        private static Polygon ParsePolygon(byte[] wkb, ref int wkbPosition)
        {
            var wkbGeometryType = GetType(wkb, ref wkbPosition);

            CheckType(WkbGeometryType.Polygon, wkbGeometryType);

            var numberOfLines = GetUInt32(wkb, ref wkbPosition);
            var lines = new List<LineString>();

            for (var v_ls = 0; v_ls < numberOfLines; ++v_ls)
            {
                Position[] positions = ParsePositions(wkb, ref wkbPosition);

                lines.Add(new LineString(positions));
            }

            return new Polygon(lines);
        }

        private static MultiPoint ParseMultiPoint(byte[] wkb, ref int wkbPosition)
        {
            var wkbGeometryType = GetType(wkb, ref wkbPosition);

            CheckType(WkbGeometryType.MultiPoint, wkbGeometryType);

            var numberOfPoints = GetUInt32(wkb, ref wkbPosition);
            var points = new List<Point>();

            for (var i = 0; i < numberOfPoints; ++i)
            {
                points.Add(ParsePoint(wkb, ref wkbPosition));
            }

            return new MultiPoint(points);
        }

        private static MultiLineString ParseMultiLineString(byte[] wkb, ref int wkbPosition)
        {
            var wkbGeometryType = GetType(wkb, ref wkbPosition);

            CheckType(WkbGeometryType.MultiLineString, wkbGeometryType);

            var numberOfLines = GetUInt32(wkb, ref wkbPosition);
            var lines = new List<LineString>();

            for (var i = 0; i < numberOfLines; ++i)
            {
                lines.Add(ParseLineString(wkb, ref wkbPosition));
            }

            return new MultiLineString(lines);
        }

        private static MultiPolygon ParseMultiPolygon(byte[] wkb, ref int wkbPosition)
        {
            var wkbGeometryType = GetType(wkb, ref wkbPosition);

            CheckType(WkbGeometryType.MultiPolygon, wkbGeometryType);

            var numberOfPolygons = GetUInt32(wkb, ref wkbPosition);
            var polygons = new List<Polygon>();

            for (var i = 0; i < numberOfPolygons; ++i)
            {
                polygons.Add(ParsePolygon(wkb, ref wkbPosition));
            }

            return new MultiPolygon(polygons);
        }

        private static GeometryCollection ParseGeometryCollection(byte[] wkb, ref int wkbPosition)
        {
            var wkbGeometryType = GetType(wkb, ref wkbPosition);

            CheckType(WkbGeometryType.GeometryCollection, wkbGeometryType);

            var numberOfShapes = GetUInt32(wkb, ref wkbPosition);
            var geometries = new List<IGeometryObject>();

            for (var i = 0; i < numberOfShapes; ++i)
            {
                geometries.Add(ParseShape(wkb, ref wkbPosition));
            }

            return new GeometryCollection(geometries);
        }

        private static IGeometryObject ParseShape(byte[] wkb, ref int wkbPosition)
        {
            var v_type = BitConverter.ToUInt32(wkb, wkbPosition + 1);

            switch (v_type)
            {
                case (uint)WkbGeometryType.Point:
                    return ParsePoint(wkb, ref wkbPosition);

                case (uint)WkbGeometryType.LineString:
                    return ParseLineString(wkb, ref wkbPosition);

                case (uint)WkbGeometryType.Polygon:
                    return ParsePolygon(wkb, ref wkbPosition);

                case (uint)WkbGeometryType.MultiPoint:
                    return ParseMultiPoint(wkb, ref wkbPosition);

                case (uint)WkbGeometryType.MultiLineString:
                    return ParseMultiLineString(wkb, ref wkbPosition);

                case (uint)WkbGeometryType.MultiPolygon:
                    return ParseMultiPolygon(wkb, ref wkbPosition);

                case (uint)WkbGeometryType.GeometryCollection:
                    return ParseGeometryCollection(wkb, ref wkbPosition);

                default:
                    throw new Exception("Unsupported type");
            }
        }

        private static uint GetUInt32(byte[] wkb, ref int wkbPosition)
        {
            var intConversion = BitConverter.ToUInt32(wkb, wkbPosition);
            wkbPosition += 4;
            return intConversion;
        }

        private static double GetDouble(byte[] wkb, ref int wkbPosition)
        {
            var doubleConversion = BitConverter.ToDouble(wkb, wkbPosition);
            wkbPosition += 8;
            return doubleConversion;
        }

        private static Position[] ParsePositions(byte[] wkb, ref int wkbPosition)
        {
            var numberOfPoints = GetUInt32(wkb, ref wkbPosition);
            Position[] positions = new Position[numberOfPoints];

            for (var i = 0; i < numberOfPoints; ++i)
            {
                positions[i] = GetGeographicPosition(wkb, ref wkbPosition);
            }

            return positions;
        }

        private static Position GetGeographicPosition(byte[] wkb, ref int wkbPosition)
        {
            var longitud = GetDouble(wkb, ref wkbPosition);
            var latitud = GetDouble(wkb, ref wkbPosition);
            return new Position(latitud, longitud);
        }

        private static uint GetType(byte[] wkb, ref int wkbPosition)
        {
            if (wkb[wkbPosition] != _wKBNDR)
            {
                throw new Exception("Only Little Endian format supported");
            }

            wkbPosition += 1;

            return GetUInt32(wkb, ref wkbPosition);
        }

        private static void CheckType(WkbGeometryType expected, uint actual)
        {
            if (actual != (uint)expected)
            {
                throw new ArgumentException($"Invalid wkb geometry type, expected {expected}, actual {actual}");
            }
        }
    }

}
