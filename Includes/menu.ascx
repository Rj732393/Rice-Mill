<%@ Control Language="C#" AutoEventWireup="true"
    CodeFile="menu.ascx.cs"
    Inherits="Includes_WebUserControl" %>

<link rel="stylesheet"
      type="text/css"
      href="<%= ResolveUrl("~/Content/Navbar.css") %>" />


<script type="text/javascript">

    function toggleSidebar() {

        $(".sidebar").toggleClass("hide");

        $(".main-content").toggleClass("full");

    }

</script>

<!-- NAVBAR -->

<nav class="navbar navbar-custom">

    <div class="container-fluid">

        <div class="navbar-header nav-flex">

            <button type="button"
                class="menu-toggle"
                onclick="toggleSidebar()">

                <i class="fa fa-bars"></i>

            </button>

            <a class="navbar-brand"
                href="Home.aspx">

                Rashmi Rice Mill Management System

            </a>

        </div>

    </div>

</nav>

<!-- SIDEBAR -->

<div class="sidebar">

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

    <a href="Sale.aspx" style="text-decoration:none;">
        <div class="side-card active-side-card">
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