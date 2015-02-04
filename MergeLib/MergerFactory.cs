using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MergeLib
{
    public class MergerFactory
    {
        public MergerFactory(){}

        public IMerger getInstance(List<string> initParams)
        {
            equalityMethods eqMethod = equalityMethods.String_equal;
            bool trimWhiteSpaces = true;

            if (initParams != null)
            {
                trimWhiteSpaces = initParams.Contains("trim");
                foreach (string m in Enum.GetNames(typeof(equalityMethods)))
                    if (initParams.Contains(m))
                    {
                        eqMethod = (equalityMethods)Enum.Parse(typeof(equalityMethods), m);
                        break;
                    }
            }

            return new ThreeWayMerge(trimWhiteSpaces, eqMethod);
        }
    }
}
