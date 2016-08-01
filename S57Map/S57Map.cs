using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

//*******************************************************************************//
//
// This is the main code that mostly contains the windows generated items for the gui.
//  However, the calls to initialization code for various components is here as well.

namespace S57Map
{
    public partial class S57Map : Form
    {
        public S57Map()
        {
            InitializeComponent();
            InitializeMyComponents();
            //new LogMessages();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void InitializeMyComponents()
        {
            // this automatically gets ran at startup time.  Called from the system initiiazer, above.
        }

        private void S57MapForm_Closing()
        {
            DebugUtil.Close();
        }

        // This displays the GSHHG or GSHHS or whatever they call themselves this week world map.  currently using the high def version
        // Note that these databases do not have meaningful attributes, so it is not possible to use a theme to display these
        // Instead, the rendoring style is assigned by layer.

        private void initializeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            S57.InitializeBackgroundMap(ref mapBox1);
#if false
            string GSHHGFileName = @"d:\Users\dwoodall\Documents\GSHHG\gshhg-shp-2.3.5-1\GSHHS_shp\f\GSHHS_f_L1.shp";

            // this is the default area to be displayed.  do not make it bigger unless you want to wait forever for it to render
            Envelope envelope = new Envelope(-75.0d, -80.0d, 23.0d, 28.0d);



            //ShapeFile GSHHGData = new ShapeFile(GSHHGFileName);
            SharpMap.Layers.VectorLayer GSHHGLayer = new SharpMap.Layers.VectorLayer("GSHHG");
            GSHHGLayer.DataSource = new SharpMap.Data.Providers.ShapeFile(GSHHGFileName);

            //Create the style for Land
            SharpMap.Styles.VectorStyle landStyle = new SharpMap.Styles.VectorStyle();
            landStyle.Fill = new System.Drawing.SolidBrush(System.Drawing.Color.SaddleBrown);
            landStyle.Outline = new System.Drawing.Pen(System.Drawing.Color.Black);

           //Create the style for Water
            SharpMap.Styles.VectorStyle waterStyle = new SharpMap.Styles.VectorStyle();
            waterStyle.Fill = new System.Drawing.SolidBrush(System.Drawing.Color.LightBlue);

            //Create the default style
            SharpMap.Styles.VectorStyle defaultStyle = new SharpMap.Styles.VectorStyle();
            defaultStyle.Fill = new System.Drawing.SolidBrush(System.Drawing.Color.Red);

            //Create the theme items
            Dictionary<string, SharpMap.Styles.IStyle> styles = new Dictionary<string, SharpMap.Styles.IStyle>();
            styles.Add("land", landStyle);
            styles.Add("water", waterStyle);
            styles.Add("default", defaultStyle); 

            //GSHHGLayer.Theme = null;
            GSHHGLayer.Style = landStyle;
            GSHHGLayer.Style.EnableOutline = true;

            //Console.WriteLine("GSHHG LayerName = " + GSHHGLayer.LayerName + " Theme = " + GSHHGLayer.Theme);

            //GSHHGLayer = S57.InitializeGSHHGMap();

            mapBox1.Map.BackColor = System.Drawing.Color.LightBlue;
            mapBox1.Map.Layers.Add(GSHHGLayer);
            mapBox1.Map.ZoomToBox(envelope);
            mapBox1.Refresh();
            mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan;
#endif
            Cursor = Cursors.Default;
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapBox1.Map = S57.Load();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form optionForm = new S57Options();
            optionForm.Show();
        }

        private void treeViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            treeView1.Nodes.Add(S57.CreateTree());
            treeView1.Show();
            Cursor.Current = Cursors.Default;
        }

        private void S57MapForm_Load(object sender, EventArgs e)
        {
        }
    }
}