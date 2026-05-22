<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PV.aspx.cs" Inherits="PV" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>::Welcome To Rice Mills Online Management System::</title>
     <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
<link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/themes/base/jquery-ui.css" rel="stylesheet" type="text/css">
<%--<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/jquery-ui.min.js"></script>--%>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css"/>
    <link href="CSS/Menu.css" rel="stylesheet" type="text/css" />
     
</head>
<body>
    <form id="form1" runat="server">
    <div class="container">
    <div class="row table-responsive" id="prntContent">
    
        <asp:PlaceHolder ID="DBDataPlaceHolder" runat="server"></asp:PlaceHolder><br />
                   
    </div>
    <div class="row">
        <input type="submit" id="btnSave" value="Click To Print" runat="server" onserverclick="btnSave_ServerClick"/>
        </div>
    </div>
    </form>
</body>
</html>
