```aspx
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
    font-family:'Segoe UI',sans-serif;
    background:#f4f7fb;
    overflow-x:hidden;
}

/* ===== MAIN CONTENT ===== */

.main-content{
    margin-left:120px;
    padding-top:120px;
    transition:0.3s;
}

.main-content.full{
    margin-left:0;
}

/* ===== WRAPPER ===== */

.payment-wrapper{
    padding:30px;
}

/* ===== BOX ===== */

.payment-box{
    background:#ffffff;
    border-radius:24px;
    padding:35px;
    box-shadow:0 10px 35px rgba(0,0,0,0.07);
    border:1px solid #eef2f7;
}

/* ===== TITLE ===== */

.payment-title{
    text-align:center;
    margin-bottom:35px;
}

.payment-title h2{
    font-size:34px;
    font-weight:800;
    color:#14532d;
    margin-bottom:8px;
}

.payment-title p{
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
    border-radius:14px !important;
    padding:12px 28px !important;
    font-size:14px !important;
    font-weight:700 !important;
    letter-spacing:0.3px;
    box-shadow:0 6px 16px rgba(22,163,74,0.22);
    transition:all 0.3s ease;
    outline:none !important;
}

.btn-card:hover{
    background:linear-gradient(135deg,#15803d,#166534) !important;
    color:#ffffff !important;
    transform:translateY(-2px);
    box-shadow:0 10px 24px rgba(22,163,74,0.35);
}

.btn-card:focus{
    color:#ffffff !important;
    outline:none !important;
}

.btn-card:active{
    transform:scale(0.98);
}

/* ===== BUTTON COLORS ===== */

#btnContinue{
    background:linear-gradient(135deg,#16a34a,#15803d) !important;
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

/* ===== TABLE ===== */

.table-box{
    background:#ffffff;
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

    .payment-title h2{
        font-size:28px;
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

            $("#Panel2").hide();
            $("#Panel3").hide();

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

                    <!-- BANK ROW -->

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
                    style="margin-top:25px;">

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

                    &nbsp;&nbsp;

                    <input type="reset"
                        id="btnReset"
                        value="Reset Form"
                        class="btn btn-card" />

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
```
