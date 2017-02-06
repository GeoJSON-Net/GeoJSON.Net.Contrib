# GeoJSON.Net.Contrib
Repository for all GeoJSON.Net *.Contrib projects

## GeoJSON.Net.Contrib.MsSqlSpatial
Allows conversion from / to Microsoft Sql Server geometry and geography data types.

### Conversion examples:

```csharp
using GeoJSON.Net.Geometry;
using Microsoft.SqlServer.Types;
using GeoJSON.Net.Contrib.MsSqlSpatial;

// SqlGeometry sample point
SqlGeometry simplePoint = SqlGeometry.Point(1, 47, 4326);

// SqlGeometry -> GeoJSON example
Point point = simplePoint.ToGeoJSONObject<Point>();

// GeoJSON -> SqlGeometry example
SqlGeometry sqlPoint = point.ToSqlGeometry(4326);
```

### WKT helper examples:

```csharp
using GeoJSON.Net.Contrib.MsSqlSpatial;
using GeoJSON.Net.Geometry;

// LineString from WKT
LineString lineString = WktHelper.GeoJSONObject<LineString>("LINESTRING(1 47,1 46,0 46,0 47,1 47)");

// LineString IGeometryObject from WKT
IGeometryObject lineStringGeom = WktHelper.GeoJSONGeometry("LINESTRING(1 47,1 46,0 46,0 47,1 47)");
```
