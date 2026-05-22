<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RiceStock.aspx.cs" Inherits="RiceStock" %>
<%@ Register src="Includes/menu.ascx" tagname="WebUserControl1" tagprefix="uc1" %>  
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>::Welcome To Rice Mills Online Management System::</title>
     <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
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
  <div id='center' class="main center">
    <div class="mainInner">
    
       <h2><span style="background-color:Yellow;">Rashmi Rice Mills Private Limited</span>
      <br />Rice Stock Daily Entry</h2>
      <div class="row" style="text-align:right !important;">
      <span style="font-weight:bold; color:Maroon;">Welcome Operator</span><br />
      <span style="font-weight:bold; color:Black;">
      Opening Stock Balance: 
          <asp:Label ID="lblOSB" runat="server" Text=""></asp:Label> (In KG)</span>
      </div>
      
  <div class="row">
      <div class="col-25">
        <label for="sdate">Select Date</label>
      </div>
      <div class="col-75">
        <input id="sdate" name="sdate" runat="server" required/>&nbsp;&nbsp;
          <asp:LinkButton
            ID="LinkButton1" runat="server" onclick="LinkButton1_Click" style="font-weight:normal !important;">Click To Next</asp:LinkButton>

      </div>
    </div>
        <asp:Panel ID="Panel1" runat="server">
         <div class="row">
      <div class="col-25">
        <label for="rweight">Rice Weight (In KG)</label>
      </div>
      <div class="col-75">
        <input id="rweight" name="rweight" required runat="server" style="text-align:right"/>
      </div>
    </div>
    <div class="row">
      <div class="col-25">
        <label for="avgrate">Average Rate Per KG (In Rs.)</label>
      </div>
      <div class="col-75">
        <input id="avgrate" required name="avgrate" runat="server" style="text-align:right"/>
      </div>
    </div>
    <div class="row">
      <div class="col-25">
        <label for="sconsume">Stock Consume (In KG)</label>
      </div>
      <div class="col-75">
        <input id="sconsume" name="sconsume" required runat="server" style="text-align:right"/>&nbsp;&nbsp;
        <a href="#" id="SCalculate" runat="server" onserverclick="SCalculate_ServerClick">
          <span class="glyphicon glyphicon-refresh"></span>
        </a>
      </div>
    </div>
    <div class="row">
      <div class="col-25">
        <label for="ramount">Rice Amount (In Rs.)</label>
      </div>
      <div class="col-75">
        <input id="ramount" name="ramount" readonly runat="server" style="text-align:right"/>
      </div>
    </div>
    <div class="row">
      <div class="col-25">
        <label for="camount">Consume Amount (In Rs.)</label>
      </div>
      <div class="col-75">
        <input id="camount" name="camount" readonly runat="server" style="text-align:right"/>
      </div>
    </div>
    <div class="row">
      <div class="col-25">
        <label for="sbalance">Stock Balance (In KG)</label>
      </div>
      <div class="col-75">
        <input id="sbalance" name="sbalance" readonly  runat="server" style="text-align:right"/>
      </div>
    </div>
    <div class="row">
      <div class="col-25">
        <label for="sbamount">Stock Balance Amount (In Rs.)</label>
      </div>
      <div class="col-75">
        <input id="sbamount" name="sbamount" readonly runat="server" style="text-align:right"/>
      </div>
    </div>
    <%--<div class="row">
      <div class="col-25">
        <label for="country">Country</label>
      </div>
      <div class="col-75">
        <select id="country" name="country">
          <option value="australia">Australia</option>
          <option value="canada">Canada</option>
          <option value="usa">USA</option>
        </select>
      </div>
    </div>--%>
    <br />
    <div class="row">
      <input type="submit" id="btnSave" value="Submit" runat="server" onserverclick="btnSave_ServerClick"/>
    </div>
        </asp:Panel>
   
    
    </div>
    
  </div>
  </div>
    </form>
</body>
</html>
