using OSGeo.OGR;
using SharpMap;
using SharpMap.Data;
using SharpMap.Layers;
using System.Windows.Forms;

namespace S57Map
{
    public partial class S57
    {
        /// <summary>
        /// This is the data structure we are going to build for each layer.  It needs to be homogeneous for all display layers.
        ///
        /// </summary>
        public class S57Layer
        {
            internal string layerName;
            internal int layercode;
            internal S52.ColorName fillColor;
            internal S52.ColorName outlineColor;
        }

        /// <summary>
        /// Render is where most of the work is done.  It builds a list of graphics items to be displayed and calls the S-52 rendorer to actually display them.
        /// We could pass the layer, but that would mean that the S-52 code would have to decipher the layers.  It seems easier just to build a list and let S-52
        /// only take care of the colors, symbol, and tiling.
        ///
        /// The goal is to create one list for each layer in the S57 record and have S52 render that layer.
        ///
        /// S-52 will return a map of the collective layers to display.  This can then either be displayed or tiled or both.
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Map Render(DataSource ds)
        {
            // the the cursor to the little swirling circle
            Cursor.Current = Cursors.WaitCursor;

            S57Layer layerCollection = new S57Layer();
            SharpMap.Data.FeatureDataSet fds = new FeatureDataSet();

            VectorLayer vectorLayer;
            Map map = new Map();

            int featureCount;
            //int iFeatureCount;
            int fieldIndex;
            string fieldValue;

            SharpMap.Data.Providers.Ogr dataProvider;
            //SharpMap.Layers.Layer thisLayer;

            OSGeo.OGR.Feature thisFeature;
            OSGeo.OGR.FeatureDefn featureDefn;
            OSGeo.OGR.FieldDefn fieldDefn;
            OSGeo.OGR.Layer thisLayer;

            Geometry geometry;
            Geometry sub_geometry;
            string WKTGeometry;

            DebugUtil.WriteLine("****************************************************************************");
            DebugUtil.WriteLine("***                        Render                                        ***");
            DebugUtil.WriteLine("****************************************************************************");

            // get the datasource that is currently open
            // ds = S57.DataSource;

            // we are only displaying the layers that our config data says to.
            int count = S57.myS57Objects.GetCount();

            for (int i = 0; i < count; i++)
            {
                if (myS57Objects.GetDisplayFlag(i))
                {
                    layerCollection.layerName = myS57Objects.S57ObjectsList[i].ObjectName;
                }
            }

            for (int iLayer = 0; iLayer < ds.GetLayerCount(); iLayer++)
            {
                thisLayer = DataSource.GetLayerByIndex(iLayer);
                DebugUtil.WriteLine("***  Layer = " + thisLayer.GetName() + "Layer Number: " + iLayer);

                featureCount = thisLayer.GetFeatureCount(0);  // a value of zero will get current feature count.  a value of 1 will force a feature count and is slow
                DebugUtil.WriteLine("***  Feature Count: " + featureCount);

                // FeatureDefn pointer
                featureDefn = thisLayer.GetLayerDefn();
                DebugUtil.WriteLine("***  Feature Definition Name: " + featureDefn.GetName() + "Field Count: " + featureDefn.GetFieldCount());

                // allocate a feature set for this layer based on featureDefn
                thisFeature = new Feature(featureDefn);
                DebugUtil.WriteLine("*** thisFeature Definition Reference: " + thisFeature.GetDefnRef());

                // get a SharpMap dataprovider for this vector layer.  this is needed before we
                // can render the file
                dataProvider = new SharpMap.Data.Providers.Ogr(S57.FileName, iLayer);

                // make sure the dataprovider is open
                if (!dataProvider.IsOpen)
                {
                    dataProvider.Open();
                }

                //featureCount = thisLayer.GetFeatureCount(0);
                //for (iFeatureCount = 0; iFeatureCount < featureCount; iFeatureCount++)

                // reset the current feature to 0 then read all the features
                thisLayer.ResetReading();
                while ((thisFeature = thisLayer.GetNextFeature()) != null)
                {
                    if (thisFeature == null)
                    {
                        MessageBox.Show("thisFeature is null and can not be processed.");
                        System.Environment.Exit(-1);
                    }
                    DebugUtil.WriteLine("Field count from thisFeature: " + thisFeature.GetFieldCount().ToString());

                    // Get the fields we want using the fieldList.  This avoids process fields that have nothing to do with display.
                    foreach (string thisField in fieldList)
                    {
                        // check each feature for the key attributes and add them to the tree if they exist.
                        if ((fieldIndex = thisFeature.GetFieldIndex(thisField)) >= 0)
                        {
                            DebugUtil.WriteLine("Field = " + thisField + ".  Field Index = " + fieldIndex);
                            fieldDefn = featureDefn.GetFieldDefn(fieldIndex);
                            fieldValue = DecodeField(thisFeature, fieldDefn, fieldIndex);

                            // valid field values are never 0 or empty
                            if (fieldValue != null && fieldValue != "" && fieldValue != "0")
                            {
                                // this gets the sub-geometries.  not all features have sub-geometries
                                geometry = thisFeature.GetGeometryRef();

                                // only render things that actually have geometries
                                if (geometry != null)
                                {
                                    // a bit confusing, but if it has no sub geometries, use the base geometry.  mostly applies to the type POINT
                                    if (geometry.GetGeometryCount() == 0)
                                    {
                                        geometry.ExportToWkt(out WKTGeometry);
                                        vectorLayer = new VectorLayer(thisLayer.GetName(), dataProvider);
                                        DebugUtil.WriteLine("Rendering: " + WKTGeometry);
                                        //vectorLayer = S52.Render(ref vectorLayer, dataProvider, thisFeature, fieldDefn, WKTGeometry);
                                        vectorLayer = S52.Render(thisLayer.GetName(), fds);
                                        map.Layers.Add(vectorLayer);
                                        //vectorLayer.Dispose();
                                    }
                                    else
                                    {
                                        for (int i = 0; i < geometry.GetGeometryCount(); i++)
                                        {
                                            sub_geometry = geometry.GetGeometryRef(i);
                                            if (sub_geometry != null)
                                            {
                                                sub_geometry.ExportToWkt(out WKTGeometry);
                                                vectorLayer = new VectorLayer(thisLayer.GetName(), dataProvider);
                                                DebugUtil.WriteLine("Rendering: " + WKTGeometry);
                                                //vectorLayer = S52.Render(ref vectorLayer, dataProvider, thisFeature, fieldDefn, WKTGeometry);
                                                vectorLayer = S52.Render(thisLayer.GetName(), fds);
                                                map.Layers.Add(vectorLayer);
                                                //vectorLayer.Dispose();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // free the feature collection for this layer
                //thisFeature.Dispose();
            }

            map.ZoomToExtents();

            //Matrix mat = new Matrix();
            //mat.RotateAt(angle, map.WorldToImage(map.Center));
            //map.MapTransform = mat;

            // change back to the defaul cursor
            Cursor.Current = Cursors.Default;
            return map;
        }
    }
}