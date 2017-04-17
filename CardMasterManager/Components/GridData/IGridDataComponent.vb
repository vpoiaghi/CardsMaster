Imports System.Drawing
Imports System.Windows.Forms

Public Interface IGridDataComponent

    Property BackColor As Color
    Property Height As Integer
    Property Left As Integer
    Property Parent As Control
    Property Top As Integer
    Property Value As Object
    Property Visible As Integer
    Property Width As Integer

    Event GotFocus(sender As Object, e As EventArgs)
    Event SizeChanged(sender As Object, e As EventArgs)
    Event ValueChanged(sender As Object, e As EventArgs)

End Interface
