Imports System.Drawing.Drawing2D
Imports CardMasterCard.Card
Imports CardMasterSkin.GraphicsElement

Namespace Skins

    Public Class SECircle
        Inherits SkinElement

        Dim m_radius As Integer

        Public Sub New(skin As Skin, radius As Integer)
            Me.New(skin, 0, 0, radius * 2, radius * 2, radius)
        End Sub

        Public Sub New(skin As Skin, x As Integer, y As Integer, width As Integer, height As Integer, radius As Integer)
            MyBase.New(skin, x, y, width, height)

            m_radius = radius

        End Sub

        Protected Overrides Function GetGraphicElements(card As Card) As List(Of GraphicElement)

            Dim graphicElementsList As New List(Of GraphicElement)()
            Dim path As New GraphicsPath()

            path.AddEllipse(X, Y, Width, Height)

            graphicElementsList.Add(New PathElement(path, GetBackground))

            path = Nothing

            Return graphicElementsList

        End Function


    End Class

End Namespace