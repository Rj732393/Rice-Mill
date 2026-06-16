<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="AddOperator.aspx.cs"
    Inherits="admin_AddOperator" %>

<%@ Register Src="../Includes/AdminMenu.ascx"
    TagName="WebUserControl1"
    TagPrefix="uc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Add Operator</title>

    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="stylesheet"
        href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />

    <link rel="stylesheet"
        href="https://use.fontawesome.com/releases/v5.7.2/css/all.css" />

    <link href="../CSS/AdminMenu.css" rel="stylesheet" />

    <style>

        body{ background:#f1f5f9; }

        .main-wrapper{
            margin-left:270px;
            margin-top:95px;
            padding:30px;
        }

        .card-box{
            background:#fff;
            padding:25px;
            border-radius:20px;
            max-width:500px;
        }

        .card-box h3{
            margin-bottom:18px;
            color:#1e293b;
        }

    </style>

</head>

<body>

<form id="form1" runat="server">

<uc1:WebUserControl1 ID="WebUserControl11" runat="server" />

<div class="main-wrapper">

    <div class="card-box">

        <h3>Add Operator</h3>

        <asp:Label ID="lblMsg" runat="server" ForeColor="Green" Font-Bold="true" /><br /><br />

        <asp:TextBox ID="txtUser" runat="server"
            CssClass="form-control"
            placeholder="Username"></asp:TextBox>

        <br />

        <asp:TextBox ID="txtPass" runat="server"
            CssClass="form-control"
            placeholder="Password"></asp:TextBox>

        <br />

        <asp:Button ID="btnAdd" runat="server"
            Text="Save"
            CssClass="btn btn-success"
            OnClick="btnAdd_Click" />

        <a href="OperatorsList.aspx" class="btn btn-secondary">View Operators</a>

    </div>

</div>

</form>

</body>
</html>