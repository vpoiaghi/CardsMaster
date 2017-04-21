Imports System.Drawing

Namespace Skins

    Public Class SERectangle
        Inherits SkinElement

        Public Sub New(width As Integer, height As Integer)
            Me.New(0, 0, width, height)
        End Sub

        Public Sub New(x As Integer, y As Integer, width As Integer, height As Integer)
            MyBase.New(x, y, width, height)
        End Sub

        Public Overrides Sub Draw(g As Graphics)

            g.FillRectangle(GetBackground, X, Y, Width, Height)

        End Sub

    End Class

End Namespace