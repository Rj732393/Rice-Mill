using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class admin_OperatorsList : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(
        ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null ||
            Session["UserType"] == null ||
            Session["UserType"].ToString() != "Admin")
        {
            Response.Redirect("../Login.aspx");
            return;
        }

        if (!IsPostBack)
        {
            BindOperators();
        }
    }

    private void BindOperators()
    {
        int companyID = Session["CompanyID"] != null ? Convert.ToInt32(Session["CompanyID"]) : 0;

        con.Open();

        SqlCommand cmd = new SqlCommand(
            "SELECT ID, UserName, CreatedDate, IsActive " +
            "FROM prabha.UserInfo " +
            "WHERE CompanyID=@cid AND UserType='Operator' " +
            "ORDER BY CreatedDate DESC", con);
        cmd.Parameters.AddWithValue("@cid", companyID);

        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);

        con.Close();

        gvOperators.DataSource = dt;
        gvOperators.DataBind();
    }

    protected void gvOperators_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int userID = Convert.ToInt32(e.CommandArgument);
        int companyID = Session["CompanyID"] != null ? Convert.ToInt32(Session["CompanyID"]) : 0;

        if (e.CommandName == "ToggleActive")
        {
            con.Open();

            SqlCommand cmd = new SqlCommand(
                "UPDATE prabha.UserInfo SET IsActive = CASE WHEN IsActive=1 THEN 0 ELSE 1 END " +
                "WHERE ID=@id AND CompanyID=@cid", con);
            cmd.Parameters.AddWithValue("@id", userID);
            cmd.Parameters.AddWithValue("@cid", companyID);
            cmd.ExecuteNonQuery();

            con.Close();

            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Status update ho gaya.";

            BindOperators();
        }
        else if (e.CommandName == "ResetPassword")
        {
            // Find the row's new-password textbox
            GridViewRow row = (GridViewRow)((System.Web.UI.WebControls.Button)e.CommandSource).NamingContainer;
            TextBox txtNewPass = (TextBox)row.FindControl("txtNewPass");

            string newPass = txtNewPass.Text.Trim();

            if (string.IsNullOrEmpty(newPass))
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Naya password khali nahi ho sakta.";
                BindOperators();
                return;
            }

            con.Open();

            SqlCommand cmd = new SqlCommand(
                "UPDATE prabha.UserInfo SET UPassword=@p " +
                "WHERE ID=@id AND CompanyID=@cid", con);
            cmd.Parameters.AddWithValue("@p", newPass);
            cmd.Parameters.AddWithValue("@id", userID);
            cmd.Parameters.AddWithValue("@cid", companyID);
            cmd.ExecuteNonQuery();

            con.Close();

            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Password reset ho gaya.";

            BindOperators();
        }
    }
}