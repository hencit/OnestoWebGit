<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Testing2.aspx.vb" Inherits="OnestoWeb.Testing2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField ID="lblBefore" runat="server" Value="" />
    <asp:HiddenField ID="lblAfter1" runat="server" Value="" />
    <asp:HiddenField ID="lblAfter2" runat="server" Value="" />
    <div>
    <asp:image ID="Image4" runat="server" ImageUrl ="testing2.aspx?FileName=''"/>
    <asp:image ID="Image5" runat="server" ImageUrl ="testing2.aspx?FileName=''"/>
    <asp:image ID="Image6" runat="server" ImageUrl ="testing2.aspx?FileName=''"/>
    </div>
    <asp:Button ID="Button1" runat="server" Text="Button" />
    </form>
</body>
</html>
