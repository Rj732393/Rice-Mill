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

public partial class Payment : System.Web.UI.Page
{
    DataTable dt;
    List<SqlParameter> param;
    DataAccessLayer dac;
    string script;

    /* ================================================================
       PAGE LOAD
    ================================================================ */
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // Session check
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            sdate.Attributes["type"] = "date";
            amountpaid.Attributes["type"] = "number";
            amountpaid.Attributes["step"] = ".01";
            amountpaid.Attributes["min"] = "0.01";
            pACNo.Attributes["type"] = "number";
            pACNo.Attributes["step"] = "1";
            lblOSB.Attributes["type"] = "number";
            lblOSB.Attributes["step"] = ".01";
            lblOSB.Attributes["min"] = "0";
            lblOSB.Attributes["placeholder"] = "Enter previous balance";

            bindParty();
            dataDisplay();
        }
    }

    /* ================================================================
       ADD PAYMENT (btnContinue)
    ================================================================ */
    public void btnContinue_ServerClick(object sender, EventArgs e)
    {
        // ---- Server-side validation ----
        string errMsg = ServerValidate();
        if (errMsg != "")
        {
            ShowAlert(errMsg);
            dataDisplay();
            return;
        }

        if (bankvalidate() != 0)
        {
            ShowAlert("Please fill all bank details correctly.");
            dataDisplay();
            return;
        }

        // ---- Build DataTable ----
        Session["Data"] = null;
        dt = new DataTable();
        dt.Columns.Add("No");
        dt.Columns.Add("DataDate");
        dt.Columns.Add("PName");
        dt.Columns.Add("AmountPaid");
        dt.Columns.Add("PaymentMode");
        dt.Columns.Add("Transaction");
        dt.Columns.Add("Bank");
        dt.Columns.Add("MPVNo");

        DataRow myrow = dt.NewRow();
        myrow["No"] = GenInvoiceNo();
        myrow["DataDate"] = Convert.ToDateTime(sdate.Value.Trim()).ToString("dd-MMM-yyyy");
        myrow["PName"] = ddlParty.SelectedItem.Text.Trim();
        myrow["AmountPaid"] = amountpaid.Value.Trim();
        myrow["PaymentMode"] = paymentmode.Value.Trim();
        myrow["Transaction"] = transaction.Value.Trim();
        myrow["Bank"] = (paymentmode.Value.Trim() == "Online")
                                ? pACName.Value.Trim() + " (" + pACNo.Value.Trim() + ")"
                                : " ";
        myrow["MPVNo"] = pvNo.Value.Trim();
        dt.Rows.Add(myrow);

        Session["Data"] = dt;
        dataDisplay();
    }

    /* ================================================================
       SUBMIT PAYMENT (btnSave)
    ================================================================ */
    public void btnSave_ServerClick(object sender, EventArgs e)
    {
        if (Session["Data"] == null)
        {
            ShowAlert("Please add at least one payment entry before submitting.");
            dataDisplay();
            return;
        }

        if (Session["User"] == null)
        {
            ShowAlert("Your session has expired. Please login again.");
            dataDisplay();
            return;
        }

        insertData();
        dataDisplay();
        CallPrint("prntContent");
    }

    /* ================================================================
       PARTY DROPDOWN CHANGED
    ================================================================ */
    protected void ddlParty_SelectedIndexChanged(object sender, EventArgs e)
    {
        Party();
    }

    /* ================================================================
       SERVER-SIDE VALIDATION  — returns error string or ""
    ================================================================ */
    private string ServerValidate()
    {
        // Date
        if (string.IsNullOrWhiteSpace(sdate.Value))
            return "Please select a date.";

        DateTime parsedDate;
        if (!DateTime.TryParse(sdate.Value.Trim(), out parsedDate))
            return "Please enter a valid date.";

        // Party
        if (ddlParty.Items.Count == 0 || string.IsNullOrWhiteSpace(ddlParty.SelectedItem.Text))
            return "Please select a party name.";

        // Amount
        if (string.IsNullOrWhiteSpace(amountpaid.Value))
            return "Please enter the amount paid.";

        double amt;
        if (!double.TryParse(amountpaid.Value.Trim(), out amt))
            return "Amount must be a valid number.";

        if (amt <= 0)
            return "Amount must be greater than zero.";

        // Transaction / Paid To
        string mode = paymentmode.Value.Trim();
        if (string.IsNullOrWhiteSpace(transaction.Value))
        {
            if (mode == "Online") return "Please enter Transaction / Reference ID.";
            else if (mode == "By Cheque") return "Please enter Cheque No. & Date.";
            else return "Please enter the name of person paid to.";
        }

        // Online extra fields
        if (mode == "Online")
        {
            if (string.IsNullOrWhiteSpace(pACName.Value))
                return "Please enter Account Holder Name.";

            if (string.IsNullOrWhiteSpace(pACNo.Value))
                return "Please enter Account Number.";

            if (ddlBank.SelectedItem == null || ddlBank.SelectedItem.Text.Trim() == "--Select Bank--")
                return "Please select a Bank.";

            if (string.IsNullOrWhiteSpace(pBankIFSC.Value))
                return "Please enter Bank IFSC Code.";
        }

        return ""; // All good
    }

    /* ================================================================
       SHOW ALERT (client-side)
    ================================================================ */
    private void ShowAlert(string msg)
    {
        script = "document.getElementById('topAlert').style.display='block';" +
                 "document.getElementById('topAlert').innerText='" + msg.Replace("'", "\\'") + "';" +
                 "document.getElementById('topAlert').className='alert-custom alert-danger-custom';" +
                 "window.scrollTo(0,0);";
        ClientScript.RegisterStartupScript(this.GetType(), "ShowAlert", script, true);
    }

    private void ShowSuccess(string msg)
    {
        script = "document.getElementById('topAlert').style.display='block';" +
                 "document.getElementById('topAlert').innerText='" + msg.Replace("'", "\\'") + "';" +
                 "document.getElementById('topAlert').className='alert-custom alert-success-custom';" +
                 "window.scrollTo(0,0);";
        ClientScript.RegisterStartupScript(this.GetType(), "ShowAlert", script, true);
    }

    /* ================================================================
       DATA DISPLAY
    ================================================================ */
    public void dataDisplay()
    {
        StringBuilder htmlTable;

        if (Session["Data"] == null)
        {
            htmlTable = new StringBuilder();
            htmlTable.Append("<table class='table' cellspacing='0'>");
            htmlTable.Append("<tr><td align='center' style='color:#94a3b8;padding:30px;'>No data added yet...</td></tr></table>");
        }
        else
        {
            dt = (DataTable)Session["Data"];
            htmlTable = new StringBuilder();
            htmlTable.Append("<div id='prntContent'>");
            htmlTable.Append("<table runat='server' style='font-size:10pt; min-width:600px;' id='printTable' cellspacing='0' border='1'>");

            // Header
            htmlTable.Append("<tr><td colspan='2' align='center'>" +
                "<span style='display:table-cell; vertical-align:top;'>" +
                "<img src='http://prabhasoftware.com/Rashmi Rice Logo (1).png' height='100px'/></span>" +
                "<span style='display:table-cell; vertical-align:top;'>" +
                "<span style='font-size:16pt; font-weight:bold;'> Rashmi Rice Mills Pvt. Ltd. </span><br/>" +
                "<span style='font-size:8pt;'>Daniyawan Chandi Road, Hasanpur, Patna- 801304 <br/>" +
                "Mob.: 9304052349, 9334280057<br/>Email: srirajbhog@gmail.com<br/>" +
                "CIN: U15312BR2014PTC022237<br/>PAN No.: AAGCR9497P<br/>GSTIN: 10AAGCR9497P1ZK</span></span></td></tr>");

            htmlTable.Append("<tr><td colspan='2' align='center'><span style='font-size:10pt; font-weight:bold;'> PAYMENT VOUCHER </span></td></tr>");

            // Voucher No
            if (dt.Rows[0]["MPVNo"].ToString().Trim() == "")
            {
                htmlTable.Append("<tr><td colspan='2' align='right'>Voucher No.: <b>" + dt.Rows[0]["No"] + "</b></td></tr>");
            }
            else
            {
                htmlTable.Append("<tr><td colspan='2' align='right'>Voucher No.: <b>" + dt.Rows[0]["No"] +
                    "</b><br/>Manual Voucher No.: <b>" + dt.Rows[0]["MPVNo"] + "</b></td></tr>");
            }

            htmlTable.Append("<tr><td colspan='2' align='right'>Payment Date: " + dt.Rows[0]["DataDate"] + "</td></tr>");

            // Amount in words
            long amtLong = Convert.ToInt64(Math.Round(Convert.ToDouble(dt.Rows[0]["AmountPaid"].ToString()), 0));
            htmlTable.Append("<tr><td colspan='2' align='left'>Amount: <b>" + dt.Rows[0]["AmountPaid"] +
                " (RUPEES " + ConvertNumbertoWords(amtLong) + " ONLY)</b></td></tr>");

            htmlTable.Append("<tr><td colspan='2' align='left'>Party Name: " + dt.Rows[0]["PName"] + "</td></tr>");

            // Payment mode details
            string pMode = dt.Rows[0]["PaymentMode"].ToString();
            if (pMode == "By Cash")
            {
                htmlTable.Append("<tr><td colspan='2' align='left'>Payment Mode: Cash</td></tr>");
                htmlTable.Append("<tr><td colspan='2' align='left'>Paid To: " + dt.Rows[0]["Transaction"] + "</td></tr>");
            }
            else if (pMode == "By Cheque")
            {
                htmlTable.Append("<tr><td colspan='2' align='left'>Payment Mode: Cheque</td></tr>");
                htmlTable.Append("<tr><td colspan='2' align='left'>Cheque No. &amp; Date: " + dt.Rows[0]["Transaction"] + "</td></tr>");
            }
            else
            {
                htmlTable.Append("<tr><td colspan='2' align='left'>Payment Mode: Online</td></tr>");
                htmlTable.Append("<tr><td colspan='2' align='left'>On Account Of: " + dt.Rows[0]["Bank"] + "</td></tr>");
                htmlTable.Append("<tr><td colspan='2' align='left'>Payment Reference No.: " + dt.Rows[0]["Transaction"] + "</td></tr>");
            }

            htmlTable.Append("<tr><td width='50%' align='left'></td>" +
                "<td align='center'><b>For Rashmi Rice Mills Pvt. Ltd.<br/><br/>Authorised Signatory</b></td></tr>");
            htmlTable.Append("</table>");
            htmlTable.Append("</div>"); // prntContent
        }

        DBDataPlaceHolder.Controls.Clear();
        DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
    }

    /* ================================================================
       PRINT
    ================================================================ */
    public void CallPrint(string strid)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<script type='text/javascript'>");
        sb.Append("var prtContent = document.getElementById('" + strid + "');");
        sb.Append("var WinPrint = window.open('','','left=50,top=40,width=600,height=500,toolbar=0,scrollbars=0,status=0');");
        sb.Append("WinPrint.document.write(prtContent.innerHTML);");
        sb.Append("WinPrint.document.close();");
        sb.Append("WinPrint.focus();");
        sb.Append("setTimeout(function(){");
        sb.Append("WinPrint.print();");
        sb.Append("WinPrint.close();");
        sb.Append("},350);");
        sb.Append("</" + "script>");
        ClientScript.RegisterStartupScript(this.GetType(), "Print", sb.ToString());
    }

    /* ================================================================
       CONVERT NUMBER TO WORDS
    ================================================================ */
    public string ConvertNumbertoWords(long number)
    {
        if (number == 0) return "ZERO";
        if (number < 0) return "MINUS " + ConvertNumbertoWords(Math.Abs(number));

        string words = "";

        if ((number / 100000) > 0)
        {
            words += ConvertNumbertoWords(number / 100000) + " LAKH ";
            number %= 100000;
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
        if (number > 0)
        {
            if (words != "") words += "AND ";
            var unitsMap = new[] {
                "ZERO","ONE","TWO","THREE","FOUR","FIVE","SIX","SEVEN","EIGHT","NINE",
                "TEN","ELEVEN","TWELVE","THIRTEEN","FOURTEEN","FIFTEEN","SIXTEEN",
                "SEVENTEEN","EIGHTEEN","NINETEEN"
            };
            var tensMap = new[] {
                "ZERO","TEN","TWENTY","THIRTY","FORTY","FIFTY","SIXTY","SEVENTY","EIGHTY","NINETY"
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

    /* ================================================================
       BIND PARTY DROPDOWN
    ================================================================ */
    public void bindParty()
    {
        param = new List<SqlParameter>();
        string q = "select concat(Party_Name, ' (Mobile No.: ',Party_Mobile,')') as PartyName," +
                   "Bank_Name,Account_No,Account_Name,IFSC_Code " +
                   "from prabha.Purchase_Party_Info order by PartyName";
        dac = new DataAccessLayer();
        DataTable dtP = dac.GetDataTable(q, param);

        ddlParty.DataSource = dtP;
        ddlParty.DataTextField = "PartyName";
        ddlParty.DataValueField = "PartyName";
        ddlParty.DataBind();
    }

    /* ================================================================
       FILL PARTY BANK DETAILS
    ================================================================ */
    public void Party()
    {
        string source = ddlParty.SelectedItem.Text.Trim();
        string[] sep = new string[] { " (Mobile No.: " };
        var parts = source.Split(sep, StringSplitOptions.None);

        if (parts.Length < 2) return;

        string Pname = parts[0];
        string PMobile = parts[1].TrimEnd(')');

        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@Party_Name", Pname));
        param.Add(new SqlParameter("@Party_Mobile", PMobile));

        string q = "select Bank_Name,Account_No,Account_Name,IFSC_Code " +
                   "from prabha.Purchase_Party_Info " +
                   "where Party_Name=@Party_Name and Party_Mobile=@Party_Mobile";

        dac = new DataAccessLayer();
        DataTable dtPBank = dac.GetDataTable(q, param);

        if (dtPBank.Rows.Count == 0) return;

        DataRow r = dtPBank.Rows[0];

        // Account Name
        pACName.Value = r["Account_Name"].ToString();
        pACName.Disabled = (r["Account_Name"].ToString() != "");

        // Account No
        pACNo.Value = r["Account_No"].ToString();
        pACNo.Disabled = (r["Account_No"].ToString() != "");

        // Bank
        if (r["Bank_Name"].ToString() == "")
        {
            ddlBank.Items.FindByText("--Select Bank--").Selected = true;
            ddlBank.Enabled = true;
        }
        else
        {
            ListItem li = ddlBank.Items.FindByText(r["Bank_Name"].ToString());
            if (li != null) { li.Selected = true; }
            ddlBank.Enabled = false;
        }

        // IFSC
        pBankIFSC.Value = r["IFSC_Code"].ToString();
        pBankIFSC.Disabled = (r["IFSC_Code"].ToString() != "");
    }

    /* ================================================================
       GENERATE INVOICE / VOUCHER NO
    ================================================================ */
    public string GenInvoiceNo()
    {
        DateTime d = Convert.ToDateTime(sdate.Value.Trim());
        int mon = d.Month;
        int yr = d.Year;
        int yr1, yr2;

        if (mon <= 3) { yr1 = yr - 1; yr2 = yr; }
        else { yr1 = yr; yr2 = yr + 1; }

        string dtFrom = "01-Apr-" + yr1;
        string dtTo = "31-Mar-" + yr2;

        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@DataDate1", Convert.ToDateTime(dtFrom).ToString("dd-MMM-yyyy")));
        param.Add(new SqlParameter("@DataDate2", Convert.ToDateTime(dtTo).ToString("dd-MMM-yyyy")));

        string q = "select max([No]) from prabha.Purchase_Payment_Info " +
                   "where DataDate>=@DataDate1 and DataDate<=@DataDate2";

        dac = new DataAccessLayer();
        object test = dac.Scalar(q, param);

        string prefix = "RR/PV/" + yr1 + "-" + yr2 + "/";

        if (test == DBNull.Value || test == null)
            return prefix + "0001";

        int next = Convert.ToInt32(test) + 1;
        return prefix + next.ToString("D4");
    }

    /* overload — for display in list */
    public string GenInvoiceNo(string a, string b)
    {
        DateTime d = Convert.ToDateTime(b);
        int mon = d.Month;
        int yr = d.Year;
        int yr1 = (mon <= 3) ? yr - 1 : yr;
        int yr2 = (mon <= 3) ? yr : yr + 1;

        return "RR/PV/" + yr1 + "-" + yr2 + "/" + a.PadLeft(4, '0');
    }

    /* ================================================================
       BANK VALIDATE — 0 = ok, 1 = error
    ================================================================ */
    public int bankvalidate()
    {
        if (paymentmode.Value.Trim() != "Online") return 0;

        if (string.IsNullOrWhiteSpace(pACName.Value) ||
            string.IsNullOrWhiteSpace(pACNo.Value) ||
            ddlBank.SelectedItem == null ||
            ddlBank.SelectedItem.Text.Trim() == "--Select Bank--" ||
            string.IsNullOrWhiteSpace(pBankIFSC.Value))
        {
            return 1;
        }
        return 0;
    }

    /* ================================================================
       INSERT DATA
    ================================================================ */
    public void insertData()
    {
        dt = (DataTable)Session["Data"];

        // Duplicate check
        if (chkDupData(dt.Rows[0]["DataDate"].ToString(),
                       dt.Rows[0]["PName"].ToString(),
                       dt.Rows[0]["AmountPaid"].ToString()) != 0)
        {
            ShowAlert("This payment entry already exists. Duplicate data not saved.");
            return;
        }

        param = new List<SqlParameter>();
        string[] Inv = dt.Rows[0]["No"].ToString().Split('/');

        param.Add(new SqlParameter("@No", Inv[3]));
        param.Add(new SqlParameter("@MPVNo", dt.Rows[0]["MPVNo"].ToString()));
        param.Add(new SqlParameter("@DataDate", Convert.ToDateTime(dt.Rows[0]["DataDate"].ToString()).ToString("dd-MMM-yyyy")));
        param.Add(new SqlParameter("@PName", dt.Rows[0]["PName"].ToString()));
        param.Add(new SqlParameter("@AmountPaid", dt.Rows[0]["AmountPaid"].ToString()));
        param.Add(new SqlParameter("@PaymentMode", dt.Rows[0]["PaymentMode"].ToString()));
        param.Add(new SqlParameter("@Transaction", dt.Rows[0]["Transaction"].ToString()));
        param.Add(new SqlParameter("@Bank", dt.Rows[0]["Bank"].ToString()));
        param.Add(new SqlParameter("@OperatorName", Session["User"].ToString()));
        param.Add(new SqlParameter("@Entry_Date", DateTime.Now.ToString("dd-MMM-yyyy")));

        string q = "insert into prabha.Purchase_Payment_Info([No],MPVNo,DataDate,PName,AmountPaid,PaymentMode,[Transaction],Bank,OperatorName,EntryDate) " +
                   "values(@No,@MPVNo,@DataDate,@PName,@AmountPaid,@PaymentMode,@Transaction,@Bank,@OperatorName,@Entry_Date)";

        dac = new DataAccessLayer();
        int msg = dac.update(q, param);

        if (msg > 0)
        {
            // Update bank details if Online
            if (dt.Rows[0]["PaymentMode"].ToString() == "Online")
            {
                string source = dt.Rows[0]["PName"].ToString();
                string[] sep = new string[] { " (Mobile No.: " };
                var parts = source.Split(sep, StringSplitOptions.None);
                string Pname = parts[0];
                string PMobile = parts[1].TrimEnd(')');

                param = new List<SqlParameter>();
                param.Add(new SqlParameter("@Bank_Name", ddlBank.SelectedItem.Text.Trim()));
                param.Add(new SqlParameter("@Account_No", pACNo.Value.Trim()));
                param.Add(new SqlParameter("@Account_Name", pACName.Value.Trim()));
                param.Add(new SqlParameter("@IFSC_Code", pBankIFSC.Value.Trim()));
                param.Add(new SqlParameter("@Party_Name", Pname));
                param.Add(new SqlParameter("@Party_Mobile", PMobile));

                q = "update prabha.Purchase_Party_Info set Bank_Name=@Bank_Name, Account_No=@Account_No, " +
                    "Account_Name=@Account_Name, IFSC_Code=@IFSC_Code " +
                    "where Party_Name=@Party_Name and Party_Mobile=@Party_Mobile";

                dac = new DataAccessLayer();
                dac.update(q, param);
            }

            ShowSuccess("Payment submitted successfully!");
            Session["Data"] = null;
        }
        else
        {
            ShowAlert("Error saving payment. Please try again.");
        }
    }

    /* ================================================================
       CHECK DUPLICATE
    ================================================================ */
    public int chkDupData(string DDate, string PN, string Am)
    {
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@DataDate", Convert.ToDateTime(DDate).ToString("dd-MMM-yyyy")));
        param.Add(new SqlParameter("@PName", PN));
        param.Add(new SqlParameter("@AmountPaid", Am));

        string q = "select * from prabha.Purchase_Payment_Info " +
                   "where DataDate=@DataDate and PName=@PName and AmountPaid=@AmountPaid";

        dac = new DataAccessLayer();
        DataTable dtOut = dac.GetDataTable(q, param);

        return (dtOut.Rows.Count > 0) ? 1 : 0;
    }

    /* ================================================================
       LINK BUTTON — PARTY PAYMENT HISTORY
    ================================================================ */
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@PName", ddlParty.SelectedItem.Text.Trim()));

        string q = "select * from prabha.Purchase_Payment_Info where PName=@PName order by DataDate";
        dac = new DataAccessLayer();
        DataTable dtPayment = dac.GetDataTable(q, param);

        StringBuilder htmlTable = new StringBuilder();

        if (dtPayment.Rows.Count == 0)
        {
            htmlTable.Append("<table class='table' cellspacing='0'>");
            htmlTable.Append("<tr><td align='center'>No payment data found for this party.</td></tr></table>");
        }
        else
        {
            htmlTable.Append("<table class='table table-bordered' id='dataTable' cellspacing='0'>");
            htmlTable.Append("<thead><tr><th>Sl. No.</th><th>Voucher No. &amp; Date</th>" +
                             "<th>Party Name</th><th>Amount Paid</th><th>Payment Mode</th><th></th></tr></thead><tbody>");

            for (int i = 0; i < dtPayment.Rows.Count; i++)
            {
                string INVNo = GenInvoiceNo(dtPayment.Rows[i]["No"].ToString(), dtPayment.Rows[i]["DataDate"].ToString());
                htmlTable.Append("<tr>");
                htmlTable.Append("<td>" + (i + 1) + "</td>");
                htmlTable.Append("<td>" + INVNo + ", " + Convert.ToDateTime(dtPayment.Rows[i]["DataDate"].ToString()).ToString("dd/MM/yyyy") + "</td>");
                htmlTable.Append("<td>" + dtPayment.Rows[i]["PName"] + "</td>");
                htmlTable.Append("<td>" + dtPayment.Rows[i]["AmountPaid"] + "</td>");
                htmlTable.Append("<td>" + dtPayment.Rows[i]["PaymentMode"] + "</td>");
                htmlTable.Append("<td><a href='PV.aspx?ID=" + dtPayment.Rows[i]["ID"] + "' target='_blank'>View Voucher</a></td>");
                htmlTable.Append("</tr>");
            }
            htmlTable.Append("</tbody></table>");
        }

        DBDataPlaceHolder.Controls.Clear();
        DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
    }
}
