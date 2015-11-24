Imports System.Data.SqlClient
Imports System.Data.OleDb
Public Class Tugas
    Inherits System.Web.UI.Page
    Dim strConnection As String = My.Settings.ConnStr
    Dim cn As SqlConnection = New SqlConnection(strConnection)
    Dim cmd As SqlCommand

    Dim m_Status As String
    Dim isShowAll As Boolean
    Dim val1, val2 As String

    Dim DA As New SqlDataAdapter
    Dim DS As New DataSet
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then 'generate form Else 'process submitted data End If
            If HiddenField.Value = 1 Then
                If Session("UserID") = Nothing Then
                    Response.Redirect("Login.aspx")
                Else
                    clear_lvw()
                End If
            Else

            End If
        Else
            If Session("UserID") = Nothing Then
                Response.Redirect("Login.aspx")
            Else
                'Add item cbStatus
                cmd = New SqlCommand("select [status] from sys_status where flag = 'delegasi_tugas' order by sort asc ", cn)

                cn.Open()
                Dim myReader = cmd.ExecuteReader

                While myReader.Read
                    cbStatus.Items.Add(myReader.GetString(0))
                End While
                cn.Close()

                clear_lvw()
            End If
        End If

    End Sub

    Private Sub lbLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLogout.Click
        Response.Redirect("Login.aspx")
    End Sub

    Private Sub lbMain_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbMain.Click
        Response.Redirect("Main.aspx")
    End Sub

    Private Sub lbStock_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbStock.Click
        Response.Redirect("Stock.aspx")
    End Sub

    Sub clear_lvw()
        Dim dateFrom, dateTo As Date
        If chbDate.Checked = True Then
            isShowAll = False
            dateFrom = CDate(txtDateFrom.Text)
            dateTo = CDate(txtDateTo.Text)
        Else
            isShowAll = True
        End If
        Try
            cmd = New SqlCommand("sp_tr_tugas_list_SEL", cn)
            cmd.CommandType = CommandType.StoredProcedure

            Dim prm1 As SqlParameter = cmd.Parameters.Add("@tugas_no", SqlDbType.NVarChar, 50)
            prm1.Value = IIf(txtNoTugas.Text = "", DBNull.Value, txtNoTugas.Text)
            Dim prm2 As SqlParameter = cmd.Parameters.Add("@tugas_description", SqlDbType.NVarChar, 255)
            prm2.Value = IIf(txtDeskripsi.Text = "", DBNull.Value, txtDeskripsi.Text)
            Dim prm3 As SqlParameter = cmd.Parameters.Add("@tugas_date_from", SqlDbType.SmallDateTime)
            prm3.Value = IIf(isShowAll = False, dateFrom, DBNull.Value)
            Dim prm4 As SqlParameter = cmd.Parameters.Add("@tugas_date_to", SqlDbType.SmallDateTime)
            prm4.Value = IIf(isShowAll = False, dateTo, DBNull.Value)
            Dim prm5 As SqlParameter = cmd.Parameters.Add("@karyawan_name", SqlDbType.NVarChar, 50)
            prm5.Value = IIf(txtNamaKaryawan.Text = "", DBNull.Value, txtNamaKaryawan.Text)
            Dim prm6 As SqlParameter = cmd.Parameters.Add("@dept_code", SqlDbType.NVarChar, 50)
            prm6.Value = IIf(txtDept.Text = "", DBNull.Value, txtDept.Text)
            Dim prm7 As SqlParameter = cmd.Parameters.Add("@tugas_status", SqlDbType.NVarChar, 50)
            If cbStatus.SelectedIndex = 0 Then
                prm7.Value = "All"
            Else
                prm7.Value = cbStatus.Text
            End If

            DA = New SqlDataAdapter(cmd)
            DS = New DataSet
            DA.Fill(DS, "_Tugas")

            ListView1.DataSource = Nothing
            ListView1.DataSource = DS.Tables("_Tugas")
            ListView1.DataBind()
        Catch ex As Exception
            If ConnectionState.Open = True Then cn.Close()
            Response.Write("Error code: " + ex.Message)
        End Try
    End Sub

    Private Sub ListView1_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles ListView1.ItemCommand
        Dim cmdName As String = e.CommandName

        If cmdName = "approve" Then
            Try
                Dim tugasNo As String = e.CommandArgument.ToString()
                Session("TugasNo") = tugasNo
                Session("flagSender") = "Tugas"
                Response.Redirect("Approve.aspx")
            Catch ex As Exception
                Response.Write("Error code: " + ex.Message)
            End Try
        End If

        If cmdName = "view" Then
            Try
                Dim tugasNo As String = e.CommandArgument.ToString()
                Session("TugasNo") = tugasNo

                Response.Redirect("DelegasiTugas.aspx")
            Catch ex As Exception
                Response.Write("Error code: " + ex.Message)
            End Try
        End If
    End Sub

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try
            Session("TugasNo") = Nothing

            Response.Redirect("DelegasiTugas.aspx")
        Catch ex As Exception
            Response.Write("Error code: " + ex.Message)
        End Try
    End Sub
End Class