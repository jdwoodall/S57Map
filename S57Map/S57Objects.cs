using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace S57Map
{
    internal class S57Objects
    {
        // this class holds the various fields that describe S-57 objects.  These are the same as the OGR Layer Names.  The Object Code is the
        // same as the OBJL member in OGR.
        internal class S57Object
        {
            internal string ObjectName;
            internal int ObjectCode;
            internal string ObjectDescription;
            internal int ObjectColor;
            internal bool ObjectDisplay;
        }

        internal List<S57Object> S57ObjectsList = new List<S57Object>();

        //
        // constructor class - does all the initialization
        //
        internal S57Objects()
        {
            string csvPath;

            // this path needs to go in the config file and option panel
            csvPath = System.AppDomain.CurrentDomain.BaseDirectory;
            csvPath = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\S-57 Object Names.csv"));
            //csvPath = @"d:\Users\dwoodall\Documents\Visual Studio 2015\Projects\S57Map\S57Map\S-57 Object Names.csv";

            // there are a lot of ways to do this.  it would probably be better to use and XML file for this
            // but for now I will use a CSV file.
            if (File.Exists(csvPath))
            {
                using (TextFieldParser parser = new TextFieldParser(csvPath))
                {
                    parser.CommentTokens = new string[] { "#" };
                    parser.SetDelimiters(new string[] { "\t" });
                    parser.HasFieldsEnclosedInQuotes = false;

                    // Skip over header line.
                    parser.ReadLine();


                    while (!parser.EndOfData)
                    {
                        S57Object tempObject = new S57Object();
                        string[] fields = parser.ReadFields();
                        tempObject.ObjectName = fields[0];
                        tempObject.ObjectCode = Convert.ToInt32(fields[1]);
                        tempObject.ObjectDescription = fields[2];
                        tempObject.ObjectColor = Convert.ToInt32(fields[3]);
                        tempObject.ObjectDisplay = Convert.ToBoolean(fields[4]);
                        S57ObjectsList.Add(tempObject);
                    }
                }
            }
            else
            {
                MessageBox.Show("Can not find file " + csvPath);
            }
        }

        internal S57Object FindObjectName(string objectName)
        {
            return S57ObjectsList.Find(x => x.ObjectName.Equals(objectName));
        }

        internal S57Object FindObjectCode(int objectCode)
        {
            return S57ObjectsList.Find(t => t.ObjectCode.Equals(objectCode));
        }

        internal bool SetDisplayFlag(string objectName)
        {
            S57Object tempObject;

            tempObject = S57ObjectsList.Find(x => x.ObjectName.Equals(objectName));

            if (tempObject != null)
            {
                tempObject.ObjectDisplay = true;
                return true;
            }
            else
                return false;
        }

        internal bool GetDisplayFlag(string objectName)
        {
            S57Object tempObject;

            tempObject = S57ObjectsList.Find(x => x.ObjectName.Equals(objectName));

            if (tempObject != null)
            {
                return tempObject.ObjectDisplay;
            }
            else
                return false;
        }
    }
}