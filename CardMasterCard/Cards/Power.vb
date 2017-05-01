Namespace Card

    Public Class Power
        Inherits ListItem

        Private m_description As String

        Public Sub New()
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

        Public Property Description As String
            Get
                Return m_description
            End Get
            Set(value As String)
                m_description = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.Description
        End Function

        Public Overrides Sub FromString(value As String)
            Me.Description = value
        End Sub

    End Class

End Namespace