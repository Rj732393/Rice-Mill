<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SalePurchaseExpense.aspx.cs" Inherits="admin_SalePurchaseExpense" %>
<%@ Register src="../Includes/adminmenu.ascx" tagname="WebUserControl1" tagprefix="uc1" %>  
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>::Welcome To Rice Mills Online Management System::</title>
     <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css"/>
    <link href="../CSS/Menu.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="header"></div>
    <input type="checkbox" class="openSidebarMenu" id="openSidebarMenu">
  <label for="openSidebarMenu" class="sidebarIconToggle">
    <div class="spinner diagonal part-1"></div>
    <div class="spinner horizontal"></div>
    <div class="spinner diagonal part-2"></div>
  </label>
  <div id="sidebarMenu">
    <uc1:WebUserControl1 ID="WebUserControl11" runat="server" /> 
  </div>
    <div class="container">
  <div id='center' class="main center">
    <div class="mainInner">
    
       <h2><span style="background-color:Yellow;">Rashmi Rice Mills Private Limited</span>
      <br />Sale, Purchase & Expense Report</h2>
      <div class="row" style="text-align:right !important;">
      <span style="font-weight:bold; color:Maroon;">Welcome Admin</span>
      </div>
      
  <div class="row">
      <div class="col-25">
        <label for="fdate">From Date</label>
      </div>
      <div class="col-25">
        <input id="fdate" name="sdate" runat="server"/>
      </div>
      <div class="col-25">
        <label for="tdate">To Date</label>
      </div>
      <div class="col-25">
        <input id="tdate" name="tdate" runat="server"/>
      </div>
    </div>
    <div class="row">
      <div class="col-25">
        <label for="rtype">Report Type</label>
      </div>
      <div class="col-75">
        <select id="srType" runat="server">
        <option>Daily</option>
        <option>Monthly</option>
        <option>Annual</option>
        </select>
      </div>
      </div>
      <div class="row">
      <div class="col-25">
      </div>
     <div class="col-75">
     <input type="submit" id="btnReport" value="Click To Generate" runat="server" onserverclick="btnReport_ServerClick"/>
     </div>
     </div>
    </div>
    <div class="row table-responsive">
    
        <asp:PlaceHolder ID="DBDataPlaceHolder" runat="server"></asp:PlaceHolder></div>
                   
    </div>
    </div>
    
    </form>
</body>
</html>
