using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using substitute;

public partial class superadmin_Dashboard : System.Web.UI.Page
{
    DataAccessLayer dac = new DataAccessLayer();

    protected void Page_Load(object sender, EventArgs e)
    {
        // Sirf SuperAdmin aa sake
        if (Session["User"] == null || Session["UserType"] == null ||
            Session["UserType"].ToString() != "SuperAdmin")
        {
            Response.Redirect("../Login.aspx");
            return;
        }

        lblAdmin.Text = "Welcome, " + Session["User"].ToString();

        if (!IsPostBack)
        {
            // Success message check
            if (Request.QueryString["msg"] == "saved")
            {
                pnlMsg.Visible = true;
                lblMsg.Text = "Company subscription successfully save ho gayi!";
            }
            else if (Request.QueryString["msg"] == "updated")
            {
                pnlMsg.Visible = true;
                lblMsg.Text = "Company details successfully update ho gayi!";
            }

            LoadCompanies();
        }
    }

    private void LoadCompanies()
    {
        DataTable dt = dac.GetDataTable(
            "SELECT * FROM prabha.Companies ORDER BY CreatedDate DESC", null);

        gvCompanies.DataSource = dt;
        gvCompanies.DataBind();

        int total = dt.Rows.Count, active = 0, expired = 0;

        foreach (DataRow row in dt.Rows)
        {
            bool isActive = Convert.ToBoolean(row["IsActive"]);
            DateTime subEnd = Convert.ToDateTime(row["SubscriptionEnd"]);

            if (isActive && subEnd >= DateTime.Today) active++;
            else expired++;
        }

        lblTotal.Text = total.ToString();
        lblActive.Text = active.ToString();
        lblExpired.Text = expired.ToString();
    }

    protected void gvCompanies_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "ToggleActive")
        {
            string[] args = e.CommandArgument.ToString().Split(',');
            int companyID = Convert.ToInt32(args[0]);
            bool currentState = Convert.ToBoolean(args[1]);
            bool newState = !currentState;

            var param = new List<SqlParameter>();
            param.Add(new SqlParameter("@IsActive", newState));
            param.Add(new SqlParameter("@CompanyID", companyID));

            dac.update(
                "UPDATE prabha.Companies SET IsActive=@IsActive WHERE CompanyID=@CompanyID",
                param);

            LoadCompanies();
        }
    }

    // Status badge HTML
    public string GetStatusBadge(object isActiveObj, object subEndObj)
    {
        bool isActive = Convert.ToBoolean(isActiveObj);
        DateTime subEnd = Convert.ToDateTime(subEndObj);

        if (!isActive)
            return "<span class='badge-inactive'>Band Hai</span>";
        if (subEnd < DateTime.Today)
            return "<span class='badge-inactive'>Expire Ho Gaya</span>";
        if (subEnd <= DateTime.Today.AddDays(30))
            return "<span class='badge-expiring'>30 Din Bacha</span>";

        return "<span class='badge-active'>Active</span>";
    }
}
