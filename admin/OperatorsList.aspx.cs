using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using substitute;

public partial class admin_OperatorsList : System.Web.UI.Page
{
    DataAccessLayer dac = new DataAccessLayer();

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

        var param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CompanyID", companyID));

        DataTable dt = dac.GetDataTable(
            @"SELECT ID, UserName, CreatedDate, IsActive
              FROM prabha.UserInfo
              WHERE CompanyID=@CompanyID AND UserType='Operator'
              ORDER BY CreatedDate DESC",
            param);

        gvOperators.DataSource = dt;
        gvOperators.DataBind();
    }

    protected void gvOperators_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int id = Convert.ToInt32(e.CommandArgument);
        int companyID = Session["CompanyID"] != null ? Convert.ToInt32(Session["CompanyID"]) : 0;

        if (e.CommandName == "ToggleActive")
        {
            var param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ID", id));
            param.Add(new SqlParameter("@CompanyID", companyID));

            dac.update(
                "UPDATE prabha.UserInfo SET IsActive = CASE WHEN IsActive=1 THEN 0 ELSE 1 END WHERE ID=@ID AND CompanyID=@CompanyID",
                param);

            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Status update ho gaya.";

            BindOperators();
        }
        else if (e.CommandName == "ResetPassword")
        {
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

            var param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UPassword", newPass));
            param.Add(new SqlParameter("@ID", id));
            param.Add(new SqlParameter("@CompanyID", companyID));

            dac.update(
                "UPDATE prabha.UserInfo SET UPassword=@UPassword WHERE ID=@ID AND CompanyID=@CompanyID",
                param);

            lblMsg.ForeColor = System.Drawing.Color.Green;
            lblMsg.Text = "Password reset ho gaya.";

            BindOperators();
        }
    }
}