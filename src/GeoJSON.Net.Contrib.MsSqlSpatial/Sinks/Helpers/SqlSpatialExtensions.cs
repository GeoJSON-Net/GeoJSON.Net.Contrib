using System;
using System.Collections.Generic;
using Microsoft.SqlServer.Types;

namespace GeoJSON.Net.Contrib.MsSqlSpatial
{
    public static class SqlSpatialExtensions
    {
        // Sql geometry extensions

        /// <summary>
        /// Computes bounding box of a geometry instance
        /// </summary>
        /// <param name="geom"></param>
        /// <returns>Array of doubles in this order: xmin, ymin, xmax, ymax</returns>
        public static double[] BoundingBox(this SqlGeometry geom)
        {
            double xmin = Double.MaxValue, ymin = Double.MaxValue, xmax = Double.MinValue, ymax = double.MinValue;
            foreach (SqlGeometry point in geom.Points())
            {
                xmin = Math.Min(point.STX.Value, xmin);
                ymin = Math.Min(point.STY.Value, ymin);
                xmax = Math.Max(point.STX.Value, xmax);
                ymax = Math.Max(point.STY.Value, ymax);
            }
            return new double[] { xmin, ymin, xmax, ymax };
        }

        /// <summary>
        /// Easier to use geometry enumerator than STGeometryN()
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public static IEnumerable<SqlGeometry> Geometries(this SqlGeometry geom)
        {
            for (int i = 1; i <= geom.STNumGeometries().Value; i++)
            {
                yield return geom.STGeometryN(i);
            }
        }

        /// <summary>
        /// Easier to use points enumerator on SqlGeometry
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public static IEnumerable<SqlGeometry> Points(this SqlGeometry geom)
        {
            for (int i = 1; i <= geom.STNumPoints().Value; i++)
            {
                yield return geom.STPointN(i);
            }
        }

        /// <summary>
        /// Easier to use interior geometry enumerator on SqlGeometry polygons
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public static IEnumerable<SqlGeometry> InteriorRings(this SqlGeometry geom)
        {
            for (int i = 1; i <= geom.STNumInteriorRing().Value; i++)
            {
                yield return geom.STInteriorRingN(i);
            }
        }

        /// <summary>
        /// Easier to use MakeValid() that validates only if required
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public static SqlGeometry MakeValidIfInvalid(this SqlGeometry geom)
        {
            if (geom == null || geom.IsNull)
            {
                return geom;
            }

            // Make valid if necessary
            if (geom.STIsValid().IsFalse)
            {
                return geom.MakeValid();
            }

            return geom;
        }

        // Sql geography extensions
        /// <summary>
        /// Computes bounding box of a geography instance
        /// </summary>
        /// <param name="geom"></param>
        /// <returns>Array of doubles in this order: xmin, ymin, xmax, ymax</returns>
        public static double[] BoundingBox(this SqlGeography geom)
        {
            double xmin = 180, ymin = 90, xmax = -180, ymax = -90;
            foreach (SqlGeography point in geom.Points())
            {
                xmin = Math.Min(point.Long.Value, xmin);
                ymin = Math.Min(point.Lat.Value, ymin);
                xmax = Math.Max(point.Long.Value, xmax);
                ymax = Math.Max(point.Lat.Value, ymax);
            }
            return new double[] { xmin, ymin, xmax, ymax };
        }

        /// <summary>
        /// Easier to use geometry enumerator than STGeometryN()
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public static IEnumerable<SqlGeography> Geometries(this SqlGeography geom)
        {
            for (int i = 1; i <= geom.STNumGeometries().Value; i++)
            {
                yield return geom.STGeometryN(i);
            }
        }

        /// <summary>
        /// Easier to use points enumerator on SqlGeography
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public static IEnumerable<SqlGeography> Points(this SqlGeography geom)
        {
            for (int i = 1; i <= geom.STNumPoints().Value; i++)
            {
                yield return geom.STPointN(i);
            }
        }

        /// <summary>
        /// Easier to use MakeValid() that validates only if required
        /// </summary>
        /// <param name="geom"></param>
        /// <returns></returns>
        public static SqlGeography MakeValidIfInvalid(this SqlGeography geom)
        {
            if (geom == null || geom.IsNull)
            {
                return geom;
            }

            // Make valid if necessary
            if (geom.STIsValid().IsFalse)
            {
                return geom.MakeValid();
            }

            return geom;
        }
    }
}
