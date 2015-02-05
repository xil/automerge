using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using MergeLib;
using System.Runtime.CompilerServices;
using merge.Properties;

[assembly: InternalsVisibleTo("MergeLibTest")]
namespace merge
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            try
            {
                if (args.Length < 4)   // print help
                {
                    if (File.Exists("readme.txt"))
                    {
                        foreach (string s in File.ReadAllLines("readme.txt", Encoding.GetEncoding("windows-1251")))
                            Console.WriteLine(s);
                    }
                    else
                        Console.WriteLine(Resources.Readme);
                    Console.ReadLine();
                }
                else if (args.Length >= 4)  //work
                {
                    if (File.Exists(args[args.Length - 2]) &&
                        File.Exists(args[args.Length - 3]) &&
                        File.Exists(args[args.Length - 4]))
                    {

                        string message = "";
                        bool workFinished = false;
                        bool silence = args.Contains<string>("silent");
                        List<string> outputFile;
                        MergerFactory mf = new MergerFactory();
                        IMerger m = mf.getInstance(args.ToList<string>());
                        if (!silence)
                            m.ProgressChanged += m_ProgressChanged;

                        Thread thread = new Thread(delegate()
                            {
                                message = m.Merge(
                                        File.ReadAllLines(args[args.Length - 4]).ToList<string>(),
                                        File.ReadAllLines(args[args.Length - 3]).ToList<string>(),
                                        File.ReadAllLines(args[args.Length - 2]).ToList<string>(),
                                        out outputFile, true);
                                File.WriteAllLines(args[args.Length - 1], outputFile);
                                workFinished = true;
                            });
                        thread.Start();

                        if (silence)
                        {
                            while (!workFinished)
                            {
                                Thread.Sleep(500);
                            }
                        }
                        else
                        {
                            Console.WriteLine(Resources.InProgress);
                            while (!workFinished)
                            {
                                Thread.Sleep(99);
                            }
                            if (message != "")
                            {
                                Console.WriteLine(message);
                                Console.ReadLine();
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine(Resources.FileNotExists);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        static void m_ProgressChanged(object sender, EventArgs e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
