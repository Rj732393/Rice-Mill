using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using substitute;

public partial class Includes_AdminMenu : System.Web.UI.UserControl
{
    DataAccessLayer dac = new DataAccessLayer();

    protected void Page_Load(object sender, EventArgs e)
    {
        // Session check
        if (Session["User"] == null)
        {
            Response.Redirect("~/Login.aspx");
            return;
        }

        // Company naam session se
        string companyName = "Rashmi Rice";
        if (Session["CompanyName"] != null &&
            !string.IsNullOrWhiteSpace(Session["CompanyName"].ToString()))
        {
            companyName = Session["CompanyName"].ToString();
        }

        // Username session se
        string userName = Session["User"] != null
            ? Session["User"].ToString()
            : "Admin";

        // Sidebar + Navbar labels
        lblSidebarCompany.Text = companyName;
        lblNavbarCompany.Text = companyName + " Management";

        // Dropdown mein naam set karo
        lblAdminName.Text = userName;
        lblDropdownName.Text = userName;

        // "Users" link sirf Admin role ko dikhe (sidebar me)
        phUserMgmt.Visible = (Session["UserType"] != null &&
            Session["UserType"].ToString() == "Admin");

        if (!IsPostBack)
        {
            BindOperators();
        }
    }

    private void BindOperators()
    {
        try
        {
            int companyID = Session["CompanyID"] != null ? Convert.ToInt32(Session["CompanyID"]) : 0;

            var param = new List<SqlParameter>();
            param.Add(new SqlParameter("@CompanyID", companyID));

            DataTable dt = dac.GetDataTable(
                @"SELECT ID, UserName, UPassword, UserType, CreatedDate, IsActive
                  FROM prabha.UserInfo
                  WHERE UserType = 'Operator' AND CompanyID = @CompanyID
                  ORDER BY CreatedDate DESC",
                param);

            gvOperators.DataSource = dt;
            gvOperators.DataBind();
        }
        catch (Exception)
        {
            // ignore - list rehne do empty
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            int companyID = Session["CompanyID"] != null ? Convert.ToInt32(Session["CompanyID"]) : 0;
            string uname = txtUser.Text.Trim();
            string pass = txtPass.Text;

            // Duplicate username check - GLOBALLY unique (Login query CompanyID ke bina match karti hai)
            var chkParam = new List<SqlParameter>();
            chkParam.Add(new SqlParameter("@UserName", uname));

            object existingCount = dac.Scalar(
                "SELECT COUNT(*) FROM prabha.UserInfo WHERE UserName=@UserName",
                chkParam);

            if (Convert.ToInt32(existingCount) > 0)
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "⚠ Operator '" + uname + "' already exists (kisi bhi company me)!";
                lblMsg.Style["display"] = "block";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openModal",
                    "document.getElementById('addOperatorModal').style.display='flex';", true);
                return;
            }

            var param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserName", uname));
            param.Add(new SqlParameter("@UPassword", pass));
            param.Add(new SqlParameter("@CompanyID", companyID));
            param.Add(new SqlParameter("@RoleID", 5)); // 5 = Operator

            dac.update(
                @"INSERT INTO prabha.UserInfo (UserName,UPassword,CreatedDate,UserType,IsActive,CompanyID,RoleID)
                  VALUES (@UserName,@UPassword,GETDATE(),'Operator',1,@CompanyID,@RoleID)",
                param);

            txtUser.Text = "";
            txtPass.Text = "";
            lblMsg.Text = "";
            BindOperators();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal",
                "closeAddOperatorModal();", true);
        }
        catch (Exception ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Style["display"] = "block";
            lblMsg.Text = "Error: " + ex.Message;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "openModal",
                "document.getElementById('addOperatorModal').style.display='flex';", true);
        }
    }

    protected void gvOperators_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int id = Convert.ToInt32(e.CommandArgument);
        int companyID = Session["CompanyID"] != null ? Convert.ToInt32(Session["CompanyID"]) : 0;

        if (e.CommandName == "EditOp")
        {
            try
            {
                var param = new List<SqlParameter>();
                param.Add(new SqlParameter("@ID", id));
                param.Add(new SqlParameter("@CompanyID", companyID));

                DataTable dt = dac.GetDataTable(
                    @"SELECT ID, UserName FROM prabha.UserInfo 
                      WHERE ID = @ID AND CompanyID = @CompanyID AND UserType = 'Operator'",
                    param);

                if (dt.Rows.Count > 0)
                {
                    hdnEditID.Value = dt.Rows[0]["ID"].ToString();
                    txtEditUser.Text = dt.Rows[0]["UserName"].ToString();
                }

                txtEditPass.Text = "";
                lblEditMsg.Text = "";
                lblEditMsg.Style["display"] = "none";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openEditModal",
                    "document.getElementById('viewOperatorsModal').style.display='flex';" +
                    "document.getElementById('editOperatorModal').style.display='flex';", true);
            }
            catch (Exception)
            {
                // ignore
            }
        }
        else if (e.CommandName == "SuspendOp")
        {
            try
            {
                var param = new List<SqlParameter>();
                param.Add(new SqlParameter("@ID", id));
                param.Add(new SqlParameter("@CompanyID", companyID));

                dac.update(
                    "UPDATE prabha.UserInfo SET IsActive = CASE WHEN IsActive=1 THEN 0 ELSE 1 END " +
                    "WHERE ID = @ID AND CompanyID = @CompanyID AND UserType = 'Operator'",
                    param);

                BindOperators();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openViewModal",
                    "document.getElementById('viewOperatorsModal').style.display='flex';", true);
            }
            catch (Exception)
            {
                // ignore
            }
        }
        else if (e.CommandName == "DeleteOp")
        {
            try
            {
                var param = new List<SqlParameter>();
                param.Add(new SqlParameter("@ID", id));
                param.Add(new SqlParameter("@CompanyID", companyID));

                dac.update(
                    "DELETE FROM prabha.UserInfo WHERE ID = @ID AND CompanyID = @CompanyID AND UserType = 'Operator'",
                    param);

                BindOperators();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openViewModal",
                    "document.getElementById('viewOperatorsModal').style.display='flex';", true);
            }
            catch (Exception)
            {
                // ignore
            }
        }
    }

    protected void btnEditSave_Click(object sender, EventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(hdnEditID.Value);
            int companyID = Session["CompanyID"] != null ? Convert.ToInt32(Session["CompanyID"]) : 0;
            string newUserName = txtEditUser.Text.Trim();

            // Duplicate check (globally unique, excluding current row)
            var chkParam = new List<SqlParameter>();
            chkParam.Add(new SqlParameter("@UserName", newUserName));
            chkParam.Add(new SqlParameter("@ID", id));

            object existingCount = dac.Scalar(
                "SELECT COUNT(*) FROM prabha.UserInfo WHERE UserName = @UserName AND ID <> @ID",
                chkParam);

            if (Convert.ToInt32(existingCount) > 0)
            {
                lblEditMsg.ForeColor = System.Drawing.Color.Red;
                lblEditMsg.Text = "⚠ Username already in use!";
                lblEditMsg.Style["display"] = "block";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openEditModal",
                    "document.getElementById('viewOperatorsModal').style.display='flex';" +
                    "document.getElementById('editOperatorModal').style.display='flex';", true);
                return;
            }

            if (string.IsNullOrEmpty(txtEditPass.Text.Trim()))
            {
                var param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserName", newUserName));
                param.Add(new SqlParameter("@ID", id));
                param.Add(new SqlParameter("@CompanyID", companyID));

                dac.update(
                    "UPDATE prabha.UserInfo SET UserName = @UserName WHERE ID = @ID AND CompanyID = @CompanyID AND UserType = 'Operator'",
                    param);
            }
            else
            {
                var param = new List<SqlParameter>();
                param.Add(new SqlParameter("@UserName", newUserName));
                param.Add(new SqlParameter("@UPassword", txtEditPass.Text));
                param.Add(new SqlParameter("@ID", id));
                param.Add(new SqlParameter("@CompanyID", companyID));

                dac.update(
                    "UPDATE prabha.UserInfo SET UserName = @UserName, UPassword = @UPassword WHERE ID = @ID AND CompanyID = @CompanyID AND UserType = 'Operator'",
                    param);
            }

            BindOperators();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeEditModal",
                "closeEditOperatorModal(); document.getElementById('viewOperatorsModal').style.display='flex';", true);
        }
        catch (Exception ex)
        {
            lblEditMsg.ForeColor = System.Drawing.Color.Red;
            lblEditMsg.Style["display"] = "block";
            lblEditMsg.Text = "Error: " + ex.Message;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "openEditModal",
                "document.getElementById('viewOperatorsModal').style.display='flex';" +
                "document.getElementById('editOperatorModal').style.display='flex';", true);
        }
    }
}