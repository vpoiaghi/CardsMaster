Imports System.Drawing

Public Class GridDataComboBox
    Inherits GridDataCell

    Private WithEvents m_comboBox As Cb

    Public Sub New(parentRow As GridDataRow)
        MyBase.New(parentRow, CellTypes.Combo)

        m_comboBox = New Cb
        With m_comboBox
            .Left = 0
            .Parent = Me
        End With

        m_mainControl = m_comboBox

    End Sub

    Public Overrides Property Value As Object
        Get
            Return m_comboBox.Text
        End Get
        Set(value As Object)
            m_comboBox.Text = value
        End Set
    End Property

    Private Sub GridDataComboBox_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged

        m_comboBox.Top = (Me.Height - m_comboBox.Height) / 2
        m_comboBox.Width = Me.Width

    End Sub

    Private Sub m_comboBox_TextChanged(sender As Object, e As EventArgs) Handles m_comboBox.TextChanged, Me.SizeChanged
        SendValueChangedEvent(sender, e)
    End Sub

    Private Class Cb
        Inherits ComboBox

        Protected Overrides Sub WndProc(ByRef m As Message)

            MyBase.WndProc(m)

            If m.Msg = &HF AndAlso ComboBoxRenderer.IsSupported Then

                Dim p As Pen = New Pen(Me.BackColor)
                Dim g As Graphics = Me.CreateGraphics

                g.DrawRectangle(p, 0, 0, Me.Width - 2, Me.Height - 1)
                g.DrawRectangle(p, 0, 0, Me.Width - 2, Me.Height - 2)
                g.DrawRectangle(p, 1, 1, Me.Width - 3, Me.Height - 3)

                g.Dispose()

            End If

        End Sub

    End Class

End Class