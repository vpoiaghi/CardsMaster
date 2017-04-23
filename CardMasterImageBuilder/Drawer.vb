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
            DrawElement(g, e)
        Next

        g.Dispose()

        Return img

    End Function

    Private Sub DrawElement(g As Graphics, e As SkinElement)

        Select Case e.GetType
            Case GetType(SERectangle) : DrawRectangle(g, e)
            Case GetType(SERoundedRectangle) : DrawRoundedRectangle(g, e)
            Case GetType(SECurvedRectangle) : DrawCurvedRectangle(g, e)
            Case GetType(SESquare) : DrawSquare(g, e)
            Case GetType(SECircle) : DrawCircle(g, e)
            Case GetType(SETextArea) : DrawTextArea(g, e)
        End Select

    End Sub

    Private Sub DrawRectangle(g As Graphics, e As SkinElement)
        e.Draw(g)
    End Sub

    Private Sub DrawRoundedRectangle(g As Graphics, e As SkinElement)
        e.Draw(g)
    End Sub

    Private Sub DrawCurvedRectangle(g As Graphics, e As SkinElement)
        e.Draw(g)
    End Sub

    Private Sub DrawSquare(g As Graphics, e As SkinElement)
        e.Draw(g)
    End Sub

    Private Sub DrawCircle(g As Graphics, e As SkinElement)
        e.Draw(g)
    End Sub

    Private Sub DrawTextArea(g As Graphics, e As SkinElement)
        e.Draw(g)
    End Sub

End Class
