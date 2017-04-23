Imports System.Windows.Forms

Public Class Frm_Parameters

    Public Property m_parameters As Parameters

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Shadows Sub Show()
    End Sub

    Private Shadows Sub ShowDialog()
    End Sub

    Private Shadows Function ShowDialog(owner As Form) As DialogResult
        Return MyBase.ShowDialog(owner)
    End Function

    Public Shared Function ChangeParameters(ByRef owner As Form, ByVal parameters As Parameters) As ParametersResult

        Dim frm As New Frm_Parameters

        frm.Txt_TexturesDirectory.Text = ""
        frm.Txt_ImagesDirectory.Text = ""

        If parameters IsNot Nothing Then
            frm.Txt_TexturesDirectory.Text = parameters.TexturesDirectory
            frm.Txt_ImagesDirectory.Text = parameters.ImagesDirectory
        End If


        Dim dlgResult As DialogResult = frm.ShowDialog(owner)
        Dim prmResult As ParametersResult

        If dlgResult = Windows.Forms.DialogResult.OK Then

            Dim prm As New Parameters

            prm.TexturesDirectory = frm.Txt_TexturesDirectory.Text
            prm.ImagesDirectory = frm.Txt_ImagesDirectory.Text

            prmResult = New ParametersResult(dlgResult, prm)

        Else
            prmResult = New ParametersResult(dlgResult, parameters)

        End If

        Return prmResult

    End Function


    Private Sub Btn_TexturesDirectory_Click(sender As Object, e As EventArgs) Handles Btn_TexturesDirectory.Click
        Txt_TexturesDirectory.Text = SelectDirectory(Txt_TexturesDirectory.Text)
    End Sub

    Private Sub Btn_ImagesDirectory_Click(sender As Object, e As EventArgs) Handles Btn_ImagesDirectory.Click
        Txt_ImagesDirectory.Text = SelectDirectory(Txt_ImagesDirectory.Text)
    End Sub

    Private Function SelectDirectory(InitDirectory As String) As String

        Dim result As String = InitDirectory

        With New FolderBrowserDialog

            .SelectedPath = InitDirectory
            .ShowNewFolderButton = True
            If .ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                result = .SelectedPath
            End If

        End With

        Return result

    End Function

End Class
