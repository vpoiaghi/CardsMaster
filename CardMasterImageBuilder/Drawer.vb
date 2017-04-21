Imports CardMasterCard.Card
Imports CardMasterSkin.Skins
Imports System.Drawing
Imports System.IO

Public Class Drawer

    Public Sub New()
    End Sub

    Public Function DrawCard(card As Card) As Image

        Dim skin As Skin = SkinFactory.GetSkin(card)

        Dim img As New Bitmap(skin.Width, skin.Height)
        img.SetResolution(96, 96)

        Dim g As Graphics = Graphics.FromImage(img)

        g.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias

        For Each e As SkinElement In skin.Elements
            DrawElement(g, e, card)
        Next

        g.Dispose()

        Return img

    End Function

    Private Sub DrawElement(g As Graphics, e As SkinElement, card As Card)

        Select Case e.GetType
            Case GetType(SERectangle) : DrawRectangle(g, e, card)
            Case GetType(SERoundedRectangle) : DrawRoundedRectangle(g, e, card)
            Case GetType(SECurvedRectangle) : DrawCurvedRectangle(g, e, card)
            Case GetType(SESquare) : DrawSquare(g, e, card)
            Case GetType(SECircle) : DrawCircle(g, e, card)
            Case GetType(SETextArea) : DrawTextArea(g, e, card)
        End Select

    End Sub

    Private Sub DrawRectangle(g As Graphics, e As SkinElement, card As Card)
        e.Draw(g)
    End Sub

    Private Sub DrawRoundedRectangle(g As Graphics, e As SkinElement, card As Card)
        e.Draw(g)
    End Sub

    Private Sub DrawCurvedRectangle(g As Graphics, e As SkinElement, card As Card)
        e.Draw(g)
    End Sub

    Private Sub DrawSquare(g As Graphics, e As SkinElement, card As Card)
        e.Draw(g)
    End Sub

    Private Sub DrawCircle(g As Graphics, e As SkinElement, card As Card)
        e.Draw(g)
    End Sub

    Private Sub DrawTextArea(g As Graphics, e As SkinElement, card As Card)
        e.Draw(g)
    End Sub

End Class
