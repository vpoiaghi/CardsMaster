Namespace Utils

    Public Class List(Of T)
        Inherits System.Collections.Generic.List(Of T)

        Public Overrides Function ToString() As String

            Dim result As String = ""
            Dim firstLine As Boolean = True

            For Each p As Object In Me

                If firstLine Then
                    result += p.ToString()
                    firstLine = False

                Else
                    result += Environment.NewLine + p.ToString()
                End If

            Next

            Return result

        End Function
    End Class

End Namespace
