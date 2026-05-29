<%@ Control Language="C#" AutoEventWireup="true"
    CodeFile="AdminMenu.ascx.cs"
    Inherits="Includes_AdminMenu" %>

<!-- SIDEBAR -->
<div class="rm-sidebar" id="rmSidebar">

    <!-- LOGO -->
    <div class="rm-logo">

        <h2>Rashmi Rice</h2>

        <span>
            Management System
        </span>

    </div>

    <!-- MENU -->
    <ul class="rm-menu">

        <li>
            <a href="Dashboard.aspx">

                <i class="fas fa-home"></i>

                Dashboard

            </a>
        </li>

        <li>
            <a href="RiceStock.aspx"
                class="active">

                <i class="fas fa-seedling"></i>

                Rice Stock

            </a>
        </li>

        <li>
            <a href="PaddyStock.aspx">

                <i class="fas fa-boxes"></i>

                Paddy Stock

            </a>
        </li>

        <li>
            <a href="SalePurchaseExpense.aspx">

                <i class="fas fa-shopping-cart"></i>

                Sale Purchase

            </a>
        </li>

        <li>
            <a href="EditData.aspx">

                <i class="fas fa-edit"></i>

                Edit Data

            </a>
        </li>

        <li>
            <a href="Logout.aspx">

                <i class="fas fa-sign-out-alt"></i>

                Logout

            </a>
        </li>

    </ul>

</div>

<!-- NAVBAR -->
<div class="rm-navbar" id="rmNavbar">

    <!-- LEFT -->
    <div class="rm-nav-left">

        <!-- TOGGLE BUTTON -->
        <div class="rm-toggle"
            onclick="toggleSidebar()">

            <span></span>
            <span></span>
            <span></span>

        </div>

        <div class="rm-title">

            Rice Stock Management

        </div>

    </div>

    <!-- RIGHT -->
    <div class="rm-admin">

        <i class="fas fa-user-shield"></i>

        Welcome Admin

    </div>

</div>

<!-- SCRIPT -->
<script type="text/javascript">

    function toggleSidebar() {

        var sidebar =
            document.getElementById("rmSidebar");

        var navbar =
            document.getElementById("rmNavbar");

        var main =
            document.querySelector(".main-wrapper");

        sidebar.classList.toggle("hideSidebar");

        navbar.classList.toggle("fullNavbar");

        main.classList.toggle("fullMain");

    }

</script>