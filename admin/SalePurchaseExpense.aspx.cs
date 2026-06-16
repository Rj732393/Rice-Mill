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

public partial class admin_SalePurchaseExpense : System.Web.UI.Page
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
<<<<<<< HEAD
        dt = new DataTable();
        string q = "";
        param = new List<SqlParameter>();//Emp_Id
        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
=======
        lblFromDateError.Text = "Please Select From Date";
        return;
    }
>>>>>>> 650d09a42ed342afcc2fc1650ba0387ab93384da

    if (string.IsNullOrWhiteSpace(tdate.Value))
    {
        lblToDateError.Text = "Please Select To Date";
        return;
    }

<<<<<<< HEAD
        q = "select * from prabha.SalePurchaseExpense where CompanyID=@CompanyID and Entry_Date>=@Entry_Date1 and Entry_Date<=@Entry_Date2";
=======
    dt = new DataTable();
    string q = "";
    param = new List<SqlParameter>();

    param.Add(new SqlParameter("@Entry_Date1",
        Convert.ToDateTime(fdate.Value.Trim()).ToString("dd-MMM-yyyy")));

    param.Add(new SqlParameter("@Entry_Date2",
        Convert.ToDateTime(tdate.Value.Trim()).ToString("dd-MMM-yyyy")));
        q = "select * from prabha.SalePurchaseExpense where Entry_Date>=@Entry_Date1 and Entry_Date<=@Entry_Date2";
>>>>>>> 650d09a42ed342afcc2fc1650ba0387ab93384da
        dac = new DataAccessLayer();
        dt = dac.GetDataTable(q, param);

        StringBuilder htmlTable = new StringBuilder();

        htmlTable.Append("<table class='table table-bordered' id='dataTable' cellspacing='0'>");
        htmlTable.Append("<thead><tr><th>Sl. No.</th><th>Date</th><th>Rice Amount (In Rs.)</th><th>Broken Amount (In Rs.)</th><th>Bran Amount (In Rs.)</th><th>Nakku Amount (In Rs.)</th>");
        htmlTable.Append("<th>Nakku Bhusi Amount (In Rs.)</th><th>Rejection Amount (In Rs.)</th><th>Husk Amount (In Rs.)</th>");
        htmlTable.Append("<th>Jute Bags Amount (In Rs.)</th><th>PP Bags Amount (In Rs.)</th><th>Grand Total (In Rs.)</th>");
        htmlTable.Append("<th>Paddy Amount (In Rs.)</th><th>Paddy Weight (In KG)</th><th>Expense (In Rs.)</th>");
        htmlTable.Append("<th>Paddy Average Rate Per KG</th><th>Purchase + Expense (In Rs.)</th><th>Net Profit (In Rs.)</th></tr></thead><tbody>");


        if (!object.Equals(dt, null))
        {
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    htmlTable.Append("<tr>");
                    htmlTable.Append("<td>" + (i + 1) + "</td>");
                    htmlTable.Append("<td>" + Convert.ToDateTime(dt.Rows[i]["Entry_Date"]).ToString("dd-MMM-yyyy") + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["Rice_Amount"].ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["Broken_Amount"].ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["Bran_Amount"].ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["Nakku_Amount"].ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["Nakku_Bhusi_Amount"].ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["Rejection_Amount"].ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["Husk_Amount"].ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["Jute_Bags_Amount"].ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["PP_Bags_Amount"].ToString() + "</td>");

                    decimal GT = Convert.ToDecimal(dt.Rows[i]["Rice_Amount"].ToString()) + Convert.ToDecimal(dt.Rows[i]["Broken_Amount"].ToString()) + Convert.ToDecimal(dt.Rows[i]["Bran_Amount"].ToString());
                    GT += Convert.ToDecimal(dt.Rows[i]["Nakku_Amount"].ToString()) + Convert.ToDecimal(dt.Rows[i]["Nakku_Bhusi_Amount"].ToString()) + Convert.ToDecimal(dt.Rows[i]["Rejection_Amount"].ToString());
                    GT += Convert.ToDecimal(dt.Rows[i]["Husk_Amount"].ToString()) + Convert.ToDecimal(dt.Rows[i]["Jute_Bags_Amount"].ToString()) + Convert.ToDecimal(dt.Rows[i]["PP_Bags_Amount"].ToString());

                    htmlTable.Append("<td>" + GT.ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["Paddy_Amount"].ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["Paddy_Weight"].ToString() + "</td>");
                    if (Convert.ToDecimal(dt.Rows[i]["Paddy_Amount"].ToString()) == 0)
                    {
                        htmlTable.Append("<td>0.00</td>");
                    }
                    else
                    {
                        htmlTable.Append("<td>" + Math.Round((Convert.ToDecimal(dt.Rows[i]["Paddy_Weight"].ToString()) / Convert.ToDecimal(dt.Rows[i]["Paddy_Amount"].ToString())), 2) + "</td>");
                    }
                    htmlTable.Append("<td>" + dt.Rows[i]["Expense_Amount"].ToString() + "</td>");

                    decimal PE = Convert.ToDecimal(dt.Rows[i]["Paddy_Amount"].ToString()) + Convert.ToDecimal(dt.Rows[i]["Expense_Amount"].ToString());

                    htmlTable.Append("<td>" + PE.ToString() + "</td>");

                    decimal NP = GT - PE;

                    htmlTable.Append("<td>" + NP.ToString() + "</td>");
                    htmlTable.Append("</tr>");
                }

            }
            else
            {
                htmlTable.Append("<tr>");
                htmlTable.Append("<td align='center' colspan='18'>There is no Record.</td>");
                htmlTable.Append("</tr>");
            }

            htmlTable.Append("</table>");
            DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
        }

    }
}