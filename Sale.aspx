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

.form-control{
    height:42px !important;
    border-radius:10px !important;
    border:1px solid #dbe2ea !important;
    padding-left:42px !important;
    box-shadow:none !important;
    font-size:13px !important;
}

.form-control:focus{
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

                <!-- ROW 1 -->

                <div class="row">

                    <div class="col-md-4">

                        <div class="input-group-custom">

                            <label>Sauda No</label>

                            <div class="input-box">

                                <i class="fa fa-file"></i>

                                <asp:TextBox ID="SaudaNo"
                                    runat="server"
                                    CssClass="form-control">
                                </asp:TextBox>

                            </div>

                        </div>

                    </div>

                    <div class="col-md-4">

                        <div class="input-group-custom">

                            <label>Sauda Date</label>

                            <div class="input-box">

                                <i class="fa fa-calendar"></i>

                                <asp:TextBox ID="SaudaDate"
                                    runat="server"
                                    CssClass="form-control">
                                </asp:TextBox>

                            </div>

                        </div>

                    </div>

                    <div class="col-md-4">

                        <div class="input-group-custom">

                            <label>Despatch No</label>

                            <div class="input-box">

                                <i class="fa fa-truck"></i>

                                <asp:TextBox ID="DespatchNo"
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

                            <label>PMN</label>

                            <div class="input-box">

                                <i class="fa fa-hashtag"></i>

                                <asp:TextBox ID="pMN"
                                    runat="server"
                                    CssClass="form-control">
                                </asp:TextBox>

                            </div>

                        </div>

                    </div>

                </div>

                <!-- BUTTON -->

                <div class="text-center"
                    style="margin-top:20px;">

                    <asp:Button ID="btnSave"
                        runat="server"
                        Text="Save Sale Entry"
                        CssClass="btn btn-card"
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