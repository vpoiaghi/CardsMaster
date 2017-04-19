Imports System.IO
Imports System.Xml.Serialization
Imports System.Runtime.Serialization.Json

Namespace Card

    Public Class CardsProject

        Public Property ImagesDirectory As String
        Public Property TexturesDirectory As String
        Public Property Cards As CardsSet

        Public Shared Function LoadProject(file As FileInfo) As CardsProject

            Dim cardsProject As CardsProject = Nothing

            Select Case LCase(file.Extension)
                Case ".xml" : cardsProject = LoadXmlProject(file)
                Case ".json" : cardsProject = LoadJsonProject(file)
            End Select

            Return cardsProject

        End Function

        Private Shared Function LoadXmlProject(file As FileInfo) As CardsProject

            Dim CardsProject As CardsProject

            Dim myFileStream As Stream = file.OpenRead()
            Dim xs As New XmlSerializer(GetType(CardsProject))

            CardsProject = xs.Deserialize(myFileStream)

            myFileStream.Close()
            myFileStream.Dispose()

            Return CardsProject

        End Function

        Private Shared Function LoadJsonProject(file As FileInfo) As CardsProject

            Dim CardsProject As CardsProject

            Dim jrd As New FileStream(file.FullName, FileMode.Open)
            Dim js As New DataContractJsonSerializer(GetType(CardsProject))

            CardsProject = CType(js.ReadObject(jrd), CardsProject)

            jrd.Close()
            jrd.Dispose()

            Return CardsProject

        End Function

    End Class

End Namespace