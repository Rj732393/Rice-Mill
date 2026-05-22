<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Rashmi Rice Mills</title>

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
    background:#f4f6f9;
}


/* ===== TOPBAR ===== */

.topbar{

    background:#34495e;

    padding:10px 0;

    color:white;

    font-size:14px;

    font-weight:600;
}


/* ===== NAVBAR ===== */

.navbar-custom{

    background:#ffffff;

    border:none !important;

    border-radius:0 !important;

    margin-bottom:0 !important;

    padding:15px 0;

    box-shadow:0 2px 12px rgba(0,0,0,0.06);
}

.navbar-brand{

    color:#e67e22 !important;

    font-size:32px;

    font-weight:800;
}

.navbar-nav > li > a{

    color:#2c3e50 !important;

    font-size:16px;

    font-weight:700;

    padding:12px 20px !important;

    transition:0.3s;

    background:none !important;
}

.navbar-nav > li > a:hover{

    color:#e67e22 !important;

    background:none !important;
}


/* ===== HERO SECTION ===== */

.hero-section{

    background:
    linear-gradient(rgba(255,255,255,0.86),rgba(255,255,255,0.86)),
    url('https://images.unsplash.com/photo-1586201375761-83865001e31c?q=80&w=1974&auto=format&fit=crop');

    background-size:cover;

    background-position:center;

    height:620px;

    display:flex;

    align-items:center;
}

.hero-content h1{

    font-size:65px;

    font-weight:900;

    color:#1e293b;

    line-height:80px;

    margin-bottom:25px;
}

.hero-content p{

    font-size:22px;

    color:#475569;

    line-height:38px;

    margin-bottom:35px;
}


/* ===== BUTTON ===== */

.btn-main{

    background:#e67e22;

    color:white;

    border:none;

    padding:15px 38px;

    border-radius:35px;

    font-size:18px;

    font-weight:bold;

    transition:0.3s;
}

.btn-main:hover{

    background:#cf711d;

    color:white;
}


/* ===== SECTION TITLE ===== */

.section-title{

    text-align:center;

    margin-top:80px;

    margin-bottom:50px;
}

.section-title h2{

    font-size:50px;

    font-weight:900;

    color:#1e293b;
}

.section-title p{

    font-size:18px;

    color:#64748b;
}


/* ===== ABOUT ===== */

.about-box{

    background:white;

    padding:30px;

    border-radius:20px;

    box-shadow:0 5px 18px rgba(0,0,0,0.07);

    margin-bottom:50px;
}


/* ===== DASHBOARD CARD ===== */

.dashboard-card{

    background:white;

    border-radius:22px;

    padding:25px 18px;

    text-align:center;

    height:290px;

    margin-bottom:30px;

    transition:0.3s;

    box-shadow:0 5px 20px rgba(0,0,0,0.08);

    border-top:4px solid #e67e22;
}

.dashboard-card:hover{

    transform:translateY(-8px);

    box-shadow:0 12px 28px rgba(0,0,0,0.12);
}

.dashboard-card i{

    font-size:50px;

    margin-bottom:18px;
}

.dashboard-card h3{

    font-size:24px;

    font-weight:800;

    color:#1e293b;

    margin-bottom:10px;
}

.dashboard-card p{

    color:#64748b;

    font-size:16px;

    line-height:28px;

    min-height:75px;
}


/* ===== CARD BUTTON ===== */

.btn-card{

    background:#e67e22;

    color:white;

    border:none;

    border-radius:30px;

    padding:10px 28px;

    font-size:15px;

    font-weight:bold;

    transition:0.3s;
}

.btn-card:hover{

    background:#cf711d;

    color:white;
}




/* ===== FOOTER ===== */

.footer{

    background:#34495e;

    color:white;

    padding:55px 0 35px 0;

    margin-top:70px;
}

.footer h3{

    color:#f4b183;

    font-size:26px;

    font-weight:800;

    margin-bottom:18px;
}

.footer p{

    color:#ecf0f1;

    font-size:16px;

    line-height:30px;
}


/* ===== MOBILE ===== */

@media(max-width:768px){

    .hero-content h1{

        font-size:42px;

        line-height:55px;
    }

    .hero-content p{

        font-size:18px;

        line-height:30px;
    }

    .hero-section{

        height:520px;
    }

    .dashboard-card{

        height:auto;
    }
}

</style>

</head>

<body>

<form id="form1" runat="server">

<!-- TOPBAR -->

<div class="topbar">

    <div class="container">

        <div class="pull-left">

            Welcome To Rashmi Rice Mills

        </div>

        <div class="pull-right">

            <i class="fa fa-phone"></i>
            +91 9905461260

            &nbsp;&nbsp;&nbsp;

            <i class="fa fa-envelope"></i>
            rashmiricemill@gmail.com

        </div>

        <div style="clear:both;"></div>

    </div>

</div>


<!-- NAVBAR -->

<nav class="navbar navbar-custom">

    <div class="container">

        <div class="navbar-header">

            <button type="button"
                class="navbar-toggle"
                data-toggle="collapse"
                data-target="#myNavbar">

                <span class="icon-bar"></span>
                <span class="icon-bar"></span>
                <span class="icon-bar"></span>

            </button>

            <a class="navbar-brand" href="#">

                Rashmi Rice Mills

            </a>

        </div>

        <div class="collapse navbar-collapse" id="myNavbar">

            <ul class="nav navbar-nav navbar-right">

                <li><a href="#">Home</a></li>

                <li><a href="#about">About</a></li>

                <li><a href="#dashboard">Dashboard</a></li>

                <li><a href="#contact">Contact</a></li>

            </ul>

        </div>

    </div>

</nav>


<!-- HERO SECTION -->

<div class="hero-section">

    <div class="container">

        <div class="hero-content">

            <h1>

                Smart Rice Mill
                Management System

            </h1>

            <p>

                Purchase, Billing, Reports,
                Payment and Rice Mill Operations
                managed in one dashboard.

            </p>

            <a href="#dashboard"
                class="btn btn-main">

                Open Dashboard

            </a>

        </div>

    </div>

</div>


<!-- ABOUT -->

<div class="container" id="about">

    <div class="section-title">

        <h2>About Our Rice Mill</h2>

        <p>
            Trusted Rice Manufacturer & Supplier
        </p>

    </div>

    <div class="about-box">

        <div class="row">

            <!-- IMAGE -->

            <div class="col-md-5">

                <img
                src="https://images.unsplash.com/photo-1516684732162-798a0062be99?q=80&w=1974&auto=format&fit=crop"
                class="img-responsive"
                style="
                border-radius:18px;
                width:100%;
                height:260px;
                object-fit:cover;
                " />

            </div>


            <!-- CONTENT -->

            <div class="col-md-7">

                <h2 style="
                margin-top:0;
                font-size:34px;
                color:#e67e22;
                font-weight:800;
                ">

                    Rashmi Rice Mills

                </h2>

                <p style="
                font-size:17px;
                line-height:30px;
                color:#475569;
                margin-top:20px;
                ">

                    Rashmi Rice Mills provides premium quality rice
                    with modern processing and trusted service.
                    Our system helps manage purchase, payment,
                    reports, billing and daily mill operations
                    easily from one dashboard.

                    <br /><br />

                    ✔ Smart Rice Mill Management
                    <br />

                    ✔ Fast Billing & Reports
                    <br />

                    ✔ Trusted Farmers & Suppliers
                    <br />

                    ✔ Quality Rice Processing

                </p>

            </div>

        </div>

    </div>

</div>


<!-- DASHBOARD -->

<div class="container" id="dashboard">

    <div class="section-title">

        <h2>Management Dashboard</h2>

        <p>Manage everything from one place</p>

    </div>

    <!-- ROW 1 -->

    <div class="row">

        <!-- PURCHASE -->

        <div class="col-md-3">

            <div class="dashboard-card">

                <i class="fa-solid fa-cart-shopping"
                    style="color:#2563eb"></i>

                <h3>Purchase</h3>

                <p>

                    Manage purchase and supplier entries.

                </p>

                <a href="PurchaseSauda.aspx"
                    class="btn btn-card">

                    Open

                </a>

            </div>

        </div>


        <!-- PURCHASE SAUDA -->

        <div class="col-md-3">

            <div class="dashboard-card">

                <i class="fa-solid fa-warehouse"
                    style="color:#16a34a"></i>

                <h3>Purchase Sauda</h3>

                <p>

                    Add paddy purchase and sauda records.

                </p>

                <a href="PurchaseSauda.aspx"
                    class="btn btn-card">

                    Open

                </a>

            </div>

        </div>


        <!-- PAYMENT -->

        <div class="col-md-3">

            <div class="dashboard-card">

                <i class="fa-solid fa-money-bill-wave"
                    style="color:#9333ea"></i>

                <h3>Payment</h3>

                <p>

                    Manage payment and transaction details.

                </p>

                <a href="Payment.aspx"
                    class="btn btn-card">

                    Open

                </a>

            </div>

        </div>


        <!-- REPORT -->

        <div class="col-md-3">

            <div class="dashboard-card">

                <i class="fa-solid fa-chart-column"
                    style="color:#f59e0b"></i>

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

    </div>


    <!-- ROW 2 -->

    <div class="row">

        <!-- BILL -->

        <div class="col-md-3">

            <div class="dashboard-card">

                <i class="fa-solid fa-file-invoice"
                    style="color:#dc2626"></i>

                <h3>Purchase Bill</h3>

                <p>

                    Create and print bills quickly.

                </p>

                <a href="PurchaseBill.aspx"
                    class="btn btn-card">

                    Open

                </a>

            </div>

        </div>


        <!-- SALE SAUDA -->

        <div class="col-md-3">

            <div class="dashboard-card">

                <i class="fa-solid fa-bag-shopping"
                    style="color:#0891b2"></i>

                <h3>Sale Sauda</h3>

                <p>

                    Manage rice sale and customer entries.

                </p>

                <a href="sale.aspx"
                    class="btn btn-card">

                    Open

                </a>

            </div>

        </div>


        <!-- PADDY PROCESSING -->

        <div class="col-md-3">

            <div class="dashboard-card">

                <i class="fa-solid fa-industry"
                    style="color:#7c3aed"></i>

                <h3>Paddy Processing</h3>

                <p>

                    Monitor paddy processing activities.

                </p>

                <a href="#"
                    class="btn btn-card">

                    Open

                </a>

            </div>

        </div>


        <!-- DAILY EXPENSE -->

        <div class="col-md-3">

            <div class="dashboard-card">

                <i class="fa-solid fa-wallet"
                    style="color:#ea580c"></i>

                <h3>Daily Expense</h3>

                <p>

                    Manage daily rice mill expenses.

                </p>

                <a href="#"
                    class="btn btn-card">

                    Open

                </a>

            </div>

        </div>

    </div>

</div>




<!-- FOOTER -->

<div class="footer" id="contact">

    <div class="container">

        <div class="row">

            <!-- COMPANY -->

            <div class="col-md-4">

                <h3>Rashmi Rice Mills</h3>

                <p>

                    Premium rice manufacturer and exporter
                    providing high quality rice products
                    across India.

                </p>

            </div>


            <!-- QUICK LINKS -->

            <div class="col-md-4">

                <h3>Quick Links</h3>

                <p>

                    Home
                    <br />

                    About
                    <br />

                    Purchase
                    <br />

                    Payment
                    <br />

                    Reports

                </p>

            </div>


            <!-- CONTACT -->

            <div class="col-md-4">

                <h3>Contact Info</h3>

                <p>

                    <i class="fa fa-phone"></i>
                    +91 9905461260

                    <br /><br />

                    <i class="fa fa-envelope"></i>
                    rashmiricemill@gmail.com

                    <br /><br />

                    <i class="fa fa-location-dot"></i>
                    Bihar, India

                </p>

            </div>

        </div>

    </div>

</div>

</form>

</body>

</html>