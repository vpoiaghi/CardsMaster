Namespace Utils

    Public Class List(Of T As ListItem)
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

            If (result = "") Then
                result = "Liste vide"
            End If

            Return result

        End Function

        Public Shared Narrowing Operator CType(ByVal value As String) As List(Of T)

            Dim list As New List(Of T)

            If Not String.IsNullOrEmpty(value) Then

                Dim values() As String = value.Split(Environment.NewLine)

                For Each v As String In values

                    Dim item As T = GetType(T).GetConstructor(Type.EmptyTypes).Invoke(Nothing)
                    item.FromString(v)

                Next

            End If

            Return list

        End Operator

    End Class

End Namespace
