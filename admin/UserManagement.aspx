<%@ Page Language="C#" AutoEventWireup="true"
    CodeFile="UserManagement.aspx.cs"
    Inherits="admin_UserManagement" %>

<%@ Register Src="../Includes/AdminMenu.ascx"
    TagName="WebUserControl1"
    TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN"
"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">

    <title>User Management | Rashmi Rice Mills</title>

    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css" />
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap" rel="stylesheet" />
    <link href="../CSS/AdminMenu.css" rel="stylesheet" type="text/css" />

    <style type="text/css">
        * { margin:0; padding:0; box-sizing:border-box; font-family:'Poppins',sans-serif; }
        body { background:#f1f5f9; overflow-x:hidden; }
        .main-wrapper { margin-left:270px; margin-top:95px; padding:30px; }

        .page-header {
            display:flex; align-items:center; justify-content:space-between;
            margin-bottom:25px;
        }
        .page-header h1 { font-size:28px; font-weight:700; color:#1e293b; }

        /* FORM BOX */
        .form-box {
            background:white; border-radius:20px; padding:28px;
            margin-bottom:28px; box-shadow:0 8px 20px rgba(0,0,0,0.06);
        }
        .form-box h3 { font-size:18px; font-weight:600; color:#1e293b; margin-bottom:20px; }

        .form-row-custom { display:flex; flex-wrap:wrap; gap:16px; margin-bottom:16px; }
        .form-group-custom { flex:1 1 200px; }
        .form-group-custom label { font-size:13px; font-weight:600; color:#475569; margin-bottom:6px; display:block; }
        .form-control-custom {
            width:100%; height:40px; border-radius:10px;
            border:1px solid #cbd5e1; padding:6px 12px;
            font-size:13px; background:white; font-family:'Poppins',sans-serif;
        }
        .form-control-custom:focus { outline:none; border-color:#2563eb; box-shadow:0 0 8px rgba(37,99,235,0.2); }

        .btn-save {
            background:linear-gradient(135deg,#2563eb,#06b6d4);
            border:none; color:white; padding:10px 28px;
            border-radius:12px; font-size:14px; font-weight:600; cursor:pointer;
        }
        .btn-save:hover { opacity:0.9; }
        .btn-cancel-form {
            background:#f1f5f9; border:1px solid #cbd5e1;
            color:#475569; padding:10px 20px;
            border-radius:12px; font-size:14px; cursor:pointer; margin-left:8px;
        }

        .msg-success { color:#16a34a; font-weight:600; font-size:14px; margin-bottom:12px; }
        .msg-error   { color:#dc2626; font-weight:600; font-size:14px; margin-bottom:12px; }

        /* USER TABLE */
        .table-box {
            background:white; border-radius:20px; padding:25px;
            box-shadow:0 8px 20px rgba(0,0,0,0.06);
        }
        .table-box h3 { font-size:18px; font-weight:600; color:#1e293b; margin-bottom:18px; }

        .user-table { width:100%; border-collapse:collapse; }
        .user-table th {
            background:#f8fafc; padding:11px 14px;
            font-size:13px; color:#475569; text-align:left;
            border-bottom:1px solid #e2e8f0;
        }
        .user-table td { padding:11px 14px; font-size:13px; border-bottom:1px solid #f1f5f9; vertical-align:middle; }
        .user-table tr:last-child td { border-bottom:none; }
        .user-table tr:hover td { background:#f8fafc; }

        .badge-active   { background:#f0fdf4; color:#16a34a; border-radius:6px; padding:3px 10px; font-size:11px; font-weight:600; }
        .badge-inactive { background:#fef2f2; color:#dc2626; border-radius:6px; padding:3px 10px; font-size:11px; font-weight:600; }

        .role-badge {
            background:#eff6ff; color:#2563eb;
            border-radius:6px; padding:3px 10px; font-size:11px; font-weight:600;
        }

        .btn-edit {
            background:#eff6ff; color:#2563eb; border:none;
            border-radius:8px; padding:5px 12px; font-size:12px; cursor:pointer;
        }
        .btn-deactivate {
            background:#fef2f2; color:#dc2626; border:none;
            border-radius:8px; padding:5px 12px; font-size:12px; cursor:pointer; margin-left:5px;
        }
        .btn-activate {
            background:#f0fdf4; color:#16a34a; border:none;
            border-radius:8px; padding:5px 12px; font-size:12px; cursor:pointer; margin-left:5px;
        }

        .no-data { text-align:center; color:#94a3b8; padding:30px; font-size:14px; }

        .hint-text { font-size:12px; color:#94a3b8; margin-top:4px; }

        @media(max-width:900px) {
            .main-wrapper { margin-left:0; padding:15px; margin-top:20px; }
            .form-row-custom { flex-direction:column; }
        }
    </style>

</head>

<body>

<form id="form1" runat="server">

    <uc1:WebUserControl1 ID="WebUserControl11" runat="server" />

    <!-- Hidden field to track editing UserID -->
    <asp:HiddenField ID="hdnUserID" runat="server" Value="0" />

    <div class="main-wrapper">

        <!-- PAGE HEADER -->
        <div class="page-header">
            <h1><i class="fas fa-users-cog" style="color:#2563eb;"></i> User Management</h1>
            <a href="Dashboard.aspx" class="btn btn-default btn-sm">
                <i class="fas fa-arrow-left"></i> Dashboard
            </a>
        </div>

        <!-- MESSAGE -->
        <asp:Label ID="lblMsg" runat="server" CssClass="msg-success" Visible="false" />

        <!-- ADD / EDIT FORM -->
        <div class="form-box">
            <h3><asp:Label ID="lblFormTitle" runat="server" Text="New User Add Karein" /></h3>

            <div class="form-row-custom">
                <div class="form-group-custom">
                    <label>Full Name</label>
                    <asp:TextBox ID="txtFullName" runat="server"
                        CssClass="form-control-custom" placeholder="Poora naam likhein" />
                </div>
                <div class="form-group-custom">
                    <label>Username *</label>
                    <asp:TextBox ID="txtUserName" runat="server"
                        CssClass="form-control-custom" placeholder="Login username" />
                </div>
            </div>

            <div class="form-row-custom">
                <div class="form-group-custom">
                    <label>Password *</label>
                    <asp:TextBox ID="txtPassword" runat="server"
                        CssClass="form-control-custom" TextMode="Password" placeholder="Password" />
                    <div class="hint-text">Edit ke time blank chhodo to password nahi badlega</div>
                </div>
                <div class="form-group-custom">
                    <label>Role *</label>
                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control-custom">
                        <asp:ListItem Value="">-- Role chunein --</asp:ListItem>
                        <asp:ListItem Value="2">Company Admin</asp:ListItem>
                        <asp:ListItem Value="3">Manager</asp:ListItem>
                        <asp:ListItem Value="4">Operator</asp:ListItem>
                        <asp:ListItem Value="5">Accountant</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="form-row-custom">
                <div class="form-group-custom">
                    <label>Email</label>
                    <asp:TextBox ID="txtEmail" runat="server"
                        CssClass="form-control-custom" placeholder="user@email.com" TextMode="Email" />
                </div>
                <div class="form-group-custom">
                    <label>Mobile</label>
                    <asp:TextBox ID="txtMobile" runat="server"
                        CssClass="form-control-custom" placeholder="Mobile number" MaxLength="15" />
                </div>
            </div>

            <asp:Button ID="btnSave" runat="server" Text="User Save Karein"
                CssClass="btn-save" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel"
                CssClass="btn-cancel-form" OnClick="btnCancelEdit_Click" Visible="false" />
        </div>

        <!-- USERS LIST TABLE -->
        <div class="table-box">
            <h3><i class="fas fa-list" style="color:#2563eb;"></i> Company Users</h3>

            <asp:Panel ID="pnlTable" runat="server">
                <div style="overflow-x:auto;">
                    <table class="user-table">
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Username</th>
                                <th>Full Name</th>
                                <th>Role</th>
                                <th>Email</th>
                                <th>Mobile</th>
                                <th>Status</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptUsers" runat="server"
                                OnItemCommand="rptUsers_ItemCommand">
                                <ItemTemplate>
                                    <tr>
                                        <td><%# Container.ItemIndex + 1 %></td>
                                        <td><strong><%# Eval("UserName") %></strong></td>
                                        <td><%# Eval("FullName") %></td>
                                        <td><span class="role-badge"><%# Eval("RoleName") %></span></td>
                                        <td><%# Eval("Email") %></td>
                                        <td><%# Eval("Mobile") %></td>
                                        <td>
                                            <%# Convert.ToBoolean(Eval("IsActive"))
                                                ? "<span class='badge-active'>Active</span>"
                                                : "<span class='badge-inactive'>Inactive</span>" %>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="btnEdit" runat="server"
                                                CommandName="EditUser"
                                                CommandArgument='<%# Eval("UserID") %>'
                                                CssClass="btn-edit">
                                                <i class="fas fa-edit"></i> Edit
                                            </asp:LinkButton>

                                            <%# Convert.ToBoolean(Eval("IsActive")) ? "" : "" %>

                                            <asp:LinkButton ID="btnToggle" runat="server"
                                                CommandName='<%# Convert.ToBoolean(Eval("IsActive")) ? "Deactivate" : "Activate" %>'
                                                CommandArgument='<%# Eval("UserID") %>'
                                                CssClass='<%# Convert.ToBoolean(Eval("IsActive")) ? "btn-deactivate" : "btn-activate" %>'
                                                OnClientClick="return confirm('Are you sure?');">
                                                <%# Convert.ToBoolean(Eval("IsActive"))
                                                    ? "<i class='fas fa-ban'></i> Deactivate"
                                                    : "<i class='fas fa-check'></i> Activate" %>
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div>
            </asp:Panel>

            <asp:Panel ID="pnlNoUsers" runat="server" Visible="false">
                <div class="no-data">
                    <i class="fas fa-user-slash" style="font-size:30px; margin-bottom:10px; display:block;"></i>
                    Abhi koi user nahi hai. Upar form se add karein.
                </div>
            </asp:Panel>

        </div>

    </div>

</form>

</body>
</html>
