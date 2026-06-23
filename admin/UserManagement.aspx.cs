using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using substitute;

public partial class admin_UserManagement : System.Web.UI.Page
{
    DataTable dt;
    List<SqlParameter> param;
    DataAccessLayer dac = new DataAccessLayer();
    int companyID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
       
        // Auth: sirf Admin ya SuperAdmin
        if (Session["User"] == null || Session["UserType"] == null)
        {
            Response.Redirect("../Login.aspx");
            return;
        }
        string userType = Session["UserType"].ToString();
        if (userType != "Admin" && userType != "SuperAdmin")
        {
            Response.Redirect("../Login.aspx");
            return;
        }

        companyID = Session["CompanyID"] != null ? Convert.ToInt32(Session["CompanyID"]) : 0;

        if (!IsPostBack)
        {
            LoadUsers();
        }
    }

    // -------------------------------------------------------
    // Load users list
    // -------------------------------------------------------
    private void LoadUsers()
    {
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CompanyID", companyID));

        dt = dac.GetDataTable(
            @"SELECT 
                u.UserID,
                u.UserName,
                ISNULL(u.FullName,'')  AS FullName,
                ISNULL(u.Email,'')     AS Email,
                ISNULL(u.Mobile,'')    AS Mobile,
                u.RoleID,
                ISNULL(r.RoleName, ISNULL(u.UserType,'Operator')) AS RoleName,
                u.IsActive
              FROM prabha.UserInfo u
              LEFT JOIN prabha.Roles r ON r.RoleID = u.RoleID
              WHERE u.CompanyID = @CompanyID
              ORDER BY u.IsActive DESC, u.UserName ASC",
            param);

        if (dt.Rows.Count > 0)
        {
            rptUsers.DataSource = dt;
            rptUsers.DataBind();
            pnlTable.Visible   = true;
            pnlNoUsers.Visible = false;
        }
        else
        {
            pnlTable.Visible   = false;
            pnlNoUsers.Visible = true;
        }
    }

    // -------------------------------------------------------
    // Save (Add or Update)
    // -------------------------------------------------------
    protected void btnSave_Click(object sender, EventArgs e)
    {
        companyID = Session["CompanyID"] != null ? Convert.ToInt32(Session["CompanyID"]) : 0;
        int userID = Convert.ToInt32(hdnUserID.Value);

        string fullName  = txtFullName.Text.Trim();
        string userName  = txtUserName.Text.Trim();
        string password  = txtPassword.Text.Trim();
        string email     = txtEmail.Text.Trim();
        string mobile    = txtMobile.Text.Trim();
        int    roleID    = ddlRole.SelectedValue != "" ? Convert.ToInt32(ddlRole.SelectedValue) : 4;
        string roleName  = ddlRole.SelectedItem.Text;

        // Basic validation
        if (string.IsNullOrEmpty(userName))
        {
            ShowMsg("Username zaroori hai!", false);
            return;
        }
        if (userID == 0 && string.IsNullOrEmpty(password))
        {
            ShowMsg("Naye user ke liye password zaroori hai!", false);
            return;
        }

        // Username duplicate check
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@UserName",  userName));
        param.Add(new SqlParameter("@CompanyID", companyID));
        param.Add(new SqlParameter("@UserID",    userID));

        object exists = dac.Scalar(
            "SELECT COUNT(*) FROM prabha.UserInfo WHERE UserName=@UserName AND CompanyID=@CompanyID AND UserID<>@UserID",
            param);

        if (Convert.ToInt32(exists) > 0)
        {
            ShowMsg("Yeh username pehle se exist karta hai!", false);
            return;
        }

        if (userID == 0)
        {
            // INSERT
            param = new List<SqlParameter>();
            param.Add(new SqlParameter("@CompanyID", companyID));
            param.Add(new SqlParameter("@FullName",  fullName));
            param.Add(new SqlParameter("@UserName",  userName));
            param.Add(new SqlParameter("@UPassword", password));
            param.Add(new SqlParameter("@Email",     email));
            param.Add(new SqlParameter("@Mobile",    mobile));
            param.Add(new SqlParameter("@RoleID",    roleID));
            param.Add(new SqlParameter("@UserType",  roleName));

            dac.update(
                @"INSERT INTO prabha.UserInfo 
                    (CompanyID, FullName, UserName, UPassword, Email, Mobile, RoleID, UserType, IsActive, CreatedDate)
                  VALUES 
                    (@CompanyID, @FullName, @UserName, @UPassword, @Email, @Mobile, @RoleID, @UserType, 1, GETDATE())",
                param);

            ShowMsg("User successfully add ho gaya!", true);
        }
        else
        {
            // UPDATE
            param = new List<SqlParameter>();
            param.Add(new SqlParameter("@FullName",  fullName));
            param.Add(new SqlParameter("@Email",     email));
            param.Add(new SqlParameter("@Mobile",    mobile));
            param.Add(new SqlParameter("@RoleID",    roleID));
            param.Add(new SqlParameter("@UserType",  roleName));
            param.Add(new SqlParameter("@UserID",    userID));
            param.Add(new SqlParameter("@CompanyID", companyID));

            string updateQ = @"UPDATE prabha.UserInfo SET
                                 FullName  = @FullName,
                                 Email     = @Email,
                                 Mobile    = @Mobile,
                                 RoleID    = @RoleID,
                                 UserType  = @UserType";

            // Password sirf tab update karo jab kuch likha ho
            if (!string.IsNullOrEmpty(password))
            {
                param.Add(new SqlParameter("@UPassword", password));
                updateQ += ", UPassword = @UPassword";
            }

            updateQ += " WHERE UserID=@UserID AND CompanyID=@CompanyID";

            dac.update(updateQ, param);
            ShowMsg("User successfully update ho gaya!", true);
        }

        ResetForm();
        LoadUsers();
    }

    // -------------------------------------------------------
    // Repeater: Edit / Activate / Deactivate
    // -------------------------------------------------------
    protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        companyID = Session["CompanyID"] != null ? Convert.ToInt32(Session["CompanyID"]) : 0;
        int targetUserID = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "EditUser")
        {
            // Load user into form
            param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserID",    targetUserID));
            param.Add(new SqlParameter("@CompanyID", companyID));

            dt = dac.GetDataTable(
                @"SELECT * FROM prabha.UserInfo 
                  WHERE UserID=@UserID AND CompanyID=@CompanyID",
                param);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                hdnUserID.Value       = targetUserID.ToString();
                txtFullName.Text      = row["FullName"] != DBNull.Value ? row["FullName"].ToString() : "";
                txtUserName.Text      = row["UserName"].ToString();
                txtPassword.Text      = "";  // password blank
                txtEmail.Text         = row["Email"] != DBNull.Value ? row["Email"].ToString() : "";
                txtMobile.Text        = row["Mobile"] != DBNull.Value ? row["Mobile"].ToString() : "";

                if (row["RoleID"] != DBNull.Value)
                    ddlRole.SelectedValue = row["RoleID"].ToString();

                lblFormTitle.Text     = "User Edit Karein";
                btnCancelEdit.Visible = true;

                // Scroll to top
                ScriptManager.RegisterStartupScript(this, GetType(), "scroll",
                    "window.scrollTo(0,0);", true);
            }
        }
        else if (e.CommandName == "Deactivate" || e.CommandName == "Activate")
        {
            bool isActive = e.CommandName == "Activate";

            param = new List<SqlParameter>();
            param.Add(new SqlParameter("@IsActive",  isActive ? 1 : 0));
            param.Add(new SqlParameter("@UserID",    targetUserID));
            param.Add(new SqlParameter("@CompanyID", companyID));

            dac.update(
                "UPDATE prabha.UserInfo SET IsActive=@IsActive WHERE UserID=@UserID AND CompanyID=@CompanyID",
                param);

            ShowMsg(isActive ? "User activate ho gaya!" : "User deactivate ho gaya!", true);
            LoadUsers();
        }
    }

    // -------------------------------------------------------
    // Cancel Edit
    // -------------------------------------------------------
    protected void btnCancelEdit_Click(object sender, EventArgs e)
    {
        ResetForm();
        LoadUsers();
    }

    // -------------------------------------------------------
    // Helpers
    // -------------------------------------------------------
    private void ResetForm()
    {
        hdnUserID.Value       = "0";
        txtFullName.Text      = "";
        txtUserName.Text      = "";
        txtPassword.Text      = "";
        txtEmail.Text         = "";
        txtMobile.Text        = "";
        ddlRole.SelectedIndex = 0;
        lblFormTitle.Text     = "New User Add Karein";
        btnCancelEdit.Visible = false;
    }

    private void ShowMsg(string msg, bool success)
    {
        lblMsg.Text      = msg;
        lblMsg.CssClass  = success ? "msg-success" : "msg-error";
        lblMsg.Visible   = true;
    }
}
