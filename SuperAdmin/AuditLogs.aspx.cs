using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using substitute;

public partial class superadmin_AuditLogs : System.Web.UI.Page
{
    DataAccessLayer dac = new DataAccessLayer();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null || Session["UserType"] == null ||
            Session["UserType"].ToString() != "SuperAdmin")
        {
            Response.Redirect("../Login.aspx");
            return;
        }

        if (!IsPostBack)
        {
            LoadCompanies();
            BindLogs();
        }
    }

    private void LoadCompanies()
    {
        DataTable dt = dac.GetDataTable("SELECT CompanyID, CompanyName FROM prabha.Companies ORDER BY CompanyName", null);

        ddlCompany.Items.Clear();
        ddlCompany.Items.Add(new ListItem("-- All Companies --", ""));
        ddlCompany.Items.Add(new ListItem("Super Admin (System-level)", "0"));

        foreach (DataRow row in dt.Rows)
        {
            ddlCompany.Items.Add(new ListItem(row["CompanyName"].ToString(), row["CompanyID"].ToString()));
        }
    }

    private void BindLogs()
    {
        StringBuilder sb = new StringBuilder(@"
            SELECT al.LogID, al.CreatedDate, al.UserName, al.UserType, al.Action,
                   al.Module, al.Description, al.IPAddress,
                   ISNULL(c.CompanyName, 'System') AS CompanyName
            FROM prabha.AuditLogs al
            LEFT JOIN prabha.Companies c ON c.CompanyID = al.CompanyID
            WHERE 1=1");

        var param = new List<SqlParameter>();

        if (!string.IsNullOrEmpty(ddlCompany.SelectedValue))
        {
            if (ddlCompany.SelectedValue == "0")
            {
                sb.Append(" AND al.CompanyID IS NULL");
            }
            else
            {
                sb.Append(" AND al.CompanyID = @CompanyID");
                param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(ddlCompany.SelectedValue)));
            }
        }

        if (!string.IsNullOrEmpty(ddlAction.SelectedValue))
        {
            sb.Append(" AND al.Action = @Action");
            param.Add(new SqlParameter("@Action", ddlAction.SelectedValue));
        }

        DateTime from, to;
        if (DateTime.TryParse(txtFrom.Text, out from))
        {
            sb.Append(" AND al.CreatedDate >= @FromDate");
            param.Add(new SqlParameter("@FromDate", from.Date));
        }
        if (DateTime.TryParse(txtTo.Text, out to))
        {
            sb.Append(" AND al.CreatedDate < @ToDate");
            param.Add(new SqlParameter("@ToDate", to.Date.AddDays(1)));
        }

        sb.Append(" ORDER BY al.CreatedDate DESC");

        DataTable dt = dac.GetDataTable(sb.ToString(), param);
        gvLogs.DataSource = dt;
        gvLogs.DataBind();
    }

    protected void btnFilter_Click(object sender, EventArgs e)
    {
        gvLogs.PageIndex = 0;
        BindLogs();
    }

    protected void gvLogs_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvLogs.PageIndex = e.NewPageIndex;
        BindLogs();
    }
}
