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
    /// <summary>
    /// Console app class
    /// </summary>
    internal static class Program
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
                    FileInfo fiA = new FileInfo(args[args.Length - 4]);
                    FileInfo fiB = new FileInfo(args[args.Length - 3]);
                    FileInfo fiO = new FileInfo(args[args.Length - 2]);

                    if (fiA.Exists && fiB.Exists && fiO.Exists)
                    {

                        string message = "";
                        bool workFinished = false;
                        bool silence = args.Contains("silent");
                        List<string> outputFile;
                        IMerger m = MergerFactory.GetInstance(args.ToList(), (int)Math.Max(Math.Max(fiA.Length,fiO.Length),fiB.Length));
                        if (!silence)
                            m.ProgressChanged += m_ProgressChanged;

                        Thread thread = new Thread(delegate()   // async merging call 
                            {
                                message = m.Merge(
                                        File.ReadAllLines(args[args.Length - 4]).ToList(),
                                        File.ReadAllLines(args[args.Length - 3]).ToList(),
                                        File.ReadAllLines(args[args.Length - 2]).ToList(),
                                        out outputFile);
                                File.WriteAllLines(args[args.Length - 1], outputFile);
                                workFinished = true;
                            });
                        thread.Start();

                        if (silence)
                        {
                            while (!workFinished)
                            {
                                Thread.Sleep(99);
                            }
                        }
                        else
                        {
                            Console.WriteLine(Resources.InProgress);
                            while (!workFinished)
                            {
                                Thread.Sleep(30);
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
                Console.WriteLine(ex.Message.ToString());
                Console.WriteLine(ex.StackTrace.ToString());
            }
        }

        static void m_ProgressChanged(object sender, EventArgs e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
