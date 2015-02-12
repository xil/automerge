# automerge
This utility merges two files on a common source without syntax analyzing.

Command line call example:
merge <params> fileA fileB originalFile outputFile

params:
silent			       - don't display messages
notIncludeOriginal - do not display the version from the source file in the conflicting blocks

Conflict resolution patterns ( all are used by default )
pattern fileA_Original_fileB__result. 
N   - empty block
X,Y - block with value
any - any value or empty block
mrg - merged block
Add pattern to params for disabling:
X_N_N__X
N_N_X__X
X_any_X__X
X_N_Y__mrg
X_X_N__N
N_X_X__N
