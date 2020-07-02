using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeoJSON.Net.Contrib.MsSqlSpatial.Test
{
    [TestClass]
    public static class AssemblyInitializer
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext _) => AssemblyInit();

        public static void AssemblyInit()
        {
            var basePath = Path.Combine(
                Path.GetDirectoryName(new Uri(typeof(AssemblyInitializer).Assembly.CodeBase).AbsolutePath) ?? Environment.CurrentDirectory,
                "SqlServerTypes",
                Environment.Is64BitProcess ? "x64" : "x86");

            void LoadLibrary(string libraryFilename)
            {
                var libraryPath = Path.Combine(basePath, libraryFilename);
                var libraryAddress = NativeMethods.LoadLibrary(libraryPath);
                if (libraryAddress == IntPtr.Zero)
                {
                    throw new Win32Exception($"Failed to load SQL Server DLL \"{libraryPath}\".");
                }
            }

            LoadLibrary("msvcr120.dll");
            LoadLibrary("SqlServerSpatial140.dll");
        }

        private static class NativeMethods
        {
            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern IntPtr LoadLibrary(string libraryName);
        }
    }

}
