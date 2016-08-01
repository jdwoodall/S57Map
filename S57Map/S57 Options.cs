using System;
using System.Windows.Forms;
using static S57Map.S57Objects;

namespace S57Map
{
    public partial class S57Options : Form
    {
        public S57Options()
        {
            InitializeComponent();
            ShowS57Objects();

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

        private void ShowS57Objects()
        {
            string[] tString = new string[10];
            S57Object tObject;
            int thisIndex;

            int count = S57.myS57Objects.GetCount();

            for (int i = 0; i < count; i++)
            {
                tObject = S57.myS57Objects.GetObject(i);

                tString[0] = tObject.ObjectName;
                tString[1] = tObject.ObjectDescription;
                tString[2] = tObject.ObjectCode.ToString(); ;
                tString[3] = tObject.ObjectColor.ToString(); ;
                tString[4] = tObject.ObjectDisplay.ToString();
                
                thisIndex = dataGridView1.Rows.Add(tString);
                dataGridView1.Rows[thisIndex].HeaderCell.Value = tObject.ObjectName;
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            // needed to extract the actual values
            DataGridViewRow row;
            row = dataGridView1.Rows[e.RowIndex];

            // this keeps us from acting on someone clicking the header -- do nothing in that case
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                DataGridViewCheckBoxCell cell = row.Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;

                // column 4 is the display flag which is a boolean value
                if (e.ColumnIndex == 4)
                {
                    // make sure we have a valid value
                    if (cell.Value != null)
                    {
                        dataGridView1.BeginEdit(false);

                        // update the check box as well as the data structure it comes from
                        if (cell.Value == cell.TrueValue)
                        {
                            cell.Value = cell.FalseValue;
                            S57.myS57Objects.SetDisplayFlag(e.RowIndex, false);
                        }
                        else
                        {
                            cell.Value = cell.TrueValue;
                            S57.myS57Objects.SetDisplayFlag(e.RowIndex, true);
                        }

                        // update the DGV...turn off edit mode
                        dataGridView1.EndEdit();
                    }
                }
            }
        }
    }
}