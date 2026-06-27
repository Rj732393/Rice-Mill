using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI.HtmlControls;
using substitute;

public partial class Expense : System.Web.UI.Page
{
    DataTable dt;
    List<SqlParameter> param;
    DataAccessLayer dac;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            sdate.Attributes["type"] = "date";
            EAmount.Attributes["type"] = "number";
            EAmount.Attributes["step"] = ".01";
            EAmount.Attributes["min"] = "0";
        }
    }

    // Safe parse helper
    private decimal SafeDecimal(string val)
    {
        decimal result = 0;
        decimal.TryParse(val, out result);
        return result;
    }

    // Decimal parameter helper
    private SqlParameter DecParam(string name, decimal value)
    {
        SqlParameter p = new SqlParameter(name, SqlDbType.Decimal);
        p.Precision = 18;
        p.Scale = 2;
        p.Value = value;
        return p;
    }

    // Unique alert — kabhi block nahi hogi
    private void ShowAlert(string msg, string type)
    {
        string key = "alert_" + DateTime.Now.Ticks.ToString();
        string script = "$(document).ready(function(){ showAlert('" + msg.Replace("'", "\\'") + "', '" + type + "'); });";
        ClientScript.RegisterStartupScript(this.GetType(), key, script, true);
    }

    private string ValidateEntry(string date, string amount, string remarks)
    {
        if (string.IsNullOrEmpty(date))
            return "Please select Date.";

        DateTime parsedDate;
        if (!DateTime.TryParse(date, out parsedDate))
            return "Please enter a valid Date.";

        if (string.IsNullOrEmpty(amount))
            return "Please enter Amount.";

        decimal tmp;
        if (!decimal.TryParse(amount, out tmp) || tmp <= 0)
            return "Amount must be a positive number.";

        if (string.IsNullOrEmpty(remarks))
            return "Please enter Remarks.";

        return null;
    }

    public void btnSave_ServerClick(object sender, EventArgs e)
    {
        try
        {
            string errorMsg = ValidateEntry(
                sdate.Value.Trim(),
                EAmount.Value.Trim(),
                ERemarks.Value.Trim()
            );

            if (!string.IsNullOrEmpty(errorMsg))
            {
                ShowAlert(errorMsg, "error");
                return;
            }

            string dataDate = Convert.ToDateTime(sdate.Value.Trim()).ToString("dd-MMM-yyyy");
            string entryDate = DateTime.Now.ToString("dd-MMM-yyyy");
            decimal amount = SafeDecimal(EAmount.Value.Trim());

            // Duplicate check
            param = new List<SqlParameter>();
            param.Add(new SqlParameter("@ExpenseType", ddlExpenseType.SelectedItem.Text.Trim()));
            param.Add(DecParam("@ExpenseAmount", amount));
            param.Add(new SqlParameter("@DataDate", dataDate));

            string q = "select COUNT(*) from prabha.Expense_Info where ExpenseType=@ExpenseType and ExpenseAmount=@ExpenseAmount and DataDate=@DataDate";

            dac = new DataAccessLayer();
            dt = dac.GetDataTable(q, param);

            int existCount = 0;
            if (dt != null && dt.Rows.Count > 0)
                int.TryParse(dt.Rows[0][0].ToString(), out existCount);

            if (existCount > 0)
            {
                ShowAlert("Data already exists!", "error");
                return;
            }

            // Insert
            param = new List<SqlParameter>();
            param.Add(new SqlParameter("@DataDate", dataDate));
            param.Add(new SqlParameter("@ExpenseType", ddlExpenseType.SelectedItem.Text.Trim()));
            param.Add(DecParam("@ExpenseAmount", amount));
            param.Add(new SqlParameter("@ExpenseRemarks", ERemarks.Value.Trim()));
            param.Add(new SqlParameter("@OperatorName", Session["User"].ToString()));
            param.Add(new SqlParameter("@EntryDate", entryDate));

            q = "insert into prabha.Expense_Info(DataDate,ExpenseType,ExpenseAmount,ExpenseRemarks,OperatorName,";
            q += " EntryDate)";
            q += " values(@DataDate,@ExpenseType,@ExpenseAmount,@ExpenseRemarks,@OperatorName,";
            q += " @EntryDate)";

            dac = new DataAccessLayer();
            int c = dac.update(q, param);

            if (c > 0)
                ShowAlert("Data successfully saved!", "success");
            else
                ShowAlert("Data not saved. Please try again.", "error");
        }
        catch (Exception ex)
        {
            ShowAlert("Data not saved. Reason: " + ex.Message, "error");
        }
    }

    protected void lbrnData_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(sdate.Value.Trim()))
            {
                ShowAlert("Please select Date to view report.", "error");
                return;
            }

            string dataDate = Convert.ToDateTime(sdate.Value.Trim()).ToString("dd-MMM-yyyy");

            param = new List<SqlParameter>();
            param.Add(new SqlParameter("@DataDate", dataDate));

            string q = "select * from prabha.Expense_Info where DataDate=@DataDate order by DataDate desc";

            dac = new DataAccessLayer();
            dt = dac.GetDataTable(q, param);

            StringBuilder htmlTable = new StringBuilder();

            if (dt == null || dt.Rows.Count == 0)
            {
                htmlTable.Append("<p style='color:#64748b;text-align:center;padding:20px;'>No data found for selected date.</p>");
            }
            else
            {
                htmlTable.Append("<table class='table table-bordered' id='dataTable' cellspacing='0'>");
                htmlTable.Append("<thead><tr>");
                htmlTable.Append("<th>Sl. No.</th>");
                htmlTable.Append("<th>Date</th>");
                htmlTable.Append("<th>Expense Type</th>");
                htmlTable.Append("<th>Amount (In Rs.)</th>");
                htmlTable.Append("<th>Remarks (If Any)</th>");
                htmlTable.Append("</tr></thead><tbody>");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    htmlTable.Append("<tr>");
                    htmlTable.Append("<td>" + (i + 1) + "</td>");
                    htmlTable.Append("<td>" + Convert.ToDateTime(dt.Rows[i]["DataDate"].ToString()).ToString("dd/MM/yyyy") + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["ExpenseType"].ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["ExpenseAmount"].ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["ExpenseRemarks"].ToString() + "</td>");
                    htmlTable.Append("</tr>");
                }

                htmlTable.Append("</tbody></table>");
            }

            DBDataPlaceHolder.Controls.Clear();
            DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
        }
        catch (Exception ex)
        {
            ShowAlert("Error loading report: " + ex.Message, "error");
        }
    }
}