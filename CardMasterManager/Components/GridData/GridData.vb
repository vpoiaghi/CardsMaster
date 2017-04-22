Imports System.Drawing

Public Class GridData
    Inherits Panel

    Public Const ROW_HEIGHT As Integer = 50
    Public Shared ReadOnly BORDER_COLOR As Color = Color.FromArgb(255, 219, 223, 230)

    Private m_backHeaderPanel As New Panel
    Private m_headerPanel As New Panel
    Private WithEvents m_flowLayout As New FlowLayoutPanel

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

        With m_backHeaderPanel
            '.Parent = m_hPanel
            .Parent = Me
            .Dock = DockStyle.Top
            .Height = 25
        End With

        With m_headerPanel
            .Parent = m_backHeaderPanel
            .Top = 0
            .Left = 0
            .Height = 25
        End With

        With m_flowLayout
            '.Parent = m_hPanel
            .Parent = Me
            .BringToFront()
            .Dock = DockStyle.Fill
            .AutoScroll = True
        End With

        With Me
            .AutoScroll = True
            .BorderStyle = Windows.Forms.BorderStyle.FixedSingle
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
            .Parent = m_headerPanel
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

        AddHandler c.SizeChanged, AddressOf ColumnSizeChanged

        m_columns.Add(c)
        ApplyWidth()

        Return c

    End Function

    Public Function AddRow() As GridDataRow

        Me.SuspendLayout()

        Dim r As New GridDataRow(Me, m_columns)
        m_rows.Add(r)
        m_flowLayout.Controls.Add(r)

        r.Visible = True

        AddHandler r.ValueChanged, AddressOf OnValueChanged
        AddHandler r.SelectionChanged, AddressOf OnSelectionChanged

        Me.ResumeLayout(False)

        Return r

    End Function

    Public Sub UnselectAll()

        For Each r As GridDataRow In m_rows
            r.UnselectRow()
        Next

    End Sub

    Private Sub ColumnSizeChanged(sender As Object, e As EventArgs)
        ApplyWidth()
    End Sub

    Private Sub OnValueChanged(sender As Object, e As EventArgs)
        RaiseEvent ValueChanged(sender, e)
    End Sub

    Private Sub OnSelectionChanged(sender As Object, e As GridDataSelectionChangedEventArgs)
        RaiseEvent SelectionChanged(sender, e)
    End Sub

    Private Sub ApplyWidth()

        Dim w As Integer = 0

        For Each col As GridDataColumn In m_columns
            w = w + col.Width
        Next

        If m_headerPanel.Width <> w Then
            m_headerPanel.Width = w
        End If

    End Sub

    Private Sub m_flowLayout_Scroll(sender As Object, e As ScrollEventArgs) Handles m_flowLayout.Scroll

        If e.ScrollOrientation = ScrollOrientation.HorizontalScroll Then

            m_headerPanel.Left = -e.NewValue

        End If

    End Sub
End Class
