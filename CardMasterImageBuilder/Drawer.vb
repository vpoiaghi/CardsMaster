Imports CardMasterCard.Card
Imports CardMasterSkin.Skins
Imports System.Drawing
Imports System.IO

Public Class Drawer

    Public Sub New()
    End Sub

    Public Function DrawCard(card As Card) As Image

        Dim skin As New Skin(300, 500, Nothing, Nothing)

        Return DrawCard(card, skin)

    End Function

    Public Function DrawCard(card As Card, skin As Skin) As Image

        Dim img As New Bitmap(Skin.Width, Skin.Height)
        img.SetResolution(96, 96)

        Dim g As Graphics = Graphics.FromImage(img)

        For Each e As SkinElement In Skin.Elements
            DrawElement(g, e, card)
        Next

        g.Dispose()

        Return img

    End Function

    Private Sub DrawElement(g As Graphics, e As SkinElement, card As Card)

        Select Case e.Type
            Case SkinElementTypes.Rectangle : DrawRectangle(g, e, card)
            Case SkinElementTypes.RoundedRectangle : DrawRoundedRectangle(g, e, card)
            Case SkinElementTypes.Square : DrawSquare(g, e, card)
            Case SkinElementTypes.Square : DrawRound(g, e, card)
            Case SkinElementTypes.Square : DrawTextArea(g, e, card)
        End Select

    End Sub

    Private Sub DrawRectangle(g As Graphics, e As SkinElement, card As Card)

        Dim brush As New SolidBrush(Color.Black)
        Dim w As Integer = e.Width
        Dim h As Integer = e.Height

        g.FillRectangle(brush, e.X, e.Y, e.Width, e.Height)

    End Sub

    Private Sub DrawRoundedRectangle(g As Graphics, e As SkinElement, card As Card)

        Dim brush As New SolidBrush(Color.Black)
        Dim w As Integer = e.Width
        Dim h As Integer = e.Height
        Dim r As Integer = 15

        ' Coin haut/gauche
        g.FillPie(brush, 0, 0, r, r, 180, 90)

        ' Coin haut/droit
        g.FillPie(brush, w - r, 0, r, r, 270, 90)

        ' Coin bas/gauche
        g.FillPie(brush, w - r, h - r, r, r, 0, 90)

        ' Coin bas/droit
        g.FillPie(brush, 0, h - r, r, r, 90, 90)

        ' Côté haut
        g.FillRectangle(brush, r, 0, w - (2 * r), r)

        ' Côté droit
        g.FillRectangle(brush, w - r, r, r, h - (2 * r))

        ' Côté bas
        g.FillRectangle(brush, r, h - r, w - (2 * r), r)

        ' Côté gauche
        g.FillRectangle(brush, 0, r, r, h - (2 * r))


    End Sub

    Private Sub DrawSquare(g As Graphics, e As SkinElement, card As Card)
    End Sub

    Private Sub DrawRound(g As Graphics, e As SkinElement, card As Card)
    End Sub

    Private Sub DrawTextArea(g As Graphics, e As SkinElement, card As Card)
    End Sub

End Class
