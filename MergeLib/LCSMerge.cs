using System;
using System.Collections.Generic;
using MergeLib.Properties;


namespace MergeLib
{
    /// <summary>
    /// Algorithm improvements over ThreeWayMerge class but eats more RAM.
    /// </summary>
    internal class LcsMerge : ThreeWayMerge, IMerger
    {
        private readonly List<Type> _actualConditions;

        public LcsMerge()
        {
            _actualConditions = new List<Type>();
            foreach (string str in Enum.GetNames(typeof (Conditions)))
            {
                _actualConditions.Add(Type.GetType("MergeLib." + str));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trimWhiteSpaces">Remove the indentation at the start and end of each line</param>
        /// <param name="equalityMethod">Choose method for string comparsion</param>
        /// <param name="conditions">Conditions for similar blocks merging</param>
        /// <param name="includeOriginalFileInOutput"></param>

        public LcsMerge(bool trimWhiteSpaces, EqualityMethods equalityMethod, List<Type> conditions, bool includeOriginalFileInOutput) :
            base(trimWhiteSpaces, equalityMethod, includeOriginalFileInOutput)
        {
            _actualConditions = conditions;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileA">File A</param>
        /// <param name="fileB">File B</param>
        /// <param name="fileO">Common ancestor</param>
        /// <param name="outputFile">Choose method for string comparsion</param>
        /// <returns>Overlapping info</returns>

        public new string Merge(List<string> fileA, List<string> fileB, List<string> fileO, 
            out List<string> outputFile)
        {
            outputFile = new List<string>();
            string message = "";
            List<string> lcs = LcsFinder.FindLcs(LcsFinder.FindLcs(fileA, fileO), LcsFinder.FindLcs(fileB, fileO));

            int indexA = 0;
            int indexB = 0;
            int indexO = 0;

            OnStateChanged(new ProgressEventArgs(Resources.ParsingDone));

            for (int i=0;i<=lcs.Count;i++)
            {
                int a;
                int b;
                int o;

                if (i < lcs.Count)
                {
                    a = fileA.FindIndex(indexA, x => StringComparator.Equal(x, lcs[i], _trimWhiteSpaces));
                    b = fileB.FindIndex(indexB, x => StringComparator.Equal(x, lcs[i], _trimWhiteSpaces));
                    o = fileO.FindIndex(indexO, x => StringComparator.Equal(x, lcs[i], _trimWhiteSpaces));
                }
                else
                {
                    a = fileA.Count;
                    b = fileB.Count;
                    o = fileO.Count;
                }

                List<string> aSubList = null;
                List<string> bSubList = null;
                List<string> oSubList = null;

                if (a - indexA > 0)
                {
                    aSubList = new List<string>();
                    aSubList.AddRange(fileA.GetRange(indexA, a - indexA));
                }
                if (b - indexB > 0)
                {
                    bSubList = new List<string>();
                    bSubList.AddRange(fileB.GetRange(indexB, b - indexB));
                }
                if (o - indexO > 0)
                {
                    oSubList = new List<string>();
                    oSubList.AddRange(fileO.GetRange(indexO, o - indexO));
                }

                int indexC = 0;
                List<string> subList = null;
                while (indexC < _actualConditions.Count && subList == null)
                {
                    ICondition cnd = (ICondition)Activator.CreateInstance(_actualConditions[indexC]);
                    subList = cnd.Check(aSubList, bSubList, oSubList, _trimWhiteSpaces, _includeOriginalFileInOutput);
                    indexC++;
                }
                if (subList == null)
                {
                    message = Resources.message_Overlapping;
                    Overlapping ov = new Overlapping();
                    subList = ov.Check(aSubList, bSubList, oSubList, _trimWhiteSpaces, _includeOriginalFileInOutput);
                }
                if (subList != null)
                    outputFile.AddRange(subList);

                if (i<lcs.Count)
                    outputFile.Add(lcs[i]);

                indexA = a + 1;
                indexB = b + 1;
                indexO = o + 1;
                
                OnProgressChanged(new ProgressEventArgs(String.Format(Resources.Parsing, indexO, fileO.Count)));
            }
            return message;
        }

        internal List<string> Merge(List<string> fileA, List<string> fileB)
        {
            List<string> result = new List<string>();
            List<string> lcs = LcsFinder.FindLcs(fileA, fileB);
            List<Type> cft = new List<Type>(){typeof(N_X__X),typeof(X_N__X),typeof(X_X__X)};
            int indexA = 0;
            int indexB = 0;

            for (int i = 0; i <= lcs.Count; i++)
            {
                int a;
                int b;

                if (i < lcs.Count)
                {
                    a = fileA.FindIndex(indexA, x => StringComparator.Equal(x, lcs[i], _trimWhiteSpaces));
                    b = fileB.FindIndex(indexB, x => StringComparator.Equal(x, lcs[i], _trimWhiteSpaces));
                }
                else
                {
                    a = fileA.Count;
                    b = fileB.Count;
                }

                List<string> aSubList = null;
                List<string> bSubList = null;

                if (a - indexA > 0)
                {
                    aSubList = new List<string>();
                    aSubList.AddRange(fileA.GetRange(indexA, a - indexA));
                }
                if (b - indexB > 0)
                {
                    bSubList = new List<string>();
                    bSubList.AddRange(fileB.GetRange(indexB, b - indexB));
                }

                int indexC = 0;
                List<string> subList = null;
                while (indexC < cft.Count && subList == null)
                {
                    IConditionForTwo cnd = (IConditionForTwo)Activator.CreateInstance(cft[indexC]);
                    subList = cnd.Check(aSubList, bSubList, _trimWhiteSpaces);
                    indexC++;
                }
                if (subList == null)
                {
                    OverlappingForTwo ov = new OverlappingForTwo();
                    subList = ov.Check(aSubList, bSubList, _trimWhiteSpaces);
                }
                if (subList != null)
                    result.AddRange(subList);

                if (i < lcs.Count)
                    result.Add(lcs[i]);

                indexA = a + 1;
                indexB = b + 1;
            }
            return result;
        }

    }
}
