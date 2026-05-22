
<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="Sale.aspx.cs"
    Inherits="Sale" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Sale Entry - Rashmi Rice Mills</title>

    <meta name="viewport"
        content="width=device-width, initial-scale=1" />

    <!-- Bootstrap -->

    <link rel="stylesheet"
        href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css" />

    <!-- Font Awesome -->

    <link rel="stylesheet"
        href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />

    <!-- Google Font -->

    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700;800&display=swap"
        rel="stylesheet" />

    <style>

        *{
            margin:0;
            padding:0;
            box-sizing:border-box;
        }

        body{
            background:#eef2f7;
            font-family:'Poppins',sans-serif;
        }

        /* TOPBAR */

        .topbar{
            background:linear-gradient(90deg,#1e293b,#334155);
            color:white;
            padding:12px 0;
            font-size:14px;
            font-weight:500;
        }

        .topbar i{
            color:#f59e0b;
        }

        /* NAVBAR */

        .navbar-custom{
            background:white;
            border:none;
            border-radius:0;
            margin-bottom:0;
            padding:14px 0;
            box-shadow:0 4px 18px rgba(0,0,0,0.06);
        }

        .navbar-brand{
            font-size:32px;
            font-weight:800;
            color:#f59e0b !important;
        }

        .navbar-nav > li > a{
            color:#1e293b !important;
            font-size:15px;
            font-weight:600;
            padding:14px 18px !important;
        }

        .navbar-nav > li > a:hover{
            color:#f59e0b !important;
        }

        /* HERO */

        .hero{
            background:
            linear-gradient(rgba(15,23,42,0.75),rgba(15,23,42,0.75)),
            url('https://images.unsplash.com/photo-1586201375761-83865001e31c?q=80&w=1974&auto=format&fit=crop');

            background-size:cover;
            background-position:center;
            padding:100px 0;
            color:white;
            text-align:center;
        }

        .hero h1{
            font-size:60px;
            font-weight:800;
            margin-bottom:20px;
        }

        .hero p{
            font-size:20px;
            color:#e2e8f0;
            max-width:800px;
            margin:auto;
            line-height:38px;
        }

        /* FORM SECTION */

        .form-section{
            margin-top:-60px;
            margin-bottom:60px;
        }

       .form-box{
    background:white;
    border-radius:28px;
    padding:45px 55px;
    box-shadow:0 12px 40px rgba(0,0,0,0.08);
    width:100%;
    max-width:1350px;
    margin:auto;
}
        .form-title{
            text-align:center;
            margin-bottom:40px;
        }

        .form-title h2{
            font-size:42px;
            font-weight:800;
            color:#0f172a;
        }

        .form-title p{
            color:#64748b;
            font-size:17px;
            margin-top:10px;
        }

        /* FORM */

        .input-group-custom{
            margin-bottom:28px;
        }

        .input-group-custom label{
            display:block;
            margin-bottom:10px;
            font-size:15px;
            font-weight:700;
            color:#334155;
        }

        .input-box{
            position:relative;
        }

        .input-box i{
            position:absolute;
            left:18px;
            top:17px;
            color:#94a3b8;
            font-size:16px;
        }

        .form-control{
            height:56px;
            border-radius:16px;
            border:1px solid #dbe2ea;
            padding-left:50px;
            font-size:15px;
            box-shadow:none;
            transition:0.3s;
        }

        .form-control:focus{
            border-color:#f59e0b;
            box-shadow:0 0 0 4px rgba(245,158,11,0.12);
        }

        /* BUTTON */

        .btn-save{
    background:linear-gradient(90deg,#f59e0b,#ea580c);
    color:white !important;
    border:none;
    height:50px;
    width:220px;
    border-radius:14px;
    font-size:16px;
    font-weight:700;
    letter-spacing:0.5px;
    transition:all 0.3s ease;
    margin-top:15px;
    box-shadow:0 8px 20px rgba(234,88,12,0.22);
}

.btn-save:hover{
    background:linear-gradient(90deg,#ea580c,#dc2626);
    transform:translateY(-2px);
    color:white !important;
}

.btn-save:focus{
    outline:none !important;
    color:white !important;
}
        /* INFO CARDS */

        .info-card{
            background:white;
            border-radius:22px;
            padding:30px;
            text-align:center;
            margin-top:25px;
            box-shadow:0 5px 20px rgba(0,0,0,0.06);
            transition:0.3s;
        }

        .info-card:hover{
            transform:translateY(-6px);
        }

        .info-card i{
            width:75px;
            height:75px;
            line-height:75px;
            border-radius:50%;
            background:#fff7ed;
            color:#f59e0b;
            font-size:28px;
            margin-bottom:18px;
        }

        .info-card h3{
            font-size:22px;
            font-weight:700;
            color:#0f172a;
            margin-bottom:10px;
        }

        .info-card p{
            color:#64748b;
            line-height:28px;
            font-size:15px;
        }

        /* FOOTER */

        .footer{
            background:#0f172a;
            color:white;
            padding:60px 0 30px;
            margin-top:60px;
        }

        .footer h3{
            color:#f59e0b;
            font-size:28px;
            font-weight:800;
            margin-bottom:20px;
        }

        .footer p{
            color:#cbd5e1;
            line-height:30px;
            font-size:15px;
        }

        .footer-bottom{
            text-align:center;
            margin-top:35px;
            padding-top:20px;
            border-top:1px solid rgba(255,255,255,0.08);
            color:#94a3b8;
        }

        /* MOBILE */

        @media(max-width:768px){

            .hero{
                padding:80px 0;
            }

            .hero h1{
                font-size:38px;
                line-height:52px;
            }

            .hero p{
                font-size:16px;
                line-height:30px;
            }

            .form-box{
                padding:28px;
            }

            .form-title h2{
                font-size:32px;
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

                <a class="navbar-brand" href="#">

                    Rashmi Rice Mills

                </a>

            </div>

            <ul class="nav navbar-nav navbar-right">

                <li><a href="#">Home</a></li>

                <li><a href="#">Sale Entry</a></li>

                <li><a href="#">Reports</a></li>

                <li><a href="#">Contact</a></li>

            </ul>

        </div>

    </nav>


    <!-- HERO -->

    <div class="hero">

        <div class="container">

            <h1>

                Smart Sale Management

            </h1>

            <p>

                Manage rice sale entries, dispatch records,
                PMN details and billing operations from
                one modern dashboard.

            </p>

        </div>

    </div>


    <!-- FORM -->

    <div class="container-fluid form-section" style="padding:0 50px;">

        <div class="form-box">

            <div class="form-title">

                <h2>Sale Entry Form</h2>

                <p>

                    Enter sale and dispatch information

                </p>

            </div>


            <div class="row">

                <!-- SAUDA NO -->

                <div class="col-md-4">

                    <div class="input-group-custom">

                        <label>Sauda No</label>

                        <div class="input-box">

                            <i class="fa fa-file"></i>

                            <asp:TextBox ID="SaudaNo"
                                runat="server"
                                CssClass="form-control">
                            </asp:TextBox>

                        </div>

                    </div>

                </div>


                <!-- SAUDA DATE -->

                <div class="col-md-4">

                    <div class="input-group-custom">

                        <label>Sauda Date</label>

                        <div class="input-box">

                            <i class="fa fa-calendar"></i>

                            <asp:TextBox ID="SaudaDate"
                                runat="server"
                                CssClass="form-control">
                            </asp:TextBox>

                        </div>

                    </div>

                </div>


                <!-- DESPATCH -->

                <div class="col-md-4">

                    <div class="input-group-custom">

                        <label>Despatch No</label>

                        <div class="input-box">

                            <i class="fa fa-truck"></i>

                            <asp:TextBox ID="DespatchNo"
                                runat="server"
                                CssClass="form-control">
                            </asp:TextBox>

                        </div>

                    </div>

                </div>

            </div>


            <div class="row">

                <!-- PMN -->

                <div class="col-md-4">

                    <div class="input-group-custom">

                        <label>PMN</label>

                        <div class="input-box">

                            <i class="fa fa-hashtag"></i>

                            <asp:TextBox ID="pMN"
                                runat="server"
                                CssClass="form-control">
                            </asp:TextBox>

                        </div>

                    </div>

                </div>

            </div>


            <!-- BUTTON -->

            <!-- BUTTON -->

<div style="text-align:center; margin-top:5px;">

    <asp:Button ID="btnSave"
        runat="server"
        Text="Save Sale Entry"
        CssClass="btn btn-save"
        OnClick="btnSave_Click" />

</div>

        </div>


        <!-- INFO CARDS -->

        <div class="row">

            <div class="col-md-4">

                <div class="info-card">

                    <i class="fa fa-chart-line"></i>

                    <h3>Smart Reports</h3>

                    <p>

                        Generate sales reports and
                        analytics instantly.

                    </p>

                </div>

            </div>


            <div class="col-md-4">

                <div class="info-card">

                    <i class="fa fa-wallet"></i>

                    <h3>Easy Billing</h3>

                    <p>

                        Fast invoice generation and
                        payment tracking system.

                    </p>

                </div>

            </div>


            <div class="col-md-4">

                <div class="info-card">

                    <i class="fa fa-industry"></i>

                    <h3>Rice Management</h3>

                    <p>

                        Complete rice mill operations
                        in one dashboard.

                    </p>

                </div>

            </div>

        </div>

    </div>


    <!-- FOOTER -->

    <div class="footer">

        <div class="container">

            <div class="row">

                <div class="col-md-4">

                    <h3>Rashmi Rice Mills</h3>

                    <p>

                        Premium rice manufacturer
                        delivering quality products
                        and smart management solutions.

                    </p>

                </div>


                <div class="col-md-4">

                    <h3>Quick Links</h3>

                    <p>

                        Home
                        <br />

                        Sale Entry
                        <br />

                        Reports
                        <br />

                        Contact

                    </p>

                </div>


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

            <div class="footer-bottom">

                © 2026 Rashmi Rice Mills. All Rights Reserved.

            </div>

        </div>

    </div>

</form>

</body>

</html>