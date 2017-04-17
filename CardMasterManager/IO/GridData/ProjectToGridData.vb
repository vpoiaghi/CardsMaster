Imports CardMasterCard.Card

Public Class ProjectToGridData

    Public Sub LoadProject(cardsProject As CardsProject, g As GridData)

        Dim i As Integer

        For Each c As Card In cardsProject.Cards

            i = -1
            With g.AddRow()
                i += 1 : .Cells(i).Value = c.Name
                i += 1 : .Cells(i).Value = c.Kind
                i += 1 : .Cells(i).Value = c.Rank
                i += 1 : .Cells(i).Value = c.Team
                i += 1 : .Cells(i).Value = c.Chakra
                i += 1 : .Cells(i).Value = c.Element
                i += 1 : .Cells(i).Value = c.Cost
                i += 1 : .Cells(i).Value = c.Attack
                i += 1 : .Cells(i).Value = c.Defense
                i += 1 : .Cells(i).Value = ""
                i += 1 : .Cells(i).Value = c.Citation
                i += 1 : .Cells(i).Value = c.Comments
                i += 1 : .Cells(i).Value = c.Background.ToString

                .Tag = c
            End With

        Next

    End Sub

End Class
