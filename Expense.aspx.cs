using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
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
            }

            sdate.Attributes["type"] = "date";

            EAmount.Attributes["type"] = "number";
            EAmount.Attributes["step"] = ".01";


        }
    }

    public void btnSave_ServerClick(object sender, EventArgs e)
    {
        string script = "";

        if (EAmount.Value.Trim() == "" || ERemarks.Value.Trim() == "")
        {


            script = "alert('Please fill all data!!');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);


        }
        else
        {
            dt = new DataTable();
            string q = "";
            param = new List<SqlParameter>();//Emp_Id

            param.Add(new SqlParameter("@ExpenseType", ddlExpenseType.SelectedItem.Text.Trim()));
            param.Add(new SqlParameter("@ExpenseAmount", EAmount.Value.Trim()));

            param.Add(new SqlParameter("@DataDate", Convert.ToDateTime(sdate.Value.Trim()).ToString("dd-MMM-yyyy")));

            q = "select * from prabha.Expense_Info where ExpenseType=@ExpenseType and ExpenseAmount=@ExpenseAmount and DataDate=@DataDate";
            dac = new DataAccessLayer();
            dt = dac.GetDataTable(q, param);

            if (dt.Rows.Count > 0)
            {
                script = "alert('Data already exist!!');";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
            }
            else
            {
                q = "";
                param = new List<SqlParameter>();//Emp_Id


                param.Add(new SqlParameter("@DataDate", Convert.ToDateTime(sdate.Value.Trim()).ToString("dd-MMM-yyyy")));
                param.Add(new SqlParameter("@ExpenseType", ddlExpenseType.SelectedItem.Text.Trim()));
                param.Add(new SqlParameter("@ExpenseAmount", EAmount.Value.Trim()));
                param.Add(new SqlParameter("@ExpenseRemarks", ERemarks.Value.Trim()));

                param.Add(new SqlParameter("@OperatorName", Session["User"].ToString()));
                param.Add(new SqlParameter("@EntryDate", Convert.ToDateTime(System.DateTime.Now.ToString()).ToString("dd-MMM-yyyy")));

                q = "insert into prabha.Expense_Info(DataDate,ExpenseType,ExpenseAmount,ExpenseRemarks,OperatorName,";
                q += " EntryDate)";
                q += " values(@DataDate,@ExpenseType,@ExpenseAmount,@ExpenseRemarks,@OperatorName,";
                q += " @EntryDate)";
                dac = new DataAccessLayer();

                int c = dac.update(q, param);

                if (c > 0)
                {
                    script = "alert('Data successfully saved');";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
                }
                else
                {
                    script = "alert('Error');";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
                }

            }
        }
    }
    protected void lbrnData_Click(object sender, EventArgs e)
    {
        dt = new DataTable();
        string q = "";
        param = new List<SqlParameter>();//Emp_Id

        param.Add(new SqlParameter("@DataDate", Convert.ToDateTime(sdate.Value.Trim()).ToString("dd-MMM-yyyy")));

        q = "select * from prabha.Expense_Info where DataDate=@DataDate order by DataDate desc";

        dac = new DataAccessLayer();
        dt = dac.GetDataTable(q, param);

        StringBuilder htmlTable = new StringBuilder();

        htmlTable.Append("<table class='table table-bordered' id='dataTable' cellspacing='0'>");
        htmlTable.Append("<thead><tr><th>Sl. No.</th><th>Date</th><th>Expense Type</th><th>Amount (In Rs.)</th><th>Remarks (If Any)</th></tr></thead><tbody>");
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
        DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
    }
}