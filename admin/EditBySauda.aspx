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
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap" rel="stylesheet" />
<link href="../CSS/AdminMenu.css" rel="stylesheet" type="text/css" />
    <style>
      body{
    background:#f1f5f9;
    font-family:'Poppins',sans-serif;
    overflow-x:hidden;
}

.main-wrapper{
    margin-left:270px;
    margin-top:100px;
    padding:30px;
}

.dashboard-card{
    background:#fff;
    border-radius:24px;
    padding:40px;
    box-shadow:0 12px 35px rgba(0,0,0,.08);
}

.page-title{
    text-align:center;
    margin-bottom:30px;
}

.page-title h1{
    font-size:32px;
    font-weight:700;
    color:#1e293b;
}

.page-title p{
    color:#64748b;
}

.section-card{
    background:#fff;
    border-radius:18px;
    padding:25px;
    margin-top:20px;
    box-shadow:0 8px 20px rgba(0,0,0,.05);
}

.form-control{
    border-radius:12px !important;
    height:48px !important;
}

.btn-search{
    background:linear-gradient(135deg,#2563eb,#06b6d4);
    border:none;
    color:#fff;
    padding:12px 25px;
    border-radius:12px;
    font-weight:600;
}

.btn-save{
    background:linear-gradient(135deg,#16a34a,#22c55e);
    border:none;
    color:#fff;
    padding:12px 25px;
    border-radius:12px;
    font-weight:600;
}

.btn-cancel{
    background:linear-gradient(135deg,#dc2626,#ef4444);
    border:none;
    color:#fff;
    padding:12px 25px;
    border-radius:12px;
    font-weight:600;
}

.section-bar{
    background:linear-gradient(135deg,#2563eb,#06b6d4);
    color:#fff;
    padding:12px 18px;
    border-radius:12px;
    font-weight:600;
    margin-bottom:15px;
}

.rate-table{
    width:100%;
    border-collapse:collapse;
}

.rate-table th{
    background:#2563eb;
    color:#fff;
    padding:12px;
}

.rate-table td{
    padding:10px;
    border:1px solid #e2e8f0;
}

.rate-table input{
    width:100%;
    padding:8px;
    border-radius:8px;
    border:1px solid #cbd5e1;
}
.container{
    width:100%;
    max-width:100%;
    padding:0;
}

.field-box input,
.field-box select{
    width:100%;
    height:50px;
    border-radius:12px;
    border:1px solid #cbd5e1;
    padding:10px 15px;
}

.info-note{
    background:#fff7ed;
    border-left:5px solid #f59e0b;
    padding:15px;
    border-radius:12px;
    margin-bottom:20px;
}
.tab-btns{
    text-align:center;
    margin-bottom:25px;
}

.tab-btns button{
    background:#fff;
    border:1px solid #dbe4f0;
    padding:12px 25px;
    border-radius:12px;
    font-size:15px;
    font-weight:600;
    color:#334155;
    margin:0 5px;
    transition:0.3s;
}

.tab-btns button i{
    margin-right:8px;
}

.tab-btns button:hover{
    background:#f8fafc;
}

.tab-btns button.active{
    background:linear-gradient(135deg,#2563eb,#06b6d4);
    color:#fff;
    border:none;
}

@media(max-width:900px){
    .main-wrapper{
        margin-left:0;
        margin-top:20px;
        padding:15px;
    }

    .dashboard-card{
        padding:20px;
    }
}
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
    <<uc1:WebUserControl1 ID="WebUserControl11" runat="server" />

<div class="main-wrapper">

<div class="dashboard-card">

<div class="page-title">

    <h1>

        <i class="fas fa-edit"
            style="color:#2563eb;"></i>

        Edit By Sauda Number

    </h1>

    <p>
        Welcome, Admin
    </p>

</div>.
      <!-- Type Tabs -->
      <div class="tab-btns">
    
    <button type="button"
        id="tabPurchase"
        class="active"
        onclick="switchTab('Purchase')">

        <i class="fas fa-shopping-cart"></i>
        Purchase Sauda

    </button>

    <button type="button"
        id="tabSale"
        onclick="switchTab('Sale')">

        <i class="fas fa-rupee-sign"></i>
        Sale Sauda

    </button>

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
    window.onload = function () {
        var t = document.getElementById('hdnSaudaType').value;
        if (t) switchTab(t);
    };
</script>
</form>
</body>
</html>