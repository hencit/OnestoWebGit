<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DelegasiTugas.aspx.vb" Inherits="OnestoWeb.DelegasiTugas" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml>
<head id="Head1" runat="server">
    <title>Tugas</title>
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" src="js/bootstrap.min.js"></script>

    <link href="pikaday-plug/pikaday.css" rel="stylesheet" type="text/css" />
    <link href="pikaday-plug/theme.css" rel="stylesheet" type="text/css" />
    <script src="pikaday-plug/moment.js" type="text/javascript"></script>
    <script src="pikaday-plug/pikaday.js" type="text/javascript"></script>
    <link href="pikaday-plug/site.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
    .img:hover
    {
    cursor:pointer;
    z-index:1; 
    height:auto;
    width:auto;
    transform:scale(4);
    -ms-transform:scale(4); /*IE9*/
    -moz-transform:scale(4); /*Firefox*/
    -webkit-transform:scale(4); /*safari and chrome*/
    -o-transform:scale(4); /*Opera*/
    box-shadow: 3px 3px 1px #111111;
    }
    .rot:hover
    {
    transition:width 2s, height 2s, transform 2s;
    -webkit-transition:width 2s, height 2s, -webkit-transform 2s; /*safari and chrome*/
    transform:rotate(360deg);
    -webkit-transform:rotate(360deg); /*safari*/   
    }
    </style>

    <script language="javascript" type="text/javascript">
        function karyawanFunction(id,desc) {
            document.getElementById('hfIDKaryawan').value = id;
            document.getElementById('txtNamaKaryawan').value = desc;
            document.getElementById('hfNamaKaryawan').value = desc;
        }

        function tugasFunction(tugas_code, tugas_description, dept_code, repeater,link_standard, val_before, val_after1, val_after2 ) {
            document.getElementById('txtKodeTugas').value = tugas_code;
            document.getElementById('hfKodeTugas').value = tugas_code;
            document.getElementById('txtDeskripsi').value = tugas_description;
            document.getElementById('hfDeskripsi').value = tugas_description;
            document.getElementById('txtKodeDept').value = dept_code;
            document.getElementById('hfDept').value = dept_code;
            document.getElementById('txtRepeat').value = repeater;
            document.getElementById('hfRepeat').value = repeater;
            document.getElementById('m_val_before').value = val_before;
            document.getElementById('m_val_after1').value = val_after1;
            document.getElementById('m_val_after2').value = val_after2;
            document.getElementById('lblStandard').value = link_standard;

            var clickButton = document.getElementById("btnRefresh");
            clickButton.click();
        }

        function deptFunction(dept_code) {
            document.getElementById('txtKodeDept').value = dept_code;
            document.getElementById('hfDept').value = dept_code;
        }

        
       function isNumberKey(evt) {
       var charCode = (evt.which) ? evt.which : event.keyCode
       if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;

        return true;
        }

        function getPikaday(id) {
        var picker = new Pikaday(
                    {
                        field: document.getElementById(id),
                        firstDay: 1,
                        numberOfMonths: 1,
                        theme: 'dark-theme',
                        format: 'DD-MM-YYYY',
                    });
        }       
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div class="container-fluid">
        <nav class="navbar navbar-inverse">
            <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
              <ul class="nav navbar-nav">
                <li><asp:LinkButton ID="lbMain" runat="server" Text="Main"></asp:LinkButton></li>
                <li><asp:LinkButton ID="lbTugas" runat="server" Text="Tugas"></asp:LinkButton></li>
                <li><asp:LinkButton ID="lbStock" runat="server" Text="Stock"></asp:LinkButton></li>
              </ul>
              
              <ul class="nav navbar-nav navbar-right">
                <li><asp:LinkButton ID="lbLogout" runat="server" Text="Logout"></asp:LinkButton></li>
              </ul>
            </div><!-- /.navbar-collapse -->
        </nav>

        <asp:HiddenField ID="HiddenField" runat="server" Value="1" />
        <asp:HiddenField ID="hfIDKaryawan" runat="server" Value="" />
        <asp:HiddenField ID="hfNamaKaryawan" runat="server" Value="" />
        <asp:HiddenField ID="hfDept" runat="server" Value="" />
        <asp:HiddenField ID="hfKodeTugas" runat="server" Value="" />
        <asp:HiddenField ID="hfDeskripsi" runat="server" Value="" />
        <asp:HiddenField ID="hfRepeat" runat="server" Value="" />
        <asp:HiddenField ID="m_val_before" runat="server" Value="" />
        <asp:HiddenField ID="m_val_after1" runat="server" Value="" />
        <asp:HiddenField ID="m_val_after2" runat="server" Value="" />
        <asp:HiddenField ID="lblStandard" runat="server" Value="" />
        <asp:HiddenField ID="lblBefore" runat="server" Value="" />
        <asp:HiddenField ID="lblAfter1" runat="server" Value="" />
        <asp:HiddenField ID="lblAfter2" runat="server" Value="" />
        <!-- Start panelHeader --> 
        <div class="col-md-4"> 
        <div class="panel panel-primary">
          <div class="panel-body">
          <asp:Panel ID="panelHeader" runat="server">
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                <asp:Label ID="Label20" runat="server" Text='Tugas No.'></asp:Label>
                <asp:TextBox ID="txtNoTugas" runat="server" MaxLength="50" ReadOnly="True" class="form-control"></asp:TextBox>
   
                <asp:Label ID="Label1" runat="server" Text='Kode Tugas'></asp:Label>
                <asp:Button ID="btnTugas" runat="server" Text="..." CssClass="btn btn-primary btn-sm" data-toggle="modal" data-target="#modalTugas" />
                <asp:TextBox ID="txtKodeTugas" runat="server" MaxLength="50"  ReadOnly="True" class="form-control"></asp:TextBox>

                <asp:Label ID="Label21" runat="server" Text='Deskripsi'></asp:Label>
                <asp:TextBox ID="txtDeskripsi" runat="server" MaxLength="255" TextMode="multiline" class="form-control"></asp:TextBox>
                
                <asp:Label ID="Label2" runat="server" Text='Nama Karyawan'></asp:Label>
                <asp:Button ID="btnKaryawan" runat="server" Text="..." CssClass="btn btn-primary btn-sm" data-toggle="modal" data-target="#modalKaryawan" />
                <asp:TextBox ID="txtNamaKaryawan" runat="server" MaxLength="50" ReadOnly="True" class="form-control"></asp:TextBox>
                
                <asp:Label ID="Label10" runat="server" Text='Dept Code'></asp:Label>
                <asp:Button ID="btnDept" runat="server" Text="..." CssClass="btn btn-primary btn-sm" data-toggle="modal" data-target="#modalDept" />
                <asp:TextBox ID="txtKodeDept" runat="server" MaxLength="50" ReadOnly="True" class="form-control"></asp:TextBox>
               
                <asp:Label ID="Label89" runat="server" Text='Tgl. Tugas Efektif'></asp:Label>
                <br />
                <asp:TextBox ID="dtpFrom" runat="server" MaxLength="10" onkeypress="return getPikaday('dtpFrom')"></asp:TextBox>
                <asp:Label ID="Label88" runat="server" Text=' - '></asp:Label>
                <asp:TextBox ID="dtpTo" runat="server" MaxLength="10" onkeypress="return getPikaday('dtpTo')"></asp:TextBox>
                
                <br />

                <asp:Label ID="Label11" runat="server" Text='Repeat Setiap'></asp:Label>
                <br />
                <asp:TextBox ID="txtRepeat" runat="server" MaxLength="3" onkeypress="return isNumberKey(event)" ></asp:TextBox>
                <br />

                <asp:Label ID="Label12" runat="server" Text='Waktu Tugas Dibuat'></asp:Label>
                <br />
                <asp:TextBox ID="dtpTugasDate" runat="server" MaxLength="10" ReadOnly=true ></asp:TextBox>
                <br />

                <asp:Label ID="Label13" runat="server" Text='Deadline'></asp:Label>
                <br />
                <asp:TextBox ID="dtpDeadline" runat="server" MaxLength="10" onkeypress="return getPikaday('dtpDeadline')"></asp:TextBox>
                
                
                <asp:Label ID="Label31" runat="server" Text='Jam'></asp:Label>
                <asp:TextBox ID="dtpDeadlineJam" runat="server" MaxLength="2" Text="17" columns="2" onkeypress="return isNumberKey(event)"></asp:TextBox>
                <asp:Label ID="Label30" runat="server" Text=':'></asp:Label>
                <asp:TextBox ID="dtpDeadlineMenit" runat="server" MaxLength="2" Text="00" columns="2" onkeypress="return isNumberKey(event)"></asp:TextBox>
                </ContentTemplate>
                </asp:UpdatePanel>
                 
           </asp:Panel>
           </div>
          
        </div>
        </div>
        <!-- End panelHeader --> 

        <!-- Start panelPicArea --> 
        <div class="col-md-8"> 
        <div class="panel panel-primary">
          <div class="panel-body">
          <asp:Panel ID="panelPicArea" runat="server" >
            <asp:Label ID="Label28" runat="server" Text='Status :'></asp:Label>
            <asp:TextBox ID="txtStatus" runat="server" MaxLength="50" columns="30" ReadOnly="True" ></asp:TextBox>
            <asp:Label ID="Label29" runat="server" Text='Ref. No :'></asp:Label>
            <asp:TextBox ID="txtRefNo" runat="server" MaxLength="50" columns="30" ReadOnly="True" ></asp:TextBox>
            <br />
            <asp:Label ID="Label19" runat="server" Text="Standard:"></asp:Label> 
            <asp:Image ID="pbStandard" runat="server" CssClass="img" ImageUrl ="DelegasiTugas.aspx?FileName=''" Width ="200" Height ="100" data-zoom-image="DelegasiTugas.aspx?FileName=''"/>
            <br />
            <br />
            <asp:Label ID="Label23" runat="server" Text="Before:"></asp:Label>
            <asp:Image ID="pbBefore" runat="server" CssClass="img" ImageUrl ="DelegasiTugas.aspx?FileName=''" Width ="200" Height ="100" data-zoom-image="DelegasiTugas.aspx?FileName=''"/>            
            <asp:Label ID="Label24" runat="server" Text="After1:"></asp:Label>
            <asp:Image ID="pbAfter1" runat="server" CssClass="img" ImageUrl ="DelegasiTugas.aspx?FileName=''" Width ="200" Height ="100" data-zoom-image="DelegasiTugas.aspx?FileName=''"/>            
            <asp:Label ID="Label27" runat="server" Text="After2:"></asp:Label>
            <asp:Image ID="pbAfter2" runat="server" CssClass="img" ImageUrl ="DelegasiTugas.aspx?FileName=''" Width ="200" Height ="100" data-zoom-image="DelegasiTugas.aspx?FileName=''"/>
            <br />
          </asp:Panel>
          </div>
        </div>
        </div>
        <!-- End panelPicArea -->

        <!-- Start panelSelesai --> 
        <div class="col-md-4"> 
        <div class="panel panel-primary">
          <div class="panel-body">
          <asp:Panel ID="panelSelesai" runat="server" >
               <asp:CheckBox ID="cbSelesai" runat="server" Enabled="False" /> 
               <asp:Label ID="Label14" runat="server" Text='Selesai'></asp:Label>
               <br />
               
               <asp:Label ID="Label15" runat="server" Text='Persentase'></asp:Label>
               <br />
               <asp:TextBox ID="txtSelesaiPersentase" runat="server" MaxLength="3" onkeypress="return isNumberKey(event)" ></asp:TextBox>
               <asp:Label ID="Label16" runat="server" Text='%'></asp:Label>
               <br />

               <asp:Label ID="Label17" runat="server" Text='Tanggal Selesai'></asp:Label>
               <br />
               <asp:TextBox ID="dtpSelesai" runat="server" MaxLength="20" ReadOnly="True"></asp:TextBox>
               <br />

               <asp:Label ID="Label32" runat="server" Text='Pastikan size file < 100 MB atau sistem akan ERROR!' Font-Bold="True" ForeColor="#FF3300" Font-Size="Large"></asp:Label>
                <br />
               <asp:Label ID="Label22" runat="server" Text='Before'></asp:Label>
               <asp:FileUpload ID="fuBefore" runat="server" />
               <br />

               <asp:Label ID="Label25" runat="server" Text='After1'></asp:Label>
               <asp:FileUpload ID="fuAfter1" runat="server" />
               <br />

               <asp:Label ID="Label26" runat="server" Text='After2'></asp:Label>
               <asp:FileUpload ID="fuAfter2" runat="server" />
               <asp:Button ID="btnUpload" runat="server" class="btn btn-primary btn-sm" Text="Upload" OnClientClick="document.getElementById('HiddenField').value='1'" />
               <br />

               <asp:Label ID="Label18" runat="server" Text='Notes'></asp:Label>
               <br />
               <asp:TextBox ID="txtSelesaiNotes" runat="server" MaxLength="255" TextMode="multiline" class="form-control" ></asp:TextBox>
               <br />

               <asp:Button ID="btnSelesai" runat="server" class="btn btn-primary pull-right" Text="Submit" OnClientClick="document.getElementById('HiddenField').value='1'" />
          </asp:Panel>
          </div>
        </div>
        </div>
        <!-- End panelSelesai --> 
        <asp:Button ID="btnSave" Width="60px" Height="30px" runat="server" Text="Save" OnClientClick="document.getElementById('HiddenField').value='1'" />
        <asp:Button ID="btnAdd" Width="60px" Height="30px" runat="server"  Text="Add" OnClientClick="document.getElementById('HiddenField').value='1'" />  
        <asp:Button ID="btnBatal" Width="60px" Height="30px" runat="server" Text="Batal" OnClientClick="document.getElementById('HiddenField').value='1'" />
        <asp:Button ID="btnApprove" Width="60px" Height="30px" runat="server" Text="Approve" OnClientClick="document.getElementById('HiddenField').value='1'" />
        <div hidden>
        <asp:Button ID="btnRefresh" Width="10px" Height="10px" runat="server" Text="Refresh" OnClientClick="document.getElementById('HiddenField').value='1'" />
        </div>

        <br />
        <!-- Start panelAlert --> 
        <div class="col-md-4"> 
        <div class="panel panel-danger">
          <div class="panel-body">
          <asp:Panel ID="panelAlert" runat="server" >
               <asp:Label ID="lblAlert" runat="server" font-size= 30px  Text=''></asp:Label>
          </asp:Panel>
          </div>
        </div>
        </div>
        <!-- End panelAlert --> 
    </div>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />

    <!-- /.modalTugas -->
    <div class="modal fade" id="modalTugas" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-keyboard="false" data-backdrop="static">
            <div class="modal-dialog">
              <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                <ContentTemplate>
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4>List Tugas</h4>
                    </div>
                    <div class="modal-body">
                         <!--Start Listview2-->
                            <table class="table table-striped table-bordered table-hover">
                                <asp:ListView ID="ListView2" runat="server" Visible="True" >
                                <LayoutTemplate>
                                    <tr>
                                        <th>
                                            <asp:Label ID="Label6" runat="server" Text='Kode Tugas'></asp:Label>
                                        </th> 
                                        <th>
                                            <asp:Label ID="Label1" runat="server" Text='Deskripsi'></asp:Label>
                                        </th>
                                        <th>
                                            <asp:Label ID="Label3" runat="server" Text='Kode Dept.'></asp:Label>
                                        </th>
                                        <th>
                                            <asp:Label ID="Label4" runat="server" Text='Repeater'></asp:Label>
                                        </th>
                                        <th>
                                            <asp:Label ID="Label7" runat="server" Text='Validasi Before'></asp:Label>
                                        </th>
                                        <th>
                                            <asp:Label ID="Label8" runat="server" Text='Validasi After1'></asp:Label>
                                        </th>
                                        <th>
                                            <asp:Label ID="Label9" runat="server" Text='Validasi After2'></asp:Label>
                                        </th>
                                        <th>
                                            <asp:Label ID="Label5" runat="server" Text='Link Standard'></asp:Label>
                                        </th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder">
                                    </tr>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="lbtnTugasCode" runat="server" Text='<% #Eval("tugas_code") %>' 
                                            OnClientClick='<%# String.Format("return tugasFunction(""{0}"", ""{1}"", ""{2}"", ""{3}"", ""{4}"", ""{5}"", ""{6}"", ""{7}"");", 
                                            Eval("tugas_code"), Eval("tugas_description"), Eval("dept_code"), Eval("repeater"), Eval("link_standard"), Eval("val_before"), Eval("val_after1"), Eval("val_after2")) %>'
                                            data-dismiss="modal" aria-hidden="true">
                                            </asp:LinkButton>
                                        </td>   
                                        <td>
                                            <asp:LinkButton ID="lbtnTugasDeskripsi" runat="server" Text='<% #Eval("tugas_description") %>' 
                                            OnClientClick='<%# String.Format("return tugasFunction(""{0}"", ""{1}"", ""{2}"", ""{3}"", ""{4}"", ""{5}"", ""{6}"", ""{7}"");", 
                                            Eval("tugas_code"), Eval("tugas_description"), Eval("dept_code"), Eval("repeater"), Eval("link_standard"), Eval("val_before"), Eval("val_after1"), Eval("val_after2")) %>'
                                            data-dismiss="modal" aria-hidden="true">
                                            </asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTugasDeptCode" runat="server" Text='<% #Eval("dept_code") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTugasRepeater" runat="server" Text='<% #Eval("repeater") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTugasValBefore" runat="server" Text='<% #Eval("val_before") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTugasValAfter1" runat="server" Text='<% #Eval("val_after1") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTugasValAfter2" runat="server" Text='<% #Eval("val_after2") %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblTugasLinkStandard" runat="server" Text='<% #Eval("link_standard") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </ItemTemplate>

                                </asp:ListView>
                            </table>
                         <!--End Listview2-->
                    </div>
                </div>
                    
               </ContentTemplate>
             </asp:UpdatePanel>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modalTugas -->
    
    <!-- /.modalKaryawan -->
    <div class="modal fade" id="modalKaryawan" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-keyboard="false" data-backdrop="static">
            <div class="modal-dialog">
              <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4>List Karyawan</h4>
                    </div>
                    <div class="modal-body">
                         <!--Start Listview1-->
                            <table class="table table-striped table-bordered table-hover">
                                <asp:ListView ID="ListView1" runat="server" Visible="True" >
                                <LayoutTemplate>
                                    <tr>
                                        <th>
                                            <asp:Label ID="Label6" runat="server" Text='ID'></asp:Label>
                                        </th> 
                                        <th>
                                            <asp:Label ID="Label1" runat="server" Text='Nama Karyawan'></asp:Label>
                                        </th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder">
                                    </tr>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="lbtnKodeKaryawan" runat="server" Text='<% #Eval("ID") %>' 
                                            OnClientClick='<%# String.Format("return karyawanFunction({0}, ""{1}"");", Eval("ID"), Eval("DESC")) %>'
                                            data-dismiss="modal" aria-hidden="true">
                                            </asp:LinkButton>
                                        </td>   
                                        <td>
                                            <asp:LinkButton ID="lbtnNamaKaryawan" runat="server" Text='<% #Eval("DESC") %>' 
                                            OnClientClick='<%# String.Format("return karyawanFunction({0}, ""{1}"");", Eval("ID"), Eval("DESC")) %>'
                                            data-dismiss="modal" aria-hidden="true">
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>

                                </asp:ListView>
                            </table>
                         <!--End Listview1-->
                    </div>
                </div>
                    
               </ContentTemplate>
             </asp:UpdatePanel>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modalKaryawan -->

        <!-- /.modalDept -->
        <div class="modal fade" id="modalDept" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" data-keyboard="false" data-backdrop="static">
                <div class="modal-dialog">
                  <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                            <h4>List Dept</h4>
                        </div>
                        <div class="modal-body">
                             <!--Start Listview3-->
                                <table class="table table-striped table-bordered table-hover">
                                    <asp:ListView ID="ListView3" runat="server" Visible="True" >
                                    <LayoutTemplate>
                                        <tr>
                                            <th>
                                                <asp:Label ID="Label6" runat="server" Text='Kode Dept.'></asp:Label>
                                            </th> 
                                            <th>
                                                <asp:Label ID="Label1" runat="server" Text='Nama Dept.'></asp:Label>
                                            </th>
                                        </tr>
                                        <tr runat="server" id="itemPlaceholder">
                                        </tr>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="lbtnKodeDept" runat="server" Text='<% #Eval("dept_code") %>' 
                                                OnClientClick='<%# String.Format("return deptFunction(""{0}"");", Eval("dept_code")) %>'
                                                data-dismiss="modal" aria-hidden="true">
                                                </asp:LinkButton>
                                            </td>   
                                            <td>
                                                <asp:LinkButton ID="lbtnNamaDept" runat="server" Text='<% #Eval("dept_name") %>' 
                                                OnClientClick='<%# String.Format("return deptFunction(""{0}"");", Eval("dept_code")) %>'
                                                data-dismiss="modal" aria-hidden="true">
                                                </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>

                                    </asp:ListView>
                                </table>
                             <!--End Listview3-->
                        </div>
                    </div>
                    
                   </ContentTemplate>
                 </asp:UpdatePanel>
                    <!-- /.modal-content -->
                </div>
                <!-- /.modal-dialog -->
            </div>
            <!-- /.modalDept -->

    </form>
     
</body>
</html>
