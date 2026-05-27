<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="Payment.aspx.cs"
    Inherits="Payment" %>

<%@ Register Src="~/Includes/menu.ascx"
    TagName="Menu"
    TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Payment Entry</title>

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

/* ===== MAIN CONTENT ===== */

.main-content{
    margin-left:120px;
    padding-top:130px;
    transition:0.3s;
}

.main-content.full{
    margin-left:0;
}

/* ===== FORM ===== */

.payment-wrapper{
    padding:30px;
}

.payment-box{
    background:white;
    border-radius:25px;
    padding:35px;
    box-shadow:0 8px 30px rgba(0,0,0,0.08);
}

.payment-title{
    text-align:center;
    margin-bottom:35px;
}

.payment-title h2{
    font-size:36px;
    font-weight:800;
    color:#1e293b;
}

.payment-title p{
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
select{
    height:42px !important;
    border-radius:10px !important;
    border:1px solid #dbe2ea !important;
    padding-left:42px !important;
    box-shadow:none !important;
    font-size:13px !important;
}

.form-control:focus,
select:focus{
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

    .payment-wrapper{
        padding:15px;
    }

    .payment-box{
        padding:20px;
    }

}

</style>

<script type="text/javascript">

    $(document).ready(function () {

        togglePanels();

        $("#paymentmode").change(function () {
            togglePanels();
        });

    });

    function togglePanels() {

        var mode = $("#paymentmode").val();

        if (mode == "Online") {

            $("#Panel2").show();
            $("#Panel3").show();

            $("#lblTransaction").html("Transaction ID");

        }
        else if (mode == "By Cheque") {

            $("#Panel2").show();
            $("#Panel3").hide();

            $("#lblTransaction").html("Cheque No. & Date");

        }
        else {

            $("#Panel2").show();
            $("#Panel3").hide();

            $("#lblTransaction").html("Receiver Name & Mobile No.");

        }
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

                <div class="payment-title">

                    <h2>Purchase Payment Entry</h2>

                    <p>
                        Enter payment and voucher details
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

                            <label>Manual Voucher No</label>

                            <div class="input-box">

                                <i class="fa fa-file"></i>

                                <input id="pvNo"
                                    runat="server"
                                    class="form-control" />

                            </div>

                        </div>

                    </div>

                    <div class="col-md-4">

                        <div class="input-group-custom">

                            <label>Previous Balance</label>

                            <div class="input-box">

                                <i class="fa fa-wallet"></i>

                                <asp:TextBox ID="lblOSB"
                                    runat="server"
                                    CssClass="form-control"
                                    ReadOnly="true">
                                </asp:TextBox>

                            </div>

                        </div>

                    </div>

                </div>

                <!-- ROW 2 -->

                <div class="row">

                    <div class="col-md-6">

                        <div class="input-group-custom">

                            <label>Select Party Name</label>

                            <div class="input-box">

                                <i class="fa fa-user"></i>

                                <asp:DropDownList ID="ddlParty"
                                    runat="server"
                                    CssClass="form-control"
                                    AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlParty_SelectedIndexChanged">
                                </asp:DropDownList>

                            </div>

                        </div>

                    </div>

                    <div class="col-md-6">

                        <div class="input-group-custom">

                            <label>Amount Paid</label>

                            <div class="input-box">

                                <i class="fa fa-indian-rupee-sign"></i>

                                <input id="amountpaid"
                                    runat="server"
                                    class="form-control" />

                            </div>

                        </div>

                    </div>

                </div>

                <!-- ROW 3 -->

                <div class="row">

                    <div class="col-md-6">

                        <div class="input-group-custom">

                            <label>Payment Mode</label>

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

                <!-- PANEL 3 -->

                <asp:Panel ID="Panel3"
                    runat="server"
                    Style="display:none;">

                    <div class="row">

                        <div class="col-md-6">

                            <div class="input-group-custom">

                                <label>A/C Name</label>

                                <div class="input-box">

                                    <i class="fa fa-user"></i>

                                    <input id="pACName"
                                        runat="server"
                                        class="form-control" />

                                </div>

                            </div>

                        </div>

                        <div class="col-md-6">

                            <div class="input-group-custom">

                                <label>A/C No</label>

                                <div class="input-box">

                                    <i class="fa fa-building-columns"></i>

                                    <input id="pACNo"
                                        runat="server"
                                        class="form-control" />

                                </div>

                            </div>

                        </div>

                    </div>

                    <div class="row">

                        <div class="col-md-6">

                            <div class="input-group-custom">

                                <label>Bank Name</label>

                                <div class="input-box">

                                    <i class="fa fa-landmark"></i>

                                    <asp:DropDownList ID="ddlBank"
                                        runat="server"
                                        CssClass="form-control">

                                        <asp:ListItem>--Select Bank--</asp:ListItem>
                                        <asp:ListItem>State Bank of India</asp:ListItem>
                                        <asp:ListItem>HDFC Bank Ltd</asp:ListItem>
                                        <asp:ListItem>ICICI Bank Ltd.</asp:ListItem>
                                        <asp:ListItem>Axis Bank Ltd.</asp:ListItem>
                                        <asp:ListItem>Punjab National Bank</asp:ListItem>

                                    </asp:DropDownList>

                                </div>

                            </div>

                        </div>

                        <div class="col-md-6">

                            <div class="input-group-custom">

                                <label>Bank IFSC Code</label>

                                <div class="input-box">

                                    <i class="fa fa-code"></i>

                                    <input id="pBankIFSC"
                                        runat="server"
                                        class="form-control" />

                                </div>

                            </div>

                        </div>

                    </div>

                </asp:Panel>

                <!-- PANEL 2 -->

                <asp:Panel ID="Panel2"
                    runat="server"
                    Style="display:none;">

                    <div class="row">

                        <div class="col-md-12">

                            <div class="input-group-custom">

                                <label id="lblTransaction">

                                    Transaction ID

                                </label>

                                <div class="input-box">

                                    <i class="fa fa-receipt"></i>

                                    <input id="transaction"
                                        runat="server"
                                        class="form-control" />

                                </div>

                            </div>

                        </div>

                    </div>

                </asp:Panel>

                <!-- BUTTONS -->

                <div class="text-center"
                    style="margin-top:20px;">

                    <input type="submit"
                        id="btnContinue"
                        runat="server"
                        value="Add Payment"
                        class="btn btn-card"
                        onserverclick="btnContinue_ServerClick" />

                    &nbsp;&nbsp;

                    <input type="submit"
                        id="btnSave"
                        runat="server"
                        value="Submit Payment"
                        class="btn btn-card"
                        onserverclick="btnSave_ServerClick" />

                </div>

                <!-- TABLE -->

                <div class="table-box">

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