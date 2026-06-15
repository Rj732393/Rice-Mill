<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="superadmin_Dashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Super Admin Panel</title>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"/>
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css"/>
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap" rel="stylesheet"/>
    <style>
        * { font-family: 'Poppins', sans-serif; box-sizing: border-box; margin: 0; padding: 0; }
        body { background: #f1f5f9; }

        .topbar {
            background: linear-gradient(90deg, #1a1a2e, #16213e);
            color: white; padding: 14px 25px;
            display: flex; justify-content: space-between; align-items: center;
        }
        .topbar h4 { margin: 0; font-size: 18px; color: #ffd700; }
        .topbar .right { display: flex; align-items: center; gap: 15px; }
        .topbar .admin-name { color: #94a3b8; font-size: 14px; }
        .topbar a { color: #94a3b8; font-size: 13px; text-decoration: none; }
        .topbar a:hover { color: #fff; }

        .container-main { padding: 25px; }

        .stat-card {
            border-radius: 16px; padding: 22px 20px;
            color: white; margin-bottom: 20px;
            box-shadow: 0 8px 20px rgba(0,0,0,0.12);
        }
        .stat-card .num  { font-size: 36px; font-weight: 700; line-height: 1; }
        .stat-card .lbl  { font-size: 13px; opacity: 0.9; margin-top: 6px; }
        .stat-card .icon { font-size: 32px; opacity: 0.6; float: right; margin-top: -40px; }
        .bg1 { background: linear-gradient(135deg, #667eea, #764ba2); }
        .bg2 { background: linear-gradient(135deg, #11998e, #38ef7d); }
        .bg3 { background: linear-gradient(135deg, #fc4a1a, #f7b733); }
        .bg4 { background: linear-gradient(135deg, #ef4444, #b91c1c); }
        .bg5 { background: linear-gradient(135deg, #0ea5e9, #6366f1); }
        .bg6 { background: linear-gradient(135deg, #f59e0b, #d97706); }

        .card-box {
            background: white; border-radius: 16px;
            padding: 22px; box-shadow: 0 4px 15px rgba(0,0,0,0.07);
            margin-bottom: 25px;
        }
        .card-box h5 { font-size: 17px; font-weight: 600; color: #1e293b; margin-bottom: 18px; }

        .btn-add {
            background: #16a34a; color: white; border: none;
            padding: 10px 22px; border-radius: 8px;
            font-size: 14px; font-weight: 600;
            text-decoration: none; display: inline-block;
            margin-bottom: 18px; margin-right: 8px;
        }
        .btn-add:hover { background: #15803d; color: white; text-decoration: none; }
        .btn-secondary {
            background: #6366f1; color: white; border: none;
            padding: 10px 22px; border-radius: 8px;
            font-size: 14px; font-weight: 600;
            text-decoration: none; display: inline-block;
            margin-bottom: 18px;
        }
        .btn-secondary:hover { background: #4f46e5; color: white; text-decoration: none; }

        table { width: 100%; border-collapse: collapse; font-size: 13px; }
        table thead tr { background: #1e293b; color: white; }
        table thead th { padding: 11px 10px; font-weight: 500; }
        table tbody tr:nth-child(even) { background: #f8fafc; }
        table tbody td { padding: 9px 10px; border-bottom: 1px solid #e2e8f0; vertical-align: middle; }

        .badge-active    { background: #dcfce7; color: #16a34a; padding: 4px 10px; border-radius: 20px; font-size: 12px; font-weight: 600; }
        .badge-inactive  { background: #fee2e2; color: #dc2626; padding: 4px 10px; border-radius: 20px; font-size: 12px; font-weight: 600; }
        .badge-expiring  { background: #fef9c3; color: #ca8a04; padding: 4px 10px; border-radius: 20px; font-size: 12px; font-weight: 600; }
        .badge-suspended { background: #ffedd5; color: #c2410c; padding: 4px 10px; border-radius: 20px; font-size: 12px; font-weight: 600; }
        .badge-plan      { background: #e0e7ff; color: #4338ca; padding: 4px 10px; border-radius: 20px; font-size: 12px; font-weight: 600; }

        .btn-edit    { background: #3b82f6; color: white; padding: 5px 10px; border-radius: 6px; font-size: 12px; border: none; text-decoration: none; display: inline-block; text-align: center; }
        .btn-sub     { background: #8b5cf6; color: white; padding: 5px 10px; border-radius: 6px; font-size: 12px; border: none; text-decoration: none; display: inline-block; text-align: center; }
        .btn-band    { background: #f59e0b; color: white; padding: 5px 10px; border-radius: 6px; font-size: 12px; border: none; cursor: pointer; text-decoration: none; display: inline-block; text-align: center; }
        .btn-chalu   { background: #22c55e; color: white; padding: 5px 10px; border-radius: 6px; font-size: 12px; border: none; cursor: pointer; text-decoration: none; display: inline-block; text-align: center; }
        .btn-block   { background: #dc2626; color: white; padding: 5px 10px; border-radius: 6px; font-size: 12px; border: none; cursor: pointer; text-decoration: none; display: inline-block; text-align: center; }
        .btn-edit:hover, .btn-sub:hover, .btn-band:hover, .btn-chalu:hover, .btn-block:hover  { color: white; text-decoration: none; opacity: 0.9; }
        .msg-success { background: #dcfce7; color: #16a34a; padding: 10px 16px; border-radius: 8px; margin-bottom: 16px; font-weight: 600; }
        .action-cell { display: flex; flex-wrap: wrap; gap: 4px; }
        .action-cell a, .action-cell button { margin-bottom: 4px; }
        .action-cell br { display: none; }
    </style>
</head>
<body>

<div class="topbar">
    <h4><i class="fas fa-crown"></i> &nbsp;Super Admin Panel — Rice Management Software</h4>
    <div class="right">
        <span class="admin-name">
            <asp:Label ID="lblAdmin" runat="server"></asp:Label>
        </span>
        <a href="AuditLogs.aspx"><i class="fas fa-history"></i> Audit Logs</a>
        <a href="../Login.aspx" style="color:#fca5a5;">
            <i class="fas fa-sign-out-alt"></i> Logout
        </a>
    </div>
</div>

<form id="form1" runat="server">
<div class="container-main">

    <%-- Success message --%>
    <asp:Panel ID="pnlMsg" runat="server" Visible="false">
        <div class="msg-success">
            <asp:Label ID="lblMsg" runat="server"></asp:Label>
        </div>
    </asp:Panel>

    <%-- Stats --%>
    <div class="row">
        <div class="col-md-3">
            <div class="stat-card bg1">
                <div class="num"><asp:Label ID="lblTotal" runat="server">0</asp:Label></div>
                <div class="lbl">Total Companies</div>
                <div class="icon"><i class="fas fa-building"></i></div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="stat-card bg2">
                <div class="num"><asp:Label ID="lblActive" runat="server">0</asp:Label></div>
                <div class="lbl">Active Companies</div>
                <div class="icon"><i class="fas fa-check-circle"></i></div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="stat-card bg3">
                <div class="num"><asp:Label ID="lblExpired" runat="server">0</asp:Label></div>
                <div class="lbl">Expired Companies</div>
                <div class="icon"><i class="fas fa-times-circle"></i></div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="stat-card bg4">
                <div class="num"><asp:Label ID="lblSuspended" runat="server">0</asp:Label></div>
                <div class="lbl">Suspended Companies</div>
                <div class="icon"><i class="fas fa-ban"></i></div>
            </div>
        </div>
    </div>

    <div class="row">
        <div class="col-md-3">
            <div class="stat-card bg5">
                <div class="num"><asp:Label ID="lblTotalUsers" runat="server">0</asp:Label></div>
                <div class="lbl">Total Users</div>
                <div class="icon"><i class="fas fa-users"></i></div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="stat-card bg6">
                <div class="num"><asp:Label ID="lblRevenue" runat="server">₹0</asp:Label></div>
                <div class="lbl">Subscription Revenue</div>
                <div class="icon"><i class="fas fa-rupee-sign"></i></div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="stat-card bg3">
                <div class="num"><asp:Label ID="lblExpiringSoon" runat="server">0</asp:Label></div>
                <div class="lbl">Expiring in 7 Days</div>
                <div class="icon"><i class="fas fa-clock"></i></div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="stat-card bg2">
                <div class="num"><asp:Label ID="lblRecent" runat="server">0</asp:Label></div>
                <div class="lbl">New This Month</div>
                <div class="icon"><i class="fas fa-star"></i></div>
            </div>
        </div>
    </div>

    <%-- Companies Table --%>
    <div class="card-box">
        <h5><i class="fas fa-list"></i> &nbsp;Sabhi Companies</h5>

        <a href="AddCompany.aspx" class="btn-add">
            <i class="fas fa-plus"></i> &nbsp;Nayi Company Banayein
        </a>

        <div style="overflow-x:auto;">
            <asp:GridView ID="gvCompanies" runat="server"
                AutoGenerateColumns="False"
                OnRowCommand="gvCompanies_RowCommand">
                <Columns>
                    <asp:BoundField DataField="CompanyID"       HeaderText="#"            />
                    <asp:BoundField DataField="CompanyName"     HeaderText="Company Naam" />
                    <asp:BoundField DataField="AdminUserName"   HeaderText="Login ID"     />
                    <asp:BoundField DataField="Phone"           HeaderText="Phone"        />
                    <asp:TemplateField HeaderText="Plan">
                        <ItemTemplate>
                            <span class="badge-plan"><%# GetPlanName(Eval("PlanName")) %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="SubscriptionEnd" HeaderText="Expiry"       DataFormatString="{0:dd-MMM-yyyy}" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <%# GetStatusBadge(Eval("Status"), Eval("SubscriptionEnd")) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <div class="action-cell">
                            <a href='<%# "AddCompany.aspx?id=" + Eval("CompanyID") %>' class="btn-edit">
                                <i class="fas fa-edit"></i> Edit
                            </a>
                            <a href='<%# "ManageSubscription.aspx?id=" + Eval("CompanyID") %>' class="btn-sub">
                                <i class="fas fa-calendar-check"></i> Subscription
                            </a>
                            <asp:LinkButton ID="btnToggleActive" runat="server"
                                CommandName="ToggleActive"
                                CommandArgument='<%# Eval("CompanyID") + "," + Eval("Status") %>'
                                CssClass='<%# Eval("Status").ToString() == "Suspended" ? "btn-chalu" : "btn-band" %>'
                                OnClientClick="return confirm('Kya aap pakka karna chahte hain?');">
                                <%# Eval("Status").ToString() == "Suspended" ? "Activate" : "Suspend" %>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnBlock" runat="server"
                                CommandName="ToggleBlock"
                                CommandArgument='<%# Eval("CompanyID") + "," + Eval("Status") %>'
                                CssClass='<%# Eval("Status").ToString() == "Blocked" ? "btn-chalu" : "btn-block" %>'
                                OnClientClick="return confirm('Kya aap pakka karna chahte hain?');">
                                <%# Eval("Status").ToString() == "Blocked" ? "Unblock" : "Block" %>
                            </asp:LinkButton>
                            <asp:LinkButton ID="btnResetPwd" runat="server"
                                CommandName="ResetPassword"
                                CommandArgument='<%# Eval("CompanyID") %>'
                                CssClass="btn-edit"
                                OnClientClick="return confirm('Admin password ko default (Admin@123) par reset karein?');">
                                Reset Pwd
                            </asp:LinkButton>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

</div>
</form>
</body>
</html>
