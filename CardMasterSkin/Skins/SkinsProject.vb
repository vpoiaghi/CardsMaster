Imports System.IO
Imports Newtonsoft.Json

Namespace Skins

    Public Class SkinsProject

        Public Property TexturesDirectory As String
        Public Property ImagesDirectory As String

        Public Shared Function LoadProject(file As FileInfo) As SkinsProject

            Dim sr As New StreamReader(file.FullName)
            Dim js As String = sr.ReadToEnd

            Dim skinsProject As SkinsProject = JsonConvert.DeserializeObject(Of SkinsProject)(js)

            sr.Close()
            sr.Dispose()

            skinsProject.TexturesDirectory = skinsProject.TexturesDirectory.Replace("\\", "\")
            skinsProject.ImagesDirectory = skinsProject.ImagesDirectory.Replace("\\", "\")

            Return skinsProject

        End Function

        Public Sub Save(file As FileInfo)

            file.Delete()

            Dim js As String = JsonConvert.SerializeObject(Me, Formatting.Indented)
            Dim sw As New StreamWriter(file.FullName, FileMode.OpenOrCreate)

            sw.Write(js)

            sw.Close()
            sw.Dispose()

        End Sub

    End Class

End Namespace