<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Testing.aspx.vb" Inherits="OnestoWeb.Testing" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Testing</title>
    <link href="css/bootstrap.min.css" rel="stylesheet" />
    <script type="text/javascript" src="js/jquery-1.11.0.min.js"></script>
    <script type="text/javascript" src="js/bootstrap.min.js"></script>

    <link href="pikaday-plug/pikaday.css" rel="stylesheet" type="text/css" />
    <link href="pikaday-plug/theme.css" rel="stylesheet" type="text/css" />
    <script src="pikaday-plug/moment.js" type="text/javascript"></script>
    <script src="pikaday-plug/pikaday.js" type="text/javascript"></script>
    <link href="pikaday-plug/site.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
    <div>
    <table style="width: 90%">
    <tr>
    <td style="width: 100px">
    Single File Upload:<br />
    <asp:FileUpload ID="FileUpload1" runat="server" /><br />
    <asp:Button ID="buttonUpload" runat="server" Text="Upload" /><br />
    <br />
    Multi-File Upload:<br />
    <asp:FileUpload ID="multiUpload1" runat="server" /><br />
    <asp:FileUpload ID="multiUpload2" runat="server" /><br />
    <asp:FileUpload ID="multiUpload3" runat="server" /><br />
    <asp:Button ID="buttonMultiUpload" runat="server" Text="Upload" /></td>
    <td style="width: 100px">
    <asp:GridView ID="UploadedFiles" DataSource="<%# GetUploadList() %>" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None">
    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <RowStyle BackColor="#EFF3FB" />
    <EditRowStyle BackColor="#2461BF" />
    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
    <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
    </td>
    </tr>
    </table>
    </div>
    </form>
</body>
</html>
