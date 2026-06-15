
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
            //pMobile.Attributes["type"] = "number";
            
            amountpaid.Attributes["type"] = "number";
            amountpaid.Attributes["step"] = ".01";

            pACNo.Attributes["type"] = "number";
            pACNo.Attributes["step"] = "1";
            
            lblOSB.Text = "0";
            bindParty();
            Session["Data"] = null;
            
            dataDisplay();
        }
    }
    public void btnContinue_ServerClick(object sender, EventArgs e)
    {
        if (bankvalidate() == 0)
        {
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
            myrow[0] = GenInvoiceNo();
            myrow[1] = Convert.ToDateTime(sdate.Value.Trim()).ToString("dd-MMM-yyyy");
            myrow[2] = ddlParty.SelectedItem.Text.Trim();

            myrow[3] = amountpaid.Value.Trim();
            myrow[4] = paymentmode.Value.Trim();
            myrow[5] = transaction.Value.Trim();
            if (paymentmode.Value.Trim() == "Online")
            {
                myrow[6] = pACName.Value.Trim() + " (" + pACNo.Value.Trim() + ")";
            }
            else
            {
                myrow[6] = " ";
            }
            myrow[7] = pvNo.Value.Trim();
            dt.Rows.Add(myrow);
            Session["Data"] = dt;
            dataDisplay();
        }
        else
        {
            script = "alert('Please fill bank details!!');";
            ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
        }
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
            insertData();

            dataDisplay();
            CallPrint("prntContent");
        }
        
    }

    
    protected void ddlParty_SelectedIndexChanged(object sender, EventArgs e)
    {
        Party();
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
            dt=(DataTable)Session["Data"];
            htmlTable = new StringBuilder();
            htmlTable.Append("<table runat='server' style='font-size:10pt; noWrap; min-width: 600px; min-height:400px;' id='printTable' cellspacing='0' border='1px'>");

            htmlTable.Append("<tr><td colspan='2' align='center'><span style='display:table-cell; vertical-align:top;'><img src='http://prabhasoftware.com/Rashmi Rice Logo (1).png' height='100px'/></span><span style='display:table-cell; vertical-align:top;'><span style='font-size:16pt; font-weight:bold;'> Rashmi Rice Mills Pvt. Ltd. </span></br><span style='font-size:8pt;'>Daniyawan Chandi Road, Hasanpur, Patna- 801304 </br>Mob.: 9304052349, 9334280057</br>Email: srirajbhog@gmail.com</br>CIN: U15312BR2014PTC022237</br>PAN No.: AAGCR9497P</br>GSTIN: 10AAGCR9497P1ZK</span></span></td></tr>");
            htmlTable.Append("<tr><td colspan='2' align='center'><span style='font-size:10pt; font-weight:bold;'> PAYMENT VOUCHER </span></td></tr>");
            if (dt.Rows[0]["MPVNo"].ToString() == "")
            {
                htmlTable.Append("<tr><td colspan='2' align='right'>Voucher No.: <b>" + dt.Rows[0]["No"].ToString() + "</b></td></tr>");
            }
            else
            {
                htmlTable.Append("<tr><td colspan='2' align='right'>Voucher No.: <b>" + dt.Rows[0]["No"].ToString() + "</b></br>Manual Voucher No.: <b>" + dt.Rows[0]["MPVNo"].ToString() + "</b></td></tr>");
            }
            htmlTable.Append("<tr><td colspan='2' align='right'>Payment Date: " + dt.Rows[0]["DataDate"].ToString() + "</td></tr>");
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
                htmlTable.Append("<tr><td colspan='2' align='left'>On Account Of: " + dt.Rows[0]["Bank"].ToString() + "</td></tr>");
                htmlTable.Append("<tr><td colspan='2' align='left'>Payment Reference No.: " + dt.Rows[0]["Transaction"].ToString() + "</td></tr>");
            }
            htmlTable.Append("<tr><td width='50%' align='left'></td><td align='center'><b>For Rashmi Rice Mills Pvt. Ltd.</br></br>Authorised Signatory</b></td></tr>");
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
    public void bindParty()
    {
        DataTable dtP = new DataTable();
        string q = "";
        param = new List<SqlParameter>();//Emp_Id
        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
        q = "select concat(Party_Name, ' (Mobile No.: ',Party_Mobile,')') as PartyName,Bank_Name,Account_No,Account_Name,IFSC_Code from prabha.Purchase_Party_Info where CompanyID=@CompanyID order by PartyName";
        dac = new DataAccessLayer();
        dtP = dac.GetDataTable(q, param);

        ddlParty.DataSource = dtP;
        ddlParty.DataTextField = "PartyName";
        ddlParty.DataValueField = "PartyName";
        ddlParty.DataBind();
    }
    public void Party()
    {
        DataTable dtPBank = new DataTable();
        string q = "";
        param = new List<SqlParameter>();//Emp_Id
        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
        
        string source = ddlParty.SelectedItem.Text.Trim();
        string[] stringSeparators = new string[] { " (Mobile No.: " };
        var result = source.Split(stringSeparators, StringSplitOptions.None);

        string Pname = result[0];
        string PMobile = result[1].Substring(0, (result[1].Length - 1));


        param.Add(new SqlParameter("@Party_Name", Pname));
        param.Add(new SqlParameter("@Party_Mobile", PMobile));
        q = "select Bank_Name,Account_No,Account_Name,IFSC_Code from prabha.Purchase_Party_Info where CompanyID=@CompanyID and Party_Name=@Party_Name and Party_Mobile=@Party_Mobile";
        dac = new DataAccessLayer();
        dtPBank = dac.GetDataTable(q, param);

        if (dtPBank.Rows[0]["Account_Name"].ToString() == "")
        {
            pACName.Value = "";
            pACName.Disabled = false;
        }
        else
        {
            pACName.Value = dtPBank.Rows[0]["Account_Name"].ToString();
            pACName.Disabled = true;
        }
        if (dtPBank.Rows[0]["Account_No"].ToString() == "")
        {
            pACNo.Value = "";
            pACNo.Disabled = false;
        }
        else
        {
            pACNo.Value = dtPBank.Rows[0]["Account_No"].ToString();
            pACNo.Disabled = true;
        }
        if (dtPBank.Rows[0]["Bank_Name"].ToString() == "")
        {
            ddlBank.SelectedValue = ddlBank.Items.FindByText("--Select Bank--").Value;
            ddlBank.Items.FindByText("--Select Bank--").Selected = true;
            ddlBank.Enabled = true;
        }
        else
        {
            ddlBank.SelectedValue=ddlBank.Items.FindByText(dtPBank.Rows[0]["Bank_Name"].ToString()).Value;
            ddlBank.Enabled = false;
        }
        if (dtPBank.Rows[0]["IFSC_Code"].ToString() == "")
        {
            pBankIFSC.Value = "";
            pBankIFSC.Disabled = false;
        }
        else
        {
            pBankIFSC.Value = dtPBank.Rows[0]["IFSC_Code"].ToString();
            pBankIFSC.Disabled = true;
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
        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));

        param.Add(new SqlParameter("@DataDate1", Convert.ToDateTime(dtFrom).ToString("dd-MMM-yyyy")));
        param.Add(new SqlParameter("@DataDate2", Convert.ToDateTime(dtTo).ToString("dd-MMM-yyyy")));

        q = "select max([No]) from prabha.Purchase_Payment_Info where CompanyID=@CompanyID and DataDate>=@DataDate1 and DataDate<=@DataDate2";
        dac = new DataAccessLayer();
        object test = dac.Scalar(q, param);
        if (test == DBNull.Value)
        {
            invoiceNo = "RR/PV/" + yr1 + "-" + yr2 + "/0001";
        }
        else
        {
            if ((Convert.ToInt32(test) + 1).ToString().Length == 1)
            {
                invoiceNo = "RR/PV/" + yr1 + "-" + yr2 + "/000" + (Convert.ToInt32(test) + 1);
            }
            else if ((Convert.ToInt32(test) + 1).ToString().Length == 2)
            {
                invoiceNo = "RR/PV/" + yr1 + "-" + yr2 + "/00" + (Convert.ToInt32(test) + 1);
            }
            else if ((Convert.ToInt32(test) + 1).ToString().Length == 3)
            {
                invoiceNo = "RR/PV/" + yr1 + "-" + yr2 + "/0" + (Convert.ToInt32(test) + 1);
            }
            else
            {
                invoiceNo = "RR/PV/" + yr1 + "-" + yr2 + "/" + Convert.ToInt32(test) + 1;
            }

        }
        return invoiceNo;
    }
    public int bankvalidate()
    {
        int vd = 0;
        if (paymentmode.Value.Trim() == "Online")
        {
            if (pACName.Value.Trim() == "" || pACNo.Value.Trim() == "" || ddlBank.SelectedItem.Text.Trim() == "--Select Bank--" || pBankIFSC.Value.Trim()=="")
            {
                vd = 1;
            }
        }
        else
        {
            vd = 0;
        }
        return vd;
    }
    public void insertData()
    {
        dt = (DataTable)Session["Data"];

        int chkValid = chkDupData(dt.Rows[0]["DataDate"].ToString(), dt.Rows[0]["PName"].ToString(), dt.Rows[0]["AmountPaid"].ToString());

        if (chkValid == 0)
        {
            int msg = 0;
            string q = "";
            param = new List<SqlParameter>();//Emp_Id

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
            param.Add(new SqlParameter("@Entry_Date", Convert.ToDateTime(System.DateTime.Now).ToString("dd-MMM-yyyy")));

            q = "insert into prabha.Purchase_Payment_Info(CompanyID,[No],MPVNo,DataDate,PName,AmountPaid,PaymentMode,[Transaction],Bank,OperatorName,EntryDate) ";
            q += " values(@No,@MPVNo,@DataDate,@PName,@AmountPaid,@PaymentMode,@Transaction,@Bank,@OperatorName,@Entry_Date)";
            dac = new DataAccessLayer();

            msg = Convert.ToInt32(dac.update(q, param));


            if (msg > 0)
            {
                if (dt.Rows[0]["PaymentMode"].ToString() == "Online")
                {
                    int OutMsg = 0;

                    q = "";
                    param = new List<SqlParameter>();
                    param.Add(new SqlParameter("@Bank_Name", ddlBank.SelectedItem.Text.Trim()));
                    param.Add(new SqlParameter("@Account_No", pACNo.Value.Trim()));
                    param.Add(new SqlParameter("@Account_Name", pACName.Value.Trim()));
                    param.Add(new SqlParameter("@IFSC_Code", pBankIFSC.Value.Trim()));

                    string source = dt.Rows[0]["PName"].ToString();
                    string[] stringSeparators = new string[] { " (Mobile No.: " };
                    var result = source.Split(stringSeparators, StringSplitOptions.None);

                    string Pname = result[0];
                    string PMobile = result[1].Substring(0, (result[1].Length - 1));


                    param.Add(new SqlParameter("@Party_Name", Pname));
                    param.Add(new SqlParameter("@Party_Mobile", PMobile));
                    q = "update prabha.Purchase_Party_Info set Bank_Name=@Bank_Name, Account_No=@Account_No, Account_Name=@Account_Name, IFSC_Code=@IFSC_Code where Party_Name=@Party_Name and Party_Mobile=@Party_Mobile";
                    dac = new DataAccessLayer();
                    OutMsg = dac.update(q, param);
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
    public int chkDupData(string DDate, string PN, string Am)
    {
        int tst = 0;
        DataTable dtOut = new DataTable();
        string q = "";
        param = new List<SqlParameter>();//Emp_Id
        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));

        param.Add(new SqlParameter("@DataDate", Convert.ToDateTime(DDate).ToString("dd-MMM-yyyy")));
        param.Add(new SqlParameter("@PName", PN));
        param.Add(new SqlParameter("@AmountPaid", Am));

        q = "select * from prabha.Purchase_Payment_Info where CompanyID=@CompanyID and DataDate=@DataDate and PName=@PName and AmountPaid=@AmountPaid";
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
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        string q = "";
        param = new List<SqlParameter>();//Emp_Id
        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
        //string source = ddlParty.SelectedItem.Text.Trim();
        //string[] stringSeparators = new string[] { " (Mobile No.: " };
        //var result = source.Split(stringSeparators, StringSplitOptions.None);

        //string Pname = result[0];
        //string PMobile = result[1].Substring(0, (result[1].Length - 1));


        param.Add(new SqlParameter("@PName", ddlParty.SelectedItem.Text.Trim()));
        
        q = "select * from prabha.Purchase_Payment_Info where CompanyID=@CompanyID and PName=@PName order by DataDate";
        dac = new DataAccessLayer();
        DataTable dtPayment = dac.GetDataTable(q, param);
        string INVNo = "";
        StringBuilder htmlTable;
        
        if (dtPayment.Rows.Count<=0)
        {
            htmlTable = new StringBuilder();
            htmlTable.Append("<table class='table' cellspacing='0'>");
            htmlTable.Append("<tr><td align='center'>No Data found...</td></tr></table>");
        }
        else
        {
            htmlTable = new StringBuilder();
            htmlTable.Append("<table class='table table-bordered' id='dataTable' cellspacing='0'>");
            htmlTable.Append("<thead><tr><th>Sl. No.</th><th>Voucher No. & Date</th><th>Party Name</th><th>Amount Paid</th><th>Payment Mode</th><th></th></tr></thead><tbody>");
            for (int i = 0; i < dtPayment.Rows.Count; i++)
            {
                htmlTable.Append("<tr>");
                htmlTable.Append("<td>" + (i + 1) + "</td>");
                INVNo = GenInvoiceNo(dtPayment.Rows[i]["No"].ToString(), dtPayment.Rows[i]["DataDate"].ToString());
                htmlTable.Append("<td>" + INVNo + ", " + Convert.ToDateTime(dtPayment.Rows[i]["DataDate"].ToString()).ToString("dd/MM/yyyy") + "</td>");
                htmlTable.Append("<td>" + dtPayment.Rows[i]["PName"].ToString() + "</td>");
                htmlTable.Append("<td>" + dtPayment.Rows[i]["AmountPaid"].ToString() + "</td>");
                htmlTable.Append("<td>" + dtPayment.Rows[i]["PaymentMode"].ToString() + "</td>");

                htmlTable.Append("<td><a href='PV.aspx?ID=" + dtPayment.Rows[i]["ID"].ToString() + "' target='_blank'>Payment Voucher</a></td></tr>");
            }
            htmlTable.Append("</tbody></table>");
            
        }
        
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