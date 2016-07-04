using OSGeo.OGR;
using System.Windows.Forms;

namespace SharpMap_OGR_Test_v2
{
    public partial class S57
    {
        /* --------------------------------------------------------------------
        *      Reading the Field Definitions.
        *
        *  These define how to interpret the Features (next section).  They do
        *  do not need to be added to the tree, but are used to interpret the
        *  Features.
        *
        * --------------------------------------------------------------------*/

        protected void Features(TreeNode parentLayerTree, Layer thisLayer)
        {
            FeatureDefn featureDefn;

            // FeatureDefn pointer
            featureDefn = thisLayer.GetLayerDefn();

#if false
            // there is litte point in adding the field defintions to the tree or the output files other than diagnostics
            // if you want to turn this on change or remove the #if/#endif

            FieldDefn fieldDefn;        // variable for local fields

            // DebugUtil.WriteLine("   ***  Field Definitions  ***");
            for (int iAttr = 0; iAttr < featureDefn.GetFieldCount(); iAttr++)
            {
                // get the collection of field information
                fieldDefn = featureDefn.GetFieldDefn(iAttr);

                // add a new local root node to hold this field and give it this field name

                thisFieldRoot = new TreeNode(fieldDefn.GetName());

                TreeUtil.AddChildNode(thisFieldRoot, "Field Name", fieldDefn.GetName());
                TreeUtil.AddChildNode(thisFieldRoot, "Name Ref", fieldDefn.GetNameRef());
                TreeUtil.AddChildNode(thisFieldRoot, "Type Name", fieldDefn.GetTypeName());
                TreeUtil.AddChildNode(thisFieldRoot, "Field Type", fieldDefn.GetFieldType().ToString());
                TreeUtil.AddChildNode(thisFieldRoot, "Width", fieldDefn.GetWidth().ToString());
                TreeUtil.AddChildNode(thisFieldRoot, "Precidsion", fieldDefn.GetPrecision().ToString());
                fieldDefn.GetTypeName();
                thisLayerRoot.Nodes.Add(thisFieldRoot);

                DebugUtil.WriteLine("   " + "NameRef: " + fieldDefn.GetNameRef() + ": " +
                    "Type: " + fieldDefn.GetFieldType() +
                    "Type Name: " + fieldDefn.GetFieldTypeName(fieldDefn.GetFieldType()) + " (" +
                    fieldDefn.GetWidth() + "." +
                    fieldDefn.GetPrecision() + ")");
        }
#endif

            /* -------------------------------------------------------------------- */
            /*      Reading the Features                                            */
            /* -------------------------------------------------------------------- */

            Feature feature = new Feature(featureDefn);
            TreeNode featureTree;

            while ((feature = thisLayer.GetNextFeature()) != null)
            {
                // create the subtree and name it
                featureTree = new TreeNode();
                featureTree.Name = featureDefn.GetName();
                featureTree.Text = featureTree.Name;

                // get the details
                ReportFeature(ref featureTree, feature, featureDefn);

                // add the subtree
                parentLayerTree.Nodes.Add(featureTree);
                feature.Dispose();
            }
        }

        /* -------------------------------------------------------------------- */
        /*      Reading the Fields in this Feature                              */
        /* -------------------------------------------------------------------- */

        public static void ReportFeature(ref TreeNode thisFeatureNode, Feature feature, FeatureDefn featureDefn)
        {
            // field definition as we iterate through the fields
            FieldDefn iFieldDefn;
            string fieldNameRef;

            DebugUtil.WriteLine();
            DebugUtil.WriteLine("   *** Feature (" + featureDefn.GetName() + ") FID: " + feature.GetFID());
            DebugUtil.WriteLine();

            TreeUtil.AddChildNode(thisFeatureNode, "Feature Name", featureDefn.GetName());
            TreeUtil.AddChildNode(thisFeatureNode, "Feature ID", feature.GetFID().ToString());

            // get the styles, if any
            if (feature.GetStyleString() != null)
                DebugUtil.WriteLine("  Style = " + feature.GetStyleString());

            // this gets the sub-geometries.  not all fatures have sub geometries
            Geometry geom = feature.GetGeometryRef();
            if (geom != null)
            {
                DebugUtil.WriteLine("   Geometry Name:" + geom.GetGeometryName() + " Sub-geometry Count: " + geom.GetGeometryCount());

                Geometry sub_geom;
                for (int i = 0; i < geom.GetGeometryCount(); i++)
                {
                    sub_geom = geom.GetGeometryRef(i);
                    if (sub_geom != null)
                    {
                        DebugUtil.Write("   sub-geometry " + i + ": ");
                        switch (sub_geom.GetGeometryType())
                        {
                            case wkbGeometryType.wkbLineString:
                                string subGeoString;
                                sub_geom.ExportToWkt(out subGeoString);
                                DebugUtil.WriteLine(subGeoString);
                                break;

                            default:
                                DebugUtil.WriteLine("Unhandled subgeometry type: " + sub_geom.GetGeometryType());
                                break;
                        }
                    }
                }

                // a bit confusing, but if it has no sub geometries, use the base geometry.  mostly applies to the type POINT
                if (geom.GetGeometryCount() == 0)
                {
                    string geom_wkt;
                    geom.ExportToWkt(out geom_wkt);
                    DebugUtil.WriteLine("   geom_wkt: " + geom_wkt);
                }
            }  // if geo != null

            for (int iField = 0; iField < feature.GetFieldCount(); iField++)
            {
                iFieldDefn = featureDefn.GetFieldDefn(iField);

                // not all fields have usable information, skip those that don't.
                if (feature.IsFieldSet(iField))
                {
                    DebugUtil.Write("   ");
                    DebugUtil.Write("Field Name: " + iFieldDefn.GetName() + ", ");
                    DebugUtil.Write("Type Name: " + iFieldDefn.GetFieldTypeName(iFieldDefn.GetFieldType()) + ", ");
                    DebugUtil.Write("Field Type: " + iFieldDefn.GetFieldType() + ", ");
                    DebugUtil.Write("Value: ");

                    //  have to interpret each data type in order to properly get the values
                    switch (iFieldDefn.GetFieldType())
                    {
                        case FieldType.OFTStringList:
                            fieldNameRef = iFieldDefn.GetNameRef();
                            string[] sList = feature.GetFieldAsStringList(iField);
                            foreach (string s in sList)
                            {
                                DebugUtil.Write("(" + s + ") ");
                            }
                            break;

                        case FieldType.OFTIntegerList:
                            {
                                int count;
                                int[] iList = feature.GetFieldAsIntegerList(iField, out count);
                                // change to foreach
                                for (int i = 0; i < count; i++)
                                {
                                    DebugUtil.Write("(" + iList[i] + ") ");
                                }
                                break;
                            }

                        case FieldType.OFTRealList:
                            {
                                int count;
                                double[] iList = feature.GetFieldAsDoubleList(iField, out count);
                                for (int i = 0; i < count; i++)
                                {
                                    DebugUtil.Write("(" + iList[i].ToString() + ") ");
                                }
                                break;
                            }

                        case FieldType.OFTInteger:
                            {
                                DebugUtil.Write("(" + feature.GetFieldAsInteger(iField).ToString() + ")");
                                break;
                            }

                        case FieldType.OFTString:
                            {
                                DebugUtil.Write("(" + feature.GetFieldAsString(iField) + ")");
                            }
                            break;

                        case FieldType.OFTReal:
                            {
                                DebugUtil.Write("(" + feature.GetFieldAsDouble(iField).ToString() + ")");
                            }
                            break;

                        default:
                            {
                                DebugUtil.Write("Unhandled Field Type: " + iFieldDefn.GetFieldType());
                            }
                            break;
                    }  // switch

                    DebugUtil.WriteLine();
                }  // if isField
            }
        }
    }
}