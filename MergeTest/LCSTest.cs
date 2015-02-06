using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MergeLib;
using System.IO;

namespace MergeLibTest
{
    [TestClass]
    public class LCSTest
    {
        [TestMethod]
        public void LCSMerge_first_Test()
        {
            List<string> o = new List<string>(new string[] { "1", "2", "3", "4", "10" });
            List<string> a = new List<string>(new string[] { "0", "1", "1.5", "2", "3", "4", "4.5", "5", "10", "11", "12" });
            List<string> b = new List<string>(new string[] { "1", "4", "4.1", "4.2", "4.3", "4.4", "5", "8", "9", "10" });
            List<string> expectation = new List<string>(new string[27] { "0", "1", 
                                          "================================ Overlapping ================================",
			                              "================================    FileA    ================================",
                                          "1.5", "2", "3", 
                                          "================================    FileO    ================================",
                                          "2", "3", 
                                          "================================     End     ================================",
                                          "4",
                                          "================================ Overlapping ================================",
			                              "================================    FileA    ================================",
                                          "4.5",
                                          "================================    FileB    ================================",
                                          "4.1", "4.2", "4.3", "4.4", 
                                          "================================     End     ================================",
                                          "5","8","9","10","11","12"});

            List<string> r;
            var twm = new LCSMerge();
            twm.Merge(a, b, o, out r);

            CollectionAssert.AreEqual(expectation, r);
        }

        [TestMethod]
        public void LCSMerge_LongestCommonSubsequence_Test()
        {
            List<string> O = new List<string>(File.ReadAllLines(@"TestData\ReleaseNotes 9_4.html"));
            List<string> A = new List<string>(File.ReadAllLines(@"TestData\ReleaseNotes 9_4_1.html"));
            List<string> B = new List<string>(File.ReadAllLines(@"TestData\ReleaseNotes 9_4_2.html"));
            List<string> expectation = new List<string>(File.ReadAllLines(@"TestData\ReleaseNotes 9_4.html.expected"));
            List<string> R = new List<string>();

            var twm = new LCSMerge();
            twm.Merge(A, B, O, out R);

            CollectionAssert.AreEqual(expectation, R);
        }
    }
}
