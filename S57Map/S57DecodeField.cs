using OSGeo.OGR;

namespace S57Map
{
    public partial class S57
    {
        /// <summary>
        /// Decode the fields of an S-57 record.  Always returns a string type.
        /// </summary>
        /// <param name="feature"></param>
        /// <param name="iFieldDefn"></param>
        /// <param name="iField"></param>
        /// <returns></returns>
        public static string DecodeField(Feature feature, FieldDefn iFieldDefn, int iField)
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