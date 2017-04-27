Public Class GridDataLabel
    Inherits GridDataCell

    Private m_label As Label

    Public Sub New(parentRow As GridDataRow)
        MyBase.New(parentRow, CellTypes.Combo)

        m_label = New Label
        With m_label
            .Left = 0
            .BorderStyle = Windows.Forms.BorderStyle.None
            .AutoSize = False
            .Parent = Me
        End With

        m_mainControl = m_label

    End Sub

    Public Overrides Property Value As Object
        Get
            Return m_label.Text
        End Get
        Set(value As Object)
            m_label.Text = value
        End Set
    End Property

    Private Sub GridDataLabel_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged, Me.SizeChanged

        m_label.Top = (Me.Height - m_label.Height) / 2
        m_label.Width = Me.Width

    End Sub

End Class
