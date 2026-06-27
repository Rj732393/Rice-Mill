<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="PaddyProcessing.aspx.cs"
    Inherits="PaddyProcessing" %>

    <%@ Register Src="~/Includes/menu.ascx"
    TagName="Menu"
    TagPrefix="uc1" %> 

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Paddy Processing</title>

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

/* ===== BOX ===== */

.processing-wrapper{
    padding:30px;
}

.processing-box{
    background:white;
    border-radius:25px;
    padding:35px;
    box-shadow:0 8px 30px rgba(0,0,0,0.08);
}

.processing-title{
    text-align:center;
    margin-bottom:35px;
}

.processing-title h2{
    font-size:36px;
    font-weight:800;
    color:#1e293b;
}

.processing-title p{
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
select{
    height:42px !important;
    border-radius:10px !important;
    border:1px solid #dbe2ea !important;
    padding-left:42px !important;
    box-shadow:none !important;
    font-size:13px !important;
    width:100%;
}

.form-control:focus,
select:focus{
    border-color:#f97316 !important;
    box-shadow:0 0 0 4px rgba(249,115,22,0.12) !important;
}

/* ===== VALIDATION ===== */

.form-control.is-invalid,
select.is-invalid {
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

    .processing-wrapper{
        padding:15px;
    }

    .processing-box{
        padding:20px;
    }

    .processing-title h2{
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

    /* ===== MAIN VALIDATION : sirf Date + Paddy (KG) required ===== */
    function validateSave() {

        clearErrors();
        var valid = true;

        /* Date */
        var sdateVal = $.trim($("#sdate").val());
        if (sdateVal === "") {
            showErr("sdate", "Please select Date.");
            valid = false;
        }

        /* Paddy (KG) */
        var paddyVal = $.trim($("#<%= PaddyWt.ClientID %>").val());
        if (paddyVal === "") {
            showErr("<%= PaddyWt.ClientID %>", "Please enter Paddy (KG).");
            valid = false;
        } else if (isNaN(paddyVal) || parseFloat(paddyVal) <= 0) {
            showErr("<%= PaddyWt.ClientID %>", "Paddy (KG) must be a positive number.");
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

<!-- MAIN -->

<div class="main-content">

    <div class="processing-wrapper">

        <div class="processing-box">

            <div class="processing-title">

                <h2>Paddy Processing</h2>

                <p>
                    Manage paddy and rice processing details
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
                                name="sdate"
                                runat="server"
                                required
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

                        Report

                    </asp:LinkButton>

                </div>

            </div>

            <!-- ROW 2 -->

            <div class="row">

                <div class="col-md-6">

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

                <div class="col-md-6">

                    <div class="input-group-custom">

                        <label>Rice Type</label>

                        <div class="input-box">

                            <i class="fa fa-bowl-rice"></i>

                            <select id="sRiceType"
                                runat="server"
                                class="form-control">

                                <option>Rashmi Ka 7 Star</option>
                                <option>Rashmi Ka Sonam</option>
                                <option>7 Star Katarni</option>
                                <option>Steam Bran</option>

                            </select>

                        </div>

                    </div>

                </div>

            </div>

            <!-- WEIGHT ROWS -->

            <div class="row">

                <div class="col-md-4">

                    <div class="input-group-custom">

                        <label>Paddy (KG) <span style="color:#dc2626;">*</span></label>

                        <div class="input-box">

                            <i class="fa fa-weight-hanging"></i>

                            <asp:TextBox ID="PaddyWt"
                                runat="server"
                                CssClass="form-control"
                                style="text-align:right"
                                OnTextChanged="PaddyWt_TextChanged"
                                AutoPostBack="true">
                            </asp:TextBox>

                        </div>
                        <div class="err-msg" id="err_<%= PaddyWt.ClientID %>"></div>

                    </div>

                </div>

                <div class="col-md-4">

                    <div class="input-group-custom">

                        <label>Rice (KG)</label>

                        <div class="input-box">

                            <i class="fa fa-weight-hanging"></i>

                            <input id="RiceWt"
                                name="RiceWt"
                                runat="server"
                                class="form-control"
                                style="text-align:right" />

                        </div>

                    </div>

                </div>

                <div class="col-md-4">

                    <div class="input-group-custom">

                        <label>Broken (KG)</label>

                        <div class="input-box">

                            <i class="fa fa-weight-hanging"></i>

                            <input id="BrokenWt"
                                name="BrokenWt"
                                runat="server"
                                class="form-control"
                                style="text-align:right" />

                        </div>

                    </div>

                </div>

            </div>

            <!-- ROW -->

            <div class="row">

                <div class="col-md-4">

                    <div class="input-group-custom">

                        <label>Bran Amount (KG)</label>

                        <div class="input-box">

                            <i class="fa fa-weight-hanging"></i>

                            <input id="BranWt"
                                name="BranWt"
                                runat="server"
                                class="form-control"
                                style="text-align:right" />

                        </div>

                    </div>

                </div>

                <div class="col-md-4">

                    <div class="input-group-custom">

                        <label>Nakku (KG)</label>

                        <div class="input-box">

                            <i class="fa fa-weight-hanging"></i>

                            <input id="NakkuWt"
                                name="NakkuWt"
                                runat="server"
                                class="form-control"
                                style="text-align:right" />

                        </div>

                    </div>

                </div>

                <div class="col-md-4">

                    <div class="input-group-custom">

                        <label>Nakku Bhusi (KG)</label>

                        <div class="input-box">

                            <i class="fa fa-weight-hanging"></i>

                            <input id="NakkuBhusi"
                                name="NakkuBhusi"
                                runat="server"
                                class="form-control"
                                style="text-align:right" />

                        </div>

                    </div>

                </div>

            </div>

            <!-- LAST ROW -->

            <div class="row">

                <div class="col-md-6">

                    <div class="input-group-custom">

                        <label>Rejection (KG)</label>

                        <div class="input-box">

                            <i class="fa fa-weight-hanging"></i>

                            <input id="RejectionWt"
                                name="RejectionWt"
                                runat="server"
                                class="form-control"
                                style="text-align:right" />

                        </div>

                    </div>

                </div>

                <div class="col-md-6">

                    <div class="input-group-custom">

                        <label>Husk (KG)</label>

                        <div class="input-box">

                            <i class="fa fa-weight-hanging"></i>

                            <input id="HuskWt"
                                name="HuskWt"
                                runat="server"
                                class="form-control"
                                style="text-align:right" />

                        </div>

                    </div>

                </div>

            </div>

            <!-- BUTTON -->

            <div class="text-center"
                style="margin-top:20px;">

                <input type="submit"
                    id="btnSave"
                    value="Submit"
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