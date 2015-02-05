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
            EqualityMethods eqMethod = EqualityMethods.StringEqual;
            bool trimWhiteSpaces = true;

            if (initParams != null)
            {
                trimWhiteSpaces = initParams.Contains("trim");
                foreach (string m in Enum.GetNames(typeof(EqualityMethods)))
                    if (initParams.Contains(m))
                    {
                        eqMethod = (EqualityMethods)Enum.Parse(typeof(EqualityMethods), m);
                        break;
                    }
            }

            return new ThreeWayMerge(trimWhiteSpaces, eqMethod);
        }
    }
}
