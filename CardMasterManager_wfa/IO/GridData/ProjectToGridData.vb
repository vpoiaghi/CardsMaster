Imports System.Text
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
                i += 1 : .Cells(i).Value = StringifyPower(c.Powers)
                i += 1 : .Cells(i).Value = c.Citation
                i += 1 : .Cells(i).Value = c.Comments
                i += 1 : .Cells(i).Value = c.Background

                .Tag = c
            End With

        Next

    End Sub

    Private Function StringifyPower(powers As List(Of Power)) As Object
        Dim sb As StringBuilder = New StringBuilder()

        Dim i As Integer
        For i = 0 To powers.Count - 1
            sb.Append(powers.ElementAt(i).Description)
            If (i <> powers.Count - 1) Then
                sb.Append(System.Environment.NewLine)

            End If
        Next
        Return sb.ToString()
    End Function
End Class
