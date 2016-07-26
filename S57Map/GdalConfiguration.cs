//////////////////////////////////////////////////////////////////////////////////////
//
// This file has been modied to only load the S-57 driver and to add my debug handler
// instead of the #if DEBUG so that it can be turned on and off without re-compiling.
// See DebugUtile.cs for more details.
//
// Also added the almost trival config method that sets environment variables.
// No error checking is to done to insure it is a valid OGR or GDAL variable.
//
// BIG BIG Note:  This seems to always use the GDAL drivers in the project directory
//                even though you may have a later version of GDAL somewhere else.
//                This is not necessarily a bad thing as SharpMap does not run with 
//                with the latest version of GDAL, Brutiles, or just about any other 
//                library.  If you want that, you need to rebuild SharpMap and resolve
//                all the issues.  This is NOT FUN.
//
// DW.
//
//////////////////////////////////////////////////////////////////////////////////////
//
//
//
/******************************************************************************
 *
 * Name:     GdalConfiguration.cs.pp
 * Project:  GDAL CSharp Interface
 * Purpose:  A static configuration utility class to enable GDAL/OGR.
 * Author:   Felix Obermaier
 *
 ******************************************************************************
 * Copyright (c) 2012, Felix Obermaier
 *
 * Permission is hereby granted, free of charge, to any person obtaining a
 * copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included
 * in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 *****************************************************************************/

using System;
using System.IO;
using System.Reflection;
using Gdal = OSGeo.GDAL.Gdal;
using Ogr = OSGeo.OGR.Ogr;

namespace S57Map
{
    public static partial class GdalConfiguration
    {
        private static volatile bool _configuredOgr;
        private static volatile bool _configuredGdal;

        /// <summary>
        /// Function to determine which platform we're on
        /// </summary>
        private static string GetPlatform() => IntPtr.Size == 4 ? "x86" : "x64";

        public static bool GdalConfig(string arg1, string arg2)
        {
            Environment.SetEnvironmentVariable(arg1, arg2);

            return (Environment.GetEnvironmentVariable(arg1) == arg2);
        }

        /// <summary>
        /// Construction of Gdal/Ogr
        /// </summary>
        static GdalConfiguration()
        {
            var executingAssemblyFile = new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
            var executingDirectory = Path.GetDirectoryName(executingAssemblyFile);

            if (string.IsNullOrEmpty(executingDirectory))
                throw new InvalidOperationException("cannot get executing directory");

            var gdalPath = Path.Combine(executingDirectory, "gdal");
            var nativePath = Path.Combine(gdalPath, GetPlatform());

            // Prepend native path to environment path, to ensure the
            // right libs are being used.
            var path = Environment.GetEnvironmentVariable("PATH");
            path = nativePath + ";" + Path.Combine(nativePath, "plugins") + ";" + path;
            Environment.SetEnvironmentVariable("PATH", path);

            // Set the additional GDAL environment variables.
            var gdalData = Path.Combine(gdalPath, "data");
            Environment.SetEnvironmentVariable("GDAL_DATA", gdalData);
            Gdal.SetConfigOption("GDAL_DATA", gdalData);

            var driverPath = Path.Combine(nativePath, "plugins");
            Environment.SetEnvironmentVariable("GDAL_DRIVER_PATH", driverPath);
            Gdal.SetConfigOption("GDAL_DRIVER_PATH", driverPath);

            Environment.SetEnvironmentVariable("GEOTIFF_CSV", gdalData);
            Gdal.SetConfigOption("GEOTIFF_CSV", gdalData);

            var projSharePath = Path.Combine(gdalPath, "share");
            Environment.SetEnvironmentVariable("PROJ_LIB", projSharePath);
            Gdal.SetConfigOption("PROJ_LIB", projSharePath);
        }

        /// <summary>
        /// Method to ensure the static constructor is being called.
        /// </summary>
        /// <remarks>Be sure to call this function before using Gdal/Ogr/Osr</remarks>
        public static void ConfigureOgr()
        {
            if (_configuredOgr) return;

            // Register drivers
            Ogr.GetDriverByName("S57");

            //Ogr.RegisterAll();
            _configuredOgr = true;

            PrintDriversOgr();
        }

        /// <summary>
        /// Method to ensure the static constructor is being called.
        /// </summary>
        /// <remarks>Be sure to call this function before using Gdal/Ogr/Osr</remarks>
        public static void ConfigureGdal()
        {
            if (_configuredGdal) return;

            // Register drivers
            Gdal.GetDriverByName("S57");
            // Gdal.AllRegister();
            _configuredGdal = true;

            PrintDriversGdal();
        }

        private static void PrintDriversOgr()
        {
            if (DebugUtil._debug)
            {
                var num = Ogr.GetDriverCount();
                for (var i = 0; i < num; i++)
                {
                    var driver = Ogr.GetDriver(i);
                    DebugUtil.WriteLine(string.Format("OGR {0}: {1}", i, driver.name));
                }
            }
        }

        private static void PrintDriversGdal()
        {
            if (DebugUtil._debug)
            {
                var num = Gdal.GetDriverCount();
                for (var i = 0; i < num; i++)
                {
                    var driver = Gdal.GetDriver(i);
                    DebugUtil.WriteLine(string.Format("GDAL {0}: {1}-{2}", i, driver.ShortName, driver.LongName));
                }
            }
        }
    }
}