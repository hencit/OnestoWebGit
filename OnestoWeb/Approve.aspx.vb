Imports System.Data.SqlClient
Imports System.Data.OleDb
Public Class Approve
    Inherits System.Web.UI.Page
    Dim strConnection As String = My.Settings.ConnStr
    Dim cn As SqlConnection = New SqlConnection(strConnection)
    Dim cmd As SqlCommand

    Dim val1, val2 As String
    Dim m_TugasNo As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then 'generate form Else 'process submitted data End If
            If HiddenField.Value = 1 Then
                If Session("UserID") = Nothing Then
                    Response.Redirect("Login.aspx")
                Else
                    view_record()
                End If
            Else

            End If
        Else
            If Session("UserID") = Nothing Then
                Response.Redirect("Login.aspx")
            ElseIf Session("TugasNo") = Nothing Then
                Response.Redirect("Tugas.aspx")
            Else
                m_TugasNo = Session("TugasNo").ToString
                view_record()
            End If
        End If
    End Sub

    Sub view_record()
        'a.tugas_no,0
        'a.tugas_code,1
        'a.tugas_description,2
        'a.tugas_date,3
        'a.karyawan_code,4
        'a.karyawan_name,5
        'a.dept_code,6
        'a.tugas_status,7
        'a.ref_no,8
        'a.effective_date_from,9
        'a.effective_date_to,10
        'a.due_date,11
        'a.link_before,12
        'a.link_after1,13
        'a.link_after2,14
        'a.repeater,15
        'a.finish_flag,16
        'a.finish_percent,17
        'a.finish_date,18
        'a.finish_notes,19
        'a.approve1_flag,20
        'a.approve1_percent,21
        'a.approve1_date,22
        'a.approve1_notes,23
        'a.approve2_flag,24
        'a.approve2_percent,25
        'a.approve2_date,26
        'a.approve2_notes,27
        'a.submit,28
        'b.link_standard,29
        'b.val_before,30
        'b.val_after1,31
        'b.val_after2,32
        Try
            cmd = New SqlCommand("sp_tr_tugas_SEL", cn)
            cmd.CommandType = CommandType.StoredProcedure

            Dim prm1 As SqlParameter = cmd.Parameters.Add("@tugas_no", SqlDbType.NVarChar, 50)
            prm1.Value = m_TugasNo

            cn.Open()

            Dim myReader As SqlDataReader = cmd.ExecuteReader()

            While myReader.Read
                txtNoTugas.Text = m_TugasNo
                txtTugasStatus.Text = myReader.GetString(7)

                'Approve 1
                txtApprove1Persentase.Text = CStr(myReader.GetInt32(21))
                txtApprove1Notes.Text = myReader.GetString(23)

                'Approve 2
                txtApprove2Persentase.Text = CStr(myReader.GetInt32(25))
                txtApprove2Notes.Text = myReader.GetString(27)


            End While

            myReader.Close()
            cn.Close()

            lock_obj()
        Catch ex As Exception
            If ConnectionState.Open = True Then cn.Close()
            Response.Write("Error code: " + ex.Message)
        End Try
    End Sub

    Sub lock_obj()
        txtNoTugas.ReadOnly = True
        txtTugasStatus.ReadOnly = True
        If txtTugasStatus.Text = "Selesai" Then
            PanelApprove1.Enabled = True
            PanelApprove2.Enabled = False
        ElseIf txtTugasStatus.Text = "Approve1" Then
            PanelApprove1.Enabled = False
            PanelApprove2.Enabled = True
        Else
            PanelApprove1.Enabled = False
            PanelApprove2.Enabled = False
        End If
    End Sub

    Private Sub btnApprove1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApprove1.Click
        val1 = "tr_tugas_"
        val2 = "approve1"
        If otorisasi(val1 + val2, Session("UserID")) = False Then
            Response.Write(val1 + val2 + "   " + Session("UserID"))
            Response.Write("Anda tidak mempunyai otorisasi " + val2 + " modul ini!,Silahkan hubungi administrator anda untuk diberikan otorisasi")
            Exit Sub
        End If

        If CInt(txtApprove1Persentase.Text) = 0 Then
            Response.Write("Approval Persentase tugas tidak boleh kosong!")
            txtApprove1Persentase.Focus()
            Exit Sub
        End If

        If CInt(txtApprove1Persentase.Text) > 100 Then
            txtApprove1Persentase.Text = "100"
        End If

        Try
            cmd = New SqlCommand("sp_tr_tugas_APPROVE1", cn)
            cmd.CommandType = CommandType.StoredProcedure

            Dim prm1 As SqlParameter = cmd.Parameters.Add("@tugas_no", SqlDbType.Int)
            prm1.Value = CInt(txtNoTugas.Text)
            Dim prm12 As SqlParameter = cmd.Parameters.Add("@approve1_flag", SqlDbType.Bit)
            prm12.Value = True
            Dim prm3 As SqlParameter = cmd.Parameters.Add("@approve1_percent", SqlDbType.Int)
            prm3.Value = CInt(txtApprove1Persentase.Text)
            Dim prm4 As SqlParameter = cmd.Parameters.Add("@approve1_date", SqlDbType.NVarChar, 30)
            prm4.Value = System.DateTime.Now
            Dim prm8 As SqlParameter = cmd.Parameters.Add("@tugas_status", SqlDbType.NVarChar, 25)
            prm8.Value = "Approve1"
            Dim prm9 As SqlParameter = cmd.Parameters.Add("@approve1_notes", SqlDbType.NVarChar, 255)
            prm9.Value = txtApprove1Notes.Text

            Dim prm22 As SqlParameter = cmd.Parameters.Add("@user_code", SqlDbType.NVarChar, 50)
            prm22.Value = Session("UserID")

            cn.Open()
            cmd.ExecuteNonQuery()
            cn.Close()

            Response.Redirect("Approve.aspx")
        Catch ex As Exception
            If ConnectionState.Open = True Then cn.Close()
            'Response.Write("Error code: " + ex.Message)
        End Try

    End Sub

    Private Sub btnApprove2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApprove2.Click
        val1 = "tr_tugas_"
        val2 = "approve2"
        If otorisasi(val1 + val2, Session("UserID")) = False Then
            Response.Write("Anda tidak mempunyai otorisasi " + val2 + " modul ini!,Silahkan hubungi administrator anda untuk diberikan otorisasi")
            Exit Sub
        End If

        If CInt(txtApprove2Persentase.Text) = 0 Then
            Response.Write("Approval Persentase tugas tidak boleh kosong!")
            txtApprove2Persentase.Focus()
            Exit Sub
        End If

        If CInt(txtApprove2Persentase.Text) > 100 Then
            txtApprove2Persentase.Text = "100"
        End If

        Try
            cmd = New SqlCommand("sp_tr_tugas_approve2", cn)
            cmd.CommandType = CommandType.StoredProcedure

            Dim prm1 As SqlParameter = cmd.Parameters.Add("@tugas_no", SqlDbType.Int)
            prm1.Value = CInt(txtNoTugas.Text)
            Dim prm12 As SqlParameter = cmd.Parameters.Add("@approve2_flag", SqlDbType.Bit)
            prm12.Value = True
            Dim prm3 As SqlParameter = cmd.Parameters.Add("@approve2_percent", SqlDbType.Int)
            prm3.Value = CInt(txtApprove2Persentase.Text)
            Dim prm4 As SqlParameter = cmd.Parameters.Add("@approve2_date", SqlDbType.NVarChar, 30)
            prm4.Value = System.DateTime.Now
            Dim prm8 As SqlParameter = cmd.Parameters.Add("@tugas_status", SqlDbType.NVarChar, 25)
            prm8.Value = "Complete"
            Dim prm9 As SqlParameter = cmd.Parameters.Add("@approve2_notes", SqlDbType.NVarChar, 255)
            prm9.Value = txtApprove2Notes.Text

            Dim prm22 As SqlParameter = cmd.Parameters.Add("@user_code", SqlDbType.NVarChar, 50)
            prm22.Value = Session("UserID")

            cn.Open()
            cmd.ExecuteNonQuery()
            cn.Close()

            Response.Redirect("Approve.aspx")
        Catch ex As Exception
            If ConnectionState.Open = True Then cn.Close()
            Response.Write("Error code: " + ex.Message)
        End Try


    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        If Session("flagSender") = "Tugas" Then
            Response.Redirect("Tugas.aspx")
        Else
            Session("TugasNo") = txtNoTugas.Text
            Response.Redirect("DelegasiTugas.aspx")
        End If

    End Sub
End Class