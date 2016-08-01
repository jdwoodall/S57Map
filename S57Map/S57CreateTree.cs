using OSGeo.OGR;
using System;
using System.IO;
using System.Windows.Forms;

namespace S57Map
{
    partial class S57
    {

        /// <summary>
        /// Iterates through the layers of the map and builds a tree based on the layer information
        /// </summary>
        /// <param name="ds"></param>
        /// <returns>"TreeNode"</returns>
        ///
        public static TreeNode CreateTree()
        {

            DebugUtil.WriteLine();
            DebugUtil.WriteLine("*******************************************************************************************************");
            DebugUtil.WriteLine("****                                      Building Tree                                            ****");
            DebugUtil.WriteLine("*******************************************************************************************************");
            DebugUtil.WriteLine();


            TreeNode topNode = new TreeNode();
            TreeNode metaTree = new TreeNode();
            Layer layer;
            string srs_wkt;
            Envelope thisEnvelope = new Envelope();
            string coordinateString;

            // add the name of the chart as the name of this root
            topNode.Text = Path.GetFileName(ds.name);
            topNode.Name = Path.GetFileName(ds.name);

            // Meta Tree
            metaTree.Text = "Meta Data";
            metaTree.Name = "Meta Data";

            // get layer 0 (should be "DSID) for the meta data
            layer = ds.GetLayerByIndex(0);

            /* -------------------------------------------------------------------- */
            /* Get the spatial reference.  It is consistent for the entire map so we*/
            /* only need to do it once.                                             */
            /* -------------------------------------------------------------------- */

            OSGeo.OSR.SpatialReference sr = layer.GetSpatialRef();
            // this should not be null
            if (sr != null)
            {
                sr.ExportToPrettyWkt(out srs_wkt, 1);
                TreeUtil.AddChildNode(metaTree, "Spatial Reference", srs_wkt);
                DebugUtil.WriteLine("Spatial Reference: " + srs_wkt);
            }
            else
            {
                TreeUtil.AddChildNode(metaTree, "Spatial Reference", "Spatial Reference Uknown");
                srs_wkt = "SRS: Unknown.";
            }

            // get Envelope for this layer
            layer.GetExtent(thisEnvelope, 1);

            // Get the Extents for the map.  Again, they are consistent for the entire map, so we only need to do this once.
            if (thisEnvelope != null)
            {
                coordinateString = String.Format("MaxX = {0}, MaxY = {1}, MinX = {2}, MinY = {3}.", thisEnvelope.MaxX, thisEnvelope.MaxY, +thisEnvelope.MinX, thisEnvelope.MinY);
                DebugUtil.WriteLine("Extents: " + coordinateString);
                TreeUtil.AddChildNode(metaTree, "Envelope: ", coordinateString);
            }

            // add metaTree to topNode
            topNode.Nodes.Add(metaTree);

            // go through all the layers starting at zero
            for (int i = 0; i < ds.GetLayerCount(); i++)
            {
                layer = ds.GetLayerByIndex(i);
                if (layer != null)
                {
                    // most of the work is done here
                    CreateTreeLayers(topNode, layer, i);
                }
                else
                {
                    DebugUtil.WriteLine("***********    Critical Error     *************");
                    DebugUtil.WriteLine("Layer: " + i + " is null and does not exist.");
                    MessageBox.Show("Critical Error: " + "Layer: " + i + " is null and does not exist.");
                }
            }

            DebugUtil.WriteLine("   TopNode node count is " + topNode.Nodes.Count + ".");

            // save the tree to a file.  this is controlled by the debug flags.  note that the file can get very large
            TreeUtil.OpenPrintRecursive("TreeNode.txt");
            TreeUtil.PrintRecursive(topNode);

            // close the debug files
            DebugUtil.Close();
            TreeUtil.Close();

            return topNode;
        }

        //
        //  build a root node for the layer then add child nodes to that root before adding the root
        //  to the topNode
        //

        private static void CreateTreeLayers(TreeNode parentTree, Layer thisLayer, int layerNumber)
        {
            string thisLayerName;

            DebugUtil.WriteLine();
            DebugUtil.WriteLine("++++++      New Layer: " + thisLayer.GetName()
                + ". Layer Number: " + layerNumber
                + ". Feature Count: " + thisLayer.GetFeatureCount(1).ToString()
                + "      +++++");
            DebugUtil.WriteLine();

            // build sub-trees by layer.  Under that there will be sub-treas for Features and Fields
            TreeNode thisLayerTree = new TreeNode();

            // create a new root node for this layer and name it
            thisLayerName = thisLayer.GetName();
            thisLayerTree.Name = thisLayerName;
            thisLayerTree.Text = thisLayerName;
            //TreeUtil.AddChildNode(thisLayerTree, "Feature Count = " + thisLayer.GetFeatureCount(1).ToString());

            // get the features
            CreateTreeFeatures(thisLayerTree, thisLayer);

            // add this layer to the parent tree
            parentTree.Nodes.Add(thisLayerTree);
        }
    }

}

