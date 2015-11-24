<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Approve.aspx.vb" Inherits="OnestoWeb.Approve" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>Approve</title>
    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />

    <script language="javascript" type="text/javascript">
       function isNumberKey(evt) {
       var charCode = (evt.which) ? evt.which : event.keyCode
       if (charCode > 31 && (charCode < 48 || charCode > 57))
            return false;

        return true;
        }      
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container-fluid">
    
        <asp:HiddenField ID="HiddenField" runat="server" Value="1" />
        <!-- Body --> 
        <br />
          <div>
                <asp:Label ID="Label5" runat="server" Text='No Tugas'></asp:Label>
                <asp:TextBox ID="txtNoTugas" runat="server" MaxLength="50" ReadOnly="True"></asp:TextBox>

                <asp:Label ID="Label6" runat="server" Text='Status'></asp:Label>
                <asp:TextBox ID="txtTugasStatus" runat="server" MaxLength="50" ReadOnly="True"></asp:TextBox>

          </div>
        <br />
        <div class="panel panel-primary">
          <div class="panel-body">
          <asp:Panel ID="PanelApprove1" runat="server" DefaultButton="btnApprove1">
            <div > 
                <asp:Label ID="Label1" runat="server" Text='% Approve1'></asp:Label>
                <asp:TextBox ID="txtApprove1Persentase" runat="server" MaxLength="3" onkeypress="return isNumberKey(event)" class="form-control"></asp:TextBox>
                
                <asp:Label ID="Label2" runat="server" Text='Keterangan'></asp:Label>
                <asp:TextBox ID="txtApprove1Notes" runat="server" MaxLength="255" TextMode="multiline" class="form-control"></asp:TextBox>
                <br />
                <asp:Button ID="btnApprove1" runat="server" class="btn btn-primary" Text="Approve1" OnClientClick="document.getElementById('HiddenField').value='0'" />  
            </div>
            
          </asp:Panel>    
          </div>
        </div>

        <div class="panel panel-primary">
          <div class="panel-body">

            <asp:Panel ID="PanelApprove2" runat="server" DefaultButton="btnApprove2">
                <div> 
                    <asp:Label ID="Label3" runat="server" Text='% Approve2'></asp:Label>
                    <asp:TextBox ID="txtApprove2Persentase" runat="server" MaxLength="3" onkeypress="return isNumberKey(event)" class="form-control"></asp:TextBox>

                    <asp:Label ID="Label4" runat="server" Text='Keterangan'></asp:Label>
                    <asp:TextBox ID="txtApprove2Notes" runat="server" MaxLength="255" TextMode="multiline" class="form-control"></asp:TextBox>
                    <br />
                    <asp:Button ID="btnApprove2" runat="server" class="btn btn-primary" Text="Approve2" OnClientClick="document.getElementById('HiddenField').value='0'" />  
                </div>
            </asp:Panel>
          </div>
        </div>
        <asp:Button ID="btnClose" runat="server" class="btn btn-primary pull-right" Text="Close" OnClientClick="document.getElementById('HiddenField').value='0'" />
    </div>
    </form>
</body>
</html>
