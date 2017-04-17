Imports System.Windows.Forms

Public Class GridDataComboBox
    Inherits GridDataControl
    Implements IGridDataComponent

    Private WithEvents m_comboBox As ComboBox

    Public Sub New()
        MyBase.New(New ComboBox)
        m_comboBox = m_control

    End Sub

    Public Overrides Property Value As Object
        Get
            Return m_comboBox.Text
        End Get
        Set(value As Object)
            m_comboBox.Text = value
        End Set
    End Property

    Private Sub m_textBox_TextChanged(sender As Object, e As EventArgs) Handles m_comboBox.TextChanged
        SendValueChangedEvent(sender, e)
    End Sub

End Class
