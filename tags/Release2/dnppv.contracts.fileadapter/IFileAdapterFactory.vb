Public Interface IFileAdapterFactory
    Function CreateFileAdapter(ByVal filename As String) As IFileAdapter
    ReadOnly Property FileExtensionsSupported() As String()
End Interface
