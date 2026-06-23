using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using substitute;

public partial class superadmin_Dashboard : System.Web.UI.Page
{
    DataAccessLayer dac = new DataAccessLayer();
    SaaSHelper saas = new SaaSHelper();

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
                lblMsg.Text = "Company successfully ban gayi!";
            }
            else if (Request.QueryString["msg"] == "updated")
            {
                pnlMsg.Visible = true;
                lblMsg.Text = "Company details successfully update ho gayi!";
            }
            else if (Request.QueryString["msg"] == "subupdated")
            {
                pnlMsg.Visible = true;
                lblMsg.Text = "Subscription successfully update ho gayi!";
            }

            LoadCompanies();
            LoadStats();
        }
    }

    private void LoadCompanies()
    {
        DataTable dt = dac.GetDataTable(@"
            SELECT c.*, sp.PlanName
            FROM prabha.Companies c
            LEFT JOIN prabha.SubscriptionPlans sp ON sp.PlanID = c.PlanID
            ORDER BY c.CreatedDate DESC", null);

        gvCompanies.DataSource = dt;
        gvCompanies.DataBind();
    }

    private void LoadStats()
    {
        DataTable dt = dac.GetDataTable("SELECT * FROM prabha.Companies", null);

        int total = dt.Rows.Count, active = 0, expired = 0, suspended = 0, expiringSoon = 0, recent = 0;

        foreach (DataRow row in dt.Rows)
        {
            string status = row["Status"].ToString();
            DateTime subEnd = Convert.ToDateTime(row["SubscriptionEnd"]);
            DateTime created = Convert.ToDateTime(row["CreatedDate"]);

            if (status == "Suspended" || status == "Blocked")
            {
                suspended++;
            }
            else if (subEnd < DateTime.Today)
            {
                expired++;
            }
            else
            {
                active++;
                if ((subEnd - DateTime.Today).Days <= 7) expiringSoon++;
            }

            if (created.Month == DateTime.Today.Month && created.Year == DateTime.Today.Year)
                recent++;
        }

        lblTotal.Text = total.ToString();
        lblActive.Text = active.ToString();
        lblExpired.Text = expired.ToString();
        lblSuspended.Text = suspended.ToString();
        lblExpiringSoon.Text = expiringSoon.ToString();
        lblRecent.Text = recent.ToString();

        // Total Users (across all companies)
        object totalUsersObj = dac.Scalar("SELECT COUNT(*) FROM prabha.UserInfo", null);
        lblTotalUsers.Text = Convert.ToString(totalUsersObj);

        // Subscription Revenue (sum of subscription amounts ever recorded)
        object revenueObj = dac.Scalar("SELECT ISNULL(SUM(Amount),0) FROM prabha.CompanySubscriptions", null);
        decimal revenue = revenueObj != null && revenueObj != DBNull.Value ? Convert.ToDecimal(revenueObj) : 0;
        lblRevenue.Text = "₹" + revenue.ToString("N0");
    }

    protected void gvCompanies_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string[] args = e.CommandArgument.ToString().Split(',');
        int companyID = Convert.ToInt32(args[0]);

        if (e.CommandName == "ToggleActive")
        {
            string currentStatus = args[1];
            string newStatus = currentStatus == "Suspended" ? "Active" : "Suspended";

            var param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Status", newStatus));
            param.Add(new SqlParameter("@CompanyID", companyID));

            dac.update("UPDATE prabha.Companies SET Status=@Status WHERE CompanyID=@CompanyID", param);

            saas.LogAction(companyID, Session["User"].ToString(), "SuperAdmin",
                newStatus == "Suspended" ? "CompanySuspended" : "CompanyActivated",
                "Company", "Company status changed to " + newStatus);

            LoadCompanies();
            LoadStats();
        }
        else if (e.CommandName == "ToggleBlock")
        {
            string currentStatus = args[1];
            string newStatus = currentStatus == "Blocked" ? "Active" : "Blocked";

            var param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Status", newStatus));
            param.Add(new SqlParameter("@CompanyID", companyID));

            dac.update("UPDATE prabha.Companies SET Status=@Status WHERE CompanyID=@CompanyID", param);

            saas.LogAction(companyID, Session["User"].ToString(), "SuperAdmin",
                newStatus == "Blocked" ? "CompanyBlocked" : "CompanyUnblocked",
                "Company", "Company status changed to " + newStatus);

            LoadCompanies();
            LoadStats();
        }
        else if (e.CommandName == "ResetPassword")
        {
            var param = new List<SqlParameter>();
            param.Add(new SqlParameter("@AdminPassword", "Admin@123"));
            param.Add(new SqlParameter("@CompanyID", companyID));

            dac.update("UPDATE prabha.Companies SET AdminPassword=@AdminPassword WHERE CompanyID=@CompanyID", param);

            saas.LogAction(companyID, Session["User"].ToString(), "SuperAdmin",
                "PasswordReset", "Company", "Company admin password reset to default");

            pnlMsg.Visible = true;
            lblMsg.Text = "Company Admin ka password 'Admin@123' kar diya gaya hai. Unhe agle login par badalne ko kahein.";

            LoadCompanies();
            LoadStats();
        }
    }

    // Plan name fallback for companies without a valid PlanID
    public string GetPlanName(object planNameObj)
    {
        if (planNameObj == null || planNameObj == DBNull.Value || string.IsNullOrWhiteSpace(planNameObj.ToString()))
            return "Not Set";

        return planNameObj.ToString();
    }

    // Status badge HTML
    public string GetStatusBadge(object statusObj, object subEndObj)
    {
        string status = statusObj.ToString();
        DateTime subEnd = Convert.ToDateTime(subEndObj);

        if (status == "Blocked")
            return "<span class='badge-inactive'>Blocked</span>";
        if (status == "Suspended")
            return "<span class='badge-suspended'>Suspended</span>";
        if (subEnd < DateTime.Today)
            return "<span class='badge-inactive'>Expired</span>";
        if (subEnd <= DateTime.Today.AddDays(7))
            return "<span class='badge-expiring'>Expiring Soon</span>";

        return "<span class='badge-active'>Active</span>";
    }
}
