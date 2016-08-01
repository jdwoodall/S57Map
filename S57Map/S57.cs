using OSGeo.OGR;

namespace S57Map
{
    /// <summary>
    /// This is the top level S57 code.  It include the initialization code as was as the outer most loop that constructs and drives the display.
    /// It is called from the top level application form only once when the "Load" option is used.
    /// </summary>
    public partial class S57
    {
        public static S57Objects myS57Objects;
        internal static DataSource ds;

        // constructor for this class
        public static void Initialize()
        {
            // Set the OGR options we want
            GdalConfiguration.GdalConfig("S57_CSV", @"C:\Program Files\gdal\bin\gdal-data");
            GdalConfiguration.GdalConfig("OGR_S57_OPTIONS", "UPDATES=APPLY,SPLIT_MULTIPOINT=OFF");
            GdalConfiguration.ConfigureGdal();
            GdalConfiguration.ConfigureOgr();
        }

        public static SharpMap.Map Load()
        {
            string fileName = @"c:\geodata\s57\US5TX51M.000";
            ds = OpenMap(fileName);
            return Render(ds);
        }
    }
}