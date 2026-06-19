<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="OperatorsList.aspx.cs"
    Inherits="admin_OperatorsList" %>

<%@ Register Src="../Includes/AdminMenu.ascx"
    TagName="WebUserControl1"
    TagPrefix="uc1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>Operators List</title>

    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="stylesheet"
        href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />

    <link rel="stylesheet"
        href="https://use.fontawesome.com/releases/v5.7.2/css/all.css" />

    <link href="../CSS/AdminMenu.css" rel="stylesheet" />

    <style>

        body{ background:#f1f5f9; }

        .main-wrapper{
            margin-left:270px;
            margin-top:95px;
            padding:30px;
        }

        .card-box{
            background:#fff;
            padding:25px;
            border-radius:20px;
        }

        .card-box h3{
            margin-bottom:18px;
            color:#1e293b;
        }

        .table thead th{
            background:#1e293b;
            color:#fff;
            border:none;
        }

        .badge-active{
            background:#16a34a;
            color:#fff;
            padding:5px 10px;
            border-radius:10px;
            font-size:12px;
        }

        .badge-inactive{
            background:#dc2626;
            color:#fff;
            padding:5px 10px;
            border-radius:10px;
            font-size:12px;
        }

        .reset-box{
            display:inline-block;
        }

        .reset-box .form-control{
            display:inline-block;
            width:140px;
            height:32px;
            padding:4px 8px;
            font-size:13px;
        }

    </style>

</head>

<body>

<form id="form1" runat="server">

<uc1:WebUserControl1 ID="WebUserControl11" runat="server" />

<div class="main-wrapper">

    <div class="card-box">

        <h3>Operators List</h3>

        <asp:Label ID="lblMsg" runat="server" Font-Bold="true" /><br /><br />

        <div class="table-responsive">

            <asp:GridView ID="gvOperators" runat="server"
                CssClass="table table-striped table-bordered"
                AutoGenerateColumns="false"
                DataKeyNames="ID"
                OnRowCommand="gvOperators_RowCommand"
                EmptyDataText="Koi operator nahi mila.">

                <Columns>

                    <asp:BoundField DataField="UserName" HeaderText="Username" />

                    <asp:BoundField DataField="CreatedDate" HeaderText="Created Date"
                        DataFormatString="{0:dd-MM-yyyy}" />

                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label runat="server"
                                CssClass='<%# (bool)Eval("IsActive") ? "badge-active" : "badge-inactive" %>'
                                Text='<%# (bool)Eval("IsActive") ? "Active" : "Inactive" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Reset Password">
                        <ItemTemplate>
                            <div class="reset-box">
                                <asp:TextBox ID="txtNewPass" runat="server"
                                    CssClass="form-control"
                                    placeholder="New password" />

                                <asp:Button ID="btnReset" runat="server"
                                    Text="Reset"
                                    CssClass="btn btn-warning btn-sm"
                                    CommandName="ResetPassword"
                                    CommandArgument='<%# Eval("ID") %>' />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:Button runat="server"
                                Text='<%# (bool)Eval("IsActive") ? "Deactivate" : "Activate" %>'
                                CssClass='<%# (bool)Eval("IsActive") ? "btn btn-danger btn-sm" : "btn btn-success btn-sm" %>'
                                CommandName="ToggleActive"
                                CommandArgument='<%# Eval("ID") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

            </asp:GridView>

        </div>

        <a href="AddOperator.aspx" class="btn btn-primary">+ Add New Operator</a>

    </div>

</div>

</form>

</body>
</html>
