using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoJSON.Net.Contrib.MsSqlSpatial.Test
{
    [TestClass]
    public class AssemblyInitializer
    {
        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}
