Imports dnppv.contracts.fileadapter
Imports dnppv.contracts.domainmodel

Public Interface IPatternRecognizer
    Function DetectPatterns(ByVal file As IFileAdapter) As IPatternList
End Interface
