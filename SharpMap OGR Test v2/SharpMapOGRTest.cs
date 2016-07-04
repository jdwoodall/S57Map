using System;
using System.Windows.Forms;

//*******************************************************************************//
//
// This is the main code that mostly contains the windows generated items for the gui.
//  However, the initialization code for various components is here as well.

namespace SharpMap_OGR_Test_v2
{
    public partial class SharpMapOGRTest : Form
    {
        private S57 myS57;

        public SharpMapOGRTest()
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

        private void initializeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GdalConfiguration.ConfigureGdal();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fileName = @"c:\geodata\s57\US5TX51M.000";
            OSGeo.OGR.DataSource ds;

            myS57 = new S57();

            ds = myS57.Initialize(fileName, 0.0f);
            treeView1.BeginUpdate();
            treeView1.Nodes.Add(myS57.createTree(ds));
            // the sort method is VERY slow. Don't use it unless it is absolutely necessary to sort the data.
            // treeView1.Sort();
            treeView1.EndUpdate();
            treeView1.Show();
        }
    }
}