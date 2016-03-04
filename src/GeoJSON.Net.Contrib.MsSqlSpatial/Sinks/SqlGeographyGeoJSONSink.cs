using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoJSON.Net.Geometry;
using Microsoft.SqlServer.Types;

namespace GeoJSON.Net.Contrib.MsSqlSpatial.Sinks
{
	/// <summary>
	/// Sink converting a SqlGeography to a GeoJSON geometry
	/// Usage : <code>SqlGeographyGeoJsonSink sink = new SqlGeographyGeoJsonSink();
	///	sqlgeography.Populate(sink);
	///	// sink.BoundingBox returns a GeoJSON compliant double[] bbox 
	///	return sink.ConstructedGeography; // returns an IGeometryObject
	///	</code> 
	/// </summary>
	internal class SqlGeographyGeoJsonSink : IGeographySink110
	{
		SinkGeometryCollection<OpenGisGeographyType> _geomCollection;
		bool _isGeometryCollection = false;
		int _nestLevel = 0;
		SinkGeometry<OpenGisGeographyType> _currentGeometry;
		SinkLineRing _currentRing;
		double _lonMin = 180;
		double _latMin = 90;
		double _lonMax = -180;
		double _latMax = -90;

		#region Sink implementation

		public void BeginGeography(OpenGisGeographyType type)
		{
			if (_geomCollection == null)
			{
				_geomCollection = new SinkGeometryCollection<OpenGisGeographyType>(type);
				if (type == OpenGisGeographyType.GeometryCollection)
				{
					_isGeometryCollection = true;
				}
			}

			_currentGeometry = new SinkGeometry<OpenGisGeographyType>(type);
			if (_isGeometryCollection && _nestLevel > 0)
			{
				if (_nestLevel == 1)
				{
					_geomCollection.SubItems.Add(new SinkGeometryCollection<OpenGisGeographyType>(type));
				}
				_geomCollection.SubItems.Last().Add(_currentGeometry);
			}
			else
			{
				_geomCollection.Add(_currentGeometry);
			}

			_nestLevel++;
		}

		public void BeginFigure(double lat, double lon, double? z, double? m)
		{
			_currentRing = new SinkLineRing();
			_currentRing.Add(new GeographicPosition(latitude: lat
																							, longitude: lon
																							, altitude: z));

			UpdateBoundingBox(lon, lat);
		}

		public void AddLine(double lat, double lon, double? z, double? m)
		{
			_currentRing.Add(new GeographicPosition(latitude: lat
																							, longitude: lon
																							, altitude: z));

			UpdateBoundingBox(lon, lat);
		}

		public void EndFigure()
		{
			if (_currentRing == null)
				return;

			_currentGeometry.Add(_currentRing);
			_currentRing = null;
		}

		public void EndGeography()
		{
			_nestLevel--;
			_currentGeometry = null;
		}

		public void SetSrid(int srid)
		{

		}

		// Not implemented
		// This one is tough ! Implementation should use SqlGeometry.CurveToLineWithTolerance
		public void AddCircularArc(double x1, double y1, double? z1, double? m1, double x2, double y2, double? z2, double? m2)
		{
			throw new NotImplementedException();
		}

		private void UpdateBoundingBox(double lon, double lat)
		{
			_lonMin = Math.Min(lon, _lonMin);
			_latMin = Math.Min(lat, _latMin);
			_lonMax = Math.Max(lon, _lonMax);
			_latMax = Math.Max(lat, _latMax);
		}

		#endregion

		public IGeometryObject ConstructedGeography
		{
			get
			{
				IGeometryObject _geometry = null;

				switch (_geomCollection.GeometryType)
				{
					case OpenGisGeographyType.Point:
					case OpenGisGeographyType.MultiPoint:
					case OpenGisGeographyType.LineString:
					case OpenGisGeographyType.MultiLineString:
					case OpenGisGeographyType.Polygon:
					case OpenGisGeographyType.MultiPolygon:

						_geometry = GeometryFromSinkGeometryCollection(_geomCollection);

						break;
					case OpenGisGeographyType.GeometryCollection:

						List<IGeometryObject> subGeometries = _geomCollection.SubItems.Select(subItem => GeometryFromSinkGeometryCollection(subItem)).ToList();
						_geometry = new GeometryCollection(subGeometries);

						((GeometryCollection)_geometry).BoundingBoxes = this.BoundingBox;
						break;
					default:
						throw new NotSupportedException("Geometry type " + _geomCollection.GeometryType.ToString() + " is not supported yet.");
				}

				return _geometry;
			}
		}

		private IGeometryObject GeometryFromSinkGeometryCollection(SinkGeometryCollection<OpenGisGeographyType> sinkCollection)
		{

			IGeometryObject _geometry = null;

			switch (sinkCollection.GeometryType)
			{
				case OpenGisGeographyType.Point:
					_geometry = ConstructGeometryPart(sinkCollection[0]);
					((Point)_geometry).BoundingBoxes = this.BoundingBox;
					break;
				case OpenGisGeographyType.MultiPoint:
					_geometry = new MultiPoint(sinkCollection.Skip(1)
																										.Select(g => (Point)ConstructGeometryPart(g))
																										.ToList());
					((MultiPoint)_geometry).BoundingBoxes = this.BoundingBox;
					break;
				case OpenGisGeographyType.LineString:
					_geometry = ConstructGeometryPart(sinkCollection[0]);
					((LineString)_geometry).BoundingBoxes = this.BoundingBox;
					break;
				case OpenGisGeographyType.MultiLineString:
					_geometry = new MultiLineString(sinkCollection.Skip(1)
																												.Select(g => (LineString)ConstructGeometryPart(g))
																												.ToList());
					((MultiLineString)_geometry).BoundingBoxes = this.BoundingBox;
					break;
				case OpenGisGeographyType.Polygon:
					_geometry = ConstructGeometryPart(sinkCollection.First());
					((Polygon)_geometry).BoundingBoxes = this.BoundingBox;
					break;
				case OpenGisGeographyType.MultiPolygon:
					_geometry = new MultiPolygon(sinkCollection.Skip(1)
																												.Select(g => (Polygon)ConstructGeometryPart(g))
																												.ToList());
					((MultiPolygon)_geometry).BoundingBoxes = this.BoundingBox;
					break;
				default:
					throw new NotSupportedException("Geometry type " + sinkCollection.GeometryType.ToString() + " is not possible in GetConstructedGeometry.");
			}

			return _geometry;

		}


		public double[] BoundingBox
		{
			get
			{
				return new double[] { _lonMin, _latMin, _lonMax, _latMax };
			}
		}

		private IGeometryObject ConstructGeometryPart(SinkGeometry<OpenGisGeographyType> geomPart)
		{

			IGeometryObject geometry = null;

			switch (geomPart.GeometryType)
			{
				case OpenGisGeographyType.Point:
					geometry = new Point(geomPart[0][0]);
					break;
				case OpenGisGeographyType.MultiPoint:
					MultiPoint mp = new MultiPoint(geomPart.Select(g => new Point(g[0])).ToList());
					geometry = mp;
					break;
				case OpenGisGeographyType.LineString:
					geometry = new LineString(geomPart[0]);
					break;
				case OpenGisGeographyType.MultiLineString:
					geometry = new MultiLineString(geomPart.Select(line => new LineString(line))
																																		.ToList()
																															);
					break;
				case OpenGisGeographyType.Polygon:
					geometry = new Polygon(geomPart.Select(line => new LineString(line))
																																		.ToList()
																															);
					break;

				default:
					throw new NotSupportedException("Geometry type " + geomPart.GeometryType.ToString() + " is not supported yet.");
			}

			return geometry;
		}

	}
}
