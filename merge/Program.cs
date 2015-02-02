using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using MergeLib;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MergeLibTest")]
namespace merge
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                foreach (string s in File.ReadAllLines("readme.txt",Encoding.GetEncoding("windows-1251")))
                    Console.WriteLine(s);
            }
            else if (args.Length == 4)
            {
                if (File.Exists(args[0]) && File.Exists(args[1]) && File.Exists(args[2]))
                {
                    string[] outputFile;
                    MergeLib.Merger merger = new MergeLib.Merger(File.ReadAllLines(args[0]), File.ReadAllLines(args[1]), File.ReadAllLines(args[2]));
                    merger.merge(out outputFile, true);
                    File.WriteAllLines(args[3], outputFile);
                }
                    
            }

            //Console.ReadLine();
        }
    }
}
