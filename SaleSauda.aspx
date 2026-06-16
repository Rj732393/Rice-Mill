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

/* ===== BUTTON ===== */

.btn-card{
    background:#f97316;
    color:white !important;
    border:none;
    border-radius:30px;
    padding:12px 28px;
    font-size:15px;
    font-weight:bold;
    transition:0.3s;
}

.btn-card:hover{
    background:#ea580c;
    color:white !important;
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

                            <label>Select Date</label>

                            <div class="input-box">

                                <i class="fa fa-calendar"></i>

                                <input id="sdate"
                                    runat="server"
                                    class="form-control" />

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
                                    CssClass="form-control">
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

                                <label>Party Name</label>

                                <div class="input-box">

                                    <i class="fa fa-user"></i>

                                    <input id="pName"
                                        type="text"
                                        runat="server"
                                        class="form-control" />

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
                                        class="form-control" />

                                </div>

                            </div>

                        </div>

                    </div>

                    <div class="row">

                        <div class="col-md-6">

                            <div class="input-group-custom">

                                <label>Party GSTIN</label>

                                <div class="input-box">

                                    <i class="fa fa-file-invoice"></i>

                                    <input id="pGST"
                                        runat="server"
                                        type="text"
                                        class="form-control" />

                                </div>

                            </div>

                        </div>

                        <div class="col-md-6">

                            <div class="input-group-custom">

                                <label>Party PAN</label>

                                <div class="input-box">

                                    <i class="fa fa-id-card"></i>

                                    <input id="pPAN"
                                        runat="server"
                                        type="text"
                                        class="form-control" />

                                </div>

                            </div>

                        </div>

                    </div>

                    <div class="row">

                        <div class="col-md-12">

                            <div class="input-group-custom">

                                <label>Party Address</label>

                                <div class="input-box">

                                    <i class="fa fa-location-dot"></i>

                                    <textarea id="pAddress"
                                        runat="server"
                                        rows="3"
                                        class="form-control"></textarea>

                                </div>

                            </div>

                        </div>

                    </div>

                </asp:Panel>

                <!-- PRODUCT ROW -->

                <div class="row">

                    <div class="col-md-4">

                        <div class="input-group-custom">

                            <label>Item Type</label>

                            <div class="input-box">

                                <i class="fa fa-seedling"></i>

                                <select id="sPaddyType"
                                    runat="server"
                                    class="form-control">

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

                            <label>Quantity (KG)</label>

                            <div class="input-box">

                                <i class="fa fa-weight-hanging"></i>

                                <input id="QIKG"
                                    runat="server"
                                    class="form-control" />

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
                                    class="form-control" />

                            </div>

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
                        onserverclick="btnContinue_ServerClick" />

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
                        onserverclick="btnSave_ServerClick" />

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