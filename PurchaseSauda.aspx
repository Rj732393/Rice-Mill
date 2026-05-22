<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="PurchaseSauda.aspx.cs"
    Inherits="PurchaseSauda" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Purchase Sauda - Rashmi Rice Mills</title>

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

    <!-- JQuery -->

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>

    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>

    <!-- JQuery UI -->

    <link href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/base/jquery-ui.css"
        rel="stylesheet" />

    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>

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
            max-width:1400px;
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
            z-index:9;
        }

        .form-control,
        select{
            height:56px !important;
            border-radius:16px !important;
            border:1px solid #dbe2ea !important;
            padding-left:50px !important;
            font-size:15px !important;
            box-shadow:none !important;
            width:100%;
        }

        .form-control:focus,
        select:focus{
            border-color:#f59e0b !important;
            box-shadow:0 0 0 4px rgba(245,158,11,0.12) !important;
        }

        /* BUTTON */

        .btn-save{
            background:linear-gradient(90deg,#f59e0b,#ea580c);
            color:white !important;
            border:none;
            height:50px;
            padding:0 30px;
            border-radius:14px;
            font-size:16px;
            font-weight:700;
            transition:0.3s;
            box-shadow:0 8px 20px rgba(234,88,12,0.22);
            display:inline-flex;
            align-items:center;
            justify-content:center;
        }

        .btn-save:hover{
            background:linear-gradient(90deg,#ea580c,#dc2626);
            color:white !important;
        }

        .table-box{
            background:#fff;
            border-radius:18px;
            padding:20px;
            margin-top:30px;
            box-shadow:0 5px 18px rgba(0,0,0,0.06);
            overflow:auto;
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
                padding:25px;
            }

            .form-title h2{
                font-size:30px;
            }

        }

    </style>

    <script type="text/javascript">

        $(document).ready(function () {
            SearchText();
        });

        function SearchText() {

            $("#txtEmpName").autocomplete({

                source: function (request, response) {

                    $.ajax({

                        type: "POST",

                        contentType: "application/json; charset=utf-8",

                        url: "PurchaseSauda.aspx/GetEmployeeName",

                        data: "{'empName':'" + document.getElementById('txtEmpName').value + "'}",

                        dataType: "json",

                        success: function (data) {

                            response(data.d);

                        }
                    });
                }
            });
        }

    </script>

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

                <a class="navbar-brand" href="Home.aspx">

                    Rashmi Rice Mills

                </a>

            </div>

            <ul class="nav navbar-nav navbar-right">

                <li><a href="Home.aspx">Home</a></li>

                <li><a href="PurchaseSauda.aspx">Purchase Sauda</a></li>

                <li><a href="PurchaseReport.aspx">Reports</a></li>

                <li><a href="#footer">Contact</a></li>

            </ul>

        </div>

    </nav>


    <!-- HERO -->

    <div class="hero">

        <div class="container">

            <h1>

                Smart Purchase Sauda Management

            </h1>

            <p>

                Manage paddy purchase entries,
                suppliers, sauda records and
                purchase reports from one dashboard.

            </p>

        </div>

    </div>


    <!-- FORM SECTION -->

    <div class="container-fluid form-section"
        style="padding:0 40px;">

        <div class="form-box">

            <div class="form-title">

                <h2>Purchase Sauda Entry</h2>

                <p>

                    Enter paddy purchase details

                </p>

            </div>

            <!-- ROW 1 -->

            <div class="row">

                <div class="col-md-4">

                    <div class="input-group-custom">

                        <label>Select Date</label>

                        <div class="input-box">

                            <i class="fa fa-calendar"></i>

                            <input id="sdate"
                                runat="server"
                                class="form-control" />

                        </div>

                    </div>

                </div>

                <div class="col-md-4">

                    <div class="input-group-custom">

                        <label>Manual Sauda No</label>

                        <div class="input-box">

                            <i class="fa fa-file"></i>

                            <input id="MPNo"
                                runat="server"
                                type="text"
                                class="form-control" />

                        </div>

                    </div>

                </div>

                <div class="col-md-4">

                    <div class="input-group-custom">

                        <label>Supplier Ref.</label>

                        <div class="input-box">

                            <i class="fa fa-user"></i>

                            <asp:TextBox ID="txtEmpName"
                                runat="server"
                                CssClass="form-control">
                            </asp:TextBox>

                        </div>

                    </div>

                </div>

            </div>


            <!-- ROW 2 -->

            <div class="row">

                <div class="col-md-6">

                    <div class="input-group-custom">

                        <label>Party Name</label>

                        <div class="input-box">

                            <i class="fa fa-users"></i>

                            <asp:DropDownList ID="sPartyName"
                                runat="server"
                                CssClass="form-control"
                                AutoPostBack="true"
                                onselectedindexchanged="sPartyName_SelectedIndexChanged">
                            </asp:DropDownList>

                        </div>

                    </div>

                </div>

                <div class="col-md-6"
                    style="padding-top:38px;">

                    <asp:LinkButton ID="lBtnSaudaParty"
                        runat="server"
                        CssClass="btn btn-save"
                        onclick="lBtnSaudaParty_Click">

                        Sauda List

                    </asp:LinkButton>

                </div>

            </div>


            <!-- OTHER PARTY PANEL -->

            <asp:Panel ID="Panel1"
                runat="server">

                <div class="row">

                    <div class="col-md-6">

                        <div class="input-group-custom">

                            <label>Party Name</label>

                            <div class="input-box">

                                <i class="fa fa-user"></i>

                                <input id="pName"
                                    runat="server"
                                    type="text"
                                    class="form-control" />

                            </div>

                        </div>

                    </div>

                    <div class="col-md-6">

                        <div class="input-group-custom">

                            <label>Party Mobile No.</label>

                            <div class="input-box">

                                <i class="fa fa-phone"></i>

                                <input id="pMN"
                                    runat="server"
                                    class="form-control" />

                            </div>

                        </div>

                    </div>

                </div>

            </asp:Panel>


            <!-- PADDY SECTION -->

            <div class="row">

                <div class="col-md-4">

                    <div class="input-group-custom">

                        <label>Paddy Type</label>

                        <div class="input-box">

                            <i class="fa fa-seedling"></i>

                            <select id="sPaddyType"
                                runat="server"
                                class="form-control">

                                <option>Rupali</option>
                                <option>Mansuri</option>
                                <option>Sonam</option>
                                <option>Hybrid</option>

                            </select>

                        </div>

                    </div>

                </div>

                <div class="col-md-4">

                    <div class="input-group-custom">

                        <label>Quantity (KG)</label>

                        <div class="input-box">

                            <i class="fa fa-weight-hanging"></i>

                            <input id="QIKG"
                                runat="server"
                                class="form-control" />

                        </div>

                    </div>

                </div>

                <div class="col-md-4">

                    <div class="input-group-custom">

                        <label>Rate (₹)</label>

                        <div class="input-box">

                            <i class="fa fa-indian-rupee-sign"></i>

                            <input id="avgrate"
                                runat="server"
                                class="form-control" />

                        </div>

                    </div>

                </div>

            </div>


            <!-- BUTTONS -->

            <div class="text-center"
                style="margin-top:20px;">

                <input type="submit"
                    id="btnContinue"
                    value="Add Purchase Data"
                    runat="server"
                    class="btn btn-save"
                    onserverclick="btnContinue_ServerClick" />

                &nbsp;&nbsp;

                <input type="submit"
                    id="btnSave"
                    value="Save Purchase Sauda"
                    runat="server"
                    class="btn btn-save"
                    onserverclick="btnSave_ServerClick" />

                &nbsp;&nbsp;

                <input type="submit"
                    id="Submit1"
                    value="Reset"
                    runat="server"
                    class="btn btn-save"
                    onserverclick="Submit1_ServerClick" />

            </div>


            <!-- DATA TABLE -->

            <div class="table-box"
                id="prntContent">

                <asp:PlaceHolder ID="DBDataPlaceHolder"
                    runat="server">
                </asp:PlaceHolder>

            </div>

        </div>

    </div>


    <!-- FOOTER -->

    <div class="footer"
        id="footer">

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

                        Purchase Sauda
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

                © 2026 Rashmi Rice Mills.
                All Rights Reserved.

            </div>

        </div>

    </div>

</form>

</body>

</html>