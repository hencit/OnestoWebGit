Imports System.Data.SqlClient
Imports System.Data.OleDb

Module moduleProcedure
    Public str_user_name As String
    Public str_user_access As String
    Public p_Group As String
    Public p_CompanyName As String

    Dim strConnection As String = My.Settings.ConnStr
    Dim cn As SqlConnection = New SqlConnection(strConnection)
    Dim cmd As SqlCommand

    
End Module
