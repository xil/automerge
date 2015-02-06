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
            List<string> actualConditions = Enum.GetNames(typeof(Conditions)).ToList();
            bool includeOriginal = true;
            
            if (initParams != null)
            {
                trimWhiteSpaces = initParams.Contains("trim");
                includeOriginal = !initParams.Contains("notIncludeOriginal");

                foreach (string condition in actualConditions)
                {
                    if (initParams.Contains(condition))
                        actualConditions.Remove(condition);
                }

                foreach (string m in Enum.GetNames(typeof(EqualityMethods)))
                    if (initParams.Contains(m))
                    {
                        eqMethod = (EqualityMethods)Enum.Parse(typeof(EqualityMethods), m);
                        break;
                    }
            }

            return new LCSMerge(trimWhiteSpaces, eqMethod, actualConditions,includeOriginal);
        }
    }
}
