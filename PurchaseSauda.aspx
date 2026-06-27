<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="PurchaseSauda.aspx.cs"
    Inherits="PurchaseSauda" %>

<%@ Register Src="~/Includes/menu.ascx"
    TagName="Menu"
    TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Purchase Sauda</title>

    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <!-- Bootstrap -->
    <link rel="stylesheet"
        href="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css" />

    <!-- Font Awesome -->
    <link rel="stylesheet"
        href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css" />

    <!-- JQuery -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.4.1/js/bootstrap.min.js"></script>

    <!-- JQuery UI -->
    <link href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/base/jquery-ui.css"
        rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>

<style>

body{
    margin:0;
    padding:0;
    font-family:'Segoe UI',sans-serif;
    background:#f4f7fb;
    overflow-x:hidden;
}

.main-content{
    margin-left:120px;
    padding:110px 25px 30px;
    transition:0.3s;
}

.main-content.full{
    margin-left:0;
}

.purchase-wrapper{
    max-width:1400px;
    margin:auto;
}

.purchase-box{
    background:#ffffff;
    border-radius:24px;
    padding:35px;
    box-shadow:0 8px 28px rgba(0,0,0,0.08);
    border:1px solid #e5e7eb;
}

.purchase-title{
    text-align:center;
    margin-bottom:35px;
}

.purchase-title h2{
    margin:0;
    font-size:34px;
    font-weight:800;
    color:#14532d;
}

.purchase-title p{
    margin-top:8px;
    color:#64748b;
    font-size:14px;
}

.input-group-custom{
    margin-bottom:24px;
}

.input-group-custom label{
    display:block;
    margin-bottom:8px;
    font-size:14px;
    font-weight:700;
    color:#334155;
}

.input-box{
    position:relative;
}

.input-box i{
    position:absolute;
    top:14px;
    left:14px;
    color:#94a3b8;
    z-index:9;
    font-size:13px;
}

.form-control,
select{
    height:46px !important;
    border-radius:12px !important;
    border:1px solid #dbe2ea !important;
    padding-left:42px !important;
    box-shadow:none !important;
    font-size:13px !important;
    transition:0.3s;
}

.form-control:focus,
select:focus{
    border-color:#16a34a !important;
    box-shadow:0 0 0 4px rgba(22,163,74,0.12) !important;
}

/* Validation error style */
.form-control.is-invalid,
select.is-invalid{
    border-color:#dc2626 !important;
    box-shadow:0 0 0 3px rgba(220,38,38,0.15) !important;
}

.err-msg{
    color:#dc2626;
    font-size:12px;
    margin-top:5px;
    display:none;
}

/* Alert box */
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

.btn-card{
    background:linear-gradient(135deg,#16a34a,#15803d) !important;
    color:#ffffff !important;
    border:none !important;
    border-radius:30px !important;
    padding:11px 24px !important;
    font-size:14px !important;
    font-weight:700 !important;
    letter-spacing:0.3px;
    box-shadow:0 6px 16px rgba(22,163,74,0.22);
    transition:all 0.3s ease;
    margin:5px;
    outline:none !important;
}

.btn-card:hover{
    background:linear-gradient(135deg,#15803d,#166534) !important;
    transform:translateY(-2px);
    color:#fff !important;
}

.btn-card:focus{
    color:#ffffff !important;
    outline:none !important;
}

.btn-card:active{
    transform:scale(0.98);
}

#btnSave{
    background:linear-gradient(135deg,#0f766e,#0d9488) !important;
}

#btnSave:hover{
    background:linear-gradient(135deg,#115e59,#0f766e) !important;
}

#btnReset{
    background:linear-gradient(135deg,#dc2626,#b91c1c) !important;
}

#btnReset:hover{
    background:linear-gradient(135deg,#b91c1c,#991b1b) !important;
}

.table-box{
    background:#ffffff;
    border-radius:18px;
    padding:20px;
    margin-top:30px;
    box-shadow:0 5px 18px rgba(0,0,0,0.06);
    overflow:auto;
    border:1px solid #e5e7eb;
}

@media(max-width:768px){
    .main-content{
        margin-left:0;
        padding:95px 12px 20px;
    }
    .purchase-box{
        padding:20px;
    }
    .purchase-title h2{
        font-size:26px;
    }
}

</style>

<script type="text/javascript">

    $(document).ready(function () {

        SearchText();

        // Reset Button
        $("#btnReset").click(function () {

            clearErrors();

            $("#sdate").val("");
            $("#MPNo").val("");
            $("#txtEmpName").val("");
            $("#QIKG").val("");
            $("#avgrate").val("");

            $("#pName").val("");
            $("#pMN").val("");

            $("#<%= sPartyName.ClientID %>").prop("selectedIndex", 0);

            return false;
        });

    });

    /* ===== AUTOCOMPLETE ===== */
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

    /* ===== SHOW ERROR ===== */
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
        $(".form-control, select").removeClass("is-invalid");
        $(".err-msg").hide();
        $("#topAlert").hide();
    }

    /* ===== SHOW TOP ALERT ===== */
    function showAlert(msg) {
        var el = $("#topAlert");
        el.removeClass("alert-danger-custom");
        el.addClass("alert-danger-custom");
        el.text(msg).show();
        $("html,body").animate({ scrollTop: 0 }, 300);
    }

    /* ===== VALIDATE ADD PURCHASE DATA ===== */
    function validateSauda() {
        clearErrors();
        var valid = true;

        /* Date */
        if ($("#sdate").val() == "") {
            showErr("sdate", "Please fill this required field.");

            valid = false;
        }

        /* Supplier Ref */
        

        /* Party Name DropDownList */
        var partyVal = $("#<%= sPartyName.ClientID %>").val();
        if (partyVal == "" || partyVal == null) {
            showErr("<%= sPartyName.ClientID %>", "Please fill this required field.");
            valid = false;
        }

        /* Panel1 fields — sirf tab validate karo jab panel visible ho */
        if ($("#<%= Panel1.ClientID %>").is(":visible")) {

            if ($("#pName").val().trim() == "") {
                showErr("pName", "Please fill this required field.");
                valid = false;
            }

            var mob = $("#pMN").val().trim();
            if (mob == "") {
                showErr("pMN", "Please fill this required field.");
                valid = false;
            } else if (!/^\d{10}$/.test(mob)) {
                showErr("pMN", "Please fill this required field.");
                valid = false;
            }
        }

        /* Quantity */
        var qty = $("#QIKG").val().trim();
        if (qty == "") {
            showErr("QIKG", "Please fill this required field.");
            valid = false;
        } else if (isNaN(qty) || parseFloat(qty) <= 0) {
            showErr("QIKG", "Quantity should be greater than zero.");
            valid = false;
        }

        /* Rate */
        var rate = $("#avgrate").val().trim();
        if (rate == "") {
            showErr("avgrate", "Please fill this required field.");
            valid = false;
        } else if (isNaN(rate) || parseFloat(rate) <= 0) {
            showErr("avgrate", "Rate should be greater than zero.");
            valid = false;
        }

        if (!valid) {
            showAlert("Please fill all required fields.");
        }

        return valid;
    }

</script>

</head>

<body>

<form id="form1" runat="server">

    <!-- MENU -->
    <uc1:Menu ID="Menu1" runat="server" />

    <!-- MAIN CONTENT -->
    <div class="main-content">

        <div class="purchase-wrapper">

            <div class="purchase-box">

                <!-- TITLE -->
                <div class="purchase-title">
                    <h2>Purchase Sauda Entry</h2>
                    <p>Enter paddy purchase details</p>
                </div>

                <!-- TOP ALERT -->
                <div id="topAlert" class="alert-custom alert-danger-custom"></div>

                <!-- ROW 1 -->
                <div class="row">

                    <div class="col-md-4">
                        <div class="input-group-custom">
                            <label>Select Date <span style="color:red">*</span></label>
                            <div class="input-box">
                                <i class="fa fa-calendar"></i>
                                <input id="sdate"
                                    runat="server"
                                    class="form-control"
                                    placeholder="Select date" />
                            </div>
                            <div class="err-msg" id="err_sdate"></div>
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
                            <label>Supplier Ref. </label>
                            <div class="input-box">
                                <i class="fa fa-user"></i>
                                <asp:TextBox ID="txtEmpName"
                                    runat="server"
                                    CssClass="form-control"
                                    placeholder="Type to search...">
                                </asp:TextBox>
                            </div>
                            <div class="err-msg" id="err_<%= txtEmpName.ClientID %>"></div>
                        </div>
                    </div>

                </div>

                <!-- ROW 2 -->
                <div class="row">

                    <div class="col-md-6">
                        <div class="input-group-custom">
                            <label>Party Name <span style="color:red">*</span></label>
                            <div class="input-box">
                                <i class="fa fa-users"></i>
                                <asp:DropDownList ID="sPartyName"
                                    runat="server"
                                    CssClass="form-control"
                                    AutoPostBack="true"
                                    onselectedindexchanged="sPartyName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="err-msg" id="err_<%= sPartyName.ClientID %>"></div>
                        </div>
                    </div>

                    <div class="col-md-6" style="padding-top:31px;">
                        <asp:LinkButton ID="lBtnSaudaParty"
                            runat="server"
                            CssClass="btn btn-card"
                            onclick="lBtnSaudaParty_Click">
                            <i class="fa fa-list"></i>
                            &nbsp; Sauda List
                        </asp:LinkButton>
                    </div>

                </div>

                <!-- PANEL 1 — Party Details (conditionally visible) -->
                <asp:Panel ID="Panel1" runat="server">

                    <div class="row">

                        <div class="col-md-6">
                            <div class="input-group-custom">
                                <label>Party Name <span style="color:red">*</span></label>
                                <div class="input-box">
                                    <i class="fa fa-user"></i>
                                    <input id="pName"
                                        runat="server"
                                        type="text"
                                        class="form-control"
                                        placeholder="Enter party name" />
                                </div>
                                <div class="err-msg" id="err_pName"></div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="input-group-custom">
                                <label>Party Mobile No. <span style="color:red">*</span></label>
                                <div class="input-box">
                                    <i class="fa fa-phone"></i>
                                    <input id="pMN"
                                        runat="server"
                                        class="form-control"
                                        placeholder="10 digit mobile no."
                                        maxlength="10" />
                                </div>
                                <div class="err-msg" id="err_pMN"></div>
                            </div>
                        </div>

                    </div>

                </asp:Panel>

                <!-- ROW 3 -->
                <div class="row">

                    <div class="col-md-4">
                        <div class="input-group-custom">
                            <label>Paddy Type <span style="color:red">*</span></label>
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
                            <label>Quantity (KG) <span style="color:red">*</span></label>
                            <div class="input-box">
                                <i class="fa fa-weight-hanging"></i>
                                <input id="QIKG"
                                    runat="server"
                                    class="form-control"
                                    placeholder="Enter quantity in KG" />
                            </div>
                            <div class="err-msg" id="err_QIKG"></div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="input-group-custom">
                            <label>Rate (&#8377;) <span style="color:red">*</span></label>
                            <div class="input-box">
                                <i class="fa fa-indian-rupee-sign"></i>
                                <input id="avgrate"
                                    runat="server"
                                    class="form-control"
                                    placeholder="Enter rate per KG" />
                            </div>
                            <div class="err-msg" id="err_avgrate"></div>
                        </div>
                    </div>

                </div>

                <!-- BUTTONS -->
                <div class="text-center" style="margin-top:20px;">

                    <input type="submit"
                        id="btnContinue"
                        value="Add Purchase Data"
                        runat="server"
                        class="btn btn-card"
                        onserverclick="btnContinue_ServerClick"
                        onclick="return validateSauda();" />

                    &nbsp;&nbsp;

                    <input type="submit"
                        id="btnSave"
                        value="Save Purchase Sauda"
                        runat="server"
                        class="btn btn-card"
                        onserverclick="btnSave_ServerClick" />

                    &nbsp;&nbsp;

                    <input type="button"
                        id="btnReset"
                        value="Reset"
                        class="btn btn-card" />

                </div>

                <!-- TABLE -->
                <div class="table-box" id="prntContent">
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
