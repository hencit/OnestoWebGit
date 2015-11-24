Public Class clsMyListStr
    Private sName As String
    Private iID As String   'You can also declare this as String.

    Public Sub New()
        sName = ""
        iID = 0
    End Sub

    Public Sub New(ByVal Name As String, ByVal ID As String)
        sName = Name
        iID = ID
    End Sub

    Public Property Name() As String
        Get
            Return sName
        End Get

        Set(ByVal sValue As String)
            sName = sValue
        End Set
    End Property

    Public Property ItemData() As String
        Get
            Return iID
        End Get

        Set(ByVal iValue As String)
            iID = iValue
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return sName
    End Function
End Class


