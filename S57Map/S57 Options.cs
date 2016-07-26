using System;
using System.Windows.Forms;

namespace S57Map
{
    public partial class S57Options : Form
    {
        public S57Options()
        {
            InitializeComponent();

            // get the initial state of the buttons

            // master debug flag
            if (DebugUtil._debug)
            {
                cBoxDebug.Checked = true;
            }
            else
            {
                cBoxDebug.Checked = false;
            }

            // S57 text output flag
            if (DebugUtil._dbgFile)
            {
                cBoxS57Out.Checked = true;
            }
            else
            {
                cBoxS57Out.Checked = false;
            }

            // Tree text output flag
            if (DebugUtil._dbgTree)
            {
                cBoxTreeOut.Checked = true;
            }
            else
            {
                cBoxTreeOut.Checked = false;
            }


            // Console text output flag
            if (DebugUtil._dbgTree)
            {
                cBoxConsoleOut.Checked = true;
            }
            else
            {
                cBoxConsoleOut.Checked = false;
            }
        }

        // master debug flag
        private void cBoxDebug_CheckedChanged(object sender, EventArgs e)
        {
            if (cBoxDebug.Checked)
            {
                DebugUtil._debug = true;
            }
            else
            {
                DebugUtil._debug = false;
            }
        }

        // S57 text file output flag
        private void cBoxS57Out_CheckedChanged(object sender, EventArgs e)
        {
            if (cBoxS57Out.Checked)
            {
                DebugUtil._dbgFile = true;
                DebugUtil._debug = true;
                cBoxDebug.Checked = true;
            }
            else
            {
                DebugUtil._dbgFile = false;
            }
        }

        // tree text file output flag
        private void cBoxTreeOut_CheckedChanged(object sender, EventArgs e)
        {
            if (cBoxDebug.Checked)
            {
                DebugUtil._dbgTree = true;
            }
            else
            {
                DebugUtil._dbgTree = false;
            }
        }

        // console output flag
        private void cBoxConsoleOut_CheckedChanged(object sender, EventArgs e)
        {
            if (cBoxDebug.Checked)
            {
                DebugUtil._dbgConsole = true;
            }
            else
            {
                DebugUtil._dbgConsole = false;
            }
        }

        private void btnOptionsOK_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}