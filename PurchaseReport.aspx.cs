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

public partial class PurchaseReport : System.Web.UI.Page
{
    DataTable dt;
    DataTable dtMain;
    List<SqlParameter> param;
    DataAccessLayer dac;

    /* ================================================================
       PAGE LOAD
    ================================================================ */
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            fdate.Attributes["type"] = "date";
            tdate.Attributes["type"] = "date";

            Party();
            Session["Data"] = null;
            Session["DataMain"] = null;
            Session["Export"] = null;
        }
    }

    /* ================================================================
       GENERATE REPORT BUTTON
    ================================================================ */
    public void btnContinue_ServerClick(object sender, EventArgs e)
    {
        string err = ServerValidate();
        if (err != "")
        {
            ShowAlert(err);
            return;
        }
        checkData();
    }

    /* ================================================================
       SERVER-SIDE VALIDATION
    ================================================================ */
    private string ServerValidate()
    {
        if (string.IsNullOrWhiteSpace(fdate.Value))
            return "Please select From Date.";

        if (string.IsNullOrWhiteSpace(tdate.Value))
            return "Please select To Date.";

        DateTime d1, d2;
        if (!DateTime.TryParse(fdate.Value.Trim(), out d1))
            return "From Date is invalid.";

        if (!DateTime.TryParse(tdate.Value.Trim(), out d2))
            return "To Date is invalid.";

        if (d1 > d2)
            return "From Date cannot be after To Date.";

        return "";
    }

    /* ================================================================
       SHOW / HIDE ALERT (client-side)
    ================================================================ */
    private void ShowAlert(string msg, bool success = false)
    {
        string cls = success ? "alert-success-custom" : "alert-danger-custom";
        string js = "var el=document.getElementById('topAlert');" +
                     "el.style.display='block';" +
                     "el.className='alert-custom " + cls + "';" +
                     "el.innerText='" + msg.Replace("'", "\\'") + "';" +
                     "window.scrollTo(0,0);";
        ClientScript.RegisterStartupScript(this.GetType(), "ShowAlert", js, true);
    }

    /* ================================================================
       BUILD REPORT TABLE
    ================================================================ */
    public void checkData()
    {
        DataTable DtData = new DataTable();
        param = new List<SqlParameter>();

        string d1 = Convert.ToDateTime(fdate.Value.Trim()).ToString("dd-MMM-yyyy");
        string d2 = Convert.ToDateTime(tdate.Value.Trim()).ToString("dd-MMM-yyyy");

        string q;

        if (sPartyName.SelectedItem.Text.Trim() == "--Select One--")
        {
            param.Add(new SqlParameter("@DataDate1", d1));
            param.Add(new SqlParameter("@DataDate2", d2));
            q = "select ID,[No],MPurNo,DataDate,PartyName,BrokerName,SaudaNo,SaudaDate,TruckNo,KantaNo,Advance " +
                "from prabha.Purchase_Master_Data " +
                "where DataDate>=@DataDate1 and DataDate<=@DataDate2 " +
                "order by [No],DataDate";
        }
        else
        {
            param.Add(new SqlParameter("@DataDate1", d1));
            param.Add(new SqlParameter("@DataDate2", d2));
            param.Add(new SqlParameter("@PartyName", sPartyName.SelectedItem.Text.Trim()));
            q = "select ID,[No],MPurNo,DataDate,PartyName,BrokerName,SaudaNo,SaudaDate,TruckNo,KantaNo,Advance " +
                "from prabha.Purchase_Master_Data " +
                "where DataDate>=@DataDate1 and DataDate<=@DataDate2 and PartyName=@PartyName " +
                "order by [No],DataDate";
        }

        dac = new DataAccessLayer();
        DtData = dac.GetDataTable(q, param);

        if (DtData.Rows.Count == 0)
        {
            ShowAlert("No data found for the selected date range / party.", false);
            DBDataPlaceHolder.Controls.Clear();
            DBDataPlaceHolder.Controls.Add(new Literal
            {
                Text = "<p style='text-align:center;color:#94a3b8;padding:30px;'>No records found.</p>"
            });
            Session["Export"] = null;
            return;
        }

        DtData.Columns.Add("Amount", typeof(string));
        DtData.Columns.Add("CD", typeof(string));
        DtData.Columns.Add("GK", typeof(string));

        StringBuilder html = new StringBuilder();

        html.Append("<table class='table table-bordered' id='dataTable' cellspacing='0'>");

        // Company header
        html.Append("<tr><td colspan='12' align='center'>" +
            "<span style='display:table-cell;vertical-align:top;'>" +
            "<img src='http://prabhasoftware.com/Rashmi Rice Logo (1).png' height='80px'/></span>" +
            "<span style='display:table-cell;vertical-align:top;padding-left:10px;'>" +
            "<span style='font-size:16pt;font-weight:bold;'>Rashmi Rice Mills Pvt. Ltd.</span><br/>" +
            "<span style='font-size:8pt;'>Daniyawan Chandi Road, Hasanpur, Patna- 801304<br/>" +
            "Mob.: 9304052349, 9334280057 | Email: srirajbhog@gmail.com<br/>" +
            "CIN: U15312BR2014PTC022237 | PAN: AAGCR9497P | GSTIN: 10AAGCR9497P1ZK</span>" +
            "</span></td></tr>");

        html.Append("<tr><td colspan='12' align='center'>" +
            "<span style='font-size:11pt;font-weight:bold;'>PURCHASE &amp; UNLOADING REPORT</span></td></tr>");

        // Period row
        html.Append("<tr><td colspan='12' align='right' style='font-size:11px;color:#64748b;'>" +
            "Period: " + Convert.ToDateTime(fdate.Value).ToString("dd/MM/yyyy") +
            " to " + Convert.ToDateTime(tdate.Value).ToString("dd/MM/yyyy") +
            (sPartyName.SelectedItem.Text.Trim() != "--Select One--"
                ? " | Party: " + sPartyName.SelectedItem.Text.Trim()
                : "") +
            "</td></tr>");

        // Table header
        html.Append("<tr style='background:#16a34a;color:white;font-weight:bold;'>" +
            "<td>Sl. No.</td><td>Invoice No. &amp; Date</td><td>Party Name</td>" +
            "<td>Broker Name</td><td>Sauda No. &amp; Date</td><td>Truck No.</td>" +
            "<td>Kanta No.</td><td>Freight Adv.</td><td>CD</td><td>GK</td>" +
            "<td>Amount</td><td></td></tr><tbody>");

        double totalAmount = 0;
        double totalCD = 0;
        double totalGK = 0;
        double totalAdvance = 0;

        for (int i = 0; i < DtData.Rows.Count; i++)
        {
            string INVNo = GenInvoiceNo(DtData.Rows[i]["No"].ToString(), DtData.Rows[i]["DataDate"].ToString());
            string[] calc = dataDisplay(DtData.Rows[i]["ID"].ToString()).Split('-');

            double rowAmt = (calc[0].Trim() == "") ? 0 : Math.Round(Convert.ToDouble(calc[0]), 0);
            double rowCD = Math.Round(Convert.ToDouble(calc[1]), 0);
            double rowGK = Math.Round(Convert.ToDouble(calc[2]), 0);
            double rowAdv = Convert.ToDouble(DtData.Rows[i]["Advance"].ToString());

            DtData.Rows[i]["Amount"] = rowAmt.ToString();
            DtData.Rows[i]["CD"] = rowCD.ToString();
            DtData.Rows[i]["GK"] = rowGK.ToString();

            totalAmount += rowAmt;
            totalCD += rowCD;
            totalGK += rowGK;
            totalAdvance += rowAdv;

            html.Append("<tr>");
            html.Append("<td>" + (i + 1) + "</td>");
            html.Append("<td>" + INVNo + ", " + Convert.ToDateTime(DtData.Rows[i]["DataDate"].ToString()).ToString("dd/MM/yyyy") + "</td>");
            html.Append("<td>" + DtData.Rows[i]["PartyName"] + "</td>");
            html.Append("<td>" + DtData.Rows[i]["BrokerName"] + "</td>");
            html.Append("<td>" + DtData.Rows[i]["SaudaNo"] + ", " + Convert.ToDateTime(DtData.Rows[i]["SaudaDate"].ToString()).ToString("dd/MM/yyyy") + "</td>");
            html.Append("<td>" + DtData.Rows[i]["TruckNo"] + "</td>");
            html.Append("<td>" + DtData.Rows[i]["KantaNo"] + "</td>");
            html.Append("<td style='text-align:right;'>" + rowAdv.ToString("N0") + "</td>");
            html.Append("<td style='text-align:right;'>" + rowCD.ToString("N0") + "</td>");
            html.Append("<td style='text-align:right;'>" + rowGK.ToString("N0") + "</td>");
            html.Append("<td style='text-align:right;font-weight:600;'>" + rowAmt.ToString("N0") + "</td>");
            html.Append("<td><a href='PurchaseBill.aspx?ID=" + DtData.Rows[i]["ID"] + "' target='_blank' class='btn btn-xs' style='background:#16a34a;color:white;border-radius:6px;padding:4px 10px;font-size:11px;'>Bill</a></td>");
            html.Append("</tr>");
        }

        // Totals row
        html.Append("<tr class='total-row'>");
        html.Append("<td colspan='7' style='text-align:right;'><b>Total (" + DtData.Rows.Count + " records)</b></td>");
        html.Append("<td style='text-align:right;'><b>" + totalAdvance.ToString("N0") + "</b></td>");
        html.Append("<td style='text-align:right;'><b>" + totalCD.ToString("N0") + "</b></td>");
        html.Append("<td style='text-align:right;'><b>" + totalGK.ToString("N0") + "</b></td>");
        html.Append("<td style='text-align:right;'><b>" + totalAmount.ToString("N0") + "</b></td>");
        html.Append("<td></td></tr>");

        html.Append("</tbody></table>");

        Session["Export"] = DtData;

        DBDataPlaceHolder.Controls.Clear();
        DBDataPlaceHolder.Controls.Add(new Literal { Text = html.ToString() });
    }

    /* ================================================================
       CALCULATE AMOUNT FOR EACH ROW
    ================================================================ */
    public string dataDisplay(string id)
    {
        double FAmount = 0;
        double LCD = 0;
        double LGK = 0;

        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@ID", id));
        dac = new DataAccessLayer();
        DataTable dtM = dac.GetDataTable("select * from [prabha].[Purchase_Master_Data] where ID=@ID", param);

        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@ID", id));
        DataTable dtI = dac.GetDataTable("select * from [prabha].[Purchase_Item_Info] where Master_ID=@ID", param);

        if (dtI == null || dtI.Rows.Count == 0 || dtM == null || dtM.Rows.Count == 0)
            return "0-0-0";

        int tBags = 0;
        double tQuantity = 0;
        double tAmount = 0;
        double LClaim = 0;

        for (int i = 0; i < dtI.Rows.Count; i++)
        {
            DataRow r = dtI.Rows[i];

            double rate = Convert.ToDouble(r["Rate"].ToString());
            double avgWt = Convert.ToDouble(r["AvgWt"].ToString());
            double fresh = Convert.ToDouble(r["FreshQuantity"].ToString());
            double moist = Convert.ToDouble(r["Moisture"].ToString());

            double am = Math.Round(fresh * avgWt * rate, 2);

            // Khakhri
            double KhRate = GetAdjRate(rate, Convert.ToDouble(r["KhakhriPer"].ToString()), 2);
            double KhAmount = Math.Round(Convert.ToDouble(r["KhakhriBags"].ToString()) * avgWt * KhRate, 2);

            // Mitti
            double MRate = GetAdjRate(rate, Convert.ToDouble(r["MittiPer"].ToString()), 0);
            double MAmount = Math.Round(Convert.ToDouble(r["MittiBags"].ToString()) * avgWt * MRate, 2);

            // Daagi
            double DRate = GetAdjRate(rate, Convert.ToDouble(r["DaagiPer"].ToString()), 0);
            double DAmount = Math.Round(Convert.ToDouble(r["DaagiBags"].ToString()) * avgWt * DRate, 2);

            // Mix
            double DMixRate = GetAdjRate(rate, Convert.ToDouble(r["MixPer"].ToString()), 0);
            double DMixAmount = Math.Round(Convert.ToDouble(r["MixBags"].ToString()) * avgWt * DMixRate, 2);

            // Other
            double ORate = GetAdjRate(rate, Convert.ToDouble(r["OtherPer"].ToString()), 0);
            double OAmount = Math.Round(Convert.ToDouble(r["OtherBags"].ToString()) * avgWt * ORate, 2);

            double rowTotal = am + KhAmount + MAmount + DAmount + DMixAmount + OAmount;

            tBags += Convert.ToInt32(r["FreshQuantity"].ToString())
                   + Convert.ToInt32(r["KhakhriBags"].ToString())
                   + Convert.ToInt32(r["MittiBags"].ToString())
                   + Convert.ToInt32(r["DaagiBags"].ToString())
                   + Convert.ToInt32(r["MixBags"].ToString())
                   + Convert.ToInt32(r["OtherBags"].ToString());

            tQuantity = Math.Round(tBags * avgWt, 2);
            tAmount += rowTotal;

            // Moisture claim
            string pName = dtM.Rows[0]["PartyName"].ToString();
            int lt = (pName == "SHIVAM BHANDAR (SUBODH JEE (Mobile No.: 9334280057)" ||
                      pName == "PRACHI TRADERS (MASURHI) (Mobile No.: 9334280057)" ||
                      pName == "Sankat Mochan Traders(Kunil jee) jahanabad (Mobile No.: 9334280057)") ? 18 : 17;

            if (moist > lt)
                LClaim += Math.Round(rowTotal * (moist - lt) / 100, 2);

            // Last item — compute final
            if (i == dtI.Rows.Count - 1)
            {
                LCD = Math.Round(tAmount * Convert.ToDouble(dtM.Rows[0]["CD"].ToString()) / 100);
                LGK = Math.Round(tQuantity / 1000 * 25, 2);
                if (LGK < 100) LGK = 100;

                double Frt = Convert.ToDouble(dtM.Rows[0]["FreightOwn"].ToString());
                double PAmount = tAmount - LCD - LClaim - Frt;

                double LAdvance = Convert.ToDouble(dtM.Rows[0]["Advance"].ToString());
                double OAdvance = (LAdvance == 0) ? LGK : LAdvance;

                double Brok = Convert.ToDouble(dtM.Rows[0]["Brokerage"].ToString());
                FAmount = PAmount - OAdvance - Brok;
            }
        }

        return FAmount + "-" + LCD + "-" + LGK;
    }

    // Helper: adjusted rate based on percentage threshold
    private double GetAdjRate(double rate, double per, double threshold)
    {
        if (per <= threshold) return rate;
        return Math.Round(rate - (rate * (per - threshold) / 100), 2);
    }

    /* ================================================================
       BIND PARTY DROPDOWN
    ================================================================ */
    public void Party()
    {
        param = new List<SqlParameter>();
        string q = "select distinct PartyName from prabha.Purchase_Master_Data order by PartyName";
        dac = new DataAccessLayer();
        dt = dac.GetDataTable(q, param);

        sPartyName.DataSource = dt;
        sPartyName.DataTextField = "PartyName";
        sPartyName.DataValueField = "PartyName";
        sPartyName.DataBind();
        sPartyName.Items.Insert(0, "--Select One--");
    }

    /* ================================================================
       GENERATE INVOICE NO — PURCHASE
    ================================================================ */
    public string GenInvoiceNo(string a, string b)
    {
        DateTime d = Convert.ToDateTime(b);
        int mon = d.Month, yr = d.Year;
        int yr1 = (mon <= 3) ? yr - 1 : yr;
        int yr2 = (mon <= 3) ? yr : yr + 1;
        return "RR/PUR/" + yr1 + "-" + yr2 + "/" + a.PadLeft(4, '0');
    }

    /* ================================================================
       GENERATE INVOICE NO — PAYMENT VOUCHER
    ================================================================ */
    public string GenInvoiceNoSale(string a, string b)
    {
        DateTime d = Convert.ToDateTime(b);
        int mon = d.Month, yr = d.Year;
        int yr1 = (mon <= 3) ? yr - 1 : yr;
        int yr2 = (mon <= 3) ? yr : yr + 1;
        return "RR/PV/" + yr1 + "-" + yr2 + "/" + a.PadLeft(4, '0');
    }

    /* ================================================================
       EXPORT TO EXCEL
    ================================================================ */
    public void Export_ServerClick(object sender, EventArgs e)
    {
        string err = ServerValidate();
        if (err != "")
        {
            ShowAlert(err);
            return;
        }

        // If report not yet generated, generate first
        if (Session["Export"] == null)
            checkData();

        if (Session["Export"] == null)
        {
            ShowAlert("No data to export. Please generate the report first.");
            return;
        }

        DataTable EData = (DataTable)Session["Export"];

        // Also pull payment data for the same period
        param = new List<SqlParameter>();
        string d1 = Convert.ToDateTime(fdate.Value.Trim()).ToString("dd-MMM-yyyy");
        string d2 = Convert.ToDateTime(tdate.Value.Trim()).ToString("dd-MMM-yyyy");
        string q;

        if (sPartyName.SelectedItem.Text.Trim() == "--Select One--")
        {
            param.Add(new SqlParameter("@DataDate1", d1));
            param.Add(new SqlParameter("@DataDate2", d2));
            q = "select ID,[No],MPVNo as MPurNo,DataDate,PName as PartyName,PaymentMode as BrokerName," +
                "'' as SaudaNo,convert(smalldatetime,'01/01/1990') as SaudaDate," +
                "Bank as TruckNo,[Transaction] as KantaNo," +
                "CAST('0' AS DECIMAL(10,2)) AS Advance," +
                "convert(varchar,AmountPaid) as Amount,'' as CD,'' as GK " +
                "from prabha.[Purchase_Payment_Info] " +
                "where DataDate>=@DataDate1 and DataDate<=@DataDate2 order by DataDate";
        }
        else
        {
            param.Add(new SqlParameter("@DataDate1", d1));
            param.Add(new SqlParameter("@DataDate2", d2));
            param.Add(new SqlParameter("@PartyName", sPartyName.SelectedItem.Text.Trim()));
            q = "select ID,[No],MPVNo as MPurNo,DataDate,PName as PartyName,PaymentMode as BrokerName," +
                "'' as SaudaNo,convert(smalldatetime,'01/01/1990') as SaudaDate," +
                "Bank as TruckNo,[Transaction] as KantaNo," +
                "CAST('0' AS DECIMAL(10,2)) AS Advance," +
                "convert(varchar,AmountPaid) as Amount,'' as CD,'' as GK " +
                "from prabha.[Purchase_Payment_Info] " +
                "where DataDate>=@DataDate1 and DataDate<=@DataDate2 and PName=@PartyName order by DataDate";
        }

        dac = new DataAccessLayer();
        DataTable DtDataF = dac.GetDataTable(q, param);

        EData.Merge(DtDataF);
        EData.DefaultView.Sort = "DataDate";
        EData = EData.DefaultView.ToTable();

        ExporttoExcel(EData);
    }

    /* ================================================================
       EXPORT HELPER
    ================================================================ */
    private void ExporttoExcel(DataTable table)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=PurchaseReport.xls");
        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        HttpContext.Current.Response.Write("<font style='font-size:10.0pt;font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt;font-family:Calibri;background:white;'><TR>");

        // Headers
        string[] headers = {
            "Sl. No.", "Purchase/Payment No.", "Manual No.", "Invoice/Payment Date",
            "Party Name", "Sauda No. & Date", "Vehicle No.", "Kanta No.",
            "Freight Adv.", "CD", "GK", "Bill Amount", "Paid Amount"
        };
        foreach (string h in headers)
            HttpContext.Current.Response.Write("<Td><B>" + h + "</B></Td>");

        HttpContext.Current.Response.Write("</TR>");

        int i = 0;
        foreach (DataRow row in table.Rows)
        {
            i++;
            bool isPayment = (row["SaudaNo"].ToString() == "");
            string InvNo = isPayment
                ? GenInvoiceNoSale(row["No"].ToString(), row["DataDate"].ToString())
                : GenInvoiceNo(row["No"].ToString(), row["DataDate"].ToString());

            HttpContext.Current.Response.Write("<TR>");
            HttpContext.Current.Response.Write("<Td>" + i + "</Td>");
            HttpContext.Current.Response.Write("<Td>" + InvNo + "</Td>");
            HttpContext.Current.Response.Write("<Td>" + row["MPurNo"] + "</Td>");
            HttpContext.Current.Response.Write("<Td>" + Convert.ToDateTime(row["DataDate"].ToString()).ToString("dd/MM/yyyy") + "</Td>");
            HttpContext.Current.Response.Write("<Td>" + row["PartyName"] + "</Td>");

            if (isPayment)
            {
                HttpContext.Current.Response.Write("<Td colspan='3'>" + row["BrokerName"] + " (" + row["KantaNo"] + ")</Td>");
                HttpContext.Current.Response.Write("<Td colspan='3'>" + row["TruckNo"] + "</Td>");
                HttpContext.Current.Response.Write("<Td>&nbsp;</Td>");
                HttpContext.Current.Response.Write("<Td>" + Math.Round(Convert.ToDouble(row["Amount"].ToString()), 2) + "</Td>");
            }
            else
            {
                HttpContext.Current.Response.Write("<Td>" + row["SaudaNo"] + ", " + Convert.ToDateTime(row["SaudaDate"].ToString()).ToString("dd/MM/yyyy") + "</Td>");
                HttpContext.Current.Response.Write("<Td>" + row["TruckNo"] + "</Td>");
                HttpContext.Current.Response.Write("<Td>" + row["KantaNo"] + "</Td>");
                HttpContext.Current.Response.Write("<Td>" + Math.Round(Convert.ToDouble(row["Advance"].ToString()), 0) + "</Td>");
                HttpContext.Current.Response.Write("<Td>" + Math.Round(Convert.ToDouble(row["CD"].ToString()), 2) + "</Td>");
                HttpContext.Current.Response.Write("<Td>" + Math.Round(Convert.ToDouble(row["GK"].ToString()), 2) + "</Td>");
                HttpContext.Current.Response.Write("<Td>" + Math.Round(Convert.ToDouble(row["Amount"].ToString()), 2) + "</Td>");
                HttpContext.Current.Response.Write("<Td>&nbsp;</Td>");
            }

            HttpContext.Current.Response.Write("</TR>");
        }

        HttpContext.Current.Response.Write("</Table></font>");
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }
}
