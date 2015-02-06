using System;
using System.Collections.Generic;
using System.IO;
using MergeLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MergeLibTest
{
    [TestClass]
    public class MergeLibTest
    {
        [TestMethod]
        public void MergeLib_first_Test()
        {
            List<string> O = new List<string>(new string[5] { "1", "2", "3", "4", "10" });
            List<string> A = new List<string>(new string[11] { "0", "1", "1.5", "2", "3", "4", "4.5", "5", "10", "11", "12" });
            List<string> B = new List<string>(new string[10] { "1", "4", "4.1", "4.2", "4.3", "4.4", "5", "8", "9", "10" });
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
            List<string> R;

            ThreeWayMerge twm = new ThreeWayMerge();
            twm.Merge(A, B, O, out R);
            
            CollectionAssert.AreEqual(expectation, R);
        }

        [TestMethod]
        public void MergeLib_Overlapping_Test()
        {
            List<string> O = new List<string>(File.ReadAllLines(@"TestData\1_simpletest_base.txt"));
            List<string> A = new List<string>(File.ReadAllLines(@"TestData\1_simpletest_contrib1.txt"));
            List<string> B = new List<string>(File.ReadAllLines(@"TestData\1_simpletest_contrib2.txt"));
            List<string> expectation = new List<string>(File.ReadAllLines(@"TestData\1_simpletest_expected_result.txt"));
            List<string> R = new List<string>();

            ThreeWayMerge twm = new ThreeWayMerge();
            twm.Merge(A, B, O, out R);
            
            CollectionAssert.AreEqual(expectation, R);
        }

        [TestMethod]
        public void MergeLib_BeginEndOverlapping_Test()
        {
            List<string> O = new List<string>(File.ReadAllLines(@"TestData\Misc.java.parent"));
            List<string> A = new List<string>(File.ReadAllLines(@"TestData\Misc.java.1st"));
            List<string> B = new List<string>(File.ReadAllLines(@"TestData\Misc.java.2nd"));
            List<string> expectation = new List<string>(File.ReadAllLines(@"TestData\Misc.java.out.expected"));
            List<string> R = new List<string>();

            ThreeWayMerge twm = new ThreeWayMerge();
            twm.Merge(A, B, O, out R);
            
            CollectionAssert.AreEqual(expectation, R);
        }

        [TestMethod]
        public void MergeLib_IdenticalChanges_Test()
        {
            List<string> O = new List<string>(File.ReadAllLines(@"TestData\About.java.parent"));
            List<string> A = new List<string>(File.ReadAllLines(@"TestData\About.java.1st"));
            List<string> B = new List<string>(File.ReadAllLines(@"TestData\About.java.2nd"));
            List<string> expectation = new List<string>(File.ReadAllLines(@"TestData\About.java.expected"));
            List<string> R = new List<string>();

            ThreeWayMerge twm = new ThreeWayMerge();
            twm.Merge(A, B, O, out R);
            
            CollectionAssert.AreEqual(expectation, R);
        }

        [TestMethod]
        public void MergeLib_IdenticalSubSetInChanges_Test()
        {
            List<string> O = new List<string>(File.ReadAllLines(@"TestData\getCLIArgs.java.parent"));
            List<string> A = new List<string>(File.ReadAllLines(@"TestData\getCLIArgs.java.1st"));
            List<string> B = new List<string>(File.ReadAllLines(@"TestData\getCLIArgs.java.2nd"));
            List<string> expectation = new List<string>(File.ReadAllLines(@"TestData\getCLIArgs.java.expected"));
            List<string> R = new List<string>();

            ThreeWayMerge twm = new ThreeWayMerge();
            twm.Merge(A, B, O, out R);

            CollectionAssert.AreEqual(expectation, R);
        }

        

        [TestMethod]
        public void MergeLib_StringComparator_Test()
        {
            List<string> O = new List<string>(File.ReadAllLines(@"TestData\Bug_ReporterApp_Parent.h"));
            List<string> A = new List<string>(File.ReadAllLines(@"TestData\Bug_ReporterApp_BranchA.h"));
            List<string> B = new List<string>(File.ReadAllLines(@"TestData\Bug_ReporterApp_BranchB.h"));
            List<string> expectation = new List<string>(File.ReadAllLines(@"TestData\Bug_ReporterApp.h.expected"));
            List<string> R = new List<string>();

            ThreeWayMerge twm = new ThreeWayMerge();
            twm.Merge(A, B, O, out R);

            CollectionAssert.AreEqual(expectation, R);
        }

        [TestMethod]
        public void NoAssert_MergeLib_MaxFileLength_Test()
        {
            List<string> O = new List<string>();
            List<string> A = new List<string>();
            List<string> B = new List<string>();
            //List<string> expectation = new List<string>(File.ReadAllLines(@"TestData\About.java.out.expected"));
            List<string> R = new List<string>();

            Random rnd = new Random();
            byte[] buffer = new byte[48];;
            for (int i = 0; i < 30000; i++)
            {
                rnd.NextBytes(buffer);
                O.Add(System.Text.Encoding.UTF8.GetString(buffer));
                rnd.NextBytes(buffer);
                A.Add(System.Text.Encoding.UTF8.GetString(buffer));
                rnd.NextBytes(buffer);
                B.Add(System.Text.Encoding.UTF8.GetString(buffer));
            }

            File.WriteAllLines(@"TestData\Big_File.O", O);
            File.WriteAllLines(@"TestData\Big_File.A", A);
            File.WriteAllLines(@"TestData\Big_File.B", B);



            ThreeWayMerge twm = new ThreeWayMerge();
            twm.Merge(A, B, O, out R);

            File.WriteAllLines(@"TestData\Big_File.B.expected", R);

            //CollectionAssert.AreEqual(expectation, R);
        }

    
    }
}
