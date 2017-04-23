Imports System.IO

Public Class FileTool

    Public Shared Function FindImage(Root As DirectoryInfo, FileNamePattern As String) As FileInfo

        Dim f As FileInfo = Nothing

        Dim fileName As String = Dir(Path.Combine(Root.FullName, FileNamePattern), FileAttribute.Normal)

        If String.IsNullOrEmpty(fileName) Then

            Dim directories() As DirectoryInfo = Root.GetDirectories()
            Dim directoriesCount As Integer = directories.Length
            Dim directoryIndex As Integer = 0

            While (f Is Nothing) AndAlso (directoryIndex < directoriesCount)

                f = FindImage(directories(directoryIndex), FileNamePattern)

                directoryIndex += 1

            End While

        Else
            f = New FileInfo(Path.Combine(Root.FullName, fileName))

        End If

        Return f

    End Function

End Class
