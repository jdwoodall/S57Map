using System.IO;
using System.Windows.Forms;

namespace S57Map
{
    public class DebugUtil
    {
        // debug config items with their defaults
        internal static bool _debug = false;

        internal static bool _dbgConsole = false;
        internal static bool _json = false;
        internal static bool _dbgFile = false;
        internal static bool _dbgTree = false;

        //File and Stream for debug file
        private static FileStream debugFile;

        internal static StreamWriter debugStream;
        internal static string debugPath = @"S57debug.txt";

        //
        // allows the user to set a few options.  The defaults are set in the declarations at the
        // top of the file.
        //
        public static void Configure(bool debug, bool dbgFile, bool dbgConsole, bool json, bool dbgTree)
        {
            _debug = debug;
            _dbgFile = dbgFile;
            _json = json;
            _dbgConsole = dbgConsole;
            _dbgTree = dbgTree;
        }

        //
        // simple method to allow us to write to either the console or a file or both
        // if console output is turned on", the application is going to get very slow
        //
        public static void Write(string s)
        {
            if (_debug)
            {
                if (_dbgConsole)
                {
                    System.Console.Write(s);
                }

                if (_dbgFile)
                {
                    debugStream.Write(s);
                }
            }
        }

        public static void WriteLine(string s)
        {
            if (_debug)
            {
                if (_dbgConsole)
                {
                    System.Console.WriteLine(s);
                }

                if (_dbgFile)
                {
                    debugStream.WriteLine(s);
                }
            }
        }

        public static void WriteLine()
        {
            if (_debug)
            {
                if (_dbgConsole)
                {
                    System.Console.WriteLine();
                }

                if (_dbgFile)
                {
                    debugStream.WriteLine();
                }
            }
        }

        public static bool Initialize()
        {
            // if debug is turned on", initialize the file
            if (_debug && _dbgFile)
            {
                // create a new output file each time", otherwise it gets very large
                if (File.Exists(debugPath))
                {
                    try
                    {
                        File.Delete(debugPath);
                    }
                    catch (System.Exception)
                    {
                        MessageBox.Show("Debug file deletion error.  Close the file and select OK.");
                    }
                    finally
                    {
                        File.Delete(debugPath);
                        debugFile = File.Open(debugPath, FileMode.OpenOrCreate, FileAccess.Write);
                        debugStream = new StreamWriter(debugFile);
                    }
                }
                else
                {
                    debugFile = File.Open(debugPath, FileMode.OpenOrCreate, FileAccess.Write);
                    debugStream = new StreamWriter(debugFile);
                }

                // check to make sure it is open for writing
                if (debugFile.CanWrite)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
                return false;
        }

        public static void Close()
        {
            if (_debug && _dbgFile)
            {
                debugStream.Flush();
                debugFile.Flush();
                debugStream.Close();
                debugFile.Close();
            }
        }
    }
}