Public Class CellTypes

    Public Shared ReadOnly Text = New CellTypes()
    Public Shared ReadOnly RichText = New CellTypes()
    Public Shared ReadOnly Check = New CellTypes()
    Public Shared ReadOnly Combo = New CellTypes()
    Public Shared ReadOnly StaticText = New CellTypes()


    Private Shared INDEX As Integer = 0
    Private m_index As Integer

    Private Sub New()
        m_index = INDEX
        INDEX += 1
    End Sub

    Public Shared Operator <>(ByVal left As CellTypes, ByVal right As CellTypes) As Boolean
        Return left.m_index <> right.m_index
    End Operator

    Public Shared Operator =(ByVal left As CellTypes, ByVal right As CellTypes) As Boolean
        Return left.m_index = right.m_index
    End Operator


End Class
