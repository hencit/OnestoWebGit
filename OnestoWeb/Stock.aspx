<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Stock.aspx.vb" Inherits="OnestoWeb.Stock" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>Stock</title>
    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
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

        <!-- Body -->
        <div class="nav navbar-nav navbar-right">
        <asp:Panel ID="Panel2" runat="server" DefaultButton="btnScan" class="floating-textarea">
          <asp:TextBox ID="txtScanCode" runat="server"></asp:TextBox>
          <asp:Button ID="btnScan" runat="server" Text="" OnClientClick="document.getElementById('HiddenField').value='1'" />
          <asp:HiddenField ID="HiddenField" runat="server" Value="1" />
        </asp:Panel>
        </div>
        <br />

         <asp:DropDownList ID="sortList" runat="server">
         </asp:DropDownList>
        <div class="col-md-12"> <!--Start Listview1-->
            <table class="table table-striped table-bordered table-hover">
                <asp:ListView ID="ListView1" runat="server" Visible="True" >
                <LayoutTemplate>
                    <tr>
                        <th>
                            <asp:Label ID="Label6" runat="server" Text='Kode Dept'></asp:Label>
                        </th> 
                        <th>
                            <asp:Label ID="Label1" runat="server" Text='Kode Barang'></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="Label2" runat="server" Text='Nama Barang'></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="Label3" runat="server" Text='Sales Qty'></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="Label5" runat="server" Text='Scan Qty'></asp:Label>
                        </th>
                        <th>
                            <asp:Label ID="Label4" runat="server" Text='Stock Qty'></asp:Label>
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
                            <asp:Label ID="lblPOSID" runat="server" Text='<% #Eval("DEPTID") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSTOCKID" runat="server" Text='<% #Eval("STOCKID") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSTOCKNAME" runat="server" Text='<% #Eval("STOCKNAME") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblQTY" runat="server" Text='<% #Eval("QTY") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblSCANQTY" runat="server" Text='<% #Eval("QTYSCAN") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label8" runat="server" Text='<% #Eval("CURQTY") %>'></asp:Label>
                        </td>
                        <td>
                            <asp:Button ID="btnBatal" runat="server" Text="Habis" OnClientClick="document.getElementById('HiddenField').value='0'" CommandArgument='<% #Eval("DEPTID") + ";" +Eval("STOCKID")%>' CommandName="batal" />
                        </td>
                    </tr>
                </ItemTemplate>

                </asp:ListView>
            </table>
        </div> <!--End Listview1-->

         
    </div>
    </form>

    <script type="text/javascript" language="javascript">
        setInterval("doSomething()", 5000);

        function doSomething() {
            // (do something here)
            document.getElementById("txtScanCode").focus();
        }
    </script>
</body>
</html>
