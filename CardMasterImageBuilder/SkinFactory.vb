Imports CardMasterCard.Card
Imports CardMasterSkin.Skins
Imports System.Drawing
Imports System.IO

Public Class SkinFactory

    Public Shared Function GetSkin(card As Card) As Skin

        Dim w As Integer = 375
        Dim h As Integer = 523
        Dim borderSize As Integer = 15

        Dim imagesDir As New DirectoryInfo("F:\Programmation\VBA\Cartes Bruno\Cartes\Images")
        Dim texturesDir As New DirectoryInfo("F:\Programmation\VBA\Cartes Bruno\Cartes\Textures")

        Dim skin As New Skin(w, h, imagesDir, texturesDir)

        Dim skinElement As SkinElement

        skinElement = New SERoundedRectangle(w, h, borderSize)
        skinElement.SetBackground(Color.Black)
        skin.Elements.Add(skinElement)

        skinElement = New SERectangle(borderSize, borderSize, w - borderSize * 2, h - borderSize * 2)
        skinElement.SetBackground(Color.Green)
        skin.Elements.Add(skinElement)

        Return skin

    End Function

End Class
