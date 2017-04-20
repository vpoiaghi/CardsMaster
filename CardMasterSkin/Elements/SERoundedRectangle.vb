Namespace Skins

    Public Class SERoundedRectangle
        Inherits SkinElement

        Private m_cornerRadius As Integer

        Public Sub New(width As Integer, height As Integer, cornerRadius As Integer)
            Me.New(0, 0, width, height, cornerRadius)
        End Sub

        Public Sub New(x As Integer, y As Integer, width As Integer, height As Integer, cornerRadius As Integer)
            MyBase.New(x, y, width, height)

            m_cornerRadius = cornerRadius

        End Sub

        Public Function GetCornerRadius() As Integer
            Return m_cornerRadius
        End Function

        Public Sub SetCornerRadius(cornerRadius As Integer)
            m_cornerRadius = cornerRadius
        End Sub

    End Class

End Namespace