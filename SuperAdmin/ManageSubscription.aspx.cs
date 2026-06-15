using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using substitute;

public partial class superadmin_ManageSubscription : System.Web.UI.Page
{
    DataAccessLayer dac = new DataAccessLayer();
    SaaSHelper saas = new SaaSHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null || Session["UserType"] == null ||
            Session["UserType"].ToString() != "SuperAdmin")
        {
            Response.Redirect("../Login.aspx");
            return;
        }

        if (Request.QueryString["id"] == null)
        {
            Response.Redirect("Dashboard.aspx");
            return;
        }

        int companyID = Convert.ToInt32(Request.QueryString["id"]);
        hfCompanyID.Value = companyID.ToString();

        if (!IsPostBack)
        {
            LoadCompany(companyID);
            LoadHistory(companyID);

            if (Request.QueryString["msg"] == "done")
            {
                pnlMsg.Visible = true;
                lblMsg.Text = "Subscription successfully update ho gayi!";
            }
        }
    }

    private void LoadCompany(int companyID)
    {
        var param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CompanyID", companyID));

        DataTable dt = dac.GetDataTable(
            "SELECT * FROM prabha.Companies WHERE CompanyID = @CompanyID", param);

        if (dt.Rows.Count == 0)
        {
            Response.Redirect("Dashboard.aspx");
            return;
        }

        DataRow r = dt.Rows[0];
        lblCompanyName.Text = r["CompanyName"].ToString();
        lblCurrentStatus.Text = r["Status"].ToString();
        lblStartDate.Text = Convert.ToDateTime(r["SubscriptionStart"]).ToString("dd-MMM-yyyy");
        lblEndDate.Text = Convert.ToDateTime(r["SubscriptionEnd"]).ToString("dd-MMM-yyyy");

        txtFromDate.Text = Convert.ToDateTime(r["SubscriptionStart"]).ToString("yyyy-MM-dd");
        txtToDate.Text = Convert.ToDateTime(r["SubscriptionEnd"]).ToString("yyyy-MM-dd");

        bool isSuspended = r["Status"].ToString() == "Suspended";
        btnSuspend.Visible = !isSuspended;
        btnActivate.Visible = isSuspended;
    }

    private void LoadHistory(int companyID)
    {
        var param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CompanyID", companyID));

        DataTable dt = dac.GetDataTable(@"
            SELECT * FROM prabha.CompanySubscriptions
            WHERE CompanyID = @CompanyID
            ORDER BY CreatedDate DESC", param);

        gvHistory.DataSource = dt;
        gvHistory.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int companyID = Convert.ToInt32(hfCompanyID.Value);

        DateTime fromDate, toDate;
        if (!DateTime.TryParse(txtFromDate.Text, out fromDate) ||
            !DateTime.TryParse(txtToDate.Text, out toDate))
        {
            lblErr.Text = "Sahi dates daalein!";
            return;
        }

        if (toDate < fromDate)
        {
            lblErr.Text = "To Date, From Date se pehle nahi ho sakti!";
            return;
        }

        // Update Companies
        var updParam = new List<SqlParameter>();
        updParam.Add(new SqlParameter("@SubStart", fromDate));
        updParam.Add(new SqlParameter("@SubEnd", toDate));
        updParam.Add(new SqlParameter("@CompanyID", companyID));

        dac.update(@"UPDATE prabha.Companies
            SET SubscriptionStart=@SubStart, SubscriptionEnd=@SubEnd
            WHERE CompanyID=@CompanyID", updParam);

        // Get existing PlanID (kept as-is for history record)
        var planParam = new List<SqlParameter>();
        planParam.Add(new SqlParameter("@CompanyID", companyID));
        DataTable dtComp = dac.GetDataTable(
            "SELECT PlanID, Status FROM prabha.Companies WHERE CompanyID=@CompanyID", planParam);

        int? planID = dtComp.Rows[0]["PlanID"] != DBNull.Value ? Convert.ToInt32(dtComp.Rows[0]["PlanID"]) : (int?)null;
        string currentStatus = dtComp.Rows[0]["Status"].ToString();

        // Insert subscription history
        var subParam = new List<SqlParameter>();
        subParam.Add(new SqlParameter("@CompanyID", companyID));
        subParam.Add(new SqlParameter("@PlanID", planID.HasValue ? (object)planID.Value : DBNull.Value));
        subParam.Add(new SqlParameter("@StartDate", fromDate));
        subParam.Add(new SqlParameter("@EndDate", toDate));
        subParam.Add(new SqlParameter("@Status", currentStatus));
        subParam.Add(new SqlParameter("@Remarks", string.IsNullOrWhiteSpace(txtRemarks.Text) ? "Subscription dates updated" : txtRemarks.Text.Trim()));
        subParam.Add(new SqlParameter("@CreatedBy", Session["User"].ToString()));

        dac.update(@"INSERT INTO prabha.CompanySubscriptions
            (CompanyID, PlanID, StartDate, EndDate, Status, Amount, Remarks, CreatedBy, CreatedDate)
            VALUES (@CompanyID, @PlanID, @StartDate, @EndDate, @Status, 0, @Remarks, @CreatedBy, GETDATE())", subParam);

        saas.LogAction(companyID, Session["User"].ToString(), "SuperAdmin",
            "SubscriptionUpdated", "Subscription",
            "Subscription set from " + fromDate.ToString("dd-MMM-yyyy") + " to " + toDate.ToString("dd-MMM-yyyy"));

        Response.Redirect("ManageSubscription.aspx?id=" + companyID + "&msg=done");
    }

    protected void btnSuspend_Click(object sender, EventArgs e)
    {
        int companyID = Convert.ToInt32(hfCompanyID.Value);

        var param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CompanyID", companyID));
        dac.update("UPDATE prabha.Companies SET Status=N'Suspended' WHERE CompanyID=@CompanyID", param);

        saas.LogAction(companyID, Session["User"].ToString(), "SuperAdmin",
            "SubscriptionSuspended", "Subscription",
            string.IsNullOrWhiteSpace(txtRemarks.Text) ? "Subscription suspended" : txtRemarks.Text.Trim());

        Response.Redirect("ManageSubscription.aspx?id=" + companyID + "&msg=done");
    }

    protected void btnActivate_Click(object sender, EventArgs e)
    {
        int companyID = Convert.ToInt32(hfCompanyID.Value);

        var param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CompanyID", companyID));
        dac.update("UPDATE prabha.Companies SET Status=N'Active' WHERE CompanyID=@CompanyID", param);

        saas.LogAction(companyID, Session["User"].ToString(), "SuperAdmin",
            "SubscriptionActivated", "Subscription",
            string.IsNullOrWhiteSpace(txtRemarks.Text) ? "Subscription re-activated" : txtRemarks.Text.Trim());

        Response.Redirect("ManageSubscription.aspx?id=" + companyID + "&msg=done");
    }

    public string GetHistoryBadge(object statusObj)
    {
        string status = statusObj.ToString();
        switch (status)
        {
            case "Active":
            case "Renewed":
                return "<span class='badge-active'>" + status + "</span>";
            case "Suspended":
                return "<span class='badge-suspended'>Suspended</span>";
            default:
                return "<span class='badge-inactive'>" + status + "</span>";
        }
    }
}
