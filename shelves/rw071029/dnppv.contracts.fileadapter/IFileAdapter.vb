Public Interface IFileAdapter
    Inherits IDisposable

    Sub Open(ByVal filename As String)
    Sub Close()

    Function Read() As Boolean
    ReadOnly Property CurrentSignal() As String

    ReadOnly Property Length() As Integer
End Interface
