<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseReport.aspx.cs" Inherits="PurchaseReport" %>

<%@ Register Src="~/Includes/menu.ascx"
    TagName="Menu"
    TagPrefix="uc1" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Purchase Report - Rashmi Rice Mills</title>

    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Bootstrap -->

    <link rel="stylesheet"
        href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css" />

    <!-- JQuery -->

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>

    <!-- Bootstrap JS -->

    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>

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
    overflow-x:hidden;
}


/* ===== MAIN ===== */

.main-content{
    margin-left:120px;
    padding-top:130px;
    transition:0.3s;
}

.main-content.full{
    margin-left:0;
}

/* ===== REPORT SECTION ===== */

.report-wrapper{
    padding:30px;
}

.report-box{
    background:white;
    border-radius:28px;
    padding:45px;
    box-shadow:0 12px 40px rgba(0,0,0,0.08);
}

.report-title{
    text-align:center;
    margin-bottom:40px;
}

.report-title h2{
    font-size:42px;
    font-weight:800;
    color:#0f172a;
}

.report-title p{
    color:#64748b;
    font-size:16px;
}

/* ===== INPUT ===== */

.input-group-custom{
    margin-bottom:28px;
}

.input-group-custom label{
    display:block;
    margin-bottom:7px;
    font-size:16px;
    font-weight:700;
    color:#334155;
}

.input-box{
    position:relative;
}

.input-box i{
    position:absolute;
    top:13px;
    left:14px;
    color:#94a3b8;
    z-index:9;
    font-size:13px;
}

.form-control,
select{
    height:45px !important;
    border-radius:12px !important;
    border:1px solid #dbe2ea !important;
    padding-left:42px !important;
    box-shadow:none !important;
    font-size:14px !important;
}

.form-control:focus{
    border-color:#f59e0b !important;
    box-shadow:0 0 0 4px rgba(245,158,11,0.12) !important;
}

/* ===== BUTTON ===== */

.btn-save{
    background:linear-gradient(90deg,#f59e0b,#ea580c);
    color:white !important;
    border:none !important;
    height:52px;
    padding:0 28px;
    border-radius:14px;
    font-size:16px;
    font-weight:700;
    transition:0.3s;
    box-shadow:0 8px 20px rgba(234,88,12,0.22);
}

.btn-save:hover{
    background:linear-gradient(90deg,#ea580c,#dc2626);
    transform:translateY(-2px);
}

/* ===== TABLE ===== */

.table-box{
    background:white;
    border-radius:20px;
    padding:25px;
    margin-top:35px;
    box-shadow:0 5px 20px rgba(0,0,0,0.06);
    overflow:auto;
}

.table{
    margin-bottom:0 !important;
}

.table th{
    background:#f59e0b;
    color:white;
    border:none !important;
}

.table td{
    vertical-align:middle !important;
}

/* ===== MOBILE ===== */

@media(max-width:768px){

    

    .main-content{
        margin-left:0;
    }

    .report-wrapper{
        padding:15px;
    }

    .report-box{
        padding:25px;
    }

    .report-title h2{
        font-size:30px;
    }

}

</style>

<script>

    function toggleSidebar() {


        $(".main-content").toggleClass("full");

    }

</script>

</head>

<body>

<form id="form1" runat="server">

<uc1:Menu ID="Menu1" runat="server" />




<!-- MAIN CONTENT -->

<div class="main-content">

    <div class="report-wrapper">

        <div class="report-box">

            <div class="report-title">

                <h2>Purchase Report</h2>

                <p>
                    Generate and export purchase reports
                </p>

            </div>

            <!-- FILTER SECTION -->

            <div class="row">

                <div class="col-md-6">

                    <div class="input-group-custom">

                        <label>From Date</label>

                        <div class="input-box">

                            <i class="fa fa-calendar"></i>

                            <input id="fdate"
                                name="fdate"
                                runat="server"
                                required
                                class="form-control" />

                        </div>

                    </div>

                </div>

                <div class="col-md-6">

                    <div class="input-group-custom">

                        <label>To Date</label>

                        <div class="input-box">

                            <i class="fa fa-calendar-days"></i>

                            <input id="tdate"
                                name="tdate"
                                runat="server"
                                required
                                class="form-control" />

                        </div>

                    </div>

                </div>

            </div>

            <!-- PARTY -->

            <div class="row">

                <div class="col-md-12">

                    <div class="input-group-custom">

                        <label>Party Name</label>

                        <div class="input-box">

                            <i class="fa fa-users"></i>

                            <asp:DropDownList ID="sPartyName"
                                runat="server"
                                CssClass="form-control">
                            </asp:DropDownList>

                        </div>

                    </div>

                </div>

            </div>

            <!-- BUTTONS -->

            <div class="text-center"
                style="margin-top:20px;">

                <input type="submit"
                    id="btnContinue"
                    value="Generate Report"
                    runat="server"
                    class="btn btn-save"
                    onserverclick="btnContinue_ServerClick" />

                &nbsp;&nbsp;

                <button type="submit"
                    id="Export"
                    runat="server"
                    class="btn btn-save"
                    onserverclick="Export_ServerClick">

                    Export Excel
                    <i class="fa fa-file-excel"></i>

                </button>

            </div>

            <!-- REPORT TABLE -->

            <div class="table-box table-responsive">

                <asp:PlaceHolder ID="DBDataPlaceHolder"
                    runat="server">
                </asp:PlaceHolder>

            </div>

        </div>

    </div>

</div>

</form>

</body>

</html>