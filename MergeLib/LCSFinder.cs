using System;
using System.Collections.Generic;

namespace MergeLib
{
    internal static class LCSFinder
    {
        public static List<string> FindLCS(List<string> fileA, List<string> fileO)
        {
            int[,] solutionTable = LCSLength(fileA, fileO);
            return Backtrack(ref solutionTable, ref fileA, ref fileO, fileA.Count, fileO.Count);
        }

        static int[,] LCSLength(IReadOnlyList<string> fileA, IReadOnlyList<string> fileO)
        {
            int m = fileA.Count+1;
            int n = fileO.Count+1;

            int[,] solutionTable = new int[m, n];
            for (int i = 0; i < m; i++)
                solutionTable[i, 0] = 0;
            for (int j = 0; j < n; j++)
                solutionTable[0, j] = 0;
            for (int i = 1; i < m; i++)
                for (int j = 1; j < n; j++)
                    if (fileA[i-1].Equals(fileO[j-1]))
                        solutionTable[i, j] = solutionTable[i - 1, j - 1] + 1;
                    else
                        solutionTable[i, j] = Math.Max(solutionTable[i, j - 1], solutionTable[i - 1, j]);
            return solutionTable;
        }

        private static List<string> Backtrack(ref int[,] solutionTable, ref List<string> fileA, ref List<string> fileO, int i, int j)
        {
            while (true)
            {
                if (i == 0 || j == 0)
                    return new List<string>();
                else if (!fileA[i - 1].Equals(fileO[j - 1]))
                {
                    if (solutionTable[i, j - 1] > solutionTable[i - 1, j])
                    {
                        j = j - 1;
                    }
                    else
                    {
                        i = i - 1;
                    }
                }
                else
                {
                    List<string> result = Backtrack(ref solutionTable, ref fileA, ref fileO, i - 1, j - 1);
                    result.Add(fileA[i - 1]);
                    return result;
                }
            }
        }
    }
}
