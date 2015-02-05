using System;
using System.Collections.Generic;

namespace MergeLib
{
    public interface IMerger
    {
        /// <summary>
        /// Starts the merge process
        /// </summary>
        /// <param name="fileA">File A</param>
        /// <param name="fileB">File B</param>
        /// <param name="fileO">Common ancestor</param>
        /// <param name="outputFile">Choose method for string comparsion</param>
        /// <param name="includeOriginalFileInOutput">If false - will NOT add FileO sections in blocks with overlapping</param>
        /// <returns>Message, "" - if no message for you</returns>
        string Merge(List<string> fileA, List<string> fileB, List<string> fileO, out List<string> outputFile, 
            bool includeOriginalFileInOutput);

        event EventHandler ProgressChanged;
    }
}
