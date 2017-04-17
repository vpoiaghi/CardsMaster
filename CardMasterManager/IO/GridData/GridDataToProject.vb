Imports CardMasterCard.Card

Public Class GridDataToProject

    Public Sub SaveProject(g As GridData, cardsProject As CardsProject)


        Dim i As Integer

        Dim cardsSet As CardsSet = cardsProject.Cards
        Dim card As Card
        Dim cells As List(Of GridDataCell)

        cardsSet.Clear()


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
            i += 1 'card.Powers = cells(i).Value
            i += 1 : card.Citation = cells(i).Value
            i += 1 : card.Comments = cells(i).Value
            i += 1 : card.Background.Name = cells(i).Value

            cardsSet.Add(card)

        Next

    End Sub

End Class
