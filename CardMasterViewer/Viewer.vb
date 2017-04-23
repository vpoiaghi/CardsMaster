Imports CardMasterCard.Card
Imports CardMasterImageBuilder
Imports System.Windows.Forms

Public Class Viewer
    Inherits PictureBox

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub ShowCard(card As Card)

        Dim cardDrawer As New Drawer(card)
        Me.Image = cardDrawer.DrawCard()

    End Sub

End Class
