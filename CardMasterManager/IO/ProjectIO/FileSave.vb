Imports CardMasterCard.Card
Imports System.IO
Imports System.Xml.Serialization
Imports System.Windows.Forms

Public Class FileSave

    Public Function GetFile(currentDir As DirectoryInfo) As FileInfo

        Dim file As FileInfo = Nothing

        With New SaveFileDialog()

            .Filter = "JSON|*.json|XML|*.xml"
            .CheckFileExists = True

            If currentDir Is Nothing Then
                .InitialDirectory = Application.ExecutablePath
            Else
                .InitialDirectory = currentDir.FullName
            End If

            .ShowDialog(Me)

            If .FileName <> "" Then
                file = New FileInfo(.FileName)
            End If

        End With

        Return file

    End Function

End Class
