<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="ramount" AutoPostBack="true" runat="server" ontextchanged="ramount_TextChanged"></asp:TextBox>
    <%--<input id="ramount" name="ramount" required runat="server" onserverchange="abc" style="text-align:right"/>--%><br /><br />
    <input id="bamount" name="bamount" required runat="server" style="text-align:right"/>
    </div>
    </form>
</body>
</html>
