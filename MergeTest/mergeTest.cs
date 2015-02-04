using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace MergeLibTest
{
    [TestClass]
    public class mergeTest
    {
        [TestMethod]
        public void merge_first_Test()
        {
            merge.Program.Main(new string[] { "silent", "a.txt", "b.txt", "o.txt", "out.txt" });
        }

        

    }
}
