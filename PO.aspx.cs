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

public partial class PO : System.Web.UI.Page
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

            try
            {
                dataDisplay();
            }
            catch (Exception ex)
            {
                Trace.Warn(ex.Message);
            }

        }
    }
    public void dataDisplay()
    {
        string q = "";
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@ID", Request.QueryString["ID"].ToString()));
        q = "select * from [prabha].[Purchase_Sauda_Info] where ID=@ID";
        dac = new DataAccessLayer();
        dt = dac.GetDataTable(q, param);
        StringBuilder htmlTable;
        if (dt.Rows.Count <= 0)
        {
            htmlTable = new StringBuilder();
            htmlTable.Append("<table class='table' cellspacing='0'>");
            htmlTable.Append("<tr><td align='center'>No Data Found...</td></tr></table>");
        }
        else
        {
            
            htmlTable = new StringBuilder();
            htmlTable.Append("<table class='table' runat='server' style='font-size:10pt; noWrap' id='printTable' cellspacing='0' border='1px'>");

            htmlTable.Append("<tr><td colspan='3' align='center'><span style='display:table-cell; vertical-align:top;'><img src='http://prabhasoftware.com/Rashmi Rice Logo (1).png' height='100px'/></span><span style='display:table-cell; vertical-align:top;'><span style='font-size:16pt; font-weight:bold;'> Rashmi Rice Mills Pvt. Ltd. </span></br><span style='font-size:8pt;'>Daniyawan Chandi Road, Hasanpur, Patna- 801304 </br>Mob.: 9304052349, 9334280057</br>Email: srirajbhog@gmail.com</br>CIN: U15312BR2014PTC022237</br>PAN No.: AAGCR9497P</br>GSTIN: 10AAGCR9497P1ZK</span></span></td></tr>");
            htmlTable.Append("<tr><td colspan='3' align='center'><span style='font-size:10pt; font-weight:bold;'>PURCHASE SAUDA REPORT </span></td></tr>");

            string source = dt.Rows[0]["PartyName"].ToString();
            string[] stringSeparators = new string[] { " (Mobile No.: " };
            var result = source.Split(stringSeparators, StringSplitOptions.None);

            string Pname = result[0];
            string PMobile = result[1].Substring(0, (result[1].Length - 1));
            htmlTable.Append("<tr><td align='left' rowspan='3' valign='top'><b>Party Details:</b></br>" + Pname + "</br>Mobile No.: " + PMobile + "</td>");
            string INVNo = "";
            INVNo = GenInvoiceNo(dt.Rows[0]["No"].ToString(), dt.Rows[0]["DataDate"].ToString());
            if (dt.Rows[0]["MNo"].ToString() == "")
            {
                htmlTable.Append("<td colspan='2' align='left'>Sauda No.: <b>" + INVNo + "</b></td></tr>");
            }
            else
            {
                htmlTable.Append("<td colspan='2' align='left'>Sauda No.: <b>" + INVNo + "</b></br>Manual Sauda No.: <b>" + dt.Rows[0]["MNo"].ToString() + "</b></td></tr>");
            }
            htmlTable.Append("<tr><td colspan='2' align='left'>Sauda Date: " + Convert.ToDateTime(dt.Rows[0]["DataDate"].ToString()).ToString("dd/MM/yyyy") + "</td></tr>");
            htmlTable.Append("<tr><td colspan='2' align='left'>Suppler's Ref.: " + dt.Rows[0]["BrokerName"].ToString() + "</td></tr>");
            htmlTable.Append("<tr><td align='center' width='60%'><b>Description of Goods</b> </td>");
            htmlTable.Append("<td align='center' width='20%'><b>Qty. In KG</b></td>");
            htmlTable.Append("<td align='center' width='20%'><b>Rate /KG</b></td></tr>");

            if (Convert.ToDouble(dt.Rows[0]["RupaliWt"].ToString()) == 0)
            {

            }
            else
            {
                htmlTable.Append("<tr><td align='left'>Rupali</td>");
                htmlTable.Append("<td align='right'>" + Math.Round(Convert.ToDouble(dt.Rows[0]["RupaliWt"].ToString()), 3).ToString() + "</td>");
                htmlTable.Append("<td align='right'>" + Math.Round(Convert.ToDouble(dt.Rows[0]["RupaliRate"].ToString()), 2).ToString() + "</td>");
                htmlTable.Append("</tr>");
            }
            if (Convert.ToDouble(dt.Rows[0]["MansuriWt"].ToString()) == 0)
            {

            }
            else
            {
                htmlTable.Append("<tr><td align='left'>Mansuri</td>");
                htmlTable.Append("<td align='right'>" + Math.Round(Convert.ToDouble(dt.Rows[0]["MansuriWt"].ToString()), 3).ToString() + "</td>");
                htmlTable.Append("<td align='right'>" + Math.Round(Convert.ToDouble(dt.Rows[0]["MansuriRate"].ToString()), 2).ToString() + "</td>");
                htmlTable.Append("</tr>");
            }
            if (Convert.ToDouble(dt.Rows[0]["SonamWt"].ToString()) == 0)
            {

            }
            else
            {
                htmlTable.Append("<tr><td align='left'>Sonam</td>");
                htmlTable.Append("<td align='right'>" + Math.Round(Convert.ToDouble(dt.Rows[0]["SonamWt"].ToString()), 3).ToString() + "</td>");
                htmlTable.Append("<td align='right'>" + Math.Round(Convert.ToDouble(dt.Rows[0]["SonamRate"].ToString()), 2).ToString() + "</td>");
                htmlTable.Append("</tr>");
            }
            if (Convert.ToDouble(dt.Rows[0]["HybridWt"].ToString()) == 0)
            {

            }
            else
            {
                htmlTable.Append("<tr><td align='left'>Hybrid</td>");
                htmlTable.Append("<td align='right'>" + Math.Round(Convert.ToDouble(dt.Rows[0]["HybridWt"].ToString()), 3).ToString() + "</td>");
                htmlTable.Append("<td align='right'>" + Math.Round(Convert.ToDouble(dt.Rows[0]["HybridRate"].ToString()), 2).ToString() + "</td>");
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
    public void btnSave_ServerClick(object sender, EventArgs e)
    {
        
            dataDisplay();
            CallPrint("prntContent");
        

    }
}