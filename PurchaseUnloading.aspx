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
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css"/>

    <link href="CSS/Menu.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        /* Validation error style */
        .is-invalid{
            border-color:#dc2626 !important;
            box-shadow:0 0 0 3px rgba(220,38,38,0.15) !important;
        }
        .err-msg{
            color:#dc2626;
            font-size:12px;
            margin-top:3px;
            display:none;
        }
        .alert-custom{
            border-radius:8px;
            padding:12px 16px;
            margin-bottom:15px;
            font-size:13px;
            font-weight:600;
            display:none;
        }
        .alert-danger-custom{
            background:#fef2f2;
            border:1px solid #fca5a5;
            color:#b91c1c;
        }
    </style>
        
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

        /* ===== SHOW ERROR ===== */
        function showErr(fieldId, msg) {
            $("#" + fieldId).addClass("is-invalid");
            $("#err_" + fieldId).text(msg).show();
        }

        /* ===== CLEAR ERROR ===== */
        function clearErr(fieldId) {
            $("#" + fieldId).removeClass("is-invalid");
            $("#err_" + fieldId).hide();
        }

        /* ===== CLEAR ALL ERRORS ===== */
        function clearErrors() {
            $(".form-control, select, input, textarea").removeClass("is-invalid");
            $(".err-msg").hide();
            $("#topAlert").hide();
        }

        /* ===== SHOW TOP ALERT ===== */
        function showAlert(msg) {
            var el = $("#topAlert");
            el.removeClass("alert-danger-custom");
            el.addClass("alert-danger-custom");
            el.text(msg).show();
            $("html,body").animate({ scrollTop: 0 }, 300);
        }

        /* ===== VALIDATE ADD UNLOADING DATA ===== */
        function validateUnloading() {
            clearErrors();
            var valid = true;

            /* Date */
            if ($("#sdate").val() == "") {
                showErr("sdate", "Please fill this required field.");
                valid = false;
            }

            /* Truck No. */
            if ($("#truckno").val().trim() == "") {
                showErr("truckno", "Please fill this required field.");
                valid = false;
            }

            /* Kanta No. */
            if ($("#katano").val().trim() == "") {
                showErr("katano", "Please fill this required field.");
                valid = false;
            }

            /* Unloaded At */
            if ($("#UAt").val().trim() == "") {
                showErr("UAt", "Please fill this required field.");
                valid = false;
            }

            /* Tare Weight */
            var tw = $("#TWeight").val().trim();
            if (tw == "") {
                showErr("TWeight", "Please fill this required field.");
                valid = false;
            } else if (isNaN(tw) || parseFloat(tw) <= 0) {
                showErr("TWeight", "Tare weight should be greater than zero.");
                valid = false;
            }

            /* Party Name DropDownList */
            var partyVal = $("#<%= sPartyName.ClientID %>").val();
            if (partyVal == "" || partyVal == null) {
                showErr("<%= sPartyName.ClientID %>", "Please fill this required field.");
                valid = false;
            }

            /* Panel1 fields — sirf tab validate karo jab panel visible ho */
            if ($("#<%= Panel1.ClientID %>").is(":visible")) {

                if ($("#pName").val().trim() == "") {
                    showErr("pName", "Please fill this required field.");
                    valid = false;
                }

                var mob = $("#pMN").val().trim();
                if (mob == "") {
                    showErr("pMN", "Please fill this required field.");
                    valid = false;
                } else if (!/^\d{10}$/.test(mob)) {
                    showErr("pMN", "Please enter a valid 10 digit mobile no.");
                    valid = false;
                }
            }

            /* Bags */
            if ($("#PBags").val().trim() == "") {
                showErr("PBags", "Please fill this required field.");
                valid = false;
            }
            if ($("#PTBags").val().trim() == "") {
                showErr("PTBags", "Please fill this required field.");
                valid = false;
            }
            if ($("#JBags").val().trim() == "") {
                showErr("JBags", "Please fill this required field.");
                valid = false;
            }
            if ($("#JTBags").val().trim() == "") {
                showErr("JTBags", "Please fill this required field.");
                valid = false;
            }

            /* Sauda No. & Date */
            if ($("#SaudaNo").val().trim() == "") {
                showErr("SaudaNo", "Please fill this required field.");
                valid = false;
            }
            if ($("#SaudaDate").val().trim() == "") {
                showErr("SaudaDate", "Please fill this required field.");
                valid = false;
            }

            /* Avg Wt Per Bag */
            var qik = $("#QIK").val().trim();
            if (qik == "") {
                showErr("QIK", "Please fill this required field.");
                valid = false;
            } else if (isNaN(qik) || parseFloat(qik) <= 0) {
                showErr("QIK", "Value should be greater than zero.");
                valid = false;
            }

            /* CD */
            if ($("#CD").val().trim() == "") {
                showErr("CD", "Please fill this required field.");
                valid = false;
            }

            /* Freight */
            if ($("#TFreight").val().trim() == "") {
                showErr("TFreight", "Please fill this required field.");
                valid = false;
            }
            if ($("#Freight").val().trim() == "") {
                showErr("Freight", "Please fill this required field.");
                valid = false;
            }
            if ($("#PFreight").val().trim() == "") {
                showErr("PFreight", "Please fill this required field.");
                valid = false;
            }

            /* Advance / Brokerage */
            if ($("#Advance").val().trim() == "") {
                showErr("Advance", "Please fill this required field.");
                valid = false;
            }
            if ($("#brokerage").val().trim() == "") {
                showErr("brokerage", "Please fill this required field.");
                valid = false;
            }

            /* Paddy Type DropDownList */
            var paddyVal = $("#<%= sPaddyType.ClientID %>").val();
            if (paddyVal == "" || paddyVal == null) {
                showErr("<%= sPaddyType.ClientID %>", "Please fill this required field.");
                valid = false;
            }

            /* Rate */
            var rate = $("#avgrate").val().trim();
            if (rate == "") {
                showErr("avgrate", "Please fill this required field.");
                valid = false;
            } else if (isNaN(rate) || parseFloat(rate) <= 0) {
                showErr("avgrate", "Rate should be greater than zero.");
                valid = false;
            }

            /* Fresh Quantity */
            var qib = $("#QIB").val().trim();
            if (qib == "") {
                showErr("QIB", "Please fill this required field.");
                valid = false;
            } else if (isNaN(qib) || parseFloat(qib) <= 0) {
                showErr("QIB", "Quantity should be greater than zero.");
                valid = false;
            }

            /* Moisture */
            if ($("#moisture").val().trim() == "") {
                showErr("moisture", "Please fill this required field.");
                valid = false;
            }

            /* Khakhri */
            if ($("#KhakhriPer").val().trim() == "") {
                showErr("KhakhriPer", "Please fill this required field.");
                valid = false;
            }
            if ($("#KhakhriBag").val().trim() == "") {
                showErr("KhakhriBag", "Please fill this required field.");
                valid = false;
            }

            /* Mitti */
            if ($("#MittiPer").val().trim() == "") {
                showErr("MittiPer", "Please fill this required field.");
                valid = false;
            }
            if ($("#MittiBag").val().trim() == "") {
                showErr("MittiBag", "Please fill this required field.");
                valid = false;
            }

            /* Daagi */
            if ($("#DaagiPer").val().trim() == "") {
                showErr("DaagiPer", "Please fill this required field.");
                valid = false;
            }
            if ($("#DaagiBag").val().trim() == "") {
                showErr("DaagiBag", "Please fill this required field.");
                valid = false;
            }

            /* Mix Rice */
            if ($("#MixRicePer").val().trim() == "") {
                showErr("MixRicePer", "Please fill this required field.");
                valid = false;
            }
            if ($("#MixRiceBag").val().trim() == "") {
                showErr("MixRiceBag", "Please fill this required field.");
                valid = false;
            }

            /* Other */
            if ($("#txtOthers").val().trim() == "") {
                showErr("txtOthers", "Please fill this required field.");
                valid = false;
            }
            if ($("#OtherPer").val().trim() == "") {
                showErr("OtherPer", "Please fill this required field.");
                valid = false;
            }
            if ($("#OtherBag").val().trim() == "") {
                showErr("OtherBag", "Please fill this required field.");
                valid = false;
            }

            if (!valid) {
                showAlert("Please fill all required fields.");
            }

            return valid;
        }

        /* ===== VALIDATE SAVE — kam se kam ek data added hona chahiye ===== */
        function validateSaveClick() {
            clearErrors();

            var contentText = $.trim($("#prntContent").text());

            if (contentText == "" || contentText.indexOf("No Data Added") !== -1) {
                showAlert("Please enter at least one data!!");
                return false;
            }

            return true;
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

      <div id="topAlert" class="alert-custom alert-danger-custom"></div>
     
   <div class="row"><label for="sdate">Select Date: </label>
      <input id="sdate" name="sdate" runat="server" required/>
      <div class="err-msg" id="err_sdate"></div>
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
      <div class="err-msg" id="err_truckno"></div>
      </div></td></tr>
      <tr><td align="left" colspan="2"><div><label for="katano" style="display:table-cell">Kanta No.: </label>
      <span style="display:table-cell">
      <input id="katano" name="katano" required runat="server" style="text-align:right"/></span>
      <div class="err-msg" id="err_katano"></div>
      </div></td></tr>
       <tr><td colspan="2" align="left"><div><label for="UAt" style="display:table-cell">Unloaded at: </label>
      <span style="display:table-cell">
      <input id="UAt" name="UAt" type="text" runat="server" required/></span>
      <div class="err-msg" id="err_UAt"></div>
      </div></td></tr>
      <tr><td align="left" colspan="2"><div><label for="TWeight" style="display:table-cell">Tare Wt. (In KG): </label>
      <span style="display:table-cell">
      <input id="TWeight" name="TWeight" required runat="server" style="text-align:right"/></span>
      <div class="err-msg" id="err_TWeight"></div>
      </div></td></tr>
      <tr><td align="left" colspan="2"><div><label for="sPartyName" style="display:table-cell">Party Name: </label>
      <span style="display:table-cell">
      <asp:DropDownList ID="sPartyName" runat="server" 
              onselectedindexchanged="sPartyName_SelectedIndexChanged" AutoPostBack="true">
              
          </asp:DropDownList>
      </span>
      <div class="err-msg" id="err_<%= sPartyName.ClientID %>"></div>
          <asp:Panel ID="Panel1" runat="server">
        <table width="100%" class="table table-bordered">
        <tr><td align="left"><div>
        <label for="pName" style="display:table-cell">Party Name: </label>
        <span style="display:table-cell">
        <input id="pName" type="text" name="pName" runat="server"/>
        </span>
        <div class="err-msg" id="err_pName"></div>
        </div></td></tr>
        <tr>
        <td align="left"><div>
        <label for="pMN" style="display:table-cell;">Party Mobile No.: </label>
        <span style="display:table-cell">
        <input id="pMN" name="pMN" runat="server"/>
        </span>
        <div class="err-msg" id="err_pMN"></div>
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
      <div class="err-msg" id="err_PBags"></div>
      </div></td>
      <td align="left"><div><label for="PTBags" style="display:table-cell">Plastic Torn Bags: </label>
      <span style="display:table-cell">
      <input id="PTBags" name="PTBags" runat="server" style="width:50px !important;" required/></span>
      <div class="err-msg" id="err_PTBags"></div>
      </div></td>
      </tr>
      <tr><td align="left"><div><label for="JBags" style="display:table-cell">Jute Bags: </label>
      <span style="display:table-cell">
      <input id="JBags" name="JBags" runat="server" style="width:50px !important;" required/></span>
      <div class="err-msg" id="err_JBags"></div>
      </div></td>
      <td align="left"><div><label for="JTBags" style="display:table-cell">Jute Torn Bags: </label>
      <span style="display:table-cell">
      <input id="JTBags" name="JTBags" runat="server" style="width:50px !important;" required/></span>
      <div class="err-msg" id="err_JTBags"></div>
      </div></td>
      </tr>
      <tr><td align="left" colspan="2"><div><label for="SaudaNo" style="display:table-cell">Sauda No. & Date: </label>
      <span style="display:table-cell">
      <input id="SaudaNo" name="SaudaNo" runat="server" style="width:150px !important;" required/></span>
      
      <span style="display:table-cell">
      <input id="SaudaDate" name="SaudaDate" runat="server" required/></span>
      <div class="err-msg" id="err_SaudaNo"></div>
      <div class="err-msg" id="err_SaudaDate"></div>
      </div></td>
      </tr>
      </table>
      </div>
      <div class="col-md-6" style="position:inherit !important;">
      <table width="100%" class="table table-bordered">
      
      <tr><td align="left"><div><label for="QIK" style="display:table-cell">Avg. Wt. Per Bag (In KG): </label>
      <span style="display:table-cell">
      <input id="QIK" name="QIK" required runat="server" style="text-align:right"/></span>
      <div class="err-msg" id="err_QIK"></div>
      </div></td></tr>
      
      <tr><td align="left"><div> <label for="CD" style="display:table-cell">CD (In %) </label>
      <span style="display:table-cell">
      <input id="CD" name="CD" required runat="server" style="width:80px; text-align:right"/>%
      </span>
      <div class="err-msg" id="err_CD"></div>
      </div></td></tr>
      <tr><td align="left">
      <div> <label for="TFreight" style="display:table-cell">Total Freight </label>
      <span style="display:table-cell">
      <input id="TFreight" name="TFreight" required runat="server" style="text-align:right"/>
      </span>
      <div class="err-msg" id="err_TFreight"></div>
      </div>
      <div> <label for="Freight" style="display:table-cell">Freight (Own) </label>
      <span style="display:table-cell">
      <input id="Freight" name="Freight" required runat="server" style="text-align:right"/>
      </span>
      <div class="err-msg" id="err_Freight"></div>
      </div>
      <div> <label for="PFreight" style="display:table-cell">Freight (Paid By Party) </label>
      <span style="display:table-cell">
      <input id="PFreight" name="PFreight" required runat="server" style="text-align:right"/>
      </span>
      <div class="err-msg" id="err_PFreight"></div>
      </div>
      
      </td></tr>
      
      <tr><td align="left"><div> <label for="Advance" style="display:table-cell">Advance </label>
      <span style="display:table-cell">
      <input id="Advance" name="Advance" required runat="server" style="text-align:right"/>
      </span>
      <div class="err-msg" id="err_Advance"></div>
      </div></td></tr>
      <tr><td align="left"><div> <label for="brokerage" style="display:table-cell">Brokerage (of Party) </label>
      <span style="display:table-cell">
      <input id="brokerage" name="brokerage" required runat="server" style="text-align:right"/>
      </span>
      <div class="err-msg" id="err_brokerage"></div>
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
      <div class="err-msg" id="err_<%= sPaddyType.ClientID %>"></div>
      </div></td></tr>
      <tr><td align="left"><div><label for="avgrate" style="display:table-cell">Rate (In Rs.): </label>
      <span style="display:table-cell">
      <input id="avgrate" name="avgrate" required runat="server" style="width:80px; text-align:right"/></span>
      <div class="err-msg" id="err_avgrate"></div>
      </div></td></tr>
      <tr><td align="left"><div><label for="QIB" style="display:table-cell">Fresh Quantity (In Bags): </label>
      <span style="display:table-cell">
      <input id="QIB" name="QIB" required runat="server" style="width:80px; text-align:right"/></span>
      <div class="err-msg" id="err_QIB"></div>
      </div></td></tr>
      <tr><td align="left"><div><label for="moisture" style="display:table-cell">Moisture (In %, <=17,18): </label>
      <span style="display:table-cell">
      <input id="moisture" name="moisture" required runat="server" style="width:80px; text-align:right"/>%
      
      </span>
      <div class="err-msg" id="err_moisture"></div>
      </div></td></tr>
   <tr><td align="left"><div><label for="KhakhriPer" style="display:table-cell">Khakhri (In %, <=2): </label>
      <span style="display:table-cell">
      <input id="KhakhriPer" name="KhakhriPer" required runat="server" style="width:60px; text-align:right"/>%&nbsp;&nbsp;
      
      </span>
      <label for="KhakhriBag" style="display:table-cell">No. of Bags: </label>
      <span style="display:table-cell">
      <input id="KhakhriBag" name="KhakhriBag" required runat="server" style="width:80px; text-align:right"/>
      
      </span>
      <div class="err-msg" id="err_KhakhriPer"></div>
      <div class="err-msg" id="err_KhakhriBag"></div>
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
      <div class="err-msg" id="err_MittiPer"></div>
      <div class="err-msg" id="err_MittiBag"></div>
      </div></td></tr>
      <tr><td align="left"><div><label for="DaagiPer" style="display:table-cell">Daagi (In %, 0): </label>
      <span style="display:table-cell">
      <input id="DaagiPer" name="DaagiPer" required runat="server" style="width:60px; text-align:right"/>%&nbsp;&nbsp;
      
      </span>
      <label for="DaagiBag" style="display:table-cell">No. of Bags: </label>
      <span style="display:table-cell">
      <input id="DaagiBag" name="DaagiBag" required runat="server" style="width:80px; text-align:right"/>
      
      </span>
      <div class="err-msg" id="err_DaagiPer"></div>
      <div class="err-msg" id="err_DaagiBag"></div>
      </div></td></tr>
      <tr><td align="left"><div><label for="MixRicePer" style="display:table-cell">Mix Rice (In %, 0): </label>
      <span style="display:table-cell">
      <input id="MixRicePer" name="MixRicePer" required runat="server" style="width:60px; text-align:right"/>%&nbsp;&nbsp;
      
      </span>
      <label for="MixRiceBag" style="display:table-cell">No. of Bags: </label>
      <span style="display:table-cell">
      <input id="MixRiceBag" name="MixRiceBag" required runat="server" style="width:80px; text-align:right"/>
      
      </span>
      <div class="err-msg" id="err_MixRicePer"></div>
      <div class="err-msg" id="err_MixRiceBag"></div>
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
      <div class="err-msg" id="err_txtOthers"></div>
      <div class="err-msg" id="err_OtherPer"></div>
      <div class="err-msg" id="err_OtherBag"></div>
      </div></td></tr>
      
      </table>
   </div>
   
   </div>  
   <br /> 
    <div class="row">
    <div class="col-md-6"><input type="submit" id="Submit1" value="Reset Data" runat="server" onserverclick="Submit1_ServerClick"/></div>
    <div class="col-md-6">
    <input type="submit" id="btnContinue" value="Click To Add" runat="server" onserverclick="btnContinue_ServerClick" onclick="return validateUnloading();"/>
    </div>
        
    </div>
    <br /> 
    <div class="row table-responsive" id="prntContent">
    
        <asp:PlaceHolder ID="DBDataPlaceHolder" runat="server"></asp:PlaceHolder>
                   
    </div>
    <br />
    <div class="row">
      <input type="submit" id="btnSave" value="Click To Save" runat="server" onserverclick="btnSave_ServerClick" onclick="return validateSaveClick();"/>
    </div>
      
   
    
    </div>
    
  </div>
  </div>
    </form>
</body>
</html>