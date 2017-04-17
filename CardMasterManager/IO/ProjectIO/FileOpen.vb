Imports CardMasterCard.Card
Imports System.Drawing
Imports System.IO
Imports System.Runtime.Serialization.Json
Imports System.Xml.Serialization
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

    Public Function LoadProject(file As FileInfo) As CardsProject

        Dim cardsProject As CardsProject = Nothing

        Select Case LCase(file.Extension)
            Case ".xml" : cardsProject = LoadXmlProject(file)
            Case ".json" : cardsProject = LoadJsonProject(file)
        End Select

        Return cardsProject

    End Function

    Private Function LoadXmlProject(file As FileInfo) As CardsProject

        Dim CardsProject As CardsProject

        Dim myFileStream As Stream = file.OpenRead()
        Dim xs As New XmlSerializer(GetType(CardsProject))

        CardsProject = xs.Deserialize(myFileStream)

        myFileStream.Close()
        myFileStream.Dispose()

        Return CardsProject

    End Function

    Private Function LoadJsonProject(file As FileInfo) As CardsProject

        Dim CardsProject As CardsProject

        Dim jrd As New FileStream(file.FullName, FileMode.Open)
        Dim js As New DataContractJsonSerializer(GetType(CardsProject))

        CardsProject = CType(js.ReadObject(jrd), CardsProject)

        jrd.Close()
        jrd.Dispose()

        Return CardsProject

    End Function


End Class
