<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="SalePurchaseExpense.aspx.cs"
    Inherits="admin_SalePurchaseExpense" %>

<%@ Register Src="../Includes/AdminMenu.ascx"
    TagName="WebUserControl1"
    TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Sale Purchase Expense Report</title>

    <meta name="viewport"
        content="width=device-width, initial-scale=1, maximum-scale=1" />

    <!-- Bootstrap -->
    <link rel="stylesheet"
        href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />

    <!-- Font Awesome -->
    <link rel="stylesheet"
        href="https://use.fontawesome.com/releases/v5.7.2/css/all.css" />

    <!-- Google Font -->
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap"
        rel="stylesheet" />

    <!-- ADMIN MENU CSS -->
    <link href="../CSS/AdminMenu.css"
        rel="stylesheet"
        type="text/css" />

    <!-- JQuery -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>

    <!-- Bootstrap -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

    <!-- Date Picker -->
    <link rel="stylesheet"
        href="https://code.jquery.com/ui/1.13.2/themes/base/jquery-ui.css" />

    <script src="https://code.jquery.com/ui/1.13.2/jquery-ui.js"></script>

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

        .main-wrapper{
            margin-left:270px;
            margin-top:95px;
            padding:35px;
            transition:0.4s;
        }

        .main-wrapper.full{
            margin-left:0;
        }

        .dashboard-card{
            background:white;
            border-radius:25px;
            padding:50px;
            max-width:1400px;
            width:100%;
            margin:auto;
            box-shadow:0 12px 35px rgba(0,0,0,0.08);
            animation:fadeIn 0.7s ease;
        }

        @keyframes fadeIn{
            from{ opacity:0; transform:translateY(20px); }
            to{ opacity:1; transform:translateY(0px); }
        }

        .page-title{
            text-align:center;
            margin-bottom:40px;
        }

        .page-title h1{
            font-size:38px;
            font-weight:700;
            color:#1e293b;
            margin-bottom:10px;
        }

        .page-title p{
            color:#64748b;
            font-size:16px;
        }

        .form-group-custom{
            margin-bottom:28px;
        }

        .form-group-custom label{
            display:block;
            margin-bottom:10px;
            font-size:15px;
            font-weight:600;
            color:#334155;
        }

        .form-control-custom{
            width:100%;
            height:52px;
            border-radius:14px;
            border:1px solid #cbd5e1;
            padding:12px 16px;
            font-size:15px;
            background:white;
            transition:0.3s;
        }

        .form-control-custom:focus{
            outline:none;
            border-color:#2563eb;
            box-shadow:0 0 12px rgba(37,99,235,0.15);
        }

        .btn-generate{
            background:linear-gradient(135deg,#2563eb,#06b6d4);
            border:none;
            color:white;
            padding:14px 35px;
            border-radius:14px;
            font-size:16px;
            font-weight:600;
            transition:0.3s;
            box-shadow:0 8px 20px rgba(37,99,235,0.25);
            width:auto;
        }

        .btn-generate:hover{
            transform:translateY(-2px);
        }

        .report-table{
            background:white;
            border-radius:20px;
            padding:25px;
            margin-top:30px;
            box-shadow:0 8px 20px rgba(0,0,0,0.05);
            overflow-x:auto;
            -webkit-overflow-scrolling:touch;
        }

        .report-table table{
            width:100%;
            border-collapse:collapse;
            min-width:900px;
        }

        .report-table table th{
            background:#2563eb;
            color:white;
            padding:14px;
            text-align:center;
            white-space:nowrap;
        }

        .report-table table td{
            padding:12px;
            border-bottom:1px solid #e2e8f0;
            text-align:center;
            white-space:nowrap;
        }

        .report-table table tr:hover{
            background:#f8fafc;
        }

        .menu-toggle{
            position:fixed;
            top:22px;
            left:20px;
            width:45px;
            height:45px;
            border-radius:10px;
            background:#2563eb;
            color:white;
            border:none;
            font-size:20px;
            z-index:2000;
            box-shadow:0 8px 20px rgba(0,0,0,0.15);
        }

        .sidebar-overlay{
            display:none;
            position:fixed;
            inset:0;
            background:rgba(15,23,42,0.5);
            z-index:1500;
        }

        .sidebar-overlay.show{
            display:block;
        }

        @media(max-width:900px){

            #sidebarArea{
                position:fixed;
                top:0;
                left:0;
                height:100%;
                z-index:1600;
                transform:translateX(-100%);
                transition:transform 0.35s ease;
                display:block !important;
            }

            #sidebarArea.show{
                transform:translateX(0);
            }

            .main-wrapper{
                margin-left:0 !important;
                margin-top:80px;
                padding:15px;
            }

            .dashboard-card{
                padding:22px;
                border-radius:18px;
            }

            .page-title h1{
                font-size:24px;
            }

            .page-title p{
                font-size:14px;
            }

            .form-control-custom{
                height:48px;
                font-size:14px;
            }

            .btn-generate{
                width:100%;
                padding:14px 0;
            }

            .report-table{
                padding:14px;
                border-radius:14px;
            }

            .report-table table th,
            .report-table table td{
                padding:9px;
                font-size:13px;
            }

            .menu-toggle{
                top:16px;
                left:14px;
                width:42px;
                height:42px;
            }
        }

        @media(max-width:480px){

            .page-title h1{
                font-size:20px;
            }

            .dashboard-card{
                padding:16px;
            }
        }

    </style>

</head>

<body>

<form id="form1" runat="server">

    <button type="button"
        class="menu-toggle"
        onclick="toggleSidebar()">

        <i class="fas fa-bars"></i>

    </button>

    <div class="sidebar-overlay"
        id="sidebarOverlay"
        onclick="toggleSidebar()"></div>

    <div id="sidebarArea">

        <uc1:WebUserControl1 ID="WebUserControl11"
            runat="server" />

    </div>

    <div class="main-wrapper"
        id="mainContent">

        <div class="dashboard-card">

            <div class="page-title">

                <h1>

                    <i class="fas fa-chart-line"
                        style="color:#2563eb;"></i>

                    Sale, Purchase & Expense Report

                </h1>

                <p>
                    <asp:Label ID="lblCompanyName" runat="server" Text="Rice Mills"></asp:Label>
                </p>

            </div>

            <div class="row">

                <div class="col-md-6">

                    <div class="form-group-custom">

                        <label>

                            <i class="fas fa-calendar-alt"></i>

                            From Date

                        </label>

                        <input type="text"
                            id="fdate"
                            name="sdate"
                            runat="server"
                            class="form-control-custom"
                            placeholder="dd/mm/yyyy" />
                            <asp:Label ID="lblFromDateError"
    runat="server"
    ForeColor="Red"></asp:Label>
                    </div>

                </div>

                <div class="col-md-6">

                    <div class="form-group-custom">

                        <label>

                            <i class="fas fa-calendar-check"></i>

                            To Date

                        </label>

                        <input type="text"
                            id="tdate"
                            name="tdate"
                            runat="server"
                            class="form-control-custom"
                            placeholder="dd/mm/yyyy" />
                            <asp:Label ID="lblToDateError"
    runat="server"
    ForeColor="Red"></asp:Label>
                    </div>

                </div>

            </div>

            <div class="row">

                <div class="col-md-12">

                    <div class="form-group-custom">

                        <label>

                            <i class="fas fa-chart-bar"></i>

                            Report Type

                        </label>

                        <select id="srType"
                            runat="server"
                            class="form-control-custom">

                            <option>Daily</option>
                            <option>Monthly</option>
                            <option>Annual</option>

                        </select>

                    </div>

                </div>

            </div>

            <div class="row">

                <div class="col-md-12 text-center">

                    <input type="submit"
                        id="btnReport"
                        value="Generate Report"
                        runat="server"
                        class="btn-generate"
                        onserverclick="btnReport_ServerClick" />

                </div>

            </div>

        </div>

        <div class="report-table table-responsive">

            <asp:PlaceHolder ID="DBDataPlaceHolder"
                runat="server">
            </asp:PlaceHolder>

        </div>

    </div>

</form>

<script type="text/javascript">

    $(function () {

        $("#fdate").datepicker({
            dateFormat: "dd/mm/yy",
            changeMonth: true,
            changeYear: true,
            yearRange: "2000:2050",
            showButtonPanel: true
        });

        $("#tdate").datepicker({
            dateFormat: "dd/mm/yy",
            changeMonth: true,
            changeYear: true,
            yearRange: "2000:2050",
            showButtonPanel: true
        });

        $("#fdate").attr("autocomplete", "off");
        $("#tdate").attr("autocomplete", "off");

    });

    function toggleSidebar() {

        var sidebar = document.getElementById("sidebarArea");
        var main = document.getElementById("mainContent");
        var overlay = document.getElementById("sidebarOverlay");

        var isMobile = window.innerWidth <= 900;

        if (isMobile) {
            sidebar.classList.toggle("show");
            overlay.classList.toggle("show");
        } else {
            if (sidebar.style.display === "none") {
                sidebar.style.display = "block";
                main.classList.remove("full");
            } else {
                sidebar.style.display = "none";
                main.classList.add("full");
            }
        }
    }

    window.addEventListener("resize", function () {
        var sidebar = document.getElementById("sidebarArea");
        var overlay = document.getElementById("sidebarOverlay");
        if (window.innerWidth > 900) {
            sidebar.classList.remove("show");
            overlay.classList.remove("show");
            sidebar.style.display = "block";
        }
    });

</script>

</body>
</html>