<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="Sale.aspx.cs"
    Inherits="Sale" %>

<%@ Register Src="~/Includes/menu.ascx"
    TagName="Menu"
    TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Sale Entry</title>

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
    font-family:'Segoe UI',sans-serif;
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

.sale-wrapper{
    padding:30px;
}

.sale-box{
    background:#ffffff;
    border-radius:28px;
    padding:38px;
    box-shadow:0 10px 35px rgba(0,0,0,0.08);
}

.sale-title{
    text-align:center;
    margin-bottom:35px;
}

.sale-title h2{
    font-size:38px;
    font-weight:800;
    color:#1e293b;
    margin-bottom:8px;
}

.sale-title p{
    color:#64748b;
    font-size:15px;
}

/* ===== INPUT ===== */

.input-group-custom{
    margin-bottom:25px;
}

.input-group-custom label{
    display:block;
    margin-bottom:8px;
    font-size:15px;
    font-weight:700;
    color:#334155;
}

.input-box{
    position:relative;
}

.input-box i{
    position:absolute;
    top:14px;
    left:15px;
    color:#94a3b8;
    z-index:9;
    font-size:14px;
}

.form-control{
    height:45px !important;
    border-radius:12px !important;
    border:1px solid #dbe2ea !important;
    padding-left:45px !important;
    box-shadow:none !important;
    font-size:14px !important;
    transition:0.3s;
}

.form-control:focus{
    border-color:#16a34a !important;
    box-shadow:0 0 0 4px rgba(22,163,74,0.12) !important;
}

/* ===== VALIDATION (same pattern as Payment.aspx) ===== */

.form-control.is-invalid{
    border-color:#dc2626 !important;
    box-shadow:0 0 0 3px rgba(220,38,38,0.15) !important;
}

.err-msg{
    color:#dc2626;
    font-size:12px;
    margin-top:5px;
    display:none;
}

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

/* ===== BUTTON ===== */

.btn-card{
    background:linear-gradient(135deg,#16a34a,#15803d) !important;
    color:#ffffff !important;
    border:none !important;
    border-radius:30px !important;
    padding:12px 32px !important;
    font-size:15px !important;
    font-weight:700 !important;
    letter-spacing:0.3px;
    transition:0.3s;
    box-shadow:0 8px 20px rgba(22,163,74,0.25);
}

.btn-card:hover{
    background:linear-gradient(135deg,#15803d,#166534) !important;
    transform:translateY(-2px);
    color:#ffffff !important;
}

/* ===== TABLE ===== */

.table-box{
    background:#ffffff;
    border-radius:18px;
    padding:22px;
    margin-top:35px;
    box-shadow:0 5px 18px rgba(0,0,0,0.06);
    overflow:auto;
}

.table th{
    background:#16a34a !important;
    color:#ffffff !important;
    border:none !important;
}

.table td{
    vertical-align:middle !important;
}

/* ===== MOBILE ===== */

@media(max-width:768px){

    .main-content{
        margin-left:0;
        padding-top:100px;
    }

    .sale-wrapper{
        padding:15px;
    }

    .sale-box{
        padding:22px;
    }

    .sale-title h2{
        font-size:28px;
    }

}

</style>

<script>

    function toggleSidebar() {

        $(".main-content").toggleClass("full");

    }

    /* ===== SHOW ERROR (same pattern as Payment.aspx) ===== */
    function showErr(fieldId, msg) {
        $("#" + fieldId).addClass("is-invalid");
        $("#err_" + fieldId).text(msg).show();
    }

    /* ===== CLEAR ERROR ===== */
    function clearErr(fieldId) {
        $("#" + fieldId).removeClass("is-invalid");
        $("#err_" + fieldId).hide();
    }

    /* ===== CLEAR ALL ERRORS ===== */
    function clearErrors() {
        $(".form-control").removeClass("is-invalid");
        $(".err-msg").hide();
        $("#topAlert").hide();
    }

    /* ===== SHOW TOP ALERT ===== */
    function showAlert(msg, type) {
        var el = $("#topAlert");
        el.removeClass("alert-danger-custom alert-success-custom");
        if (type == "success") {
            el.addClass("alert-success-custom");
        } else {
            el.addClass("alert-danger-custom");
        }
        el.text(msg).show();
        $("html,body").animate({ scrollTop: 0 }, 300);
    }

    /* ===== VALIDATE SALE ENTRY ===== */
    function validateSale() {

        clearErrors();
        var valid = true;

        /* Sauda No */
        var saudaNo = $("#<%= SaudaNo.ClientID %>").val();
        if (saudaNo == null || saudaNo.trim() == "") {
            showErr("<%= SaudaNo.ClientID %>", "Please enter Sauda No.");
            valid = false;
        }

        /* Sauda Date */
        var saudaDate = $("#<%= SaudaDate.ClientID %>").val();
        if (saudaDate == null || saudaDate.trim() == "") {
            showErr("<%= SaudaDate.ClientID %>", "Please select Sauda Date.");
            valid = false;
        } else if (isNaN(new Date(saudaDate).getTime())) {
            showErr("<%= SaudaDate.ClientID %>", "Please enter a valid Sauda Date.");
            valid = false;
        }

        /* Despatch No */
        var despatchNo = $("#<%= DespatchNo.ClientID %>").val();
        if (despatchNo == null || despatchNo.trim() == "") {
            showErr("<%= DespatchNo.ClientID %>", "Please enter Despatch No.");
            valid = false;
        }

        /* PMN */
        var pmnVal = $("#<%= pMN.ClientID %>").val();
        if (pmnVal == null || pmnVal.trim() == "") {
            showErr("<%= pMN.ClientID %>", "Please enter PMN.");
            valid = false;
        } else if (isNaN(pmnVal)) {
            showErr("<%= pMN.ClientID %>", "PMN must be a valid number.");
            valid = false;
        } else if (parseInt(pmnVal) <= 0) {
            showErr("<%= pMN.ClientID %>", "PMN must be greater than zero.");
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

    <!-- MENU -->

    <uc1:Menu ID="Menu1" runat="server" />

    <!-- MAIN -->

    <div class="main-content">

        <div class="sale-wrapper">

            <div class="sale-box">

                <div class="sale-title">

                    <h2>Sale Entry</h2>

                    <p>
                        Enter sale and dispatch details
                    </p>

                </div>

                <!-- TOP ALERT -->

                <div id="topAlert" class="alert-custom alert-danger-custom"></div>

                <!-- ROW 1 -->

                <div class="row">

                    <div class="col-md-4">

                        <div class="input-group-custom">

                            <label>Sauda No <span style="color:red">*</span></label>

                            <div class="input-box">

                                <i class="fa fa-file"></i>

                                <asp:TextBox ID="SaudaNo"
                                    runat="server"
                                    CssClass="form-control">
                                </asp:TextBox>

                            </div>

                            <div class="err-msg" id="err_<%= SaudaNo.ClientID %>"></div>

                        </div>

                    </div>

                    <div class="col-md-4">

                        <div class="input-group-custom">

                            <label>Sauda Date <span style="color:red">*</span></label>

                            <div class="input-box">

                                <i class="fa fa-calendar"></i>

                                <asp:TextBox ID="SaudaDate"
                                    runat="server"
                                    CssClass="form-control">
                                </asp:TextBox>

                            </div>

                            <div class="err-msg" id="err_<%= SaudaDate.ClientID %>"></div>

                        </div>

                    </div>

                    <div class="col-md-4">

                        <div class="input-group-custom">

                            <label>Despatch No <span style="color:red">*</span></label>

                            <div class="input-box">

                                <i class="fa fa-truck"></i>

                                <asp:TextBox ID="DespatchNo"
                                    runat="server"
                                    CssClass="form-control">
                                </asp:TextBox>

                            </div>

                            <div class="err-msg" id="err_<%= DespatchNo.ClientID %>"></div>

                        </div>

                    </div>

                </div>

                <!-- ROW 2 -->

                <div class="row">

                    <div class="col-md-6">

                        <div class="input-group-custom">

                            <label>PMN <span style="color:red">*</span></label>

                            <div class="input-box">

                                <i class="fa fa-hashtag"></i>

                                <asp:TextBox ID="pMN"
                                    runat="server"
                                    CssClass="form-control">
                                </asp:TextBox>

                            </div>

                            <div class="err-msg" id="err_<%= pMN.ClientID %>"></div>

                        </div>

                    </div>

                </div>

                <!-- BUTTON -->

                <div class="text-center"
                    style="margin-top:25px;">

                    <asp:Button ID="btnSave"
                        runat="server"
                        Text="Save Sale Entry"
                        CssClass="btn btn-card"
                        OnClientClick="return validateSale();"
                        OnClick="btnSave_Click" />

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
