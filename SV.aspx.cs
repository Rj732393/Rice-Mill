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

public partial class SV : System.Web.UI.Page
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
        q = "select * from [prabha].[Sale_Payment_Info] where ID=@ID";
        dac = new DataAccessLayer();
        Session["Data"] = dac.GetDataTable(q, param);
        StringBuilder htmlTable;
        if (Session["Data"] == null)
        {
            htmlTable = new StringBuilder();
            htmlTable.Append("<table class='table' cellspacing='0'>");
            htmlTable.Append("<tr><td align='center'>No Data Added...</td></tr></table>");
        }
        else
        {
            dt = (DataTable)Session["Data"];
            htmlTable = new StringBuilder();
            htmlTable.Append("<table runat='server' style='font-size:10pt; noWrap; min-width: 600px; min-height:400px;' id='printTable' cellspacing='0' border='1px'>");

            htmlTable.Append("<tr><td colspan='2' align='center'><span style='display:table-cell; vertical-align:top;'><img src='http://prabhasoftware.com/Rashmi Rice Logo (1).png' height='100px'/></span><span style='display:table-cell; vertical-align:top;'><span style='font-size:16pt; font-weight:bold;'> Rashmi Rice Mills Pvt. Ltd. </span></br><span style='font-size:8pt;'>Daniyawan Chandi Road, Hasanpur, Patna- 801304 </br>Mob.: 9304052349, 9334280057</br>Email: srirajbhog@gmail.com</br>CIN: U15312BR2014PTC022237</br>PAN No.: AAGCR9497P</br>GSTIN: 10AAGCR9497P1ZK</span></span></td></tr>");
            htmlTable.Append("<tr><td colspan='2' align='center'><span style='font-size:10pt; font-weight:bold;'> RECEIPT VOUCHER </span></td></tr>");

            string InvNo = GenInvoiceNo(dt.Rows[0]["No"].ToString(), dt.Rows[0]["DataDate"].ToString());
            if (dt.Rows[0]["MRVNo"].ToString() == "")
            {
                htmlTable.Append("<tr><td colspan='2' align='right'>Voucher No.: <b>" + InvNo + "</b></td></tr>");
            }
            else
            {
                htmlTable.Append("<tr><td colspan='2' align='right'>Voucher No.: <b>" + InvNo + "</b></br>Manual Voucher No.: <b>" + dt.Rows[0]["MPVNo"].ToString() + "</b></td></tr>");
            }
            htmlTable.Append("<tr><td colspan='2' align='right'>Payment Date: " + Convert.ToDateTime(dt.Rows[0]["DataDate"].ToString()).ToString("dd/MM/yyyy") + "</td></tr>");
            htmlTable.Append("<tr><td colspan='2' align='left'>Amount: <b>" + dt.Rows[0]["AmountPaid"].ToString() + " (RUPEES " + ConvertNumbertoWords(Convert.ToInt64(Math.Round(Convert.ToDouble(dt.Rows[0]["AmountPaid"].ToString()), 0))) + " ONLY)</b></td></tr>");
            htmlTable.Append("<tr><td colspan='2' align='left'>Party Name: " + dt.Rows[0]["PName"].ToString() + "</td></tr>");

            if (dt.Rows[0]["PaymentMode"].ToString() == "By Cash")
            {
                htmlTable.Append("<tr><td colspan='2' align='left'>Payment Mode: Cash</td></tr>");
                htmlTable.Append("<tr><td colspan='2' align='left'>Amount Paid To: " + dt.Rows[0]["Transaction"].ToString() + "</td></tr>");
            }
            else if (dt.Rows[0]["PaymentMode"].ToString() == "By Cheque")
            {
                htmlTable.Append("<tr><td colspan='2' align='left'>Payment Mode: Cheque</td></tr>");
                htmlTable.Append("<tr><td colspan='2' align='left'>Cheque No. & Date: " + dt.Rows[0]["Transaction"].ToString() + "</td></tr>");
            }
            else
            {
                htmlTable.Append("<tr><td colspan='2' align='left'>Payment Mode: Online</td></tr>");
                
                htmlTable.Append("<tr><td colspan='2' align='left'>Payment Reference No.: " + dt.Rows[0]["Transaction"].ToString() + "</td></tr>");
            }
            htmlTable.Append("<tr><td width='50%' align='left'></td><td align='center'><b>For Rashmi Rice Mills Pvt. Ltd.</br></br>Authorised Signatory</b></td></tr>");
            htmlTable.Append("</table>");
        }
        DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
    }
    public void btnSave_ServerClick(object sender, EventArgs e)
    {

        dataDisplay();
        CallPrint("prntContent");


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
            invoiceNo = "RR/PV/" + yr1 + "-" + yr2 + "/000" + a;
        }
        else if (a.Length == 2)
        {
            invoiceNo = "RR/PV/" + yr1 + "-" + yr2 + "/00" + a;
        }
        else if (a.Length == 3)
        {
            invoiceNo = "RR/PV/" + yr1 + "-" + yr2 + "/0" + a;
        }
        else
        {
            invoiceNo = "RR/PV/" + yr1 + "-" + yr2 + "/" + a;
        }


        return invoiceNo;
    }
}