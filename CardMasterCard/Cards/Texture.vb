Imports System.Drawing
Imports System.IO

Namespace Card

    Public Class Texture

        Private m_name As String = Nothing
        Private m_color As String = Nothing

        Public Sub New()
            Reset()
        End Sub

        Public Sub New(name As String)
            Me.Name = name
        End Sub

        Public Sub New(name As String, color As String)
            Me.Name = name
            Me.Color = color
        End Sub

        Public Property Name As String
            Get
                Return m_name
            End Get
            Set(value As String)
                m_name = value
            End Set
        End Property

        Public Property Color As String
            Get
                Return m_color
            End Get
            Set(value As String)
                m_color = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.Name
        End Function

    End Class

End Namespace