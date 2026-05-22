<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaleSauda.aspx.cs" Inherits="SaleSauda" %>
<%@ Register src="Includes/menu.ascx" tagname="WebUserControl1" tagprefix="uc1" %>  
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>::Welcome To Rice Mills Online Management System::</title>
     <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
<link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/themes/base/jquery-ui.css" rel="stylesheet" type="text/css">
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.1/jquery-ui.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css"/>
    <link href="CSS/Menu.css" rel="stylesheet" type="text/css" />
        
    <script type="text/javascript">
        $(document).ready(function () {
            SearchText();
        });
        function SearchText() {
            $("#txtEmpName").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        url: "PurchaseUnloading.aspx/GetEmployeeName",
                        data: "{'empName':'" + document.getElementById('txtEmpName').value + "'}",
                        dataType: "json",
                        success: function (data) {
                            response(data.d);
                        },
                        error: function (result) {
                            //alert("No Match");
                        }
                    });
                }
            });
        }  
    </script>    
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
      <br /><span style="font-size:18px !important;">Sale Sauda</span></h2>
      <div class="row" style="text-align:right !important;">
      <span style="font-weight:bold; color:Maroon;">Welcome Operator</span><br />
     
      </div>
     
   <div class="row"><label for="sdate">Select Date: </label>
      <input id="sdate" name="sdate" runat="server" required/>
      </div>
   <div class="row">
      
      <table width="100%" class="table table-bordered">
      <tr><td align="left" colspan="2"><div><label for="MPNo" style="display:table-cell;">Manual Sauda No. (If any): </label>
      <span style="display:table-cell">
      <input id="MPNo" name="MPNo" runat="server" type="text"/></span>
      </div></td></tr>
      <tr><td align="left" colspan="2"><div><label for="sPartyName" style="display:table-cell">Party Name: </label>
      <span style="display:table-cell">
      <asp:DropDownList ID="sPartyName" runat="server" 
              onselectedindexchanged="sPartyName_SelectedIndexChanged" AutoPostBack="true">
              
          </asp:DropDownList>
      </span><span style="display:table-cell">
          <asp:LinkButton ID="lBtnSaudaParty" runat="server" 
              onclick="lBtnSaudaParty_Click">Sauda List</asp:LinkButton></span>
          <asp:Panel ID="Panel1" runat="server">
        <table width="100%" class="table table-bordered">
        <tr><td align="left"><div>
        <label for="pName" style="display:table-cell">Party Name: </label>
        <span style="display:table-cell">
        <input id="pName" type="text" name="pName" runat="server"/>
        </span>
        </div></td></tr>
        <tr>
        <td align="left"><div>
        <label for="pMN" style="display:table-cell;">Party Mobile No.: </label>
        <span style="display:table-cell">
        <input id="pMN" name="pMN" runat="server" />
        </span>
        </div></td>
        </tr>
       <tr>
        <td align="left" valign="top"><div style="vertical-align:top;">
        <label for="pAddress" style="display:table-cell; vertical-align:top;">Party Address: </label>
        <span style="display:table-cell">
        <textarea id="pAddress" name="pAddress" runat="server" cols="30" rows="3"></textarea>
        
        </span>
        </div></td>
        </tr>
        <tr>
        <td align="left"><div>
        <label for="pGST" style="display:table-cell;">Party GSTIN: </label>
        <span style="display:table-cell">
        <input id="pGST" name="pGST" type="text" runat="server"/>
        </span>
        </div></td>
        </tr>
        <tr>
        <td align="left"><div>
        <label for="pPAN" style="display:table-cell;">Party PAN: </label>
        <span style="display:table-cell">
        <input id="pPAN" name="pPAN" type="text" runat="server"/>
        </span>
        </div></td>
        </tr>
        </table>
        
        </asp:Panel>
      </div></td></tr>
      <tr><td align="left" colspan="2"><div> <label for="txtEmpName" style="display:table-cell">Supplier's Ref.: </label>
      <span style="display:table-cell">
      <asp:TextBox ID="txtEmpName" runat="server"></asp:TextBox></span>
      </div></td></tr>
       
      </table>
      
      
   </div><br />
   <div class="row">
   <table width="100%" class="table table-bordered">
   <tr><td align="center"><div> <label for="sPaddyType" style="display:table-cell">Item Type: </label></div></td>
   <td align="center"><div> <label for="QIKG" style="display:table-cell">Quantity (In KG): </label></div></td>
   <td align="center"><div> <label for="avgrate" style="display:table-cell">Rate (In Rs.): </label></div></td>
   <td align="center"><div>&nbsp;</div></td>
   </tr>
   <tr><td><div>
      <span style="display:table-cell">
      <select id="sPaddyType" runat="server">
      <option>Arwa Rice</option>
              <option>Rashmi Ka 7 Star</option>
              <option>Rashmi Ka Sonam</option>
              <option>7 Star Katarni</option>
              <option>Sri Rajbhog Rice</option>
              <option>Parmal Rice</option>
              <option>Steam Bran</option>
              <option>Naku</option>
              <option>Naku Bhusi</option>
              <option>Husk</option>
              <option>Broken</option>
              <option>Rejection</option>
              <option>Khakhri</option>
              <option>Dust</option>
              <option>PP Bag</option>
              <option>Jute Bag</option>
          </select></span>
      </div></td>
      <td align="left"><div>
      <span style="display:table-cell">
      <input id="QIKG" name="QIKG" required runat="server" style="width:120px; text-align:right"/></span>
      </div></td>
      <td align="left"><div>
      <span style="display:table-cell">
      <input id="avgrate" name="avgrate" required runat="server" style="width:80px; text-align:right"/></span>
      </div></td>
      <td align="center"><div>
      <input type="submit" id="btnContinue" value="Click To Add" runat="server" onserverclick="btnContinue_ServerClick"/>
      </div></td>
      </tr>
      
   </table>
   </div>
   
    <br /> 
    <div class="row table-responsive" id="prntContent">
    
        <asp:PlaceHolder ID="DBDataPlaceHolder" runat="server"></asp:PlaceHolder>
                   
    </div>
    <br />
    <div class="row">
    <div class="col-md-6"><input type="submit" id="Submit1" value="Reset Data" runat="server" onserverclick="Submit1_ServerClick"/></div>
    <div class="col-md-6">
    <input type="submit" id="btnSave" value="Click To Save" runat="server" onserverclick="btnSave_ServerClick"/>
    </div>
        
    </div>
    </div>
    
  </div>
  </div>
    </form>
</body>
</html>
