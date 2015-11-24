Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Data
Imports System.IO

Public Class Testing
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            UploadedFiles.DataBind()
        End If
    End Sub

    Protected Function GetUploadList() As String()
        Dim folder As String = GetSysInit("lampiran")
        Dim files() As String = Directory.GetFiles(folder)
        Dim fileNames(files.Length - 1) As String
        Array.Sort(files)

        For i As Integer = 0 To files.Length - 1
            fileNames(i) = Path.GetFileName(files(i))
        Next

        Return fileNames
    End Function

    Protected Sub UploadThisFile(ByVal upload As FileUpload)
        If upload.HasFile Then
            Dim theFileName As String = Path.Combine(GetSysInit("lampiran"), upload.FileName)
            If File.Exists(theFileName) Then
                File.Delete(theFileName)
            End If
            upload.SaveAs(theFileName)
        End If
    End Sub

    Protected Sub buttonUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonUpload.Click
        UploadThisFile(FileUpload1)
        UploadedFiles.DataBind()
    End Sub

    Protected Sub buttonMultiUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles buttonMultiUpload.Click
        UploadThisFile(multiUpload1)
        UploadThisFile(multiUpload2)
        UploadThisFile(multiUpload3)
        UploadedFiles.DataBind()
    End Sub
End Class