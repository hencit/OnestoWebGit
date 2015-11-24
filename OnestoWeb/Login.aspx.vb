Imports System.Data.SqlClient
Imports System.Data.OleDb
Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("UserID") = Nothing
        txtUserID.Focus()
    End Sub

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLogin.Click
        Dim strConnection As String = My.Settings.ConnStr
        Dim cn As SqlConnection = New SqlConnection(strConnection)
        Dim cmd As SqlCommand = New SqlCommand("SELECT user_code,[password] FROM mt_user where active=1 and user_code='" & txtUserID.Text & "' ", cn)

        cn.Open()

        Dim myReader As SqlDataReader = cmd.ExecuteReader()

        If Not myReader.HasRows Then
            Response.Write("User Belum terdaftar")
        Else
            myReader.Read()
            '-------------------------DECRYPT FROM DB-------------------------------
            Dim cipherText As String = myReader.GetString(1)
            Dim password As String = "dexter"
            Dim wrapper As New Dencrypt(password)
            Dim LoginPassword As String = wrapper.DecryptData(cipherText)
            '-------------------------END OF DECRYPT--------------------------------

            If txtPassword.Text <> LoginPassword Then
                Response.Write("Password Salah")
            Else
                Session("UserID") = txtUserID.Text
                'Server.Transfer("Main.aspx", True)
                Response.Redirect("Main.aspx")
                myReader.Close()
            End If


        End If
        myReader.Close()
        cn.Close()
    End Sub
End Class