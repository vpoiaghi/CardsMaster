Namespace Skins

    Public Class SkinElement

        Public Property X As Integer
        Public Property Y As Integer
        Public Property Width As Integer
        Public Property Height As Integer
        Public Property Type As SkinElementTypes

        Public Sub New(type As SkinElementTypes, width As Integer, height As Integer)
            Me.Type = type
            Me.X = 0
            Me.Y = 0
            Me.Width = width
            Me.Height = height
            Me.Type = type
        End Sub

    End Class

End Namespace