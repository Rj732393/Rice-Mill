<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Rice Mill Management System</title>

    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Bootstrap -->

    <link rel="stylesheet"
        href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css" />

    <!-- Font Awesome -->

    <link rel="stylesheet"
        href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>

    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>

<style>

body{
    margin:0;
    padding:0;
    font-family:'Segoe UI';
    background:#f4f7fb;
}





/* ===== NAVBAR ===== */

.navbar-custom{
    background:white;
    border:none !important;
    border-radius:0 !important;
    margin-bottom:0 !important;
    padding:12px 0;
    box-shadow:0 2px 12px rgba(0,0,0,0.08);
}

.navbar-header{
    width:100%;
}

.navbar-brand{
    float:left !important;
    text-align:left !important;
    width:100%;
    color:#f97316 !important;
    font-size:30px;
    font-weight:900;
}

.navbar-nav > li > a{
    color:#1e293b !important;
    font-size:16px;
    font-weight:700;
    padding:14px 18px !important;
    transition:0.3s;
    
}

.navbar-nav > li > a:hover{
    color:#f97316 !important;
    background:none !important;
}



/* ===== HERO ===== */

.hero-section{
    background:linear-gradient(135deg,#fff7ed,#ffffff);
    padding:10px 0 20px 0;
    text-align:center;
    border-bottom:1px solid #e5e7eb;
}

.hero-section h1{
    font-size:42px;
    font-weight:900;
    color:#1e293b;
    margin-bottom:8px;
}





/* ===== DASHBOARD ===== */

.dashboard-section{
    padding:30px 0 60px 0;
}


/* ===== CARDS ===== */

.dashboard-card{
    background:white;
    border-radius:22px;
    padding:28px 20px;
    text-align:center;
    height:280px;
    margin-bottom:30px;
    transition:0.35s;
    box-shadow:0 5px 22px rgba(0,0,0,0.08);
    border-top:5px solid #f97316;
}

.dashboard-card:hover{
    transform:translateY(-8px);
    box-shadow:0 15px 35px rgba(0,0,0,0.12);
}

.dashboard-card i{
    font-size:52px;
    margin-bottom:18px;
}

.dashboard-card h3{
    font-size:24px;
    font-weight:800;
    color:#1e293b;
    margin-bottom:12px;
}

.dashboard-card p{
    color:#64748b;
    font-size:15px;
    line-height:28px;
    min-height:60px;
}


/* ===== BUTTON ===== */

.btn-card{
    background:#f97316;
    color:white !important;
    border:none;
    border-radius:30px;
    padding:10px 28px;
    font-size:15px;
    font-weight:bold;
    transition:0.3s;
    text-decoration:none !important;
}

.btn-card:hover{
    background:#ea580c;
    color:white !important;
}





/* ===== MOBILE ===== */

@media(max-width:768px){

    .hero-section h1{
        font-size:38px;
        line-height:50px;
    }

    .dashboard-card{
        height:auto;
    }

    .navbar-brand{
        font-size:24px;
    }
}

</style>

</head>

<body>

<form id="form1" runat="server">



<!-- NAVBAR -->

<nav class="navbar navbar-custom">

    <div class="container-fluid">

        <div class="navbar-header">

            <a class="navbar-brand" href="#">
                Rashmi Rice Mill Management System
            </a>

        </div>

    </div>

</nav>



<!-- DASHBOARD -->

<div class="dashboard-section" id="dashboard">

    <div class="container">

        <div class="row">

            <!-- Purchase Sauda -->

            <div class="col-md-3">

                <div class="dashboard-card">

                    <i class="fa-solid fa-cart-shopping"
                        style="color:#2563eb"></i>

                    <h3>Purchase Sauda</h3>

                    <p>
                        Manage paddy purchase and supplier records.
                    </p>

                    <a href="PurchaseSauda.aspx"
                        class="btn btn-card">

                        Open

                    </a>

                </div>

            </div>


            <!-- Purchase Payment -->

            <div class="col-md-3">

                <div class="dashboard-card">

                    <i class="fa-solid fa-money-bill-wave"
                        style="color:#16a34a"></i>

                    <h3>Purchase Payment</h3>

                    <p>
                        Manage purchase payment transactions.
                    </p>

                    <a href="Payment.aspx"
                        class="btn btn-card">

                        Open

                    </a>

                </div>

            </div>


            <!-- Purchase Report -->

            <div class="col-md-3">

                <div class="dashboard-card">

                    <i class="fa-solid fa-chart-column"
                        style="color:#9333ea"></i>

                    <h3>Purchase Report</h3>

                    <p>
                        Generate purchase reports instantly.
                    </p>

                    <a href="PurchaseReport.aspx"
                        class="btn btn-card">

                        Open

                    </a>

                </div>

            </div>


            <!-- Sale Sauda -->

            <div class="col-md-3">

                <div class="dashboard-card">

                    <i class="fa-solid fa-bag-shopping"
                        style="color:#f59e0b"></i>

                    <h3>Sale Sauda</h3>

                    <p>
                        Manage rice sale and customer records.
                    </p>

                    <a href="Sale.aspx"
                        class="btn btn-card">

                        Open

                    </a>

                </div>

            </div>

        </div>


        <!-- SECOND ROW -->

        <div class="row">

            <!-- Sale Payment -->

            <div class="col-md-3">

                <div class="dashboard-card">

                    <i class="fa-solid fa-credit-card"
                        style="color:#dc2626"></i>

                    <h3>Sale Payment</h3>

                    <p>
                        Manage customer payment transactions.
                    </p>

                    <a href="SalePayment.aspx"
                        class="btn btn-card">

                        Open

                    </a>

                </div>

            </div>


            <!-- Sale Report -->

            <div class="col-md-3">

                <div class="dashboard-card">

                    <i class="fa-solid fa-file-lines"
                        style="color:#0891b2"></i>

                    <h3>Sale Report</h3>

                    <p>
                        Generate sale reports and analytics.
                    </p>

                    <a href="SaleReport.aspx"
                        class="btn btn-card">

                        Open

                    </a>

                </div>

            </div>


            <!-- Paddy Processing -->

            <div class="col-md-3">

                <div class="dashboard-card">

                    <i class="fa-solid fa-industry"
                        style="color:#7c3aed"></i>

                    <h3>Paddy Processing</h3>

                    <p>
                        Monitor paddy processing activities.
                    </p>

                    <a href="PaddyProcessing.aspx"
                        class="btn btn-card">

                        Open

                    </a>

                </div>

            </div>


            <!-- Daily Expense -->

            <div class="col-md-3">

                <div class="dashboard-card">

                    <i class="fa-solid fa-wallet"
                        style="color:#ea580c"></i>

                    <h3>Daily Expense</h3>

                    <p>
                        Manage daily rice mill expenses.
                    </p>

                    <a href="Expense.aspx"
                        class="btn btn-card">

                        Open

                    </a>

                </div>

            </div>

        </div>

    </div>

</div>




    </div>

</div>

</form>

</body>

</html>