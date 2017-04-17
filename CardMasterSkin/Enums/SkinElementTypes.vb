Namespace Skins

    Public NotInheritable Class SkinElementTypes

        Public Shared Round As New SkinElementTypes
        Public Shared Square As New SkinElementTypes
        Public Shared Rectangle As New SkinElementTypes
        Public Shared RoundedRectangle As New SkinElementTypes
        Public Shared TextArea As New SkinElementTypes

        Private Shared TYPE_INDEX As Integer = 0
        Private index As Integer

        Private Sub New()
            index = TYPE_INDEX
            TYPE_INDEX += 1
        End Sub

        Public Shared Operator <>(ByVal left As SkinElementTypes, ByVal right As SkinElementTypes) As Boolean
            Return left.index <> right.index
        End Operator

        Public Shared Operator =(ByVal left As SkinElementTypes, ByVal right As SkinElementTypes) As Boolean
            Return left.index = right.index
        End Operator

        Private Shadows Function Equals(obj As Object) As Boolean
            Return False
        End Function

        Private Shared Shadows Function ReferenceEquals(objA As Object, objB As Object) As Boolean
            Return False
        End Function

        Private Shadows Function GetHashCode() As Integer
            Return -1
        End Function

        Public Shadows Function ToString()
            Return Nothing
        End Function

    End Class

End Namespace