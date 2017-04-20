Namespace Skins

    Public Class SESquare
        Inherits SERectangle

        Public Sub New(size As Integer)
            Me.New(0, 0, size)
        End Sub

        Public Sub New(x As Integer, y As Integer, size As Integer)
            MyBase.New(x, y, size, size)
        End Sub

    End Class

End Namespace