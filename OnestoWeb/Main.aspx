<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Main.aspx.vb" Inherits="OnestoWeb.Main" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>Main</title>
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

         
    </div>
    </form>
</body>
</html>
