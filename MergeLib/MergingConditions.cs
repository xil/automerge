using System.Collections.Generic;
using MergeLib.Properties;


namespace MergeLib
{
    /// <summary>
    /// Naming of this classes describe conditions. 
    /// Pattern A_O_B__result. 
    /// N   - for nothing
    /// X,Y - value
    /// any - any value
    /// mrg - value will be merged from blocks
    /// </summary>
    internal class X_N_N__X : ICondition
    {
        public List<string> Check(List<string> aStrList, List<string> bStrList, List<string> oStrList,
            bool trim, bool includeOriginal)
        {
            if (oStrList == null && aStrList != null && bStrList == null)
                return aStrList;
            return null;
        }
    }

    internal class N_N_X__X : ICondition
    {
        public List<string> Check(List<string> aStrList, List<string> bStrList, List<string> oStrList,
            bool trim, bool includeOriginal)
        {
            if (oStrList == null && aStrList == null && bStrList != null)
                return bStrList;
            return null;
        }
    }

    internal class X_any_X__X : ICondition
    {
        public List<string> Check(List<string> aStrList, List<string> bStrList, List<string> oStrList,
            bool trim, bool includeOriginal)
        {
            if (aStrList != null && bStrList != null && aStrList.Count == bStrList.Count)
            {
                int n = 0;
                while (n < aStrList.Count && StringComparator.Equal(aStrList[n], bStrList[n], trim))
                    n++;
                if (n == aStrList.Count)
                {
                    return aStrList;
                }
            }
            return null;
        }
    }

    internal class X_N_Y__mrg : ICondition
    {
        public List<string> Check(List<string> aStrList, List<string> bStrList, List<string> oStrList,
            bool trim, bool includeOriginal)
        {
            if (oStrList == null && aStrList != null && bStrList != null)
            {
                LcsMerge mrg = new LcsMerge();
                return mrg.Merge(aStrList, bStrList);
            }
            return null;
        }
    }

    internal class X_X_N__N : ICondition
    {
        public List<string> Check(List<string> aStrList, List<string> bStrList, List<string> oStrList,
            bool trim, bool includeOriginal)
        {
            if (oStrList != null && aStrList != null && bStrList == null && aStrList.Count == oStrList.Count)
            {
                int n = 0;
                while (n < aStrList.Count && StringComparator.Equal(aStrList[n], oStrList[n], trim))
                    n++;
                if (n == aStrList.Count)
                {
                    return new List<string>();
                }
            }
            return null;
        }
    }

    internal class N_X_X__N : ICondition
    {
        public List<string> Check(List<string> aStrList, List<string> bStrList, List<string> oStrList,
            bool trim, bool includeOriginal)
        {
            if (oStrList != null && aStrList == null && bStrList != null && bStrList.Count == oStrList.Count)
            {
                int n = 0;
                while (n < bStrList.Count && StringComparator.Equal(bStrList[n], oStrList[n], trim))
                    n++;
                if (n == bStrList.Count)
                {
                    return new List<string>();
                }
            }
            return null;
        }
    }

    internal class Overlapping : ICondition
    {
        public List<string> Check(List<string> aStrList, List<string> bStrList, List<string> oStrList,
            bool trim, bool includeOriginal)
        {
            if (aStrList == null && bStrList == null && oStrList == null)
                return null;

            List<string> result = new List<string> {Resources.divLineBegin};
            if (aStrList != null && aStrList.Count > 0)
            {
                result.Add(Resources.divLineA);
                result.AddRange(aStrList);
            }
            if (bStrList != null && bStrList.Count > 0)
            {
                result.Add(Resources.divLineB);
                result.AddRange(bStrList);
            }
            if (oStrList != null && oStrList.Count > 0 && includeOriginal)
            {
                result.Add(Resources.divLineO);
                result.AddRange(oStrList);
            }
            result.Add(Resources.divLineEnd);
            return result;
        }
    }



    internal class X_N__X : IConditionForTwo
    {
        public List<string> Check(List<string> aStrList, List<string> bStrList, bool trim)
        {
            if (aStrList != null && bStrList == null)
                return aStrList;
            return null;
        }
    }

    internal class N_X__X : IConditionForTwo
    {
        public List<string> Check(List<string> aStrList, List<string> bStrList, bool trim)
        {
            if (aStrList == null && bStrList != null)
                return bStrList;
            return null;
        }
    }

    internal class X_X__X : IConditionForTwo
    {
        public List<string> Check(List<string> aStrList, List<string> bStrList, bool trim)
        {
            if (aStrList != null && bStrList != null && aStrList.Count == bStrList.Count)
            {
                int n = 0;
                while (n < aStrList.Count && StringComparator.Equal(aStrList[n], bStrList[n], trim))
                    n++;
                if (n == aStrList.Count)
                {
                    return aStrList;
                }
            }
            return null;
        }
    }

    internal class OverlappingForTwo : IConditionForTwo
    {
        public List<string> Check(List<string> aStrList, List<string> bStrList, bool trim)
        {
            if (aStrList == null && bStrList == null)
                return null;

            List<string> result = new List<string> { Resources.divLineBegin };
            if (aStrList != null && aStrList.Count > 0)
            {
                result.Add(Resources.divLineA);
                result.AddRange(aStrList);
            }
            if (bStrList.Count > 0)
            {
                result.Add(Resources.divLineB);
                result.AddRange(bStrList);
            }
            
            result.Add(Resources.divLineEnd);
            return result;
        }
    }

    public enum Conditions
    {
        X_N_N__X,
        N_N_X__X,
        X_any_X__X,
        X_N_Y__mrg,
        X_X_N__N,
        N_X_X__N,
    }

    public enum ConditionsForTwo
    {
        X_N__X,
        N_X__X,
        X_X__X,
        X_Y__mrg,
    }

}

