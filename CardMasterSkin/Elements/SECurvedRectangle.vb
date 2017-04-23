Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO

Namespace Skins

    Public Class SECurvedRectangle
        Inherits SkinElement

        Private m_curveSize As Integer

        Public Sub New(width As Integer, height As Integer, curveSize As Integer)
            Me.New(0, 0, width, height, curveSize)
        End Sub

        Public Sub New(x As Integer, y As Integer, width As Integer, height As Integer, curveSize As Integer)
            MyBase.New(x, y, width, height)

            m_curveSize = curveSize

        End Sub

        Public Function GetCurveSize() As Integer
            Return m_curveSize
        End Function

        Public Sub SetCurveSize(curveSize As Integer)
            m_curveSize = curveSize
        End Sub

        Public Overrides Sub Draw(g As Graphics)

            Dim w As Integer = Width
            Dim h As Integer = Height
            Dim c As Integer = GetCurveSize()
            Dim d As Integer = 2 * c

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

            g.FillPath(GetBackground, path)


        End Sub

    End Class

End Namespace