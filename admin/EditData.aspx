<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditData.aspx.cs" Inherits="admin_EditData" %>
<%@ Register src="../Includes/adminmenu.ascx" tagname="WebUserControl1" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>::Edit Data - Rice Mills Admin::</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css"/>
    <link href="../CSS/Menu.css" rel="stylesheet" type="text/css" />
    <style>
        .edit-panel { background:#fff; border:1px solid #ddd; border-radius:6px; padding:20px; margin-top:18px; }
        .edit-panel h3 { color:#8B0000; border-bottom:2px solid #f0c040; padding-bottom:8px; margin-bottom:16px; }
        .tbl-edit td, .tbl-edit th { vertical-align:middle !important; font-size:13px; }
        .btn-edit-row { background:#1a6496; color:#fff; border:none; border-radius:3px; padding:3px 10px; cursor:pointer; }
        .btn-edit-row:hover { background:#2980b9; }
        .alert-msg { padding:10px; border-radius:4px; margin-bottom:12px; font-weight:bold; }
        .alert-success { background:#d4edda; color:#155724; border:1px solid #c3e6cb; }
        .alert-error   { background:#f8d7da; color:#721c24; border:1px solid #f5c6cb; }
        select.form-control { max-width:340px; }
        .view-cell { max-width:180px; overflow:hidden; text-overflow:ellipsis; white-space:nowrap; }
    </style>
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
          <h2>
            <span style="background-color:Yellow;">Rashmi Rice Mills Private Limited</span>
            <br />Edit Database Records
          </h2>
          <div class="row" style="text-align:right !important;">
            <span style="font-weight:bold; color:Maroon;">Welcome Admin</span>
          </div>

          <!-- Table Selector -->
          <div class="edit-panel">
            <h3><i class="fas fa-database"></i> Select Table to Edit</h3>
            <div class="row">
              <div class="col-sm-4">
                <label style="font-weight:bold;color:#555;">Table Name</label>
                <asp:DropDownList ID="ddlTable" runat="server" CssClass="form-control">
                    <asp:ListItem Value="">-- Select Table --</asp:ListItem>
                    <asp:ListItem Value="prabha.Purchase_Party_Info">Purchase Party Info</asp:ListItem>
                    <asp:ListItem Value="prabha.Purchase_Sauda_Info">Purchase Sauda Info</asp:ListItem>
                    <asp:ListItem Value="prabha.Purchase_Master_Data">Purchase Master Data</asp:ListItem>
                    <asp:ListItem Value="prabha.Purchase_Payment_Info">Purchase Payment Info</asp:ListItem>
                    <asp:ListItem Value="prabha.Sale_Sauda_Master">Sale Sauda Master</asp:ListItem>
                    <asp:ListItem Value="prabha.Sale_Master_Data">Sale Master Data</asp:ListItem>
                    <asp:ListItem Value="prabha.Sale_Payment_Info">Sale Payment Info</asp:ListItem>
                    <asp:ListItem Value="prabha.PaddyProcessing">Paddy Processing</asp:ListItem>
                    <asp:ListItem Value="prabha.PaddyStock">Paddy Stock</asp:ListItem>
                    <asp:ListItem Value="prabha.RiceStock">Rice Stock</asp:ListItem>
                    <asp:ListItem Value="prabha.Expense_Info">Expense Info</asp:ListItem>
                    <asp:ListItem Value="prabha.SalePurchaseExpense">Sale Purchase Expense</asp:ListItem>
                    <asp:ListItem Value="prabha.UserInfo">User Info</asp:ListItem>
                </asp:DropDownList>
              </div>
              <div class="col-sm-3" style="padding-top:24px;">
                <input type="submit" id="btnLoad" runat="server" value="Load Data"
                    onserverclick="btnLoad_ServerClick"
                    style="background:#8B0000;color:#fff;border:none;padding:6px 22px;border-radius:4px;font-size:14px;cursor:pointer;" />
              </div>
            </div>
          </div>

          <!-- Message Area -->
          <asp:PlaceHolder ID="phMessage" runat="server"></asp:PlaceHolder>

          <!-- Data Table -->
          <div class="edit-panel" id="dataPanel" runat="server" visible="false">
            <h3><i class="fas fa-table"></i> <asp:Label ID="lblTableTitle" runat="server" Text=""></asp:Label></h3>
            <div class="row table-responsive">
                <asp:PlaceHolder ID="phTable" runat="server"></asp:PlaceHolder>
            </div>
          </div>

          <!-- Edit Form -->
          <div class="edit-panel" id="editFormPanel" runat="server" visible="false">
            <h3><i class="fas fa-edit"></i> Edit Record &nbsp;
                <small><asp:Label ID="lblEditID" runat="server" Text="" style="color:#888;font-size:13px;"></asp:Label></small>
            </h3>
            <div class="row">
                <asp:PlaceHolder ID="phEditForm" runat="server"></asp:PlaceHolder>
            </div>
            <br />
            <input type="hidden" id="hdnEditID" runat="server" />
            <input type="hidden" id="hdnTableName" runat="server" />
            <input type="submit" id="btnSave" runat="server" value="Save Changes"
                onserverclick="btnSave_ServerClick"
                style="background:#27ae60;color:#fff;border:none;padding:7px 22px;border-radius:4px;font-size:14px;cursor:pointer;" />
            &nbsp;
            <input type="submit" id="btnCancelEdit" runat="server" value="Cancel"
                onserverclick="btnCancelEdit_ServerClick"
                style="background:#c0392b;color:#fff;border:none;padding:7px 18px;border-radius:4px;font-size:14px;cursor:pointer;" />
          </div>

        </div>
      </div>
    </div>
    </form>
</body>
</html>