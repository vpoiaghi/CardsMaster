Imports CardMasterCard.Card
Imports CardMasterSkin.Skins
Imports System.Drawing
Imports System.IO

Public Class SkinFactory

    Public Shared Function GetSkin(card As Card, skinsFile As FileInfo, skinName As String) As Skin

        Dim skin As Skin = Nothing

        Dim w As Integer = 375
        Dim h As Integer = 523
        Dim borderSize As Integer = 15

        Dim skinsProject As SkinsProject = SkinsProject.LoadProject(skinsFile)

        If skinsProject IsNot Nothing Then

            Dim texturesDirectory As New DirectoryInfo(skinsProject.TexturesDirectory)
            Dim imagesDirectory As New DirectoryInfo(skinsProject.ImagesDirectory)

            skin = New Skin(w, h, imagesDirectory, texturesDirectory)
            Dim skinElement As SkinElement

            ' Bordures
            skinElement = New SERoundedRectangle(skin, w, h, borderSize)
            skinElement.SetBackground(Color.Black)
            skin.Elements.Add(skinElement)

            ' Texture de fond
            skinElement = New SERectangle(skin, borderSize, borderSize, w - borderSize * 2, h - borderSize * 2)
            skinElement.SetBackground("eau")
            skin.Elements.Add(skinElement)

            ' Zone entête
            skinElement = New SECurvedRectangle(skin, 22, 22, w - 44, 40, 8)
            skinElement.SetBackground("Pierre 01")
            skin.Elements.Add(skinElement)

            skinElement = New SETextArea(skin, 22, 22, w - 44, 40, "<Nom>")
            CType(skinElement, SETextArea).TextAttribute = "Name"
            CType(skinElement, SETextArea).TextVerticalAlign = VerticalAlignment.Center
            skin.Elements.Add(skinElement)

            ' Zone de coût
            skinElement = New SEImage(skin, w - 65, 27, 30, 30, "mana_circle")
            skin.Elements.Add(skinElement)

            skinElement = New SETextArea(skin, w - 65, 27, 30, 30, "?")
            CType(skinElement, SETextArea).TextAttribute = "Cost"
            CType(skinElement, SETextArea).TextAlign = HorizontalAlignment.Center
            CType(skinElement, SETextArea).TextVerticalAlign = VerticalAlignment.Center
            skin.Elements.Add(skinElement)

            ' Image
            skinElement = New SEImage(skin, 30, 62, w - 60, 220)
            CType(skinElement, SEImage).NameAttribute = "Name"
            CType(skinElement, SEImage).ResourceType = ResourceTypes.Image
            skin.Elements.Add(skinElement)

            ' Zone équipe
            skinElement = New SECurvedRectangle(skin, 22, 282, w - 44, 40, 8)
            skinElement.SetBackground("Pierre 01")
            skin.Elements.Add(skinElement)

            skinElement = New SETextArea(skin, 22, 282, w - 44, 40, "<Equipe>")
            CType(skinElement, SETextArea).TextAttribute = "Team"
            CType(skinElement, SETextArea).TextVerticalAlign = VerticalAlignment.Center
            skin.Elements.Add(skinElement)

            ' Zone de pouvoirs
            skinElement = New SERectangle(skin, 30, 322, w - 60, 150)
            skinElement.SetBackground("Pierre 01")
            skin.Elements.Add(skinElement)

            skinElement = New SETextArea(skin, 35, 327, w - 70, 140, "<Epouvoirs>")
            CType(skinElement, SETextArea).TextAttribute = "Powers"
            skin.Elements.Add(skinElement)

        End If

        Return skin

    End Function

End Class
