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
        body { font-family: Arial, sans-serif; background: #f4f4f4; }
        .page-title { color: #8B0000; font-size: 22px; font-weight: bold; margin-bottom: 6px; }
        .welcome-bar { text-align:right; font-weight:bold; color:Maroon; margin-bottom:10px; }
        .card { background:#fff; border:1px solid #ddd; border-radius:6px; padding:20px; margin-top:16px; box-shadow:0 1px 4px rgba(0,0,0,0.07); }
        .card h3 { color:#8B0000; border-bottom:2px solid #f0c040; padding-bottom:8px; margin-bottom:16px; font-size:16px; }
        .alert-msg { padding:10px 14px; border-radius:4px; margin-bottom:12px; font-weight:bold; font-size:13px; }
        .alert-success { background:#d4edda; color:#155724; border:1px solid #c3e6cb; }
        .alert-error   { background:#f8d7da; color:#721c24; border:1px solid #f5c6cb; }
        .section-bar { background:#8B0000; color:#fff; padding:7px 14px; border-radius:4px; margin:18px 0 10px 0; font-size:13px; font-weight:bold; }
        .field-row { display:flex; flex-wrap:wrap; gap:12px; margin-bottom:8px; }
        .field-box { flex: 0 0 calc(33% - 12px); min-width:160px; }
        .field-box label { font-weight:bold; font-size:12px; color:#555; display:block; margin-bottom:3px; }
        .field-box input { width:100%; padding:5px 8px; border:1px solid #aaa; border-radius:3px; box-sizing:border-box; font-size:13px; }
        .field-box input[readonly] { background:#f5f5f5; color:#888; border-color:#ddd; cursor:not-allowed; }
        .rate-table { width:100%; border-collapse:collapse; font-size:13px; margin-bottom:10px; }
        .rate-table th { background:#8B0000; color:#fff; padding:8px 10px; text-align:center; }
        .rate-table td { padding:7px 8px; border:1px solid #ddd; text-align:center; }
        .rate-table tr:nth-child(even) { background:#f9f9f9; }
        .rate-table input { width:110px; padding:4px 6px; border:1px solid #aaa; border-radius:3px; text-align:right; }
        .linked-block { border:1px solid #c8dff5; border-radius:5px; padding:12px; margin-bottom:14px; background:#f0f7ff; }
        .linked-title { font-weight:bold; color:#1a6496; margin-bottom:8px; font-size:13px; }
        .item-table { width:100%; border-collapse:collapse; font-size:12px; }
        .item-table th { background:#1a6496; color:#fff; padding:6px 8px; }
        .item-table td { padding:6px 8px; border:1px solid #ccc; text-align:center; }
        .item-table input { width:90px; padding:3px 5px; border:1px solid #aaa; border-radius:3px; text-align:right; }
        .btn-search { background:#8B0000; color:#fff; border:none; padding:7px 24px; border-radius:4px; font-size:14px; cursor:pointer; }
        .btn-save   { background:#27ae60; color:#fff; border:none; padding:8px 26px; border-radius:4px; font-size:14px; cursor:pointer; margin-right:8px; }
        .btn-cancel { background:#c0392b; color:#fff; border:none; padding:8px 20px; border-radius:4px; font-size:14px; cursor:pointer; }
        .tab-btns { margin-bottom:0; }
        .tab-btns button { padding:8px 22px; border:1px solid #8B0000; background:#fff; color:#8B0000; cursor:pointer; font-size:13px; border-radius:4px 4px 0 0; margin-right:4px; }
        .tab-btns button.active { background:#8B0000; color:#fff; font-weight:bold; }
        .info-note { background:#fff8e1; border:1px solid #f0c040; border-radius:4px; padding:8px 12px; font-size:12px; color:#7d5700; margin-bottom:10px; }
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

      <p class="page-title"><i class="fas fa-edit"></i> Edit By Sauda Number</p>
      <p class="welcome-bar">Welcome, Admin</p>

      <!-- Type Tabs -->
      <div class="tab-btns">
          <button type="button" id="tabPurchase" class="active"
              onclick="switchTab('Purchase')">🛒 Purchase Sauda</button>
          <button type="button" id="tabSale"
              onclick="switchTab('Sale')">💰 Sale Sauda</button>
      </div>
      <input type="hidden" id="hdnSaudaType" runat="server" value="Purchase" />

      <!-- Search Card -->
      <div class="card" style="border-top-left-radius:0;">
        <h3><i class="fas fa-search"></i> Sauda Number se Search Karo</h3>
        <div class="row">
          <div class="col-sm-4">
            <label style="font-weight:bold;color:#555;font-size:13px;">Sauda Number</label>
            <asp:TextBox ID="txtSaudaNo" runat="server" CssClass="form-control"
                placeholder="e.g. RR/PS/2024-2025/0005 ya sirf 5 ya MNo"></asp:TextBox>
          </div>
          <div class="col-sm-3">
            <label style="font-weight:bold;color:#555;font-size:13px;">Financial Year</label>
            <asp:DropDownList ID="ddlFinancialYear" runat="server" CssClass="form-control" style="font-size:13px;">
                <asp:ListItem Value="0" Text="-- Sab Years --"></asp:ListItem>
                <asp:ListItem Value="2026-2027" Text="2026-2027"></asp:ListItem>
                <asp:ListItem Value="2025-2026" Text="2025-2026"></asp:ListItem>
                <asp:ListItem Value="2024-2025" Text="2024-2025"></asp:ListItem>
                <asp:ListItem Value="2023-2024" Text="2023-2024"></asp:ListItem>
                <asp:ListItem Value="2022-2023" Text="2022-2023"></asp:ListItem>
                <asp:ListItem Value="2021-2022" Text="2021-2022"></asp:ListItem>
            </asp:DropDownList>
          </div>
          <div class="col-sm-3" style="padding-top:24px;">
            <input type="submit" id="btnSearch" runat="server" value="Search"
                onserverclick="btnSearch_ServerClick" class="btn-search" />
          </div>
        </div>
      </div>

      <!-- Messages -->
      <asp:PlaceHolder ID="phMessage" runat="server"></asp:PlaceHolder>

      <!-- Edit Panel -->
      <asp:Panel ID="editPanel" runat="server" Visible="false">
        <div class="card">
          <h3><i class="fas fa-pen"></i> Edit Sauda &amp; Linked Bills —
              <asp:Label ID="lblSaudaHeading" runat="server" style="color:#1a6496;"></asp:Label>
          </h3>

          <div class="info-note">
              ⚠️ <b>Rate aur Weight yahan edit karne se:</b><br/>
              ✅ Is Sauda ki details update ho jaayegi (future ke naye bills isme se rate uthayenge)<br/>
              ✅ Is Sauda se bane purane bills mein bhi rate update ho jaayega
          </div>

          <!-- Sauda Fields -->
          <div class="section-bar">📋 Sauda Details</div>
          <div class="field-row">
              <div class="field-box">
                  <label>Sauda ID (Read Only)</label>
                  <input type="text" id="hdnSaudaID" runat="server" readonly />
              </div>
              <div class="field-box">
                  <label>Sauda Date</label>
                  <input type="text" id="saudaDate" runat="server" />
              </div>
              <div class="field-box">
                  <label>Party Name</label>
                  <input type="text" id="saudaParty" runat="server" />
              </div>
              <div class="field-box">
                  <label>Broker / Supplier's Ref.</label>
                  <input type="text" id="saudaBroker" runat="server" />
              </div>
              <div class="field-box">
                  <label>Manual No. (MNo)</label>
                  <input type="text" id="saudaMNo" runat="server" />
              </div>
          </div>

          <!-- Purchase Rates Table (shown for Purchase Sauda) -->
          <asp:Panel ID="pnlPurchaseRates" runat="server" Visible="false">
              <div class="section-bar">🌾 Paddy Rates &amp; Weight (Purchase Sauda)</div>
              <div style="background:#fff3cd;border:1px solid #ffc107;border-radius:4px;padding:7px 12px;margin-bottom:8px;font-size:12px;color:#856404;">
                  ⚡ <b>Yahan rate badalne se neeche ke <u>sab linked bills ke Rate bhi automatically update</u> ho jaayenge.</b>
                  Neeche "Paddy Items" table mein Rate field sirf <b>display ke liye</b> hai — Save karne ke baad Sauda rate se update hoga.
              </div>
              <table class="rate-table">
                  <thead>
                      <tr><th>Paddy Type</th><th>Weight (KG)</th><th>Rate (₹/KG)</th></tr>
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
          </asp:Panel>

          <!-- Sale Items Table (shown for Sale Sauda) -->
          <asp:Panel ID="pnlSaleItems" runat="server" Visible="false">
              <div class="section-bar">📦 Sale Sauda Items (Rate &amp; Qty)</div>
              <asp:PlaceHolder ID="phSaleItems" runat="server"></asp:PlaceHolder>
          </asp:Panel>

          <!-- Linked Bills (Purchase Unloading entries) -->
          <div class="section-bar">🚛 Is Sauda se Bane Purane Bills (Yahan bhi Rate Update Hoga)</div>
          <asp:PlaceHolder ID="phLinkedEntries" runat="server"></asp:PlaceHolder>

          <br />
          <input type="submit" id="btnSave" runat="server" value="✅ Save All Changes"
              onserverclick="btnSave_ServerClick" class="btn-save" />
          <input type="submit" id="btnCancel" runat="server" value="❌ Cancel"
              onserverclick="btnCancel_ServerClick" class="btn-cancel" />
        </div>
      </asp:Panel>

    </div>
  </div>
</div>

<script type="text/javascript">
    function switchTab(type) {
        document.getElementById('hdnSaudaType').value = type;
        document.getElementById('tabPurchase').className = (type === 'Purchase') ? 'active' : '';
        document.getElementById('tabSale').className = (type === 'Sale') ? 'active' : '';
        // placeholder text update
        var ph = document.getElementById('<%= txtSaudaNo.ClientID %>');
        if (type === 'Purchase')
            ph.placeholder = 'e.g. RR/PS/2024-2025/0005 ya sirf 5 ya MNo';
        else
            ph.placeholder = 'e.g. RR/SS/2024-2025/0003 ya sirf 3 ya MNo';
    }
    // On load restore tab
    window.onload = function() {
        var t = document.getElementById('hdnSaudaType').value;
        if (t) switchTab(t);
    };
</script>
</form>
</body>
</html>
