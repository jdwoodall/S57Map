using OSGeo.OGR;
using System.Windows.Forms;

namespace S57Map
{
    public partial class S57
    {
        //
        // This is the list of all the fields we will collect data on and render.  They will not exist for all features. They are not order dependent except
        // that the order they are in is the the order they will be in the tree.  I try to keep the sorted.
        //
        static internal string[] fieldList = { "BCNSHP", "CATACH", "CATCBL", "CATOBJ", "CATWRK", "CATZOC", "COLOUR", "COLPAT", "DRVAL1", "DRVAL2", "EXPSOU", "HORCLR",
                "HEIGHT", "INFORM", "OBJL", "SCAMAX", "SCAMIN", "SOUNDG", "STATUS", "TRAFIC", "TXTDSC", "VALSOU", "VERCCL", "VERCLR", "VERCOP", "VERDAT" };

        /* --------------------------------------------------------------------
        *      Reading the Field Definitions.
        *
        *  These define how to interpret the Features (next section).  They do
        *  do not need to be added to the tree, but are used to interpret the
        *  Features.
        *
        * --------------------------------------------------------------------*/

        protected static void CreateTreeFeatures(TreeNode parentLayerTree, Layer thisLayer)
        {
            FeatureDefn featureDefn;

            //
            // These are fields from the DSID layer.  They have odd names, but we need this data.
            //
            string[] DSIDList = {"DSID_EXPP","DSID_INTU","DSID_DSNM", "DSID_EDTN", "DSID_UPDN", "DSID_UADT", "DSID_ISDT", "DSID_STED", "DSID_PRSP", "DSID_PSDN",
                "DSID_PRED", "DSID_PROF", "DSID_AGEN", "DSID_COMT", "DSSI_DSTR", "DSSI_AALL", "DSSI_NALL", "DSSI_NOMR", "DSSI_NOCR", "DSSI_NOGR",   "DSSI_NOLR",
                "DSSI_NOIN", "DSSI_NOCN", "DSSI_NOED", "DSSI_NOFA", "DSPM_HDAT", "DSPM_VDAT", "DSPM_SDAT", "DSPM_CSCL", "DSPM_DUNI", "DSPM_HUNI", "DSPM_PUNI",
                "DSPM_COUN", "DSPM_COMF", "DSPM_SOMF", "DSPM_COMT" };

            // This is the list of feature fields used to contain names.  Not all objects are nameed.  Some that are not named have useful comments.
            // This is order dependenet.
            string[] featureNameList = { "OBJNAM", "CATSEA", "CATLND" };

            Feature feature;
            TreeNode featureTree;
            FieldDefn fieldDefn;
            int objIndex;                       // index used to search within fields
            string fieldName;                   // name of a specific field
            string fieldValue;                  // value for a specific field

#if false
            // there is litte point in adding the field defintions to the tree or the output files other than diagnostics
            // if you want to turn this on change or remove the #if/#endif.  FYI it makes bot the file and the tree much larger
            // and therefore slower to build and display.

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
                TreeUtil.AddChildNode(thisFieldRoot, "Precision", fieldDefn.GetPrecision().ToString());
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

            // Get the FeatureDefn pointer for this layer
            featureDefn = thisLayer.GetLayerDefn();

            // get the feature list for this layer
            feature = new Feature(featureDefn);

            while ((feature = thisLayer.GetNextFeature()) != null)
            {
                // create the subtree and name it
                featureTree = new TreeNode();

                // get the field name.  Most lights have names as do some bouys, bridges and other landmarks.  The featureNameList controls the fields
                // we look for names in.  Most feature do NOT have names.
                fieldName = "";
                foreach (string field in featureNameList)
                {
                    if ((objIndex = featureDefn.GetFieldIndex(field)) >= 0)
                    {
                        // if the name is already found, do not reassign it
                        if (fieldName == "")
                        {
                            fieldDefn = featureDefn.GetFieldDefn(objIndex);
                            fieldName = DecodeField(feature, fieldDefn, objIndex);
                            if (fieldName != null && fieldName != "")
                            {
                                featureTree.Name = fieldName;
                                featureTree.Text = featureTree.Name;
                            }
                        }
                    }
                }

                // if no name is found above, assign it "unnamed" unless the layer is DSID
                if (fieldName == "" & thisLayer.GetName() != "DSID")
                {
                    fieldName = "Unnamed";
                    featureTree.Name = fieldName;
                    featureTree.Text = featureTree.Name;
                }

                // DSID has to be handled differently. It has not displayable data and has it's own fieldlist
                if (thisLayer.GetName() == "DSID")
                {
                    foreach (string field in DSIDList)
                    {
                        if ((objIndex = featureDefn.GetFieldIndex(field)) >= 0)
                        {
                            fieldDefn = featureDefn.GetFieldDefn(objIndex);
                            fieldValue = DecodeField(feature, fieldDefn, objIndex);
                            // field values are never 0
                            if (fieldValue != null && fieldValue != "")
                            {
                                TreeUtil.AddChildNode(parentLayerTree, field, fieldValue);
                            }
                        }
                    }
                }
                else
                {
                    // The fieldList contains the names of the fields are interested in.  We do not want all fields as many of them are
                    // irrelevant to diplaying the image such as the agency names and quality of the data, etc.

                    foreach (string field in fieldList)
                    {
                        // check each feature for the key attributes and add them to the tree if they exist.
                        if ((objIndex = featureDefn.GetFieldIndex(field)) >= 0)
                        {
                            if (feature.IsFieldSet(objIndex))
                            {
                                fieldDefn = featureDefn.GetFieldDefn(objIndex);
                                fieldValue = DecodeField(feature, fieldDefn, objIndex);
                                // valid field values are never 0 or empty
                                if (fieldValue != null && fieldValue != "")
                                {
                                    TreeUtil.AddChildNode(featureTree, field, fieldValue);
                                }
                            }
                        }
                    }
                }

                // get the details of the fields.  This is mostly diagnostic code, but it does populate the tree with positional information
                CreateTreeFields(ref featureTree, feature, featureDefn);

                // add the subtree
                parentLayerTree.Nodes.Add(featureTree);
                feature.Dispose();
            }
        }

        /* -------------------------------------------------------------------- */
        /*      Reading the Fields in this Feature                              */
        /* -------------------------------------------------------------------- */

        public static void CreateTreeFields(ref TreeNode thisFeatureTree, Feature feature, FeatureDefn featureDefn)
        {
            // field definition as we iterate through the fields
            FieldDefn iFieldDefn;
            int nameIndex;
            int objlIndex;
            string nameString;
            string objlString;

            // To get the name and the S-57 Type code (OBJL), we have to look forward into the fields.  This piece of code 
            // is mostly here to show how that is done as will need this when rendering these objects.  You could easily add
            // shapes or color, but it is not necessary here.  The name and the code is nice for the debug file though so we can
            // see what each feature is.

            // OBJL field should always exist - the exception are the meta data fields such as DSID which has no code.
            objlIndex = featureDefn.GetFieldIndex("OBJL");
            if (objlIndex > 0)
                objlString = DecodeField(feature, featureDefn.GetFieldDefn(objlIndex), objlIndex);
            else
                objlString = "No Code";

            // name field may not exist.  we could get either the info or text description if they exist
            nameIndex = featureDefn.GetFieldIndex("OBJNAM");
            if (nameIndex > 0)
                nameString = DecodeField(feature, featureDefn.GetFieldDefn(nameIndex), nameIndex);
            else
                nameString = "Not Named";

            DebugUtil.WriteLine();
            DebugUtil.WriteLine(string.Format("   *** Feature: {0}, Name: {1}, S-57 Code: {2}", featureDefn.GetName(), nameString, objlString));
            DebugUtil.WriteLine();

            // get the styles, if any
            if (feature.GetStyleString() != null)
                DebugUtil.WriteLine("  Style = " + feature.GetStyleString());

            // this gets the sub-geometries.  not all features have sub-geometries
            Geometry geom = feature.GetGeometryRef();
            if (geom != null)
            {
                DebugUtil.WriteLine("   Geometry Name: " + geom.GetGeometryName() + " Sub-geometry Count: " + geom.GetGeometryCount());

                Geometry sub_geom;
                for (int i = 0; i < geom.GetGeometryCount(); i++)
                {
                    sub_geom = geom.GetGeometryRef(i);
                    if (sub_geom != null)
                    {
                        string subGeoString;
                        DebugUtil.Write("   sub-geometry " + i + ": ");
                        switch (sub_geom.GetGeometryType())
                        {
                            case wkbGeometryType.wkbLineString:
                            case wkbGeometryType.wkbPoint25D:
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
                if (feature.IsFieldSet(iField))  // IsFieldSet is a handy way to see if the field has any actual information
                {
                    DebugUtil.Write("   ");
                    DebugUtil.Write("Field Name: " + iFieldDefn.GetName() + ", ");
                    DebugUtil.Write("Type Name: " + iFieldDefn.GetFieldTypeName(iFieldDefn.GetFieldType()) + ", ");
                    DebugUtil.Write("Field Type: " + iFieldDefn.GetFieldType() + ", ");
                    DebugUtil.Write("Value: " + DecodeField(feature, iFieldDefn, iField));
                    DebugUtil.WriteLine();
                }  // if isField
            }
        }
    }
}