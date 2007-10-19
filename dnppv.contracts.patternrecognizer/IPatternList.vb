Public Interface IPatternList
    Inherits IEnumerable(Of IPattern)

    Property SignalCount() As Integer
    Property Count() As Integer
    Property IPattern(ByVal index As Integer) As IPattern
End Interface

Public Interface IPattern
    Inherits IEnumerable(Of IPatternOccurrence)

    Property Size() As Integer
    Property Count() As Integer
    Property IPattern(ByVal index As Integer) As IPatternOccurrence
End Interface

Public Interface IPatternOccurrence
    Property Start() As Integer
    Property [End]() As Integer
End Interface