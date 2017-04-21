Imports System.Drawing
Imports System.Windows.Forms

Public Class GridData
    Inherits Panel

    Public Const ROW_HEIGHT As Integer = 50
    Public Shared ReadOnly BORDER_COLOR As Color = Color.FromArgb(255, 219, 223, 230)

    Private WithEvents m_hScrollBar As HScrollBar
    Private WithEvents m_vScrollBar As VScrollBar
    Private WithEvents m_hBackPanel As Panel
    Private WithEvents m_hScrollPanel As Panel
    Private WithEvents m_headersPanel As Panel
    Private WithEvents m_vBackPanel As Panel
    Private WithEvents m_vScrollPanel As Panel

    Private m_columns As List(Of GridDataColumn)
    Private m_rows As List(Of GridDataRow)


    Event ValueChanged(sender As Object, e As EventArgs)
    Event SelectionChanged(sender As Object, e As GridDataSelectionChangedEventArgs)

    Public Sub New()
        MyBase.New()

        m_columns = New List(Of GridDataColumn)
        m_rows = New List(Of GridDataRow)

        InitComponents()

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()

        m_columns.Clear()
        m_columns = Nothing

    End Sub

    Private Sub InitComponents()

        Me.SuspendLayout()

        m_hScrollBar = New HScrollBar
        With m_hScrollBar
            .Parent = Me
            .Dock = DockStyle.Bottom
            .Minimum = 0
            .Visible = True
        End With

        m_vScrollBar = New VScrollBar
        With m_vScrollBar
            .Parent = Me
            .Dock = DockStyle.Right
            .Minimum = 0
            .Visible = True
        End With

        m_hBackPanel = New Panel
        With m_hBackPanel
            .Parent = Me
            .BringToFront()
            .Dock = DockStyle.Fill
            .BorderStyle = Windows.Forms.BorderStyle.None
            .Visible = True
        End With

        m_hScrollPanel = New Panel
        With m_hScrollPanel
            .Parent = m_hBackPanel
            .Width = m_hBackPanel.Width
            .Height = m_hBackPanel.Height
            .Top = 0
            .Left = 0
            .BorderStyle = Windows.Forms.BorderStyle.None
            .Visible = True
        End With

        m_headersPanel = New Panel
        With m_headersPanel
            .Parent = m_hScrollPanel
            .Dock = DockStyle.Top
            .Height = 25
            .BorderStyle = Windows.Forms.BorderStyle.None
            .Visible = True
        End With

        m_vBackPanel = New Panel
        With m_vBackPanel
            .Parent = m_hScrollPanel
            .BringToFront()
            .Dock = DockStyle.Fill
            .BorderStyle = Windows.Forms.BorderStyle.None
            .Visible = True
        End With

        m_vScrollPanel = New Panel
        With m_vScrollPanel
            .Parent = m_vBackPanel
            .Top = 0
            .Left = 0
            .Width = m_vBackPanel.Width
            .Height = m_vBackPanel.Height
            .BorderStyle = Windows.Forms.BorderStyle.None
            .Visible = True
        End With

        Me.ResumeLayout(True)

    End Sub

    Public Function Rows() As List(Of GridDataRow)
        Return m_rows.ToList
    End Function

    Public Function AddColumn(Text As String) As GridDataColumn
        Return AddColumn(Text, CellTypes.Text)
    End Function

    Public Function AddColumn(Text As String, CellType As CellTypes) As GridDataColumn

        Dim c As New GridDataColumn(CellType)
        With c
            .Parent = m_headersPanel
            .Text = Text
            .AutoSize = False
            .Width = 60
            .SendToBack()
            .Dock = DockStyle.Left
            .BorderStyle = Windows.Forms.BorderStyle.None
            .TextAlign = ContentAlignment.MiddleLeft
            .BringToFront()
            .Visible = True
        End With

        m_columns.Add(c)

        Dim w As Integer
        w = 0

        For Each col As GridDataColumn In m_columns
            w += col.Width
        Next

        m_hScrollPanel.Width = w

        Return c

    End Function

    Public Function AddRow() As GridDataRow

        Dim r As New GridDataRow(Me, m_columns)
        m_rows.Add(r)

        With r
            .Parent = m_vScrollPanel
            .BringToFront()
            .Dock = DockStyle.Top
            .Visible = True
        End With

        m_vScrollPanel.Height = m_rows.Count * ROW_HEIGHT

        AddHandler r.ValueChanged, AddressOf OnValueChanged
        AddHandler r.SelectionChanged, AddressOf OnSelectionChanged

        Return r

    End Function

    Public Sub UnselectAll()

        For Each r As GridDataRow In m_rows
            r.UnselectRow()
        Next

    End Sub

    Private Sub m_backPanel_SizeChanged(sender As Object, e As EventArgs) Handles m_hBackPanel.SizeChanged

        m_hScrollPanel.Height = m_hBackPanel.Height

        If m_hScrollPanel.Width < m_hBackPanel.Width Then
            m_hScrollPanel.Width = m_hBackPanel.Width
        End If

    End Sub

    Private Sub m_hScrollPanel_SizeChanged(sender As Object, e As EventArgs) Handles m_hScrollPanel.SizeChanged

        m_vScrollPanel.Width = m_hScrollPanel.Width

        If m_vScrollPanel.Height < m_hScrollPanel.Height Then
            m_vScrollPanel.Height = m_hScrollPanel.Height
        End If

        m_hScrollBar.Minimum = 0
        m_hScrollBar.Maximum = m_hScrollPanel.Width - m_hBackPanel.Width

    End Sub

    Private Sub m_vScrollPanel_SizeChanged(sender As Object, e As EventArgs) Handles m_vScrollPanel.SizeChanged

        m_vScrollBar.Minimum = 0
        m_vScrollBar.Maximum = m_vScrollPanel.Height - m_hScrollPanel.Height

    End Sub

    Private Sub m_vScrollBar_Scroll(sender As Object, e As ScrollEventArgs) Handles m_vScrollBar.Scroll
        m_vScrollPanel.Top = -m_vScrollBar.Value
    End Sub

    Private Sub m_hScrollBar_Scroll(sender As Object, e As ScrollEventArgs) Handles m_hScrollBar.Scroll
        m_hScrollPanel.Left = -m_hScrollBar.Value
    End Sub

    Private Sub OnValueChanged(sender As Object, e As EventArgs)
        RaiseEvent ValueChanged(sender, e)
    End Sub

    Private Sub OnSelectionChanged(sender As Object, e As GridDataSelectionChangedEventArgs)
        RaiseEvent SelectionChanged(sender, e)
    End Sub

End Class
