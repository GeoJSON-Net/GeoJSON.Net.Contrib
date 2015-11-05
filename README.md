# GeoJSON.Net.Contrib
Repository for all GeoJSON.Net *.Contrib projects

## GeoJSON.Net.Contrib.MsSqlSpatial
Allows conversion from / to Microsoft Sql Server geometry and geography data types.

Usage:
```csharp
// Example from SqlGeometry to GeoJSON
SqlGeometry simplePoint = SqlGeometry.Point(1, 47, 4326);
GeoJSON.Net.Geometry.Point point = MsSqlSpatialConvert.ToGeoJSONObject<Point>(simplePoint);

// Example from GeoJSON to SqlGeometry
simplePoint = point.ToSqlGeometry(4326);
```
