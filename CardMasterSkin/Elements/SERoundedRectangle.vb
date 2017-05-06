Imports System.Drawing.Drawing2D
Imports CardMasterCard.Card
Imports CardMasterSkin.GraphicsElement

Namespace Skins

    Public Class SERoundedRectangle
        Inherits SkinElement

        Private m_cornerRadius As Integer

        Public Sub New(skin As Skin, width As Integer, height As Integer, cornerRadius As Integer)
            Me.New(skin, 0, 0, width, height, cornerRadius)
        End Sub

        Public Sub New(skin As Skin, x As Integer, y As Integer, width As Integer, height As Integer, cornerRadius As Integer)
            MyBase.New(skin, x, y, width, height)

            m_cornerRadius = cornerRadius

        End Sub

        Public Function GetCornerRadius() As Integer
            Return m_cornerRadius
        End Function

        Public Sub SetCornerRadius(cornerRadius As Integer)
            m_cornerRadius = cornerRadius
        End Sub

        Protected Overrides Function GetGraphicElements(card As Card) As List(Of GraphicElement)

            Dim graphicElementsList As New List(Of GraphicElement)()

            Dim path As New GraphicsPath()

            Dim w As Integer = Width
            Dim h As Integer = Height
            Dim b As Integer = GetCornerRadius()
            Dim r As Integer = 2 * b

            path.StartFigure()

            ' Coin haut/gauche
            path.AddArc(0, 0, r, r, 180, 90)
            ' Côté haut
            path.AddLine(b, 0, w - b, 0)
            ' Coin haut/droit
            path.AddArc(w - r, 0, r, r, 270, 90)
            ' Côté droit
            path.AddLine(w, b, w, h - b)
            ' Coin bas/droit
            path.AddArc(w - r, h - r, r, r, 0, 90)
            ' Côté bas
            path.AddLine(w - b, h, b, h)
            ' Coin bas/gauche
            path.AddArc(0, h - r, r, r, 90, 90)

            path.CloseFigure()

            graphicElementsList.Add(New PathElement(path, GetBackground))

            path = Nothing

            Return graphicElementsList

        End Function

    End Class

End Namespace