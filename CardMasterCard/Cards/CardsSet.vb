Namespace Card

    Public Class CardsSet
        Inherits List(Of Card)

        Public Sub New()
            MyBase.New()
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

    End Class

End Namespace