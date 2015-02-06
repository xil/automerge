using System.Collections.Generic;

namespace MergeLib
{
    /// <summary>
    /// Interface for merging without ancestor file
    /// </summary>
    interface IConditionForTwo
    {
        List<string> Check(List<string> aStrList, List<string> bStrList, bool trim);
    }
}
