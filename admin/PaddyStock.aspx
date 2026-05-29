<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="PaddyStock.aspx.cs"
    Inherits="admin_PaddyStock" %>

<%@ Register Src="../Includes/AdminMenu.ascx"
    TagName="WebUserControl1"
    TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Paddy Stock Report</title>

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

    <!-- Admin Menu CSS -->
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

        body{
            background:#f1f5f9;
            font-family:'Poppins',sans-serif;
            overflow-x:hidden;
        }

        /* MAIN CONTENT */

        .main-wrapper{
            margin-left:270px;
            margin-top:100px;
            padding:30px;
            transition:0.4s;
        }

        /* CARD */

        .dashboard-card{
    background:white;
    border-radius:24px;
    padding:45px;
    max-width:1300px;
    width:100%;
    margin:auto;
    box-shadow:0 12px 35px rgba(0,0,0,0.08);
    animation:fadeIn 0.7s ease;
}
        @keyframes fadeIn{

            from{
                opacity:0;
                transform:translateY(20px);
            }

            to{
                opacity:1;
                transform:translateY(0px);
            }
        }

        /* TITLE */

        .page-title{
            text-align:center;
            margin-bottom:35px;
        }

        .page-title h1{
            font-size:34px;
            font-weight:700;
            color:#1e293b;
            margin-bottom:8px;
        }

        .page-title p{
            color:#64748b;
            font-size:15px;
        }

        /* FORM */

        .form-group-custom{
            margin-bottom:24px;
        }

        .form-group-custom label{
            display:block;
            margin-bottom:8px;
            font-size:15px;
            font-weight:600;
            color:#334155;
        }

        .form-control-custom{
            width:100%;
            height:48px;
            border-radius:12px;
            border:1px solid #cbd5e1;
            padding:10px 14px;
            font-size:15px;
            background:white;
            transition:0.3s;
        }

        .form-control-custom:focus{
            outline:none;
            border-color:#2563eb;
            box-shadow:0 0 10px rgba(37,99,235,0.15);
        }

        /* BUTTON */

        .btn-generate{
            background:linear-gradient(135deg,#2563eb,#06b6d4);
            border:none;
            color:white;
            padding:12px 30px;
            border-radius:12px;
            font-size:15px;
            font-weight:600;
            transition:0.3s;
        }

        .btn-generate:hover{
            opacity:0.9;
            transform:translateY(-2px);
        }

        /* TABLE */

        .report-table{
            background:white;
            border-radius:18px;
            padding:20px;
            margin-top:25px;
            box-shadow:0 8px 20px rgba(0,0,0,0.05);
        }

        .report-table table{
            width:100%;
            border-collapse:collapse;
        }

        .report-table table th{
            background:#2563eb;
            color:white;
            padding:12px;
            text-align:center;
        }

        .report-table table td{
            padding:10px;
            border-bottom:1px solid #e2e8f0;
            text-align:center;
        }

        .report-table table tr:hover{
            background:#f8fafc;
        }

        /* RESPONSIVE */

        @media(max-width:900px){

            .main-wrapper{
                margin-left:0;
                margin-top:20px;
                padding:15px;
            }

            .dashboard-card{
                padding:25px;
            }

            .page-title h1{
                font-size:26px;
            }
        }

    </style>

</head>

<body>

<form id="form1" runat="server">

    <!-- ADMIN MENU -->
    <uc1:WebUserControl1 ID="WebUserControl11"
        runat="server" />

    <!-- MAIN CONTENT -->
    <div class="main-wrapper">

        <!-- CARD -->
        <div class="dashboard-card">

            <!-- TITLE -->
            <div class="page-title">

                <h1>

                    <i class="fas fa-boxes"
                        style="color:#2563eb;"></i>

                    Paddy Stock Report

                </h1>

                <p>
                    Rashmi Rice Mills Private Limited
                </p>

            </div>

            <!-- FORM -->
            <div class="row">

                <!-- FROM DATE -->
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

                    </div>

                </div>

                <!-- TO DATE -->
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

                    </div>

                </div>

            </div>

            <!-- REPORT TYPE -->
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

                        </select>

                    </div>

                </div>

            </div>

            <!-- BUTTON -->
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

        <!-- REPORT TABLE -->
        <div class="report-table table-responsive">

            <asp:PlaceHolder ID="DBDataPlaceHolder"
                runat="server">
            </asp:PlaceHolder>

        </div>

    </div>

</form>

<!-- DATE PICKER -->

<script type="text/javascript">

    $(function () {

        $("#fdate").datepicker({
            dateFormat: "dd/mm/yy",
            changeMonth: true,
            changeYear: true
        });

        $("#tdate").datepicker({
            dateFormat: "dd/mm/yy",
            changeMonth: true,
            changeYear: true
        });

    });

</script>

</body>
</html>