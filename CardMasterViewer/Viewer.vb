Imports CardMasterCard.Card
Imports CardMasterImageBuilder
Imports System.Windows.Forms

Public Class Viewer
    Inherits PictureBox

    Private m_cardDrawer As New Drawer

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub ShowCard(card As Card)
        Me.Image = m_cardDrawer.DrawCard(card)
    End Sub

End Class
