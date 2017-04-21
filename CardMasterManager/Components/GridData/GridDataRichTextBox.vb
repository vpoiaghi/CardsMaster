Imports System.Windows.Forms

Public Class GridDataRichTextBox
    Inherits GridDataControl
    Implements IGridDataComponent

    Private WithEvents m_textBox As RichTextBox



    Public Sub New()
        MyBase.New(New RichTextBox)
        m_textBox = m_control

        m_textBox.BorderStyle = Windows.Forms.BorderStyle.None
        m_textBox.Margin = New Padding(5)
    End Sub

    Public Overrides Property Value As Object
        Get
            Return m_textBox.Text
        End Get
        Set(value As Object)
            m_textBox.Text = value
        End Set
    End Property

    Private Sub m_textBox_TextChanged(sender As Object, e As EventArgs) Handles m_textBox.TextChanged
        SendValueChangedEvent(sender, e)
    End Sub

End Class
