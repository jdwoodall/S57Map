using OSGeo.OGR;
using System;
using System.Windows.Forms;

//using SharpMap.Data.Providers;

//*******************************************************************************//
//
// This is the main code that mostly contains the windows generated items for the gui.
//  However, the initialization code for various components is here as well.

namespace S57Map
{
    public partial class S57Form : Form
    {
        private S57 myS57;

        public S57Form()
        {
            InitializeComponent();
            //new LogMessages();


        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private string fileName = @"c:\geodata\s57\US5TX51M.000";
        private DataSource ds;

        private void InitializeMyComponents()
        {
            // this automatically gets ran at startup time.  Called from the system initiiazer, above.

            // Intialize the S57Objects
            S57Objects myS57Objects = new S57Objects();

            // Set the OGR options we want
            GdalConfiguration.GdalConfig("S57_CSV", @"C:\Program Files\gdal\bin\gdal-data");
            GdalConfiguration.GdalConfig("OGR_S57_OPTIONS", "UPDATES=APPLY,SPLIT_MULTIPOINT=OFF");
            GdalConfiguration.ConfigureGdal();
            GdalConfiguration.ConfigureOgr();

            if (!DebugUtil.Initialize())
            {
                MessageBox.Show("Debug Initialization Error.  Deubber output disabled.");
                DebugUtil._debug = false;
            }


            myS57 = new S57();
            ds = myS57.Initialize(fileName);
        }

        private void SharpMapOGRForm_Closing()
        {
            DebugUtil.Close();
        }

        private void initializeToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mapBox1.Map = myS57.Render(0.0f);
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form optionForm = new S57Options();
            optionForm.Show();
        }

        private void treeViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            treeView1.Nodes.Add(myS57.CreateTree(ds));
            treeView1.Show();
            Cursor.Current = Cursors.Default;
        }

        private void SharpMapOGRForm_Load(object sender, EventArgs e)
        {
            // initialize local things
            InitializeMyComponents();

        }
    }
}