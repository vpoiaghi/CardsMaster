Imports System.Drawing.Drawing2D
Imports CardMasterCard.Card
Imports CardMasterSkin.GraphicsElement

Namespace Skins

    Public Class SECurvedRectangle
        Inherits SkinElement

        Private m_curveSize As Integer

        Public Sub New(skin As Skin, width As Integer, height As Integer, curveSize As Integer)
            Me.New(skin, 0, 0, width, height, curveSize)
        End Sub

        Public Sub New(skin As Skin, x As Integer, y As Integer, width As Integer, height As Integer, curveSize As Integer)
            MyBase.New(skin, x, y, width, height)

            m_curveSize = curveSize

        End Sub

        Public Function GetCurveSize() As Integer
            Return m_curveSize
        End Function

        Public Sub SetCurveSize(curveSize As Integer)
            m_curveSize = curveSize
        End Sub

        Protected Overrides Function GetGraphicElements(card As Card) As List(Of GraphicElement)

            Dim w As Integer = Width
            Dim h As Integer = Height
            Dim c As Integer = GetCurveSize()
            Dim d As Integer = 2 * c

            Dim graphicElementsList As New List(Of GraphicElement)()
            Dim path As New GraphicsPath()

            path.StartFigure()

            ' Côté gauche
            path.AddArc(X, Y, d, h, 90, 180)
            ' Côté haut
            path.AddLine(X + c, Y, X + w - c, Y)
            ' Côté droit
            path.AddArc(X + w - d, Y, d, h, 270, 180)

            ' Côté bas
            path.CloseFigure()

            graphicElementsList.Add(New PathElement(path, GetBackground))

            Return graphicElementsList

        End Function

    End Class

End Namespace