<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="Dashboard.aspx.cs"
    Inherits="admin_Dashboard" %>

<%@ Register Src="../Includes/AdminMenu.ascx"
    TagName="WebUserControl1"
    TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Admin Dashboard | Rice Mill</title>

    <meta name="viewport"
        content="width=device-width, initial-scale=1" />

    <!-- Bootstrap -->
    <link rel="stylesheet"
        href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />

    <!-- Font Awesome -->
    <link rel="stylesheet"
        href="https://use.fontawesome.com/releases/v5.7.2/css/all.css" />

    <!-- Google Font -->
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap"
        rel="stylesheet" />

    <!-- Sidebar CSS -->
    <link href="../CSS/AdminMenu.css"
        rel="stylesheet"
        type="text/css" />

    <style type="text/css">

        *{
            margin:0;
            padding:0;
            box-sizing:border-box;
            font-family:'Poppins',sans-serif;
        }

        body{
            background:#f1f5f9;
            overflow-x:hidden;
        }

        /* MAIN */

        .main-wrapper{
            margin-left:270px;
            margin-top:95px;
            padding:30px;
            transition:0.4s;
        }

        /* TOP BANNER */

        .welcome-banner{
            background:linear-gradient(135deg,#2563eb,#06b6d4);
            border-radius:28px;
            padding:45px;
            color:white;
            position:relative;
            overflow:hidden;
            margin-bottom:30px;
            box-shadow:0 15px 40px rgba(37,99,235,0.25);
        }

        .welcome-banner:before{
            content:'';
            position:absolute;
            width:250px;
            height:250px;
            background:rgba(255,255,255,0.1);
            border-radius:50%;
            top:-80px;
            right:-60px;
        }

        .welcome-banner h1{
            font-size:42px;
            font-weight:700;
            margin-bottom:10px;
        }

        .welcome-banner p{
            font-size:16px;
            opacity:0.95;
        }

        @keyframes fadeInUp {
    from { opacity: 0; transform: translateY(40px); }
    to   { opacity: 1; transform: translateY(0px);  }
}

.welcome-banner {
    animation: fadeInUp 0.5s ease both;
}

.col-md-3:nth-child(1) .dashboard-card { animation: fadeInUp 0.6s ease 0.1s both; }
.col-md-3:nth-child(2) .dashboard-card { animation: fadeInUp 0.6s ease 0.2s both; }
.col-md-3:nth-child(3) .dashboard-card { animation: fadeInUp 0.6s ease 0.3s both; }
.col-md-3:nth-child(4) .dashboard-card { animation: fadeInUp 0.6s ease 0.4s both; }

.quick-links a:nth-child(1) { animation: fadeInUp 0.6s ease 0.2s both; }
.quick-links a:nth-child(2) { animation: fadeInUp 0.6s ease 0.3s both; }
.quick-links a:nth-child(3) { animation: fadeInUp 0.6s ease 0.4s both; }
.quick-links a:nth-child(4) { animation: fadeInUp 0.6s ease 0.5s both; }

.activity-box {
    animation: fadeInUp 0.6s ease 0.6s both;
}

.section-title {
    animation: fadeInUp 0.5s ease 0.1s both;
}
        /* CARDS */

        .dashboard-card{
            background:white;
            border-radius:24px;
            padding:28px;
            position:relative;
            overflow:hidden;
            margin-bottom:30px;
            transition:0.4s;
            box-shadow:0 10px 25px rgba(0,0,0,0.06);
        }

        .dashboard-card:hover{
            transform:translateY(-8px);
        }

        .dashboard-card h2{
            font-size:34px;
            font-weight:700;
            margin-top:12px;
        }
        
        

        .dashboard-card p{
            color:#64748b;
            font-size:15px;
            margin-top:5px;
        }

        .dashboard-icon{
            width:70px;
            height:70px;
            border-radius:18px;
            display:flex;
            align-items:center;
            justify-content:center;
            font-size:30px;
            color:white;
        }

        .bg-blue{
            background:linear-gradient(135deg,#2563eb,#3b82f6);
        }

        .bg-green{
            background:linear-gradient(135deg,#16a34a,#22c55e);
        }

        .bg-orange{
            background:linear-gradient(135deg,#ea580c,#f97316);
        }

        .bg-purple{
            background:linear-gradient(135deg,#7c3aed,#9333ea);
        }

        /* SECTION */

        .section-title{
            font-size:28px;
            font-weight:700;
            color:#1e293b;
            margin-bottom:25px;
        }

        /* QUICK LINKS */

        .quick-links{
            display:grid;
            grid-template-columns:repeat(auto-fit,minmax(220px,1fr));
            gap:20px;
        }

        .quick-box{
            background:white;
            border-radius:20px;
            padding:30px;
            text-align:center;
            text-decoration:none;
            transition:0.4s;
            color:#1e293b;
            box-shadow:0 10px 20px rgba(0,0,0,0.05);
        }

        .quick-box:hover{
            transform:translateY(-6px);
            text-decoration:none;
            color:#2563eb;
        }

        .quick-box i{
            font-size:42px;
            margin-bottom:18px;
            color:#2563eb;
        }

        .quick-box h4{
            font-size:18px;
            font-weight:600;
        }

        /* ACTIVITY */

        .activity-box{
            background:white;
            border-radius:22px;
            padding:30px;
            margin-top:35px;
            box-shadow:0 10px 20px rgba(0,0,0,0.05);
        }

        .activity-item{
            display:flex;
            align-items:center;
            gap:18px;
            padding:18px 0;
            border-bottom:1px solid #e2e8f0;
        }

        .activity-item:last-child{
            border-bottom:none;
        }

        .activity-icon{
            width:55px;
            height:55px;
            border-radius:16px;
            background:#eff6ff;
            color:#2563eb;
            display:flex;
            align-items:center;
            justify-content:center;
            font-size:22px;
        }

        .activity-text h5{
            font-size:16px;
            font-weight:600;
            margin-bottom:4px;
        }

        .activity-text p{
            margin:0;
            color:#64748b;
            font-size:13px;
        }

        /* RESPONSIVE */

        @media(max-width:900px){

            .main-wrapper{
                margin-left:0;
                padding:15px;
                margin-top:20px;
            }

            .welcome-banner{
                padding:30px;
            }

            .welcome-banner h1{
                font-size:30px;
            }
        }

    </style>

</head>

<body>

<form id="form1" runat="server">

    <!-- ADMIN MENU -->
    <uc1:WebUserControl1 ID="WebUserControl11"
        runat="server" />

    <!-- MAIN -->
    <div class="main-wrapper">

        <!-- WELCOME -->
        <div class="welcome-banner">

            <h1>
                Welcome <asp:Label ID="lblUserName" runat="server">Admin</asp:Label> 🌾
            </h1>

            <p>
                Rice Mill Management Dashboard
            </p>

        </div>

        <!-- STATISTICS -->
        <div class="row">

            <!-- CARD 1 -->
            <div class="col-md-3">

                <div class="dashboard-card">

                    <div class="dashboard-icon bg-blue">
                        <i class="fas fa-seedling"></i>
                    </div>

                    <h2><asp:Label ID="lblRiceStock" runat="server">0</asp:Label></h2>

                    <p>Total Rice Stock (KG)</p>

                </div>

            </div>

            <!-- CARD 2 -->
            <div class="col-md-3">

                <div class="dashboard-card">

                    <div class="dashboard-icon bg-green">
                        <i class="fas fa-boxes"></i>
                    </div>

                    <h2><asp:Label ID="lblPaddyStock" runat="server">0</asp:Label></h2>

                    <p>Total Paddy Stock (KG)</p>

                </div>

            </div>

            <!-- CARD 3 -->
            <div class="col-md-3">

                <div class="dashboard-card">

                    <div class="dashboard-icon bg-orange">
                        <i class="fas fa-shopping-cart"></i>
                    </div>

                    <h2><asp:Label ID="lblTotalSales" runat="server">0</asp:Label></h2>

                    <p>Total Sales</p>

                </div>

            </div>

            <!-- CARD 4 -->
            <div class="col-md-3">

                <div class="dashboard-card">

                    <div class="dashboard-icon bg-purple">
                        <i class="fas fa-rupee-sign"></i>
                    </div>

                    <h2><asp:Label ID="lblRevenue" runat="server">10</asp:Label></h2>

                    <p>Total Revenue Received</p>

                </div>

            </div>

        </div>

        <!-- QUICK LINKS -->
        <h2 class="section-title">
            Quick Access
        </h2>

        <div class="quick-links">

            <a href="RiceStock.aspx"
                class="quick-box">

                <i class="fas fa-warehouse"></i>

                <h4>Rice Stock</h4>

            </a>

            <a href="PaddyStock.aspx"
                class="quick-box">

                <i class="fas fa-box"></i>

                <h4>Paddy Stock</h4>

            </a>

            <a href="SalePurchaseExpense.aspx"
                class="quick-box">

                <i class="fas fa-chart-line"></i>

                <h4>Sale Purchase</h4>

            </a>

            <a href="EditData.aspx"
                class="quick-box">

                <i class="fas fa-edit"></i>

                <h4>Edit Data</h4>

            </a>

        </div>

        <!-- RECENT ACTIVITY -->
        <div class="activity-box">

            <h2 class="section-title">
                Recent Activity
            </h2>

            <asp:Repeater ID="rptActivity" runat="server">
                <ItemTemplate>
                    <div class="activity-item">

                        <div class="activity-icon">
                            <i class="fas fa-check"></i>
                        </div>

                        <div class="activity-text">

                            <h5>
                                <%# Eval("ActivityText") %>
                            </h5>

                            <p>
                                <%# Eval("ActivityDate", "{0:dd-MMM-yyyy}") %>
                            </p>

                        </div>

                    </div>
                </ItemTemplate>
            </asp:Repeater>

            <asp:Panel ID="pnlNoActivity" runat="server" Visible="false">
                <div class="activity-item">
                    <div class="activity-text">
                        <p>Abhi tak koi activity nahi hui hai.</p>
                    </div>
                </div>
            </asp:Panel>

        </div>

    </div>

</form>

</body>
</html>