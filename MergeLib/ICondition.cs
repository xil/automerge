using System.Collections.Generic;

namespace MergeLib
{
    interface ICondition
    {
        List<string> Check(List<string> aStrList, List<string> bStrList, List<string> oStrList, 
            bool trim, bool includeOriginal);
    }
}
