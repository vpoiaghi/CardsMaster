Public Class GridDataSelectionChangedEventArgs
    Inherits EventArgs

    Private m_row As GridDataRow

    Public Sub New(Row As GridDataRow)
        MyBase.New()

        m_row = Row

    End Sub

    Public Function GetRow() As GridDataRow
        Return m_row
    End Function

End Class
