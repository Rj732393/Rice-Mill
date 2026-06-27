using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI.HtmlControls;
using substitute;

public partial class PaddyProcessing : System.Web.UI.Page
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

            PaddyWt.Attributes["type"] = "number";
            PaddyWt.Attributes["step"] = ".001";
            PaddyWt.Attributes["min"] = "0";

            RiceWt.Attributes["type"] = "number";
            RiceWt.Attributes["step"] = ".001";
            RiceWt.Attributes["min"] = "0";

            BrokenWt.Attributes["type"] = "number";
            BrokenWt.Attributes["step"] = ".001";
            BrokenWt.Attributes["min"] = "0";

            BranWt.Attributes["type"] = "number";
            BranWt.Attributes["step"] = ".001";
            BranWt.Attributes["min"] = "0";

            NakkuWt.Attributes["type"] = "number";
            NakkuWt.Attributes["step"] = ".001";
            NakkuWt.Attributes["min"] = "0";

            NakkuBhusi.Attributes["type"] = "number";
            NakkuBhusi.Attributes["step"] = ".001";
            NakkuBhusi.Attributes["min"] = "0";

            RejectionWt.Attributes["type"] = "number";
            RejectionWt.Attributes["step"] = ".001";
            RejectionWt.Attributes["min"] = "0";

            HuskWt.Attributes["type"] = "number";
            HuskWt.Attributes["step"] = ".001";
            HuskWt.Attributes["min"] = "0";
        }
    }

    // Safe parse helper — empty ya invalid ho to 0 return karo
    private decimal SafeDecimal(string val)
    {
        decimal result = 0;
        decimal.TryParse(val, out result);
        return result;
    }

    // Decimal parameter banane ka helper
    private SqlParameter DecParam(string name, decimal value)
    {
        SqlParameter p = new SqlParameter(name, SqlDbType.Decimal);
        p.Precision = 18;
        p.Scale = 3;
        p.Value = value;
        return p;
    }

    // Unique key se alert — kabhi block nahi hogi
    private void ShowAlert(string msg, string type)
    {
        string key = "alert_" + DateTime.Now.Ticks.ToString();
        string script = "$(document).ready(function(){ showAlert('" + msg.Replace("'", "\\'") + "', '" + type + "'); });";
        ClientScript.RegisterStartupScript(this.GetType(), key, script, true);
    }

    protected void PaddyWt_TextChanged(object sender, EventArgs e)
    {
        decimal paddy = SafeDecimal(PaddyWt.Text.Trim());

        if (paddy > 0)
        {
            RiceWt.Value = Math.Round(paddy * 48 / 100, 2).ToString();
            BrokenWt.Value = Math.Round(paddy * 17 / 48, 2).ToString();
            BranWt.Value = Math.Round(paddy * 9 / 48, 2).ToString();
            NakkuWt.Value = Math.Round(paddy * 2 / 48, 2).ToString();
            NakkuBhusi.Value = Math.Round(paddy * 2 / 48, 2).ToString();
            RejectionWt.Value = Math.Round(paddy * 2 / 48, 2).ToString();
            HuskWt.Value = Math.Round(paddy * 9 / 48, 2).ToString();
        }
    }

    private string ValidateEntry(string date, string paddyWt)
    {
        if (string.IsNullOrEmpty(date))
            return "Please select Date.";

        DateTime parsedDate;
        if (!DateTime.TryParse(date, out parsedDate))
            return "Please enter a valid Date.";

        if (string.IsNullOrEmpty(paddyWt))
            return "Please enter Paddy (KG).";

        decimal tmp;
        if (!decimal.TryParse(paddyWt, out tmp) || tmp <= 0)
            return "Paddy (KG) must be a positive number.";

        return null;
    }

    public void btnSave_ServerClick(object sender, EventArgs e)
    {
        try
        {
            string errorMsg = ValidateEntry(
                sdate.Value.Trim(),
                PaddyWt.Text.Trim()
            );

            if (!string.IsNullOrEmpty(errorMsg))
            {
                ShowAlert(errorMsg, "error");
                return;
            }

            string dataDate = Convert.ToDateTime(sdate.Value.Trim()).ToString("dd-MMM-yyyy");
            string entryDate = DateTime.Now.ToString("dd-MMM-yyyy");
            decimal paddyWt = SafeDecimal(PaddyWt.Text.Trim());

            // Calculated values
            decimal riceWt = Math.Round(paddyWt * 48 / 100, 2);
            decimal brokenWt = Math.Round(paddyWt * 17 / 48, 2);
            decimal branWt = Math.Round(paddyWt * 9 / 48, 2);
            decimal nakkuWt = Math.Round(paddyWt * 2 / 48, 2);
            decimal nakkuBhusi = Math.Round(paddyWt * 2 / 48, 2);
            decimal rejectionWt = Math.Round(paddyWt * 2 / 48, 2);
            decimal huskWt = Math.Round(paddyWt * 9 / 48, 2);

            // DB column size check — agar koi value 99999 se badi ho
            if (paddyWt > 99999 || riceWt > 99999 || brokenWt > 99999 ||
                branWt > 99999 || nakkuWt > 99999 || nakkuBhusi > 99999 ||
                rejectionWt > 99999 || huskWt > 99999)
            {
                ShowAlert("Data not saved. Entered Paddy (KG) value is too large. Calculated values exceed database limit. Please enter a smaller value.", "error");
                return;
            }

            // Duplicate check
            param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PaddyType", sPaddyType.Value.Trim()));
            param.Add(new SqlParameter("@RiceType", sRiceType.Value.Trim()));
            param.Add(DecParam("@PaddyWt", paddyWt));
            param.Add(new SqlParameter("@DataDate", dataDate));

            string q = "select COUNT(*) from prabha.PaddyProcessing where PaddyType=@PaddyType and RiceType=@RiceType and PaddyWt=@PaddyWt and DataDate=@DataDate";

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
            param.Add(new SqlParameter("@PaddyType", sPaddyType.Value.Trim()));
            param.Add(DecParam("@PaddyWt", paddyWt));
            param.Add(new SqlParameter("@RiceType", sRiceType.Value.Trim()));
            param.Add(DecParam("@RiceWt", riceWt));
            param.Add(DecParam("@BrokenWt", brokenWt));
            param.Add(DecParam("@BranWt", branWt));
            param.Add(DecParam("@NakkuWt", nakkuWt));
            param.Add(DecParam("@NakkuBhusiWt", nakkuBhusi));
            param.Add(DecParam("@RejectionWt", rejectionWt));
            param.Add(DecParam("@HuskWt", huskWt));
            param.Add(new SqlParameter("@UserName", Session["User"].ToString()));
            param.Add(new SqlParameter("@EntryDate", entryDate));

            q = "insert into prabha.PaddyProcessing(DataDate,PaddyType,PaddyWt,RiceType,RiceWt,";
            q += " BrokenWt,BranWt,NakkuWt,NakkuBhusiWt,RejectionWt,HuskWt,UserName,EntryDate)";
            q += " values(@DataDate,@PaddyType,@PaddyWt,@RiceType,@RiceWt,";
            q += " @BrokenWt,@BranWt,@NakkuWt,@NakkuBhusiWt,@RejectionWt,@HuskWt,@UserName,@EntryDate)";

            dac = new DataAccessLayer();
            int c = dac.update(q, param);

            if (c > 0)
            {
                ShowAlert("Data successfully saved!", "success");
            }
            else
            {
                ShowAlert("Data not saved. Please try again.", "error");
            }
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

            string q = "select * from prabha.PaddyProcessing where DataDate=@DataDate order by DataDate desc";

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
                htmlTable.Append("<th>Paddy Type</th>");
                htmlTable.Append("<th>Rice Type</th>");
                htmlTable.Append("<th>Paddy (In KG)</th>");
                htmlTable.Append("<th>Rice (In KG)</th>");
                htmlTable.Append("<th>Broken (In KG)</th>");
                htmlTable.Append("<th>Bran Amount (In KG)</th>");
                htmlTable.Append("<th>Nakku (In KG)</th>");
                htmlTable.Append("<th>Nakku Bhusi (In KG)</th>");
                htmlTable.Append("<th>Rejection (In KG)</th>");
                htmlTable.Append("<th>Husk (In KG)</th>");
                htmlTable.Append("</tr></thead><tbody>");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    htmlTable.Append("<tr>");
                    htmlTable.Append("<td>" + (i + 1) + "</td>");
                    htmlTable.Append("<td>" + Convert.ToDateTime(dt.Rows[i]["DataDate"].ToString()).ToString("dd/MM/yyyy") + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["PaddyType"].ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["RiceType"].ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["PaddyWt"].ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["RiceWt"].ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["BrokenWt"].ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["BranWt"].ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["NakkuWt"].ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["NakkuBhusiWt"].ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["RejectionWt"].ToString() + "</td>");
                    htmlTable.Append("<td>" + dt.Rows[i]["HuskWt"].ToString() + "</td>");
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