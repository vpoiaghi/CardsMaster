Imports System.Windows.Forms

Public Class GridDataLabel
    Inherits GridDataControl
    Implements IGridDataComponent


    Private m_label As Label

    Public Sub New()
        MyBase.New(New Label)
        m_label = m_control

        m_label.BorderStyle = Windows.Forms.BorderStyle.None
        m_label.Margin = New Padding(5)

    End Sub

    Public Overrides Property Value As Object
        Get
            Return m_label.Text
        End Get
        Set(value As Object)
            m_label.Text = value
        End Set
    End Property

End Class
