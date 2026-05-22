<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaleReport.aspx.cs" Inherits="PurchaseUnloading" %>
<%@ Register src="Includes/menu.ascx" tagname="WebUserControl1" tagprefix="uc1" %>  
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
  <div id='center' class="main center" style="font-size:12px !important;">
    <div class="mainInner">
    
       <h2><span style="background-color:Yellow; font-size:20px !important;">Rashmi Rice Mills Private Limited</span>
      <br /><span style="font-size:18px !important;">Sale Report</span></h2>
      <div class="row" style="text-align:right !important;">
      <span style="font-weight:bold; color:Maroon;">Welcome Operator</span><br />
     
      </div>
      <%--<div class="row">
      
        <label for="lblNo">Invoice No.: </label>
      
          <asp:Label ID="lblNo" runat="server" Text=""></asp:Label>

      
    </div>--%>
  
   <div class="row">
      
      <table width="100%" class="table table-bordered">
      <tr><td align="left"><div><label for="fdate">From Date: </label>
      <input id="fdate" name="fdate" runat="server" required/></div></td>
      <td align="left"><div><label for="tdate">To Date: </label>
      <input id="tdate" name="tdate" runat="server" required/></div></td>
      </tr>
      <tr><td align="left" colspan="2"><div><label for="sPartyName" style="display:table-cell">Party Name: </label>
      <span style="display:table-cell">
          <asp:DropDownList ID="sPartyName" runat="server" class="input-group-sm">
              
          </asp:DropDownList>
      </span>
          
      </div></td></tr>
     
      </table>
      
      
   </div>  
   <br /> 
    <div class="row">
    <div class="col-md-6"></div>
    <div class="col-md-6">
    <input type="submit" id="btnContinue" value="Click To Generate" runat="server" onserverclick="btnContinue_ServerClick"/>
    </div>
        
    </div>
    <br /> 
        <br /> 
    <br /> 
     <div class="row">
    <div class="col-md-6"></div>
    <div class="col-md-6 pull-right">
    <%--<input type="submit" id="Submit1" value="Click To Generate" runat="server" onserverclick="btnContinue_ServerClick"/>--%>
    <button type="submit" id="Export" runat="server" style="font-size:14px" onserverclick="Export_ServerClick">Click To Export <i class="fa fa-file-excel-o"></i></button>
    </div>
        
    </div><br />
    <div class="row table-responsive">
    
        <asp:PlaceHolder ID="DBDataPlaceHolder" runat="server"></asp:PlaceHolder>
                   
    </div>
    
    </div>
    
  </div>
  </div>
    </form>
</body>
</html>
