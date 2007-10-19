Public Interface IFileAdapter
    Inherits IDisposable

    Sub Open(ByVal filename As String)
    Sub Close()

    Function Read() As Boolean
    Property CurrentSignal() As String

    Property Length() As Integer
End Interface
