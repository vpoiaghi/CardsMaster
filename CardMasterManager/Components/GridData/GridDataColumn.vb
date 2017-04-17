Imports System.Drawing
Imports System.Windows.Forms
Imports System.Drawing.Drawing2D

Public Class GridDataColumn
    Inherits Label

    Private m_text As String
    Private m_cellType As CellTypes

    Public Sub New(CellType As CellTypes)
        MyBase.Text = ""
        m_cellType = CellType
    End Sub

    Public Overrides Property Text As String
        Get
            Return m_text
        End Get
        Set(value As String)
            m_text = value
            Refresh()
        End Set
    End Property

    Public Property CellType As CellTypes
        Get
            Return m_cellType
        End Get
        Set(value As CellTypes)
            m_cellType = value
            Refresh()
        End Set
    End Property

    Protected Overrides Sub OnPaint(ByVal e As PaintEventArgs)
        MyBase.OnPaint(e)

        Dim r1 As Rectangle = New Rectangle(0, 0, Me.Width, Me.Height)
        Dim r2 As Rectangle = New Rectangle(0, 0, Me.Width, Me.Height - 1)
        Dim c1 As Color = Color.White
        Dim c2 As Color = Color.FromArgb(255, 232, 232, 232)

        With e.Graphics
            .FillRectangle(New LinearGradientBrush(r1, c1, c2, 90), r1)
            .DrawString(Me.Text, Me.Font, Brushes.Black, New Point(2, 5))
            .DrawRectangle(New Pen(GridData.BORDER_COLOR, 1), r2)
        End With

    End Sub

End Class