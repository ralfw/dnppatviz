Public Interface IPatternList
    Inherits IEnumerable(Of IPattern)

    ReadOnly Property SignalCount() As Integer
    ReadOnly Property Count() As Integer
    Default ReadOnly Property Pattern(ByVal index As Integer) As IPattern
End Interface

Public Interface IPattern
    Inherits IEnumerable(Of IPatternOccurrence)

    ReadOnly Property Size() As Integer
    ReadOnly Property Count() As Integer
    Default ReadOnly Property Occurrence(ByVal index As Integer) As IPatternOccurrence
End Interface

Public Interface IPatternOccurrence
    ReadOnly Property Start() As Integer
    ReadOnly Property [End]() As Integer
End Interface