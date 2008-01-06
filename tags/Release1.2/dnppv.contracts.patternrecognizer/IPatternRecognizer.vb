Imports dnppv.contracts.fileadapter

Public Interface IPatternRecognizer
    Function DetectPatterns(ByVal file As IFileAdapter) As IPatternList
End Interface
