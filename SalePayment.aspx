<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SalePayment.aspx.cs" Inherits="SalePayment" %>

<%@ Register Src="~/Includes/menu.ascx"
    TagName="Menu"
    TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Sale Payment - Rashmi Rice Mills</title>

    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Bootstrap -->
    <link rel="stylesheet"
        href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css" />

    <!-- Font Awesome -->
    <link rel="stylesheet"
        href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />

    <!-- JQuery -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>

    <!-- Bootstrap JS -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>

    <!-- Google Font -->
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700;800&display=swap"
        rel="stylesheet" />

<style>

* {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
}

body {
    background: #eef2f7;
    font-family: 'Poppins', sans-serif;
    overflow-x: hidden;
}

/* ===== MAIN ===== */

.main-content {
    margin-left: 120px;
    padding-top: 130px;
    transition: 0.3s;
}

.main-content.full {
    margin-left: 0;
}

/* ===== FORM ===== */

.payment-wrapper {
    padding: 30px;
}

.payment-box {
    background: white;
    border-radius: 28px;
    padding: 45px;
    box-shadow: 0 12px 40px rgba(0,0,0,0.08);
}

.payment-title {
    text-align: center;
    margin-bottom: 40px;
}

.payment-title h2 {
    font-size: 42px;
    font-weight: 800;
    color: #0f172a;
}

.payment-title p {
    color: #64748b;
    font-size: 16px;
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

.input-group-custom {
    margin-bottom: 28px;
}

.input-group-custom label {
    display: block;
    margin-bottom: 7px;
    font-size: 16px;
    font-weight: 700;
    color: #334155;
}

.input-box {
    position: relative;
}

.input-box i {
    position: absolute;
    top: 13px;
    left: 14px;
    color: #94a3b8;
    z-index: 9;
    font-size: 13px;
}

.form-control,
select {
    height: 45px !important;
    border-radius: 12px !important;
    border: 1px solid #dbe2ea !important;
    padding-left: 42px !important;
    box-shadow: none !important;
    font-size: 14px !important;
}

.form-control:focus,
select:focus {
    border-color: #16a34a !important;
    box-shadow: 0 0 0 4px rgba(22,163,74,0.12) !important;
}

/* ===== VALIDATION ===== */

.form-control.is-invalid,
select.is-invalid {
    border-color: #dc2626 !important;
    box-shadow: 0 0 0 3px rgba(220,38,38,0.15) !important;
}

.err-msg {
    color: #dc2626;
    font-size: 12px;
    margin-top: 5px;
    display: none;
}

/* ===== BUTTON ===== */

.btn-save {
    background: linear-gradient(135deg, #16a34a, #15803d);
    color: white !important;
    border: none !important;
    height: 48px;
    width: 220px;
    border-radius: 14px;
    font-size: 16px;
    font-weight: 600;
    transition: 0.3s;
    box-shadow: 0 8px 20px rgba(22,163,74,0.22);
}

.btn-save:hover {
    background: linear-gradient(135deg, #15803d, #166534);
    transform: translateY(-2px);
    color: white !important;
}

/* ===== TABLE ===== */

.table-box {
    background: white;
    border-radius: 20px;
    padding: 25px;
    margin-top: 35px;
    box-shadow: 0 5px 20px rgba(0,0,0,0.06);
    overflow: auto;
}

/* ===== PAYMENT LINK ===== */

.payment-link {
    display: inline-block;
    margin-top: 12px;
    font-weight: 600;
    color: #15803d;
    text-decoration: none !important;
}

.payment-link:hover {
    color: #166534;
}

/* ===== MOBILE ===== */

@media (max-width: 768px) {

    .main-content {
        margin-left: 0;
        padding-top: 100px;
    }

    .payment-wrapper {
        padding: 15px;
    }

    .payment-box {
        padding: 25px;
    }

    .payment-title h2 {
        font-size: 30px;
    }

    .btn-save {
        width: 100%;
        margin-bottom: 10px;
    }

}

</style>

<script type="text/javascript">

    $(document).ready(function () {

        checkPaymentMode();

        $("#paymentmode").change(function () {
            checkPaymentMode();
            clearErr("transaction");
        });

    });

    /* ===== PAYMENT MODE LABEL ===== */
    function checkPaymentMode() {

        var mode = $("#paymentmode").val();

        if (mode === "Online") {
            $("#<%= Panel2.ClientID %>").show();
            $("#lblTransaction").html("Transaction ID <span style='color:red'>*</span>");
        } else if (mode === "By Cheque") {
            $("#<%= Panel2.ClientID %>").show();
            $("#lblTransaction").html("Cheque No. &amp; Date <span style='color:red'>*</span>");
        } else {
            $("#<%= Panel2.ClientID %>").show();
            $("#lblTransaction").html("Receiver Name &amp; Mobile <span style='color:red'>*</span>");
        }
    }

    /* ===== VALIDATION HELPERS ===== */
    function showErr(fieldId, msg) {
        $("#" + fieldId).addClass("is-invalid");
        var errEl = $("#err_" + fieldId);
        if (errEl.length) { errEl.text(msg).show(); }
    }

    function clearErr(fieldId) {
        $("#" + fieldId).removeClass("is-invalid");
        $("#err_" + fieldId).hide();
    }

    function clearErrors() {
        $(".form-control, select").removeClass("is-invalid");
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
    function validatePayment() {

        clearErrors();
        var valid = true;

        /* Payment Date */
        var dt = $("#sdate").val();
        if (!dt || dt.trim() === "") {
            showErr("sdate", "Please select Payment Date.");
            valid = false;
        }

        /* Party Name */
        var party = $("#<%= ddlParty.ClientID %>").val();
        if (!party || party.trim() === "") {
            showErr("<%= ddlParty.ClientID %>", "Please select Party Name.");
            valid = false;
        }

        /* Amount Paid */
        var amt = $("#amountpaid").val();
        if (!amt || amt.trim() === "") {
            showErr("amountpaid", "Please enter Amount Paid.");
            valid = false;
        } else if (isNaN(amt) || parseFloat(amt) <= 0) {
            showErr("amountpaid", "Amount must be greater than zero.");
            valid = false;
        }

        /* Transaction */
        var txn = $("#transaction").val();
        if (!txn || txn.trim() === "") {
            showErr("transaction", "Please fill this field.");
            valid = false;
        }

        if (!valid) {
            showAlert("Please fill all required fields correctly.", "error");
        }

        return valid;
    }

    /* ===== SAVE VALIDATION ===== */
    function validateSave() {

        clearErrors();

        var contentText = $.trim($("#prntContent").text());

        if (contentText === "" || contentText.indexOf("No Data Added") !== -1) {
            showAlert("Please enter at least one data!!", "error");
            return false;
        }

        return true;
    }


    /* ===== SIDEBAR ===== */
    function toggleSidebar() {
        $(".sidebar").toggleClass("hide");
        $(".main-content").toggleClass("full");
    }

</script>

</head>

<body>

<form id="form1" runat="server">

    <!-- MENU -->
    <uc1:Menu ID="Menu1" runat="server" />

    <!-- MAIN CONTENT -->
    <div class="main-content">

        <div class="payment-wrapper">

            <div class="payment-box">

                <!-- TITLE -->
                <div class="payment-title">
                    <h2>Sale Payment Entry</h2>
                    <p>Manage customer payment transactions</p>
                </div>

                <!-- TOP ALERT -->
                <div id="topAlert" class="alert-custom alert-danger-custom"></div>

                <!-- BALANCE -->
                <div class="row">
                    <div class="col-md-12 text-right">
                        <h4 style="font-weight:700; color:#dc2626; margin-top:0px;">
                            Balance :
                            <asp:Label ID="lblOSB" runat="server" Text=""></asp:Label>
                            (In Rs.)
                        </h4>
                    </div>
                </div>

                <!-- ROW 1 : Date + Manual Voucher -->
                <div class="row">

                    <div class="col-md-6">
                        <div class="input-group-custom">
                            <label>Payment Date <span style="color:red">*</span></label>
                            <div class="input-box">
                                <i class="fa fa-calendar"></i>
                                <input id="sdate"
                                    runat="server"
                                    class="form-control"
                                    required="required" />
                            </div>
                            <div class="err-msg" id="err_sdate"></div>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="input-group-custom">
                            <label>Manual Voucher No.</label>
                            <div class="input-box">
                                <i class="fa fa-file"></i>
                                <input id="pvNo"
                                    runat="server"
                                    type="text"
                                    class="form-control" />
                            </div>
                        </div>
                    </div>

                </div>

                <!-- PARTY -->
                <div class="row">
                    <div class="col-md-12">
                        <div class="input-group-custom">
                            <label>Select Party Name <span style="color:red">*</span></label>
                            <div class="input-box">
                                <i class="fa fa-users"></i>
                                <asp:DropDownList ID="ddlParty"
                                    runat="server"
                                    CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="err-msg" id="err_<%= ddlParty.ClientID %>"></div>
                            <asp:LinkButton ID="LinkButton1"
                                runat="server"
                                CssClass="payment-link"
                                OnClick="LinkButton1_Click">
                                View Payment List
                            </asp:LinkButton>
                        </div>
                    </div>
                </div>

                <!-- ROW 3 : Amount + Payment Mode -->
                <div class="row">

                    <div class="col-md-6">
                        <div class="input-group-custom">
                            <label>Amount Paid <span style="color:red">*</span></label>
                            <div class="input-box">
                                <i class="fa fa-indian-rupee-sign"></i>
                                <input id="amountpaid"
                                    runat="server"
                                    class="form-control"
                                    required="required" />
                            </div>
                            <div class="err-msg" id="err_amountpaid"></div>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="input-group-custom">
                            <label>Payment Mode <span style="color:red">*</span></label>
                            <div class="input-box">
                                <i class="fa fa-credit-card"></i>
                                <select id="paymentmode"
                                    runat="server"
                                    class="form-control">
                                    <option>By Cash</option>
                                    <option>By Cheque</option>
                                    <option>Online</option>
                                </select>
                            </div>
                        </div>
                    </div>

                </div>

                <!-- TRANSACTION -->
                <asp:Panel ID="Panel2" runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="input-group-custom">
                                <label id="lblTransaction">
                                    Receiver Name &amp; Mobile <span style="color:red">*</span>
                                </label>
                                <div class="input-box">
                                    <i class="fa fa-receipt"></i>
                                    <input id="transaction"
                                        runat="server"
                                        type="text"
                                        class="form-control" />
                                </div>
                                <div class="err-msg" id="err_transaction"></div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <!-- BUTTONS -->
                <div class="text-center" style="margin-top:20px;">

                    <input type="submit"
                        id="btnContinue"
                        value="Click To Add"
                        runat="server"
                        class="btn btn-save"
                        onserverclick="btnContinue_ServerClick"
                        onclick="return validatePayment();"
                        
                        
                         />

                    &nbsp;&nbsp;

                   <input type="submit"
                        id="btnSave"
                        value="Submit"
                        runat="server"
                        class="btn btn-save"
                        onserverclick="btnSave_ServerClick"
                        onclick="return validateSave();" />

                </div>

                <!-- TABLE / PRINT AREA -->
                <div class="table-box" id="prntContent">
                    <asp:PlaceHolder ID="DBDataPlaceHolder" runat="server">
                    </asp:PlaceHolder>
                </div>

            </div>

        </div>

    </div>

</form>

</body>

</html>
