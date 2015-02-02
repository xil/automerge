using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MergeLib.Properties;

namespace MergeLib
{
    /// <summary>
    /// MergeLib main class
    /// </summary>

    public class Merger
    {
        List<string> _fileA;
        List<string> _fileB;
        List<string> _fileO;

        public Merger(ref string[] fileA, ref string[] fileB, ref string[] originalFile)
        {
            _fileA = fileA.ToList<string>();
            _fileB = fileB.ToList<string>();
            _fileO = originalFile.ToList<string>();
        }

        public Merger(string[] fileA, string[] fileB, string[] originalFile)
        {
            _fileA = fileA.ToList<string>();
            _fileB = fileB.ToList<string>();
            _fileO = originalFile.ToList<string>();
        }

        /// <summary>
        /// Start the merge process, not working
        /// </summary>
        /// <param name="result">Merged array</param>
        /// <param name="trimWhiteSpaces">Remove the indentation at the start and end of each line</param>
        /// <param name="equalityMethod">Choose method for string comparsion</param>
        /// <returns>True if overlapping found</returns>

        public bool merge(out string[] result, bool trimWhiteSpaces, equalityMethods equalityMethod)
        {
            if (trimWhiteSpaces)
            {
                _trim();
            }

            switch (equalityMethod)
            {
                //case equalityMethods.CheckSum
            }

            result = new string[1];
            return false;
        }


        /// <summary>
        /// Start the merge process, equality method wil be automatically selected, depending on the file length
        /// </summary>
        /// <param name="result">Merged array</param>
        /// <param name="trimWhiteSpaces">Remove the indentation at the start and end of each line</param>
        /// <returns>True if overlapping found</returns>

        public bool merge(out string[] result, bool trimWhiteSpaces)
        {
            if (trimWhiteSpaces)
            {
                _trim();
            }

            // need to calc ↓
            //equalityMethods.String_equal;

            List<int[]> blocksA = new List<int[]>(); //Parsed blocks from fileA
            List<int[]> blocksB = new List<int[]>(); //Parsed blocks from fileB
            List<int[]> blocksO = new List<int[]>(); //Parsed blocks from fileO

            int O = -1;  //_fileO index 
            int A = -1;  //_fileA index
            int B = -1;  //_fileB index
            int j;  //straight line index
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
                    (O + j) < _fileO.Count &&
                    (A + j) < _fileA.Count &&
                    (B + j) < _fileB.Count &&
                    _fileA[A + j].Equals(_fileO[O + j]) &&
                    _fileB[B + j].Equals(_fileO[O + j])
                    );

                if (j == 1)    // Trying to find equal elements in A and B for nearest element in O !! there must be values from LCS function !!
                {
                    indexO = O;
                    int indexA;
                    int indexB;

                    do
                    {
                        indexO++;
                        indexA = _fileA.FindIndex(A + 1, item => item.Equals(_fileO[indexO]));
                        indexB = _fileB.FindIndex(B + 1, item => item.Equals(_fileO[indexO]));
                    }
                    while (indexO < _fileO.Count && (indexA == -1 || indexB == -1));

                    if (indexO < _fileO.Count)     // Push unstable block
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
            while ((O + j) < _fileO.Count && (A + j) < _fileA.Count && (B + j) < _fileB.Count && indexO < _fileO.Count);

            if (O < _fileO.Count-1 || A < _fileA.Count-1 || B < _fileB.Count-1) // next straight line with not equal elements does not exist
            {
                _addBlock(ref blocksA, A + 1, _fileA.Count-1);
                _addBlock(ref blocksB, B + 1, _fileB.Count-1);
                _addBlock(ref blocksO, O + 1, _fileO.Count-1);
            }
            #endregion

            #region merging
            List<string> mergedOutput = new List<string>();                                         // A O B => ?
            for (int i = 0; i < blocksO.Count; i++)
            {
                if (blocksO[i][0] == -1 && blocksA[i][0] != -1 && blocksB[i][0] == -1)              // X - - => X
                {
                    foreach (int strIndex in blocksA[i]) mergedOutput.Add(_fileA[strIndex]);
                    continue;
                }

                if (blocksO[i][0] == -1 && blocksA[i][0] == -1 && blocksB[i][0] != -1)              // - - X => X
                {
                    foreach (int strIndex in blocksB[i]) mergedOutput.Add(_fileB[strIndex]);
                    continue;
                }

                if (blocksA[i][0] != -1 && blocksB[i][0] != -1 && blocksA[i].Length == blocksB[i].Length)   // X ? X => X
                {
                    int n = 0;
                    while (n < blocksA[i].Length && _fileA[blocksA[i][n]].Equals(_fileB[blocksB[i][n]]))
                        n++;
                    if (n == blocksA[i].Length)
                    {
                        foreach (int strIndex in blocksA[i]) mergedOutput.Add(_fileA[strIndex]);
                        continue;
                    }
                }

                if (blocksB[i][0] == -1 && blocksA[i][0] != -1 && blocksO[i][0] != -1 && blocksA[i].Length == blocksO[i].Length)                    // X X - => -
                {
                    int n = 0;
                    while (n < blocksA[i].Length && _fileA[blocksA[i][n]].Equals(_fileO[blocksO[i][n]]))
                        n++;
                    if (n == blocksA[i].Length)
                    {
                        continue;
                    }
                }

                if (blocksA[i][0] == -1 && blocksB[i][0] != -1 && blocksO[i][0] != -1 && blocksB[i].Length == blocksO[i].Length)                    // - X X => -
                {
                    int n = 0;
                    while (n < blocksB[i].Length && _fileB[blocksB[i][n]].Equals(_fileO[blocksO[i][n]]))
                        n++;
                    if (n == blocksB[i].Length)
                    {
                        continue;
                    }
                }

                mergedOutput.Add(Resources.divLineBegin);

                overlappingFound = true;

                if (blocksA[i][0] != -1)
                {
                    mergedOutput.Add(Resources.divLineA);
                    foreach (int strIndex in blocksA[i]) mergedOutput.Add(_fileA[strIndex]);
                }

                if (blocksB[i][0] != -1)
                {
                    mergedOutput.Add(Resources.divLineB);
                    foreach (int strIndex in blocksB[i]) mergedOutput.Add(_fileB[strIndex]);
                }

                if (blocksO[i][0] != -1)
                {
                    mergedOutput.Add(Resources.divLineO);
                    foreach (int strIndex in blocksO[i]) mergedOutput.Add(_fileO[strIndex]);
                }

                mergedOutput.Add(Resources.divLineEnd);
            }
            #endregion

            result = mergedOutput.ToArray();
            return overlappingFound;
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

        void _trim()
        {
            for (int i = 0; i < _fileA.Count; i++)
                _fileA[i].Trim();
            for (int i = 0; i < _fileB.Count; i++)
                _fileA[i].Trim();
            for (int i = 0; i < _fileO.Count; i++)
                _fileA[i].Trim();
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
