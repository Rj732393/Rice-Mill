using System;


using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Includes_AdminMenu : System.Web.UI.UserControl
{
    SqlConnection con = new SqlConnection(
        ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        // Session check
        if (Session["User"] == null)
        {
            Response.Redirect("~/Login.aspx");
            return;
        }

        // Company naam session se
        

        // Username session se
        string userName = Session["User"] != null
            ? Session["User"].ToString()
            : "Admin";

        // Sidebar + Navbar labels
        lblSidebarCompany.Text = "Rice Mill";
        lblNavbarCompany.Text = "Rice Mill Management System";

        lblAdminName.Text = userName;
        lblDropdownName.Text = userName;

        // 👇👇 YAHAN ADD KARNA HAI
        if (!IsPostBack)
        {
            BindOperators();

            string page = System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToLower();

            lnkDashboard.Attributes.Remove("class");
            lnkRiceStock.Attributes.Remove("class");
            lnkPaddyStock.Attributes.Remove("class");
            lnkSalePurchase.Attributes.Remove("class");
            lnkEditSauda.Attributes.Remove("class");

            switch (page)
            {
                case "dashboard.aspx":
                    lnkDashboard.Attributes["class"] = "active";
                    break;

                case "ricestock.aspx":
                    lnkRiceStock.Attributes["class"] = "active";
                    break;

                case "paddystock.aspx":
                    lnkPaddyStock.Attributes["class"] = "active";
                    break;

                case "salepurchaseexpense.aspx":
                    lnkSalePurchase.Attributes["class"] = "active";
                    break;

                case "editbysauda.aspx":
                    lnkEditSauda.Attributes["class"] = "active";
                    break;
            }
        }
    }
    private void BindOperators()
    {
        try
        {
            int companyID = Session["CompanyID"] != null ? Convert.ToInt32(Session["CompanyID"]) : 0;
            // sirf Operator dikhna chaiye, Admin nahi
            SqlCommand cmd = new SqlCommand(
                "SELECT ID, UserName, UPassword, UserType, CreatedDate, IsActive " +
                "FROM prabha.UserInfo " +
                "WHERE UserType = 'Operator' AND CompanyID = @cid " +
                "ORDER BY CreatedDate DESC", con);
            cmd.Parameters.AddWithValue("@cid", companyID);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            con.Open();
            da.Fill(dt);
            con.Close();
            gvOperators.DataSource = dt;
            gvOperators.DataBind();
        }
        catch (Exception)
        {
            if (con.State == ConnectionState.Open) con.Close();
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            con.Open();
            int companyID = Session["CompanyID"] != null ? Convert.ToInt32(Session["CompanyID"]) : 0;
            // Pehle check karo username already exist karta hai ya nahi
            SqlCommand checkCmd = new SqlCommand(
                "SELECT COUNT(*) FROM prabha.UserInfo WHERE UserName = @u AND CompanyID = @cid", con);
            checkCmd.Parameters.AddWithValue("@u", txtUser.Text.Trim());
            checkCmd.Parameters.AddWithValue("@cid", companyID);
            int count = (int)checkCmd.ExecuteScalar();
            if (count > 0)
            {
                con.Close();
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "⚠ Operator '" + txtUser.Text.Trim() + "' already exists!";
                lblMsg.Style["display"] = "block";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openModal",
                    "document.getElementById('addOperatorModal').style.display='flex';", true);
                return;
            }
            // Nahi hai toh insert karo
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO prabha.UserInfo (UserName,UPassword,CreatedDate,UserType,IsActive,CompanyID) " +
                "VALUES (@u,@p,GETDATE(),'Operator',1,@cid)", con);
            cmd.Parameters.AddWithValue("@u", txtUser.Text.Trim());
            cmd.Parameters.AddWithValue("@p", txtPass.Text);
            cmd.Parameters.AddWithValue("@cid", companyID);
            cmd.ExecuteNonQuery();
            con.Close();
            txtUser.Text = "";
            txtPass.Text = "";
            lblMsg.Text = "";
            BindOperators(); // list refresh - naya operator bhi dikhega
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeModal",
                "closeAddOperatorModal();", true);
        }
        catch (Exception ex)
        {
            if (con.State == ConnectionState.Open) con.Close();
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
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "SELECT ID, UserName FROM prabha.UserInfo " +
                    "WHERE ID = @id AND CompanyID = @cid AND UserType = 'Operator'", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@cid", companyID);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    hdnEditID.Value = rdr["ID"].ToString();
                    txtEditUser.Text = rdr["UserName"].ToString();
                }
                rdr.Close();
                con.Close();
                txtEditPass.Text = "";
                lblEditMsg.Text = "";
                lblEditMsg.Style["display"] = "none";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openEditModal",
                    "document.getElementById('viewOperatorsModal').style.display='flex';" +
                    "document.getElementById('editOperatorModal').style.display='flex';", true);
            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
        }
        else if (e.CommandName == "SuspendOp")
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "UPDATE prabha.UserInfo SET IsActive = CASE WHEN IsActive=1 THEN 0 ELSE 1 END " +
                    "WHERE ID = @id AND CompanyID = @cid AND UserType = 'Operator'", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@cid", companyID);
                cmd.ExecuteNonQuery();
                con.Close();
                BindOperators();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openViewModal",
                    "document.getElementById('viewOperatorsModal').style.display='flex';", true);
            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
        }
        else if (e.CommandName == "DeleteOp")
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM prabha.UserInfo WHERE ID = @id AND CompanyID = @cid AND UserType = 'Operator'", con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@cid", companyID);
                cmd.ExecuteNonQuery();
                con.Close();
                BindOperators();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "openViewModal",
                    "document.getElementById('viewOperatorsModal').style.display='flex';", true);
            }
            catch (Exception)
            {
                if (con.State == ConnectionState.Open) con.Close();
            }
        }
    }

    protected void btnEditSave_Click(object sender, EventArgs e)
    {
        try
        {
            int id = Convert.ToInt32(hdnEditID.Value);
            int companyID = Session["CompanyID"] != null ? Convert.ToInt32(Session["CompanyID"]) : 0;

            con.Open();

            // duplicate username check (excluding current row)
            SqlCommand checkCmd = new SqlCommand(
                "SELECT COUNT(*) FROM prabha.UserInfo WHERE UserName = @u AND CompanyID = @cid AND ID <> @id", con);
            checkCmd.Parameters.AddWithValue("@u", txtEditUser.Text.Trim());
            checkCmd.Parameters.AddWithValue("@cid", companyID);
            checkCmd.Parameters.AddWithValue("@id", id);
            int count = (int)checkCmd.ExecuteScalar();
            if (count > 0)
            {
                con.Close();
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
                SqlCommand cmd = new SqlCommand(
                    "UPDATE prabha.UserInfo SET UserName = @u WHERE ID = @id AND CompanyID = @cid AND UserType = 'Operator'", con);
                cmd.Parameters.AddWithValue("@u", txtEditUser.Text.Trim());
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@cid", companyID);
                cmd.ExecuteNonQuery();
            }
            else
            {
                SqlCommand cmd = new SqlCommand(
                    "UPDATE prabha.UserInfo SET UserName = @u, UPassword = @p WHERE ID = @id AND CompanyID = @cid AND UserType = 'Operator'", con);
                cmd.Parameters.AddWithValue("@u", txtEditUser.Text.Trim());
                cmd.Parameters.AddWithValue("@p", txtEditPass.Text);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@cid", companyID);
                cmd.ExecuteNonQuery();
            }

            con.Close();
            BindOperators();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "closeEditModal",
                "closeEditOperatorModal(); document.getElementById('viewOperatorsModal').style.display='flex';", true);
        }
        catch (Exception ex)
        {
            if (con.State == ConnectionState.Open) con.Close();
            lblEditMsg.ForeColor = System.Drawing.Color.Red;
            lblEditMsg.Style["display"] = "block";
            lblEditMsg.Text = "Error: " + ex.Message;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "openEditModal",
                "document.getElementById('viewOperatorsModal').style.display='flex';" +
                "document.getElementById('editOperatorModal').style.display='flex';", true);
        }
    }
}
