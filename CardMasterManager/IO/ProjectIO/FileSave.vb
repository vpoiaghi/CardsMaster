Imports CardMasterCard.Card
Imports System.IO
Imports System.Runtime.Serialization.Json
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

    Public Sub Save(ByRef cardsProject As CardsProject, ByRef file As FileInfo)

        Select Case LCase(file.Extension)
            Case ".xml" : SaveXmlProject(cardsProject, file)
            Case ".json" : SaveJsonProject(cardsProject, file)
        End Select

    End Sub

    Private Sub SaveXmlProject(ByRef cardsProject As CardsProject, ByRef file As FileInfo)

        Dim xs As New XmlSerializer(GetType(CardsProject))
        Dim xwr As New StreamWriter(file.FullName)

        xs.Serialize(xwr, cardsProject)

        xwr.Close()
        xwr.Dispose()

    End Sub

    Private Sub SaveJsonProject(ByRef cardsProject As CardsProject, ByRef file As FileInfo)

        Dim js As New DataContractJsonSerializer(GetType(CardsProject))
        Dim jwr As New FileStream(file.FullName, FileMode.OpenOrCreate)

        js.WriteObject(jwr, cardsProject)

        jwr.Close()
        jwr.Dispose()

    End Sub

End Class
