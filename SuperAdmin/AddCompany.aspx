<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddCompany.aspx.cs" Inherits="superadmin_AddCompany" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Company Subscription</title>
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
        .form-wrap {
            max-width: 820px; margin: 30px auto;
            background: white; border-radius: 16px;
            padding: 30px 35px;
            box-shadow: 0 4px 20px rgba(0,0,0,0.09);
        }
        .section-head {
            background: #f8fafc;
            border-left: 4px solid #3b82f6;
            padding: 9px 16px; border-radius: 4px;
            font-weight: 600; color: #1e293b;
            margin-bottom: 20px; margin-top: 10px;
            font-size: 15px;
        }
        .form-group label { font-weight: 500; color: #374151; font-size: 14px; }
        .form-control { border-radius: 8px; border: 1px solid #d1d5db; font-size: 14px; height: 40px; }
        textarea.form-control { height: auto; }
        .btn-save {
            background: #16a34a; color: white; border: none;
            padding: 11px 28px; border-radius: 8px;
            font-size: 15px; font-weight: 600; cursor: pointer;
        }
        .btn-save:hover { background: #15803d; }
        .btn-cancel {
            background: #6b7280; color: white; border: none;
            padding: 11px 28px; border-radius: 8px;
            font-size: 15px; font-weight: 600;
            text-decoration: none; display: inline-block;
            margin-left: 10px;
        }
        .btn-cancel:hover { background: #4b5563; color: white; text-decoration: none; }
        .err-msg { color: #dc2626; font-weight: 600; font-size: 14px; margin-bottom: 14px; display: block; }
        .note { color: #6b7280; font-size: 12px; margin-top: 4px; }
    </style>
</head>
<body>

<div class="topbar">
    <h4><i class="fas fa-plus-circle"></i> &nbsp;Nayi Company ka Subscription</h4>
    <a href="Dashboard.aspx" style="color:#94a3b8; text-decoration:none; font-size:14px;">
        <i class="fas fa-arrow-left"></i> Wapas Dashboard
    </a>
</div>

<form id="form1" runat="server">
<div class="form-wrap">

    <asp:Label ID="lblMsg" runat="server" CssClass="err-msg"></asp:Label>
    <asp:HiddenField ID="hfCompanyID" runat="server" Value="0" />

    <%-- Section 1: Company Details --%>
    <div class="section-head"><i class="fas fa-building"></i> &nbsp;Company ki Jaankari</div>

    <div class="row">
        <div class="col-md-12">
            <div class="form-group">
                <label>Company ka Naam *</label>
                <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" placeholder="jaise: Ramesh Rice Mill Pvt Ltd" />
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label>Owner</label>
                <asp:TextBox ID="txtOwnerName" runat="server" CssClass="form-control" placeholder="jaise: Ramesh Kumar" />
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label>Phone Number</label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="9876543210" />
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label>Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="company@email.com" />
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label>City</label>
                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" placeholder="jaise: Patna" />
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label>State</label>
                <asp:TextBox ID="txtState" runat="server" CssClass="form-control" placeholder="jaise: Bihar" />
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label>GST Number</label>
                <asp:TextBox ID="txtGST" runat="server" CssClass="form-control" placeholder="22AAAAA0000A1Z5" />
            </div>
        </div>

        <%-- NAYE 3 FIELDS --%>
        <div class="col-md-6">
            <div class="form-group">
                <label>CIN Number</label>
                <asp:TextBox ID="txtCIN" runat="server" CssClass="form-control" placeholder="U15312BR2014PTC022237" />
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label>PAN Number</label>
                <asp:TextBox ID="txtPAN" runat="server" CssClass="form-control" placeholder="AAGCR9497P" />
            </div>
        </div>
        <div class="col-md-12">
            <div class="form-group">
                <label>Logo URL</label>
                <asp:TextBox ID="txtLogoUrl" runat="server" CssClass="form-control" placeholder="https://yoursite.com/logo.png" />
                <span class="note">Company ke logo ki URL daalein jo bill mein show hogi</span>
            </div>
        </div>
        <%-- NAYE FIELDS KHATAM --%>

        <div class="col-md-12">
            <div class="form-group">
                <label>Poora Address</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="Gali, Mohalla, Shahar..." />
            </div>
        </div>
    </div>

    <%-- Section 2: Login Details --%>
    <div class="section-head"><i class="fas fa-key"></i> &nbsp;Company ka Login ID aur Password</div>

    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label>Login Username *</label>
                <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" placeholder="jaise: ramesh_admin" />
                <span class="note">Ye username company ka Admin login mein use karega</span>
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label>Password *</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" placeholder="Strong password daalein" />
            </div>
        </div>
    </div>

    <%-- Section 3: Subscription Dates --%>
    <div class="section-head"><i class="fas fa-calendar-alt"></i> &nbsp;Subscription Dates</div>

    <div class="row">
        <div class="col-md-6">
            <div class="form-group">
                <label>Subscription Shuru *</label>
                <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control" TextMode="SingleLine" type="date" />
            </div>
        </div>
        <div class="col-md-6">
            <div class="form-group">
                <label>Subscription Khatam *</label>
                <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control" TextMode="SingleLine" type="date" />
            </div>
        </div>
    </div>

    <%-- Buttons --%>
    <div style="margin-top:20px;">
        <asp:Button ID="btnSave" runat="server" Text="Save Karo" CssClass="btn-save" OnClick="btnSave_Click" />
        <a href="Dashboard.aspx" class="btn-cancel">Cancel</a>
    </div>

</div>
</form>
</body>
</html>