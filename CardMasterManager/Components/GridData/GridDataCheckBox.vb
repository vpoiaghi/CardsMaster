Imports System.Windows.Forms

Public Class GridDataCheckBox
    Inherits GridDataControl
    Implements IGridDataComponent


    Private WithEvents m_checkBox As CheckBox

    Public Sub New()
        MyBase.New(New CheckBox)
        m_checkBox = m_control

        m_checkBox.Text = ""

    End Sub

    Public Overrides Property Value As Object
        Get
            Return m_checkBox.Checked
        End Get
        Set(value As Object)
            m_checkBox.Checked = value
        End Set
    End Property

    Private Sub m_checkBox_CheckedChanged(sender As Object, e As EventArgs) Handles m_checkBox.CheckedChanged
        SendValueChangedEvent(sender, e)
    End Sub

End Class
