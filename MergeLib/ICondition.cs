using System.Collections.Generic;

namespace MergeLib
{
    /// <summary>
    /// Interface for merging conditions
    /// </summary>
    interface ICondition
    {
        List<string> Check(List<string> aStrList, List<string> bStrList, List<string> oStrList, 
            bool trim, bool includeOriginal);
    }
}
