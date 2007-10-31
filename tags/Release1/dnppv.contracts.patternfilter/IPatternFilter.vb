Imports dnppv.contracts.fileadapter
Imports dnppv.contracts.patternrecognizer

Public Interface IPatternFilter
    Function Analyse(ByVal file As IFileAdapter) As IPatternList
End Interface
