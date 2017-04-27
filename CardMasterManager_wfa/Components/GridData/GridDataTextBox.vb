Imports System.Windows.Forms

Public Class GridDataTextBox
    Inherits GridDataCell

    Private WithEvents m_textBox As TextBox

    Public Sub New(parentRow As GridDataRow)
        MyBase.New(parentRow, CellTypes.Combo)

        m_textBox = New TextBox
        With m_textBox
            .BorderStyle = Windows.Forms.BorderStyle.None
            .Left = 0
            .Parent = Me
        End With

        m_mainControl = m_textBox

    End Sub

    Public Overrides Property Value As Object
        Get
            Return m_textBox.Text
        End Get
        Set(value As Object)
            If value Is Nothing Then
                m_textBox.Text = ""
            Else
                m_textBox.Text = value.ToString
            End If
        End Set
    End Property

    Private Sub m_textBox_TextChanged(sender As Object, e As EventArgs) Handles m_textBox.TextChanged
        SendValueChangedEvent(sender, e)
    End Sub

    Private Sub GridDataTextBox_Resize(sender As Object, e As EventArgs) Handles Me.Resize, Me.SizeChanged

        m_textBox.Top = (Me.Height - m_textBox.Height) / 2
        m_textBox.Width = Me.Width

    End Sub


End Class
