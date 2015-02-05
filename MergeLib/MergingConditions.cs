using System;
using System.Collections.Generic;
using System.Linq;
using MergeLib.Properties;


namespace MergeLib
{
    static class MergingConditions
    {


        /*public static List<string> ApplyConditions( ref List<int[]> blocksO,ref List<int[]> blocksA, ref List<int[]> blocksB, 
                                                    List<string> fileA, List<string> fileB, List<string> fileO, 
                                                    out bool overlappingFound )
        {
            List<string> outputFile = new List<string>();                                                   // |A|O|B|=?
            for (int i = 0; i < blocksO.Count; i++)
            {
                if (blocksO[i][0] == -1 && blocksA[i][0] != -1 && blocksB[i][0] == -1)                      // |X|-|-|=X
                {
                    outputFile.AddRange(blocksA[i].Select(strIndex => fileA[strIndex]));
                    continue;
                }

                if (blocksO[i][0] == -1 && blocksA[i][0] == -1 && blocksB[i][0] != -1)                      // |-|-|X=X
                {
                    outputFile.AddRange(blocksB[i].Select(strIndex => fileB[strIndex]));
                    continue;
                }

                if (blocksA[i][0] != -1 && blocksB[i][0] != -1 && blocksA[i].Length == blocksB[i].Length)   // |X|?|X|=X
                {
                    int n = 0;
                    while (n < blocksA[i].Length && fileA[blocksA[i][n]].Equals(fileB[blocksB[i][n]]))
                        n++;
                    if (n == blocksA[i].Length)
                    {
                        outputFile.AddRange(blocksA[i].Select(strIndex => fileA[strIndex]));
                        continue;
                    }
                }

                if (blocksO[i][0] == -1 && blocksA[i][0] != -1 && blocksB[i][0] != -1)                      // |X|-|Y|=X-Y
                {
                    List<string> changesA = blocksA[i].Select(a => fileA[a]).ToList();
                    List<string> changesB = blocksB[i].Select(b => fileB[b]).ToList();

                    if (changesA.Intersect(changesB).Count() != 0)
                    {
                        List<string> changesR;
                        ThreeWayMerge twm = new ThreeWayMerge();
                        merge(changesA, changesB, changesB, out changesR, false);

                        outputFile.AddRange(changesR);

                        continue;
                    }
                }

                if (blocksB[i][0] == -1 && blocksA[i][0] != -1 && blocksO[i][0] != -1 &&
                    blocksA[i].Length == blocksO[i].Length)                                                 // |X|X|-|=-
                {
                    int n = 0;
                    while (n < blocksA[i].Length && fileA[blocksA[i][n]].Equals(fileO[blocksO[i][n]]))
                        n++;
                    if (n == blocksA[i].Length)
                    {
                        continue;
                    }
                }

                if (blocksA[i][0] == -1 && blocksB[i][0] != -1 && blocksO[i][0] != -1 &&
                    blocksB[i].Length == blocksO[i].Length)                                                 // |-|X|X|=-
                {
                    int n = 0;
                    while (n < blocksB[i].Length && fileB[blocksB[i][n]].Equals(fileO[blocksO[i][n]]))
                        n++;
                    if (n == blocksB[i].Length)
                    {
                        continue;
                    }
                }

                outputFile.Add(Resources.divLineBegin);

                overlappingFound = true;

                if (blocksA[i][0] != -1)
                {
                    outputFile.Add(Resources.divLineA);
                    outputFile.AddRange(blocksA[i].Select(strIndex => fileA[strIndex]));
                }

                if (blocksB[i][0] != -1)
                {
                    outputFile.Add(Resources.divLineB);
                    outputFile.AddRange(blocksB[i].Select(strIndex => fileB[strIndex]));
                }

                if (blocksO[i][0] != -1)
                {
                    outputFile.Add(Resources.divLineO);
                    outputFile.AddRange(blocksO[i].Select(strIndex => fileO[strIndex]));
                }

                outputFile.Add(Resources.divLineEnd);
            }
        }*/
    }
}
