Imports System.Drawing
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

        Public Overrides Sub Draw(g As Graphics)

            g.FillEllipse(GetBackground, X, Y, Width, Height)

        End Sub

    End Class

End Namespace