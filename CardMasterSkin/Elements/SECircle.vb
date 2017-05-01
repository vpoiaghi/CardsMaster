Imports CardMasterCard.Card
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.IO

Namespace Skins

    Public Class SECircle
        Inherits SkinElement

        Dim m_radius As Integer

        Public Sub New(radius As Integer)
            Me.New(0, 0, radius * 2, radius * 2, radius)
        End Sub

        Public Sub New(x As Integer, y As Integer, width As Integer, height As Integer, radius As Integer)
            MyBase.New(x, y, width, height)

            m_radius = radius

        End Sub

        Public Overrides Function GetPath(card As Card) As GraphicsPath

            Dim path As New GraphicsPath()

            path.AddEllipse(X, Y, Width, Height)

            Return path

        End Function


    End Class

End Namespace