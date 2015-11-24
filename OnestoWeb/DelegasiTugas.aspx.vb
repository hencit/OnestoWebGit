Imports System.Data.SqlClient
Imports System.Data.OleDb
Imports System.Data
Imports System.IO

Public Class DelegasiTugas
    Inherits System.Web.UI.Page
    Dim strConnection As String = My.Settings.ConnStr
    Dim cn As SqlConnection = New SqlConnection(strConnection)
    Dim cmd As SqlCommand
    Dim flag As Integer
    Dim flagLampiran As Integer
    Dim vb_val_before, vb_val_after1, vb_val_after2 As Boolean
    Dim m_kode_karyawan As String
    Dim val1, val2 As String

    'Untuk slideshow
    Dim excelicon As String = "excel.png"
    Dim wordicon As String = "word.png"
    Dim pdficon As String = "pdf.png"
    Dim videoicon As String = "video.png"
    Dim emptyicon As String = "empty.png"
    Dim elseicon As String = "else.png"
    Dim deleteicon As String = "delete.png"


    Dim m_Status As String
    Dim isShowAll As Boolean

    Dim DA As New SqlDataAdapter
    Dim DS As New DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("FileName") IsNot Nothing Then
            Try
                ' Read the file and convert it to Byte Array
                Dim filePath As String = GetSysInit("Lampiran")
                Dim filename As String = Request.QueryString("FileName")
                Dim contenttype As String = "image/" & Path.GetExtension(filename).Replace(".", "")

                Dim fs As FileStream = New FileStream(filePath & filename, FileMode.Open, FileAccess.Read)
                'Dim fs As FileStream = New FileStream(filename, FileMode.Open, FileAccess.Read)
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

        If IsPostBack Then 'generate form Else 'process submitted data End If
            If HiddenField.Value = 1 Then
                If Session("UserID") = Nothing Then
                    Response.Redirect("Login.aspx")
                Else
                    txtKodeTugas.Text = hfKodeTugas.Value
                    txtDeskripsi.Text = hfDeskripsi.Value
                    txtNamaKaryawan.Text = hfNamaKaryawan.Value
                    txtKodeDept.Text = hfDept.Value
                    txtRepeat.Text = hfRepeat.Value

                End If
            Else

            End If
        Else
            If Session("UserID") = Nothing Then
                Response.Redirect("Login.aspx")
            ElseIf Session("TugasNo") = Nothing Then
                getKaryawan()
                getTugas()
                getDept()

                clear_obj()
                lock_obj(False)
            Else
                getKaryawan()
                getTugas()
                getDept()
                txtNoTugas.Text = Session("TugasNo").ToString
                view_record()
                lock_obj(False)
            End If
        End If
        lblAlert.Text = ""
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

    Private Sub lbTugas_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbTugas.Click
        Response.Redirect("Tugas.aspx")
    End Sub

    Public Property m_Flag() As Integer
        Get
            Return flag
        End Get
        Set(ByVal Value As Integer)
            flag = Value
        End Set
    End Property

    Public Property m_TugasNo() As String
        Get
            Return txtNoTugas.Text
        End Get
        Set(ByVal Value As String)
            txtNoTugas.Text = Value
        End Set
    End Property

    Public Property TugasCode() As String
        Get
            Return txtKodeTugas.Text
        End Get
        Set(ByVal Value As String)
            txtKodeTugas.Text = Value
        End Set
    End Property

    Public Property TugasDeskripsi() As String
        Get
            Return txtDeskripsi.Text
        End Get
        Set(ByVal Value As String)
            txtDeskripsi.Text = Value
        End Set
    End Property

    Public Property DeptCode() As String
        Get
            Return txtKodeDept.Text
        End Get
        Set(ByVal Value As String)
            txtKodeDept.Text = Value
        End Set
    End Property

    Public Property NamaKaryawan() As String
        Get
            Return txtNamaKaryawan.Text
        End Get
        Set(ByVal Value As String)
            txtNamaKaryawan.Text = Value
        End Set
    End Property

    Public Property LinkStandard() As String
        Get
            Return lblStandard.Value
        End Get
        Set(ByVal Value As String)
            lblStandard.Value = Value
        End Set
    End Property

    Public Property KodeKaryawan() As String
        Get
            Return m_kode_karyawan
        End Get
        Set(ByVal Value As String)
            m_kode_karyawan = Value
        End Set
    End Property

    Public Property ValBefore() As Boolean
        Get
            Return vb_val_before
        End Get
        Set(ByVal Value As Boolean)
            vb_val_before = Value
        End Set
    End Property

    Public Property ValAfter1() As Boolean
        Get
            Return vb_val_after1
        End Get
        Set(ByVal Value As Boolean)
            vb_val_after1 = Value
        End Set
    End Property

    Public Property ValAfter2() As Boolean
        Get
            Return vb_val_after2
        End Get
        Set(ByVal Value As Boolean)
            vb_val_after2 = Value
        End Set
    End Property

    Public Property Repeater() As String
        Get
            Return txtRepeat.Text
        End Get
        Set(ByVal Value As String)
            txtRepeat.Text = Value
        End Set
    End Property

    Sub getKaryawan()
        cmd = New SqlCommand("SELECT ID,[DESC] FROM im_karyawan ", cn)
        DA = New SqlDataAdapter(cmd)
        DS = New DataSet
        DA.Fill(DS, "_Karyawan")

        ListView1.DataSource = Nothing
        ListView1.DataSource = DS.Tables("_Karyawan")
        ListView1.DataBind()
    End Sub

    Sub getTugas()
        cmd = New SqlCommand("sp_mt_tugas_SEL", cn)
        cmd.CommandType = CommandType.StoredProcedure

        'Dim prm2 As SqlParameter = cmd.Parameters.Add("@tugas_description", SqlDbType.NVarChar, 50)
        'prm2.Value = IIf(txtTugas.Text = "", DBNull.Value, txtTugas.Text)
        'Dim prm3 As SqlParameter = cmd.Parameters.Add("@dept_code", SqlDbType.NVarChar, 50)
        'prm3.Value = IIf(txtDept.Text = "", DBNull.Value, txtDept.Text)

        DA = New SqlDataAdapter(cmd)
        DS = New DataSet
        DA.Fill(DS, "_Tugas")

        ListView2.DataSource = Nothing
        ListView2.DataSource = DS.Tables("_Tugas")
        ListView2.DataBind()
    End Sub

    Sub getDept()
        cmd = New SqlCommand("sp_mt_dept_SEL", cn)
        cmd.CommandType = CommandType.StoredProcedure

        'Dim prm2 As SqlParameter = cmd.Parameters.Add("@tugas_description", SqlDbType.NVarChar, 50)
        'prm2.Value = IIf(txtTugas.Text = "", DBNull.Value, txtTugas.Text)
        'Dim prm3 As SqlParameter = cmd.Parameters.Add("@dept_code", SqlDbType.NVarChar, 50)
        'prm3.Value = IIf(txtDept.Text = "", DBNull.Value, txtDept.Text)

        DA = New SqlDataAdapter(cmd)
        DS = New DataSet
        DA.Fill(DS, "_Dept")

        ListView3.DataSource = Nothing
        ListView3.DataSource = DS.Tables("_Dept")
        ListView3.DataBind()
    End Sub

    Sub UploadThisFile(ByVal upload As FileUpload, ByVal flag As String)
        If upload.HasFile Then
            Dim theFileName As String = Path.Combine(GetSysInit("Lampiran"), upload.FileName)
            If File.Exists(theFileName) Then
                File.Delete(theFileName)
            End If
            upload.SaveAs(theFileName)
            If flag = "before" Then
                lblBefore.Value = upload.FileName

                saveImage(upload.FileName, flag)

            ElseIf flag = "after1" Then
                lblAfter1.Value = upload.FileName

                saveImage(upload.FileName, flag)


            ElseIf flag = "after2" Then
                lblAfter2.Value = upload.FileName

                saveImage(upload.FileName, flag)

            End If
        End If
    End Sub

    Sub saveImage(ByVal path As String, ByVal flag As String)
        Try
            cmd = New SqlCommand("update tr_tugas set link_" + flag + " = '" + path + "' where tugas_no = '" + txtNoTugas.Text + "' ", cn)

            cn.Open()
            cmd.ExecuteNonQuery()
            cn.Close()
        Catch ex As Exception
            lblAlert.Text = "Error Message : " + ex.Message
            If ConnectionState.Open = 1 Then cn.Close()
        End Try
    End Sub

    Function returnImage(ByVal ext As String) As String
        Dim extension As String
        extension = Path.GetExtension(ext)
        If extension = ".jpg" Or extension = ".png" Or extension = "jpeg" Then
            Return "1"
        ElseIf extension = ".xls" Or extension = ".xlsx" Then
            Return excelicon
        ElseIf extension = ".pdf" Then
            Return pdficon
        ElseIf extension = ".mp4" Or extension = ".wav" Then
            Return videoicon
        ElseIf extension = ".doc" Or extension = ".docx" Then
            Return wordicon
        ElseIf extension = "" Then
            Return emptyicon
        Else
            Return elseicon
        End If
    End Function

    Sub showImage()
        If returnImage(CStr(lblStandard.Value)) = "1" Then
            pbStandard.ImageUrl = "DelegasiTugas.aspx?FileName=" + CStr(lblStandard.Value)
        Else
            pbStandard.ImageUrl = "DelegasiTugas.aspx?FileName=" + returnImage(CStr(lblStandard.Value))
        End If

        If returnImage(CStr(lblBefore.Value)) = "1" Then
            pbBefore.ImageUrl = "DelegasiTugas.aspx?FileName=" + CStr(lblBefore.Value)
        Else
            pbBefore.ImageUrl = "DelegasiTugas.aspx?FileName=" + returnImage(CStr(lblBefore.Value))
        End If

        If returnImage(CStr(lblAfter1.Value)) = "1" Then
            pbAfter1.ImageUrl = "DelegasiTugas.aspx?FileName=" + CStr(lblAfter1.Value)
        Else
            pbAfter1.ImageUrl = "DelegasiTugas.aspx?FileName=" + returnImage(CStr(lblAfter1.Value))
        End If

        If returnImage(CStr(lblAfter2.Value)) = "1" Then
            pbAfter2.ImageUrl = "DelegasiTugas.aspx?FileName=" + CStr(lblAfter2.Value)
        Else
            pbAfter2.ImageUrl = "DelegasiTugas.aspx?FileName=" + returnImage(CStr(lblAfter2.Value))
        End If

    End Sub

    Private Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        Try
            UploadThisFile(fuBefore, "before")
            UploadThisFile(fuAfter1, "after1")
            UploadThisFile(fuAfter2, "after2")

            showImage()
        Catch ex As Exception
            lblAlert.Text = "Pastikan size file yang diupload tidak lebih dari 100 MB" + vbCrLf + "Error code: " + ex.Message
        End Try
    End Sub

    Sub clear_obj()
        HiddenField.Value = 1
        flag = 0
        flagLampiran = 0
        txtNoTugas.Text = ""
        txtKodeTugas.Text = ""
        txtDeskripsi.Text = ""
        m_kode_karyawan = ""
        txtNamaKaryawan.Text = ""
        txtKodeDept.Text = ""
        dtpFrom.Text = System.DateTime.Now.ToString("dd-MM-yyyy")
        dtpTo.Text = System.DateTime.Now.ToString("dd-MM-yyyy")
        dtpTugasDate.Text = System.DateTime.Now.ToString("dd-MM-yyyy")
        dtpDeadline.Text = System.DateTime.Now.ToString("dd-MM-yyyy")
        txtStatus.Text = ""
        txtRepeat.Text = "0"
        lblStandard.Value = ""
        lblBefore.Value = ""
        lblAfter1.Value = ""
        lblAfter2.Value = ""
        cbSelesai.Checked = False
        'dtpSelesai.Value = System.DateTime.Now
        txtSelesaiPersentase.Text = "0"
        txtSelesaiNotes.Text = ""
        'cbApprove1.Checked = False
        'dtpApprove1.Value = System.DateTime.Now
        'txtApprove1Persentase.Text = "0"
        'txtApprove1Notes.Text = ""
        'cbApprove2.Checked = False
        'dtpApprove2.Value = System.DateTime.Now
        'txtApprove2Persentase.Text = "0"
        'txtApprove2Notes.Text = ""
        'txtRefNo.Text = ""
        'dtpTugasDate.Value = System.DateTime.Now
        'txtSubmit.Text = ""
        vb_val_before = False
        vb_val_after1 = False
        vb_val_after2 = False
        lblAlert.Text = ""
        showImage()
    End Sub

    Sub lock_obj(ByVal isLock As Boolean)
        txtDeskripsi.ReadOnly = isLock
        txtRepeat.ReadOnly = isLock
        dtpDeadline.Enabled = Not isLock

        dtpFrom.Enabled = Not isLock
        dtpTo.Enabled = Not isLock

        btnAdd.Enabled = Not isLock
        btnSave.Enabled = Not isLock
        btnBatal.Enabled = Not isLock

        If flag = 0 Then
            btnDept.Enabled = True
            btnKaryawan.Enabled = True
            btnBatal.Enabled = False
            btnAdd.Enabled = False

            Label31.Visible = True
            dtpDeadlineJam.Visible = True
            Label30.Visible = True
            dtpDeadlineMenit.Visible = True
        Else
            btnDept.Enabled = False
            btnKaryawan.Enabled = False
            dtpDeadline.Enabled = False

            Label31.Visible = False
            dtpDeadlineJam.Visible = False
            Label30.Visible = False
            dtpDeadlineMenit.Visible = False
        End If

        If txtRefNo.Text <> "" Then
            panelHeader.Enabled = False
            btnSave.Enabled = False
            btnBatal.Enabled = False
        End If

        If txtStatus.Text = "" Then
            panelSelesai.Enabled = False
            btnBatal.Enabled = False
        Else
            btnSave.Enabled = False
        End If

        If txtStatus.Text = "Outstanding" Then
            panelHeader.Enabled = False
            panelSelesai.Enabled = Not isLock
        End If

        If txtStatus.Text = "Selesai" Then
            panelHeader.Enabled = False
            panelSelesai.Enabled = False
        End If

        If txtStatus.Text = "Approve1" Then
            panelHeader.Enabled = False
            panelSelesai.Enabled = False
        End If

        If txtStatus.Text = "Complete" Then
            panelHeader.Enabled = False
            panelSelesai.Enabled = False
        End If

        If txtStatus.Text = "Batal" Then
            btnBatal.Enabled = False
            panelHeader.Enabled = False
            panelSelesai.Enabled = False
        End If
    End Sub

    Sub view_record()
        Try
            cmd = New SqlCommand("sp_tr_tugas_SEL", cn)
            cmd.CommandType = CommandType.StoredProcedure

            Dim prm1 As SqlParameter = cmd.Parameters.Add("@tugas_no", SqlDbType.NVarChar, 50)
            prm1.Value = txtNoTugas.Text

            cn.Open()

            Dim myReader As SqlDataReader = cmd.ExecuteReader()

            While myReader.Read
                flag = 1
                'txtNoTugas.Text = myReader.GetInt32(0)
                txtKodeTugas.Text = myReader.GetString(1)
                txtDeskripsi.Text = myReader.GetString(2)
                dtpTugasDate.Text = myReader.GetDateTime(3)
                m_kode_karyawan = myReader.GetString(4)
                txtNamaKaryawan.Text = myReader.GetString(5)
                txtKodeDept.Text = myReader.GetString(6)
                txtStatus.Text = myReader.GetString(7)
                If myReader.IsDBNull(myReader.GetOrdinal("ref_no")) Then
                    txtRefNo.Text = ""
                Else
                    txtRefNo.Text = myReader.GetString(8)
                End If
                dtpFrom.Text = myReader.GetDateTime(9)
                dtpTo.Text = myReader.GetDateTime(10)
                dtpDeadline.Text = myReader.GetDateTime(11)
                If myReader.IsDBNull(myReader.GetOrdinal("link_before")) Then
                    lblBefore.Value = ""
                Else
                    lblBefore.Value = myReader.GetString(12)
                End If
                If myReader.IsDBNull(myReader.GetOrdinal("link_after1")) Then
                    lblAfter1.Value = ""
                Else
                    lblAfter1.Value = myReader.GetString(13)
                End If
                If myReader.IsDBNull(myReader.GetOrdinal("link_after2")) Then
                    lblAfter2.Value = ""
                Else
                    lblAfter2.Value = myReader.GetString(14)
                End If
                txtRepeat.Text = myReader.GetInt32(15)

                'Selesai
                cbSelesai.Checked = myReader.GetBoolean(16)
                txtSelesaiPersentase.Text = CStr(myReader.GetInt32(17))
                If myReader.IsDBNull(myReader.GetOrdinal("finish_date")) Then
                    dtpSelesai.Text = System.DateTime.Now
                Else
                    dtpSelesai.Text = myReader.GetDateTime(18)
                End If
                txtSelesaiNotes.Text = myReader.GetString(19)

                lblStandard.Value = myReader.GetString(29)
                vb_val_before = myReader.GetBoolean(30)
                vb_val_after1 = myReader.GetBoolean(31)
                vb_val_after2 = myReader.GetBoolean(32)

                showImage()
            End While

            myReader.Close()
            cn.Close()

            lock_obj(True)
        Catch ex As Exception
            If ConnectionState.Open = True Then cn.Close()
            lblAlert.Text = "Error code: " + ex.Message
        End Try

    End Sub

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        val1 = "tr_tugas_"
        val2 = "tambah"
        If otorisasi(val1 + val2, Session("UserID").ToString) = False Then
            lblAlert.Text = "Anda tidak mempunyai otorisasi " + val2 + " modul ini!,Silahkan hubungi administrator anda untuk diberikan otorisasi"
            Exit Sub
        End If

        Session("TugasNo") = Nothing
        Response.Redirect("DelegasiTugas.aspx")
    End Sub

    Private Sub btnBatal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBatal.Click
        val1 = "tr_tugas_"
        val2 = "batal"
        If otorisasi(val1 + val2, Session("UserID").ToString) = False Then
            lblAlert.Text = "Anda tidak mempunyai otorisasi " + val2 + " modul ini!,Silahkan hubungi administrator anda untuk diberikan otorisasi"
            Exit Sub
        End If


        Try
            cmd = New SqlCommand("sp_tr_tugas_BATAL", cn)
            cmd.CommandType = CommandType.StoredProcedure

            Dim prm1 As SqlParameter = cmd.Parameters.Add("@tugas_code", SqlDbType.NVarChar, 50)
            prm1.Value = txtKodeTugas.Text
            Dim prm2 As SqlParameter = cmd.Parameters.Add("@ref_no", SqlDbType.NVarChar, 25)
            prm2.Value = txtNoTugas.Text
            Dim prm3 As SqlParameter = cmd.Parameters.Add("@tugas_status", SqlDbType.NVarChar, 25)
            prm3.Value = "Outstanding"
            Dim prm4 As SqlParameter = cmd.Parameters.Add("@effective_date_from", SqlDbType.DateTime)
            prm4.Value = dtpFrom.Text
            Dim prm5 As SqlParameter = cmd.Parameters.Add("@effective_date_to", SqlDbType.DateTime)
            prm5.Value = dtpTo.Text
            Dim prm22 As SqlParameter = cmd.Parameters.Add("@user_code", SqlDbType.NVarChar, 50)
            prm22.Value = Session("UserID").ToString()

            cn.Open()
            cmd.ExecuteNonQuery()
            cn.Close()

            lblAlert.Text = "Tugas " + txtDeskripsi.Text + vbCrLf + " yang masih tersisa untuk karyawan dengan kode : " + txtNamaKaryawan.Text + " sudah dibatalkan!"
            view_record()
            lock_obj(False)

        Catch ex As Exception
            If ConnectionState.Open = True Then cn.Close()
            lblAlert.Text = "Error Message : " + ex.Message
        End Try


    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        val1 = "tr_tugas_"
        val2 = "simpan"
        If otorisasi(val1 + val2, Session("UserID").ToString) = False Then
            lblAlert.Text = "Anda tidak mempunyai otorisasi " + val2 + " modul ini!,Silahkan hubungi administrator anda untuk diberikan otorisasi"
            Exit Sub
        End If

        If txtKodeTugas.Text = "" Then
            lblAlert.Text = "Kode tugas tidak boleh kosong!"
            txtKodeTugas.Focus()
            Exit Sub
        End If

        If txtDeskripsi.Text = "" Then
            lblAlert.Text = "Deskripsi tidak boleh kosong!"
            txtDeskripsi.Focus()
            Exit Sub
        End If

        If txtNamaKaryawan.Text = "" Then
            lblAlert.Text = "Karyawan tidak boleh kosong!"
            btnKaryawan.Focus()
            Exit Sub
        End If

        If txtKodeDept.Text = "" Then
            lblAlert.Text = "Departemen tidak boleh kosong!"
            btnDept.Focus()
            Exit Sub
        End If

        If CInt(dtpDeadlineJam.Text) > 23 Then
            lblAlert.Text = "Format Jam seharusnya dari 00:00 sampai 23:59"
            dtpDeadlineJam.Focus()
            Exit Sub
        End If

        If CInt(dtpDeadlineMenit.Text) > 59 Then
            lblAlert.Text = "Format Jam seharusnya dari 00:00 sampai 23:59"
            dtpDeadlineMenit.Focus()
            Exit Sub
        End If

        If txtRepeat.Text = "" Then
            txtRepeat.Text = "0"
        End If

        Try
            If flag = 0 Then
                cmd = New SqlCommand("sp_tr_tugas_INS", cn)
                cmd.CommandType = CommandType.StoredProcedure

                Dim prm2 As SqlParameter = cmd.Parameters.Add("@tugas_code", SqlDbType.NVarChar, 50)
                prm2.Value = txtKodeTugas.Text
                Dim prm3 As SqlParameter = cmd.Parameters.Add("@tugas_description", SqlDbType.NVarChar, 255)
                prm3.Value = txtDeskripsi.Text
                Dim prm4 As SqlParameter = cmd.Parameters.Add("@karyawan_code", SqlDbType.NVarChar, 50)
                prm4.Value = hfIDKaryawan.Value
                Dim prm5 As SqlParameter = cmd.Parameters.Add("@karyawan_name", SqlDbType.NVarChar, 50)
                prm5.Value = txtNamaKaryawan.Text
                Dim prm6 As SqlParameter = cmd.Parameters.Add("@dept_code", SqlDbType.NVarChar, 50)
                prm6.Value = txtKodeDept.Text
                Dim prm7 As SqlParameter = cmd.Parameters.Add("@effective_date_from", SqlDbType.DateTime)
                prm7.Value = dtpFrom.Text
                Dim prm8 As SqlParameter = cmd.Parameters.Add("@effective_date_to", SqlDbType.DateTime)
                prm8.Value = dtpTo.Text
                Dim prm9 As SqlParameter = cmd.Parameters.Add("@due_date", SqlDbType.DateTime)
                prm9.Value = dtpDeadline.Text + " " + dtpDeadlineJam.Text + ":" + dtpDeadlineMenit.Text
                Dim prm10 As SqlParameter = cmd.Parameters.Add("@repeat", SqlDbType.Int)
                prm10.Value = CInt(txtRepeat.Text)
                Dim prm11 As SqlParameter = cmd.Parameters.Add("@tugas_date", SqlDbType.SmallDateTime)
                prm11.Value = dtpTugasDate.Text

                Dim prm20 As SqlParameter = cmd.Parameters.Add("@tugas_status", SqlDbType.NVarChar, 25)
                prm20.Value = "Outstanding"

                Dim prm21 As SqlParameter = cmd.Parameters.Add("@ref_no", SqlDbType.NVarChar, 25)
                prm21.Value = ""

                Dim prm22 As SqlParameter = cmd.Parameters.Add("@user_code", SqlDbType.NVarChar, 50)
                prm22.Value = Session("UserID").ToString()

                Dim prm23 As SqlParameter = cmd.Parameters.Add("@tugas_no", SqlDbType.Int)
                prm23.Direction = ParameterDirection.Output

                cn.Open()
                cmd.ExecuteReader()
                cn.Close()
                txtNoTugas.Text = CStr(prm23.Value)
                insert_Scheduler(dtpFrom.Text, dtpTo.Text)

            ElseIf flag <> 0 Then
               
            End If

            view_record()
            lock_obj(False)
            flag = 1
        Catch ex As Exception
            If ConnectionState.Open = True Then cn.Close()
            lblAlert.Text = "Error Message : " + ex.Message
        End Try
    End Sub

    Sub insert_Scheduler(ByVal timeFrom As DateTime, ByVal timeTo As DateTime)

        Dim m_datediff As Integer = CInt(DateDiff(DateInterval.Day, timeFrom, timeTo))
        Dim m_repeat As Integer = CInt(txtRepeat.Text)
        If m_datediff <> 0 And m_repeat <> 0 Then
            Try
                Dim range, i As Integer
                Dim m_tugas_date As DateTime = dtpTugasDate.Text
                Dim m_tugas_deadline As DateTime = dtpDeadline.Text
                range = m_datediff \ m_repeat

                For i = 1 To range

                    m_tugas_date = DateAdd(DateInterval.Day, CDbl(txtRepeat.Text), m_tugas_date)
                    m_tugas_deadline = DateAdd(DateInterval.Day, CDbl(txtRepeat.Text), m_tugas_deadline)

                    cmd = New SqlCommand("sp_tr_tugas_INS", cn)
                    cmd.CommandType = CommandType.StoredProcedure

                    Dim prm2 As SqlParameter = cmd.Parameters.Add("@tugas_code", SqlDbType.NVarChar, 50)
                    prm2.Value = txtKodeTugas.Text
                    Dim prm3 As SqlParameter = cmd.Parameters.Add("@tugas_description", SqlDbType.NVarChar, 255)
                    prm3.Value = txtDeskripsi.Text
                    Dim prm4 As SqlParameter = cmd.Parameters.Add("@karyawan_code", SqlDbType.NVarChar, 50)
                    prm4.Value = hfIDKaryawan.Value
                    Dim prm5 As SqlParameter = cmd.Parameters.Add("@karyawan_name", SqlDbType.NVarChar, 50)
                    prm5.Value = txtNamaKaryawan.Text
                    Dim prm6 As SqlParameter = cmd.Parameters.Add("@dept_code", SqlDbType.NVarChar, 50)
                    prm6.Value = txtKodeDept.Text
                    Dim prm7 As SqlParameter = cmd.Parameters.Add("@effective_date_from", SqlDbType.DateTime)
                    prm7.Value = dtpFrom.Text
                    Dim prm8 As SqlParameter = cmd.Parameters.Add("@effective_date_to", SqlDbType.DateTime)
                    prm8.Value = dtpTo.Text
                    Dim prm9 As SqlParameter = cmd.Parameters.Add("@due_date", SqlDbType.SmallDateTime)
                    prm9.Value = m_tugas_deadline + " " + dtpDeadlineJam.Text + ":" + dtpDeadlineMenit.Text
                    Dim prm10 As SqlParameter = cmd.Parameters.Add("@repeat", SqlDbType.Int)
                    prm10.Value = CInt(txtRepeat.Text)
                    Dim prm11 As SqlParameter = cmd.Parameters.Add("@tugas_date", SqlDbType.SmallDateTime)
                    prm11.Value = m_tugas_date

                    Dim prm20 As SqlParameter = cmd.Parameters.Add("@tugas_status", SqlDbType.NVarChar, 25)
                    prm20.Value = "Outstanding"

                    Dim prm21 As SqlParameter = cmd.Parameters.Add("@ref_no", SqlDbType.NVarChar, 25)
                    prm21.Value = txtNoTugas.Text

                    Dim prm22 As SqlParameter = cmd.Parameters.Add("@user_code", SqlDbType.NVarChar, 50)
                    prm22.Value = Session("UserID").ToString()

                    Dim prm23 As SqlParameter = cmd.Parameters.Add("@tugas_no", SqlDbType.Int)
                    prm23.Direction = ParameterDirection.Output

                    cn.Open()
                    cmd.ExecuteNonQuery()
                    cn.Close()
                Next
            Catch ex As Exception
                If ConnectionState.Open = True Then cn.Close()
                lblAlert.Text = "Error Message : " + ex.Message
            End Try
        End If
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        showImage()
    End Sub

    Private Sub btnApprove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApprove.Click
        Try
            Session("TugasNo") = txtNoTugas.Text
            Session("flagSender") = "DelegasiTugas"
            Response.Redirect("Approve.aspx")
        Catch ex As Exception
            lblAlert.Text = "Error Message : " + ex.Message
        End Try
    End Sub

    Private Sub btnSelesai_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSelesai.Click
        val1 = "tr_tugas_"
        val2 = "submit"
        If otorisasi(val1 + val2, Session("UserID").ToString) = False Then
            lblAlert.Text = "Anda tidak mempunyai otorisasi " + val2 + " modul ini!,Silahkan hubungi administrator anda untuk diberikan otorisasi"
            Exit Sub
        End If

        If CInt(txtSelesaiPersentase.Text) = 0 Then
            lblAlert.Text = "Persentase tugas tidak boleh kosong!"
            txtSelesaiPersentase.Focus()
            Exit Sub
        End If

        If lblBefore.Value = "" And m_val_before.Value = "True" Then
            lblAlert.Text = "Tugas ini memerlukan lampiran before sebelum bisa disubmit selesai!"
            fuBefore.Focus()
            Exit Sub
        End If

        If lblAfter1.Value = "" And m_val_after1.Value = "True" Then
            lblAlert.Text = "Tugas ini memerlukan lampiran after 1 sebelum bisa disubmit selesai!"
            fuAfter1.Focus()
            Exit Sub
        End If

        If lblAfter2.Value = "" And m_val_after2.Value = "True" Then
            lblAlert.Text = "Tugas ini memerlukan lampiran after 2 sebelum bisa disubmit selesai!"
            fuAfter2.Focus()
            Exit Sub
        End If

        Try
            cmd = New SqlCommand("sp_tr_tugas_SELESAI", cn)
            cmd.CommandType = CommandType.StoredProcedure

            Dim prm1 As SqlParameter = cmd.Parameters.Add("@tugas_no", SqlDbType.Int)
            prm1.Value = CInt(txtNoTugas.Text)
            Dim prm12 As SqlParameter = cmd.Parameters.Add("@finish_flag", SqlDbType.Bit)
            prm12.Value = True
            Dim prm3 As SqlParameter = cmd.Parameters.Add("@finish_percent", SqlDbType.Int)
            prm3.Value = CInt(txtSelesaiPersentase.Text)
            Dim prm4 As SqlParameter = cmd.Parameters.Add("@finish_date", SqlDbType.NVarChar, 30)
            prm4.Value = dtpSelesai.Text
            Dim prm5 As SqlParameter = cmd.Parameters.Add("@link_before", SqlDbType.NVarChar, 100)
            prm5.Value = lblBefore.Value
            Dim prm6 As SqlParameter = cmd.Parameters.Add("@link_after1", SqlDbType.NVarChar, 100)
            prm6.Value = lblAfter1.Value
            Dim prm7 As SqlParameter = cmd.Parameters.Add("@link_after2", SqlDbType.NVarChar, 100)
            prm7.Value = lblAfter2.Value
            Dim prm8 As SqlParameter = cmd.Parameters.Add("@tugas_status", SqlDbType.NVarChar, 25)
            prm8.Value = "Selesai"
            Dim prm9 As SqlParameter = cmd.Parameters.Add("@finish_notes", SqlDbType.NVarChar, 255)
            prm9.Value = txtSelesaiNotes.Text

            Dim prm22 As SqlParameter = cmd.Parameters.Add("@user_code", SqlDbType.NVarChar, 50)
            prm22.Value = Session("UserID").ToString()

            cn.Open()
            cmd.ExecuteNonQuery()
            cn.Close()

            view_record()
            lock_obj(False)
            flag = 1
        Catch ex As Exception
            If ConnectionState.Open = True Then cn.Close()
            MsgBox(ex.Message)
        End Try

        btnRefresh_Click(sender, e)
    End Sub
End Class