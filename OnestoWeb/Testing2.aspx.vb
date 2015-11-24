Imports System.IO
Public Class Testing2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("FileName") IsNot Nothing Then
            Try
                ' Read the file and convert it to Byte Array
                Dim filePath As String = "\\INTEGRALINDO2\lampiran\"
                Dim filename As String = Request.QueryString("FileName")
                Dim contenttype As String = "image/" & Path.GetExtension(filename).Replace(".", "")

                Dim fs As FileStream = New FileStream(filePath & filename, FileMode.Open, FileAccess.Read)
                Dim br As BinaryReader = New BinaryReader(fs)
                Dim bytes As Byte() = br.ReadBytes(Convert.ToInt32(fs.Length))
                br.Close()
                fs.Close()

                'Write the file to Reponse
                Response.Buffer = True
                Response.Charset = ""
                Response.Cache.SetCacheability(HttpCacheability.NoCache)
                Response.ContentType = contenttype
                Response.AddHeader("content-disposition", "attachment;filename=" & filename)
                Response.BinaryWrite(bytes)
                Response.Flush()
                Response.End()
            Catch

            End Try
        End If
    End Sub


    Private Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Image4.ImageUrl = "testing2.aspx?FileName=" + "Hendra.jpg"
    End Sub
End Class