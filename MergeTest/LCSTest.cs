using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using MergeLib;

namespace MergeLibTest
{
    [TestClass]
    public class LCSTest
    {
        [TestMethod]
        public void LCS_first_Test()
        {
            List<string> O = new List<string>(new string[5] { "1", "2", "3", "4", "10" });
            List<string> A = new List<string>(new string[11] { "0", "1", "1.5", "2", "3", "4", "4.5", "5", "10", "11", "12" });
            List<string> B = new List<string>(new string[10] { "1", "4", "4.1", "4.2", "4.3", "4.4", "5", "8", "9", "10" });
            /*List<string> expectation = new List<string>(new string[27] { "0", "1", 
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
             */
            List<string> R;

            LCSFinder finder = new LCSFinder();
            R = finder.findLCS(A, O);

            //CollectionAssert.AreEqual(expectation, R);
        }
    }
}
