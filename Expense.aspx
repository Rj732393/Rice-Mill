<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Expense.aspx.cs" Inherits="Expense" %>
<%@ Register Src="~/Includes/menu.ascx"
    TagName="Menu"
    TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Daily Expense</title>

    <meta name="viewport"
        content="width=device-width, initial-scale=1" />

    <!-- Bootstrap -->

    <link rel="stylesheet"
        href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css" />

    <!-- Font Awesome -->

    <link rel="stylesheet"
        href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />

    <!-- JQuery -->

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>

    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>

<style>

body{
    margin:0;
    padding:0;
    font-family:'Segoe UI';
    background:#f4f7fb;
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

/* ===== FORM ===== */

.expense-wrapper{
    padding:30px;
}

.expense-box{
    background:white;
    border-radius:25px;
    padding:35px;
    box-shadow:0 8px 30px rgba(0,0,0,0.08);
}

.expense-title{
    text-align:center;
    margin-bottom:35px;
}

.expense-title h2{
    font-size:36px;
    font-weight:800;
    color:#1e293b;
}

.expense-title p{
    color:#64748b;
    font-size:15px;
}

/* ===== ALERT ===== */

.alert-custom {
    border-radius: 12px;
    padding: 14px 18px;
    margin-bottom: 20px;
    font-size: 13px;
    font-weight: 600;
    display: none;
}

.alert-danger-custom {
    background: #fef2f2;
    border: 1px solid #fca5a5;
    color: #b91c1c;
}

.alert-success-custom {
    background: #f0fdf4;
    border: 1px solid #86efac;
    color: #15803d;
}

/* ===== INPUT ===== */

.input-group-custom{
    margin-bottom:25px;
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
select,
textarea{
    width:100%;
    border-radius:10px !important;
    border:1px solid #dbe2ea !important;
    padding-left:42px !important;
    box-shadow:none !important;
    font-size:13px !important;
}

.form-control,
select{
    height:42px !important;
}

textarea{
    padding-top:12px !important;
    min-height:120px;
    resize:none;
}

.form-control:focus,
select:focus,
textarea:focus{
    border-color:#f97316 !important;
    box-shadow:0 0 0 4px rgba(249,115,22,0.12) !important;
}

/* ===== VALIDATION ===== */

.form-control.is-invalid,
select.is-invalid,
textarea.is-invalid {
    border-color: #dc2626 !important;
    box-shadow: 0 0 0 3px rgba(220,38,38,0.10) !important;
}

.err-msg {
    color: #dc2626;
    font-size: 12px;
    margin-top: 4px;
    display: none;
}

/* ===== BUTTON ===== */

.btn-card{
    background:linear-gradient(135deg,#16a34a,#15803d);
    color:white !important;
    border:none;
    border-radius:50px;
    padding:12px 30px;
    font-size:15px;
    font-weight:700;
    letter-spacing:0.5px;
    box-shadow:0 8px 20px rgba(37,99,235,0.35);
    transition:all 0.3s ease;
}

.btn-card:hover{
   background:linear-gradient(135deg,#15803d,#166534);
    transform:translateY(-2px);
    box-shadow:0 12px 25px rgba(37,99,235,0.45);
    color:#fff !important;
}

.btn-card i{
    margin-left:8px;
}

/* ===== TABLE ===== */

.table-box{
    background:#fff;
    border-radius:18px;
    padding:20px;
    margin-top:30px;
    box-shadow:0 5px 18px rgba(0,0,0,0.06);
    overflow:auto;
}

/* ===== MOBILE ===== */

@media(max-width:768px){

    .main-content{
        margin-left:0;
    }

    .expense-wrapper{
        padding:15px;
    }

    .expense-box{
        padding:20px;
    }

    .expense-title h2{
        font-size:28px;
    }

}

</style>

<script>

    function toggleSidebar() {
        $(".main-content").toggleClass("full");
    }

    /* ===== VALIDATION HELPERS ===== */

    function showErr(fieldId, msg) {
        $("#" + fieldId).addClass("is-invalid");
        var errEl = $("#err_" + fieldId);
        if (errEl.length) { errEl.text(msg).show(); }
    }

    function clearErrors() {
        $(".form-control, select, textarea").removeClass("is-invalid");
        $(".err-msg").hide();
        $("#topAlert").hide();
    }

    function showAlert(msg, type) {
        var el = $("#topAlert");
        el.removeClass("alert-danger-custom alert-success-custom");
        el.addClass(type === "success" ? "alert-success-custom" : "alert-danger-custom");
        el.text(msg).show();
        $("html,body").animate({ scrollTop: 0 }, 300);
    }

    /* ===== MAIN VALIDATION ===== */

    function validateSave() {

        clearErrors();
        var valid = true;

        /* Date */
        var sdateVal = $.trim($("#sdate").val());
        if (sdateVal === "") {
            showErr("sdate", "Please select Date.");
            valid = false;
        }

        /* Amount */
        var amountVal = $.trim($("#EAmount").val());
        if (amountVal === "") {
            showErr("EAmount", "Please enter Amount.");
            valid = false;
        } else if (isNaN(amountVal) || parseFloat(amountVal) <= 0) {
            showErr("EAmount", "Amount must be a positive number.");
            valid = false;
        }

        /* Remarks */
        var remarksVal = $.trim($("#ERemarks").val());
        if (remarksVal === "") {
            showErr("ERemarks", "Please enter Remarks.");
            valid = false;
        }

        if (!valid) {
            showAlert("Please fill all required fields correctly.", "error");
        }

        return valid;
    }

</script>

</head>

<body>

<form id="form1" runat="server">

<uc1:Menu ID="Menu1" runat="server" />

<!-- MAIN CONTENT -->

<div class="main-content">

    <div class="expense-wrapper">

        <div class="expense-box">

            <div class="expense-title">

                <h2>Daily Expense Entry</h2>

                <p>
                    Manage all daily rice mill expenses
                </p>

            </div>

            <!-- TOP ALERT -->
            <div id="topAlert" class="alert-custom alert-danger-custom"></div>

            <!-- ROW 1 -->

            <div class="row">

                <div class="col-md-6">

                    <div class="input-group-custom">

                        <label>Select Date <span style="color:#dc2626;">*</span></label>

                        <div class="input-box">

                            <i class="fa fa-calendar"></i>

                            <input id="sdate"
                                runat="server"
                                class="form-control" />

                        </div>
                        <div class="err-msg" id="err_sdate"></div>

                    </div>

                </div>

                <div class="col-md-6"
                    style="padding-top:32px;">

                    <asp:LinkButton ID="lbrnData"
                        runat="server"
                        CssClass="btn btn-card"
                        onclick="lbrnData_Click">

                        Expense Report

                    </asp:LinkButton>

                </div>

            </div>

            <!-- ROW 2 -->

            <div class="row">

                <div class="col-md-12">

                    <div class="input-group-custom">

                        <label>Expense Type</label>

                        <div class="input-box">

                            <i class="fa fa-list"></i>

                            <asp:DropDownList ID="ddlExpenseType"
                                runat="server"
                                CssClass="form-control">

                                <asp:ListItem>Freight Exp</asp:ListItem>
                                <asp:ListItem>Truck</asp:ListItem>
                                <asp:ListItem>Electric Bill</asp:ListItem>
                                <asp:ListItem>Bank Interest</asp:ListItem>
                                <asp:ListItem>Salary to Staff ( Mill)</asp:ListItem>
                                <asp:ListItem>Repair &amp; Maintenance</asp:ListItem>
                                <asp:ListItem>Mobile Recharge Exp</asp:ListItem>
                                <asp:ListItem>Petrol Exp</asp:ListItem>
                                <asp:ListItem>Misc.Exp</asp:ListItem>
                                <asp:ListItem>Other Exp</asp:ListItem>

                            </asp:DropDownList>

                        </div>

                    </div>

                </div>

            </div>

            <!-- ROW 3 -->

            <div class="row">

                <div class="col-md-6">

                    <div class="input-group-custom">

                        <label>Amount (₹) <span style="color:#dc2626;">*</span></label>

                        <div class="input-box">

                            <i class="fa fa-indian-rupee-sign"></i>

                            <input id="EAmount"
                                runat="server"
                                class="form-control" />

                        </div>
                        <div class="err-msg" id="err_EAmount"></div>

                    </div>

                </div>

            </div>

            <!-- ROW 4 -->

            <div class="row">

                <div class="col-md-12">

                    <div class="input-group-custom">

                        <label>Remarks <span style="color:#dc2626;">*</span></label>

                        <div class="input-box">

                            <i class="fa fa-note-sticky"></i>

                            <textarea id="ERemarks"
                                runat="server"
                                class="form-control"></textarea>

                        </div>
                        <div class="err-msg" id="err_ERemarks"></div>

                    </div>

                </div>

            </div>

            <!-- BUTTON -->

            <div class="text-center"
                style="margin-top:20px;">

                <input type="submit"
                    id="btnSave"
                    value="Save Expense"
                    runat="server"
                    class="btn btn-card"
                    onserverclick="btnSave_ServerClick"
                    onclick="return validateSave();" />

            </div>

            <!-- TABLE -->

            <div class="table-box"
                id="prntContent">

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