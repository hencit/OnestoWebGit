<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="OnestoWeb.Login" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title>Login</title>
    <link href="css/bootstrap.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
     <div class="container-fluid">
        <!-- Header -->
        <div class="col-md-12">
            <br />
            <br />
            <asp:Image ID="companyLogo" runat="server" ImageUrl="~/Pic/company-logo.jpg" class="img-responsive center-block" />
            <br />
            <br />
        </div>

        <!-- Menu -->
        <div class="col-md-4">
        </div>
        <div class="col-md-4" style="border:1px solid #888;box-shadow:0px 2px 5px #ccc;">
            <div class="form-group">
            <br />
            <asp:Label ID="Label1" runat="server" Text="User ID"></asp:Label>
            <asp:TextBox ID="txtUserID" runat="server" MaxLength="50" type="text" class="form-control" style="text-transform:uppercase;"></asp:TextBox>
          </div>
          <div class="form-group">
            <asp:Label ID="Label2" runat="server" Text="Password"></asp:Label>
            <asp:TextBox ID="txtPassword" runat="server" MaxLength="50" type="password" class="form-control"></asp:TextBox>
          </div>
        <asp:Button ID="btnLogin" runat="server" Text="Login" class="btn btn-primary" />
            <br />
            <br />
        </div>
    </div>
    </form>
</body>
</html>
