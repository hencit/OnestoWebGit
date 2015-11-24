Imports System
Imports System.Data
Imports System.Web
Public Class Main
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("UserID") = Nothing Then
            Response.Redirect("Login.aspx")
        End If
    End Sub

    Private Sub lbLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLogout.Click
        Response.Redirect("Login.aspx")
    End Sub

    Private Sub lbStock_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbStock.Click
        Response.Redirect("Stock.aspx")
    End Sub

    Private Sub lbTugas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTugas.Click
        Response.Redirect("Tugas.aspx")
    End Sub
End Class