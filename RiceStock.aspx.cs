using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using substitute;

public partial class RiceStock : System.Web.UI.Page
{
    DataTable dt;
    List<SqlParameter> param;
    DataAccessLayer dac;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["User"] == null || Session["CompanyID"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            sdate.Attributes["type"] = "date";
            //sdate.Value = System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString();

            rweight.Attributes["type"] = "number";
            rweight.Attributes["step"] = ".001";

            avgrate.Attributes["type"] = "number";
            avgrate.Attributes["step"] = ".01";

            sconsume.Attributes["type"] = "number";
            sconsume.Attributes["step"] = ".001";

            ramount.Attributes["type"] = "number";
            ramount.Attributes["step"] = ".01";

            camount.Attributes["type"] = "number";
            camount.Attributes["step"] = ".01";

            sbalance.Attributes["type"] = "number";
            sbalance.Attributes["step"] = ".001";

            sbamount.Attributes["type"] = "number";
            sbamount.Attributes["step"] = ".01";

            Panel1.Visible = false;

            lblOSB.Text = "0";

        }
    }
    public void SCalculate_ServerClick(object sender, EventArgs e)
    {
        calc();
    }
    public void btnSave_ServerClick(object sender, EventArgs e)
    {

        string script = "";
        if (ramount.Value.Trim() == "" || camount.Value.Trim() == "" || sbalance.Value.Trim() == "" || sbamount.Value.Trim() == "")
        {
            calc();

            script = "alert('Please verify all data!!');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
        }
        else
        {
            DataTable dt1 = checkData();


            if (dt1.Rows.Count > 0)
            {
                script = "alert('Data already exist!!');";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
            }
            else
            {
                string q = "";
                param = new List<SqlParameter>();//Emp_Id

                param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
                param.Add(new SqlParameter("@Rice_Weight", (Convert.ToDecimal(rweight.Value.Trim()) + Convert.ToDecimal(lblOSB.Text.Trim()))));
                param.Add(new SqlParameter("@Avg_Rate", avgrate.Value.Trim()));
                param.Add(new SqlParameter("@Stock_Consume", sconsume.Value.Trim()));
                param.Add(new SqlParameter("@User_Name", Session["User"].ToString()));
                param.Add(new SqlParameter("@Entry_Date", Convert.ToDateTime(sdate.Value.Trim()).ToString("dd-MMM-yyyy")));

                q = "insert into prabha.RiceStock(CompanyID,Rice_Weight,Avg_Rate,Stock_Consume,User_Name,Entry_Date) values(@CompanyID,@Rice_Weight,@Avg_Rate,@Stock_Consume,@User_Name,@Entry_Date)";
                dac = new DataAccessLayer();

                int c = dac.update(q, param);

                if (c > 0)
                {
                    resetField();
                    Panel1.Visible = false;
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

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        int m = chkDate();
        if (m == 1)
        {
            string script = "alert('Invalid Date');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
        }
        else
        {
            DataTable dtData = new DataTable();
            string q = "";
            param = new List<SqlParameter>();//Emp_Id

            param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
            param.Add(new SqlParameter("@Entry_Date", Convert.ToDateTime(sdate.Value.Trim()).AddDays(-1).ToString("dd-MMM-yyyy")));

            q = "select * from prabha.RiceStock where CompanyID=@CompanyID and Entry_Date=@Entry_Date";
            dac = new DataAccessLayer();
            dtData = dac.GetDataTable(q, param);

            if (dtData.Rows.Count > 0)
            {
                lblOSB.Text = (Convert.ToDecimal(dtData.Rows[0]["Rice_Weight"].ToString()) - Convert.ToDecimal(dtData.Rows[0]["Stock_Consume"].ToString())).ToString();
            }
            else
            {
                lblOSB.Text = "0";
            }

            Panel1.Visible = true;

            DataTable dt1 = checkData();
            if (dt1.Rows.Count > 0)
            {
                rweight.Value = (Convert.ToDecimal(dt1.Rows[0]["Rice_Weight"].ToString()) - Convert.ToDecimal(lblOSB.Text.Trim())).ToString();
                avgrate.Value = dt1.Rows[0]["Avg_Rate"].ToString();
                sconsume.Value = dt1.Rows[0]["Stock_Consume"].ToString();
                calc();
                btnSave.Visible = false;
            }
            else
            {
                resetField();
                if (dtData.Rows.Count > 0)
                {
                    lblOSB.Text = (Convert.ToDecimal(dtData.Rows[0]["Rice_Weight"].ToString()) - Convert.ToDecimal(dtData.Rows[0]["Stock_Consume"].ToString())).ToString();
                }
                else
                {
                    lblOSB.Text = "0";
                }
                btnSave.Visible = true;
            }
        }
    }
    public void resetField()
    {
        rweight.Value = "";
        avgrate.Value = "";
        sconsume.Value = "";
        ramount.Value = "";
        camount.Value = "";
        sbalance.Value = "";
        sbamount.Value = "";
        lblOSB.Text = "0";
    }
    public int chkDate()
    {
        int i = 0;
        try
        {
            string dat = Convert.ToDateTime(sdate.Value.Trim()).ToString("dd-MMM-yyyy");
        }
        catch
        {
            i = i + 1;
        }
        finally
        {


        }
        return i;
    }
    public void calc()
    {
        decimal ra = 0;
        decimal ca = 0;
        decimal sb = 0;
        decimal sba = 0;
        if (rweight.Value.Trim() == "")
        {
            rweight.Value = "0";
        }
        if (avgrate.Value.Trim() == "")
        {
            avgrate.Value = "0";
        }
        if (sconsume.Value.Trim() == "")
        {
            sconsume.Value = "0";
        }

        ra = Math.Round(Convert.ToDecimal(rweight.Value.Trim()) * Convert.ToDecimal(avgrate.Value.Trim()), 2);
        ramount.Value = ra.ToString();

        ca = Math.Round(Convert.ToDecimal(sconsume.Value.Trim()) * Convert.ToDecimal(avgrate.Value.Trim()), 2);
        camount.Value = ca.ToString();

        sb = Math.Round(Convert.ToDecimal(rweight.Value.Trim()) + Convert.ToDecimal(lblOSB.Text.Trim()) - Convert.ToDecimal(sconsume.Value.Trim()), 3);
        sbalance.Value = sb.ToString();

        sba = Math.Round(sb * Convert.ToDecimal(avgrate.Value.Trim()), 2);
        sbamount.Value = sba.ToString();
    }
    public DataTable checkData()
    {
        dt = new DataTable();
        string q = "";
        param = new List<SqlParameter>();//Emp_Id

        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
        param.Add(new SqlParameter("@Entry_Date", Convert.ToDateTime(sdate.Value.Trim()).ToString("dd-MMM-yyyy")));

        q = "select * from prabha.RiceStock where CompanyID=@CompanyID and Entry_Date=@Entry_Date";
        dac = new DataAccessLayer();
        dt = dac.GetDataTable(q, param);
        return dt;
    }
}