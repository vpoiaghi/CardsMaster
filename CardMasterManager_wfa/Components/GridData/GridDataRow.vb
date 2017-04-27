Imports System.Drawing
Imports System.Windows.Forms

Public Class GridDataRow
    Inherits Panel

    Private m_parentGrid As GridData

    Private m_cells As New List(Of GridDataCell)
    Private m_columns As List(Of GridDataColumn)

    Event ValueChanged(sender As Object, e As EventArgs)
    Event SelectionChanged(sender As Object, e As GridDataSelectionChangedEventArgs)

    Public Sub New(parentGrid As GridData, columns As List(Of GridDataColumn))

        m_parentGrid = parentGrid
        m_columns = columns

        With Me
            .Height = GridData.ROW_HEIGHT
            .BorderStyle = Windows.Forms.BorderStyle.None
            .BackColor = Color.White
            .Margin = New Padding(0)
        End With

        AddCells(m_columns)

    End Sub

    Private Sub AddCells(columns As List(Of GridDataColumn))

        Dim w As Integer = 0

        For Each col As GridDataColumn In columns
            AddCell(col)
            w += col.Width
        Next

        Me.Width = w

    End Sub

    Private Sub AddCell(col As GridDataColumn)

        Dim c As GridDataCell = Nothing

        Select Case col.CellType
            Case CellTypes.Text : c = New GridDataTextBox(Me)
            Case CellTypes.Check : c = New GridDataCheckBox(Me)
            Case CellTypes.Combo : c = New GridDataComboBox(Me)
            Case CellTypes.RichText : c = New GridDataRichTextBox(Me)
            Case CellTypes.StaticText : c = New GridDataLabel(Me)
        End Select

        If c IsNot Nothing Then

            c.Left = col.Left + 3
            c.Width = col.Width - 6

            AddHandler c.ValueChanged, AddressOf Cell_ValueChanged

            m_cells.Add(c)
        End If

    End Sub

    Public Function Cells() As List(Of GridDataCell)
        Return m_cells.ToList
    End Function

    Public Sub SelectRow()

        m_parentGrid.UnselectAll()

        ColorRow(Color.Aquamarine)

        RaiseEvent SelectionChanged(m_parentGrid, New GridDataSelectionChangedEventArgs(Me))

    End Sub

    Public Sub UnselectRow()

        ColorRow(Color.White)

    End Sub

    Private Sub ColorRow(color As Color)

        Me.BackColor = color

        For Each c As GridDataCell In m_cells
            c.BackColor = color

            For Each ctrl As Control In c.Controls
                ctrl.BackColor = color
            Next
        Next

    End Sub

    Private Sub m_rowPanel_Click(sender As Object, e As EventArgs) Handles Me.Click
        SelectRow()
    End Sub

    Private Sub m_rowPanel_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint

        Dim i As Integer = 0
        Dim p As Pen = New Pen(GridData.BORDER_COLOR, 1)

        For i = 0 To m_columns.Count - 1
            With m_columns(i)
                e.Graphics.DrawRectangle(p, New Rectangle(.Left, 0, .Width, GridData.ROW_HEIGHT))
            End With
        Next

    End Sub

    Private Sub Cell_ValueChanged(sender As Object, e As EventArgs)
        RaiseEvent ValueChanged(sender, e)
    End Sub


End Class