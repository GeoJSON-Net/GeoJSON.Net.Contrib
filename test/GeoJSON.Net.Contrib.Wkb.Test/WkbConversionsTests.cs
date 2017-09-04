using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;

namespace GeoJSON.Net.Contrib.Wkb.Test
{
    public partial class WkbConversionsTests
    {
        Point point;
        MultiPoint multiPoint;
        LineString lineString;
        MultiLineString multiLineString;
        Polygon polygon;
        Polygon polygonWithHole;
        Polygon polygonWithHoleReverseWinding;
        MultiPolygon multiPolygon;
        GeometryCollection geomCollection;
        Feature.Feature feature;
        FeatureCollection featureCollection;

        public WkbConversionsTests()
        {
            point = new Point(new Position(53.2455662, 90.65464646));

            multiPoint = new MultiPoint(new List<Point>
                {
                    new Point(new Position(52.379790828551016, 5.3173828125)),
                    new Point(new Position(52.36721467920585, 5.456085205078125)),
                    new Point(new Position(52.303440474272755, 5.386047363281249, 4.23))
                });
            lineString = new LineString(new List<IPosition>
                {
                    new Position(52.379790828551016, 5.3173828125),
                    new Position(52.36721467920585, 5.456085205078125),
                    new Position(52.303440474272755, 5.386047363281249, 4.23)
                });
            multiLineString = new MultiLineString(new List<LineString>
                {
                    new LineString(new List<IPosition>
                    {
                        new Position(52.379790828551016, 5.3173828125),
                        new Position(52.36721467920585, 5.456085205078125),
                        new Position(52.303440474272755, 5.386047363281249, 4.23)
                    }),
                    new LineString(new List<IPosition>
                    {
                        new Position(52.379790828551016, 5.3273828125),
                        new Position(52.36721467920585, 5.486085205078125),
                        new Position(52.303440474272755, 5.426047363281249, 4.23)
                    })
                });
            /*
			 * POLYGON (
			 *	new Position(5.6718750056992775 43.179268827576763), 
			 *	new Position(5.7627938771963274 43.282358019007539), 
			 *	new Position(5.6827878158334473 43.399165901196014), 
			 *	new Position(5.7883490332690677 43.420263481346808), 
			 *	new Position(5.6569195891695419 43.575280226136485), 
			 *	new Position(5.7992059672926253 43.659928964120652), 
			 *	new Position(5.772453944482355 43.722182053435269), 
			 *	new Position(5.5305079449053451 43.659231446664869), 
			 *	new Position(4.7390611308576647 43.924068511657794), 
			 *	new Position(4.641909591242106 43.867477587972004), 
			 *	new Position(4.6268107341244811 43.688132623094383), 
			 *	new Position(4.4864081749462246 43.698853136943086), 
			 *	new Position(4.4608683979209367 43.58945970637793), 
			 *	new Position(4.2379330762447731 43.49708004345662), 
			 *	new Position(4.55238424144851 43.446971295015622), 
			 *	new Position(4.6618166942350943 43.346294896388663), 
			 *	new Position(4.8579247842437638 43.334654947962143), 
			 *	new Position(4.861467735270022 43.455412079597927), 
			 *	new Position(5.0545884574514082 43.326670147834825), 
			 *	new Position(5.3160671845314269 43.359760733800755), 
			 *	new Position(5.3405286722678431 43.214414007811236), 
			 *	new Position(5.6718750056992775 43.179268827576763), 
			 *	
			 *  new Position(5.0144408111937375, 43.555545986597537), 
			 *	new Position( 5.2267178460469337, 43.453922302237586), 
			 *	new Position( 5.0599489280101588, 43.404914999013144), 
			 *	new Position(5.0144408111937375, 43.555545986597537)
			*/
            polygonWithHole = new Polygon(new List<LineString>
                {
                    new LineString(new List<Position>
                    {
                                            new Position( 43.179268827576763 ,5.6718750056992775),
                                            new Position(43.282358019007539   ,5.7627938771963274),
                                            new Position(43.399165901196014   ,5.6827878158334473),
                                            new Position(43.420263481346808   ,5.7883490332690677),
                                            new Position(43.575280226136485   ,5.6569195891695419),
                                            new Position(43.659928964120652   ,5.7992059672926253),
                                            new Position(43.722182053435269   ,5.772453944482355 ),
                                            new Position(43.659231446664869   ,5.5305079449053451),
                                            new Position(43.924068511657794   ,4.7390611308576647),
                                            new Position(43.867477587972004   ,4.641909591242106 ),
                                            new Position(43.688132623094383   ,4.6268107341244811),
                                            new Position(43.698853136943086   ,4.4864081749462246),
                                            new Position(43.58945970637793        ,4.4608683979209367),
                                            new Position(43.49708004345662        ,4.2379330762447731),
                                            new Position(43.446971295015622   ,4.55238424144851  ),
                                            new Position(43.346294896388663   ,4.6618166942350943),
                                            new Position(43.334654947962143   ,4.8579247842437638),
                                            new Position(43.455412079597927   ,4.861467735270022 ),
                                            new Position(43.326670147834825   ,5.0545884574514082),
                                            new Position(43.359760733800755   ,5.3160671845314269),
                                            new Position(43.214414007811236   ,5.3405286722678431),
                                            new Position(43.179268827576763   ,5.6718750056992775)
                    }),
                                         new LineString(new List<Position>
                                            {
                                                  new Position( 43.555545986597537,5.0144408111937375 ),
                                                    new Position( 43.453922302237586, 5.2267178460469337),
                                                     new Position( 43.404914999013144, 5.0599489280101588),
                                                     new Position( 43.555545986597537,5.0144408111937375   )
                                            })
                                });
            polygonWithHoleReverseWinding = new Polygon(new List<LineString>
                {
                    new LineString( polygonWithHole.Coordinates[0].Coordinates.Select(c => (Position)c).Reverse().ToList()),
                                        new LineString( polygonWithHole.Coordinates[1].Coordinates.Select(c => (Position)c).Reverse().ToList())
                                });
            polygon = new Polygon(new List<LineString>
                {
                    new LineString(new List<Position>
                    {
                        new Position(52.379790828551016, 5.3173828125),
                        new Position(52.303440474272755, 5.386047363281249, 4.23),
                        new Position(52.36721467920585, 5.456085205078125),
                        new Position(52.379790828551016, 5.3173828125)
                    })
                });

            multiPolygon = new MultiPolygon(new List<Polygon>
                {
                    new Polygon(new List<LineString>
                    {
                        new LineString(new List<IPosition>
                        {
                            new Position(52.959676831105995, -2.6797102391514338),
                            new Position(52.930592009390175, -2.6548779332193022),
                            new Position(52.89564268523565, -2.6931334629377890),
                            new Position(52.878791122091066, -2.6932445076063951),
                            new Position(52.875255907042678, -2.6373482332006359),
                            new Position(52.882954723868622, -2.6050779098387191),
                            new Position(52.875476700983896, -2.5851645010668989),
                            new Position(52.891287242948195, -2.5815104708998668),
                            new Position(52.908449372833715, -2.6079763270327119),
                            new Position(52.9608756693609, -2.6769029474483279),
                            new Position(52.959676831105995, -2.6797102391514338),
                        })
                    }),
                    new Polygon(new List<LineString>
                    {
                        new LineString(new List<IPosition>
                        {
                            new Position(52.89610842810761, -2.69628632041613),
                            new Position(52.926572918779222, -2.6996509024137052),
                            new Position(52.920394929466184, -2.772273870352612),
                            new Position(52.937353122653533, -2.7978187468478741),
                            new Position(52.94013913205788, -2.838979264607087),
                            new Position(52.929801009654575, -2.83848602260174),
                            new Position(52.90253773227807, -2.804554822840895),
                            new Position(52.89938894657412, -2.7663172788742449),
                            new Position(52.8894641454077, -2.75901233808515),
                            new Position(52.89610842810761, -2.69628632041613)
                        })
                    })
                });

            geomCollection = new GeometryCollection(new List<IGeometryObject>
                {
                    point,
                    multiPoint,
                    lineString,
                    multiLineString,
                    polygon,
                    multiPolygon
                });

            feature = new Feature.Feature(polygon, new Dictionary<string, object>() { { "Key", "Value" } }, "Id");

            featureCollection = new FeatureCollection(new List<Feature.Feature> {
                    feature, new Feature.Feature(multiPolygon, null)
            });
        }
    }
}
