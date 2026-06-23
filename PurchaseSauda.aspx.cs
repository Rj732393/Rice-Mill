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

public partial class PurchaseSauda : System.Web.UI.Page
{
    DataTable dt;
    DataTable dtMain;
    DataRow dtRow;
    DataRow rmain;
    List<SqlParameter> param;
    DataAccessLayer dac;
    string script = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            sdate.Attributes["type"] = "date";
            //sdate.Value = System.DateTime.Now.Day.ToString() + "-" + System.DateTime.Now.Month.ToString() + "-" + System.DateTime.Now.Year.ToString();


            pMN.Attributes["type"] = "number";
            pMN.Attributes["step"] = "1";

            QIKG.Attributes["type"] = "number";
            QIKG.Attributes["step"] = ".001";

            avgrate.Attributes["type"] = "number";
            avgrate.Attributes["step"] = ".01";

            Party();

            if (sPartyName.SelectedItem.Text.Trim() == "Other")
            {
                Panel1.Visible = true;
            }
            else
            {
                Panel1.Visible = false;
            }
            Session["Data"] = null;
            Session["DataMain"] = null;

            dataDisplay();
        }
    }
    public void Submit1_ServerClick(object sender, EventArgs e)
    {
        Session["Data"] = null;
        Session["DataMain"] = null;
        dataDisplay();
    }
    public void btnSave_ServerClick(object sender, EventArgs e)
    {
        if (Session["Data"] == null)
        {
            script = "alert('Please add atleast one data!!');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
        }
        else if (Session["User"] == null)
        {
            script = "alert('Your Sessioin has expired!!');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
        }
        else
        {
            insertPurchaseData();
            dataDisplay();
            CallPrint("prntContent");
        }

    }
    public void btnContinue_ServerClick(object sender, EventArgs e)
    {
        int chk = PartyValidation();
        if (chk == 1)
        {
            dtMain = new DataTable();
            dtMain.Columns.Add("No", typeof(string));
            dtMain.Columns.Add("DataDate", typeof(string));
            dtMain.Columns.Add("MNo", typeof(string));
            dtMain.Columns.Add("PartyName", typeof(string));
            dtMain.Columns.Add("BrokerName", typeof(string));

            rmain = dtMain.NewRow();
            rmain[0] = GenInvoiceNo();
            rmain[1] = Convert.ToDateTime(sdate.Value.Trim()).ToString("dd-MMM-yyyy");
            rmain[2] = MPNo.Value.Trim();
            if (sPartyName.SelectedItem.Text.Trim() == "Other")
            {
                rmain[3] = pName.Value.Trim() + " (Mobile No.: " + pMN.Value.Trim() + ")";
            }
            else
            {
                rmain[3] = sPartyName.SelectedItem.Text.Trim();
            }
            rmain[4] = txtEmpName.Text.Trim();

            dtMain.Rows.Add(rmain);
            Session["DataMain"] = null;
            Session["DataMain"] = dtMain;

            if (Session["Data"] == null)
            {
                DataTable dtData = new DataTable();
                dtData.Columns.Add("PaddyType", typeof(string));
                dtData.Columns.Add("QIKG", typeof(string));
                dtData.Columns.Add("Rate", typeof(string));

                dtRow = dtData.NewRow();
                dtRow[0] = sPaddyType.Value.Trim();
                dtRow[1] = QIKG.Value.Trim();
                dtRow[2] = avgrate.Value.Trim();
                dtData.Rows.Add(dtRow);
                Session["Data"] = null;
                Session["Data"] = dtData;
            }
            else
            {
                DataTable dtData = (DataTable)Session["Data"];

                for (int i = dtData.Rows.Count - 1; i >= 0; i--)
                {
                    DataRow dr = dtData.Rows[i];
                    if (dr["PaddyType"] == sPaddyType.Value.Trim())
                        dr.Delete();
                }
                dtData.AcceptChanges();

                dtRow = dtData.NewRow();
                dtRow[0] = sPaddyType.Value.Trim();
                dtRow[1] = QIKG.Value.Trim();
                dtRow[2] = avgrate.Value.Trim();

                dtData.Rows.Add(dtRow);
                Session["Data"] = null;
                Session["Data"] = dtData;
            }


            dataDisplay();
        }
        else
        {

            script = "alert('Please fill all filed of Party!!');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
        }

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
    public void dataDisplay()
    {
        StringBuilder htmlTable;
        if (Session["Data"] == null)
        {
            htmlTable = new StringBuilder();
            htmlTable.Append("<table class='table' cellspacing='0'>");
            htmlTable.Append("<tr><td align='center'>No Data Added...</td></tr></table>");
        }
        else
        {
            dtMain = (DataTable)Session["DataMain"];
            dt = (DataTable)Session["Data"];


            htmlTable = new StringBuilder();
            htmlTable.Append("<table class='table' runat='server' style='font-size:10pt; noWrap' id='printTable' cellspacing='0' border='1px'>");

            htmlTable.Append("<tr><td colspan='3' align='center'><span style='display:table-cell; vertical-align:top;'><img src='http://prabhasoftware.com/Rashmi Rice Logo (1).png' height='100px'/></span><span style='display:table-cell; vertical-align:top;'><span style='font-size:16pt; font-weight:bold;'> Rashmi Rice Mills Pvt. Ltd. </span></br><span style='font-size:8pt;'>Daniyawan Chandi Road, Hasanpur, Patna- 801304 </br>Mob.: 9304052349, 9334280057</br>Email: srirajbhog@gmail.com</br>CIN: U15312BR2014PTC022237</br>PAN No.: AAGCR9497P</br>GSTIN: 10AAGCR9497P1ZK</span></span></td></tr>");
            htmlTable.Append("<tr><td colspan='3' align='center'><span style='font-size:10pt; font-weight:bold;'>PURCHASE SAUDA REPORT </span></td></tr>");


            string source = dtMain.Rows[0]["PartyName"].ToString();
            string[] stringSeparators = new string[] { " (Mobile No.: " };
            var result = source.Split(stringSeparators, StringSplitOptions.None);

            string Pname = result[0];
            string PMobile = result[1].Substring(0, (result[1].Length - 1));
            htmlTable.Append("<tr><td align='left' rowspan='3' valign='top'><b>Party Details:</b></br>" + Pname + "</br>Mobile No.: " + PMobile + "</td>");
            if (dtMain.Rows[0]["MNo"].ToString() == "")
            {
                htmlTable.Append("<td colspan='2' align='left'>Sauda No.: <b>" + dtMain.Rows[0]["No"].ToString() + "</b></td></tr>");
            }
            else
            {
                htmlTable.Append("<td colspan='2' align='left'>Sauda No.: <b>" + dtMain.Rows[0]["No"].ToString() + "</b></br>Manual Sauda No.: <b>" + dtMain.Rows[0]["MNo"].ToString() + "</b></td></tr>");
            }
            htmlTable.Append("<tr><td colspan='2' align='left'>Sauda Date: " + Convert.ToDateTime(dtMain.Rows[0]["DataDate"].ToString()).ToString("dd/MM/yyyy") + "</td>");
            htmlTable.Append("<tr><td colspan='2' align='left'>Suppler's Ref.: " + dtMain.Rows[0]["BrokerName"].ToString() + "</td>");
            htmlTable.Append("<tr><td align='center' width='60%'><b>Description of Goods</b> </td>");
            htmlTable.Append("<td align='center' width='20%'><b>Qty. In KG</b></td>");
            htmlTable.Append("<td align='center' width='20%'><b>Rate /KG</b></td></tr>");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                htmlTable.Append("<tr><td align='left'>" + dt.Rows[i]["PaddyType"].ToString() + "</td>");
                htmlTable.Append("<td align='right'>" + Math.Round(Convert.ToDouble(dt.Rows[i]["QIKG"].ToString()), 3).ToString() + "</td>");
                htmlTable.Append("<td align='right'>" + Math.Round(Convert.ToDouble(dt.Rows[i]["Rate"].ToString()), 2).ToString() + "</td>");
                htmlTable.Append("</tr>");
            }

            htmlTable.Append("<tr><td align='left'><span style='font-size:7pt;'><b>Note:</b></br>All claim disputes will be resolved within 2 working days from the date of issue of this Purchase Order and receipt of a copy of this Order to you.</br>दावे से सम्बंधित सभी विवादों का समाधान इस खरीद आदेश के जारी होने और इस आदेश की प्रति आपको प्राप्त होने की तारीख से 2 कार्य दिवसों के भीतर किया जाएगा।</span></td>");
            htmlTable.Append("<td colspan='2' align='center'><b>For Rashmi Rice Mills Pvt. Ltd.</br></br></br>Authorised Signatory</b></td>");
            htmlTable.Append("</table>");

        }
        DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
    }
    public void CallPrint(string strid)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<script type = 'text/javascript'>");
        sb.Append("var prtContent = document.getElementById('" + strid + "');");
        sb.Append("var WinPrint = window.open('', '', 'letf=50,top=40,width=400,height=400,toolbar=0,scrollbars=0,status=0');");
        sb.Append("WinPrint.document.write(prtContent.innerHTML);");
        sb.Append("WinPrint.document.close();");
        sb.Append("WinPrint.focus();");
        sb.Append("setTimeout(function() {");
        sb.Append("WinPrint.print();");
        //sb.Append("return false;");
        sb.Append("WinPrint.close();");
        sb.Append("}, 250);");

        sb.Append("</script>");
        ClientScript.RegisterStartupScript(this.GetType(), "Print", sb.ToString());


    }
    public string ConvertNumbertoWords(long number)
    {
        if (number == 0) return "ZERO";
        if (number < 0) return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 1000000) > 0)
        {
            words += ConvertNumbertoWords(number / 100000) + " LAKH ";
            number %= 1000000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " THOUSAND ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
            number %= 100;
        }
        //if ((number / 10) > 0)  
        //{  
        // words += ConvertNumbertoWords(number / 10) + " RUPEES ";  
        // number %= 10;  
        //}  
        if (number > 0)
        {
            if (words != "") words += "AND ";
            var unitsMap = new[]   
        {  
            "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN"  
        };
            var tensMap = new[]   
        {  
            "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY"  
        };
            if (number < 20) words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0) words += " " + unitsMap[number % 10];
            }
        }
        return words;
    }
    public int PartyValidation()
    {
        int i = 1;
        if (sPartyName.SelectedItem.Text.Trim() == "Other")
        {
            if (pName.Value.Trim() == "" || pMN.Value.Trim() == "")
            {
                i = 0;
            }
            else
            {
                i = 1;
            }

        }
        else
        {
            i = 1;
        }
        return i;
    }
    public void Party()
    {
        dt = new DataTable();
        string q = "";
        param = new List<SqlParameter>();//Emp_Id
        q = "select concat(Party_Name, ' (Mobile No.: ',Party_Mobile,')') as PartyName from prabha.Purchase_Party_Info order by PartyName";
        dac = new DataAccessLayer();
        dt = dac.GetDataTable(q, param);

        sPartyName.DataSource = dt;
        sPartyName.DataTextField = "PartyName";
        sPartyName.DataValueField = "PartyName";
        sPartyName.DataBind();
        sPartyName.Items.Add("Other");
    }
    [WebMethod]
    public static List<string> GetEmployeeName(string empName)
    {
        List<string> empResult = new List<string>();
        using (SqlConnection con = new SqlConnection(@"Server=ws241.win.arvixe.com;Database=sati1983_farming;User ID=prabha;Password=prabha@#*2022;Trusted_Connection=False;"))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "SELECT [BrokerName] FROM [prabha].Purchase_Sauda_Info where [BrokerName] like ''+@SearchEmpName+'%'";
                cmd.Connection = con;
                con.Open();
                cmd.Parameters.AddWithValue("@SearchEmpName", empName);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    empResult.Add(dr["BrokerName"].ToString());
                }
                con.Close();
                return empResult;
            }
        }

    }
    protected void sPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (sPartyName.SelectedItem.Text.Trim() == "Other")
        {
            Panel1.Visible = true;
        }
        else
        {
            Panel1.Visible = false;
        }
    }
    public string GenInvoiceNo()
    {
        int mon = Convert.ToDateTime(sdate.Value.Trim()).Month;
        int yr = Convert.ToDateTime(sdate.Value.Trim()).Year;
        int yr1 = 0;
        int yr2 = 0;
        string dtFrom;
        string dtTo;

        if (mon <= 3)
        {
            dtFrom = "01-Apr-" + (yr - 1);
            dtTo = "31-Mar-" + yr;
            yr1 = yr - 1;
            yr2 = yr;
        }
        else
        {
            dtFrom = "01-Apr-" + yr;
            dtTo = "31-Mar-" + (yr + 1);
            yr1 = yr;
            yr2 = yr + 1;
        }

        string invoiceNo = "";
        string q = "";

        param = new List<SqlParameter>();//Emp_Id

        param.Add(new SqlParameter("@DataDate1", Convert.ToDateTime(dtFrom).ToString("dd-MMM-yyyy")));
        param.Add(new SqlParameter("@DataDate2", Convert.ToDateTime(dtTo).ToString("dd-MMM-yyyy")));

        q = "select max([No]) from prabha.Purchase_Sauda_Info where DataDate>=@DataDate1 and DataDate<=@DataDate2";
        dac = new DataAccessLayer();
        object test = dac.Scalar(q, param);
        if (test == DBNull.Value)
        {
            invoiceNo = "RR/PS/" + yr1 + "-" + yr2 + "/0001";
        }
        else
        {
            if ((Convert.ToInt32(test) + 1).ToString().Length == 1)
            {
                invoiceNo = "RR/PS/" + yr1 + "-" + yr2 + "/000" + (Convert.ToInt32(test) + 1);
            }
            else if ((Convert.ToInt32(test) + 1).ToString().Length == 2)
            {
                invoiceNo = "RR/PS/" + yr1 + "-" + yr2 + "/00" + (Convert.ToInt32(test) + 1);
            }
            else if ((Convert.ToInt32(test) + 1).ToString().Length == 3)
            {
                invoiceNo = "RR/PS/" + yr1 + "-" + yr2 + "/0" + (Convert.ToInt32(test) + 1);
            }
            else
            {
                invoiceNo = "RR/PS/" + yr1 + "-" + yr2 + "/" + Convert.ToInt32(test) + 1;
            }

        }
        return invoiceNo;
    }
    public void insertPurchaseData()
    {

        dtMain = (DataTable)Session["DataMain"];
        dt = (DataTable)Session["Data"];

        int chkValid = chkData(dtMain.Rows[0]["DataDate"].ToString(), dtMain.Rows[0]["PartyName"].ToString());

        if (chkValid == 0)
        {
            int msg = 0;
            string q = "";
            param = new List<SqlParameter>();//Emp_Id

            string[] Inv = dtMain.Rows[0]["No"].ToString().Split('/');
            param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
            param.Add(new SqlParameter("@No", Inv[3]));
            param.Add(new SqlParameter("@DataDate", Convert.ToDateTime(dtMain.Rows[0]["DataDate"].ToString()).ToString("dd-MMM-yyyy")));
            param.Add(new SqlParameter("@MNo", dtMain.Rows[0]["MNo"].ToString()));
            param.Add(new SqlParameter("@PartyName", dtMain.Rows[0]["PartyName"].ToString()));
            param.Add(new SqlParameter("@BrokerName", dtMain.Rows[0]["BrokerName"].ToString()));

            double RWt = 0;
            double RRate = 0;
            double MWt = 0;
            double MRate = 0;
            double SWt = 0;
            double SRate = 0;
            double HWt = 0;
            double HRate = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["PaddyType"].ToString() == "Rupali")
                {
                    RWt = Convert.ToDouble(dt.Rows[i]["QIKG"].ToString());
                    RRate = Convert.ToDouble(dt.Rows[i]["Rate"].ToString());
                }
                if (dt.Rows[i]["PaddyType"].ToString() == "Mansuri")
                {
                    MWt = Convert.ToDouble(dt.Rows[i]["QIKG"].ToString());
                    MRate = Convert.ToDouble(dt.Rows[i]["Rate"].ToString());
                }
                if (dt.Rows[i]["PaddyType"].ToString() == "Sonam")
                {
                    SWt = Convert.ToDouble(dt.Rows[i]["QIKG"].ToString());
                    SRate = Convert.ToDouble(dt.Rows[i]["Rate"].ToString());
                }
                if (dt.Rows[i]["PaddyType"].ToString() == "Hybrid")
                {
                    HWt = Convert.ToDouble(dt.Rows[i]["QIKG"].ToString());
                    HRate = Convert.ToDouble(dt.Rows[i]["Rate"].ToString());
                }
            }
            param.Add(new SqlParameter("@RupaliWt", RWt.ToString()));
            param.Add(new SqlParameter("@RupaliRate", RRate.ToString()));
            param.Add(new SqlParameter("@MansuriWt", MWt.ToString()));
            param.Add(new SqlParameter("@MansuriRate", MRate.ToString()));
            param.Add(new SqlParameter("@SonamWt", SWt.ToString()));
            param.Add(new SqlParameter("@SonamRate", SRate.ToString()));
            param.Add(new SqlParameter("@HybridWt", HWt.ToString()));
            param.Add(new SqlParameter("@HybridRate", HRate.ToString()));
            param.Add(new SqlParameter("@OperatorName", Session["User"].ToString()));
            param.Add(new SqlParameter("@EntryDate", Convert.ToDateTime(System.DateTime.Now).ToString("dd-MMM-yyyy")));
            param.Add(new SqlParameter("@IsActive", "1"));

            q = "insert into prabha.Purchase_Sauda_Info([No],DataDate,MNo,PartyName,BrokerName,";
            q += "RupaliWt,RupaliRate,MansuriWt,MansuriRate,SonamWt,SonamRate,HybridWt,HybridRate,OperatorName,EntryDate,IsActive) ";
            q += " values(@CompanyID,@No,@DataDate,@MNo,@PartyName,@BrokerName,";
            q += "@RupaliWt,@RupaliRate,@MansuriWt,@MansuriRate,@SonamWt,@SonamRate,@HybridWt,@HybridRate,@OperatorName,@EntryDate,@IsActive) ";
            dac = new DataAccessLayer();

            msg = dac.update(q, param);


            if (msg > 0)
            {

                if (sPartyName.SelectedItem.Text.Trim() == "Other")
                {
                    string source = dtMain.Rows[0]["PartyName"].ToString();
                    string[] stringSeparators = new string[] { " (Mobile No.: " };
                    var result = source.Split(stringSeparators, StringSplitOptions.None);

                    string Pname = result[0];
                    string PMobile = result[1].Substring(0, (result[1].Length - 1));

                    q = "";
                    param = new List<SqlParameter>();
                    param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
                    param.Add(new SqlParameter("@Party_Name", Pname));
                    param.Add(new SqlParameter("@Party_Mobile", PMobile));
                    q = "insert into prabha.Purchase_Party_Info(Party_Name,Party_Mobile) values(@Party_Name,@Party_Mobile)";
                    dac = new DataAccessLayer();
                    int OutMsg = dac.update(q, param);
                }
            }
            else
            {
                script = "alert('Error!!');";
                ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
            }
        }
        else
        {
            script = "alert('Data Already Exist!!');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
        }

    }
    public int chkData(string DDate, string PN)
    {
        int tst = 0;
        DataTable dtOut = new DataTable();
        string q = "";
        param = new List<SqlParameter>();//Emp_Id

        param.Add(new SqlParameter("@DataDate", Convert.ToDateTime(DDate).ToString("dd-MMM-yyyy")));

        param.Add(new SqlParameter("@PartyName", PN));

        q = "select * from prabha.Purchase_Sauda_Info where DataDate=@DataDate and PartyName=@PartyName";
        dac = new DataAccessLayer();
        dtOut = dac.GetDataTable(q, param);
        if (dtOut.Rows.Count > 0)
        {
            tst = 1;
        }
        else
        {
            tst = 0;
        }
        return tst;
    }
    protected void lBtnSaudaParty_Click(object sender, EventArgs e)
    {
        dt = new DataTable();
        string q = "";
        param = new List<SqlParameter>();//Emp_Id

        param.Add(new SqlParameter("@PartyName", sPartyName.SelectedItem.Text.Trim()));

        q = "select ID,[No],DataDate,PartyName,BrokerName from prabha.Purchase_Sauda_Info where PartyName=@PartyName order by DataDate desc";

        dac = new DataAccessLayer();
        dt = dac.GetDataTable(q, param);

        StringBuilder htmlTable = new StringBuilder();
        string INVNo = "";
        htmlTable.Append("<table class='table table-bordered' id='dataTable' cellspacing='0'>");
        htmlTable.Append("<thead><tr><th>Sl. No.</th><th>Sauda No. & Date</th><th>Party Name</th><th>Supplier's Ref.</th><th></th></tr></thead><tbody>");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            htmlTable.Append("<tr>");
            htmlTable.Append("<td>" + (i + 1) + "</td>");
            INVNo = GenInvoiceNo(dt.Rows[i]["No"].ToString(), dt.Rows[i]["DataDate"].ToString());
            htmlTable.Append("<td><a href='PO.aspx?ID=" + dt.Rows[i]["ID"].ToString() + "' target='_blank'>" + INVNo + ", " + Convert.ToDateTime(dt.Rows[i]["DataDate"].ToString()).ToString("dd/MM/yyyy") + "</a></td>");
            htmlTable.Append("<td>" + dt.Rows[i]["PartyName"].ToString() + "</td>");
            htmlTable.Append("<td>" + dt.Rows[i]["BrokerName"].ToString() + "</td>");

            htmlTable.Append("<td><a href='PurchaseUnloading.aspx?ID=" + dt.Rows[i]["ID"].ToString() + "' target='_blank'>Purchase Entry</a></td></tr>");
        }
        htmlTable.Append("</tbody></table>");
        DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
    }
    public string GenInvoiceNo(string a, string b)
    {
        int mon = Convert.ToDateTime(b).Month;
        int yr = Convert.ToDateTime(b).Year;
        int yr1 = 0;
        int yr2 = 0;

        if (mon <= 3)
        {
            yr1 = yr - 1;
            yr2 = yr;
        }
        else
        {
            yr1 = yr;
            yr2 = yr + 1;
        }

        string invoiceNo = "";

        if (a.Length == 1)
        {
            invoiceNo = "RR/PS/" + yr1 + "-" + yr2 + "/000" + a;
        }
        else if (a.Length == 2)
        {
            invoiceNo = "RR/PS/" + yr1 + "-" + yr2 + "/00" + a;
        }
        else if (a.Length == 3)
        {
            invoiceNo = "RR/PS/" + yr1 + "-" + yr2 + "/0" + a;
        }
        else
        {
            invoiceNo = "RR/PS/" + yr1 + "-" + yr2 + "/" + a;
        }


        return invoiceNo;
    }
}
