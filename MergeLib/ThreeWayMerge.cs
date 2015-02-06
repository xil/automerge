using System;
using System.Collections.Generic;
using System.Linq;
using MergeLib.Properties;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MergeLibTest")]
namespace MergeLib
{
    internal class ThreeWayMerge :IMerger
    {
        protected readonly bool _trimWhiteSpaces;
        protected EqualityMethods _equalityMethod;
        protected DateTime _lastTimeProgressEventOccurred = DateTime.Now;
        protected bool _includeOriginalFileInOutput;
        public event EventHandler ProgressChanged;

        protected void OnProgressChanged(ProgressEventArgs e)
        {
            if (ProgressChanged != null)
            {
                if (DateTime.Now.Subtract(_lastTimeProgressEventOccurred).Milliseconds > 500)
                {
                    ProgressChanged(this, e);
                    _lastTimeProgressEventOccurred = DateTime.Now;
                }

            }
        }

        protected void OnStateChanged(ProgressEventArgs e)
        {
            if (ProgressChanged == null) return;
            ProgressChanged(this, e);
            _lastTimeProgressEventOccurred = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trimWhiteSpaces">Remove the indentation at the start and end of each line</param>
        /// <param name="equalityMethod">Choose method for string comparsion</param>

        public ThreeWayMerge(bool trimWhiteSpaces, EqualityMethods equalityMethod, bool includeOriginalFileInOutput) 
        {
            _trimWhiteSpaces = trimWhiteSpaces;
            _equalityMethod = equalityMethod;
            _includeOriginalFileInOutput = includeOriginalFileInOutput;
        }

        public ThreeWayMerge()
        {
            _trimWhiteSpaces=true;
            _equalityMethod = EqualityMethods.StringEqual;
            _includeOriginalFileInOutput = true;
        }

        /// <summary>
        /// Starts the merge process
        /// </summary>
        /// <param name="fileA">File A</param>
        /// <param name="fileB">File B</param>
        /// <param name="fileO">Common ancestor</param>
        /// <param name="outputFile">Choose method for string comparsion</param>
        /// <param name="includeOriginalFileInOutput">Duct tape for two files merging *facepalm*</param>
        /// <returns>True if overlapping found</returns>

        public string Merge(List<string> fileA, List<string> fileB, List<string> fileO, out List<string> outputFile)
        {

            List<int[]> blocksA = new List<int[]>(); //Parsed blocks from fileA
            List<int[]> blocksB = new List<int[]>(); //Parsed blocks from fileB
            List<int[]> blocksO = new List<int[]>(); //Parsed blocks from fileO

            int O = -1;  //fileO index 
            int A = -1;  //fileA index
            int B = -1;  //fileB index
            int j;       //straight line index
            int indexO = 0;
            bool overlappingFound = false;

            #region parsing
            do
            {

                j = 0;
                // Trying to find next straight line with unequal elements

                do
                {
                    j++;
                }
                while (
                    (O + j) < fileO.Count &&
                    (A + j) < fileA.Count &&
                    (B + j) < fileB.Count &&
                    StringComparator.Equal(fileA[A + j], fileO[O + j], _trimWhiteSpaces) &&
                    StringComparator.Equal(fileB[B + j], fileO[O + j], _trimWhiteSpaces)
                    );

                if ((O + j) <= fileO.Count &&        
                    (A + j) <= fileA.Count &&        // Includes equality because otherwise last straight line won't become block
                    (B + j) <= fileB.Count)
                {
                    OnProgressChanged(new ProgressEventArgs(String.Format(Resources.Parsing, O, fileO.Count)));

                    if (j == 1)    // Trying to find equal elements in A and B for nearest element in O !! there must be values from LCS function !!
                    {
                        indexO = O;
                        int indexA = -1;
                        int indexB = -1;


                        while (indexO < fileO.Count - 1 && (indexA == -1 || indexB == -1))  // .Count - 1 because otherwise indexOutOfRangeException
                        {
                            indexO++;
                            indexA = fileA.FindIndex(A + 1, item => StringComparator.Equal(item,fileO[indexO],_trimWhiteSpaces));
                            indexB = fileB.FindIndex(B + 1, item => StringComparator.Equal(item,fileO[indexO],_trimWhiteSpaces));

                        }
                        if (indexO >= fileO.Count - 1 && (indexA == -1 || indexB == -1))
                            break;

                        if (indexO < fileO.Count)     // Push unstable block
                        {
                            _addBlock(ref blocksA, A + 1, indexA - 1);
                            _addBlock(ref blocksB, B + 1, indexB - 1);
                            if (_includeOriginalFileInOutput)
                                _addBlock(ref blocksO, O + 1, indexO - 1);
                            else _addBlock(ref blocksO, 1, 0);

                            O = indexO - 1;
                            A = indexA - 1;
                            B = indexB - 1;

                            continue;
                        }
                        else
                            break;
                    }
                    else if (j > 1)     // Push stable block
                    {
                        _addBlock(ref blocksA, A + 1, A + j - 1);
                        _addBlock(ref blocksB, B + 1, B + j - 1);
                        if (_includeOriginalFileInOutput)
                            _addBlock(ref blocksO, O + 1, O + j - 1);
                        else _addBlock(ref blocksO, 1, 0);

                        O = O + j - 1;
                        A = A + j - 1;
                        B = B + j - 1;

                        continue;
                    }
                }
                else
                    break;
            }
            while (true);   // duct tape is everywere

            if (O < fileO.Count - 1 || A < fileA.Count - 1 || B < fileB.Count - 1) // next straight line with not equal elements does not exist
            {
                _addBlock(ref blocksA, A + 1, fileA.Count - 1);
                _addBlock(ref blocksB, B + 1, fileB.Count - 1);
                if (_includeOriginalFileInOutput) 
                    _addBlock(ref blocksO, O + 1, fileO.Count - 1);
                else _addBlock(ref blocksO, 1, 0);
            }
            #endregion

            OnStateChanged(new ProgressEventArgs(Resources.ParsingDone));

            #region merging
            outputFile = new List<string>();                                                                // A O B => ?
            for (int i = 0; i < blocksO.Count; i++)
            {
                OnProgressChanged(new ProgressEventArgs(String.Format(Resources.Merging, i, blocksO.Count)));

                if (blocksO[i][0] == -1 && blocksA[i][0] != -1 && blocksB[i][0] == -1)                      // X - - => X
                {
                    outputFile.AddRange(blocksA[i].Select(strIndex => fileA[strIndex]));
                    continue;
                }

                if (blocksO[i][0] == -1 && blocksA[i][0] == -1 && blocksB[i][0] != -1)                      // - - X => X
                {
                    outputFile.AddRange(blocksB[i].Select(strIndex => fileB[strIndex]));
                    continue;
                }

                if (blocksA[i][0] != -1 && blocksB[i][0] != -1 && blocksA[i].Length == blocksB[i].Length)   // X ? X => X
                {
                    int n = 0;
                    while (n < blocksA[i].Length && 
                        StringComparator.Equal(fileA[blocksA[i][n]], fileB[blocksB[i][n]],_trimWhiteSpaces))
                        n++;
                    if (n == blocksA[i].Length)
                    {
                        outputFile.AddRange(blocksA[i].Select(strIndex => fileA[strIndex]));
                        continue;
                    }
                }

                if (blocksO[i][0] == -1 && blocksA[i][0] != -1 && blocksB[i][0] != -1)                      // X - Y => |X - Y|
                {
                    List<string> changesA = blocksA[i].Select(a => fileA[a]).ToList();
                    List<string> changesB = blocksB[i].Select(b => fileB[b]).ToList();
                    
                    if (changesA.Intersect(changesB).Count() != 0)
                    {
                        List<string> changesR;
                        Merge(changesA, changesB, changesB, out changesR);

                        outputFile.AddRange(changesR);

                        continue;
                    }
                }

                if (blocksB[i][0] == -1 && blocksA[i][0] != -1 && blocksO[i][0] != -1 && 
                    blocksA[i].Length == blocksO[i].Length)                                                 // X X - => -
                {
                    int n = 0;
                    while (n < blocksA[i].Length && 
                        StringComparator.Equal(fileA[blocksA[i][n]], fileO[blocksO[i][n]],_trimWhiteSpaces))
                        n++;
                    if (n == blocksA[i].Length)
                    {
                        continue;
                    }
                }

                if (blocksA[i][0] == -1 && blocksB[i][0] != -1 && blocksO[i][0] != -1 && 
                    blocksB[i].Length == blocksO[i].Length)                                                 // - X X => -
                {
                    int n = 0;
                    while (n < blocksB[i].Length && 
                        StringComparator.Equal(fileB[blocksB[i][n]],fileO[blocksO[i][n]],_trimWhiteSpaces))
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
            #endregion
            OnStateChanged(new ProgressEventArgs(Resources.MergingDone));
            if (overlappingFound)
                return Resources.message_Overlapping;
            else return "";
        }
        

        void _addBlock(ref List<int[]> blockList, int firstValue, int lastValue)
        {
            if (firstValue < lastValue)
                blockList.Add(Enumerable.Range(firstValue, lastValue - firstValue + 1).ToArray<int>());
            else if (firstValue == lastValue)
                blockList.Add(new int[] { firstValue });
            else
                blockList.Add(new int[] { -1 });
        }

    }

    public class ProgressEventArgs : EventArgs
    {
        readonly string _progress;
        public ProgressEventArgs(string progress)
        {
            _progress = progress;
        }

        public override string ToString()
        {
            return _progress;
        }
    }

    /// <summary>
    /// 
    /// </summary>

    public enum EqualityMethods
    {
        StringEqual,
        CryptoHash,
        CheckSum,
    }
}
