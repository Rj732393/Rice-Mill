using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using substitute;

public partial class admin_Dashboard : System.Web.UI.Page
{
    DataAccessLayer dac = new DataAccessLayer();

    protected void Page_Load(object sender, EventArgs e)
    {
       
        // Session check - sirf Admin (ya Manager/Accountant jo dashboard dekh sake) aa sake
        if (Session["User"] == null || Session["UserType"] == null || Session["CompanyID"] == null)
        {
            Response.Redirect("../Login.aspx");
            return;
        }

        if (Session["UserType"].ToString() != "Admin" && Session["UserType"].ToString() != "User")
        {
            Response.Redirect("../Login.aspx");
            return;
        }

        if (!IsPostBack)
        {
            
            lblUserName.Text = Session["User"].ToString();

            LoadStats();
            LoadRecentActivity();
        }
    }

    private void LoadStats()
    {
        int companyID = Convert.ToInt32(Session["CompanyID"]);
        var param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CompanyID", companyID));

        // Total Rice Stock
        object riceStockObj = dac.Scalar(
            "SELECT ISNULL(SUM(Rice_Weight),0) FROM prabha.RiceStock WHERE CompanyID=@CompanyID", param);
        lblRiceStock.Text = FormatNumber(riceStockObj);

        // Total Paddy Stock
        var param2 = new List<SqlParameter>();
        param2.Add(new SqlParameter("@CompanyID", companyID));
        object paddyStockObj = dac.Scalar(
            "SELECT ISNULL(SUM(Paddy_Weight),0) FROM prabha.PaddyStock WHERE CompanyID=@CompanyID", param2);
        lblPaddyStock.Text = FormatNumber(paddyStockObj);

        // Total Sales (count of Sale Sauda entries)
        var param3 = new List<SqlParameter>();
        param3.Add(new SqlParameter("@CompanyID", companyID));
        object salesCountObj = dac.Scalar(
            "SELECT COUNT(*) FROM prabha.Sale_Sauda_Master WHERE CompanyID=@CompanyID", param3);
        lblTotalSales.Text = Convert.ToString(salesCountObj);

        // Total Revenue (sum of Sale_Payment_Info AmountPaid)
        var param4 = new List<SqlParameter>();
        param4.Add(new SqlParameter("@CompanyID", companyID));
        object revenueObj = dac.Scalar(
            "SELECT ISNULL(SUM(CAST(AmountPaid AS DECIMAL(18,2))),0) FROM prabha.Sale_Payment_Info WHERE CompanyID=@CompanyID", param4);
        decimal revenue = revenueObj != null && revenueObj != DBNull.Value ? Convert.ToDecimal(revenueObj) : 0;
        lblRevenue.Text = "₹" + FormatRevenue(revenue);
    }

    private void LoadRecentActivity()
    {
        int companyID = Convert.ToInt32(Session["CompanyID"]);
        var param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CompanyID", companyID));

        DataTable dt = dac.GetDataTable(@"
            SELECT TOP 5 ActivityText, ActivityDate FROM (
                SELECT TOP 5 'Sale Sauda - ' + PartyName AS ActivityText, EntryDate AS ActivityDate
                FROM prabha.Sale_Sauda_Master WHERE CompanyID=@CompanyID
                UNION ALL
                SELECT TOP 5 'Purchase Sauda - ' + PartyName AS ActivityText, EntryDate AS ActivityDate
                FROM prabha.Purchase_Sauda_Info WHERE CompanyID=@CompanyID
                UNION ALL
                SELECT TOP 5 'Paddy Processing - ' + PaddyType AS ActivityText, EntryDate AS ActivityDate
                FROM prabha.PaddyProcessing WHERE CompanyID=@CompanyID
            ) AS Combined
            ORDER BY ActivityDate DESC", param);

        rptActivity.DataSource = dt;
        rptActivity.DataBind();

        pnlNoActivity.Visible = dt.Rows.Count == 0;
    }

    private string FormatNumber(object val)
    {
        if (val == null || val == DBNull.Value) return "0";
        decimal d = Convert.ToDecimal(val);
        return d.ToString("N0");
    }

    private string FormatRevenue(decimal revenue)
    {
        if (revenue >= 100000)
            return (revenue / 100000m).ToString("0.0") + "L";
        if (revenue >= 1000)
            return (revenue / 1000m).ToString("0.0") + "K";
        return revenue.ToString("N0");
    }
}
