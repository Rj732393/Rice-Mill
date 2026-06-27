<%@ Control Language="C#" AutoEventWireup="true"
    CodeFile="AdminMenu.ascx.cs"
    Inherits="Includes_AdminMenu" %>

<!-- SIDEBAR -->
<div class="rm-sidebar" id="rmSidebar">

    <div class="rm-logo">
        <h2>
            <asp:Label ID="lblSidebarCompany" runat="server" Text="Rice Mill"></asp:Label>
        </h2>
        <span>Management System</span>
    </div>

    <ul class="rm-menu">

        <li>
            <a href="Dashboard.aspx">
                <i class="fas fa-home"></i>
                Dashboard
            </a>
        </li>

        <li>
            <a href="RiceStock.aspx" class="active">
                <i class="fas fa-seedling"></i>
                Rice Stock
            </a>
        </li>

        <li>
            <a href="PaddyStock.aspx">
                <i class="fas fa-boxes"></i>
                Paddy Stock
            </a>
        </li>

        <li>
            <a href="SalePurchaseExpense.aspx">
                <i class="fas fa-shopping-cart"></i>
                Sale Purchase
            </a>
        </li>

        <li>
            <a href="EditBySauda.aspx">
                <i class="fas fa-boxes"></i>
                Edit Sauda
            </a>
        </li>

        <li>
            <a href="logout.aspx">
                <i class="fas fa-sign-out-alt"></i>
                Logout
            </a>
        </li>

    </ul>

</div>

<!-- NAVBAR -->
<div class="rm-navbar" id="rmNavbar">

    <div class="rm-nav-left">

        <div class="rm-toggle" onclick="toggleSidebar()">
            <span></span>
            <span></span>
            <span></span>
        </div>

        <div class="rm-title">
            <asp:Label ID="lblNavbarCompany" runat="server" 
                Text="Rice Stock Management"></asp:Label>
        </div>

    </div>

    <!-- RIGHT: Admin Dropdown -->
    <div class="rm-admin" onclick="toggleAdminMenu()" 
         style="position:relative; cursor:pointer;">

        <i class="fas fa-user-shield"></i>
        Welcome,
        <asp:Label ID="lblAdminName" runat="server" 
            style="color:white; font-weight:600;"></asp:Label>

        <i class="fas fa-chevron-down" 
           style="font-size:11px; margin-left:5px;"></i>

        <!-- Dropdown -->
        <div id="adminDropdown" style="
            display:none;
            position:absolute;
            top:110%;
            right:0;
            background:#fff;
            box-shadow:0 8px 25px rgba(0,0,0,0.12);
            border-radius:12px;
            min-width:190px;
            z-index:9999;
            overflow:hidden;">

            <!-- User Info -->
            <div style="padding:12px 16px; 
                        border-bottom:1px solid #f1f5f9;
                        font-size:13px; color:#64748b;">
                <i class="fas fa-user" style="margin-right:6px;"></i>
                <asp:Label ID="lblDropdownName" runat="server"></asp:Label>
            </div>

            <!-- Add Operator -->
            <a href="javascript:void(0)" 
               onclick="openAddOperatorModal()" 
               style="display:block; padding:11px 16px; 
                      color:#1e293b; text-decoration:none;
                      font-size:14px;">
                <i class="fas fa-user-plus" 
                   style="margin-right:8px; color:#6366f1;"></i>
                Add Operator
            </a>

            <!-- View Operators -->
            <a href="javascript:void(0)" 
               onclick="openViewOperatorsModal()" 
               style="display:block; padding:11px 16px; 
                      color:#1e293b; text-decoration:none;
                      font-size:14px;">
                <i class="fas fa-users" 
                   style="margin-right:8px; color:#6366f1;"></i>
                View Operators
            </a>

            <!-- Logout -->
            <a href="logout.aspx" 
               style="display:block; padding:11px 16px; 
                      color:#ef4444; text-decoration:none;
                      font-size:14px; font-weight:600;
                      border-top:1px solid #f1f5f9;">
                <i class="fas fa-sign-out-alt" 
                   style="margin-right:8px;"></i>
                Logout
            </a>

        </div>

    </div>

</div>

<!-- ===== ADD OPERATOR MODAL ===== -->
<div id="addOperatorModal" style="display:none; position:fixed; inset:0; 
     background:rgba(15,23,42,0.55); z-index:9999; 
     justify-content:center; align-items:center;">

    <div style="background:#fff; border-radius:16px; padding:32px 28px 28px; 
         width:100%; max-width:460px; 
         box-shadow:0 20px 60px rgba(0,0,0,0.18); 
         position:relative; animation:popIn .22s ease;">

        <button type="button" onclick="closeAddOperatorModal()" 
            style="position:absolute; top:14px; right:18px; 
                   background:none; border:none; font-size:22px; 
                   color:#94a3b8; cursor:pointer;">&times;</button>

        <h3 style="margin:0 0 18px; color:#1e293b;">
            <i class="fas fa-user-plus" 
               style="color:#6366f1; margin-right:8px;"></i>
            Add Operator
        </h3>

        <asp:Label ID="lblMsg" runat="server" Font-Bold="true"
            style="display:none; margin-bottom:10px; 
                   padding:8px 12px; border-radius:8px;" />

        <asp:TextBox ID="txtUser" runat="server"
            CssClass="form-control"
            placeholder="Username"
            style="margin-bottom:12px;" />
        <br />

        <asp:TextBox ID="txtPass" runat="server"
            CssClass="form-control"
            placeholder="Password"
            TextMode="Password"
            style="margin-bottom:12px;" />
        <br />

        <asp:Button ID="btnAdd" runat="server"
            Text="Save"
            CssClass="btn btn-success"
            OnClick="btnAdd_Click" />

        <a href="javascript:void(0)" 
           onclick="closeAddOperatorModal(); openViewOperatorsModal();" 
           class="btn btn-secondary" 
           style="margin-left:8px;">
            View Operators
        </a>

    </div>
</div>

<!-- ===== VIEW OPERATORS MODAL ===== -->
<div id="viewOperatorsModal" style="display:none; position:fixed; inset:0; 
     background:rgba(15,23,42,0.55); z-index:9999; 
     justify-content:center; align-items:center;">

    <div style="background:#fff; border-radius:16px; padding:32px 28px 28px; 
         width:fit-content; max-width:95vw; max-height:85vh; overflow-y:auto; 
         box-shadow:0 20px 60px rgba(0,0,0,0.18); 
         position:relative; animation:popIn .22s ease;">

        <button type="button" onclick="closeViewOperatorsModal()" 
            style="position:absolute; top:14px; right:18px; 
                   background:none; border:none; font-size:22px; 
                   color:#94a3b8; cursor:pointer;">&times;</button>

        <h3 style="margin:0 0 18px; color:#1e293b;">
            <i class="fas fa-users" 
               style="color:#6366f1; margin-right:8px;"></i>
            Operators List
        </h3>

        <asp:GridView ID="gvOperators" runat="server" 
            CssClass="table table-bordered table-striped"
            AutoGenerateColumns="false" DataKeyNames="ID"
            OnRowCommand="gvOperators_RowCommand" 
            style="width:auto;">
            <Columns>
                <asp:BoundField DataField="UserName" HeaderText="Username" />
                <asp:BoundField DataField="UPassword" HeaderText="Password" />
                <asp:BoundField DataField="CreatedDate" HeaderText="Created On" 
                    DataFormatString="{0:dd-MMM-yyyy}" />
                <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <span style='padding:3px 10px; border-radius:12px; 
                            font-size:12px; font-weight:600; 
                            <%# Convert.ToBoolean(Eval("IsActive")) 
                                ? "background:#dcfce7;color:#16a34a;" 
                                : "background:#fee2e2;color:#dc2626;" %>'>
                            <%# Convert.ToBoolean(Eval("IsActive")) 
                                ? "Active" : "Suspended" %>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Actions" 
                    ItemStyle-HorizontalAlign="Center" 
                    HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <div style="display:flex; flex-wrap:nowrap; 
                             align-items:center; justify-content:center; gap:6px;">

                            <asp:LinkButton ID="btnEdit" runat="server" 
                                CommandName="EditOp" 
                                CommandArgument='<%# Eval("ID") %>'
                                CssClass="btn btn-sm" 
                                style="background:#6366f1;color:#fff;border:none;
                                       padding:5px 10px;border-radius:6px;
                                       white-space:nowrap;">
                                <i class="fas fa-edit"></i> Edit
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnSuspend" runat="server" 
                                CommandName="SuspendOp" 
                                CommandArgument='<%# Eval("ID") %>'
                                CssClass="btn btn-sm" 
                                style="background:#f59e0b;color:#fff;border:none;
                                       padding:5px 10px;border-radius:6px;
                                       white-space:nowrap;"
                                OnClientClick='<%# "return confirm(\"" + 
                                    (Convert.ToBoolean(Eval("IsActive")) 
                                    ? "Suspend" : "Activate") + 
                                    " this operator?\");" %>'>
                                <i class='<%# Convert.ToBoolean(Eval("IsActive")) 
                                    ? "fas fa-ban" : "fas fa-check" %>'></i>
                                <%# Convert.ToBoolean(Eval("IsActive")) 
                                    ? "Suspend" : "Activate" %>
                            </asp:LinkButton>

                            <asp:LinkButton ID="btnDelete" runat="server" 
                                CommandName="DeleteOp" 
                                CommandArgument='<%# Eval("ID") %>'
                                CssClass="btn btn-sm" 
                                style="background:#ef4444;color:#fff;border:none;
                                       padding:5px 10px;border-radius:6px;
                                       white-space:nowrap;"
                                OnClientClick="return confirm('Delete this operator permanently?');">
                                <i class="fas fa-trash"></i> Delete
                            </asp:LinkButton>

                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

    </div>
</div>

<!-- ===== EDIT OPERATOR MODAL ===== -->
<div id="editOperatorModal" style="display:none; position:fixed; inset:0; 
     background:rgba(15,23,42,0.55); z-index:9999; 
     justify-content:center; align-items:center;">

    <div style="background:#fff; border-radius:16px; padding:32px 28px 28px; 
         width:100%; max-width:460px; 
         box-shadow:0 20px 60px rgba(0,0,0,0.18); 
         position:relative; animation:popIn .22s ease;">

        <button type="button" onclick="closeEditOperatorModal()" 
            style="position:absolute; top:14px; right:18px; 
                   background:none; border:none; font-size:22px; 
                   color:#94a3b8; cursor:pointer;">&times;</button>

        <h3 style="margin:0 0 18px; color:#1e293b;">
            <i class="fas fa-user-edit" 
               style="color:#6366f1; margin-right:8px;"></i>
            Edit Operator
        </h3>

        <asp:Label ID="lblEditMsg" runat="server" Font-Bold="true"
            style="display:none; margin-bottom:10px; 
                   padding:8px 12px; border-radius:8px;" />

        <asp:HiddenField ID="hdnEditID" runat="server" />

        <asp:TextBox ID="txtEditUser" runat="server"
            CssClass="form-control"
            placeholder="Username"
            style="margin-bottom:12px;" />
        <br />

        <asp:TextBox ID="txtEditPass" runat="server"
            CssClass="form-control"
            placeholder="New Password (leave blank to keep same)"
            TextMode="Password"
            style="margin-bottom:12px;" />
        <br />

        <asp:Button ID="btnEditSave" runat="server"
            Text="Update"
            CssClass="btn btn-success"
            OnClick="btnEditSave_Click" />

    </div>
</div>

<style>
    @keyframes popIn {
        from { transform: scale(0.92); opacity: 0; }
        to   { transform: scale(1); opacity: 1; }
    }
</style>

<script type="text/javascript">

    function toggleSidebar() {
        var sidebar = document.getElementById("rmSidebar");
        var navbar = document.getElementById("rmNavbar");
        var main = document.querySelector(".main-wrapper");
        sidebar.classList.toggle("hideSidebar");
        navbar.classList.toggle("fullNavbar");
        main.classList.toggle("fullMain");
    }

    function toggleAdminMenu() {
        var dropdown = document.getElementById("adminDropdown");
        dropdown.style.display =
            (dropdown.style.display === "block") ? "none" : "block";
    }

    document.addEventListener("click", function (e) {
        var dropdown = document.getElementById("adminDropdown");
        var adminBox = document.querySelector(".rm-admin");
        if (dropdown && adminBox && !adminBox.contains(e.target)) {
            dropdown.style.display = "none";
        }
    });

    function openAddOperatorModal() {
        document.getElementById("adminDropdown").style.display = "none";
        var userBox = document.querySelector("[id$='txtUser']");
        var passBox = document.querySelector("[id$='txtPass']");
        var msg = document.querySelector("[id$='lblMsg']");
        if (userBox) userBox.value = "";
        if (passBox) passBox.value = "";
        if (msg) { msg.innerText = ""; msg.style.display = "none"; }
        document.getElementById("addOperatorModal").style.display = "flex";
    }

    function closeAddOperatorModal() {
        document.getElementById("addOperatorModal").style.display = "none";
    }

    document.getElementById("addOperatorModal").addEventListener("click", function (e) {
        if (e.target === this) closeAddOperatorModal();
    });

    function openViewOperatorsModal() {
        document.getElementById("adminDropdown").style.display = "none";
        document.getElementById("viewOperatorsModal").style.display = "flex";
    }

    function closeViewOperatorsModal() {
        document.getElementById("viewOperatorsModal").style.display = "none";
    }

    document.getElementById("viewOperatorsModal").addEventListener("click", function (e) {
        if (e.target === this) closeViewOperatorsModal();
    });

    function closeEditOperatorModal() {
        document.getElementById("editOperatorModal").style.display = "none";
    }

    var editModalEl = document.getElementById("editOperatorModal");
    if (editModalEl) {
        editModalEl.addEventListener("click", function (e) {
            if (e.target === this) closeEditOperatorModal();
        });
    }

    window.onload = function () {
        var msg = document.querySelector("[id$='lblMsg']");
        if (msg && msg.innerText.trim() !== "") {
            msg.style.display = "block";
            document.getElementById("addOperatorModal").style.display = "flex";
        }
        var editMsg = document.querySelector("[id$='lblEditMsg']");
        if (editMsg && editMsg.innerText.trim() !== "") {
            editMsg.style.display = "block";
            document.getElementById("viewOperatorsModal").style.display = "flex";
            document.getElementById("editOperatorModal").style.display = "flex";
        }
    };

</script>