<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AuditLogs.aspx.cs" Inherits="superadmin_AuditLogs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Audit Logs</title>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css"/>
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css"/>
    <link href="https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap" rel="stylesheet"/>
    <style>
        * { font-family: 'Poppins', sans-serif; box-sizing: border-box; }
        body { background: #f1f5f9; margin: 0; }

        .topbar {
            background: linear-gradient(90deg, #1a1a2e, #16213e);
            color: white; padding: 14px 25px;
            display: flex; justify-content: space-between; align-items: center;
        }
        .topbar h4 { margin: 0; font-size: 17px; color: #ffd700; }
        .topbar a { color: #94a3b8; text-decoration: none; font-size: 14px; }
        .topbar a:hover { color: #fff; }

        .container-main { padding: 25px; }

        .card-box {
            background: white; border-radius: 16px;
            padding: 22px; box-shadow: 0 4px 15px rgba(0,0,0,0.07);
        }
        .card-box h5 { font-size: 16px; font-weight: 600; color: #1e293b; margin-bottom: 16px; }

        .filter-row { display: flex; gap: 12px; margin-bottom: 16px; flex-wrap: wrap; align-items: flex-end; }
        .form-control { border-radius: 8px; border: 1px solid #d1d5db; font-size: 14px; height: 38px; }
        .form-group label { font-weight: 500; color: #374151; font-size: 13px; display:block; margin-bottom: 4px; }
        .btn-filter { background: #3b82f6; color: white; border: none; padding: 9px 20px; border-radius: 8px; font-weight: 600; font-size: 14px; }

        table { width: 100%; border-collapse: collapse; font-size: 13px; }
        table thead tr { background: #1e293b; color: white; }
        table thead th { padding: 9px 10px; font-weight: 500; }
        table tbody tr:nth-child(even) { background: #f8fafc; }
        table tbody td { padding: 8px 10px; border-bottom: 1px solid #e2e8f0; }

        .badge-action { background: #e0e7ff; color: #4338ca; padding: 3px 9px; border-radius: 14px; font-size: 12px; font-weight: 600; }
    </style>
</head>
<body>

<div class="topbar">
    <h4><i class="fas fa-history"></i> &nbsp;Audit Logs</h4>
    <a href="Dashboard.aspx"><i class="fas fa-arrow-left"></i> Wapas Dashboard</a>
</div>

<form id="form1" runat="server">
<div class="container-main">

    <div class="card-box">
        <h5><i class="fas fa-filter"></i> &nbsp;Filter Logs</h5>

        <div class="filter-row">
            <div class="form-group" style="min-width:200px;">
                <label>Company</label>
                <asp:DropDownList ID="ddlCompany" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group" style="min-width:160px;">
                <label>Action</label>
                <asp:DropDownList ID="ddlAction" runat="server" CssClass="form-control">
                    <asp:ListItem Text="-- All --" Value="" />
                    <asp:ListItem Text="Login" Value="Login" />
                    <asp:ListItem Text="Logout" Value="Logout" />
                    <asp:ListItem Text="CompanyCreated" Value="CompanyCreated" />
                    <asp:ListItem Text="CompanyUpdated" Value="CompanyUpdated" />
                    <asp:ListItem Text="CompanySuspended" Value="CompanySuspended" />
                    <asp:ListItem Text="CompanyActivated" Value="CompanyActivated" />
                    <asp:ListItem Text="CompanyBlocked" Value="CompanyBlocked" />
                    <asp:ListItem Text="SubscriptionRenewed" Value="SubscriptionRenewed" />
                    <asp:ListItem Text="SubscriptionExtended" Value="SubscriptionExtended" />
                    <asp:ListItem Text="SubscriptionSuspended" Value="SubscriptionSuspended" />
                    <asp:ListItem Text="UserCreated" Value="UserCreated" />
                    <asp:ListItem Text="SettingsChanged" Value="SettingsChanged" />
                    <asp:ListItem Text="RecordDeleted" Value="RecordDeleted" />
                    <asp:ListItem Text="PasswordReset" Value="PasswordReset" />
                </asp:DropDownList>
            </div>
            <div class="form-group" style="min-width:150px;">
                <label>From Date</label>
                <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control" TextMode="SingleLine" type="date" />
            </div>
            <div class="form-group" style="min-width:150px;">
                <label>To Date</label>
                <asp:TextBox ID="txtTo" runat="server" CssClass="form-control" TextMode="SingleLine" type="date" />
            </div>
            <div class="form-group">
                <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn-filter" OnClick="btnFilter_Click" />
            </div>
        </div>
    </div>

    <br />

    <div class="card-box">
        <h5><i class="fas fa-list"></i> &nbsp;Activity Log</h5>
        <div style="overflow-x:auto;">
            <asp:GridView ID="gvLogs" runat="server" AutoGenerateColumns="False"
                EmptyDataText="Koi log nahi mila." AllowPaging="True" PageSize="50"
                OnPageIndexChanging="gvLogs_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="CreatedDate" HeaderText="Date/Time" DataFormatString="{0:dd-MMM-yyyy HH:mm:ss}" />
                    <asp:BoundField DataField="CompanyName" HeaderText="Company" />
                    <asp:BoundField DataField="UserName" HeaderText="User" />
                    <asp:BoundField DataField="UserType" HeaderText="Role" />
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <span class="badge-action"><%# Eval("Action") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Module" HeaderText="Module" />
                    <asp:BoundField DataField="Description" HeaderText="Details" />
                    <asp:BoundField DataField="IPAddress" HeaderText="IP" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

</div>
</form>
</body>
</html>
