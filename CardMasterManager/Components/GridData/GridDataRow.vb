Imports System.Drawing
Imports System.Windows.Forms

Public Class GridDataRow
    Inherits Panel

    Private m_parentGrid As GridData

    Private m_cells As New List(Of GridDataCell)
    Private m_lefts As New List(Of Integer)
    Private m_widths As New List(Of Integer)

    Event ValueChanged(sender As Object, e As EventArgs)
    Event SelectionChanged(sender As Object, e As GridDataSelectionChangedEventArgs)

    Public Sub New(parentGrid As GridData, columns As List(Of GridDataColumn))
        m_parentGrid = parentGrid

        With Me
            .Height = GridData.ROW_HEIGHT
            .BorderStyle = Windows.Forms.BorderStyle.None
            .BackColor = Color.White
        End With

        For Each col As GridDataColumn In columns
            m_lefts.Add(col.Left)
            m_widths.Add(col.Width)



            AddCell(col.CellType)
        Next


    End Sub

    Public Function AddCell(cellType As CellTypes) As GridDataCell

        Dim c As New GridDataCell(Me, cellType)

        Select Case cellType
            Case CellTypes.Text
                c.Left = m_lefts(m_cells.Count) + 5
                c.Width = m_widths(m_cells.Count) - 5
            Case CellTypes.Check
                c.Left = m_lefts(m_cells.Count) + 1
                c.Width = m_widths(m_cells.Count) - 1
            Case CellTypes.Combo
                c.Left = m_lefts(m_cells.Count)
                c.Width = m_widths(m_cells.Count) + 1
            Case CellTypes.StaticText
                c.Left = m_lefts(m_cells.Count) + 5
                c.Width = m_widths(m_cells.Count) - 5
        End Select

        AddHandler c.ValueChanged, AddressOf Cell_ValueChanged

        m_cells.Add(c)

        Return c

    End Function

    Public Function Cells() As List(Of GridDataCell)
        Return m_cells.ToList
    End Function

    Private Sub m_rowPanel_Click(sender As Object, e As EventArgs) Handles Me.Click
        SelectRow()
    End Sub

    Public Sub SelectRow()

        m_parentGrid.UnselectAll()

        Me.BackColor = Color.Aquamarine

        For Each c As GridDataCell In m_cells
            c.BackColor = Color.Aquamarine
        Next

        RaiseEvent SelectionChanged(m_parentGrid, New GridDataSelectionChangedEventArgs(Me))

    End Sub

    Public Sub UnselectRow()

        Me.BackColor = Color.White

        For Each c As GridDataCell In m_cells
            c.BackColor = Color.White
        Next

    End Sub

    Private Sub m_rowPanel_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint

        Dim i As Integer = 0
        Dim p As Pen = New Pen(GridData.BORDER_COLOR, 1)

        For i = 0 To m_lefts.Count - 1
            e.Graphics.DrawRectangle(p, New Rectangle(m_lefts(i), 0, m_widths(i), GridData.ROW_HEIGHT))
        Next

    End Sub

    Private Sub Cell_ValueChanged(sender As Object, e As EventArgs)
        RaiseEvent ValueChanged(sender, e)
    End Sub


End Class