using System;
using System.IO;

namespace SharpMap_OGR_Test_v2
{
    public class DebugUtil
    {
        // debug config items with their defaults
        private static bool _debug = true;

        private static bool _dbgConsole = false;
        private static bool _json = false;
        private static bool _dbgFile = true;
        internal static bool _dbgTree = false;

        //File and Stream for debug file
        private static FileStream debugFile;

        private static StreamWriter debugStream;
        private static string debugPath = @"S57debug.txt";

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
        // if console output is turned on, the application is going to get very slow
        //
        public static void Write(string s)
        {
            if (_debug)
            {
                if (_dbgConsole)
                {
                    DebugUtil.Write(s);
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
                    DebugUtil.WriteLine(s);
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
                    DebugUtil.WriteLine();
                }

                if (_dbgFile)
                {
                    debugStream.WriteLine();
                }
            }
        }

        public static void Initialize()
        {
            // create a new output file each time, otherwise it gets very large
            if (File.Exists(debugPath))
            {
                File.Delete(debugPath);
            }

            //File.Open(debugPath, FileMode.OpenOrCreate, FileAccess.Write);

            // if debug is turned on, initialize the file
            if (_debug && _dbgFile)
            {
                //File.Create(debugPath);
                debugFile = new FileStream(debugPath, FileMode.OpenOrCreate, FileAccess.Write);
                debugStream = new StreamWriter(debugFile);
            }
        }

        public static void Close()
        {
            if (_debug && _dbgFile)
            {

            }
        }
    }
}