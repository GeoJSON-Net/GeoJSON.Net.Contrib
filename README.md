[![NuGet Version](http://img.shields.io/nuget/v/GeoJSON.NET.Contrib.MsSqlSpatial.svg?style=flat&label=nuget%20MsSqlSpatial)](https://www.nuget.org/packages/GeoJSON.NET.Contrib.MsSqlSpatial/)
[![NuGet Version](http://img.shields.io/nuget/v/GeoJSON.NET.Contrib.Wkb.svg?style=flat&label=nuget%20Wkb)](https://www.nuget.org/packages/GeoJSON.NET.Contrib.Wkb/) 
[![NuGet Version](http://img.shields.io/nuget/v/GeoJSON.NET.Contrib.EntityFramework.svg?style=flat&label=nuget%20EntityFramework)](https://www.nuget.org/packages/GeoJSON.NET.Contrib.EntityFramework/) 
[![Build status](https://ci.appveyor.com/api/projects/status/8i73123t14xro67k?svg=true)](https://ci.appveyor.com/project/GeojsonNet/geojson-net-contrib)

# GeoJSON.Net.Contrib 
Repository for all GeoJSON.Net *.Contrib projects

## GeoJSON.Net.Contrib.MsSqlSpatial
Allows conversion from / to Microsoft Sql Server geometry and geography data types.

[NuGet package](https://www.nuget.org/packages/GeoJSON.Net.Contrib.MsSqlSpatial):
`Install-Package GeoJSON.Net.Contrib.MsSqlSpatial`

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

### WKT conversion examples:

```csharp
using GeoJSON.Net.Contrib.MsSqlSpatial;
using GeoJSON.Net.Geometry;

// LineString from WKT
LineString lineString = WktConvert.GeoJSONObject<LineString>("LINESTRING(1 47,1 46,0 46,0 47,1 47)");

// LineString IGeometryObject from WKT
IGeometryObject lineStringGeom = WktConvert.GeoJSONGeometry("LINESTRING(1 47,1 46,0 46,0 47,1 47)");
```


## GeoJSON.Net.Contrib.EntityFramework
Allows conversion from / to EntityFramework geometry and geography data types.

[NuGet package](https://www.nuget.org/packages/GeoJSON.Net.Contrib.EntityFramework):
`Install-Package GeoJSON.Net.Contrib.EntityFramework`

### Conversion examples:

```csharp
using GeoJSON.Net.Geometry;
using GeoJSON.Net.Contrib.EntityFramework;

// DbGeography sample point
var dbGeographyPoint = DbGeography.FromText("POINT(30 10)", 4326);

// DbGeography -> GeoJSON example
Point point = dbGeographyPoint.ToGeoJSONObject<Point>();

// GeoJSON -> DbGeography example
DbGeography dbGeographyPoint = point.ToDbGeography();
```


## GeoJSON.Net.Contrib.Wkb
Allows conversion from / to Wkb binary types. Only X,Y,Z coordinates are supported, attempting to convert a geometry with M coordinates will throw an `Exception`.

[NuGet package](https://www.nuget.org/packages/GeoJSON.Net.Contrib.Wkb):
`Install-Package GeoJSON.Net.Contrib.Wkb`

### Conversion examples:

```csharp
using GeoJSON.Net.Geometry;
using GeoJSON.Net.Contrib.Wkb;

// GeoJson sample point
Point point = new Point(new Position(53.2455662, 90.65464646));

// GeoJson -> Wkb example
byte[] wkbPoint = point.ToWkb();

// Wkb -> GeoJson example
Point pointFromWkb = wkbPoint.ToGeoJSONObject<Point>();
```


# Contribution Guide

## Development Environment

- [Git](https://git-scm.com/downloads) client 2.0+
- [.NET Core SDK](https://dotnet.microsoft.com/download/dotnet-core) 3.0+
- (Optional) Development IDE, common choices are:
    - [Visual Studio Code](https://code.visualstudio.com/) with [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
    - [Visual Studio 2019](https://visualstudio.microsoft.com/vs/)
    - [JetBrains Rider](https://www.jetbrains.com/rider/)

## Build, test and package the solution

Use shell of your choice (`cmd.exe`, `powershell.exe`, `pwsh`, `bash`, etc) to run the following commands:

```sh
# verify .NET SDK
dotnet --info
# => .NET Core SDK (reflecting any global.json):
# => Version: 3.1.301
# => ...

# download repository
cd <development-directory-root>
git clone https://github.com/GeoJSON-Net/GeoJSON.Net.Contrib.git
cd GeoJSON.Net.Contrib

# build ("Debug" configuration)
dotnet build src/GeoJSON.Net.Contrib.sln

# test ("Debug" configuration)
dotnet test src/GeoJSON.Net.Contrib.sln

# package ("Release" configuration)
git clean -dfx
dotnet pack --configuration Release src/GeoJSON.Net.Contrib.sln

# push all projects to NuGet server (requires "package" step above to be run first)
# note: in addition to ".nupkg", the command below will automatically
# detect ".snupkg" symbol package and push both ".nupkg" and ".snupkg"
# to the specified NuGet server; for more information see
# https://docs.microsoft.com/en-us/nuget/create-packages/symbol-packages-snupkg
dotnet nuget push **/*.nupkg --api-key SECRET --source https://api.nuget.org/v3/index.json
```
