Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Data.Odbc
Imports System.IO

Public Class Stock
    Inherits System.Web.UI.Page
    Dim strConnection As String = My.Settings.ConnStr
    Dim cn As SqlConnection = New SqlConnection(strConnection)
    Dim cmd As SqlCommand
    Dim sqlreader As SqlDataReader

    Dim strConnection2 As String = My.Settings.ConnStr2
    Dim cn2 As New OdbcConnection(strConnection2)
    Dim strSQL As String
    Dim cmd1, cmd2, cmd3, cmd4 As String
    Dim comm As OdbcCommand
    Dim myReader As OdbcDataReader

    Dim path, path_header, path_detail, ext, tanggal, bulan, tahun As String
    Dim dtpPeriod, dtpPeriodNow As Date
    Dim val1, val2 As String

    Dim DS2 As New DataSet
    Dim flagPostBack As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack Then 'generate form Else 'process submitted data End If
            If HiddenField.Value = 1 Then
                If Session("UserID") = Nothing Then
                    Response.Redirect("Login.aspx")
                Else
                    Response.AppendHeader("Refresh", "200")
                    getPenjualan()
                    txtScanCode.Focus()
                End If
            Else
                txtScanCode.Focus()
            End If
        Else
            If Session("UserID") = Nothing Then
                Response.Redirect("Login.aspx")
            Else
                Response.AppendHeader("Refresh", "200")
                sortList.Items.Add("Sort DEPTID")
                sortList.Items.Add("Sort STOCKID")
                getPenjualan()
                txtScanCode.Focus()
            End If
        End If
        txtScanCode.Focus()
       
    End Sub

    Private Sub lbTugas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTugas.Click
        Response.Redirect("Tugas.aspx")
    End Sub

    Private Sub lbLogout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbLogout.Click
        Response.Redirect("Login.aspx")
    End Sub

    Private Sub lbMain_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbMain.Click
        Response.Redirect("Main.aspx")
    End Sub

    Sub batalProcess(ByVal STOCKID As String)
        Try
            cmd = New SqlCommand("sp_linq_penjualan_BATAL", cn)
            cmd.CommandType = CommandType.StoredProcedure

            Dim prm3 As SqlParameter = cmd.Parameters.Add("@STOCKID", SqlDbType.NVarChar, 50)
            prm3.Value = STOCKID
            Dim prm10 As SqlParameter = cmd.Parameters.Add("@user_code", SqlDbType.NVarChar, 50)
            prm10.Value = Session("UserID")

            cn.Open()
            cmd.ExecuteNonQuery()
            cn.Close()

            HiddenField.Value = 1
            Page_Load(Nothing, EventArgs.Empty)
        Catch ex As Exception
            If ConnectionState.Open = True Then cn.Close()
            HiddenField.Value = 1
            Page_Load(Nothing, EventArgs.Empty)
            Response.Write("Error code: " + ex.Message)
        End Try
    End Sub

    Sub scanProcess(ByVal STOCKID As String)
        Try
            cmd = New SqlCommand("sp_linq_penjualan_SCAN", cn)
            cmd.CommandType = CommandType.StoredProcedure

            Dim prm3 As SqlParameter = cmd.Parameters.Add("@STOCKID", SqlDbType.NVarChar, 50)
            prm3.Value = STOCKID
            Dim prm10 As SqlParameter = cmd.Parameters.Add("@user_code", SqlDbType.NVarChar, 50)
            prm10.Value = Session("UserID")

            cn.Open()
            cmd.ExecuteNonQuery()
            cn.Close()

            Page_Load(Nothing, EventArgs.Empty)
        Catch ex As Exception
            If ConnectionState.Open = True Then cn.Close()
            Response.Write("Error code: " + ex.Message)
        End Try
    End Sub

    Sub getPenjualan()
        Try
            Dim bedaHari As Integer

            'dtpPeriodNow = GetServerDate()
            dtpPeriodNow = "2014-09-30"
            dtpPeriod = GetSysInit("tanggal_stock")

            bedaHari = DateDiff(DateInterval.Day, dtpPeriod, dtpPeriodNow)

            'Response.Write(CStr(dtpPeriod) + " " + CStr(dtpPeriodNow) + " " + CStr(rangeHari))

            cmd1 = ""
            cmd2 = ""
            cmd3 = ""
            strSQL = "SELECT path_header, path_detail FROM sys_path"

            Dim DA As New SqlDataAdapter(strSQL, cn)
            Dim DS As New DataSet
            Dim DT As DataTable
            DA.Fill(DS, "_path")

            DT = DS.Tables("_path")

            ext = ".dat"

            cmd1 = "SELECT ISNULL(cc.DEPTID,0) as DEPTID,aa.STOCKID,aa.STOCKNAME,SUM(aa.QTY) as QTY,aa.QTYSCAN,cc.CURQTY,aa.CANCEL FROM ("

            For q = 0 To bedaHari - 1
                If CInt(dtpPeriod.Month) < 10 Then
                    bulan = "0" + CStr(dtpPeriod.Month)
                Else
                    bulan = dtpPeriod.Month
                End If

                tahun = dtpPeriod.Year

                If CInt(dtpPeriod.Day) < 10 Then
                    tanggal = "0" + CStr(dtpPeriod.Day)
                Else
                    tanggal = dtpPeriod.Day
                End If

                '---------------- Start GetPenjualan PerHari --------------------------
                For z = 0 To DT.Rows.Count - 1
                    path_header = DT.Rows(z).Item("path_header")
                    path_detail = DT.Rows(z).Item("path_detail")

                    path = path_header + path_detail + "\" + tahun + "-" + bulan + "\IT" + bulan + "" + tanggal + ext
                    'path = "D:\MEGA\DATA\POS06\2014-09\0924.dat"

                    If File.Exists(path) Then
                        'If CInt(z) = 0 And CInt(q) = 0 Then

                        'Else
                        '    cmd2 = cmd2 + " UNION ALL "
                        'End If

                        cmd2 = cmd2 + "SELECT a.STOCKID,a.STOCKNAME,a.QTY,ISNULL(b.QTYSCAN,0) as QTYSCAN,ISNULL(b.cancel,0) as CANCEL " & _
                            "FROM OPENQUERY(MEGA, 'SELECT STOCKID,STOCKNAME,QTY FROM " + Chr(34) + path + Chr(34) + " WHERE ISVOID = FALSE') a " & _
                            "LEFT JOIN sqlserver.dbo.linq_penjualan b ON a.STOCKID = b.STOCKID "
                        cmd2 = cmd2 + " UNION ALL "
                    End If
                Next
                '---------------- END GetPenjualan PerHari --------------------------
                dtpPeriod = dtpPeriod.AddDays(1)

            Next

            cmd2 = cmd2.Remove(cmd2.Length - 11)
            cmd3 = ") aa INNER JOIN (SELECT c.DEPTID,c.STOCKID,c.CURQTY " & _
                "FROM OPENQUERY(MEGA, 'SELECT DEPTID,STOCKID,CURQTY FROM " + Chr(34) + GetSysInit("etstore_master_stock") + Chr(34) + "')c)cc ON aa.STOCKID = cc.STOCKID " & _
                "WHERE aa.CANCEL = 0 GROUP BY cc.DEPTID,aa.STOCKID,aa.STOCKNAME,aa.QTYSCAN,cc.CURQTY,aa.CANCEL " & _
                "HAVING SUM(aa.QTY)>AA.QTYSCAN "

            If sortList.SelectedIndex = 1 Then
                cmd4 = "ORDER BY aa.STOCKID"
            Else
                cmd4 = "ORDER BY cc.DEPTID"
            End If

            'Response.Write(cmd1 + cmd2 + cmd3 + cmd4)

            Dim DA2 As New SqlDataAdapter(cmd1 + cmd2 + cmd3 + cmd4, cn)
            DA2.SelectCommand.CommandTimeout = 600
            DS2 = New DataSet
            DA2.Fill(DS2, "_transaksi")

            ListView1.DataSource = Nothing
            ListView1.DataSource = DS2.Tables("_transaksi")
            ListView1.DataBind()

        Catch ex As Exception
            If ConnectionState.Open = True Then cn.Close()
            Response.Write("Error code: " + ex.Message)
        End Try
    End Sub

    Private Sub btnScan_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnScan.Click
        Dim scanCode, scanTRANSNO, scanSTOCKID As String
        scanTRANSNO = ""
        scanSTOCKID = ""
        Dim flag As Boolean = False
        scanCode = txtScanCode.Text

        Try
            For i = 0 To DS2.Tables("_transaksi").Rows.Count - 1
                scanSTOCKID = DS2.Tables("_transaksi").Rows(i).Item(1).ToString

                If scanCode = scanSTOCKID Then
                    flag = True
                    Exit For
                End If
            Next
            If flag = False Then
                Response.Write("Stock yang discan tidak ada dalam daftar tunggu!")
            Else
                scanProcess(scanSTOCKID)
            End If

            txtScanCode.Text = ""
        Catch ex As Exception
            If ConnectionState.Open = True Then cn.Close()
            Response.Write("Error code: " + ex.Message)
        End Try
    End Sub

    Private Sub ListView1_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) Handles ListView1.ItemCommand
        Dim cmdName As String = e.CommandName

        If cmdName = "batal" Then
            Try
                Dim arg() As String = e.CommandArgument.ToString().Split(";")

                Dim cancelDEPTID As String = arg(0)
                Dim cancelSTOCKID As String = arg(1)

                batalProcess(cancelSTOCKID)

            Catch ex As Exception
                Response.Write("Error code: " + ex.Message)
            End Try
        End If
    End Sub

    Private Sub txtScanCode_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtScanCode.Disposed
        txtScanCode.Focus()
    End Sub
End Class