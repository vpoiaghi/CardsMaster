Namespace Skins

    Public Class SESquare
        Inherits SERectangle

        Public Sub New(skin As Skin, size As Integer)
            Me.New(skin, 0, 0, size)
        End Sub

        Public Sub New(skin As Skin, x As Integer, y As Integer, size As Integer)
            MyBase.New(skin, x, y, size, size)
        End Sub

    End Class

End Namespace