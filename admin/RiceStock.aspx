<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="RiceStock.aspx.cs"
    Inherits="admin_RiceStock" %>

<%@ Register Src="../Includes/AdminMenu.ascx"
    TagName="WebUserControl1"
    TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Rice Stock Report</title>

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
<<<<<<< HEAD

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

        .main-wrapper{
            margin-left:270px;
            margin-top:100px;
            padding:30px;
        }

        .dashboard-card{
            background:white;
            border-radius:25px;
            padding:40px;
            box-shadow:0 10px 30px rgba(0,0,0,0.08);
        }

        .page-title{
            text-align:center;
            margin-bottom:40px;
        }

        .page-title h1{
            font-size:42px;
            font-weight:700;
            color:#1e293b;
        }

        .page-title p{
            color:#64748b;
            font-size:16px;
        }

        .form-group-custom{
    margin-bottom:18px;
}

        .form-group-custom label{
            font-size:15px;
            font-weight:600;
            color:#334155;
            margin-bottom:10px;
            display:block;
        }

        .form-control-custom{
    width:100%;
    height:42px;
    border-radius:10px;
    border:1px solid #cbd5e1;
    padding:6px 12px;
    font-size:14px;
    background:white;
}

        .form-control-custom:focus{
            outline:none;
            border-color:#2563eb;
            box-shadow:0 0 10px rgba(37,99,235,0.2);
        }

        .btn-generate{
            background:linear-gradient(135deg,#2563eb,#06b6d4);
            border:none;
            color:white;
            padding:14px 35px;
            border-radius:14px;
            font-size:16px;
            font-weight:600;
        }

        .btn-generate:hover{
            opacity:0.9;
        }

        .report-table{
            margin-top:30px;
            background:white;
            border-radius:20px;
            padding:20px;
            box-shadow:0 8px 20px rgba(0,0,0,0.05);
        }

        @media(max-width:900px){

            .main-wrapper{
                margin-left:0;
                margin-top:20px;
            }
        }

    </style>

=======
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css"/>
    <%--<link href="../CSS/Menu.css" rel="stylesheet" type="text/css" />--%>
>>>>>>> 142e84a7cc553931f4021ab129795d4c5cb79082
</head>

<body>
<<<<<<< HEAD

<form id="form1" runat="server">

    <!-- ADMIN MENU -->
    <uc1:WebUserControl1 ID="WebUserControl11"
        runat="server" />

    <!-- MAIN -->
    <div class="main-wrapper">

        <div class="dashboard-card">

            <!-- TITLE -->
            <div class="page-title">

                <h1>
                    <i class="fas fa-warehouse"
                        style="color:#2563eb;"></i>

                    Rice Stock Report
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

        <!-- TABLE -->
        <div class="report-table table-responsive">

            <asp:PlaceHolder ID="DBDataPlaceHolder"
                runat="server">
            </asp:PlaceHolder>

        </div>

=======
    <form id="form1" runat="server">
    <div class="header"></div>
    <input type="checkbox" class="openSidebarMenu" id="openSidebarMenu">
  <label for="openSidebarMenu" class="sidebarIconToggle">
    <div class="spinner diagonal part-1"></div>
    <div class="spinner horizontal"></div>
    <div class="spinner diagonal part-2"></div>
  </label>
  <div id="sidebarMenu">
    <uc1:WebUserControl1 ID="WebUserControl11" runat="server" /> 
  </div>
    <div class="container">
  <div id='center' class="main center">
    <div class="mainInner">
    
       <h2><span>Rashmi Rice Mills Private Limited</span>
      <br />Rice Stock Report</h2>
      <div class="row" style="text-align:right !important;">
      <span style="font-weight:bold; color:Maroon;">Welcome Admin</span>
      </div>
      
  <div class="row">
      <div class="col-25">
        <label for="fdate">From Date</label>
      </div>
      <div class="col-25">
        <input id="fdate" name="sdate" runat="server"/>
      </div>
      <div class="col-25">
        <label for="tdate">To Date</label>
      </div>
      <div class="col-25">
        <input id="tdate" name="tdate" runat="server"/>
      </div>
>>>>>>> 142e84a7cc553931f4021ab129795d4c5cb79082
    </div>

</form>

<script type="text/javascript">

    $(function () {

        $("#fdate").datepicker({
            dateFormat: "dd/mm/yy"
        });

        $("#tdate").datepicker({
            dateFormat: "dd/mm/yy"
        });

    });

</script>

</body>
</html>