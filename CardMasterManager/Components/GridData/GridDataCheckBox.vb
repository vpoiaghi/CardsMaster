Imports System.Windows.Forms

Public Class GridDataCheckBox
    Inherits GridDataCell

    Private WithEvents m_checkBox As CheckBox

    Public Sub New(parentRow As GridDataRow)
        MyBase.New(parentRow, CellTypes.Check)

        m_checkBox = New CheckBox
        With m_checkBox
            .Text = ""
            .Left = 0
            .Parent = Me
        End With

        m_mainControl = m_checkBox

    End Sub

    Public Overrides Property Value As Object
        Get
            Return m_checkBox.Checked
        End Get
        Set(value As Object)
            m_checkBox.Checked = value
        End Set
    End Property

    Private Sub GridDataCheckBox_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged, Me.SizeChanged

        m_checkBox.Top = (Me.Height - m_checkBox.Height) / 2
        m_checkBox.Width = Me.Width

    End Sub

    Private Sub m_checkBox_CheckedChanged(sender As Object, e As EventArgs) Handles m_checkBox.CheckedChanged
        SendValueChangedEvent(sender, e)
    End Sub

End Class
