using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoJSON.Net.Geometry;
using Microsoft.SqlServer.Types;

namespace GeoJSON.Net.Contrib.MsSqlSpatial.Sinks
{
	/// <summary>
	/// Sink converting a SqlGeometry to a GeoJSON geometry
	/// Usage : <code>SqlGeometryGeoJsonSink sink = new SqlGeometryGeoJsonSink();
	///	sqlGeometry.Populate(sink);
	///	// sink.BoundingBox returns a GeoJSON compliant double[] bbox 
	///	return sink.ConstructedGeometry; // returns an IGeometryObject
	///	
	///	</code> 
	/// </summary>
	internal class SqlGeometryGeoJsonSink : IGeometrySink110
	{
		SinkGeometryCollection<OpenGisGeometryType> _geomCollection;
		bool _isGeometryCollection = false;
		int _nestLevel = 0;
		SinkGeometry<OpenGisGeometryType> _currentGeometry;
		SinkLineRing _currentRing;
		double _xmin = double.MaxValue;
		double _ymin = double.MaxValue;
		double _xmax = double.MinValue;
		double _ymax = double.MinValue;

	    private readonly bool _withBoundingBox;

	    public SqlGeometryGeoJsonSink(bool withBoundingBox = true)
	    {
	        this._withBoundingBox = withBoundingBox;
	    }

		#region Sink implementation

		public void BeginGeometry(OpenGisGeometryType type)
		{
			if (_geomCollection == null)
			{
				_geomCollection = new SinkGeometryCollection<OpenGisGeometryType>(type);
				if (type == OpenGisGeometryType.GeometryCollection)
				{
					_isGeometryCollection = true;
				}
			}

			_currentGeometry = new SinkGeometry<OpenGisGeometryType>(type);
			if (_isGeometryCollection && _nestLevel > 0)
			{
				if (_nestLevel == 1)
				{
					_geomCollection.SubItems.Add(new SinkGeometryCollection<OpenGisGeometryType>(type));
				}
				_geomCollection.SubItems.Last().Add(_currentGeometry);
			}
			else
			{
				_geomCollection.Add(_currentGeometry);
			}

			_nestLevel++;
		}

		public void BeginFigure(double x, double y, double? z, double? m)
		{
			_currentRing = new SinkLineRing();
			_currentRing.Add(new Position(y, x, z));

			UpdateBoundingBox(x, y);
		}

		public void AddLine(double x, double y, double? z, double? m)
		{
			_currentRing.Add(new Position(y, x, z));

			UpdateBoundingBox(x, y);
		}

		public void EndFigure()
		{
			if (_currentRing == null)
				return;

			_currentGeometry.Add(_currentRing);
			_currentRing = null;
		}

		public void EndGeometry()
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


		private void UpdateBoundingBox(double x, double y)
		{
			_xmin = Math.Min(x, _xmin);
			_ymin = Math.Min(y, _ymin);
			_xmax = Math.Max(x, _xmax);
			_ymax = Math.Max(y, _ymax);
		}

		#endregion

		public IGeometryObject ConstructedGeometry
		{
			get
			{
				IGeometryObject _geometry = null;

				switch (_geomCollection.GeometryType)
				{
					case OpenGisGeometryType.Point:
					case OpenGisGeometryType.MultiPoint:
					case OpenGisGeometryType.LineString:
					case OpenGisGeometryType.MultiLineString:
					case OpenGisGeometryType.Polygon:
					case OpenGisGeometryType.MultiPolygon:

						_geometry = GeometryFromSinkGeometryCollection(_geomCollection);

						break;
					case OpenGisGeometryType.GeometryCollection:

						List<IGeometryObject> subGeometries = _geomCollection.SubItems.Select(subItem => GeometryFromSinkGeometryCollection(subItem)).ToList();
						_geometry = new GeometryCollection(subGeometries);

					    ((GeometryCollection)_geometry).BoundingBoxes = this._withBoundingBox ? this.BoundingBox : null;
						break;
					default:
						throw new NotSupportedException("Geometry type " + _geomCollection.GeometryType.ToString() + " is not supported yet.");
				}

				return _geometry;
			}
		}

		private IGeometryObject GeometryFromSinkGeometryCollection(SinkGeometryCollection<OpenGisGeometryType> sinkCollection)
		{

			IGeometryObject _geometry = null;

			switch (sinkCollection.GeometryType)
			{
				case OpenGisGeometryType.Point:
					_geometry = ConstructGeometryPart(sinkCollection[0]);
					((Point)_geometry).BoundingBoxes = this._withBoundingBox ? this.BoundingBox : null;
					break;
				case OpenGisGeometryType.MultiPoint:
					_geometry = new MultiPoint(sinkCollection.Skip(1)
																										.Select(g => (Point)ConstructGeometryPart(g))
																										.ToList());
					((MultiPoint)_geometry).BoundingBoxes = this._withBoundingBox ? this.BoundingBox : null;
					break;
				case OpenGisGeometryType.LineString:
					_geometry = ConstructGeometryPart(sinkCollection[0]);
					((LineString)_geometry).BoundingBoxes = this._withBoundingBox ? this.BoundingBox : null;
					break;
				case OpenGisGeometryType.MultiLineString:
					_geometry = new MultiLineString(sinkCollection.Skip(1)
																												.Select(g => (LineString)ConstructGeometryPart(g))
																												.ToList());
					((MultiLineString)_geometry).BoundingBoxes = this._withBoundingBox ? this.BoundingBox : null;
					break;
				case OpenGisGeometryType.Polygon:
					_geometry = ConstructGeometryPart(sinkCollection.First());
					((Polygon)_geometry).BoundingBoxes = this._withBoundingBox ? this.BoundingBox : null;
					break;
				case OpenGisGeometryType.MultiPolygon:
					_geometry = new MultiPolygon(sinkCollection.Skip(1)
																												.Select(g => (Polygon)ConstructGeometryPart(g))
																												.ToList());
					((MultiPolygon)_geometry).BoundingBoxes = this._withBoundingBox ? this.BoundingBox : null;
					break;
				default:
					throw new NotSupportedException("Geometry type " + sinkCollection.GeometryType.ToString() + " is not possible in GetConstructedGeometry.");
			}

			return _geometry;

		}

		public double[] BoundingBox
		{
			get { return this._withBoundingBox ? new double[] {_xmin, _ymin, _xmax, _ymax} : null; }
		}

		private IGeometryObject ConstructGeometryPart(SinkGeometry<OpenGisGeometryType> geomPart)
		{

			IGeometryObject geometry = null;

			switch (geomPart.GeometryType)
			{
				case OpenGisGeometryType.Point:
					geometry = new Point(geomPart[0][0]);
					break;
				case OpenGisGeometryType.MultiPoint:
					MultiPoint mp = new MultiPoint(geomPart.Select(g => new Point(g[0])).ToList());
					geometry = mp;
					break;
				case OpenGisGeometryType.LineString:
					geometry = new LineString(geomPart[0]);
					break;
				case OpenGisGeometryType.MultiLineString:
                    geometry = new MultiLineString(geomPart.Select(line => new LineString(line)).ToList());
					break;
				case OpenGisGeometryType.Polygon:
                    geometry = new Polygon(geomPart.Select(line => new LineString(line)).ToList());
					break;
				case OpenGisGeometryType.MultiPolygon:
                    //geometry = new MultiPolygon(geomPart.Skip(1)
                    //.Select(g => (Polygon)ConstructGeometryPart(g))
                    //.ToList());
                    geometry = new MultiPolygon(geomPart.Select(polygon => new Polygon(geomPart.Select(line => new LineString(line)))));

                    break;

				default:
					throw new NotSupportedException("Geometry type " + geomPart.GeometryType.ToString() + " is not supported yet.");
			}

			return geometry;
		}


	}
}
