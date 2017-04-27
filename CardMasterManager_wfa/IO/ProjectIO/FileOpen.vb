Imports CardMasterCard.Card
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms

Public Class FileOpen

    Public Function GetFile(currentDir As DirectoryInfo) As FileInfo

        Dim file As FileInfo = Nothing

        With New OpenFileDialog()

            .Filter = "JSON|*.json|XML|*.xml"
            .Multiselect = False
            .CheckFileExists = True

            If currentDir Is Nothing Then
                .InitialDirectory = Application.ExecutablePath
            Else
                .InitialDirectory = currentDir.FullName
            End If

            .ShowDialog()

            If .FileName <> "" Then
                file = New FileInfo(.FileName)
            End If

        End With

        Return file

    End Function




End Class
