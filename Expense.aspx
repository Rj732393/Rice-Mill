<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Expense.aspx.cs" Inherits="Expense" %>
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
      <br />Daily Expense</h2>
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
        <label for="sExpenseType">Expense Type</label>
      </div>
      <div class="col-75">
          <asp:DropDownList ID="ddlExpenseType" Width="250px" runat="server">
          <asp:ListItem>Freight Exp</asp:ListItem>
<asp:ListItem>Truck</asp:ListItem>
<asp:ListItem>1839 Bank Interest</asp:ListItem>
<asp:ListItem>Abhijeet Ji Cmera Exp</asp:ListItem>
<asp:ListItem>Accounting Charge</asp:ListItem>
<asp:ListItem>Air Exp (Drayar)</asp:ListItem>
<asp:ListItem>Annual Charge</asp:ListItem>
<asp:ListItem>Arya Collecteral Warehouseing</asp:ListItem>
<asp:ListItem>ARYADHAN RENT</asp:ListItem>
<asp:ListItem>Bank Charge</asp:ListItem>
<asp:ListItem>Bank Interest</asp:ListItem>
<asp:ListItem>Begursarai Exp</asp:ListItem>
<asp:ListItem>Birendar Ji Ca</asp:ListItem>
<asp:ListItem>Birendar Sir (Interest)</asp:ListItem>
<asp:ListItem>Broker Comission</asp:ListItem>
<asp:ListItem>BY INST 5128 : CTO337-1 DAY  LAT</asp:ListItem>
<asp:ListItem>Ca Murari</asp:ListItem>
<asp:ListItem>Cd@2% (Sale)</asp:ListItem>
<asp:ListItem>Cibil Contrucation Exp</asp:ListItem>
<asp:ListItem>Civil Contruction</asp:ListItem>
<asp:ListItem>Cleaining Exp</asp:ListItem>
<asp:ListItem>Cma Report Exp</asp:ListItem>
<asp:ListItem>Commission Agent</asp:ListItem>
<asp:ListItem>Commission Broker</asp:ListItem>
<asp:ListItem>Decument Charge</asp:ListItem>
<asp:ListItem>Devalepment Exp</asp:ListItem>
<asp:ListItem>Devanand Singh (Rent)</asp:ListItem>
<asp:ListItem>Electric Bill</asp:ListItem>
<asp:ListItem>FOODING EXP</asp:ListItem>
<asp:ListItem>Freight Genral Exp</asp:ListItem>
<asp:ListItem>Gas Exp (Truck)</asp:ListItem>
<asp:ListItem>GOLU KUMAR</asp:ListItem>
<asp:ListItem>Gst Tax</asp:ListItem>
<asp:ListItem>Hira Lal Ji (Broker)</asp:ListItem>
<asp:ListItem>House Hold Exp</asp:ListItem>
<asp:ListItem>IDBI BANK CHARGE</asp:ListItem>
<asp:ListItem>IDBI BANK INTEREST</asp:ListItem>
<asp:ListItem>Indal Ji (Salary)</asp:ListItem>
<asp:ListItem>INNOVATIVE SERVICE</asp:ListItem>
<asp:ListItem>Instllemnet Loan</asp:ListItem>
<asp:ListItem>Insurence Charge (Idbi Bank)</asp:ListItem>
<asp:ListItem>INSURENCE EXP</asp:ListItem>
<asp:ListItem>Interest (Birendar Ji )</asp:ListItem>
<asp:ListItem>Interest + Bc (Sanjay Ji)</asp:ListItem>
<asp:ListItem>Interest Charge</asp:ListItem>
<asp:ListItem>Intrest Charge</asp:ListItem>
<asp:ListItem>Laxmi Network</asp:ListItem>
<asp:ListItem>Leas Rent (Satendar Kumar )</asp:ListItem>
<asp:ListItem>Lebour Cost</asp:ListItem>
<asp:ListItem>Lebour Cost</asp:ListItem>
<asp:ListItem>Legal Exp</asp:ListItem>
<asp:ListItem>Lis Assigment Charge</asp:ListItem>
<asp:ListItem>Loding & Unloding Charge</asp:ListItem>
<asp:ListItem>Mandir Contrator</asp:ListItem>
<asp:ListItem>Maruti Nandan Milling</asp:ListItem>
<asp:ListItem>Maurya Motor Pvt.Ltd</asp:ListItem>
<asp:ListItem>Md Zahid Husain</asp:ListItem>
<asp:ListItem>Mill Exp</asp:ListItem>
<asp:ListItem>Mill Parts Exp</asp:ListItem>
<asp:ListItem>Milltech Machineary</asp:ListItem>
<asp:ListItem>MISC.EXP</asp:ListItem>
<asp:ListItem>Misc.Exp</asp:ListItem>
<asp:ListItem>Missing Exp</asp:ListItem>
<asp:ListItem>Mobile Recharge Exp</asp:ListItem>
<asp:ListItem>MUKESH JI MISTRI (PERSONAL)</asp:ListItem>
<asp:ListItem>Mukesh Mistri</asp:ListItem>
<asp:ListItem>Munindra Kumar (Eta )</asp:ListItem>
<asp:ListItem>NARAYANI PUMPUS PVT.LTD</asp:ListItem>
<asp:ListItem>Net Charge</asp:ListItem>
<asp:ListItem>New Truck</asp:ListItem>
<asp:ListItem>Oil Expensese</asp:ListItem>
<asp:ListItem>OTHER EXP</asp:ListItem>
<asp:ListItem>Personal Exp (Office)</asp:ListItem>
<asp:ListItem>Pest Control</asp:ListItem>
<asp:ListItem>PETROL EXP</asp:ListItem>
<asp:ListItem>Prabha Software Exp</asp:ListItem>
<asp:ListItem>Pravin Prasad Gupta</asp:ListItem>
<asp:ListItem>Preeti Yadav</asp:ListItem>
<asp:ListItem>Printing & Statinoery Exp</asp:ListItem>
<asp:ListItem>Processing Fee</asp:ListItem>
<asp:ListItem>PUNJAB NATIONAL BANK</asp:ListItem>
<asp:ListItem>R.K Singh Pvc . (Pipe)</asp:ListItem>
<asp:ListItem>R.S Traders (Fatwah)</asp:ListItem>
<asp:ListItem>R/off</asp:ListItem>
<asp:ListItem>Raj Kishor Singh (Rent)</asp:ListItem>
<asp:ListItem>Raj Kumar (Prabha Salary Ka Hai )</asp:ListItem>
<asp:ListItem>Rambabu Ji Exp</asp:ListItem>
<asp:ListItem>Repair & Maintenance</asp:ListItem>
<asp:ListItem>Repair & Parts</asp:ListItem>
<asp:ListItem>Salary to Staff ( Mill)</asp:ListItem>
<asp:ListItem>Sale Freght</asp:ListItem>
<asp:ListItem>Sanjay Ji  Broker (Mh)</asp:ListItem>
<asp:ListItem>Sanjay Ji (Poldar)</asp:ListItem>
<asp:ListItem>Service Charge  (Md Sazid)</asp:ListItem>
<asp:ListItem>Service Charge Exp (Mills)</asp:ListItem>
<asp:ListItem>Sohan Kumar</asp:ListItem>
<asp:ListItem>SRI RAM TRADING EXP</asp:ListItem>
<asp:ListItem>Staff Salary (Mills)</asp:ListItem>
<asp:ListItem>Staff Salary (Office)</asp:ListItem>
<asp:ListItem>Stamps Paper</asp:ListItem>
<asp:ListItem>Tds Exp</asp:ListItem>
<asp:ListItem>The New India Assurance</asp:ListItem>
<asp:ListItem>Toll Tax Exp</asp:ListItem>
<asp:ListItem>Udan Capital Exp</asp:ListItem>
<asp:ListItem>Wastege Item</asp:ListItem>
<asp:ListItem>Welfare Exp</asp:ListItem>

          </asp:DropDownList>
      </div>
    </div>
    
    <div class="row">
    
      <div class="col-25">
        <label for="EAmount">Amount (In Rs.)</label>
      </div>
      <div class="col-75">
          <input id="EAmount" name="EAmount" required runat="server" style="text-align:right"/>
      </div>
    </div>
    
     
    <div class="row">
    
      <div class="col-25">
        <label for="ERemarks">Remarks (If any)</label>
      </div>
      <div class="col-75">
          <textarea id="ERemarks" runat="server" cols="40" rows="5" required></textarea>
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
