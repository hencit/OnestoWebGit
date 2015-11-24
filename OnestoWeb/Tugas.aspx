<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Tugas.aspx.vb" Inherits="OnestoWeb.Tugas" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>Tugas</title>
    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
    
    <link href="pikaday-plug/pikaday.css" rel="stylesheet" type="text/css" />
    <link href="pikaday-plug/theme.css" rel="stylesheet" type="text/css" />
    <script src="pikaday-plug/moment.js" type="text/javascript"></script>
    <script src="pikaday-plug/pikaday.js" type="text/javascript"></script>
    <link href="pikaday-plug/site.css" rel="stylesheet" type="text/css" />
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
        <!-- Body --> 
        <div class="panel panel-primary">
          <div class="panel-body">
          <asp:Panel ID="Panel4" runat="server" DefaultButton="btnShow">
            
            <div class="col-md-12"> 
                <asp:CheckBox ID="chbDate" runat="server"></asp:CheckBox>
                <asp:Label ID="Label89" runat="server" Text='Dari Tanggal'></asp:Label>
                <asp:TextBox ID="txtDateFrom" runat="server" MaxLength="10"></asp:TextBox>
                <script type="text/javascript">
                    var picker = new Pikaday(
                    {
                        field: document.getElementById('txtDateFrom'),
                        firstDay: 1,
                        numberOfMonths: 1,
                        theme: 'dark-theme',
                        format: 'DD-MM-YYYY',
                    });
                </script>
                <asp:Label ID="Label88" runat="server" Text=' - '></asp:Label>
                <asp:TextBox ID="txtDateTo" runat="server" MaxLength="10"></asp:TextBox>
                <script type="text/javascript">
                    var picker = new Pikaday(
                    {
                        field: document.getElementById('txtDateTo'),
                        firstDay: 1,
                        numberOfMonths: 1,
                        theme: 'dark-theme',
                        format: 'DD-MM-YYYY',
                    });
                </script>   
            </div>

            <div class="col-md-4"> 
            <asp:Label ID="Label20" runat="server" Text='Tugas No.'></asp:Label>
            <asp:TextBox ID="txtNoTugas" runat="server" MaxLength="50" class="form-control"></asp:TextBox>

            <asp:Label ID="Label21" runat="server" Text='Deskripsi'></asp:Label>
            <asp:TextBox ID="txtDeskripsi" runat="server" MaxLength="255" class="form-control"></asp:TextBox>
            </div>

            <div class="col-md-4"> 
            <asp:Label ID="Label22" runat="server" Text='Nama Karyawan'></asp:Label>
            <asp:TextBox ID="txtNamaKaryawan" runat="server" MaxLength="50" class="form-control"></asp:TextBox>

            <asp:Label ID="Label23" runat="server" Text='Departemen'></asp:Label>
            <asp:TextBox ID="txtDept" runat="server" MaxLength="50" class="form-control"></asp:TextBox>
            </div>

            <div class="col-md-4"> 
            <asp:Label ID="Label24" runat="server" Text='Status'></asp:Label>
            <asp:DropDownList ID="cbStatus" runat="server" class="form-control"></asp:DropDownList>
            <br />
            <br />
            <br />
            <br />
            <br />
            <asp:Button ID="btnShow" runat="server" class="btn btn-primary pull-right" Text="Show" OnClientClick="document.getElementById('HiddenField').value='1'" />
            </div>
        </asp:Panel>
          
          </div>
        </div>
        <div>
        <asp:Button ID="btnAdd" runat="server" class="btn btn-primary" Text="Tambah Baru" OnClientClick="document.getElementById('HiddenField').value='0'" />
        <br />
        </div>
        
        <div class="col-md-12"> <!--Start Listview1-->
            <table class="table table-striped table-bordered table-hover">
                <asp:ListView ID="ListView1" runat="server" Visible="True" >
                <LayoutTemplate>
                    <tr>
                        <th>
                            <asp:Label ID="Label6" runat="server" Text='No Tugas'></asp:Label>
                        </th> 
                        <th>
                            <asp:Label ID="Label1" runat="server" Text='Tanggal Tugas'></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="Label2" runat="server" Text='Deskripsi'></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="Label3" runat="server" Text='Nama Karyawan'></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="Label5" runat="server" Text='Departemen'></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="Label4" runat="server" Text='Status'></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="Label9" runat="server" Text='Deadline'></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="Label10" runat="server" Text='Tanggal Selesai'></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="Label11" runat="server" Text='% Selesai'></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="Label7" runat="server" Text=''></asp:Label>
                        </th>
                    </tr>
                    <tr runat="server" id="itemPlaceholder">
                    </tr>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:LinkButton ID="lbtnTugasNo" runat="server" Text='<% #Eval("tugas_no") %>' OnClientClick="document.getElementById('HiddenField').value='0'" CommandArgument='<% #Eval("tugas_no") %>' CommandName="view"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:Label ID="lbTugasDate" runat="server" Text='<% #Eval("tugas_date") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:LinkButton ID="lbtnTugasDescripton" runat="server" Text='<% #Eval("tugas_description") %>' OnClientClick="document.getElementById('HiddenField').value='0'" CommandArgument='<% #Eval("tugas_no") %>' CommandName="view"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:Label ID="lbKaryawanName" runat="server" Text='<% #Eval("karyawan_name") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbDeptCode" runat="server" Text='<% #Eval("dept_code") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbTugasStatus" runat="server" Text='<% #Eval("tugas_status") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbDueDate" runat="server" Text='<% #Eval("due_date") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbFinishDate" runat="server" Text='<% #Eval("finish_date") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbFinishPercent" runat="server" Text='<% #Eval("finish_percent") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="btnApprove" runat="server" Text="Approve" OnClientClick="document.getElementById('HiddenField').value='0'" CommandArgument='<% #Eval("tugas_no") %>' CommandName="approve" />
                        </td>
                    </tr>
                </ItemTemplate>

                </asp:ListView>
            </table>
        </div> <!--End Listview1-->

    </div>
    </form>
</body>
</html>
