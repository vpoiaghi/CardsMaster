Imports System.Drawing

Public MustInherit Class GridDataCell
    Inherits Panel

    Private m_parentRow As GridDataRow
    Protected m_cellType As CellTypes
    Protected WithEvents m_mainControl As Control

    Public MustOverride Property Value As Object

    Public Event ValueChanged(sender As Object, e As EventArgs)

    Public Sub New(parentRow As GridDataRow, cellType As CellTypes)
        MyBase.New()

        m_parentRow = parentRow

        With Me
            .Parent = m_parentRow
            .Top = 1
            .Height = GridData.ROW_HEIGHT - 2
            .Visible = True
            .Margin = New Padding(0)
        End With

    End Sub

    Protected Sub SendValueChangedEvent(sender, e)
        RaiseEvent ValueChanged(sender, e)
    End Sub

    Private Sub GridDataCell_GotFocus(sender As Object, e As EventArgs) Handles Me.GotFocus
        m_parentRow.SelectRow()
    End Sub

    Protected Sub SelectRow()
        m_parentRow.SelectRow()
    End Sub

    Private Sub m_mainControl_GotFocus(sender As Object, e As EventArgs) Handles m_mainControl.GotFocus
        SelectRow()
    End Sub

    Private Sub GridDataCell_MouseClick(sender As Object, e As MouseEventArgs) Handles Me.MouseClick
        SelectRow()
    End Sub

End Class
