using OSGeo.OGR;
using System;
using System.IO;
using System.Windows.Forms;

// the code to span the map largely came from ogrinfo which is in gdal\csharp distribution

namespace SharpMap_OGR_Test_v2
{
    public partial class S57
    {
        private static DataSource ds;
        private static string _S57FileName;
        private static DataSource _S57DataSource;
        private static string _S57DataSourceName;
        private static Driver _S57Driver;

        // public functions to retrive DataSource,  DataSourceName, number of layer and driver
        public static DataSource S57DataSource => _S57DataSource;

        public static string S57DataSourceName => _S57DataSourceName;

        public static int S57NumberLayers() => ds.GetLayerCount();

        public static Driver S57Driver() => _S57Driver;

        //  simple function that only opens the driver and returns a pointer to the provider
        public DataSource Initialize(String fileName, float angle)
        {
            // initialize the debug stuff - check the defaults if you don't call DebugUtil.Config
            DebugUtil.Initialize();

            // save the dataset name
            _S57FileName = fileName;

            // register all the drivers so we can find the one we need - this is slow if debug output is on
            Ogr.RegisterAll();

            // open the specified fileName with update set to R/O (=0)
            ds = Ogr.Open(fileName, 0);
            if (ds == null)
            {
                MessageBox.Show("Ogr.Open returned a zero.  Make sure your file pathname is correct.");
                _S57DataSource = null;
                _S57FileName = "";
            }
            else
            {
                DebugUtil.WriteLine("DataSource is: " + ds.ToString());
                DebugUtil.WriteLine("DataSource Name is: " + ds.name.ToString());
                _S57DataSource = ds;
                _S57DataSourceName = ds.name;

                Driver drv = ds.GetDriver();
                _S57Driver = drv;

                if (drv == null)
                {
                    MessageBox.Show("S-57 driver is null.  Make sure your drivers are in the correct directory and GDAL_DRV points to them.");
                    ds = null;
                }
                else
                {
                    DebugUtil.WriteLine("Driver is: " + drv.ToString());
                }
            }

            return ds;
        }

        /// <summary>
        /// Iterates through the layers of the map and builds a tree based on the layer information
        /// </summary>
        /// <param name="ds"></param>
        /// <returns>"TreeNode"</returns>
        ///
        public TreeNode createTree(DataSource ds)
        {
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

            // get layer 0 (should be DSID) for the meta data
            layer = ds.GetLayerByIndex(0);

            /* -------------------------------------------------------------------- */
            /* Get the spatial reference.  It is consistent for the entire map so we*/
            /* only need to do it once.                                             */
            /* -------------------------------------------------------------------- */

            //  sr can be null.  this is not an error it only meant that there is no sr
            //  associated with this layer
            OSGeo.OSR.SpatialReference sr = layer.GetSpatialRef();

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
                    BuildLayers(topNode, layer, i);
                }
                else
                {
                    DebugUtil.WriteLine("***********    Critical Error     *************");
                    DebugUtil.WriteLine("Layer: " + i + " is null and does not exist.");
                    MessageBox.Show("Critical Error: " + "Layer: " + i + " is null and does not exist.");
                }
            }

            DebugUtil.WriteLine("TopNode node count is " + topNode.Nodes.Count + ".");

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

        private void BuildLayers(TreeNode parentTree, Layer thisLayer, int layerNumber)
        {
            string thisLayerName;

            DebugUtil.WriteLine();
            DebugUtil.WriteLine("++++++      New Layer: " + thisLayer.GetName() + ". Layer Number: " + layerNumber + "      +++++");
            DebugUtil.WriteLine();

            // build sub-trees by layer.  Under that there will be sub-treas for Features and Fields
            TreeNode thisLayerTree = new TreeNode();

            // create a new root node for this layer and name it
            thisLayerName = thisLayer.GetName();
            thisLayerTree.Name = thisLayerName;
            thisLayerTree.Text = thisLayerName;
            TreeUtil.AddChildNode(thisLayerTree, "Feature Count", thisLayer.GetFeatureCount(1).ToString());

            DebugUtil.WriteLine("Layer name: " + thisLayerName);
            DebugUtil.WriteLine("Feature Count: " + thisLayer.GetFeatureCount(1).ToString());

            // get the features
            Features(thisLayerTree, thisLayer);

            // add this layer to the parent tree
            parentTree.Nodes.Add(thisLayerTree);
        }  // BuildLayers
    }
}