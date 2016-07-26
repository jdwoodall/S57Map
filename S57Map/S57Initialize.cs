using OSGeo.OGR;
using System;
using System.Windows.Forms;

// the code to span the map largely came from ogrinfo which is in gdal\csharp distribution
// be careful about mixing SharpMap.Data.Provider and OSGeo.OGR as they contain conflicting definitions

namespace S57Map
{
    public partial class S57
    {
        private static DataSource ds;
        private static string _S57FileName;
        private static DataSource _S57DataSource;
        private static string _S57DataSourceName;
        private static Driver _S57Driver;

        // public functions to retrieve DataSource,  DataSourceName, number of layer and driver
        public static DataSource DataSource => _S57DataSource;

        public static string DataSourceName => _S57DataSourceName;

        public static string FileName => _S57FileName;

        public static int NumberLayers() => ds.GetLayerCount();

        public static Driver Driver() => _S57Driver;

        //  simple function that only opens the driver and returns a pointer to the provider
        public DataSource Initialize(String fileName)
        {
            // save the dataset name
            _S57FileName = fileName;

            // register all the drivers so we can find the one we need - this is slow if debug output is on
            Ogr.RegisterAll();

            // open the specified fileName with update set to R/O (=0)
            ds = Ogr.Open(fileName, 0);
            if (ds == null)
            {
                MessageBox.Show("Ogr.Open returned a zero.  Make sure your file pathname is correct.");
                _S57DataSource = null;
                _S57FileName = "";
            }
            else
            {
                DebugUtil.WriteLine("DataSource is: " + ds.ToString());
                DebugUtil.WriteLine("DataSource Name is: " + ds.name.ToString());
                _S57DataSource = ds;
                _S57DataSourceName = ds.name;

                Driver drv = ds.GetDriver();
                _S57Driver = drv;

                if (drv == null)
                {
                    MessageBox.Show("S-57 driver is null.  Make sure your drivers are in the correct directory and GDAL_DRV points to them.");
                    ds = null;
                }
                else
                {
                    DebugUtil.WriteLine("Driver is: " + drv.ToString());
                }
            }

            return ds;
        }
    }
}