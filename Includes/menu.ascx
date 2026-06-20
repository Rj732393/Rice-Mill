<%@ Control Language="C#" AutoEventWireup="true"
    CodeFile="menu.ascx.cs"
    Inherits="Includes_WebUserControl" %>

      <link rel="stylesheet" type="text/css" 
      href="<%= ResolveUrl("~/CSS/Menu.css") %>" />

<script type="text/javascript">

    function toggleSidebar() {
        $(".sidebar").toggleClass("hide");
        $(".main-content").toggleClass("sidebar-hidden");
    }

    // Highlight active sidebar card based on current page
    $(document).ready(function () {
        var path = window.location.pathname.toLowerCase();
        $(".side-card").each(function () {
            var href = $(this).closest("a").attr("href");
            if (href && path.indexOf(href.toLowerCase().replace(".aspx", "")) !== -1) {
                $(".side-card").removeClass("active-side-card");
                $(this).addClass("active-side-card");
            }
        });
    });

</script>

<!-- ===== TOP NAVBAR ===== -->
<nav class="navbar-custom">
    <div class="navbar-inner">

        <!-- Left: Hamburger + Logo -->
        <div class="nav-left">
            <button class="menu-toggle" onclick="toggleSidebar()" title="Toggle Sidebar">
                <i class="fa fa-bars"></i>
            </button>

            <div class="brand-wrap">
                <img src="<%= ResolveUrl("~/Content/Images/logo.png") %>"
                     class="brand-logo" alt="Logo"
                     onerror="this.style.display='none'" />
                <a class="navbar-brand" href="Home.aspx">
    <%= Session["CompanyName"] != null ? Session["CompanyName"].ToString() + " Management System" : "Rice Mill Management System" %>
</a>
            </div>
        </div>

        <!-- Right: Nav links + User -->
        <div class="nav-right">
            <a href="Home.aspx" class="nav-link-item">Home</a>


            <div class="nav-dropdown">
                <a href="#" class="nav-link-item dropdown-toggle-link">
                    Reports <i class="fa fa-chevron-down" style="font-size:11px;"></i>
                </a>
                <div class="nav-dropdown-menu">
                    <a href="PurchaseReport.aspx">Purchase Report</a>
                    <a href="SaleReport.aspx">Sale Report</a>
                </div>
            </div>

<!-- User Dropdown -->
<div class="nav-user-wrap" style="position:relative;">

    <!-- User Button -->
    <div class="nav-user" onclick="toggleUserMenu()" 
         style="cursor:pointer;">
        <div class="user-avatar">
            <i class="fa-solid fa-user"></i>
        </div>
        <span class="user-name" style="color:white;">
            <%= Session["User"] != null ? Session["User"].ToString() : "User" %>
        </span>
        <i class="fa fa-chevron-down" 
           style="color:white; font-size:11px; margin-left:5px;"></i>
    </div>

    <!-- Dropdown Menu -->
    <div id="userDropdown" style="
        display:none;
        position:absolute;
        right:0;
        top:48px;
        background:white;
        border-radius:12px;
        box-shadow:0 8px 25px rgba(0,0,0,0.15);
        min-width:160px;
        z-index:9999;
        overflow:hidden;">

        <!-- Username Info -->
        <div style="padding:12px 16px; 
                    border-bottom:1px solid #f1f5f9;
                    font-size:13px; 
                    color:#64748b;">
            <i class="fa-solid fa-user" style="margin-right:6px;"></i>
            <%= Session["User"] != null ? Session["User"].ToString() : "User" %>
        </div>

        <!-- Logout -->
        <a href="Login.aspx" style="
            display:flex;
            align-items:center;
            gap:8px;
            padding:12px 16px;
            text-decoration:none;
            color:#ef4444;
            font-size:14px;
            font-weight:600;
            transition:0.2s;">
            <i class="fa-solid fa-right-from-bracket"></i>
            Logout
        </a>

    </div>
</div>

<script type="text/javascript">
    function toggleUserMenu() {
        var menu = document.getElementById("userDropdown");
        menu.style.display = (menu.style.display === "none") ? "block" : "none";
    }

    // Bahar click karne pe band ho jaye
    document.addEventListener("click", function (e) {
        var wrap = document.querySelector(".nav-user-wrap");
        if (wrap && !wrap.contains(e.target)) {
            document.getElementById("userDropdown").style.display = "none";
        }
    });
</script>
    </div>
</nav>


<!-- ===== LEFT SIDEBAR ===== -->
<div class="sidebar" id="mainSidebar">

    <a href="Home.aspx" style="text-decoration:none;">
        <div class="side-card">
            <i class="fa-solid fa-house"></i>
            <h5>Home</h5>
        </div>
    </a>

    <a href="PurchaseSauda.aspx" style="text-decoration:none;">
        <div class="side-card">
            <i class="fa-solid fa-cart-shopping"></i>
            <h5>Purchase Sauda</h5>
        </div>
    </a>

    <a href="Payment.aspx" style="text-decoration:none;">
        <div class="side-card">
            <i class="fa-solid fa-money-bill-wave"></i>
            <h5>Purchase Payment</h5>
        </div>
    </a>

    <a href="PurchaseReport.aspx" style="text-decoration:none;">
        <div class="side-card">
            <i class="fa-solid fa-chart-column"></i>
            <h5>Purchase Report</h5>
        </div>
    </a>

    <a href="SaleSauda.aspx" style="text-decoration:none;">
        <div class="side-card">
            <i class="fa-solid fa-bag-shopping"></i>
            <h5>Sale Sauda</h5>
        </div>
    </a>

     

    <a href="SalePayment.aspx" style="text-decoration:none;">
        <div class="side-card">
            <i class="fa-solid fa-credit-card"></i>
            <h5>Sale Payment</h5>
        </div>
    </a>

    <a href="SaleReport.aspx" style="text-decoration:none;">
        <div class="side-card">
            <i class="fa-solid fa-file-lines"></i>
            <h5>Sale Report</h5>
        </div>
    </a>

    <a href="PaddyProcessing.aspx" style="text-decoration:none;">
        <div class="side-card">
            <i class="fa-solid fa-industry"></i>
            <h5>Paddy Process</h5>
        </div>
    </a>

    <a href="Expense.aspx" style="text-decoration:none;">
        <div class="side-card">
            <i class="fa-solid fa-wallet"></i>
            <h5>Daily Expense</h5>
        </div>
    </a>

</div>
