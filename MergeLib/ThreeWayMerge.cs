using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MergeLib.Properties;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("MergeLibTest")]
namespace MergeLib
{
    /// <summary>
    /// MergeLib main class
    /// </summary>

    internal class ThreeWayMerge :IMerger
    {
        bool _trimWhiteSpaces;
        equalityMethods _equalityMethod;
        DateTime _lastTimeProgressEventOccurred = DateTime.Now;

        public event EventHandler ProgressChanged;

        protected virtual void OnProgressChanged(ProgressEventArgs e)
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

        protected virtual void OnStateChanged(ProgressEventArgs e)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(this, e);
                _lastTimeProgressEventOccurred = DateTime.Now;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trimWhiteSpaces">Remove the indentation at the start and end of each line</param>
        /// <param name="equalityMethod">Choose method for string comparsion</param>

        public ThreeWayMerge(bool trimWhiteSpaces, equalityMethods equalityMethod) 
        {
            _trimWhiteSpaces = trimWhiteSpaces;
            _equalityMethod = equalityMethod;
        }

        public ThreeWayMerge()
        {
            _trimWhiteSpaces=true;
            _equalityMethod = equalityMethods.String_equal;
        }

        /// <summary>
        /// Starts the merge process
        /// </summary>
        /// <param name="fileA">File A</param>
        /// <param name="fileB">File B</param>
        /// <param name="fileO">Common ancestor</param>
        /// <param name="outputFile">Choose method for string comparsion</param>
        /// <returns>True if overlapping found</returns>

        public string merge(List<string> fileA, List<string> fileB, List<string> fileO, out List<string> outputFile)
        {
            if (_trimWhiteSpaces)
            {
                for (int i = 0; i < fileA.Count; i++)
                    fileA[i].Trim();
                for (int i = 0; i < fileB.Count; i++)
                    fileB[i].Trim();
                for (int i = 0; i < fileO.Count; i++)
                    fileO[i].Trim();
            }

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
                    fileA[A + j].Equals(fileO[O + j]) &&
                    fileB[B + j].Equals(fileO[O + j])
                    );
                
                OnProgressChanged(new ProgressEventArgs(String.Format(Resources.Parsing, O, fileO.Count)));
                
                if (j == 1)    // Trying to find equal elements in A and B for nearest element in O !! there must be values from LCS function !!
                {
                    indexO = O + 1;
                    int indexA = -1;
                    int indexB = -1;

                    while (indexO < fileO.Count && (indexA == -1 || indexB == -1))
                    {
                        indexA = fileA.FindIndex(A + 1, item => item.Equals(fileO[indexO]));
                        indexB = fileB.FindIndex(B + 1, item => item.Equals(fileO[indexO]));
                        indexO++;
                    }


                    if (indexO < fileO.Count)     // Push unstable block
                    {
                        _addBlock(ref blocksA, A + 1, indexA - 1);
                        _addBlock(ref blocksB, B + 1, indexB - 1);
                        _addBlock(ref blocksO, O + 1, indexO - 1);

                        O = indexO - 1;
                        A = indexA - 1;
                        B = indexB - 1;
                    }
                }
                else if (j > 1)     // Push stable block
                {
                    _addBlock(ref blocksA, A + 1, A + j - 1);
                    _addBlock(ref blocksB, B + 1, B + j - 1);
                    _addBlock(ref blocksO, O + 1, O + j - 1);

                    O = O + j - 1;
                    A = A + j - 1;
                    B = B + j - 1;
                }
            }
            while ((O + j) < fileO.Count && (A + j) < fileA.Count && (B + j) < fileB.Count && indexO < fileO.Count);

            if (O < fileO.Count - 1 || A < fileA.Count - 1 || B < fileB.Count - 1) // next straight line with not equal elements does not exist
            {
                _addBlock(ref blocksA, A + 1, fileA.Count - 1);
                _addBlock(ref blocksB, B + 1, fileB.Count - 1);
                _addBlock(ref blocksO, O + 1, fileO.Count - 1);
            }
            #endregion
            OnStateChanged(new ProgressEventArgs(Resources.ParsingDone));
            #region merging
            outputFile = new List<string>();                                         // A O B => ?
            for (int i = 0; i < blocksO.Count; i++)
            {
                OnProgressChanged(new ProgressEventArgs(String.Format(Resources.Merging, i, blocksO.Count)));

                if (blocksO[i][0] == -1 && blocksA[i][0] != -1 && blocksB[i][0] == -1)              // X - - => X
                {
                    foreach (int strIndex in blocksA[i]) outputFile.Add(fileA[strIndex]);
                    continue;
                }

                if (blocksO[i][0] == -1 && blocksA[i][0] == -1 && blocksB[i][0] != -1)              // - - X => X
                {
                    foreach (int strIndex in blocksB[i]) outputFile.Add(fileB[strIndex]);
                    continue;
                }

                if (blocksA[i][0] != -1 && blocksB[i][0] != -1 && blocksA[i].Length == blocksB[i].Length)   // X ? X => X
                {
                    int n = 0;
                    while (n < blocksA[i].Length && fileA[blocksA[i][n]].Equals(fileB[blocksB[i][n]]))
                        n++;
                    if (n == blocksA[i].Length)
                    {
                        foreach (int strIndex in blocksA[i]) outputFile.Add(fileA[strIndex]);
                        continue;
                    }
                }

                if (blocksB[i][0] == -1 && blocksA[i][0] != -1 && blocksO[i][0] != -1 && blocksA[i].Length == blocksO[i].Length)                    // X X - => -
                {
                    int n = 0;
                    while (n < blocksA[i].Length && fileA[blocksA[i][n]].Equals(fileO[blocksO[i][n]]))
                        n++;
                    if (n == blocksA[i].Length)
                    {
                        continue;
                    }
                }

                if (blocksA[i][0] == -1 && blocksB[i][0] != -1 && blocksO[i][0] != -1 && blocksB[i].Length == blocksO[i].Length)                    // - X X => -
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
                    foreach (int strIndex in blocksA[i]) outputFile.Add(fileA[strIndex]);
                }

                if (blocksB[i][0] != -1)
                {
                    outputFile.Add(Resources.divLineB);
                    foreach (int strIndex in blocksB[i]) outputFile.Add(fileB[strIndex]);
                }

                if (blocksO[i][0] != -1)
                {
                    outputFile.Add(Resources.divLineO);
                    foreach (int strIndex in blocksO[i]) outputFile.Add(fileO[strIndex]);
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
        string _progress;
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

    public enum equalityMethods
    {
        CryptoHash,
        CheckSum,
        String_equal,
    }
}
