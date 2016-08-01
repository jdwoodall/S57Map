using System;
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

            S57.Initialize();
        }

        private void S57MapForm_Closing()
        {
            DebugUtil.Close();
        }

        private void initializeToolStripMenuItem_Click(object sender, EventArgs e)
        {
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