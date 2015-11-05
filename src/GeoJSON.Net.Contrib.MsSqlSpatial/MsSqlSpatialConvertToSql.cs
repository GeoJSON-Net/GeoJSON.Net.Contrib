using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoJSON.Net.Geometry;
using GeoJSON.Net.Contrib.MsSqlSpatial.Sinks;
using Microsoft.SqlServer.Types;
using GeoJSON.Net.Feature;

namespace GeoJSON.Net.Contrib.MsSqlSpatial
{
	/// <summary>
	/// Partial class. Only methods from GeoJSON to Sql Server are here
	/// For Sql spatial types to GeoJSON, see MsSqlSpatialConvertToGeoJson.cs file
	/// </summary>
	public static partial class MsSqlSpatialConvert
	{
		public static SqlGeometry ToSqlGeometry(this Point point, int srid = 4326)
		{
			SqlGeometryBuilder gb = new SqlGeometryBuilder();
			gb.SetSrid(srid);

			Internal_FillGeometryBuilder(gb, point);

			return gb.ConstructedGeometry;
		}
		private static void Internal_FillGeometryBuilder(SqlGeometryBuilder gb, Point point)
		{
			gb.BeginGeometry(OpenGisGeometryType.Point);
			GeographicPosition pos = point.Coordinates as GeographicPosition;
			gb.BeginFigure(pos.Longitude, pos.Latitude);
			gb.EndFigure();
			gb.EndGeometry();
		}

		public static SqlGeometry ToSqlGeometry(this MultiPoint multiPoint, int srid = 4326)
		{
			SqlGeometryBuilder gb = new SqlGeometryBuilder();
			gb.SetSrid(srid);
			Internal_FillGeometryBuilder(gb, multiPoint);
			return gb.ConstructedGeometry;
		}
		private static void Internal_FillGeometryBuilder(SqlGeometryBuilder gb, MultiPoint multiPoint)
		{
			gb.BeginGeometry(OpenGisGeometryType.MultiPoint);
			List<Point> coords = multiPoint.Coordinates as List<Point>;
			foreach (var coord in coords)
			{
				GeographicPosition pos = coord.Coordinates as GeographicPosition;
				gb.BeginGeometry(OpenGisGeometryType.Point);
				gb.BeginFigure(pos.Longitude, pos.Latitude);
				gb.EndFigure();
				gb.EndGeometry();
			}
			gb.EndGeometry();
		}

		public static SqlGeometry ToSqlGeometry(this LineString lineString, int srid = 4326)
		{
			SqlGeometryBuilder gb = new SqlGeometryBuilder();
			gb.SetSrid(srid);
			Internal_FillGeometryBuilder(gb, lineString);
			return gb.ConstructedGeometry;
		}
		private static void Internal_FillGeometryBuilder(SqlGeometryBuilder gb, LineString lineString)
		{
			gb.BeginGeometry(OpenGisGeometryType.LineString);
			bool beginFigureCalled = false;
			foreach (var ipos in lineString.Coordinates)
			{
				GeographicPosition pos = ipos as GeographicPosition;
				if (!beginFigureCalled)
				{
					gb.BeginFigure(pos.Longitude, pos.Latitude);
					beginFigureCalled = true;

				}
				else
				{
					gb.AddLine(pos.Longitude, pos.Latitude);
				}

			}
			gb.EndFigure();
			gb.EndGeometry();
		}

		public static SqlGeometry ToSqlGeometry(this MultiLineString multiLineString, int srid = 4326)
		{
			SqlGeometryBuilder gb = new SqlGeometryBuilder();
			gb.SetSrid(srid);
			Internal_FillGeometryBuilder(gb, multiLineString);
			return gb.ConstructedGeometry;
		}
		private static void Internal_FillGeometryBuilder(SqlGeometryBuilder gb, MultiLineString multiLineString)
		{
			gb.BeginGeometry(OpenGisGeometryType.MultiLineString);
			foreach (var lineString in multiLineString.Coordinates)
			{
				gb.BeginGeometry(OpenGisGeometryType.LineString);
				bool beginFigureCalled = false;
				foreach (var ipos in lineString.Coordinates)
				{
					GeographicPosition pos = ipos as GeographicPosition;
					if (!beginFigureCalled)
					{
						gb.BeginFigure(pos.Longitude, pos.Latitude);
						beginFigureCalled = true;
					}
					else
					{
						gb.AddLine(pos.Longitude, pos.Latitude);
					}
				}
				gb.EndFigure();
				gb.EndGeometry();
			}
			gb.EndGeometry();
		}

		public static SqlGeometry ToSqlGeometry(this Polygon polygon, int srid = 4326)
		{
			SqlGeometryBuilder gb = new SqlGeometryBuilder();
			gb.SetSrid(srid);
			Internal_FillGeometryBuilder(gb, polygon);
			return gb.ConstructedGeometry;
		}
		private static void Internal_FillGeometryBuilder(SqlGeometryBuilder gb, Polygon polygon)
		{
			gb.BeginGeometry(OpenGisGeometryType.Polygon);
			foreach (var lineString in polygon.Coordinates)
			{
				bool beginFigureCalled = false;
				foreach (var ipos in lineString.Coordinates)
				{
					GeographicPosition pos = ipos as GeographicPosition;
					if (!beginFigureCalled)
					{
						gb.BeginFigure(pos.Longitude, pos.Latitude);
						beginFigureCalled = true;
					}
					else
					{
						gb.AddLine(pos.Longitude, pos.Latitude);
					}
				}
				gb.EndFigure();
			}
			gb.EndGeometry();
		}

		public static SqlGeometry ToSqlGeometry(this MultiPolygon multiPolygon, int srid = 4326)
		{
			SqlGeometryBuilder gb = new SqlGeometryBuilder();
			gb.SetSrid(srid);
			gb.BeginGeometry(OpenGisGeometryType.MultiPolygon);
			foreach (var polygon in multiPolygon.Coordinates)
			{
				Internal_FillGeometryBuilder(gb, polygon);
			}
			gb.EndGeometry();
			return gb.ConstructedGeometry;
		}
		private static void Internal_FillGeometryBuilder(SqlGeometryBuilder gb, MultiPolygon multiPolygon)
		{
			gb.BeginGeometry(OpenGisGeometryType.MultiPolygon);
			foreach (var polygon in multiPolygon.Coordinates)
			{
				Internal_FillGeometryBuilder(gb, polygon);
			}
			gb.EndGeometry();
		}

		public static SqlGeometry ToSqlGeometry(this GeometryCollection geometryCollection, int srid = 4326)
		{
			SqlGeometryBuilder gb = new SqlGeometryBuilder();
			gb.SetSrid(srid);
			gb.BeginGeometry(OpenGisGeometryType.GeometryCollection);
			foreach (var geom in geometryCollection.Geometries)
			{
				switch(geom.Type)
				{
					case GeoJSONObjectType.LineString:
						Internal_FillGeometryBuilder(gb, geom as LineString);
						break;
					case GeoJSONObjectType.MultiLineString:
						Internal_FillGeometryBuilder(gb, geom as MultiLineString);
						break;
					case GeoJSONObjectType.Point:
						Internal_FillGeometryBuilder(gb, geom as Point);
						break;
					case GeoJSONObjectType.MultiPoint:
						Internal_FillGeometryBuilder(gb, geom as MultiPoint);
						break;
					case GeoJSONObjectType.Polygon:
						Internal_FillGeometryBuilder(gb, geom as Polygon);
						break;
					case GeoJSONObjectType.MultiPolygon:
						Internal_FillGeometryBuilder(gb, geom as MultiPolygon);
						break;
					default:
						throw new NotSupportedException("Geometry conversion is not supported for " + geom.Type.ToString());
				}
			}
			gb.EndGeometry();
			return gb.ConstructedGeometry;
		}

		public static SqlGeometry ToSqlGeometry(this Feature.Feature feature, int srid = 4326)
		{
			switch (feature.Geometry.Type)
			{
				case GeoJSONObjectType.LineString:
					return ((LineString)feature.Geometry).ToSqlGeometry(srid);

				case GeoJSONObjectType.MultiLineString:
					return ((MultiLineString)feature.Geometry).ToSqlGeometry(srid);

				case GeoJSONObjectType.Point:
					return ((Point)feature.Geometry).ToSqlGeometry(srid);

				case GeoJSONObjectType.MultiPoint:
					return ((MultiPoint)feature.Geometry).ToSqlGeometry(srid);

				case GeoJSONObjectType.Polygon:
					return ((Polygon)feature.Geometry).ToSqlGeometry(srid);

				case GeoJSONObjectType.MultiPolygon:
					return ((MultiPolygon)feature.Geometry).ToSqlGeometry(srid);

				default:
					throw new NotSupportedException("Geometry conversion is not supported for " + feature.Type.ToString());
			}
		}

		public static List<SqlGeometry> ToSqlGeometry(this FeatureCollection featureCollection, int srid = 4326)
		{
			List<SqlGeometry> retList = new List<SqlGeometry>();
			foreach(var feature in featureCollection.Features)
			{
				var sqlGeom = feature.ToSqlGeometry(srid);
				if (sqlGeom != null)
					retList.Add(sqlGeom);
			}
			return retList;
		}

		//public static SqlGeometry ToSqlGeometry<T>(T geoJsonObject, int srid = 0) where T : GeoJSONObject
		//{
		//	throw new NotImplementedException();
		//}


		//public static SqlGeography ToSqlGeography(IGeometryObject geoJsonGeometry)
		//{
		//	throw new NotImplementedException();
		//	//switch(geoJsonGeometry.Type)
		//	//{
		//	//	case GeoJSONObjectType.
		//	//}
		//}
	}
}
