<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SalePayment.aspx.cs" Inherits="SalePayment" %>
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
    <script type="text/javascript">
        $(function () {
            if ($('[id*=paymentmode]').val() == 'Online') {
                $('[id*=Panel2]').show();
                $('[id*=Panel3]').show();
                $('[id*=lblTransaction]').html('Transaction ID: ');
            }
            else if ($('[id*=paymentmode]').val() == 'By Cheque') {
                $('[id*=Panel2]').show();
                $('[id*=Panel3]').hide();
                $('[id*=lblTransaction]').html('Cheque No. & Date: ');
            }
            else {
                $('[id*=Panel2]').show();
                $('[id*=Panel3]').hide();
                $('[id*=lblTransaction]').html('Receiver Name & Mobile No.: ');
            }
            $('[id*=paymentmode]').change(function () {
                if ($('[id*=paymentmode]').val() == 'Online') {
                    $('[id*=Panel2]').show();
                    $('[id*=Panel3]').show();
                    $('[id*=lblTransaction]').html('Transaction ID: ');
                }
                else if ($('[id*=paymentmode]').val() == 'By Cheque') {
                    $('[id*=Panel2]').show();
                    $('[id*=Panel3]').hide();
                    $('[id*=lblTransaction]').html('Cheque No. & Date: ');
                }
                else {
                    $('[id*=Panel2]').show();
                    $('[id*=Panel3]').hide();
                    $('[id*=lblTransaction]').html('Receiver Name & Mobile No.: ');
                }
            });
        });
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
  <div id='center' class="main center">
    <div class="mainInner">
    
       <h2><span style="background-color:Yellow;">Rashmi Rice Mills Private Limited</span>
      <br />Payment By Party</h2>
      <div class="row" style="text-align:right !important;">
      <span style="font-weight:bold; color:Maroon;">Welcome Operator</span><br />
      <span style="font-weight:bold; color:Black;">
      Balance: 
          <asp:Label ID="lblOSB" runat="server" Text=""></asp:Label> (In Rs.)</span>
      </div>
      
  <div class="row">
  <div class="col-25">
        <label for="sdate">Payment Date</label>
      </div>
      <div class="col-75">
        <input id="sdate" name="sdate" runat="server" required/>&nbsp;&nbsp;
          
      </div></div>
      <div class="row">
  <div class="col-25">
        <label for="pvNo">Manual Voucher No. (If any)</label>
      </div>
      <div class="col-75">
        <input id="pvNo" name="pvNo" runat="server" type="text"/>
          
      </div></div>
      <div class="row">
      <div class="col-25">
        <label for="ddlParty">Select Party Name</label>
      </div>
      <div class="col-75">
          <asp:DropDownList ID="ddlParty" runat="server">
              
          </asp:DropDownList>&nbsp;&nbsp;<asp:LinkButton ID="LinkButton1" runat="server" 
              onclick="LinkButton1_Click">Payment List</asp:LinkButton>
      </div>
      </div>
   
        
      <div class="row">
         <div class="col-25">
        <label for="amountpaid">Amount Paid</label>
      </div>
      <div class="col-75">
        <input id="amountpaid" name="amountpaid" runat="server" required/>
          
      </div></div>
      
      <div class="row">
         <div class="col-25">
        <label for="paymentmode">Payment Mode</label>
      </div>
      <div class="col-75">
        <select id="paymentmode" runat="server">
              <option>By Cash</option>
              <option>By Cheque</option>
              <option>Online</option>
          </select>
          
      </div></div>
      
      
          <asp:Panel ID="Panel2" runat="server" style="display:none;">
        <table width="100%">
        <tr><td align="left" colspan="2"><div> <label id="lblTransaction" for="transaction" style="display:table-cell">
          </label>
      <span style="display:table-cell">
          <input id="transaction" type="text" runat="server" required />
      </span>
      </div></td></tr>
        </table></asp:Panel>
         <br /> 
    <div class="row">
    <div class="col-md-6"></div>
    <div class="col-md-6">
    <input type="submit" id="btnContinue" value="Click To Add" runat="server" onserverclick="btnContinue_ServerClick"/>
    </div>
        
    </div><br />
       <div class="row" id="prntContent">
    
        <asp:PlaceHolder ID="DBDataPlaceHolder" runat="server"></asp:PlaceHolder>
                   
    </div>
    <br />
    <div class="row">
      <input type="submit" id="btnSave" value="Submit" runat="server" onserverclick="btnSave_ServerClick"/>
    </div>
      
    <br />
   </div>
    </div>
    </div>
    
    </form>
</body>
</html>
