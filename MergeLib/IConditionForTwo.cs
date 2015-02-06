using System.Collections.Generic;

namespace MergeLib
{
    interface IConditionForTwo
    {
        List<string> Check(List<string> aStrList, List<string> bStrList, bool trim);
    }
}
