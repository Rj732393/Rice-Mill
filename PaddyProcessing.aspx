<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaddyProcessing.aspx.cs" Inherits="PaddyProcessing" %>
<%@ Register src="Includes/menu.ascx" tagname="WebUserControl1" tagprefix="uc1" %>  
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>::Welcome To Rice Mills Online Management System::</title>
     <meta name="viewport" content="width=device-width, initial-scale=1"/>
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
      <br />Paddy Processing</h2>
      <div class="row" style="text-align:right !important;">
      <span style="font-weight:bold; color:Maroon;">Welcome Operator</span>
      </div>
  <div class="row">
      <div class="col-25">
        <label for="sdate">Select Date</label>
      </div>
      <div class="col-75"><span style="display:table-cell;"><input id="sdate" name="sdate" runat="server" required></span>
        <span style="display:table-cell;"></span>
          <asp:LinkButton ID="lbrnData" runat="server" onclick="lbrnData_Click">Report</asp:LinkButton>
      </div>
    </div>
    <div class="row">
      <div class="col-25">
        <label for="sPaddyType">Paddy Type</label>
      </div>
      <div class="col-75">
        <select id="sPaddyType" runat="server" style="width:250px;">
              <option>Rupali</option>
              <option>Mansuri</option>
              <option>Sonam</option>
              <option>Hybrid</option>
              
          </select>
      </div>
    </div>
    <div class="row">
    
      <div class="col-25">
        <label for="sRiceType">Rice Type</label>
      </div>
      <div class="col-75">
          <select id="sRiceType" runat="server" style="width:250px;">
              <option>Rashmi Ka 7 Star</option>
              <option>Rashmi Ka Sonam</option>
              <option>7 Star Katarni</option>
              <option>Steam Bran</option>
              
          </select>
      </div>
    </div>
    <div class="row">
    
      <div class="col-25">
        <label for="PaddyWt">Paddy (In KG)</label>
      </div>
      <div class="col-75">
          <asp:TextBox ID="PaddyWt" runat="server" style="text-align:right" OnTextChanged="PaddyWt_TextChanged" AutoPostBack="true"></asp:TextBox>
        <%--<input id="PaddyWt" name="PaddyWt" required runat="server" style="text-align:right" autopostback=true ontextchanged="PaddyWt_TextChanged"/>--%>
      </div>
    </div>
    
     
    <div class="row">
    
      <div class="col-25">
        <label for="RiceWt">Rice (In KG)</label>
      </div>
      <div class="col-75">
      
        <input id="RiceWt" name="RiceWt" required runat="server" style="text-align:right"/>
      </div>
    </div>
    <div class="row">
      <div class="col-25">
        <label for="BrokenWt">Broken (In KG)</label>
      </div>
      <div class="col-75">
        <input id="BrokenWt" name="BrokenWt" required runat="server" style="text-align:right"/>
      </div>
    </div>
    <div class="row">
      <div class="col-25">
        <label for="BranWt">Bran Amount (In KG)</label>
      </div>
      <div class="col-75">
        <input id="BranWt" name="BranWt" required runat="server" style="text-align:right"/>
      </div>
    </div>
    <div class="row">
      <div class="col-25">
        <label for="NakkuWt">Nakku (In KG)</label>
      </div>
      <div class="col-75">
        <input id="NakkuWt" name="NakkuWt" required runat="server" style="text-align:right"/>
      </div>
    </div>
    <div class="row">
      <div class="col-25">
        <label for="NakkuBhusi">Nakku Bhusi (In KG)</label>
      </div>
      <div class="col-75">
        <input id="NakkuBhusi" name="NakkuBhusi" required runat="server" style="text-align:right">
      </div>
    </div>
    <div class="row">
      <div class="col-25">
        <label for="RejectionWt">Rejection (In KG)</label>
      </div>
      <div class="col-75">
        <input id="RejectionWt" name="RejectionWt" required runat="server" style="text-align:right">
      </div>
    </div>
    <div class="row">
      <div class="col-25">
        <label for="HuskWt">Husk (In KG)</label>
      </div>
      <div class="col-75">
        <input id="HuskWt" name="HuskWt" required runat="server" style="text-align:right">
      </div>
    </div>
    
    <br />
    <div class="row">
      <input type="submit" id="btnSave" value="Submit" runat="server" onserverclick="btnSave_ServerClick"/>
    </div>
    <br />
    <div class="row table-responsive" id="prntContent">
    
        <asp:PlaceHolder ID="DBDataPlaceHolder" runat="server"></asp:PlaceHolder>
                   
    </div>
    </div>
    
  </div>
  </div>
    </form>
</body>
</html>
