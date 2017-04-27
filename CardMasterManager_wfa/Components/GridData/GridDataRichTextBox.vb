Public Class GridDataRichTextBox
    Inherits GridDataCell

    Private WithEvents m_richTextBox As RichTextBox

    Public Sub New(parentRow As GridDataRow)
        MyBase.New(parentRow, CellTypes.Combo)

        m_richTextBox = New RichTextBox
        With m_richTextBox
            .BorderStyle = Windows.Forms.BorderStyle.None
            .AutoSize = False
            .Dock = DockStyle.Fill
            .Parent = Me
        End With

        m_mainControl = m_richTextBox

    End Sub

    Public Overrides Property Value As Object
        Get
            Return m_richTextBox.Text
        End Get
        Set(value As Object)
            m_richTextBox.Text = value
        End Set
    End Property

    Private Sub m_richtextBox_TextChanged(sender As Object, e As EventArgs) Handles m_richTextBox.TextChanged
        SendValueChangedEvent(sender, e)
    End Sub

End Class
