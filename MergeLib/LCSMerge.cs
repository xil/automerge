using System;
using System.Collections.Generic;
using System.Linq;
using MergeLib.Properties;


namespace MergeLib
{
    internal class LCSMerge : ThreeWayMerge//, IMerger
    {
        private List<string> _conditions = Enum.GetNames(typeof(Conditions)).ToList();
        
        public LCSMerge():base(){}
        public LCSMerge(bool trimWhiteSpaces, EqualityMethods equalityMethod, List<string> conditions, bool includeOriginalFileInOutput) :
            base(trimWhiteSpaces, equalityMethod, includeOriginalFileInOutput)
        {
            _conditions = conditions;
        }

        public string Merge(List<string> fileA, List<string> fileB, List<string> fileO, 
            out List<string> outputFile)
        {
            outputFile = new List<string>();
            string message = "";
            List<string> lcs = LCSFinder.FindLCS(LCSFinder.FindLCS(fileA, fileO), LCSFinder.FindLCS(fileB, fileO));

            int indexA = 0;
            int indexB = 0;
            int indexO = 0;

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
                while (indexC < _conditions.Count && subList == null)
                {
                    ICondition cnd = (ICondition)Activator.CreateInstance(Type.GetType("MergeLib."+_conditions[indexC]));
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
            List<string> lcs = LCSFinder.FindLCS(fileA, fileB);
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

        public new event EventHandler ProgressChanged;
    }
}
