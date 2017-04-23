Imports CardMasterCard.Card

Public Class GridDataToProject

    Public Sub SaveProject(g As GridData, cardsProject As CardsProject)


        Dim i As Integer

        Dim cardsSet As New CardsSet
        Dim card As Card
        Dim cells As List(Of GridDataCell)

        For Each r As GridDataRow In g.Rows

            cells = r.Cells

            i = 0
            card = New Card(cells(0).Value)

            i += 1 : card.Kind = cells(i).Value
            i += 1 : card.Rank = cells(i).Value
            i += 1 : card.Team = cells(i).Value
            i += 1 : card.Chakra = cells(i).Value
            i += 1 : card.Element = cells(i).Value
            i += 1 : card.Cost = cells(i).Value
            i += 1 : card.Attack = cells(i).Value
            i += 1 : card.Defense = cells(i).Value
            i += 1 : card.Powers = PowersFromString(cells(i).Value)
            i += 1 : card.Citation = cells(i).Value
            i += 1 : card.Comments = cells(i).Value
            i += 1 : card.Background.Name = cells(i).Value

            cardsSet.Add(card)

        Next

        cardsProject.Cards = cardsSet

    End Sub

    Private Function PowersFromString(ByVal powers As String) As List(Of Power)

        Dim powersList As New List(Of Power)

        powers = powers.Trim

        If Not String.IsNullOrEmpty(powers) Then

            Dim powersArray() As String = powers.Split(vbLf)
            Dim power As Power

            For Each powerDescription As String In powersArray

                power = New Power()
                power.Description = powerDescription

                powersList.Add(power)

            Next

        End If

        Return powersList

    End Function

End Class
