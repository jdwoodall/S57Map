using System.IO;
using System.Windows.Forms;

namespace S57Map
{
    public class TreeUtil
    {
        // this is the StreamWriter for transversing the root.
        // if we add to the tree outpt  to the other diagnostic output file the
        // file gets really big.  best to have its own file.
        //
        // variables for StreamWriter util
        internal static StreamWriter prsw = null;

        internal static FileStream prfs = null;

        //
        // add a childnode with the text contained in "caption"
        // add another childnode under that (grandchild) to that that contains the data in "value"
        // of "value" is emptry, then skip that part
        //
        public static void AddChildNode(TreeNode parent, string caption, string value)
        {
            TreeNode thisChild, thisGrandChild;

            // add caption as a child node
            thisChild = new TreeNode();
            thisChild.Name = caption;
            thisChild.Text = caption;

            if (value != null && value != "")
            {
                // add values as a Grand Child
                thisGrandChild = new TreeNode();
                thisGrandChild.Name = value;
                thisGrandChild.Text = value;
                thisChild.Nodes.Add(thisGrandChild);
            }
            parent.Nodes.Add(thisChild);

#if false
            //generates a lot of output and is redundant if you are printing from your app
            DebugUtil.WriteLine("Child Node added. Caption:  " + caption);
            if (value != "")
            {
                DebugUtil.WriteLine("   Value: " + value + ".");
            }
#endif
        }

        // add child not only with no grandchild
        public static void AddChildNode(TreeNode parent, string caption)
        {
            TreeNode thisChild;

            // add caption as a child node
            thisChild = new TreeNode();
            thisChild.Name = caption;
            thisChild.Text = caption;

            parent.Nodes.Add(thisChild);

#if false
            //generates a lot of output and is redundant if you are printing from your app
            DebugUtil.WriteLine("Child Node added. Caption:  " + caption);
            if (value != "")
            {
                DebugUtil.WriteLine("   Value: " + value + ".");
            }
#endif
        }

        //
        // print all the nodes in the passed tree
        //
        public static void OpenPrintRecursive(string filename)
        {
            if (DebugUtil._dbgTree)
            {
                prfs = new FileStream(filename, FileMode.OpenOrCreate);
                prsw = new StreamWriter(prfs);
            }
        }

        public static void PrintRecursive(TreeNode treeNode)
        {
            if (DebugUtil._dbgTree)
            {
                // Print the Node
                prsw.WriteLine(treeNode.Text);

                // Print each node recursively.
                foreach (TreeNode tn in treeNode.Nodes)
                {
                    PrintRecursive(tn);
                }
            }
        }

        // Call the procedure using the TreeView.
        public static void CallRecursive(TreeView treeView)
        {
            if (DebugUtil._dbgTree)
            {
                // Print each node recursively.
                TreeNodeCollection nodes = treeView.Nodes;
                foreach (TreeNode n in nodes)
                {
                    PrintRecursive(n);
                }
            }
        }

        public static void Close()
        {
            if (prsw != null && DebugUtil._dbgTree)
            {
                prsw.Flush();
                prsw.Close();
            }
        }
    }
}