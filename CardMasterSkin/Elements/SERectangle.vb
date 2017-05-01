Imports CardMasterCard.Card
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO

Namespace Skins

    Public Class SERectangle
        Inherits SkinElement

        Public Sub New(width As Integer, height As Integer)
            Me.New(0, 0, width, height)
        End Sub

        Public Sub New(x As Integer, y As Integer, width As Integer, height As Integer)
            MyBase.New(x, y, width, height)
        End Sub

        Public Overrides Function GetPath(card As Card) As GraphicsPath

            Dim path As New GraphicsPath()

            path.AddRectangle(New Rectangle(X, Y, Width, Height))

            Return path

        End Function

    End Class

End Namespace