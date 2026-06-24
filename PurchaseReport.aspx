<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseReport.aspx.cs" Inherits="PurchaseReport" %>

<%@ Register Src="~/Includes/menu.ascx"
    TagName="Menu"
    TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Purchase Report - Rashmi Rice Mills</title>

    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700;800&display=swap" rel="stylesheet" />

<style>

*{ margin:0; padding:0; box-sizing:border-box; }

body{
    background:#eef2f7;
    font-family:'Poppins',sans-serif;
    overflow-x:hidden;
}

.main-content{
    margin-left:120px;
    padding-top:130px;
    transition:0.3s;
}

.main-content.full{ margin-left:0; }

.report-wrapper{ padding:30px; }

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

.input-group-custom{ margin-bottom:28px; }

.input-group-custom label{
    display:block;
    margin-bottom:7px;
    font-size:15px;
    font-weight:700;
    color:#334155;
}

.input-box{ position:relative; }

.input-box i{
    position:absolute;
    top:14px;
    left:14px;
    color:#94a3b8;
    z-index:9;
    font-size:13px;
}

.form-control, select{
    height:46px !important;
    border-radius:12px !important;
    border:1px solid #dbe2ea !important;
    padding-left:42px !important;
    box-shadow:none !important;
    font-size:14px !important;
    transition:0.3s;
}

.form-control:focus, select:focus{
    border-color:#16a34a !important;
    box-shadow:0 0 0 4px rgba(22,163,74,0.12) !important;
}

/* Validation */
.form-control.is-invalid, select.is-invalid{
    border-color:#dc2626 !important;
    box-shadow:0 0 0 3px rgba(220,38,38,0.15) !important;
}

.err-msg{
    color:#dc2626;
    font-size:12px;
    margin-top:5px;
    display:none;
}

/* Alert */
.alert-custom{
    border-radius:12px;
    padding:14px 18px;
    margin-bottom:20px;
    font-size:13px;
    font-weight:600;
    display:none;
}

.alert-danger-custom{
    background:#fef2f2;
    border:1px solid #fca5a5;
    color:#b91c1c;
}

.alert-success-custom{
    background:#f0fdf4;
    border:1px solid #86efac;
    color:#15803d;
}

/* Buttons */
.btn-save{
    background:linear-gradient(135deg,#16a34a,#15803d);
    color:white !important;
    border:none !important;
    height:50px;
    padding:0 28px;
    border-radius:14px;
    font-size:15px;
    font-weight:700;
    transition:0.3s;
    box-shadow:0 8px 20px rgba(22,163,74,0.22);
    cursor:pointer;
}

.btn-save:hover{
    background:linear-gradient(135deg,#15803d,#166534);
    transform:translateY(-2px);
    color:white !important;
    box-shadow:0 12px 28px rgba(22,163,74,0.35);
}

.btn-export{
    background:linear-gradient(135deg,#0f766e,#0d9488) !important;
}

.btn-export:hover{
    background:linear-gradient(135deg,#115e59,#0f766e) !important;
}

/* Table */
.table-box{
    background:white;
    border-radius:20px;
    padding:25px;
    margin-top:35px;
    box-shadow:0 5px 20px rgba(0,0,0,0.06);
    overflow:auto;
}

.table{ margin-bottom:0 !important; }

.table th{
    background:#16a34a !important;
    color:white;
    border:none !important;
    white-space:nowrap;
}

.table td{
    vertical-align:middle !important;
    white-space:nowrap;
}

/* Totals row */
.total-row td{
    background:#f0fdf4 !important;
    font-weight:700;
    color:#14532d;
    border-top:2px solid #16a34a !important;
}

@media(max-width:768px){
    .main-content{ margin-left:0; }
    .report-wrapper{ padding:15px; }
    .report-box{ padding:20px; }
    .report-title h2{ font-size:28px; }
}

</style>

<script type="text/javascript">

    /* ===== VALIDATE ===== */
    function validateForm() {
        var valid = true;
        var missing = [];

        // Clear previous errors
        $(".form-control, select").removeClass("is-invalid");
        $(".err-msg").hide();
        $("#topAlert").hide();

        // From Date
        if ($("#fdate").val() == "") {
            $("#fdate").addClass("is-invalid");
            $("#err_fdate").text("Please fill this required field.").show();
            missing.push("From Date");
            valid = false;
        }

        // To Date
        if ($("#tdate").val() == "") {
            $("#tdate").addClass("is-invalid");
            $("#err_tdate").text("Please fill this required field.").show();
            missing.push("To Date");
            valid = false;
        }

        // Date range check
        if ($("#fdate").val() != "" && $("#tdate").val() != "") {
            if (new Date($("#fdate").val()) > new Date($("#tdate").val())) {
                $("#tdate").addClass("is-invalid");
                $("#err_tdate").text("To Date cannot be before From Date.").show();
                missing.push("valid Date Range");
                valid = false;
            }
        }

        if (!valid) {
            $("#topAlert")
                .removeClass("alert-success-custom")
                .addClass("alert-danger-custom")
                .text("Please fill all required fields: " + missing.join(", "))
                .show();
            $("html,body").animate({ scrollTop: 0 }, 300);
        }

        return valid;
    }

</script>

</head>

<body>

<form id="form1" runat="server">

<uc1:Menu ID="Menu1" runat="server" />

<div class="main-content">

    <div class="report-wrapper">

        <div class="report-box">

            <div class="report-title">
                <h2>Purchase Report</h2>
                <p>Generate and export purchase &amp; payment reports</p>
            </div>

            <!-- TOP ALERT -->
            <div id="topAlert" class="alert-custom alert-danger-custom"></div>

            <!-- FILTER SECTION -->
            <div class="row">

                <div class="col-md-6">
                    <div class="input-group-custom">
                        <label>From Date <span style="color:red">*</span></label>
                        <div class="input-box">
                            <i class="fa fa-calendar"></i>
                            <input id="fdate" name="fdate" runat="server" class="form-control" />
                        </div>
                        <div class="err-msg" id="err_fdate"></div>
                    </div>
                </div>

                <div class="col-md-6">
                    <div class="input-group-custom">
                        <label>To Date <span style="color:red">*</span></label>
                        <div class="input-box">
                            <i class="fa fa-calendar-days"></i>
                            <input id="tdate" name="tdate" runat="server" class="form-control" />
                        </div>
                        <div class="err-msg" id="err_tdate"></div>
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
                            <asp:DropDownList ID="sPartyName" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>

            <!-- BUTTONS -->
            <div class="text-center" style="margin-top:20px;">

                <input type="submit"
                    id="btnContinue"
                    value="Generate Report"
                    runat="server"
                    class="btn btn-save"
                    onserverclick="btnContinue_ServerClick"
                    onclick="return validateForm();" />

                &nbsp;&nbsp;

                <button type="submit"
                    id="Export"
                    runat="server"
                    class="btn btn-save btn-export"
                    onserverclick="Export_ServerClick"
                    onclick="return validateForm();">
                    Export Excel &nbsp;<i class="fa fa-file-excel"></i>
                </button>

            </div>

            <!-- REPORT TABLE -->
            <div class="table-box table-responsive">
                <asp:PlaceHolder ID="DBDataPlaceHolder" runat="server"></asp:PlaceHolder>
            </div>

        </div>

    </div>

</div>

</form>

</body>
</html>
