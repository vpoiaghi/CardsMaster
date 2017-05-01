Imports CardMasterCard.Card
Imports CardMasterSkin.Skins
Imports System.Drawing
Imports System.IO

Public Class SkinFactory

    Public Shared Function GetSkin(card As Card, texturesDirectory As DirectoryInfo) As Skin

        Dim w As Integer = 375
        Dim h As Integer = 523
        Dim borderSize As Integer = 15

        Dim imagesDir As New DirectoryInfo("F:\Programmation\VBA\Cartes Bruno\Cartes\Images")
        'Dim texturesDir As New DirectoryInfo("F:\Programmation\VBA\Cartes Bruno\Cartes\Textures")

        Dim skin As New Skin(w, h, imagesDir, texturesDirectory)
        Dim skinElement As SkinElement

        ' Bordures
        skinElement = New SERoundedRectangle(w, h, borderSize)
        skinElement.SetBackground(Color.Black)
        skin.Elements.Add(skinElement)

        ' Texture de fond
        skinElement = New SERectangle(borderSize, borderSize, w - borderSize * 2, h - borderSize * 2)
        skinElement.SetBackground(texturesDirectory, "eau")
        skin.Elements.Add(skinElement)

        ' Zone entête
        skinElement = New SECurvedRectangle(22, 22, w - 44, 40, 8)
        skinElement.SetBackground(texturesDirectory, "Pierre 01")
        skin.Elements.Add(skinElement)

        skinElement = New SETextArea(22, 22, w - 44, 40, "<Nom>")
        CType(skinElement, SETextArea).TextAttribute = "Name"
        skin.Elements.Add(skinElement)

        ' Zone de coût
        skinElement = New SECircle(w - 65, 27, 30, 30, 360)
        skinElement.SetBackground(texturesDirectory, "Pierre 01")
        skinElement.SetShadow(3, 135)
        skin.Elements.Add(skinElement)

        skinElement = New SETextArea(w - 65, 27, 30, 30, "?")
        CType(skinElement, SETextArea).TextAttribute = "Cost"
        CType(skinElement, SETextArea).TextAlign = HorizontalAlignment.Center
        skin.Elements.Add(skinElement)

        ' Image
        skinElement = New SERectangle(30, 62, w - 60, 220)
        skinElement.SetBackground(Color.Blue)
        skin.Elements.Add(skinElement)

        ' Zone équipe
        skinElement = New SECurvedRectangle(22, 282, w - 44, 40, 8)
        skinElement.SetBackground(texturesDirectory, "Pierre 01")
        skin.Elements.Add(skinElement)

        skinElement = New SETextArea(22, 282, w - 44, 40, "<Equipe>")
        CType(skinElement, SETextArea).TextAttribute = "Team"
        skin.Elements.Add(skinElement)

        ' Zone de pouvoirs
        skinElement = New SERectangle(30, 322, w - 60, 150)
        skinElement.SetBackground(texturesDirectory, "Pierre 01")
        skin.Elements.Add(skinElement)

        skinElement = New SETextArea(30, 322, w - 60, 160, "<Epouvoirs>")
        CType(skinElement, SETextArea).TextAttribute = "Powers"
        skin.Elements.Add(skinElement)


        Return skin

    End Function

End Class
