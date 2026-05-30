<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rashmi Rice Mill - Login</title>
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link href="CSS/login.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.7.2/css/all.min.css"/>
</head>

<body>

<form id="form1" runat="server">

<div class="page">

    <div class="login-card">

        <!-- LOGO -->
        <div class="logo">
            <img src="Images/logo_circle.png" />
        </div>

        <!-- TITLE -->
        <h2>Rice Mill Management System</h2>
        <p>Login to your dashboard</p>

        <!-- USERNAME -->
        <div class="input">
            <i class="fa fa-user"></i>
            <asp:TextBox ID="userName" runat="server" placeholder="Username"></asp:TextBox>
        </div>

        <!-- PASSWORD -->
        <div class="input">
            <i class="fa fa-lock"></i>
            <asp:TextBox ID="pwd" runat="server" TextMode="Password" placeholder="Password"></asp:TextBox>
        </div>

        <!-- BUTTON -->
        <asp:Button ID="login" runat="server"
            Text="Login"
            CssClass="btn"
            OnClick="login_Click" />

        <!-- MESSAGE -->
        <asp:Label ID="lblMsg" runat="server"></asp:Label>

    </div>

</div>

</form>

</body>
</html>