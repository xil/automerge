using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeLib
{
    internal class LCSFinder
    {
        public LCSFinder()
        {

        }

        public List<string> findLCS(List<string> fileA, List<string> fileO)
        {
            int[,] C = LCSLength(fileA, fileO);
            return backtrack(ref C, ref fileA, ref fileO, fileA.Count, fileO.Count);
        }

        int[,] LCSLength(List<string> fileA, List<string> fileO)
        {
            int m = fileA.Count+1;
            int n = fileO.Count+1;

            int[,] C = new int[m, n];
            for (int i = 0; i < m; i++)
                C[i, 0] = 0;
            for (int j = 0; j < n; j++)
                C[0, j] = 0;
            for (int i = 1; i < m; i++)
                for (int j = 1; j < n; j++)
                    if (fileA[i-1].Equals(fileO[j-1]))
                        C[i, j] = C[i - 1, j - 1] + 1;
                    else
                        C[i,j]=Math.Max(C[i,j-1],C[i-1,j]);
            return C;
        }

        List<string> backtrack(ref int[,] C, ref List<string> fileA, ref List<string> fileO, int i, int j)
        {
            if (i == 0 || j == 0)
                return new List<string>();
            else if (!fileA[i - 1].Equals(fileO[j - 1]))
            {
                if (C[i, j - 1] > C[i - 1, j])
                    return backtrack(ref C, ref fileA, ref fileO, i, j - 1);
                else
                    return backtrack(ref C, ref fileA, ref fileO, i - 1, j);
            }
            else
            {
                List<string> result = backtrack(ref C, ref fileA, ref fileO, i - 1, j - 1);
                result.Add(fileA[i - 1]);
                return result;
            }
        }
    }
}
