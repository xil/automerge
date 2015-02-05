using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeLib
{
    internal class LCSMerge : ThreeWayMerge//, IMerger
    {
        public string Merge(List<string> fileA, List<string> fileB, List<string> fileO, 
            out List<string> outputFile, bool includeOriginalFileInOutput)
        {
            List<int[]> blocksA = new List<int[]>(); //Parsed blocks from fileA
            List<int[]> blocksB = new List<int[]>(); //Parsed blocks from fileB
            List<int[]> blocksO = new List<int[]>(); //Parsed blocks from fileO
            outputFile = new List<string>();
            List<string> lcs = LCSFinder.FindLCS(LCSFinder.FindLCS(fileA, fileO), LCSFinder.FindLCS(fileB, fileO));

            int indexA = 0;
            int indexB = 0;

            foreach (string str in lcs)
            {
                int a = fileA.FindIndex(indexA, x => StringComparator.Compare(x, str, _trimWhiteSpaces));
                int b = fileB.FindIndex(indexB, x => StringComparator.Compare(x, str, _trimWhiteSpaces));

                if (a - indexA > 0) // instead of -1
                {
                    
                }

                if (b - indexB > 0)
                {

                }
                
                outputFile.Add(str);
            }
        }

        public event EventHandler ProgressChanged;
    }
}
