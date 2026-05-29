<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="EditData.aspx.cs"
    Inherits="admin_EditData" %>

<%@ Register Src="../Includes/AdminMenu.ascx"
    TagName="WebUserControl1"
    TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>Edit Data | Rashmi Rice Mills</title>

    <meta name="viewport"
        content="width=device-width, initial-scale=1" />

    <!-- Bootstrap -->
    <link rel="stylesheet"
        href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />

    <!-- Font Awesome -->
    <link rel="stylesheet"
        href="https://use.fontawesome.com/releases/v5.7.2/css/all.css" />

    <!-- Google Font -->
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap"
        rel="stylesheet" />

    <!-- Admin Menu CSS -->
    <link href="../CSS/AdminMenu.css"
        rel="stylesheet"
        type="text/css" />

    <!-- JQuery -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.2.1/jquery.min.js"></script>

    <!-- Bootstrap -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>

    <style type="text/css">

        body{
            background:#f1f5f9;
            font-family:'Poppins',sans-serif;
            overflow-x:hidden;
        }

        /* MAIN */

        .main-wrapper{
            margin-left:270px;
            margin-top:100px;
            padding:35px;
            transition:0.4s;
        }

        /* CARD */

        .dashboard-card{
            background:white;
            border-radius:24px;
            padding:40px;
            box-shadow:0 12px 35px rgba(0,0,0,0.08);
            animation:fadeIn 0.7s ease;
        }

        @keyframes fadeIn{

            from{
                opacity:0;
                transform:translateY(20px);
            }

            to{
                opacity:1;
                transform:translateY(0px);
            }
        }

        /* TITLE */

        .page-title{
            text-align:center;
            margin-bottom:35px;
        }

        .page-title h1{
            font-size:34px;
            font-weight:700;
            color:#1e293b;
            margin-bottom:8px;
        }

        .page-title p{
            color:#64748b;
            font-size:15px;
        }

        /* PANEL */

        .edit-panel{
            background:#ffffff;
            border-radius:18px;
            padding:25px;
            margin-bottom:25px;
            box-shadow:0 8px 20px rgba(0,0,0,0.05);
        }

        .edit-panel h3{
            font-size:24px;
            font-weight:700;
            color:#1e293b;
            margin-bottom:25px;
        }

        /* FORM */

        .form-control{
            height:48px;
            border-radius:12px;
            border:1px solid #cbd5e1;
            box-shadow:none;
            font-size:14px;
        }

        .form-control:focus{
            border-color:#2563eb;
            box-shadow:0 0 10px rgba(37,99,235,0.15);
        }

        /* BUTTONS */

        .btn-custom{
            border:none;
            padding:12px 28px;
            border-radius:12px;
            color:white;
            font-size:14px;
            font-weight:600;
            transition:0.3s;
        }

        .btn-load{
            background:linear-gradient(135deg,#2563eb,#06b6d4);
        }

        .btn-save{
            background:linear-gradient(135deg,#16a34a,#22c55e);
        }

        .btn-cancel{
            background:linear-gradient(135deg,#dc2626,#ef4444);
        }

        .btn-custom:hover{
            transform:translateY(-2px);
            opacity:0.9;
        }

        /* TABLE */

        .table{
            background:white;
            border-radius:14px;
            overflow:hidden;
        }

        .table th{
            background:#2563eb;
            color:white;
            text-align:center;
            padding:14px !important;
            font-size:14px;
        }

        .table td{
            text-align:center;
            vertical-align:middle !important;
            padding:12px !important;
            font-size:13px;
        }

        .table tr:hover{
            background:#f8fafc;
        }

        /* ALERTS */

        .alert-msg{
            padding:14px;
            border-radius:12px;
            margin-bottom:20px;
            font-weight:600;
        }

        .alert-success{
            background:#dcfce7;
            color:#166534;
        }

        .alert-error{
            background:#fee2e2;
            color:#991b1b;
        }

        /* RESPONSIVE */

        @media(max-width:900px){

            .main-wrapper{
                margin-left:0;
                margin-top:20px;
                padding:15px;
            }

            .dashboard-card{
                padding:25px;
            }

            .page-title h1{
                font-size:26px;
            }
        }

    </style>

</head>

<body>

<form id="form1" runat="server">

    <!-- ADMIN MENU -->
    <uc1:WebUserControl1 ID="WebUserControl11"
        runat="server" />

    <!-- MAIN CONTENT -->
    <div class="main-wrapper">

        <div class="dashboard-card">

            <!-- TITLE -->
            <div class="page-title">

                <h1>

                    <i class="fas fa-edit"
                        style="color:#2563eb;"></i>

                    Edit Database Records

                </h1>

                <p>
                    Rashmi Rice Mills Private Limited
                </p>

            </div>

            <!-- SELECT TABLE -->
            <div class="edit-panel">

                <h3>

                    <i class="fas fa-database"></i>

                    Select Table To Edit

                </h3>

                <div class="row">

                    <div class="col-md-6">

                        <label style="font-weight:600;margin-bottom:8px;">
                            Table Name
                        </label>

                        <asp:DropDownList ID="ddlTable"
                            runat="server"
                            CssClass="form-control">

                            <asp:ListItem Value="">-- Select Table --</asp:ListItem>
                            <asp:ListItem Value="prabha.Purchase_Party_Info">Purchase Party Info</asp:ListItem>
                            <asp:ListItem Value="prabha.Purchase_Sauda_Info">Purchase Sauda Info</asp:ListItem>
                            <asp:ListItem Value="prabha.Purchase_Item_Info">Purchase Item Info</asp:ListItem>
                            <asp:ListItem Value="prabha.Purchase_Master_Data">Purchase Master Data</asp:ListItem>
                            <asp:ListItem Value="prabha.Purchase_Payment_Info">Purchase Payment Info</asp:ListItem>
                            <asp:ListItem Value="prabha.Sale_Sauda_Master">Sale Sauda Master</asp:ListItem>
                            <asp:ListItem Value="prabha.Sale_Master_Data">Sale Master Data</asp:ListItem>
                            <asp:ListItem Value="prabha.Sale_Payment_Info">Sale Payment Info</asp:ListItem>
                            <asp:ListItem Value="prabha.PaddyProcessing">Paddy Processing</asp:ListItem>
                            <asp:ListItem Value="prabha.PaddyStock">Paddy Stock</asp:ListItem>
                            <asp:ListItem Value="prabha.RiceStock">Rice Stock</asp:ListItem>
                            <asp:ListItem Value="prabha.Expense_Info">Expense Info</asp:ListItem>
                            <asp:ListItem Value="prabha.SalePurchaseExpense">Sale Purchase Expense</asp:ListItem>
                            <asp:ListItem Value="prabha.UserInfo">User Info</asp:ListItem>

                        </asp:DropDownList>

                    </div>

                    <div class="col-md-3"
                        style="padding-top:25px;">

                        <input type="submit"
                            id="btnLoad"
                            runat="server"
                            value="Load Data"
                            onserverclick="btnLoad_ServerClick"
                            class="btn-custom btn-load" />

                    </div>

                </div>

            </div>

            <!-- MESSAGE -->
            <asp:PlaceHolder ID="phMessage"
                runat="server">
            </asp:PlaceHolder>

            <!-- DATA TABLE -->
            <div class="edit-panel"
                id="dataPanel"
                runat="server"
                visible="false">

                <h3>

                    <i class="fas fa-table"></i>

                    <asp:Label ID="lblTableTitle"
                        runat="server"
                        Text="">
                    </asp:Label>

                </h3>

                <div class="table-responsive">

                    <asp:PlaceHolder ID="phTable"
                        runat="server">
                    </asp:PlaceHolder>

                </div>

            </div>

            <!-- EDIT FORM -->
            <div class="edit-panel"
                id="editFormPanel"
                runat="server"
                visible="false">

                <h3>

                    <i class="fas fa-pen"></i>

                    Edit Record

                    <small>

                        <asp:Label ID="lblEditID"
                            runat="server"
                            Text=""
                            style="font-size:13px;color:#64748b;">
                        </asp:Label>

                    </small>

                </h3>

                <div class="row">

                    <asp:PlaceHolder ID="phEditForm"
                        runat="server">
                    </asp:PlaceHolder>

                </div>

                <br />

                <input type="hidden"
                    id="hdnEditID"
                    runat="server" />

                <input type="hidden"
                    id="hdnTableName"
                    runat="server" />

                <!-- BUTTONS -->
                <input type="submit"
                    id="btnSave"
                    runat="server"
                    value="Save Changes"
                    onserverclick="btnSave_ServerClick"
                    class="btn-custom btn-save" />

                &nbsp;

                <input type="submit"
                    id="btnCancelEdit"
                    runat="server"
                    value="Cancel"
                    onserverclick="btnCancelEdit_ServerClick"
                    class="btn-custom btn-cancel" />

            </div>

        </div>

    </div>

</form>

</body>
</html>