Imports CardMasterCard.Card
Imports CardMasterImageBuilder
Imports System.IO
Imports System.Windows.Forms

Public Class Viewer
    Inherits PictureBox

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub ShowCard(card As Card, texturesDirectory As DirectoryInfo)

        With New Drawer(card, texturesDirectory)
            Me.Image = .DrawCard()
        End With

    End Sub

    Public Sub EraseCard()

        Me.Image = Nothing

    End Sub

End Class
