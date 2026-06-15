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

public partial class PaddyProcessing : System.Web.UI.Page
{
    DataTable dt;
    List<SqlParameter> param;
    DataAccessLayer dac;
    SaaSHelper saas = new SaaSHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["User"] == null || Session["CompanyID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            // Subscription check (in case of long-running session)
            int cid = Convert.ToInt32(Session["CompanyID"]);
            if (cid > 0)
            {
                string status = saas.GetSubscriptionStatus(cid);
                if (status == "Suspended" || status == "Blocked" || status == "Expired")
                {
                    Session.Clear();
                    Response.Redirect("Login.aspx?expired=1");
                    return;
                }
            }

            sdate.Attributes["type"] = "date";

            PaddyWt.Attributes["type"] = "number";
            PaddyWt.Attributes["step"] = ".001";

            RiceWt.Attributes["type"] = "number";
            RiceWt.Attributes["step"] = ".001";

            BrokenWt.Attributes["type"] = "number";
            BrokenWt.Attributes["step"] = ".001";

            BranWt.Attributes["type"] = "number";
            BranWt.Attributes["step"] = ".001";

            NakkuWt.Attributes["type"] = "number";
            NakkuWt.Attributes["step"] = ".001";

            NakkuBhusi.Attributes["type"] = "number";
            NakkuBhusi.Attributes["step"] = ".001";

            RejectionWt.Attributes["type"] = "number";
            RejectionWt.Attributes["step"] = ".001";

            HuskWt.Attributes["type"] = "number";
            HuskWt.Attributes["step"] = ".001";

            
            
        }
    }
    protected void PaddyWt_TextChanged(object sender, EventArgs e)
    {
        if (PaddyWt.Text.Trim() == "")
        {

        }
        else
        {
            RiceWt.Value = Math.Round(Convert.ToDecimal(PaddyWt.Text.Trim()) * 48 / 100, 2).ToString();
            BrokenWt.Value = Math.Round(Convert.ToDecimal(PaddyWt.Text.Trim()) * 17 / 48, 2).ToString();
            BranWt.Value = Math.Round(Convert.ToDecimal(PaddyWt.Text.Trim()) * 9 / 48, 2).ToString();
            NakkuWt.Value = Math.Round(Convert.ToDecimal(PaddyWt.Text.Trim()) * 2 / 48, 2).ToString();
            NakkuBhusi.Value = Math.Round(Convert.ToDecimal(PaddyWt.Text.Trim()) * 2 / 48, 2).ToString();
            RejectionWt.Value = Math.Round(Convert.ToDecimal(PaddyWt.Text.Trim()) * 2 / 48, 2).ToString();
            HuskWt.Value = Math.Round(Convert.ToDecimal(PaddyWt.Text.Trim()) * 9 / 48, 2).ToString();
        }
    }


    public void btnSave_ServerClick(object sender, EventArgs e)
    {
        string script = "";

        if (PaddyWt.Text.Trim() == "" || RiceWt.Value.Trim() == "" || BrokenWt.Value.Trim() == "" ||
            BranWt.Value.Trim() == "" || NakkuWt.Value.Trim() == "" || NakkuBhusi.Value.Trim() == "" ||
            RejectionWt.Value.Trim() == "" || HuskWt.Value.Trim() == "")
        {
            
            
            script = "alert('Please fill all data!!');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
            

        }
        else
        {
            dt = new DataTable();
            string q = "";
            param = new List<SqlParameter>();//Emp_Id

            int companyID = Convert.ToInt32(Session["CompanyID"]);

            param.Add(new SqlParameter("@CompanyID", companyID));
            param.Add(new SqlParameter("@PaddyType", sPaddyType.Value.Trim()));
            param.Add(new SqlParameter("@RiceType", sRiceType.Value.Trim()));
            param.Add(new SqlParameter("@PaddyWt", PaddyWt.Text.Trim()));
            param.Add(new SqlParameter("@DataDate", Convert.ToDateTime(sdate.Value.Trim()).ToString("dd-MMM-yyyy")));

            q = "select * from prabha.PaddyProcessing where CompanyID=@CompanyID and PaddyType=@PaddyType and RiceType=@RiceType and PaddyWt=@PaddyWt and DataDate=@DataDate";
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


                param.Add(new SqlParameter("@CompanyID", companyID));
                param.Add(new SqlParameter("@DataDate", Convert.ToDateTime(sdate.Value.Trim()).ToString("dd-MMM-yyyy")));
                param.Add(new SqlParameter("@PaddyType", sPaddyType.Value.Trim()));
                param.Add(new SqlParameter("@PaddyWt", PaddyWt.Text.Trim()));
                param.Add(new SqlParameter("@RiceType", sRiceType.Value.Trim()));
                param.Add(new SqlParameter("@RiceWt", RiceWt.Value.Trim()));
                param.Add(new SqlParameter("@BrokenWt", BrokenWt.Value.Trim()));
                param.Add(new SqlParameter("@BranWt", BranWt.Value.Trim()));
                param.Add(new SqlParameter("@NakkuWt", NakkuWt.Value.Trim()));
                param.Add(new SqlParameter("@NakkuBhusiWt", NakkuBhusi.Value.Trim()));
                param.Add(new SqlParameter("@RejectionWt", RejectionWt.Value.Trim()));
                param.Add(new SqlParameter("@HuskWt", HuskWt.Value.Trim()));
                param.Add(new SqlParameter("@UserName", Session["User"].ToString()));
                param.Add(new SqlParameter("@EntryDate", Convert.ToDateTime(System.DateTime.Now.ToString()).ToString("dd-MMM-yyyy")));

                q = "insert into prabha.PaddyProcessing(CompanyID,DataDate,PaddyType,PaddyWt,RiceType,RiceWt,";
                q += " BrokenWt,BranWt,NakkuWt,NakkuBhusiWt,RejectionWt,HuskWt,UserName,EntryDate)";
                q += " values(@CompanyID,@DataDate,@PaddyType,@PaddyWt,@RiceType,@RiceWt,";
                q += " @BrokenWt,@BranWt,@NakkuWt,@NakkuBhusiWt,@RejectionWt,@HuskWt,@UserName,@EntryDate)";
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

        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
        param.Add(new SqlParameter("@DataDate", Convert.ToDateTime(sdate.Value.Trim()).ToString("dd-MMM-yyyy")));

        q = "select * from prabha.PaddyProcessing where CompanyID=@CompanyID and DataDate=@DataDate order by DataDate desc";

        dac = new DataAccessLayer();
        dt = dac.GetDataTable(q, param);

        StringBuilder htmlTable = new StringBuilder();
        
        htmlTable.Append("<table class='table table-bordered' id='dataTable' cellspacing='0'>");
        htmlTable.Append("<thead><tr><th>Sl. No.</th><th>Date</th><th>Paddy Type</th><th>Rice Type</th><th>Paddy (In KG)</th><th>Rice (In KG)</th>");
        htmlTable.Append("<th>Broken (In KG)</th><th>Bran Amount (In KG)</th><th>Nakku (In KG)</th><th>Nakku Bhusi (In KG)</th><th>Rejection (In KG)</th><th>Husk (In KG)</th></tr></thead><tbody>");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            htmlTable.Append("<tr>");
            htmlTable.Append("<td>" + (i + 1) + "</td>");
            htmlTable.Append("<td>" +Convert.ToDateTime(dt.Rows[i]["DataDate"].ToString()).ToString("dd/MM/yyyy") + "</td>");
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
        DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
    }
}