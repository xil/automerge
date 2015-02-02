using System;
using MergeLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MergeTest
{
    [TestClass]
    public class MergeLibTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string[] O = new string[10] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            string[] A = new string[11] { "0", "1", "1.5", "2", "3", "4", "4.5", "5", "10", "11", "12" };
            string[] B = new string[10] { "1", "4", "4.1", "4.2", "4.3", "4.4", "7", "8", "9", "10" };
            string[] R;
            string[] expectation = new string[0];

            var mrg = new MergeLib.MergeLib(ref A, ref B, ref O);
            mrg.merge(out R, true);

            Assert.AreEqual(expectation, R);
        }
    }
}
