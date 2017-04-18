Namespace Card

    Public Class Card

        Private m_powers As List(Of Power)

        Public Property Kind As String
        Public Property Name As String
        Public Property Rank As String
        Public Property Team As String
        Public Property Chakra As String
        Public Property Element As String
        Public Property Cost As Integer
        Public Property Attack As Integer
        Public Property Defense As Integer
        Public Property Citation As String
        Public Property Comments As String
        Public Property Background As New Texture

        Public Sub New()
            m_powers = New List(Of Power)
        End Sub

        Public Sub New(name As String)
            Me.Name = name
            m_powers = New List(Of Power)

            Dim p As New Power()
            p.Description = "Super pouvoir !"
            m_powers.Add(p)

        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()

            m_powers.Clear()
            m_powers = Nothing

            Me.Background = Nothing

        End Sub

        Public Property Powers As List(Of Power)
            Get
                Return m_powers
            End Get
            Set(value As List(Of Power))
                m_powers = value
            End Set
        End Property

        Public Overrides Function ToString() As String
            Return Me.Name
        End Function

    End Class

End Namespace