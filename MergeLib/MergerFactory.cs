using MergeLib.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;

namespace MergeLib
{
    /// <summary>
    /// This class controls the behavior of the MergeLib library
    /// </summary>
    public static class MergerFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="initParams">Conditions and flags and so on</param>
        /// <param name="sizeOfLargestFile">Size of the largest files in bytes</param>
        /// <returns>Instance of MergeLib</returns>
        public static IMerger GetInstance(List<string> initParams, int sizeOfLargestFile)
        {
            EqualityMethods eqMethod = EqualityMethods.StringEqual;
            bool trimWhiteSpaces = true;
            bool includeOriginal = true;
            List<Type> actualConditions = Enum.GetNames(typeof (Conditions)).Select(str => Type.GetType("MergeLib." + str)).ToList();
            List<string> excludeConditionsList = new List<string>();
            int fileSizeThreshold = 1048576;

            if (initParams != null)
            {
                trimWhiteSpaces = initParams.Contains("trim");
                includeOriginal = !initParams.Contains("notIncludeOriginal");

                excludeConditionsList.AddRange(Enum.GetNames(typeof (Conditions)).Where(initParams.Contains));

                foreach (string str in excludeConditionsList)
                {
                    actualConditions.Remove(actualConditions.Find(item => item.Name == str));
                }

                foreach (string m in Enum.GetNames(typeof(EqualityMethods)))
                    if (initParams.Contains(m))
                    {
                        eqMethod = (EqualityMethods)Enum.Parse(typeof(EqualityMethods), m);
                        break;
                    }
            }

            if (sizeOfLargestFile > fileSizeThreshold)
                return new ThreeWayMerge(trimWhiteSpaces, eqMethod, includeOriginal);
            else
                return new LcsMerge(trimWhiteSpaces, eqMethod, actualConditions, includeOriginal);

        }
    }
}
