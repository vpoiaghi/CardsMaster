Imports System.IO
Imports System.Xml.Serialization
Imports Newtonsoft.Json


Namespace Card

    Public Class CardsProject

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
            myFileStream = Nothing

            Return CardsProject

        End Function

        Private Shared Function LoadJsonProject(file As FileInfo) As CardsProject

            Dim sr As New StreamReader(file.FullName)
            Dim js As String = sr.ReadToEnd

            Dim cardsProject As CardsProject = JsonConvert.DeserializeObject(Of CardsProject)(js)

            sr.Close()
            sr.Dispose()
            sr = Nothing

            Return cardsProject

        End Function

        Public Sub Save(ByRef file As FileInfo)

            Select Case LCase(file.Extension)
                Case ".xml" : SaveXmlProject(file)
                Case ".json" : SaveJsonProject(file)
            End Select

        End Sub

        Private Sub SaveXmlProject(ByRef file As FileInfo)

            Dim xs As New XmlSerializer(GetType(CardsProject))
            Dim xwr As New StreamWriter(file.FullName)

            xs.Serialize(xwr, Me)

            xwr.Close()
            xwr.Dispose()
            xwr = Nothing

        End Sub

        Private Sub SaveJsonProject(ByRef file As FileInfo)

            file.Delete()

            Dim js As String = JsonConvert.SerializeObject(Me, Formatting.Indented)
            Dim sw As New StreamWriter(file.FullName, FileMode.OpenOrCreate)

            sw.Write(js)

            sw.Close()
            sw.Dispose()
            sw = Nothing

        End Sub

    End Class

End Namespace