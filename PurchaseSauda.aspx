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

/* ===== MAIN CONTENT ===== */

.main-content{
    margin-left:120px;
    padding:110px 25px 30px;
    transition:0.3s;
}

.main-content.full{
    margin-left:0;
}

/* ===== WRAPPER ===== */

.purchase-wrapper{
    max-width:1400px;
    margin:auto;
}

/* ===== BOX ===== */

.purchase-box{
    background:#ffffff;
    border-radius:24px;
    padding:35px;
    box-shadow:0 8px 28px rgba(0,0,0,0.08);
    border:1px solid #e5e7eb;
}

/* ===== TITLE ===== */

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

/* ===== INPUT ===== */

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

/* ===== BUTTON ===== */

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
}

.btn-card:hover{
    background:linear-gradient(135deg,#15803d,#166534) !important;
    transform:translateY(-2px);
    color:#fff !important;
}

/* ===== BUTTON COLORS ===== */

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

/* ===== TABLE ===== */

.table-box{
    background:#ffffff;
    border-radius:18px;
    padding:20px;
    margin-top:30px;
    box-shadow:0 5px 18px rgba(0,0,0,0.06);
    overflow:auto;
    border:1px solid #e5e7eb;
}

/* ===== MOBILE ===== */

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

            $("#form1")[0].reset();

            return false;
        });

    });

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

                    <p>
                        Enter paddy purchase details
                    </p>

                </div>

                <!-- ROW 1 -->
                <div class="row">

                    <div class="col-md-4">

                        <div class="input-group-custom">

                            <label>Select Date</label>

                            <div class="input-box">

                                <i class="fa fa-calendar"></i>

                                <input id="sdate"
                                    runat="server"
                                    class="form-control"
                                    required />

                            </div>

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
                                    CssClass="form-control"
                                    required>
                                </asp:TextBox>

                            </div>

                        </div>

                    </div>

                </div>

                <!-- ROW 2 -->
                <div class="row">

                    <div class="col-md-6">

                        <div class="input-group-custom">

                            <label>Party Name</label>

                            <div class="input-box">

                                <i class="fa fa-users"></i>

                                <asp:DropDownList ID="sPartyName"
                                    runat="server"
                                    CssClass="form-control"
                                    AutoPostBack="true"
                                    onselectedindexchanged="sPartyName_SelectedIndexChanged"
                                    required>
                                </asp:DropDownList>

                            </div>

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

                <!-- PANEL -->
                <asp:Panel ID="Panel1" runat="server">

                    <div class="row">

                        <div class="col-md-6">

                            <div class="input-group-custom">

                                <label>Party Name</label>

                                <div class="input-box">

                                    <i class="fa fa-user"></i>

                                    <input id="pName"
                                        runat="server"
                                        type="text"
                                        class="form-control"
                                        required />

                                </div>

                            </div>

                        </div>

                        <div class="col-md-6">

                            <div class="input-group-custom">

                                <label>Party Mobile No.</label>

                                <div class="input-box">

                                    <i class="fa fa-phone"></i>

                                    <input id="pMN"
                                        runat="server"
                                        class="form-control"
                                        required />

                                </div>

                            </div>

                        </div>

                    </div>

                </asp:Panel>

                <!-- ROW 3 -->
                <div class="row">

                    <div class="col-md-4">

                        <div class="input-group-custom">

                            <label>Paddy Type</label>

                            <div class="input-box">

                                <i class="fa fa-seedling"></i>

                                <select id="sPaddyType"
                                    runat="server"
                                    class="form-control"
                                    required>

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

                            <label>Quantity (KG)</label>

                            <div class="input-box">

                                <i class="fa fa-weight-hanging"></i>

                                <input id="QIKG"
                                    runat="server"
                                    class="form-control"
                                    required />

                            </div>

                        </div>

                    </div>

                    <div class="col-md-4">

                        <div class="input-group-custom">

                            <label>Rate (₹)</label>

                            <div class="input-box">

                                <i class="fa fa-indian-rupee-sign"></i>

                                <input id="avgrate"
                                    runat="server"
                                    class="form-control"
                                    required />

                            </div>

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
                        onserverclick="btnContinue_ServerClick" />

                    <input type="submit"
                        id="btnSave"
                        value="Save Purchase Sauda"
                        runat="server"
                        class="btn btn-card"
                        onserverclick="btnSave_ServerClick" />

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
