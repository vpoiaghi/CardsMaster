Imports System.Drawing

Namespace Skins

    Public Class SETextArea
        Inherits SkinElement

        Private m_text As String

        Public Sub New(width As Integer, height As Integer)
            Me.New(0, 0, width, height, Nothing)
        End Sub

        Public Sub New(width As Integer, height As Integer, text As String)
            Me.New(0, 0, width, height, text)
        End Sub

        Public Sub New(x As Integer, y As Integer, width As Integer, height As Integer)
            Me.New(x, y, width, height, Nothing)
        End Sub

        Public Sub New(x As Integer, y As Integer, width As Integer, height As Integer, text As String)
            MyBase.New(x, y, width, height)

            m_text = text

        End Sub

        Public Overrides Sub Draw(g As Graphics)

            Dim f As New Font("Arial", 15)

            g.DrawString(m_text, f, Brushes.Black, X, Y)

        End Sub

    End Class

End Namespace