<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>::Welcome To Rice Mills Online Management System::</title>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta1/dist/css/bootstrap.min.css"/>
    <script type = "text/javascript" src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.0-beta1/dist/js/bootstrap.bundle.min.js"></script>
    <script type = "text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css"/>
    <link href="CSS/login.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <div class="wrapper">
        <div class="logo">
            <img src="Images/logo_circle.png" alt="Prabha Software Technology Pvt. Ltd.">
        </div>
        <div class="text-center mt-4 name">
            Prabha Software Technologies<br />Private Limited
        </div>
        <form id="Form1" class="p-3 mt-3" runat="server">
            <div class="form-field d-flex align-items-center">
                <span class="far fa-user"></span>
                
                <input type="text" runat="server" name="userName" required id="userName" placeholder="Username"/>
            </div>
            <div class="form-field d-flex align-items-center">
                <span class="fas fa-key"></span>
                <input type="password" runat="server" name="pwd" required id="pwd" placeholder="Password"/>
            </div>
        <asp:Button ID="login" runat="server" Text="Login" CssClass="btn mt-3" 
                onclick="login_Click" /><br />
        <asp:Label ID="lblMsg" runat="server" Font-Bold="True" Font-Size="Small" 
                ForeColor="#CC0000"></asp:Label>
            <%--<button class="btn mt-3" runat="server" onserverclick="login">Login</button>--%>
        </form>
        
    </div>
</body>
</html>
