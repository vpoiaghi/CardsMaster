Imports CardMasterCard.Card
Imports CardMasterSkin.Skins
Imports System.Drawing
Imports System.IO

Public Class Drawer

    Private m_card As Card
    Private m_skin As Skin

    Public Sub New(card As Card, texturesDirectory As DirectoryInfo)
        m_card = card
        m_skin = SkinFactory.GetSkin(m_card, texturesDirectory)
    End Sub

    Public Function DrawCard() As Image

        Dim img As New Bitmap(m_skin.Width, m_skin.Height)
        img.SetResolution(96, 96)

        Dim g As Graphics = Graphics.FromImage(img)

        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        For Each e As SkinElement In m_skin.Elements
            e.Draw(g, m_card)
        Next

        g.Dispose()

        Return img

    End Function

End Class
