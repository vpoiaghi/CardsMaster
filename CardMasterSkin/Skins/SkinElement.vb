Imports System.Drawing

Namespace Skins

    Public MustInherit Class SkinElement

        Public Property X As Integer
        Public Property Y As Integer
        Public Property Width As Integer
        Public Property Height As Integer

        Private m_background As Brush

        Protected Sub New(width As Integer, height As Integer)
            Me.New(0, 0, width, height)
        End Sub

        Protected Sub New(x As Integer, y As Integer, width As Integer, height As Integer)
            Me.X = x
            Me.Y = y
            Me.Width = width
            Me.Height = height

            m_background = New SolidBrush(Color.Black)

        End Sub

        Public Sub SetBackground(color As Color)
            m_background = New SolidBrush(color)
        End Sub

        Public Function GetBackground() As Brush
            Return m_background
        End Function

    End Class

End Namespace