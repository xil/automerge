using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MergeLibTest
{
    [TestClass]
    public class mergeTest
    {
        [TestMethod]
        public void MainTest()
        {
            merge.Program.Main(new string[] { "a.txt", "b.txt", "o.txt", "out.txt" });
        }
    }
}
