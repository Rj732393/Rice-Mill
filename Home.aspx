<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

<%@ Register Src="~/Includes/menu.ascx" TagPrefix="uc1" TagName="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Rashmi Rice Mill Management System</title>

    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Bootstrap -->
    <link rel="stylesheet"
        href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css" />

    <!-- Font Awesome -->
    <link rel="stylesheet"
        href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />

    <!-- JQuery -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>

    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>

<style>

body{
    margin:0;
    padding:0;
    font-family:'Segoe UI',sans-serif;
    background:#f5fff7;
    overflow-x:hidden;
}

/* ===== MAIN CONTENT ===== */

.main-content{
    margin-left:250px;
    padding:90px 20px 20px;
    transition:0.3s;
}

.main-content.sidebar-hidden{
    margin-left:0;
}

/* ===== TOP WELCOME ===== */

.top-banner{
    background:linear-gradient(135deg,#16a34a,#15803d);
    border-radius:20px;
    padding:28px;
    margin-bottom:25px;
    color:white;
    position:relative;
    overflow:hidden;
    box-shadow:0 10px 25px rgba(22,163,74,0.18);
}

.top-banner:before{
    content:'';
    position:absolute;
    right:-40px;
    top:-40px;
    width:180px;
    height:180px;
    background:rgba(255,255,255,0.08);
    border-radius:50%;
}

.top-banner h1{
    margin:0;
    font-size:30px;
    font-weight:800;
}

.top-banner p{
    margin-top:8px;
    font-size:14px;
    opacity:0.95;
}

/* ===== SECTION ===== */

.section-box{
    background:white;
    border-radius:18px;
    padding:18px;
    margin-bottom:22px;
    box-shadow:0 3px 12px rgba(0,0,0,0.06);
    border:1px solid #e5f7ea;
}

/* ===== SECTION TITLE ===== */

.section-title{
    display:flex;
    align-items:center;
    justify-content:space-between;
    margin-bottom:18px;
}

.section-title-left{
    display:flex;
    align-items:center;
}

.section-title-left i{
    width:38px;
    height:38px;
    background:#dcfce7;
    border-radius:10px;
    display:flex;
    align-items:center;
    justify-content:center;
    color:#16a34a;
    margin-right:10px;
    font-size:16px;
}

.section-title h2{
    margin:0;
    font-size:20px;
    color:#14532d;
    font-weight:700;
}

.section-badge{
    background:#dcfce7;
    color:#166534;
    padding:5px 12px;
    border-radius:30px;
    font-size:12px;
    font-weight:600;
}

/* ===== DASHBOARD CARD ===== */

.dashboard-card{
    background:#fbfffc;
    border:1px solid #e5f7ea;
    border-radius:16px;
    padding:18px 15px;
    transition:0.3s;
    cursor:pointer;
    margin-bottom:18px;
    position:relative;
    overflow:hidden;
    min-height:185px;
}

.dashboard-card:hover{
    transform:translateY(-4px);
    box-shadow:0 8px 22px rgba(0,0,0,0.08);
    border-color:#16a34a;
}

/* ===== ICON ===== */

.card-icon{
    width:55px;
    height:55px;
    border-radius:14px;
    background:linear-gradient(135deg,#dcfce7,#bbf7d0);
    display:flex;
    align-items:center;
    justify-content:center;
    margin-bottom:15px;
}

.card-icon i{
    font-size:24px;
    color:#16a34a;
}

/* ===== CARD TEXT ===== */

.dashboard-card h3{
    margin:0 0 8px;
    font-size:18px;
    font-weight:700;
    color:#14532d;
}

.dashboard-card p{
    font-size:13px;
    color:#64748b;
    line-height:22px;
    min-height:42px;
}


/* ===== BUTTON ===== */

.open-btn{
    display:inline-flex;
    align-items:center;
    justify-content:center;
    gap:6px;

    margin-top:12px;

    background:linear-gradient(135deg,#16a34a,#15803d);
    color:#ffffff !important;

    padding:8px 18px;

    border-radius:10px;

    font-size:12px;
    font-weight:700;

    text-decoration:none !important;

    border:none;

    box-shadow:0 4px 10px rgba(22,163,74,0.22);

    transition:all 0.25s ease;
}

/* Hover */

.open-btn:hover{

    background:linear-gradient(135deg,#15803d,#166534);

    transform:translateY(-2px);

    box-shadow:0 8px 18px rgba(22,163,74,0.30);

    color:#ffffff !important;
}

/* Arrow Icon */

.open-btn i{
    font-size:11px;
    transition:0.25s;
}

/* Hover Icon Animation */

.dashboard-card:hover .open-btn i{
    transform:translateX(3px);
}

/* ===== MOBILE ===== */

@media(max-width:991px){

    .main-content{
        margin-left:0;
        padding:85px 12px 15px;
    }

    .top-banner h1{
        font-size:24px;
    }

    .section-title h2{
        font-size:18px;
    }

    .dashboard-card{
        min-height:auto;
    }
}

</style>

</head>

<body>

<form id="form1" runat="server">

    <!-- MENU -->
    <uc1:Menu ID="Menu1" runat="server" />

    <!-- MAIN CONTENT -->
    <div class="main-content">

        <div class="container-fluid">

            <!-- PURCHASE SECTION -->
            <div class="section-box">

                <div class="section-title">

                    <div class="section-title-left">

                        <i class="fa-solid fa-cart-shopping"></i>

                        <h2>Purchase Management</h2>

                    </div>

                    <div class="section-badge">
                        3 Modules
                    </div>

                </div>

                <div class="row">

                    <!-- Purchase Sauda -->
                    <div class="col-md-4 col-sm-6">

                        <div class="dashboard-card"
                            onclick="window.location='PurchaseSauda.aspx'">

                            <div class="card-icon">
                                <i class="fa-solid fa-cart-shopping"></i>
                            </div>

                            <h3>Purchase Sauda</h3>

                            <p>
                                Manage paddy purchase entries and supplier records.
                            </p>

                            <span class="open-btn">
                                Open
                                <i class="fa fa-arrow-right"></i>
                            </span>

                        </div>

                    </div>

                    <!-- Purchase Payment -->
                    <div class="col-md-4 col-sm-6">

                        <div class="dashboard-card"
                            onclick="window.location='Payment.aspx'">

                            <div class="card-icon">
                                <i class="fa-solid fa-money-bill-wave"></i>
                            </div>

                            <h3>Purchase Payment</h3>

                            <p>
                                Handle supplier payments and transactions.
                            </p>

                            <span class="open-btn">
                                Open
                                <i class="fa fa-arrow-right"></i>
                            </span>

                        </div>

                    </div>

                    <!-- Purchase Report -->
                    <div class="col-md-4 col-sm-6">

                        <div class="dashboard-card"
                            onclick="window.location='PurchaseReport.aspx'">

                            <div class="card-icon">
                                <i class="fa-solid fa-chart-column"></i>
                            </div>

                            <h3>Purchase Report</h3>

                            <p>
                                View and generate detailed purchase reports.
                            </p>

                            <span class="open-btn">
                                Open
                                <i class="fa fa-arrow-right"></i>
                            </span>

                        </div>

                    </div>

                </div>

            </div>

            <!-- SALE SECTION -->
            <div class="section-box">

                <div class="section-title">

                    <div class="section-title-left">

                        <i class="fa-solid fa-bag-shopping"></i>

                        <h2>Sale Management</h2>

                    </div>

                    <div class="section-badge">
                        3 Modules
                    </div>

                </div>

                <div class="row">

                    <!-- Sale Sauda -->
                    <div class="col-md-4 col-sm-6">

                        <div class="dashboard-card"
                            onclick="window.location='Sale.aspx'">

                            <div class="card-icon">
                                <i class="fa-solid fa-bag-shopping"></i>
                            </div>

                            <h3>Sale Sauda</h3>

                            <p>
                                Manage rice sales and customer information.
                            </p>

                            <span class="open-btn">
                                Open
                                <i class="fa fa-arrow-right"></i>
                            </span>

                        </div>

                    </div>

                    <!-- Sale Payment -->
                    <div class="col-md-4 col-sm-6">

                        <div class="dashboard-card"
                            onclick="window.location='SalePayment.aspx'">

                            <div class="card-icon">
                                <i class="fa-solid fa-credit-card"></i>
                            </div>

                            <h3>Sale Payment</h3>

                            <p>
                                Handle customer payments and dues.
                            </p>

                            <span class="open-btn">
                                Open
                                <i class="fa fa-arrow-right"></i>
                            </span>

                        </div>

                    </div>

                    <!-- Sale Report -->
                    <div class="col-md-4 col-sm-6">

                        <div class="dashboard-card"
                            onclick="window.location='SaleReport.aspx'">

                            <div class="card-icon">
                                <i class="fa-solid fa-file-lines"></i>
                            </div>

                            <h3>Sale Report</h3>

                            <p>
                                Generate detailed sales analytics and reports.
                            </p>

                            <span class="open-btn">
                                Open
                                <i class="fa fa-arrow-right"></i>
                            </span>

                        </div>

                    </div>

                </div>

            </div>

            <!-- OTHER SECTION -->
            <div class="section-box">

                <div class="section-title">

                    <div class="section-title-left">

                        <i class="fa-solid fa-industry"></i>

                        <h2>Operations</h2>

                    </div>

                    <div class="section-badge">
                        2 Modules
                    </div>

                </div>

                <div class="row">

                    <!-- Paddy Processing -->
                    <div class="col-md-6 col-sm-6">

                        <div class="dashboard-card"
                            onclick="window.location='PaddyProcessing.aspx'">

                            <div class="card-icon">
                                <i class="fa-solid fa-industry"></i>
                            </div>

                            <h3>Paddy Processing</h3>

                            <p>
                                Monitor rice processing and production activities.
                            </p>

                            <span class="open-btn">
                                Open
                                <i class="fa fa-arrow-right"></i>
                            </span>

                        </div>

                    </div>

                    <!-- Daily Expense -->
                    <div class="col-md-6 col-sm-6">

                        <div class="dashboard-card"
                            onclick="window.location='Expense.aspx'">

                            <div class="card-icon">
                                <i class="fa-solid fa-wallet"></i>
                            </div>

                            <h3>Daily Expense</h3>

                            <p>
                                Track and manage daily rice mill expenses.
                            </p>

                            <span class="open-btn">
                                Open
                                <i class="fa fa-arrow-right"></i>
                            </span>

                        </div>

                    </div>

                </div>

            </div>

        </div>

    </div>

</form>

</body>

</html>