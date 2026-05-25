<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseUnloading.aspx.cs" Inherits="PurchaseUnloading" %>
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
      <br /><span style="font-size:18px !important;">Purchase & Unloading Data Entry</span></h2>
      <div class="row" style="text-align:right !important;">
      <span style="font-weight:bold; color:Maroon;">Welcome Operator</span><br />
     
      </div>
     
   <div class="row"><label for="sdate">Select Date: </label>
      <input id="sdate" name="sdate" runat="server" required/>
      </div>
   <div class="row">
      <div class="col-md-6" style="position:inherit !important;">
      <table width="100%" class="table table-bordered">
      <tr><td align="left" colspan="2"><div><label for="MPNo" style="display:table-cell;">Manual Purchase Order No. (If any): </label>
      <span style="display:table-cell">
      <input id="MPNo" name="MPNo" runat="server" type="text"/></span>
      </div></td></tr>
      <tr><td align="left" colspan="2"><div><label for="truckno" style="display:table-cell;">Truck No.: </label>
      <span style="display:table-cell">
      <input id="truckno" name="truckno" runat="server" required/></span>
      </div></td></tr>
      <tr><td align="left" colspan="2"><div><label for="katano" style="display:table-cell">Kanta No.: </label>
      <span style="display:table-cell">
      <input id="katano" name="katano" required runat="server" style="text-align:right"/></span>
      </div></td></tr>
       <tr><td colspan="2" align="left"><div><label for="UAt" style="display:table-cell">Unloaded at: </label>
      <span style="display:table-cell">
      <input id="UAt" name="UAt" type="text" runat="server" required/></span>
      </div></td></tr>
      <tr><td align="left" colspan="2"><div><label for="TWeight" style="display:table-cell">Tare Wt. (In KG): </label>
      <span style="display:table-cell">
      <input id="TWeight" name="TWeight" required runat="server" style="text-align:right"/></span>
      </div></td></tr>
      <tr><td align="left" colspan="2"><div><label for="sPartyName" style="display:table-cell">Party Name: </label>
      <span style="display:table-cell">
      <asp:DropDownList ID="sPartyName" runat="server" 
              onselectedindexchanged="sPartyName_SelectedIndexChanged" AutoPostBack="true">
              
          </asp:DropDownList>
      </span>
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
        <input id="pMN" name="pMN" runat="server"/>
        </span>
        </div></td>
        </tr>
       
        </table>
        
        </asp:Panel>
      </div></td></tr>
      <tr><td align="left" colspan="2"><div> <label for="txtEmpName" style="display:table-cell">Broker Name: </label>
      <span style="display:table-cell">
      <asp:TextBox ID="txtEmpName" runat="server"></asp:TextBox></span>
      </div></td></tr>
       <tr><td align="left"><div><label for="PBags" style="display:table-cell">Plastic Bags: </label>
      <span style="display:table-cell">
      <input id="PBags" name="PBags" runat="server" style="width:50px !important;" required/></span>
      </div></td>
      <td align="left"><div><label for="PTBags" style="display:table-cell">Plastic Torn Bags: </label>
      <span style="display:table-cell">
      <input id="PTBags" name="PTBags" runat="server" style="width:50px !important;" required/></span>
      </div></td>
      </tr>
      <tr><td align="left"><div><label for="JBags" style="display:table-cell">Jute Bags: </label>
      <span style="display:table-cell">
      <input id="JBags" name="JBags" runat="server" style="width:50px !important;" required/></span>
      </div></td>
      <td align="left"><div><label for="JTBags" style="display:table-cell">Jute Torn Bags: </label>
      <span style="display:table-cell">
      <input id="JTBags" name="JTBags" runat="server" style="width:50px !important;" required/></span>
      </div></td>
      </tr>
      <tr><td align="left" colspan="2"><div><label for="SaudaNo" style="display:table-cell">Sauda No. & Date: </label>
      <span style="display:table-cell">
      <input id="SaudaNo" name="SaudaNo" runat="server" style="width:150px !important;" required/></span>
      
      <span style="display:table-cell">
      <input id="SaudaDate" name="SaudaDate" runat="server" required/></span>
      </div></td>
      </tr>
      </table>
      </div>
      <div class="col-md-6" style="position:inherit !important;">
      <table width="100%" class="table table-bordered">
      
      <tr><td align="left"><div><label for="QIK" style="display:table-cell">Avg. Wt. Per Bag (In KG): </label>
      <span style="display:table-cell">
      <input id="QIK" name="QIK" required runat="server" style="text-align:right"/></span>
      </div></td></tr>
      
      <tr><td align="left"><div> <label for="CD" style="display:table-cell">CD (In %) </label>
      <span style="display:table-cell">
      <input id="CD" name="CD" required runat="server" style="width:80px; text-align:right"/>%
      </span>
      </div></td></tr>
      <tr><td align="left">
      <div> <label for="TFreight" style="display:table-cell">Total Freight </label>
      <span style="display:table-cell">
      <input id="TFreight" name="TFreight" required runat="server" style="text-align:right"/>
      </span>
      </div>
      <div> <label for="Freight" style="display:table-cell">Freight (Own) </label>
      <span style="display:table-cell">
      <input id="Freight" name="Freight" required runat="server" style="text-align:right"/>
      </span>
      </div>
      <div> <label for="PFreight" style="display:table-cell">Freight (Paid By Party) </label>
      <span style="display:table-cell">
      <input id="PFreight" name="PFreight" required runat="server" style="text-align:right"/>
      </span>
      </div>
      
      </td></tr>
      
      <tr><td align="left"><div> <label for="Advance" style="display:table-cell">Advance </label>
      <span style="display:table-cell">
      <input id="Advance" name="Advance" required runat="server" style="text-align:right"/>
      </span>
      </div></td></tr>
      <tr><td align="left"><div> <label for="brokerage" style="display:table-cell">Brokerage (of Party) </label>
      <span style="display:table-cell">
      <input id="brokerage" name="brokerage" required runat="server" style="text-align:right"/>
      </span>
      </div></td></tr>
      
      </table>
      </div>
   </div><br />
   <div class="row">
   <div class="col-md-6" style="position:inherit !important;">
   <table width="100%" class="table table-bordered">
   <tr><td align="left"><div> <label for="sPaddyType" style="display:table-cell">Paddy Type: </label>
      <span style="display:table-cell">
          <asp:DropDownList ID="sPaddyType" runat="server" 
           onselectedindexchanged="sPaddyType_SelectedIndexChanged" AutoPostBack="true">
          </asp:DropDownList>
      <%--<select id="sPaddyType" runat="server">
              <option>Rupali</option>
              <option>Mansuri</option>
              <option>Sonam</option>
              <option>Hybrid</option>
              <option>Other</option>
          </select>--%></span><span style="display:table-cell"><asp:Label ID="lblRBalance" runat="server"
              Text="Label"></asp:Label></span>
      </div></td></tr>
      <tr><td align="left"><div><label for="avgrate" style="display:table-cell">Rate (In Rs.): </label>
      <span style="display:table-cell">
      <input id="avgrate" name="avgrate" required runat="server" style="width:80px; text-align:right"/></span>
      </div></td></tr>
      <tr><td align="left"><div><label for="QIB" style="display:table-cell">Fresh Quantity (In Bags): </label>
      <span style="display:table-cell">
      <input id="QIB" name="QIB" required runat="server" style="width:80px; text-align:right"/></span>
      </div></td></tr>
      <tr><td align="left"><div><label for="moisture" style="display:table-cell">Moisture (In %, <=17,18): </label>
      <span style="display:table-cell">
      <input id="moisture" name="moisture" required runat="server" style="width:80px; text-align:right"/>%
      
      </span>
      </div></td></tr>
   <tr><td align="left"><div><label for="KhakhriPer" style="display:table-cell">Khakhri (In %, <=2): </label>
      <span style="display:table-cell">
      <input id="KhakhriPer" name="KhakhriPer" required runat="server" style="width:60px; text-align:right"/>%&nbsp;&nbsp;
      
      </span>
      <label for="KhakhriBag" style="display:table-cell">No. of Bags: </label>
      <span style="display:table-cell">
      <input id="KhakhriBag" name="KhakhriBag" required runat="server" style="width:80px; text-align:right"/>
      
      </span>
      </div></td></tr>
   </table>
   </div>
   <div class="col-md-6" style="position:inherit !important;">
   <table width="100%" class="table table-bordered">
      <tr><td align="left"><div><label for="MittiPer" style="display:table-cell">Mitti (In %, 0): </label>
      <span style="display:table-cell">
      <input id="MittiPer" name="MittiPer" required runat="server" style="width:60px; text-align:right"/>%&nbsp;&nbsp;
      
      </span>
      <label for="MittiBag" style="display:table-cell">No. of Bags: </label>
      <span style="display:table-cell">
      <input id="MittiBag" name="MittiBag" required runat="server" style="width:80px; text-align:right"/>
      
      </span>
      </div></td></tr>
      <tr><td align="left"><div><label for="DaagiPer" style="display:table-cell">Daagi (In %, 0): </label>
      <span style="display:table-cell">
      <input id="DaagiPer" name="DaagiPer" required runat="server" style="width:60px; text-align:right"/>%&nbsp;&nbsp;
      
      </span>
      <label for="DaagiBag" style="display:table-cell">No. of Bags: </label>
      <span style="display:table-cell">
      <input id="DaagiBag" name="DaagiBag" required runat="server" style="width:80px; text-align:right"/>
      
      </span>
      </div></td></tr>
      <tr><td align="left"><div><label for="MixRicePer" style="display:table-cell">Mix Rice (In %, 0): </label>
      <span style="display:table-cell">
      <input id="MixRicePer" name="MixRicePer" required runat="server" style="width:60px; text-align:right"/>%&nbsp;&nbsp;
      
      </span>
      <label for="MixRiceBag" style="display:table-cell">No. of Bags: </label>
      <span style="display:table-cell">
      <input id="MixRiceBag" name="MixRiceBag" required runat="server" style="width:80px; text-align:right"/>
      
      </span>
      </div></td></tr>
      <tr><td align="left"><div><label for="OtherPer" style="display:table-cell">Other (In %, 0): </label>
      <span style="display:table-cell">
      <input id="txtOthers" name="txtOthers" required runat="server" style="width:100px;"/> &nbsp;&nbsp;     
      </span>
      <span style="display:table-cell">
      <input id="OtherPer" name="OtherPer" required runat="server" style="width:30px; text-align:right"/>%&nbsp;&nbsp;      
      </span>
      <label for="OtherBag" style="display:table-cell;">No. of Bags: </label>
      <span style="display:table-cell">
      <input id="OtherBag" name="OtherBag" required runat="server" style="width:50px; text-align:right"/>
      
      </span>
      </div></td></tr>
      
      </table>
   </div>
   
   </div>  
   <br /> 
    <div class="row">
    <div class="col-md-6"><input type="submit" id="Submit1" value="Reset Data" runat="server" onserverclick="Submit1_ServerClick"/></div>
    <div class="col-md-6">
    <input type="submit" id="btnContinue" value="Click To Add" runat="server" onserverclick="btnContinue_ServerClick"/>
    </div>
        
    </div>
    <br /> 
    <div class="row table-responsive" id="prntContent">
    
        <asp:PlaceHolder ID="DBDataPlaceHolder" runat="server"></asp:PlaceHolder>
                   
    </div>
    <br />
    <div class="row">
      <input type="submit" id="btnSave" value="Click To Save" runat="server" onserverclick="btnSave_ServerClick"/>
    </div>
      
   
    
    </div>
    
  </div>
  </div>
    </form>
</body>
</html>
