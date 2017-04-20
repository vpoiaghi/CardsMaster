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
            Case GetType(SESquare) : DrawSquare(g, e, card)
            Case GetType(SECircle) : DrawCircle(g, e, card)
            Case GetType(SETextArea) : DrawTextArea(g, e, card)
        End Select

    End Sub

    Private Sub DrawRectangle(g As Graphics, e As SkinElement, card As Card)

        g.FillRectangle(e.GetBackground, e.X, e.Y, e.Width, e.Height)

    End Sub

    Private Sub DrawRoundedRectangle(g As Graphics, e As SkinElement, card As Card)

        With CType(e, SERoundedRectangle)

            Dim brush As Brush = e.GetBackground
            Dim w As Integer = e.Width
            Dim h As Integer = e.Height
            Dim b As Integer = .GetCornerRadius
            Dim r As Integer = 2 * b

            Dim br As New SolidBrush(Color.Yellow)

            ' Côté haut
            g.FillRectangle(brush, b, 0, w - (2 * b), b)

            ' Côté droit
            g.FillRectangle(brush, w - b, b, b, h - (2 * b))

            ' Côté bas
            g.FillRectangle(brush, b, h - b, w - (2 * b), b)

            ' Côté gauche
            g.FillRectangle(brush, 0, b, b, h - (2 * b))

            ' Coin haut/gauche
            g.FillPie(brush, 0, 0, r, r, 180, 90)

            ' Coin haut/droit
            g.FillPie(brush, w - r, 0, r, r, 270, 90)

            ' Coin bas/droit
            g.FillPie(brush, w - r, h - r, r, r, 0, 90)

            ' Coin bas/gauche
            g.FillPie(brush, 0, h - r, r, r, 90, 90)

        End With

    End Sub

    Private Sub DrawSquare(g As Graphics, e As SkinElement, card As Card)
    End Sub

    Private Sub DrawCircle(g As Graphics, e As SkinElement, card As Card)
    End Sub

    Private Sub DrawTextArea(g As Graphics, e As SkinElement, card As Card)
    End Sub

End Class
