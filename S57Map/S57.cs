using OSGeo.OGR;
using System.Collections.Generic;

namespace S57Map
{
    /// <summary>
    /// This is the top level S57 code.  It include the initialization code as was as the outer most loop that constructs and drives the display.
    /// It is called from the top level application form only once when the "Load" option is used.
    /// </summary>
    public partial class S57
    {
        public static S57Objects myS57Objects;
        static DataSource ds;

        // constructor for this class
        public static void Initialize()
        {
            // Set the OGR options we want
            GdalConfiguration.GdalConfig("S57_CSV", @"C:\Program Files\gdal\bin\gdal-data");
            GdalConfiguration.GdalConfig("OGR_S57_OPTIONS", "UPDATES=APPLY,SPLIT_MULTIPOINT=OFF");
            GdalConfiguration.ConfigureGdal();
            GdalConfiguration.ConfigureOgr();
        }

        public static void InitializeBackgroundMap(ref SharpMap.Forms.MapBox mapBox1)
        {
            string GSHHGFileName = @"d:\Users\dwoodall\Documents\GSHHG\gshhg-shp-2.3.5-1\GSHHS_shp\f\GSHHS_f_L1.shp";

            // this is the default area to be displayed.  do not make it bigger unless you want to wait forever for it to render
            GeoAPI.Geometries.Envelope envelope = new GeoAPI.Geometries.Envelope(-75.0d, -80.0d, 23.0d, 28.0d);

            //ShapeFile GSHHGData = new ShapeFile(GSHHGFileName);
            SharpMap.Layers.VectorLayer GSHHGLayer = new SharpMap.Layers.VectorLayer("GSHHG");
            GSHHGLayer.DataSource = new SharpMap.Data.Providers.ShapeFile(GSHHGFileName, true);

            if (!GSHHGLayer.DataSource.IsOpen)
            {
                GSHHGLayer.DataSource.Open();
            }

            //Create the style for Land
            SharpMap.Styles.VectorStyle landStyle = new SharpMap.Styles.VectorStyle();
            landStyle.Fill = new System.Drawing.SolidBrush(S52.colorMaptoRGBI(S52.ColorName.LANDF));
            landStyle.Outline = new System.Drawing.Pen(System.Drawing.Color.Black);

            //Create the style for Water
            SharpMap.Styles.VectorStyle waterStyle = new SharpMap.Styles.VectorStyle();
            waterStyle.Fill = new System.Drawing.SolidBrush(S52.colorMaptoRGBI(S52.ColorName.DEPMD));

            //Create the default style
            SharpMap.Styles.VectorStyle defaultStyle = new SharpMap.Styles.VectorStyle();
            defaultStyle.Fill = new System.Drawing.SolidBrush(System.Drawing.Color.Red);

            //Create the theme items
            Dictionary<string, SharpMap.Styles.IStyle> styles = new Dictionary<string, SharpMap.Styles.IStyle>();
            styles.Add("land", landStyle);
            styles.Add("water", waterStyle);
            styles.Add("default", defaultStyle);

            GSHHGLayer.Style = landStyle;
            GSHHGLayer.Style.EnableOutline = true;

            mapBox1.BackColor = S52.colorMaptoRGB(S52.ColorName.UINFB);

            mapBox1.Map.Layers.Add(GSHHGLayer);
            mapBox1.Map.ZoomToExtents();
            mapBox1.Refresh();

            GeoAPI.CoordinateSystems.Transformations.ICoordinateTransformationFactory ctFact
                = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();

            GSHHGLayer.CoordinateTransformation =
                ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84,
                ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator);

            GSHHGLayer.ReverseCoordinateTransformation =
                ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator,
                ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84);

            //mapBox1.Map.BackgroundLayer.Add(new SharpMap.Layers.TileAsyncLayer(
            //        new BruTile.Web.OsmTileSource(), "OSM"));

            //mapBox1.Map.ZoomToBox(envelope);
            mapBox1.Map.ZoomToExtents();
            mapBox1.Refresh();
            mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan;
        }

        public static SharpMap.Layers.VectorLayer InitializeGSHHGMap()
        {
            SharpMap.Layers.VectorLayer GSHHGLayer = new SharpMap.Layers.VectorLayer("GSHHG");
            GSHHGLayer.DataSource = new SharpMap.Data.Providers.ShapeFile(@"d:\Users\dwoodall\Documents\GSHHG\gshhg-shp-2.3.5-1\GSHHS_shp\f\GSHHS_f_L1.shp", true);

            //Create the style for Land
            SharpMap.Styles.VectorStyle landStyle = new SharpMap.Styles.VectorStyle();
            landStyle.Fill = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(232, 232, 232));

            //Create the style for Water
            SharpMap.Styles.VectorStyle waterStyle = new SharpMap.Styles.VectorStyle();
            waterStyle.Fill = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(198, 198, 255));

            //Create the theme items
            Dictionary<string, SharpMap.Styles.IStyle> styles = new Dictionary<string, SharpMap.Styles.IStyle>();
            styles.Add("land", landStyle);
            styles.Add("water", waterStyle);

            //Assign the theme
            GSHHGLayer.Theme = new SharpMap.Rendering.Thematics.UniqueValuesTheme<string>("class", styles, landStyle);

            return GSHHGLayer;

            //mapBox.Map.Layers.Add(vlay);

            //ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory ctFact = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();
            //vlay.CoordinateTransformation = ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84, ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator);
            //vlay.ReverseCoordinateTransformation = ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator, ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84);

            //mapBox1.Map.BackgroundLayer.Add(new SharpMap.Layers.TileAsyncLayer(
            //            new BruTile.Web.OsmTileSource(), "OSM"));

            //mapBox.Map.ZoomToExtents();
            //mapBox.Refresh();
            //mapBox.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan;
            //return mapBox;
        }

        public static SharpMap.Map Load()
        {
            string fileName = @"c:\geodata\s57\US5TX51M.000";
            ds = OpenMap(fileName);
            return Render(ds);
        }
    }
}