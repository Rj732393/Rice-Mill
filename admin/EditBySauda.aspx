<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditBySauda.aspx.cs" Inherits="admin_EditBySauda" %>
<%@ Register src="../Includes/adminmenu.ascx" tagname="WebUserControl1" tagprefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>::Edit By Sauda No - Rice Mills Admin::</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css"/>
    <link href="../CSS/Menu.css" rel="stylesheet" type="text/css" />
    <style>
        .edit-panel { background:#fff; border:1px solid #ddd; border-radius:6px; padding:20px; margin-top:18px; }
        .edit-panel h3 { color:#8B0000; border-bottom:2px solid #f0c040; padding-bottom:8px; margin-bottom:16px; }
        .alert-msg { padding:10px; border-radius:4px; margin-bottom:12px; font-weight:bold; }
        .alert-success { background:#d4edda; color:#155724; border:1px solid #c3e6cb; }
        .alert-error { background:#f8d7da; color:#721c24; border:1px solid #f5c6cb; }
        .section-title { background:#8B0000; color:#fff; padding:8px 14px; border-radius:4px; margin:16px 0 10px 0; font-size:14px; font-weight:bold; }
        .field-group { display:flex; flex-wrap:wrap; gap:12px; margin-bottom:10px; }
        .field-box { flex:0 0 calc(33% - 12px); }
        .field-box label { font-weight:bold; font-size:12px; color:#555; display:block; margin-bottom:4px; }
        .field-box input { width:100%; padding:5px 8px; border:1px solid #aaa; border-radius:3px; box-sizing:border-box; font-size:13px; }
        .field-box input[readonly] { background:#f5f5f5; color:#999; border-color:#ddd; }
        .paddy-table { width:100%; border-collapse:collapse; font-size:13px; margin-bottom:10px; }
        .paddy-table th { background:#8B0000; color:#fff; padding:8px; text-align:center; }
        .paddy-table td { padding:7px 8px; border:1px solid #ddd; text-align:center; }
        .paddy-table tr:nth-child(even) { background:#f9f9f9; }
        .paddy-table input { width:100%; padding:4px 6px; border:1px solid #aaa; border-radius:3px; text-align:right; box-sizing:border-box; }
        .btn-search { background:#8B0000; color:#fff; border:none; padding:7px 24px; border-radius:4px; font-size:14px; cursor:pointer; }
        .btn-save { background:#27ae60; color:#fff; border:none; padding:8px 26px; border-radius:4px; font-size:14px; cursor:pointer; margin-right:8px; }
        .btn-cancel { background:#c0392b; color:#fff; border:none; padding:8px 20px; border-radius:4px; font-size:14px; cursor:pointer; }
        .purchase-entry-block { border:1px solid #ddd; border-radius:5px; padding:12px; margin-bottom:14px; background:#fafafa; }
        .purchase-entry-title { font-weight:bold; color:#1a6496; margin-bottom:8px; font-size:13px; }
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
      <div id="center" class="main center">
        <div class="mainInner">
          <h2>
            <span style="background-color:Yellow;">Rashmi Rice Mills Private Limited</span>
            <br />Edit By Sauda Number
          </h2>
          <div class="row" style="text-align:right !important;">
            <span style="font-weight:bold; color:Maroon;">Welcome Admin</span>
          </div>

          <!-- Search Panel -->
          <div class="edit-panel">
            <h3><i class="fas fa-search"></i> Search by Sauda Number</h3>
            <div class="row">
              <div class="col-sm-5">
                <label style="font-weight:bold;color:#555;">Sauda Number</label>
                <asp:TextBox ID="txtSaudaNo" runat="server" CssClass="form-control"
                    placeholder="Sauda No., MNo (Supplier Ref.) ya ID"></asp:TextBox>
              </div>
              <div class="col-sm-3" style="padding-top:24px;">
                <input type="submit" id="btnSearch" runat="server" value="Search"
                    onserverclick="btnSearch_ServerClick" class="btn-search" />
              </div>
            </div>
          </div>

          <!-- Message -->
          <asp:PlaceHolder ID="phMessage" runat="server"></asp:PlaceHolder>

          <!-- Edit Panel -->
          <div class="edit-panel" id="editPanel" runat="server" visible="false">
            <h3><i class="fas fa-edit"></i> Edit Sauda &amp; Linked Entries</h3>

            <!-- Sauda Info -->
            <div class="section-title">📋 Sauda Details — <asp:Label ID="lblSaudaNo" runat="server"></asp:Label></div>
            <div class="field-group">
                <div class="field-box">
                    <label>Sauda ID (Read Only)</label>
                    <input type="text" id="hdnSaudaID" runat="server" readonly />
                </div>
                <div class="field-box">
                    <label>Date</label>
                    <input type="text" id="saudaDate" runat="server" />
                </div>
                <div class="field-box">
                    <label>Party Name</label>
                    <input type="text" id="saudaParty" runat="server" />
                </div>
                <div class="field-box">
                    <label>Broker Name</label>
                    <input type="text" id="saudaBroker" runat="server" />
                </div>
                <div class="field-box">
                    <label>MNo (Supplier Ref.)</label>
                    <input type="text" id="saudaMNo" runat="server" />
                </div>
            </div>

            <!-- Paddy Rates in Sauda -->
            <div class="section-title">🌾 Paddy Rates &amp; Weights (Sauda)</div>
            <table class="paddy-table">
                <thead>
                    <tr>
                        <th>Paddy Type</th>
                        <th>Weight (KG)</th>
                        <th>Rate (₹)</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td><b>Rupali</b></td>
                        <td><input type="text" id="sRupaliWt" runat="server" /></td>
                        <td><input type="text" id="sRupaliRate" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><b>Mansuri</b></td>
                        <td><input type="text" id="sMansuriWt" runat="server" /></td>
                        <td><input type="text" id="sMansuriRate" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><b>Sonam</b></td>
                        <td><input type="text" id="sSonamWt" runat="server" /></td>
                        <td><input type="text" id="sSonamRate" runat="server" /></td>
                    </tr>
                    <tr>
                        <td><b>Hybrid</b></td>
                        <td><input type="text" id="sHybridWt" runat="server" /></td>
                        <td><input type="text" id="sHybridRate" runat="server" /></td>
                    </tr>
                </tbody>
            </table>

            <!-- Linked Purchase Entries -->
            <div class="section-title">🚛 Linked Purchase Entries (Unloading)</div>
            <asp:PlaceHolder ID="phPurchaseEntries" runat="server"></asp:PlaceHolder>

            <br />
            <input type="submit" id="btnSave" runat="server" value="Save All Changes"
                onserverclick="btnSave_ServerClick" class="btn-save" />
            <input type="submit" id="btnCancel" runat="server" value="Cancel"
                onserverclick="btnCancel_ServerClick" class="btn-cancel" />
          </div>

        </div>
      </div>
    </div>
    </form>
</body>
</html>