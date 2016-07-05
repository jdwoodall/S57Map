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
            FieldDefn fieldDefn;
            int objIndex;                       // index used to search within fields
            string fieldName;                   // name of a specific field
            string fieldValue;                  // value for a specific field

            //
            // this is a list of all the fields we will collect data on.  they will not exist for all features.  they are not order dependent except that the order they are in is the
            // the order they will be in the tree.  I try to keep the sorted.
            //
            string[] fieldList = { "BCNSHP", "CATCBL", "COLOUR", "DRVAL1", "DRVAL2", "HORCLR", "INFORM", "SCAMIN", "STATUS", "VERCCL", "VERCLR", "VERCOP" };

            // This is the list of feature fields used to contain names.  Not all objects are nameed.  Some that are not named have useful comments.  Otherwise use the FIDN
            // as their name.  This is order dependenet
            string[] featureNameList = { "OBJNAM", "INFORM", "FIDN" };  // these are ordered in the priority the names should be used.  THey have to exactly match the field names


            while ((feature = thisLayer.GetNextFeature()) != null)
            {
                // create the subtree and name it
                featureTree = new TreeNode();

                // get the field name.  Most lights have names as do some bouys, bridges and other landmarks.  If not name exist, use the INFORM field if that exist, other wise
                // uses the FIDN which is a unique number assigned to objects.
                fieldName = "";
                foreach (string field in featureNameList)
                {
                    if ((objIndex = featureDefn.GetFieldIndex(field)) >= 0)
                    {
                        // if the name is already found, do not reassign it
                        if (fieldName == "")
                        {
                            fieldDefn = featureDefn.GetFieldDefn(objIndex);
                            fieldName = decodeField(feature, fieldDefn, objIndex);
                            if (fieldName != null && fieldName != "")
                            {
                                featureTree.Name = fieldName;
                                featureTree.Text = featureTree.Name;
                            }
                        }
                    }
                }

                // if no name is found above, assign it "unnamed"
                if (fieldName == "")
                {
                    fieldName = "Unnamed";
                    featureTree.Name = fieldName;
                    featureTree.Text = featureTree.Name;
                }

#if false
                // get the object name of this feature.  it may not exist
                if ((objIndex = featureDefn.GetFieldIndex("OBJNAM")) >= 0)
                {
                    fieldName = "OBJNAME";
                    fieldDefn = featureDefn.GetFieldDefn(objIndex);
                    featureTree.Name = feature.GetFieldAsString(objIndex);
                    featureTree.Text = featureTree.Name;
                }
                else if ((objIndex = featureDefn.GetFieldIndex("INFORM")) >= 0)
                {
                    fieldName = "INFORM";
                    fieldDefn = featureDefn.GetFieldDefn(objIndex);
                    featureTree.Name = feature.GetFieldAsString(objIndex);
                    featureTree.Text = featureTree.Name;
                }
                else if ((objIndex = featureDefn.GetFieldIndex("FIDN")) >= 0)
                {
                    fieldName = "FIDN";
                    fieldDefn = featureDefn.GetFieldDefn(objIndex);
                    featureTree.Name = feature.GetFieldAsString(objIndex);
                    featureTree.Text = featureTree.Name;
                }
                else
                {
                    fieldName = "No Name";
                    featureTree.Name = fieldName;
                    featureTree.Text = featureTree.Name;
                }
#endif
                // check each feature for the key attributes and add them to the tree if they exist. 
                foreach (string field in fieldList)
                {
                    if ((objIndex = featureDefn.GetFieldIndex(field)) >= 0)
                    {
                        fieldDefn = featureDefn.GetFieldDefn(objIndex);
                        fieldValue = decodeField(feature, fieldDefn, objIndex);
                        if (fieldValue != null && fieldValue != "")
                        {
                            TreeUtil.AddChildNode(featureTree, field, fieldValue);
                        }
                    }
                }

                // get the details of the fields.  This is mostly diagnostic code, but it does populate the tree with positional information
                ReportFeature(ref featureTree, feature, featureDefn);

                // add the subtree
                parentLayerTree.Nodes.Add(featureTree);
                feature.Dispose();
            }
        }

        /* -------------------------------------------------------------------- */
        /*      Reading the Fields in this Feature                              */
        /* -------------------------------------------------------------------- */

        public static void ReportFeature(ref TreeNode thisFeatureTree, Feature feature, FeatureDefn featureDefn)
        {
            // field definition as we iterate through the fields
            FieldDefn iFieldDefn;

            DebugUtil.WriteLine();
            DebugUtil.WriteLine("   *** Feature (" + featureDefn.GetName() + ") FID: " + feature.GetFID());
            DebugUtil.WriteLine();

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
                                TreeUtil.AddChildNode(thisFeatureTree, subGeoString);
                                break;

                            default:
                                DebugUtil.WriteLine("Unhandled subgeometry type: " + sub_geom.GetGeometryType());
                                TreeUtil.AddChildNode(thisFeatureTree, "Unhandled sub-geomtry type", sub_geom.GetGeometryType().ToString());
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
                    TreeUtil.AddChildNode(thisFeatureTree, geom_wkt);
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
                    DebugUtil.Write("Value: " + decodeField(feature, iFieldDefn, iField));
                    DebugUtil.WriteLine();
                }  // if isField
            }
        }

        //
        // decodes the various field types.  Always returns a string type so cast it if you need something else
        //
        public static string decodeField(Feature feature, FieldDefn iFieldDefn, int iField)
        {
            string outString;
            //  have to interpret each data type in order to properly get the values

            outString = "";
            switch (iFieldDefn.GetFieldType())
            {
                case FieldType.OFTStringList:
                    string[] sList = feature.GetFieldAsStringList(iField);
                    foreach (string s in sList)
                    {
                        //DebugUtil.Write(" " + s + " ");
                        outString += s + ",";
                    }
                    break;

                case FieldType.OFTIntegerList:
                    {
                        int count;
                        int[] iList = feature.GetFieldAsIntegerList(iField, out count);
                        // change to foreach
                        for (int i = 0; i < count; i++)
                        {
                            //DebugUtil.Write("(" + iList[i] + ") ");
                            outString += iList[i].ToString() + " ";
                        }
                        break;
                    }

                case FieldType.OFTRealList:
                    {
                        int count;
                        double[] iList = feature.GetFieldAsDoubleList(iField, out count);
                        for (int i = 0; i < count; i++)
                        {
                            //DebugUtil.Write("(" + iList[i].ToString() + ") ");
                            outString += iList[i].ToString() + " ";
                        }
                        break;
                    }

                case FieldType.OFTInteger:
                    {
                        //DebugUtil.Write("(" + feature.GetFieldAsInteger(iField).ToString() + ")");
                        outString += feature.GetFieldAsInteger(iField).ToString();
                        break;
                    }

                case FieldType.OFTString:
                    {
                        //DebugUtil.Write("(" + feature.GetFieldAsString(iField) + ")");
                        outString += feature.GetFieldAsString(iField);
                    }
                    break;

                case FieldType.OFTReal:
                    {
                        //DebugUtil.Write("(" + feature.GetFieldAsString(iField) + ")");
                        outString += feature.GetFieldAsString(iField);
                    }
                    break;

                default:
                    {
                        //DebugUtil.Write("Unhandled Field Type: " + iFieldDefn.GetFieldType());
                        outString += "Unhandled Field Type: " + iFieldDefn.GetFieldType();
                    }
                    break;
            }  // switch

            return outString;
        }
    }
}