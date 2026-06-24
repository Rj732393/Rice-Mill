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

.sale-wrapper{
    padding:30px;
}

.sale-box{
    background:white;
    border-radius:25px;
    padding:35px;
    box-shadow:0 8px 30px rgba(0,0,0,0.08);
}

.sale-title{
    text-align:center;
    margin-bottom:35px;
}

.sale-title h2{
    font-size:36px;
    font-weight:800;
    color:#1e293b;
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
    padding-top:12px;
}

.form-control:focus,
select:focus,
textarea:focus{
    border-color:#f97316 !important;
    box-shadow:0 0 0 4px rgba(249,115,22,0.12) !important;
}

/* ===== VALIDATION ===== */

.val-msg{
    color:#dc2626;
    font-size:12px;
    margin-top:4px;
    display:none;
}

.val-msg.show{
    display:block;
}

.field-error{
    border-color:#dc2626 !important;
    box-shadow:0 0 0 3px rgba(220,38,38,0.10) !important;
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

    .sale-wrapper{
        padding:15px;
    }

    .sale-box{
        padding:20px;
    }

}

</style>

<script type="text/javascript">

    $(document).ready(function () {

        SearchText();

    });

    function SearchText() {

        $("#txtEmpName").autocomplete({

            source: function (request, response) {

                $.ajax({

                    type: "POST",

                    contentType: "application/json; charset=utf-8",

                    url: "PurchaseUnloading.aspx/GetEmployeeName",

                    data: "{'empName':'" + document.getElementById('txtEmpName').value + "'}",

                    dataType: "json",

                    success: function (data) {

                        response(data.d);

                    }

                });

            }

        });

    }

    /* ===== CLIENT-SIDE VALIDATION ===== */

    function validateContinue() {

        var valid = true;

        /* Date */
        var sdate = $.trim($("#<%= sdate.ClientID %>").val());
        if (sdate === "") {
            $("#errDate").text("Date zaroori hai.").addClass("show");
            $("#<%= sdate.ClientID %>").addClass("field-error");
            valid = false;
        } else {
            var d = new Date(sdate);
            var today = new Date();
            today.setHours(0, 0, 0, 0);
            if (isNaN(d.getTime())) {
                $("#errDate").text("Valid date enter karein.").addClass("show");
                $("#<%= sdate.ClientID %>").addClass("field-error");
                valid = false;
            } else if (d > today) {
                $("#errDate").text("Future date allowed nahi hai.").addClass("show");
                $("#<%= sdate.ClientID %>").addClass("field-error");
                valid = false;
            } else {
                $("#errDate").text("").removeClass("show");
                $("#<%= sdate.ClientID %>").removeClass("field-error");
            }
        }

        /* Supplier Ref */
        var emp = $.trim($("#<%= txtEmpName.ClientID %>").val());
        if (emp === "") {
            $("#errEmp").text("Supplier Ref. zaroori hai.").addClass("show");
            $("#<%= txtEmpName.ClientID %>").addClass("field-error");
            valid = false;
        } else {
            $("#errEmp").text("").removeClass("show");
            $("#<%= txtEmpName.ClientID %>").removeClass("field-error");
        }

        /* Party - Other selected ho to panel fields check */
        var partyVal = $.trim($("#<%= sPartyName.ClientID %> option:selected").text());
        if (partyVal === "Other") {

            var pname = $.trim($("#<%= pName.ClientID %>").val());
            if (pname === "") {
                $("#errPName").text("Party Name zaroori hai.").addClass("show");
                $("#<%= pName.ClientID %>").addClass("field-error");
                valid = false;
            } else {
                $("#errPName").text("").removeClass("show");
                $("#<%= pName.ClientID %>").removeClass("field-error");
            }

            var pmn = $.trim($("#<%= pMN.ClientID %>").val());
            if (pmn === "") {
                $("#errPMN").text("Mobile No. zaroori hai.").addClass("show");
                $("#<%= pMN.ClientID %>").addClass("field-error");
                valid = false;
            } else {
                $("#errPMN").text("").removeClass("show");
                $("#<%= pMN.ClientID %>").removeClass("field-error");
            }

            var pgst = $.trim($("#<%= pGST.ClientID %>").val());
            if (pgst === "") {
                $("#errPGST").text("GSTIN zaroori hai.").addClass("show");
                $("#<%= pGST.ClientID %>").addClass("field-error");
                valid = false;
            } else {
                $("#errPGST").text("").removeClass("show");
                $("#<%= pGST.ClientID %>").removeClass("field-error");
            }

            var ppan = $.trim($("#<%= pPAN.ClientID %>").val());
            if (ppan === "") {
                $("#errPPAN").text("PAN zaroori hai.").addClass("show");
                $("#<%= pPAN.ClientID %>").addClass("field-error");
                valid = false;
            } else {
                $("#errPPAN").text("").removeClass("show");
                $("#<%= pPAN.ClientID %>").removeClass("field-error");
            }

            var paddr = $.trim($("#<%= pAddress.ClientID %>").val());
            if (paddr === "") {
                $("#errPAddr").text("Address zaroori hai.").addClass("show");
                $("#<%= pAddress.ClientID %>").addClass("field-error");
                valid = false;
            } else {
                $("#errPAddr").text("").removeClass("show");
                $("#<%= pAddress.ClientID %>").removeClass("field-error");
            }

        }

        /* Quantity */
        var qikg = $.trim($("#<%= QIKG.ClientID %>").val());
        if (qikg === "") {
            $("#errQIKG").text("Quantity zaroori hai.").addClass("show");
            $("#<%= QIKG.ClientID %>").addClass("field-error");
            valid = false;
        } else if (isNaN(qikg) || parseFloat(qikg) <= 0) {
            $("#errQIKG").text("Quantity ek positive number hona chahiye.").addClass("show");
            $("#<%= QIKG.ClientID %>").addClass("field-error");
            valid = false;
        } else {
            $("#errQIKG").text("").removeClass("show");
            $("#<%= QIKG.ClientID %>").removeClass("field-error");
        }

        /* Rate */
        var rate = $.trim($("#<%= avgrate.ClientID %>").val());
        if (rate === "") {
            $("#errRate").text("Rate zaroori hai.").addClass("show");
            $("#<%= avgrate.ClientID %>").addClass("field-error");
            valid = false;
        } else if (isNaN(rate) || parseFloat(rate) <= 0) {
            $("#errRate").text("Rate ek positive number hona chahiye.").addClass("show");
            $("#<%= avgrate.ClientID %>").addClass("field-error");
            valid = false;
        } else {
            $("#errRate").text("").removeClass("show");
            $("#<%= avgrate.ClientID %>").removeClass("field-error");
        }

        return valid;

    }

    function validateSave() {

        var tableDiv = document.getElementById("prntContent");
        if (tableDiv) {
            var tables = tableDiv.getElementsByTagName("table");
            if (tables.length === 0) {
                alert("Pehle data add karein, phir save karein!");
                return false;
            }
        }
        return true;

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

                    <h2>Sale Sauda Entry</h2>

                    <p>
                        Enter sale sauda details
                    </p>

                </div>

                <!-- ROW 1 -->

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

                            <span id="errDate" class="val-msg"></span>

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

                            <label>Supplier Ref. <span style="color:#dc2626;">*</span></label>

                            <div class="input-box">

                                <i class="fa fa-user"></i>

                                <asp:TextBox ID="txtEmpName"
                                    runat="server"
                                    required="required"
                                    CssClass="form-control">
                                </asp:TextBox>

                            </div>

                            <span id="errEmp" class="val-msg"></span>

                        </div>

                    </div>

                </div>

                <!-- ROW 2 -->

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
                                    onselectedindexchanged="sPartyName_SelectedIndexChanged">
                                </asp:DropDownList>

                            </div>

                        </div>

                    </div>

                    <div class="col-md-6"
                        style="padding-top:38px;">

                        <asp:LinkButton ID="lBtnSaudaParty"
                            runat="server"
                            CssClass="btn btn-card"
                            onclick="lBtnSaudaParty_Click">

                            Sauda List

                        </asp:LinkButton>

                    </div>

                </div>

                <!-- PANEL -->

                <asp:Panel ID="Panel1"
                    runat="server">

                    <div class="row">

                        <div class="col-md-6">

                            <div class="input-group-custom">

                                <label>Party Name <span style="color:#dc2626;">*</span></label>

                                <div class="input-box">

                                    <i class="fa fa-user"></i>

                                    <input id="pName"
                                        type="text"
                                        runat="server"
                                        required
                                        class="form-control" />

                                </div>

                                <span id="errPName" class="val-msg"></span>

                            </div>

                        </div>

                        <div class="col-md-6">

                            <div class="input-group-custom">

                                <label>Party Mobile No. <span style="color:#dc2626;">*</span></label>

                                <div class="input-box">

                                    <i class="fa fa-phone"></i>

                                    <input id="pMN"
                                        runat="server"
                                        required
                                        class="form-control" />

                                </div>

                                <span id="errPMN" class="val-msg"></span>

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
                                        required
                                        class="form-control" />

                                </div>

                                <span id="errPGST" class="val-msg"></span>

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
                                        required
                                        class="form-control" />

                                </div>

                                <span id="errPPAN" class="val-msg"></span>

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
                                        required
                                        class="form-control"></textarea>

                                </div>

                                <span id="errPAddr" class="val-msg"></span>

                            </div>

                        </div>

                    </div>

                </asp:Panel>

                <!-- PRODUCT ROW -->

                <div class="row">

                    <div class="col-md-4">

                        <div class="input-group-custom">

                            <label>Item Type <span style="color:#dc2626;">*</span></label>

                            <div class="input-box">

                                <i class="fa fa-seedling"></i>

                                <select id="sPaddyType"
                                    runat="server"
                                    required
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
                                    required
                                    class="form-control" />

                            </div>

                            <span id="errQIKG" class="val-msg"></span>

                        </div>

                    </div>

                    <div class="col-md-4">

                        <div class="input-group-custom">

                            <label>Rate (&#8377;) <span style="color:#dc2626;">*</span></label>

                            <div class="input-box">

                                <i class="fa fa-indian-rupee-sign"></i>

                                <input id="avgrate"
                                    runat="server"
                                    required
                                    class="form-control" />

                            </div>

                            <span id="errRate" class="val-msg"></span>

                        </div>

                    </div>

                </div>

                <!-- BUTTONS -->

                <div class="text-center"
                    style="margin-top:20px;">

                    <input type="submit"
                        id="btnContinue"
                        value="Click To Add"
                        runat="server"
                        class="btn btn-card"
                        onserverclick="btnContinue_ServerClick"
                        onclientclick="return validateContinue();" />

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
                        onclientclick="return validateSave();" />

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
