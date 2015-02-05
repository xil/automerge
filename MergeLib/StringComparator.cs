namespace MergeLib
{
    static class StringComparator
    {
        // not ready
        public static bool Compare(string strA, string strB, bool trimWhiteSpaces, EqualityMethods equalityMethod)
        {
            if (!trimWhiteSpaces) return strA.Equals(strB);
            string trimmedStr = strA.Trim();
            return trimmedStr.Equals(strB.Trim());
        }

        /// <summary>
        /// Compares two strings
        /// </summary>
        /// <param name="strA">First string to compare</param>
        /// <param name="strB">Second string to compare</param>
        /// <param name="trimWhiteSpaces">Remove the indentation at the start and end of each line</param>
        /// <returns>true if strings are equal</returns>

        public static bool Compare(string strA, string strB, bool trimWhiteSpaces)
        {
            if (!trimWhiteSpaces) return strA.Equals(strB);
            string trimmedStr = strA.Trim();
            return trimmedStr.Equals(strB.Trim());
        }
    }
}
