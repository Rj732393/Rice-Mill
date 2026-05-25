<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Expense.aspx.cs" Inherits="Expense" %>
<%@ Register Src="~/Includes/menu.ascx"
    TagName="Menu"
    TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Daily Expense</title>

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
    margin-left:110px;
    padding-top:90px;
    transition:0.3s;
}

.main-content.full{
    margin-left:0;
}

/* ===== FORM ===== */

.expense-wrapper{
    padding:30px;
}

.expense-box{
    background:white;
    border-radius:25px;
    padding:35px;
    box-shadow:0 8px 30px rgba(0,0,0,0.08);
}

.expense-title{
    text-align:center;
    margin-bottom:35px;
}

.expense-title h2{
    font-size:36px;
    font-weight:800;
    color:#1e293b;
}

.expense-title p{
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
    padding-top:12px !important;
    min-height:120px;
    resize:none;
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

    .expense-wrapper{
        padding:15px;
    }

    .expense-box{
        padding:20px;
    }

    .expense-title h2{
        font-size:28px;
    }

}

</style>

<script>

    function toggleSidebar() {

        

        $(".main-content").toggleClass("full");

    }

</script>

</head>

<body>

<form id="form1" runat="server">

<uc1:Menu ID="Menu1" runat="server" />

<!-- MAIN CONTENT -->

<div class="main-content">

    <div class="expense-wrapper">

        <div class="expense-box">

            <div class="expense-title">

                <h2>Daily Expense Entry</h2>

                <p>
                    Manage all daily rice mill expenses
                </p>

            </div>

            <!-- ROW 1 -->

            <div class="row">

                <div class="col-md-6">

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

                <div class="col-md-6"
                    style="padding-top:32px;">

                    <asp:LinkButton ID="lbrnData"
                        runat="server"
                        CssClass="btn btn-card"
                        onclick="lbrnData_Click">

                        Expense Report

                    </asp:LinkButton>

                </div>

            </div>

            <!-- ROW 2 -->

            <div class="row">

                <div class="col-md-12">

                    <div class="input-group-custom">

                        <label>Expense Type</label>

                        <div class="input-box">

                            <i class="fa fa-list"></i>

                            <asp:DropDownList ID="ddlExpenseType"
                                runat="server"
                                CssClass="form-control">

                                <asp:ListItem>Freight Exp</asp:ListItem>
                                <asp:ListItem>Truck</asp:ListItem>
                                <asp:ListItem>Electric Bill</asp:ListItem>
                                <asp:ListItem>Bank Interest</asp:ListItem>
                                <asp:ListItem>Salary to Staff ( Mill)</asp:ListItem>
                                <asp:ListItem>Repair & Maintenance</asp:ListItem>
                                <asp:ListItem>Mobile Recharge Exp</asp:ListItem>
                                <asp:ListItem>Petrol Exp</asp:ListItem>
                                <asp:ListItem>Misc.Exp</asp:ListItem>
                                <asp:ListItem>Other Exp</asp:ListItem>

                            </asp:DropDownList>

                        </div>

                    </div>

                </div>

            </div>

            <!-- ROW 3 -->

            <div class="row">

                <div class="col-md-6">

                    <div class="input-group-custom">

                        <label>Amount (₹)</label>

                        <div class="input-box">

                            <i class="fa fa-indian-rupee-sign"></i>

                            <input id="EAmount"
                                runat="server"
                                class="form-control"
                                required />

                        </div>

                    </div>

                </div>

            </div>

            <!-- ROW 4 -->

            <div class="row">

                <div class="col-md-12">

                    <div class="input-group-custom">

                        <label>Remarks</label>

                        <div class="input-box">

                            <i class="fa fa-note-sticky"></i>

                            <textarea id="ERemarks"
                                runat="server"
                                class="form-control"
                                required></textarea>

                        </div>

                    </div>

                </div>

            </div>

            <!-- BUTTON -->

            <div class="text-center"
                style="margin-top:20px;">

                <input type="submit"
                    id="btnSave"
                    value="Save Expense"
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