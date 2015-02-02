using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeLib
{
    /// <summary>
    /// MergeLib main class
    /// </summary>

    public class MergeLib
    {
        List<string> _fileA;
        List<string> _fileB;
        List<string> _fileO;

        public MergeLib(ref string[] fileA, ref string[] fileB, ref string[] originalFile)
        {
            _fileA = fileA.ToList<string>();
            _fileB = fileB.ToList<string>();
            _fileO = originalFile.ToList<string>();
        }


        /// <summary>
        /// Start the merge process, not working
        /// </summary>
        /// <param name="trimWhiteSpaces">Remove the indentation at the start and end of each line</param>
        /// <param name="equalityMethod">Choose method for string comparsion</param>

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
        /// Start the merge process, equality method wiil be automatically selected, depending on the file length
        /// </summary>
        /// <param name="trimWhiteSpaces">Remove the indentation at the start and end of each line</param>

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

            // A |0|+123+49++
            // O |0|123456789
            // B |0|3++++6789
            #region parse
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

                if ((O + j) < _fileO.Count && (A + j) < _fileA.Count && (B + j) < _fileB.Count)  // straight line with not equal elements found
                {
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
            }
            while ((O + j) < _fileO.Count && (A + j) < _fileA.Count && (B + j) < _fileB.Count && indexO < _fileO.Count);

            if (O < _fileO.Count-1 || A < _fileA.Count-1 || B < _fileB.Count-1) // next straight line with not equal elements does not exist
            {
                _addBlock(ref blocksA, A + 1, _fileA.Count-1);
                _addBlock(ref blocksB, B + 1, _fileB.Count-1);
                _addBlock(ref blocksO, O + 1, _fileO.Count-1);
            }
            #endregion

            #region merge
            List<string> mergedOutput = new List<string>();
            for (int i = 0; i < blocksO.Count; i++)
            {
                mergedOutput.Add("block"+i.ToString());
                foreach (int strIndex in blocksA[i])
                {
                    if (strIndex >= 0)
                        mergedOutput.Add(_fileA[strIndex]);
                }
                foreach (int strIndex in blocksB[i])
                {
                    if (strIndex >= 0)
                        mergedOutput.Add(_fileB[strIndex]);
                }
                
            }
            #endregion
            result = mergedOutput.ToArray();
            return true;
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
