Public Interface IFileAdapterFactory
    Function CreateFileAdapter(ByVal filename As String) As IFileAdapter
    ReadOnly Property Filetypes() As Dictionary(Of String, String)
End Interface
