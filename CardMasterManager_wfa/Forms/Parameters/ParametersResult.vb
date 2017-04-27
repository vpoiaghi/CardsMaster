Public Class ParametersResult

    Private m_dialogResult As DialogResult
    Private m_parameters As Parameters

    Public Sub New(dialogResult As DialogResult, parameters As Parameters)
        m_dialogResult = dialogResult
        m_parameters = parameters
    End Sub

    Public ReadOnly Property DialogResult
        Get
            Return m_dialogResult
        End Get
    End Property

    Public ReadOnly Property Parameters As Parameters
        Get
            Return m_parameters
        End Get
    End Property

End Class
