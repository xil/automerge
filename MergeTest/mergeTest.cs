using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MergeLibTest
{
    [TestClass]
    public class MergeTest
    {
        [TestMethod]
        public void merge_first_Test()
        {
            merge.Program.Main(new[] { "silent", "a.txt", "b.txt", "o.txt", "out.txt" });
        }

        

    }
}
