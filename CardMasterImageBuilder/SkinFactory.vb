Imports CardMasterCard.Card
Imports CardMasterSkin.Skins
Imports System.Drawing
Imports System.IO

Public Class SkinFactory

    Public Shared Function GetSkin(card As Card) As Skin

        Dim w As Integer = 375
        Dim h As Integer = 523
        Dim borderSize As Integer = 15
        Dim margin As Integer

        Dim imagesDir As New DirectoryInfo("F:\Programmation\VBA\Cartes Bruno\Cartes\Images")
        Dim texturesDir As New DirectoryInfo("F:\Programmation\VBA\Cartes Bruno\Cartes\Textures")

        Dim skin As New Skin(w, h, imagesDir, texturesDir)
        Dim skinElement As SkinElement

        ' Bordures
        skinElement = New SERoundedRectangle(w, h, borderSize)
        skinElement.SetBackground(Color.Black)
        skin.Elements.Add(skinElement)

        ' Texture de fond
        skinElement = New SERectangle(borderSize, borderSize, w - borderSize * 2, h - borderSize * 2)
        skinElement.SetBackground(Color.Green)
        skin.Elements.Add(skinElement)

        ' Zone entête
        margin = 7
        skinElement = New SECurvedRectangle(borderSize + margin, borderSize + margin, w - ((borderSize + margin) * 2), 40, 8)
        skinElement.SetBackground(Color.Yellow)
        skin.Elements.Add(skinElement)

        ' Image
        skinElement = New SERectangle(borderSize + margin + 8, borderSize + margin + 40, w - ((borderSize + margin + 8) * 2), 220)
        skinElement.SetBackground(Color.Blue)
        skin.Elements.Add(skinElement)

        ' Zone équipe
        skinElement = New SECurvedRectangle(borderSize + margin, borderSize + margin + 40 + 220, w - ((borderSize + margin) * 2), 40, 8)
        skinElement.SetBackground(Color.Yellow)
        skin.Elements.Add(skinElement)

        ' Zone de pouvoirs
        skinElement = New SERectangle(borderSize + margin + 8, borderSize + margin + 40 + 220 + 40, w - ((borderSize + margin + 8) * 2), 150)
        skinElement.SetBackground(Color.Blue)
        skin.Elements.Add(skinElement)

        ' Zone de coût
        skinElement = New SECircle(w - 60, borderSize + margin + 5, 30, 30, 360)
        skinElement.SetBackground(Color.Brown)
        skin.Elements.Add(skinElement)

        Return skin

    End Function

End Class
