Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports CardMasterCard.Card
Imports CardMasterSkin.GraphicsElement

Namespace Skins

    Public Class SERectangle
        Inherits SkinElement

        Public Sub New(skin As Skin, width As Integer, height As Integer)
            Me.New(skin, 0, 0, width, height)
        End Sub

        Public Sub New(skin As Skin, x As Integer, y As Integer, width As Integer, height As Integer)
            MyBase.New(skin, x, y, width, height)
        End Sub

        Protected Overrides Function GetGraphicElements(card As Card) As List(Of GraphicElement)

            Dim graphicElementsList As New List(Of GraphicElement)()
            Dim path As New GraphicsPath()

            path.AddRectangle(New Rectangle(X, Y, Width, Height))

            graphicElementsList.Add(New PathElement(path, GetBackground))

            Return graphicElementsList

        End Function

    End Class

End Namespace