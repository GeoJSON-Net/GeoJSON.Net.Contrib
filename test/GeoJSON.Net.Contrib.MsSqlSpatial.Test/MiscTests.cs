using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.SqlServer.Types;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace GeoJSON.Net.Contrib.MsSqlSpatial.Test
{
    [TestClass]
    public class MiscTests
    {
        /// <summary>
        /// See https://github.com/GeoJSON-Net/GeoJSON.Net.Contrib/issues/21
        /// </summary>
        [TestMethod]
        public void Issue21_PointInsideGeography()
        {
            var sqlGeographyPoint = SqlGeography.Point(47.2219, -122.4479, 4326);

            // The point should be inside Washington state
            var stateWashington = "{\"type\":\"Feature\",\"geometry\":{\"type\":\"Polygon\",\"coordinates\":[[[-116.9980726794949,46.330169786151487],[-116.90652787968992,46.177775987322832],[-116.91500281458571,45.999983222022593],[-118.97823605027111,45.992567653988658],[-119.33699968145743,45.888336290055861],[-119.59204952047821,45.932235419287906],[-119.78730891989301,45.850767523779837],[-120.15826818910023,45.740670884781935],[-120.60963598695361,45.753900051448625],[-120.83719315655094,45.672948920263423],[-121.0467410894945,45.637498887711104],[-121.203889120094,45.689898790055224],[-121.56107662009549,45.732764390641307],[-121.78809118715356,45.700983384781807],[-122.17491512107449,45.595150051447945],[-122.42045568910925,45.591997789078221],[-122.65170772361546,45.63008331967734],[-122.72578588930577,45.770333156917559],[-122.75224422263926,45.93807485613678],[-122.9379693202963,46.12909678810388],[-123.22849422264113,46.186250922218562],[-123.47137345441303,46.277253119484669],[-123.77670365460955,46.282033189471633],[-123.99867632678428,46.2830694871044],[-124.079635,46.86475],[-124.39567,47.72017000000011],[-124.68721008300781,48.184432983398381],[-124.56610107421875,48.379714965820426],[-123.12,48.04],[-122.58713802146673,47.095885321636388],[-122.34002132224703,47.359951890647778],[-122.49983068910967,48.180160223984387],[-122.84,49.000000000000114],[-120.00269628968556,49.000885321643864],[-117.03142981653929,48.999309190458973],[-117.02664974655227,47.72292715106596],[-116.9980726794949,46.330169786151487]]]},\"properties\":{\"scalerank\":2,\"adm1_code\":\"USA-3519\",\"diss_me\":3519,\"adm1_cod_1\":\"USA-3519\",\"iso_3166_2\":\"US-WA\",\"wikipedia\":\"http://en.wikipedia.org/wiki/Washington_(state)\",\"sr_sov_a3\":\"US1\",\"sr_adm0_a3\":\"USA\",\"iso_a2\":\"US\",\"adm0_sr\":6,\"admin0_lab\":2,\"name\":\"Washington\",\"name_alt\":\"WA|Wash.\",\"name_local\":null,\"type\":\"State\",\"type_en\":\"State\",\"code_local\":\"US53\",\"code_hasc\":\"US.WA\",\"note\":null,\"hasc_maybe\":null,\"region\":\"West\",\"region_cod\":null,\"region_big\":\"Pacific\",\"big_code\":null,\"provnum_ne\":0,\"gadm_level\":1,\"check_me\":0,\"scaleran_1\":2,\"datarank\":1,\"abbrev\":\"Wash.\",\"postal\":\"WA\",\"area_sqkm\":0,\"sameascity\":-99,\"labelrank\":0,\"featurec_1\":\"Admin-1 scale rank\",\"admin\":\"United States of America\",\"name_len\":10,\"mapcolor9\":1,\"mapcolor13\":1,\"featureclass\":\"Admin-1 scale rank\"}}";
            var geoJsonWashington = JsonConvert.DeserializeObject<GeoJSON.Net.Feature.Feature>(stateWashington);

            var geo = geoJsonWashington.ToSqlGeography(4326);
            if (geo.STIsValid().IsFalse) geo = geo.MakeValid();
            if (geo.EnvelopeAngle() >= 90) geo = geo.ReorientObject();
            bool within = sqlGeographyPoint.STWithin(geo).IsTrue;
            bool intersects = sqlGeographyPoint.STIntersects(geo).IsTrue;

            Assert.IsTrue(within && intersects);



        }

        /// <summary>
        /// See https://github.com/GeoJSON-Net/GeoJSON.Net.Contrib/issues/21
        /// </summary>
        [TestMethod]
        public void Issue21_PointOutsideGeography()
        {
            var sqlGeographyPoint = SqlGeography.Point(47.2219, -122.4479, 4326);

            // It shouldn't be in Arkansas, but this poly has incorrect ring orientation
            // After ReorientObject() this should be ok
            var stateArkansas = "{\"type\":\"Feature\",\"geometry\":{\"type\":\"Polygon\",\"coordinates\":[[[-89.662919481143419,36.023072821591967],[-89.673513149763252,35.94000295668279],[-89.775109015649065,35.799236355119717],[-89.950266282902376,35.701877956681827],[-89.988894416040552,35.536229152970236],[-90.147101813502132,35.404996853165017],[-90.13493201369738,35.113955186497193],[-90.249240281927,35.020834255507239],[-90.268283047226561,34.941459255506913],[-90.446618415065814,34.866838487277448],[-90.4503132799747,34.7218602564826],[-90.584206916042945,34.454098822562258],[-90.6995487129184,34.397461452770358],[-90.876307949572748,34.261474921194306],[-90.9821412829065,34.0551050888367],[-91.20068091506883,33.706392523731139],[-91.223444383493415,33.469326890592171],[-91.1080767484018,33.2068364527656],[-91.156239183297828,33.010000922165858],[-92.001303880566837,33.043874823533173],[-93.094027879594648,33.010517686488768],[-94.059757046265162,33.012119655889819],[-94.002086147827441,33.57991445569678],[-94.233338182333569,33.583609320605646],[-94.427538214886425,33.570380153938927],[-94.479912279014258,33.635983384733464],[-94.451361050172991,34.510710354138],[-94.430173712933311,35.483312486303348],[-94.628611212934118,36.540586452778939],[-93.412587246457889,36.52629791925024],[-92.30717668330243,36.523662421203355],[-91.251478848011743,36.52314565688043],[-90.112194383488969,36.461754055317684],[-90.029098680363632,36.337937323546356],[-90.141804979192216,36.230502020811556],[-90.253994513697862,36.122549953753833],[-90.315386115260608,36.023072821591967],[-89.662919481143419,36.023072821591967]]]},\"properties\":{\"scalerank\":2,\"adm1_code\":\"USA-3528\",\"diss_me\":3528,\"adm1_cod_1\":\"USA-3528\",\"iso_3166_2\":\"US-AR\",\"wikipedia\":\"http://en.wikipedia.org/wiki/Arkansas\",\"sr_sov_a3\":\"US1\",\"sr_adm0_a3\":\"USA\",\"iso_a2\":\"US\",\"adm0_sr\":1,\"admin0_lab\":2,\"name\":\"Arkansas\",\"name_alt\":\"AR|Ark.\",\"name_local\":null,\"type\":\"State\",\"type_en\":\"State\",\"code_local\":\"US05\",\"code_hasc\":\"US.AR\",\"note\":null,\"hasc_maybe\":null,\"region\":\"South\",\"region_cod\":null,\"region_big\":\"West South Central\",\"big_code\":null,\"provnum_ne\":0,\"gadm_level\":1,\"check_me\":0,\"scaleran_1\":2,\"datarank\":1,\"abbrev\":\"Ark.\",\"postal\":\"AR\",\"area_sqkm\":0,\"sameascity\":-99,\"labelrank\":0,\"featurec_1\":\"Admin-1 scale rank\",\"admin\":\"United States of America\",\"name_len\":8,\"mapcolor9\":1,\"mapcolor13\":1,\"featureclass\":\"Admin-1 scale rank\"}}";


            var geoJsonArkansas = JsonConvert.DeserializeObject<GeoJSON.Net.Feature.Feature>(stateArkansas);

            var geo = geoJsonArkansas.ToSqlGeography(4326);
            if (geo.STIsValid().IsFalse) geo = geo.MakeValid();
            bool within = sqlGeographyPoint.STWithin(geo).IsTrue;
            bool intersects = sqlGeographyPoint.STIntersects(geo).IsTrue;

            Assert.IsTrue(within && intersects);


            if (geo.EnvelopeAngle() >= 90) geo = geo.ReorientObject();
            within = sqlGeographyPoint.STWithin(geo).IsTrue;
            intersects = sqlGeographyPoint.STIntersects(geo).IsTrue;

            Assert.IsFalse(within);
            Assert.IsFalse(intersects);

        }
    }


}
