<%@ Page Language="C#" AutoEventWireup="true" CodeFile="logout.aspx.cs" Inherits="Logout" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Logout - Rashmi Rice Mill</title>

    <link href="CSS/logout.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.1/css/all.min.css" />
</head>

<body>

<form id="form1" runat="server">

<div class="page">

    <div class="logout-card">

        <div class="icon">
            <i class="fas fa-sign-out-alt"></i>
        </div>

        <h2>You Have Been Logged Out</h2>

        <p>
            Thank you for using Rashmi Rice Mill Management System.
        </p>

        <a href="Login.aspx" class="btn-login">
            Login Again
        </a>

    </div>

</div>

</form>

</body>
</html>