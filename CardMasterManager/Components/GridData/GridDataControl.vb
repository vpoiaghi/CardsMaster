Imports System.Drawing
Imports System.Windows.Forms

Public MustInherit Class GridDataControl
    Implements IGridDataComponent

    Protected WithEvents m_control As Control

    Public MustOverride Property Value As Object Implements IGridDataComponent.Value

    Public Event GotFocus(sender As Object, e As EventArgs) Implements IGridDataComponent.GotFocus
    Public Event SizeChanged(sender As Object, e As EventArgs) Implements IGridDataComponent.SizeChanged
    Public Event ValueChanged(sender As Object, e As EventArgs) Implements IGridDataComponent.ValueChanged

    Protected Sub New(control As Control)
        m_control = control
    End Sub

    Public Property BackColor As Color Implements IGridDataComponent.BackColor
        Get
            Return m_control.BackColor
        End Get
        Set(value As Color)
            m_control.BackColor = value
        End Set
    End Property

    Public Property Height As Integer Implements IGridDataComponent.Height
        Get
            Return m_control.Height
        End Get
        Set(value As Integer)
            m_control.Height = value
        End Set
    End Property

    Public Property Left As Integer Implements IGridDataComponent.Left
        Get
            Return m_control.Left
        End Get
        Set(value As Integer)
            m_control.Left = value
        End Set
    End Property

    Public Property Parent As Control Implements IGridDataComponent.Parent
        Get
            Return m_control.Parent
        End Get
        Set(value As Control)
            m_control.Parent = value
        End Set
    End Property

    Public Property Top As Integer Implements IGridDataComponent.Top
        Get
            Return m_control.Top
        End Get
        Set(value As Integer)
            m_control.Top = value
        End Set
    End Property

    Public Property Visible As Integer Implements IGridDataComponent.Visible
        Get
            Return m_control.Visible
        End Get
        Set(value As Integer)
            m_control.Visible = value
        End Set
    End Property

    Public Property Width As Integer Implements IGridDataComponent.Width
        Get
            Return m_control.Width
        End Get
        Set(value As Integer)
            m_control.Width = value
        End Set
    End Property


    Private Sub m_control_GotFocus(sender As Object, e As EventArgs) Handles m_control.GotFocus
        RaiseEvent GotFocus(sender, e)
    End Sub

    Private Sub m_control_SizeChanged(sender As Object, e As EventArgs) Handles m_control.SizeChanged
        RaiseEvent SizeChanged(sender, e)
    End Sub

    Protected Sub SendValueChangedEvent(sender As Object, e As EventArgs)
        RaiseEvent ValueChanged(sender, e)
    End Sub

End Class
