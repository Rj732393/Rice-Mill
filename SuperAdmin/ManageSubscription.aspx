<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManageSubscription.aspx.cs" Inherits="superadmin_ManageSubscription" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Manage Subscription</title>
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

        .container-main { padding: 25px; max-width: 1000px; margin: 0 auto; }

        .card-box {
            background: white; border-radius: 16px;
            padding: 22px; box-shadow: 0 4px 15px rgba(0,0,0,0.07);
            margin-bottom: 22px;
        }
        .card-box h5 { font-size: 16px; font-weight: 600; color: #1e293b; margin-bottom: 16px; }

        .info-row { display: flex; gap: 20px; flex-wrap: wrap; margin-bottom: 16px; }
        .info-item { flex: 1; min-width: 180px; }
        .info-item .lbl { font-size: 12px; color: #6b7280; text-transform: uppercase; }
        .info-item .val { font-size: 16px; font-weight: 600; color: #1e293b; }

        .form-group label { font-weight: 500; color: #374151; font-size: 14px; }
        .form-control { border-radius: 8px; border: 1px solid #d1d5db; font-size: 14px; height: 40px; }

        .btn-action {
            border: none; border-radius: 8px; padding: 10px 22px;
            font-size: 14px; font-weight: 600; cursor: pointer; color: white;
            margin-right: 8px; margin-bottom: 8px;
            text-decoration: none; display: inline-block;
        }
        .btn-save     { background: #16a34a; }
        .btn-suspend  { background: #f59e0b; }
        .btn-activate { background: #22c55e; }
        .btn-action:hover { opacity: 0.9; color: white; text-decoration: none; }

        table { width: 100%; border-collapse: collapse; font-size: 13px; }
        table thead tr { background: #1e293b; color: white; }
        table thead th { padding: 9px 10px; font-weight: 500; }
        table tbody tr:nth-child(even) { background: #f8fafc; }
        table tbody td { padding: 8px 10px; border-bottom: 1px solid #e2e8f0; }

        .badge-active    { background: #dcfce7; color: #16a34a; padding: 4px 10px; border-radius: 20px; font-size: 12px; font-weight: 600; }
        .badge-inactive  { background: #fee2e2; color: #dc2626; padding: 4px 10px; border-radius: 20px; font-size: 12px; font-weight: 600; }
        .badge-suspended { background: #ffedd5; color: #c2410c; padding: 4px 10px; border-radius: 20px; font-size: 12px; font-weight: 600; }

        .msg-success { background: #dcfce7; color: #16a34a; padding: 10px 16px; border-radius: 8px; margin-bottom: 16px; font-weight: 600; }
        .err-msg { color: #dc2626; font-weight: 600; font-size: 14px; margin-bottom: 14px; display: block; }
    </style>
</head>
<body>

<div class="topbar">
    <h4><i class="fas fa-calendar-check"></i> &nbsp;Manage Subscription</h4>
    <a href="Dashboard.aspx"><i class="fas fa-arrow-left"></i> Wapas Dashboard</a>
</div>

<form id="form1" runat="server">
<div class="container-main">

    <asp:Panel ID="pnlMsg" runat="server" Visible="false">
        <div class="msg-success"><asp:Label ID="lblMsg" runat="server"></asp:Label></div>
    </asp:Panel>
    <asp:Label ID="lblErr" runat="server" CssClass="err-msg"></asp:Label>
    <asp:HiddenField ID="hfCompanyID" runat="server" />

    <%-- Current Status --%>
    <div class="card-box">
        <h5><i class="fas fa-info-circle"></i> &nbsp;Current Status — <asp:Label ID="lblCompanyName" runat="server" /></h5>
        <div class="info-row">
            <div class="info-item">
                <div class="lbl">Status</div>
                <div class="val"><asp:Label ID="lblCurrentStatus" runat="server" /></div>
            </div>
            <div class="info-item">
                <div class="lbl">Start Date</div>
                <div class="val"><asp:Label ID="lblStartDate" runat="server" /></div>
            </div>
            <div class="info-item">
                <div class="lbl">Expiry Date</div>
                <div class="val"><asp:Label ID="lblEndDate" runat="server" /></div>
            </div>
        </div>
    </div>

    <%-- Actions --%>
    <div class="card-box">
        <h5><i class="fas fa-cogs"></i> &nbsp;Subscription Update</h5>

        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <label>From Date</label>
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="SingleLine" type="date" />
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label>To Date</label>
                    <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="SingleLine" type="date" />
                </div>
            </div>
            <div class="col-md-4">
                <div class="form-group">
                    <label>Remarks</label>
                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="form-control" placeholder="Reason / note" />
                </div>
            </div>
        </div>

        <div style="margin-top:10px;">
            <asp:Button ID="btnSave" runat="server" Text="Save Subscription" CssClass="btn-action btn-save" OnClick="btnSave_Click" />
            <asp:Button ID="btnSuspend" runat="server" Text="Suspend Company" CssClass="btn-action btn-suspend"
                OnClick="btnSuspend_Click" OnClientClick="return confirm('Company ko suspend karein?');" />
            <asp:Button ID="btnActivate" runat="server" Text="Activate Company" CssClass="btn-action btn-activate" OnClick="btnActivate_Click" />
        </div>
    </div>

    <%-- History --%>
    <div class="card-box">
        <h5><i class="fas fa-history"></i> &nbsp;Subscription History</h5>
        <div style="overflow-x:auto;">
            <asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="False"
                EmptyDataText="Koi history nahi mili.">
                <Columns>
                    <asp:BoundField DataField="StartDate" HeaderText="From" DataFormatString="{0:dd-MMM-yyyy}" />
                    <asp:BoundField DataField="EndDate" HeaderText="To" DataFormatString="{0:dd-MMM-yyyy}" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <%# GetHistoryBadge(Eval("Status")) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                    <asp:BoundField DataField="CreatedBy" HeaderText="By" />
                    <asp:BoundField DataField="CreatedDate" HeaderText="Date" DataFormatString="{0:dd-MMM-yyyy HH:mm}" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

</div>
</form>
</body>
</html>
