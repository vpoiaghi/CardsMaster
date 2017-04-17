Imports System.Drawing

Public Class GridDataCell

    Private m_parentRow As GridDataRow
    Private WithEvents m_ctrl As IGridDataComponent

    Public Property CellType As CellTypes

    Public Event ValueChanged(sender As Object, e As EventArgs)

    Public Sub New(parentRow As GridDataRow, cellType As CellTypes)

        Me.CellType = cellType
        m_parentRow = parentRow

        Select Case cellType
            Case CellTypes.Text : m_ctrl = New GridDataTextBox
            Case CellTypes.Check : m_ctrl = New GridDataCheckBox
            Case CellTypes.Combo : m_ctrl = New GridDataComboBox
            Case CellTypes.StaticText : m_ctrl = New GridDataLabel
        End Select

        With m_ctrl
            .Parent = m_parentRow
            .Height = GridData.ROW_HEIGHT
            .Visible = True
        End With

    End Sub

    Private Sub m_ctrl_GotFocus(sender As Object, e As EventArgs) Handles m_ctrl.GotFocus
        m_parentRow.SelectRow()
    End Sub

    Private Sub m_ctrl_SizeChanged(sender As Object, e As EventArgs) Handles m_ctrl.SizeChanged
        sender.top = (GridData.ROW_HEIGHT - sender.height) / 2
    End Sub

    Public Property Value As Object
        Get
            Return m_ctrl.Value
        End Get
        Set(value As Object)
            m_ctrl.Value = value
        End Set
    End Property

    Public Property Left As Integer
        Get
            Return m_ctrl.Left
        End Get
        Set(value As Integer)
            m_ctrl.Left = value
        End Set
    End Property

    Public Property Width As Integer
        Get
            Return m_ctrl.Width
        End Get
        Set(value As Integer)
            m_ctrl.Width = value
        End Set
    End Property

    Public Property BackColor As Color
        Get
            Return m_ctrl.BackColor
        End Get
        Set(value As Color)
            m_ctrl.BackColor = value
        End Set
    End Property

    Private Sub m_ctrl_ValueChanged(sender As Object, e As EventArgs) Handles m_ctrl.ValueChanged
        RaiseEvent ValueChanged(sender, e)
    End Sub
End Class
