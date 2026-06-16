using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using substitute;

public partial class admin_PaddyStock : System.Web.UI.Page
{
    DataTable dt;
    List<SqlParameter> param;
    DataAccessLayer dac;
    protected void Page_Load(object sender, EventArgs e)
    {
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

        if (!Page.IsPostBack)
        {
         

            // Company naam session se set karo
            string companyName = Session["CompanyName"] != null
                ? Session["CompanyName"].ToString()
                : "Rice Mills";
            lblCompanyName.Text = companyName;
        }
    }
   public void btnReport_ServerClick(object sender, EventArgs e)
{
    lblFromDateError.Text = "";
    lblToDateError.Text = "";

    if (string.IsNullOrWhiteSpace(fdate.Value))
    {
        lblFromDateError.Text = "Please Select From Date";
        return;
    }

    if (string.IsNullOrWhiteSpace(tdate.Value))
    {
        lblToDateError.Text = "Please Select To Date";
        return;
    }

    dt = new DataTable();
    string q = "";
    param = new List<SqlParameter>();

    param.Add(new SqlParameter("@Entry_Date1",
        Convert.ToDateTime(fdate.Value.Trim()).ToString("dd-MMM-yyyy")));

    param.Add(new SqlParameter("@Entry_Date2",
        Convert.ToDateTime(tdate.Value.Trim()).ToString("dd-MMM-yyyy")));

        if (srType.Value.Trim() == "Daily")
        {
            q = "select * from prabha.PaddyStock where Entry_Date>=@Entry_Date1 and Entry_Date<=@Entry_Date2";
        }
        else if (srType.Value.Trim() == "Monthly")
        {

            q = "SELECT MONTH(Entry_Date) AS Month, YEAR(Entry_Date) AS Year, SUM(Paddy_Weight) AS Paddy_Weight, AVG(Avg_Rate) AS Avg_Rate, SUM(Stock_Consume) AS Stock_Consume ";
            q += " FROM   PaddyStock where Entry_Date>=@Entry_Date1 and Entry_Date<=@Entry_Date2  GROUP BY MONTH(Entry_Date), YEAR(Entry_Date)";

        }
        else
        {

        }
        dac = new DataAccessLayer();
        dt = dac.GetDataTable(q, param);

        StringBuilder htmlTable = new StringBuilder();

        htmlTable.Append("<table class='table table-bordered' id='dataTable' cellspacing='0'>");
        htmlTable.Append("<thead><tr><th>Sl. No.</th><th>");
        if (srType.Value.Trim() == "Daily")
        {
            htmlTable.Append("Date");
        }
        else if (srType.Value.Trim() == "Monthly")
        {

            htmlTable.Append("Month/Year");

        }
        else
        {

        }

        htmlTable.Append("</th><th>Paddy Weight (In KG)</th><th>Average Rate Per KG</th><th>Paddy Amount (In Rs.)</th><th>Stock Consume (In KG)</th>");
        htmlTable.Append("<th>Consume Amount (In Rs.)</th><th>Stock Balance (In KG)</th><th>Stock Balance Amount (In Rs.)</th></tr></thead><tbody>");


        if (!object.Equals(dt, null))
        {
            if (dt.Rows.Count > 0)
            {
                decimal pSBalance = 0;
                decimal LSBalance = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    htmlTable.Append("<tr>");
                    htmlTable.Append("<td>" + (i + 1) + "</td>");
                    if (srType.Value.Trim() == "Daily")
                    {
                        htmlTable.Append("<td>" + Convert.ToDateTime(dt.Rows[i]["Entry_Date"]).ToString("dd-MMM-yyyy") + "</td>");
                    }
                    else if (srType.Value.Trim() == "Monthly")
                    {

                        q = "";
                        param = new List<SqlParameter>();//Emp_Id
                        string mont = calMonth(Convert.ToInt32(dt.Rows[i]["Month"].ToString()));
                        param.Add(new SqlParameter("@Entry_Date", Convert.ToDateTime("01-" + mont + "-" + dt.Rows[i]["Year"].ToString()).AddDays(-1).ToString("dd-MMM-yyyy")));

                        q = "select (ISNULL(Paddy_Weight,0)-ISNULL(Stock_Consume,0)) from prabha.PaddyStock where Entry_Date=@Entry_Date";
                        dac = new DataAccessLayer();
                        pSBalance = Convert.ToDecimal(dac.Scalar(q, param));

                        q = "";
                        param = new List<SqlParameter>();//Emp_Id

                        param.Add(new SqlParameter("@Month", dt.Rows[i]["Month"].ToString()));
                        param.Add(new SqlParameter("@Year", dt.Rows[i]["Year"].ToString()));

                        q = "select (ISNULL(Paddy_Weight,0)-ISNULL(Stock_Consume,0)) from prabha.PaddyStock where Entry_Date=(select max(Entry_Date) from prabha.PaddyStock where Month(Entry_Date)=@Month and Year(Entry_Date)=@Year)";
                        dac = new DataAccessLayer();
                        LSBalance = Convert.ToDecimal(dac.Scalar(q, param));

                        dt.Rows[i]["Paddy_Weight"] = (Convert.ToDecimal(dt.Rows[i]["Paddy_Weight"]) - (Convert.ToDecimal(dt.Rows[i]["Paddy_Weight"]) - Convert.ToDecimal(dt.Rows[i]["Stock_Consume"]) + pSBalance - LSBalance)).ToString();
                        //(Convert.ToDecimal(dt.Rows[i]["Paddy_Weight"].ToString())-(Convert.ToDecimal(dt.Rows[i]["Paddy_Weight"].ToString()) - (Convert.ToDecimal(dt.Rows[i]["Stock_Consume"].ToString()) + pSBalance - LSBalance))).ToString();

                        htmlTable.Append("<td>" + dt.Rows[i]["Month"].ToString() + "/" + dt.Rows[i]["Year"].ToString() + "</td>");

                    }
                    else
                    {

                    }



                    htmlTable.Append("<td>" + dt.Rows[i]["Paddy_Weight"].ToString() + "</td>");
                    htmlTable.Append("<td>" + Math.Round((Convert.ToDecimal(dt.Rows[i]["Avg_Rate"].ToString())), 2) + "</td>");
                    htmlTable.Append("<td>" + Math.Round((Convert.ToDecimal(dt.Rows[i]["Paddy_Weight"].ToString()) * Convert.ToDecimal(dt.Rows[i]["Avg_Rate"].ToString())), 2) + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["Stock_Consume"].ToString() + "</td>");
                    htmlTable.Append("<td>" + Math.Round((Convert.ToDecimal(dt.Rows[i]["Stock_Consume"].ToString()) * Convert.ToDecimal(dt.Rows[i]["Avg_Rate"].ToString())), 2) + "</td>");
                    if (srType.Value.Trim() == "Daily")
                    {
                        htmlTable.Append("<td>" + (Convert.ToDecimal(dt.Rows[i]["Paddy_Weight"].ToString()) - Convert.ToDecimal(dt.Rows[i]["Stock_Consume"].ToString())) + "</td>");
                        htmlTable.Append("<td>" + Math.Round(((Convert.ToDecimal(dt.Rows[i]["Paddy_Weight"].ToString()) - Convert.ToDecimal(dt.Rows[i]["Stock_Consume"].ToString())) * Convert.ToDecimal(dt.Rows[i]["Avg_Rate"].ToString())), 2) + "</td>");
                    }
                    else if (srType.Value.Trim() == "Monthly")
                    {
                        htmlTable.Append("<td>" + LSBalance.ToString() + "</td>");
                        htmlTable.Append("<td>" + Math.Round(Convert.ToDecimal(LSBalance) * Convert.ToDecimal(dt.Rows[i]["Avg_Rate"].ToString()), 2) + "</td>");
                    }
                    else
                    {

                    }
                    htmlTable.Append("</tr>");
                }

            }
            else
            {
                htmlTable.Append("<tr>");
                htmlTable.Append("<td align='center' colspan='9'>There is no Record.</td>");
                htmlTable.Append("</tr>");
            }

            htmlTable.Append("</table>");
            DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
        }

    }
    public string calMonth(int m)
    {
        string mon = "";
        if (m == 1)
        {
            mon = "Jan";
        }
        else if (m == 2)
        {
            mon = "Feb";
        }
        else if (m == 3)
        {
            mon = "Mar";
        }
        else if (m == 4)
        {
            mon = "Apr";
        }
        else if (m == 5)
        {
            mon = "May";
        }
        else if (m == 6)
        {
            mon = "Jun";
        }
        else if (m == 7)
        {
            mon = "Jul";
        }
        else if (m == 8)
        {
            mon = "Aug";
        }
        else if (m == 9)
        {
            mon = "Sep";
        }
        else if (m == 10)
        {
            mon = "Oct";
        }
        else if (m == 11)
        {
            mon = "Nov";
        }
        else if (m == 12)
        {
            mon = "Dec";
        }
        else
        {

        }
        return mon;
    }
}