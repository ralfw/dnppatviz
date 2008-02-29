Imports dnppv.contracts.fileadapter
Imports dnppv.contracts.domainmodel

Public Interface IPatternFilter
    Function Analyse(ByVal file As IFileAdapter) As IPatternList
End Interface
