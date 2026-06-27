<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="SaleSauda.aspx.cs"
    Inherits="SaleSauda" %>

<%@ Register Src="~/Includes/menu.ascx"
    TagName="Menu"
    TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Sale Sauda</title>

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

    <!-- JQuery UI -->
    <link href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/base/jquery-ui.css"
        rel="stylesheet" />
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>

<style>

body {
    margin: 0;
    padding: 0;
    font-family: 'Segoe UI';
    background: #f4f7fb;
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

.sale-wrapper {
    padding: 30px;
}

.sale-box {
    background: white;
    border-radius: 25px;
    padding: 35px;
    box-shadow: 0 8px 30px rgba(0,0,0,0.08);
}

.sale-title {
    text-align: center;
    margin-bottom: 35px;
}

.sale-title h2 {
    font-size: 36px;
    font-weight: 800;
    color: #1e293b;
}

.sale-title p {
    color: #64748b;
    font-size: 15px;
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
    margin-bottom: 25px;
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
select,
textarea {
    width: 100%;
    border-radius: 10px !important;
    border: 1px solid #dbe2ea !important;
    padding-left: 42px !important;
    box-shadow: none !important;
    font-size: 13px !important;
}

.form-control,
select {
    height: 42px !important;
}

textarea {
    padding-top: 12px;
}

.form-control:focus,
select:focus,
textarea:focus {
    border-color: #f97316 !important;
    box-shadow: 0 0 0 4px rgba(249,115,22,0.12) !important;
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

.btn-card {
    background: linear-gradient(135deg, #16a34a, #15803d);
    color: white !important;
    border: none;
    border-radius: 50px;
    padding: 12px 30px;
    font-size: 15px;
    font-weight: 700;
    letter-spacing: 0.5px;
    box-shadow: 0 8px 20px rgba(37,99,235,0.35);
    transition: all 0.3s ease;
}

.btn-card:hover {
    background: linear-gradient(135deg, #15803d, #166534);
    transform: translateY(-2px);
    box-shadow: 0 12px 25px rgba(37,99,235,0.45);
    color: #fff !important;
}

.btn-card i {
    margin-left: 8px;
}

/* ===== TABLE ===== */

.table-box {
    background: #fff;
    border-radius: 18px;
    padding: 20px;
    margin-top: 30px;
    box-shadow: 0 5px 18px rgba(0,0,0,0.06);
    overflow: auto;
}

/* ===== MOBILE ===== */

@media (max-width: 768px) {

    .main-content {
        margin-left: 0;
    }

    .sale-wrapper {
        padding: 15px;
    }

    .sale-box {
        padding: 20px;
    }

}

</style>

<script type="text/javascript">

    $(document).ready(function () {
        SearchText();
    });

    /* ===== AUTOCOMPLETE ===== */
    function SearchText() {
        $("#<%= txtEmpName.ClientID %>").autocomplete({
            source: function (request, response) {
                $.ajax({
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    url: "PurchaseUnloading.aspx/GetEmployeeName",
                    data: "{'empName':'" + document.getElementById('<%= txtEmpName.ClientID %>').value + "'}",
                    dataType: "json",
                    success: function (data) { response(data.d); }
                });
            }
        });
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

    /* ===== CONTINUE VALIDATION ===== */
    function validateContinue() {

        clearErrors();
        var valid = true;

        /* Date */
        var sdate = $.trim($("#sdate").val());
        if (sdate === "") {
            showErr("sdate", "Please select Date.");
            valid = false;
        } else {
            var d = new Date(sdate);
            var today = new Date();
            today.setHours(0, 0, 0, 0);
            if (isNaN(d.getTime())) {
                showErr("sdate", "Please enter a valid Date.");
                valid = false;
            } else if (d > today) {
                showErr("sdate", "Future date is not allowed.");
                valid = false;
            }
        }

        

        /* Party - Other selected ho to panel fields check */
        var partyVal = $.trim($("#<%= sPartyName.ClientID %> option:selected").text());
        if (partyVal === "Other") {

            var pname = $.trim($("#pName").val());
            if (pname === "") {
                showErr("pName", "Please enter Party Name.");
                valid = false;
            }

            var pmn = $.trim($("#pMN").val());
            if (pmn === "") {
                showErr("pMN", "Please enter Mobile No.");
                valid = false;
            }

            var pgst = $.trim($("#pGST").val());
            if (pgst === "") {
                showErr("pGST", "Please enter GSTIN.");
                valid = false;
            }

            var ppan = $.trim($("#pPAN").val());
            if (ppan === "") {
                showErr("pPAN", "Please enter PAN.");
                valid = false;
            }

            var paddr = $.trim($("#pAddress").val());
            if (paddr === "") {
                showErr("pAddress", "Please enter Address.");
                valid = false;
            }
        }

        /* Quantity */
        var qikg = $.trim($("#<%= QIKG.ClientID %>").val());
        if (qikg === "") {
            showErr("<%= QIKG.ClientID %>", "Please enter Quantity.");
            valid = false;
        } else if (isNaN(qikg) || parseFloat(qikg) <= 0) {
            showErr("<%= QIKG.ClientID %>", "Quantity must be a positive number.");
            valid = false;
        }

        /* Rate */
        var rate = $.trim($("#<%= avgrate.ClientID %>").val());
        if (rate === "") {
            showErr("<%= avgrate.ClientID %>", "Please enter Rate.");
            valid = false;
        } else if (isNaN(rate) || parseFloat(rate) <= 0) {
            showErr("<%= avgrate.ClientID %>", "Rate must be a positive number.");
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

    <!-- MAIN -->
    <div class="main-content">

        <div class="sale-wrapper">

            <div class="sale-box">

                <!-- TITLE -->
                <div class="sale-title">
                    <h2>Sale Sauda Entry</h2>
                    <p>Enter sale sauda details</p>
                </div>

                <!-- TOP ALERT -->
                <div id="topAlert" class="alert-custom alert-danger-custom"></div>

                <!-- ROW 1 : Date + Manual No + Supplier -->
                <div class="row">

                    <div class="col-md-4">
                        <div class="input-group-custom">
                            <label>Select Date <span style="color:#dc2626;">*</span></label>
                            <div class="input-box">
                                <i class="fa fa-calendar"></i>
                                <input id="sdate"
                                    runat="server"
                                    required
                                    class="form-control" />
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
                            <label>Supplier Ref.</label>
                            <div class="input-box">
                                <i class="fa fa-user"></i>
                                <asp:TextBox ID="txtEmpName"
                                    runat="server"
                                    CssClass="form-control">
                                </asp:TextBox>
                            </div>
                            <div class="err-msg" id="err_<%= txtEmpName.ClientID %>"></div>
                        </div>
                    </div>

                </div>

                <!-- ROW 2 : Party Dropdown + Sauda List -->
                <div class="row">

                    <div class="col-md-6">
                        <div class="input-group-custom">
                            <label>Party Name <span style="color:#dc2626;">*</span></label>
                            <div class="input-box">
                                <i class="fa fa-users"></i>
                                <asp:DropDownList ID="sPartyName"
                                    runat="server"
                                    CssClass="form-control"
                                    AutoPostBack="true"
                                    OnSelectedIndexChanged="sPartyName_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6" style="padding-top:38px;">
                        <asp:LinkButton ID="lBtnSaudaParty"
                            runat="server"
                            CssClass="btn btn-card"
                            OnClick="lBtnSaudaParty_Click">
                            Sauda List
                        </asp:LinkButton>
                    </div>

                </div>

                <!-- PANEL : Other Party Fields -->
                <asp:Panel ID="Panel1" runat="server">

                    <div class="row">

                        <div class="col-md-6">
                            <div class="input-group-custom">
                                <label>Party Name <span style="color:#dc2626;">*</span></label>
                                <div class="input-box">
                                    <i class="fa fa-user"></i>
                                    <input id="pName"
                                        type="text"
                                        runat="server"
                                        class="form-control" />
                                </div>
                                <div class="err-msg" id="err_pName"></div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="input-group-custom">
                                <label>Party Mobile No. <span style="color:#dc2626;">*</span></label>
                                <div class="input-box">
                                    <i class="fa fa-phone"></i>
                                    <input id="pMN"
                                        runat="server"
                                        class="form-control" />
                                </div>
                                <div class="err-msg" id="err_pMN"></div>
                            </div>
                        </div>

                    </div>

                    <div class="row">

                        <div class="col-md-6">
                            <div class="input-group-custom">
                                <label>Party GSTIN <span style="color:#dc2626;">*</span></label>
                                <div class="input-box">
                                    <i class="fa fa-file-invoice"></i>
                                    <input id="pGST"
                                        runat="server"
                                        type="text"
                                        class="form-control" />
                                </div>
                                <div class="err-msg" id="err_pGST"></div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="input-group-custom">
                                <label>Party PAN <span style="color:#dc2626;">*</span></label>
                                <div class="input-box">
                                    <i class="fa fa-id-card"></i>
                                    <input id="pPAN"
                                        runat="server"
                                        type="text"
                                        class="form-control" />
                                </div>
                                <div class="err-msg" id="err_pPAN"></div>
                            </div>
                        </div>

                    </div>

                    <div class="row">

                        <div class="col-md-12">
                            <div class="input-group-custom">
                                <label>Party Address <span style="color:#dc2626;">*</span></label>
                                <div class="input-box">
                                    <i class="fa fa-location-dot"></i>
                                    <textarea id="pAddress"
                                        runat="server"
                                        rows="3"
                                        class="form-control"></textarea>
                                </div>
                                <div class="err-msg" id="err_pAddress"></div>
                            </div>
                        </div>

                    </div>

                </asp:Panel>

                <!-- PRODUCT ROW : Item + Qty + Rate -->
                <div class="row">

                    <div class="col-md-4">
                        <div class="input-group-custom">
                            <label>Item Type <span style="color:#dc2626;">*</span></label>
                            <div class="input-box">
                                <i class="fa fa-seedling"></i>
                                <select id="sPaddyType"
                                    runat="server"
                                    class="form-control">
                                    <option value="">-- Select Item --</option>
                                    <option>Arwa Rice</option>
                                    <option>Rashmi Ka 7 Star</option>
                                    <option>Rashmi Ka Sonam</option>
                                    <option>7 Star Katarni</option>
                                    <option>Sri Rajbhog Rice</option>
                                    <option>Parmal Rice</option>
                                    <option>Steam Bran</option>
                                    <option>Naku</option>
                                    <option>Naku Bhusi</option>
                                    <option>Husk</option>
                                    <option>Broken</option>
                                    <option>Rejection</option>
                                    <option>Khakhri</option>
                                    <option>Dust</option>
                                    <option>PP Bag</option>
                                    <option>Jute Bag</option>
                                </select>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="input-group-custom">
                            <label>Quantity (KG) <span style="color:#dc2626;">*</span></label>
                            <div class="input-box">
                                <i class="fa fa-weight-hanging"></i>
                                <input id="QIKG"
                                    runat="server"
                                    class="form-control" />
                            </div>
                            <div class="err-msg" id="err_<%= QIKG.ClientID %>"></div>
                        </div>
                    </div>

                    <div class="col-md-4">
                        <div class="input-group-custom">
                            <label>Rate (&#8377;) <span style="color:#dc2626;">*</span></label>
                            <div class="input-box">
                                <i class="fa fa-indian-rupee-sign"></i>
                                <input id="avgrate"
                                    runat="server"
                                    class="form-control" />
                            </div>
                            <div class="err-msg" id="err_<%= avgrate.ClientID %>"></div>
                        </div>
                    </div>

                </div>

                <!-- BUTTONS -->
                <div class="text-center" style="margin-top:20px;">

                    <input type="submit"
                        id="btnContinue"
                        value="Click To Add"
                        runat="server"
                        class="btn btn-card"
                        onserverclick="btnContinue_ServerClick"
                        onclick="return validateContinue();" />

                    &nbsp;&nbsp;

                    <input type="submit"
                        id="Submit1"
                        value="Reset Data"
                        runat="server"
                        class="btn btn-card"
                        onserverclick="Submit1_ServerClick" />

                    &nbsp;&nbsp;

                    <input type="submit"
                        id="btnSave"
                        value="Click To Save"
                        runat="server"
                        class="btn btn-card"
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
