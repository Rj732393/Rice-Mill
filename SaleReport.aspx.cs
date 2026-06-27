using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI.HtmlControls;
using substitute;

public partial class PurchaseUnloading : System.Web.UI.Page
{
    DataTable dt;
    DataTable dtMain;
    DataRow dtRow;
    DataRow rmain;
    List<SqlParameter> param;
    DataAccessLayer dac;

    string script = "";
    DataRow companyRow;

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
        }
    }

    private void LoadCompanyDetails()
    {
        try
        {
            int cid = Session["CompanyID"] != null ? Convert.ToInt32(Session["CompanyID"]) : 0;

            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@CompanyID", cid));

            DataTable dtC = new DataAccessLayer().GetDataTable(
                "SELECT * FROM prabha.Companies WHERE CompanyID=@CompanyID", p);

            if (dtC.Rows.Count > 0)
                companyRow = dtC.Rows[0];
            else
                companyRow = null;
        }
        catch (Exception)
        {
            companyRow = null;
        }
    }

    private string SafeCol(DataRow row, string colName)
    {
        if (row == null) return "";
        if (!row.Table.Columns.Contains(colName)) return "";
        return row[colName] == DBNull.Value ? "" : row[colName].ToString();
    }

    private void ShowAlert(string msg, string type)
    {
        string key = "alert_" + DateTime.Now.Ticks.ToString();
        string script = "$(document).ready(function(){ showAlert('" + msg.Replace("'", "\\'") + "', '" + type + "'); });";
        ClientScript.RegisterStartupScript(this.GetType(), key, script, true);
    }

    private string ValidateDates(string fDate, string tDate)
    {
        if (string.IsNullOrEmpty(fDate))
            return "Please select From Date.";

        DateTime parsedF;
        if (!DateTime.TryParse(fDate, out parsedF))
            return "Please enter a valid From Date.";

        if (string.IsNullOrEmpty(tDate))
            return "Please select To Date.";

        DateTime parsedT;
        if (!DateTime.TryParse(tDate, out parsedT))
            return "Please enter a valid To Date.";

        if (parsedT < parsedF)
            return "To Date cannot be before From Date.";

        return null;
    }

    public void btnContinue_ServerClick(object sender, EventArgs e)
    {
        try
        {
            string errorMsg = ValidateDates(
                fdate.Value.Trim(),
                tdate.Value.Trim()
            );

            if (!string.IsNullOrEmpty(errorMsg))
            {
                ShowAlert(errorMsg, "error");
                return;
            }

            checkData();
        }
        catch (Exception ex)
        {
            ShowAlert("Error generating report: " + ex.Message, "error");
        }
    }

    public void Party()
    {
        try
        {
            dt = new DataTable();
            param = new List<SqlParameter>();

            string q = "select distinct PartyName from prabha.Sale_Master_Data order by PartyName";
            dac = new DataAccessLayer();
            dt = dac.GetDataTable(q, param);

            sPartyName.DataSource = dt;
            sPartyName.DataTextField = "PartyName";
            sPartyName.DataValueField = "PartyName";
            sPartyName.DataBind();
            sPartyName.Items.Insert(0, "--Select One--");
        }
        catch (Exception ex)
        {
            ShowAlert("Error loading party list: " + ex.Message, "error");
        }
    }

    public void checkData()
    {
        try
        {
            LoadCompanyDetails();

            DataTable DtData = new DataTable();
            string q = "";
            param = new List<SqlParameter>();

            string fDate = Convert.ToDateTime(fdate.Value.Trim()).ToString("dd-MMM-yyyy");
            string tDate = Convert.ToDateTime(tdate.Value.Trim()).ToString("dd-MMM-yyyy");

            if (sPartyName.SelectedItem.Text.Trim() == "--Select One--")
            {
                param.Add(new SqlParameter("@DataDate1", fDate));
                param.Add(new SqlParameter("@DataDate2", tDate));
                q = "select ID,[No],ManualInvoice,DataDate,PartyName,BOrderNo,BOrderDate,DespNo,DespDate,DespVNo,Destination from prabha.Sale_Master_Data where DataDate>=@DataDate1 and DataDate<=@DataDate2 order by [No],DataDate";
            }
            else
            {
                param.Add(new SqlParameter("@DataDate1", fDate));
                param.Add(new SqlParameter("@DataDate2", tDate));
                param.Add(new SqlParameter("@PartyName", sPartyName.SelectedItem.Text.Trim()));
                q = "select ID,[No],ManualInvoice,DataDate,PartyName,BOrderNo,BOrderDate,DespNo,DespDate,DespVNo,Destination from prabha.Sale_Master_Data where DataDate>=@DataDate1 and DataDate<=@DataDate2 and PartyName=@PartyName order by [No],DataDate";
            }

            dac = new DataAccessLayer();
            DtData = dac.GetDataTable(q, param);
            DtData.Columns.Add("Amount", typeof(string));
            DtData.Columns.Add("CD", typeof(string));

            string compName = SafeCol(companyRow, "CompanyName");
            if (string.IsNullOrWhiteSpace(compName)) compName = "Company";
            string compAddress = (SafeCol(companyRow, "Address") + ", " + SafeCol(companyRow, "City") + ", " + SafeCol(companyRow, "State")).Trim(',', ' ');
            string compPhone = SafeCol(companyRow, "Phone");
            string compEmail = SafeCol(companyRow, "Email");
            string compCIN = SafeCol(companyRow, "CIN");
            string compPAN = SafeCol(companyRow, "PAN");
            string compGST = SafeCol(companyRow, "GSTNumber");

            StringBuilder htmlTable = new StringBuilder();
            string INVNo = "";
            htmlTable.Append("<table class='table table-bordered' id='dataTable' cellspacing='0'>");
            htmlTable.Append("<tr><td colspan='9' align='center'><span style='display:table-cell; vertical-align:top;'><span style='font-size:16pt; font-weight:bold;'> " + compName + " </span></br><span style='font-size:8pt;'>" + compAddress + " </br>Mob.: " + compPhone + "</br>Email: " + compEmail + "</br>CIN: " + compCIN + "</br>PAN No.: " + compPAN + "</br>GSTIN: " + compGST + "</span></span></td></tr>");
            htmlTable.Append("<tr><td colspan='9' align='center'><span style='font-size:10pt; font-weight:bold;'> SALE REPORT </span></td></tr>");
            htmlTable.Append("<tr><td>Sl. No.</td><td>Invoice No. & Date</td><td>Party Name</td><td>Buyer's Order No. & Date</td><td>Despatch Doc No. & Date</td><td>Destination</td><td>CD</td><td>Amount (In Rs.)</td><td></td></tr>");

            for (int i = 0; i < DtData.Rows.Count; i++)
            {
                htmlTable.Append("<tr>");
                htmlTable.Append("<td>" + (i + 1) + "</td>");
                INVNo = GenInvoiceNo(DtData.Rows[i]["No"].ToString(), DtData.Rows[i]["DataDate"].ToString());
                htmlTable.Append("<td>" + INVNo + ", " + Convert.ToDateTime(DtData.Rows[i]["DataDate"].ToString()).ToString("dd/MM/yyyy") + "</td>");
                htmlTable.Append("<td>" + DtData.Rows[i]["PartyName"].ToString() + "</td>");
                htmlTable.Append("<td>" + DtData.Rows[i]["BOrderNo"].ToString() + ", " + Convert.ToDateTime(DtData.Rows[i]["BOrderDate"].ToString()).ToString("dd/MM/yyyy") + "</td>");
                htmlTable.Append("<td>" + DtData.Rows[i]["DespNo"].ToString() + ", " + Convert.ToDateTime(DtData.Rows[i]["DespDate"].ToString()).ToString("dd/MM/yyyy") + "</td>");
                htmlTable.Append("<td>" + DtData.Rows[i]["Destination"].ToString() + "</td>");
                string[] calc = dataDisplay(DtData.Rows[i]["ID"].ToString()).Split('-');
                DtData.Rows[i]["Amount"] = Math.Round(Convert.ToDouble(calc[1].ToString()), 0);
                DtData.Rows[i]["CD"] = Math.Round(Convert.ToDouble(calc[0].ToString()), 0);
                htmlTable.Append("<td>" + DtData.Rows[i]["CD"].ToString() + "</td>");
                htmlTable.Append("<td>" + DtData.Rows[i]["Amount"].ToString() + "</td>");
                htmlTable.Append("<td><a href='BillofSupply.aspx?ID=" + DtData.Rows[i]["ID"].ToString() + "' target='_blank'>Bill of Supply</a></td></tr>");
            }

            Session["Export"] = null;
            Session["Export"] = DtData;
            htmlTable.Append("</table>");
            DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, "error");
        }
    }

    public string dataDisplay(string a)
    {
        string q = "";
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@ID", a));
        q = "select * from [prabha].[Sale_Master_Data] where ID=@ID";
        dac = new DataAccessLayer();
        Session["DataMain"] = dac.GetDataTable(q, param);

        q = "";
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@ID", a));
        q = "select * from [prabha].[Sale_Item_Info] where Master_ID=@ID";
        dac = new DataAccessLayer();
        Session["Data"] = dac.GetDataTable(q, param);

        double CD = 0;
        double GT = 0;

        if (Session["Data"] != null)
        {
            dtMain = (DataTable)Session["DataMain"];
            dt = (DataTable)Session["Data"];

            double am = 0;
            double wt = 0;
            double CGST = 0;
            double IGST = 0;
            double SGST = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                wt = wt + (Convert.ToDouble(dt.Rows[i]["Quantity"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()));
                am = am + (Convert.ToDouble(dt.Rows[i]["Quantity"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * Convert.ToDouble(dt.Rows[i]["Rate"].ToString()));

                if (dt.Rows[i]["RiceType"].ToString() == "Steam Bran")
                {
                    if (dtMain.Rows[0]["PGSTIN"].ToString() == "NA")
                    {
                        CGST += 0; IGST += 0; SGST += 0;
                    }
                    else
                    {
                        double kt = (Convert.ToDouble(dt.Rows[i]["Quantity"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * Convert.ToDouble(dt.Rows[i]["Rate"].ToString()));
                        kt = kt - Math.Round((kt * Convert.ToDouble(dtMain.Rows[0]["CD"].ToString()) / 100), 0);
                        if (dtMain.Rows[0]["PGSTIN"].ToString().Substring(0, 2) == "10")
                        {
                            CGST += Math.Round(kt * 2.5 / 100, 0);
                            SGST = Math.Round(kt * 2.5 / 100, 0);
                        }
                        else
                        {
                            IGST += Math.Round(kt * 5 / 100, 0);
                        }
                    }
                }
            }

            CD = Math.Round((am * Convert.ToDouble(dtMain.Rows[0]["CD"].ToString()) / 100), 0);
            double amwgst = am - Math.Round((am * Convert.ToDouble(dtMain.Rows[0]["CD"].ToString()) / 100), 0);
            GT = Math.Round(amwgst + IGST + CGST + SGST + Convert.ToDouble(dtMain.Rows[0]["Freight"].ToString()), 0);
        }

        return CD + "-" + GT;
    }

    public string GenInvoiceNo(string a, string b)
    {
        int mon = Convert.ToDateTime(b).Month;
        int yr = Convert.ToDateTime(b).Year;
        int yr1 = 0;
        int yr2 = 0;

        if (mon <= 3) { yr1 = yr - 1; yr2 = yr; }
        else { yr1 = yr; yr2 = yr + 1; }

        string invoiceNo = "";
        if (a.Length == 1) invoiceNo = "RR/INV/" + yr1 + "-" + yr2 + "/000" + a;
        else if (a.Length == 2) invoiceNo = "RR/INV/" + yr1 + "-" + yr2 + "/00" + a;
        else if (a.Length == 3) invoiceNo = "RR/INV/" + yr1 + "-" + yr2 + "/0" + a;
        else invoiceNo = "RR/INV/" + yr1 + "-" + yr2 + "/" + a;

        return invoiceNo;
    }

    public void Export_ServerClick(object sender, EventArgs e)
    {
        try
        {
            string errorMsg = ValidateDates(
                fdate.Value.Trim(),
                tdate.Value.Trim()
            );

            if (!string.IsNullOrEmpty(errorMsg))
            {
                ShowAlert(errorMsg, "error");
                return;
            }

            if (Session["Export"] == null)
            {
                ShowAlert("Please generate report first before exporting.", "error");
                return;
            }

            DataTable EData = (DataTable)Session["Export"];

            DataTable DtDataF = new DataTable();
            string q = "";
            param = new List<SqlParameter>();

            string fDate = Convert.ToDateTime(fdate.Value.Trim()).ToString("dd-MMM-yyyy");
            string tDate = Convert.ToDateTime(tdate.Value.Trim()).ToString("dd-MMM-yyyy");

            if (sPartyName.SelectedItem.Text.Trim() == "--Select One--")
            {
                param.Add(new SqlParameter("@DataDate1", fDate));
                param.Add(new SqlParameter("@DataDate2", tDate));
                q = "select ID,[No],MRVNo as ManualInvoice,DataDate,PName as PartyName,PaymentMode as BOrderNo,convert(smalldatetime,'01/01/1990') as BOrderDate,[Transaction] as DespNo,convert(smalldatetime,'01/01/1990') as DespDate,'' as DespVNo,'' as CD,convert(varchar,AmountPaid) as Amount from prabha.[Sale_Payment_Info] where DataDate>=@DataDate1 and DataDate<=@DataDate2 order by DataDate";
            }
            else
            {
                param.Add(new SqlParameter("@DataDate1", fDate));
                param.Add(new SqlParameter("@DataDate2", tDate));
                param.Add(new SqlParameter("@PartyName", sPartyName.SelectedItem.Text.Trim()));
                q = "select ID,[No],MRVNo as ManualInvoice,DataDate,PName as PartyName,PaymentMode as BOrderNo,convert(smalldatetime,'01/01/1990') as BOrderDate,[Transaction] as DespNo,convert(smalldatetime,'01/01/1990') as DespDate,'' as DespVNo,'' as CD,convert(varchar,AmountPaid) as Amount from prabha.[Sale_Payment_Info] where DataDate>=@DataDate1 and DataDate<=@DataDate2 and PName=@PartyName order by DataDate";
            }

            dac = new DataAccessLayer();
            DtDataF = dac.GetDataTable(q, param);

            EData.Merge(DtDataF);
            EData.DefaultView.Sort = "DataDate";
            EData = EData.DefaultView.ToTable();
            ExporttoExcel(EData);
        }
        catch (Exception ex)
        {
            ShowAlert("Error exporting data: " + ex.Message, "error");
        }
    }

    private void ExporttoExcel(DataTable table)
    {
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ClearContent();
        HttpContext.Current.Response.ClearHeaders();
        HttpContext.Current.Response.Buffer = true;
        HttpContext.Current.Response.ContentType = "application/ms-excel";
        HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=Reports.xls");
        HttpContext.Current.Response.Charset = "utf-8";
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");

        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' borderColor='#000000' cellSpacing='0' cellPadding='0' style='font-size:10.0pt; font-family:Calibri; background:white;'>");

        HttpContext.Current.Response.Write("<TR>");
        HttpContext.Current.Response.Write("<Td><B>Sl. No.</B></Td>");
        HttpContext.Current.Response.Write("<Td><B>Invoice/Payment No.</B></Td>");
        HttpContext.Current.Response.Write("<Td><B>Manual No.</B></Td>");
        HttpContext.Current.Response.Write("<Td><B>Invoice/Payment Date</B></Td>");
        HttpContext.Current.Response.Write("<Td><B>Party Name</B></Td>");
        HttpContext.Current.Response.Write("<Td><B>Sauda No. &amp; Date</B></Td>");
        HttpContext.Current.Response.Write("<Td><B>Despatch Doc No. &amp; Date</B></Td>");
        HttpContext.Current.Response.Write("<Td><B>Destination</B></Td>");
        HttpContext.Current.Response.Write("<Td><B>Vehicle No.</B></Td>");
        HttpContext.Current.Response.Write("<Td><B>CD</B></Td>");
        HttpContext.Current.Response.Write("<Td><B>Bill Amount</B></Td>");
        HttpContext.Current.Response.Write("<Td><B>Paid Amount</B></Td>");
        HttpContext.Current.Response.Write("</TR>");

        int i = 0;
        string InvoiceNo = "";

        foreach (DataRow row in table.Rows)
        {
            i = i + 1;
            HttpContext.Current.Response.Write("<TR>");
            HttpContext.Current.Response.Write("<Td>" + i.ToString() + "</Td>");

            if (Convert.ToDateTime(row["BOrderDate"].ToString()).ToString("dd-MMM-yyyy") == "01-Jan-1990")
                InvoiceNo = GenInvoiceNoSale(row["No"].ToString(), row["DataDate"].ToString());
            else
                InvoiceNo = GenInvoiceNo(row["No"].ToString(), row["DataDate"].ToString());

            HttpContext.Current.Response.Write("<Td>" + InvoiceNo + "</Td>");
            HttpContext.Current.Response.Write("<Td>" + row["ManualInvoice"].ToString() + "</Td>");
            HttpContext.Current.Response.Write("<Td>" + Convert.ToDateTime(row["DataDate"].ToString()).ToString("dd/MM/yyyy") + "</Td>");
            HttpContext.Current.Response.Write("<Td>" + row["PartyName"].ToString() + "</Td>");

            if (Convert.ToDateTime(row["BOrderDate"].ToString()).ToString("dd-MMM-yyyy") == "01-Jan-1990")
            {
                HttpContext.Current.Response.Write("<Td>" + row["BOrderNo"].ToString() + "</Td>");
                HttpContext.Current.Response.Write("<Td colspan='3'>" + row["DespNo"].ToString() + "</Td>");
                HttpContext.Current.Response.Write("<Td>0</Td>");
                HttpContext.Current.Response.Write("<Td>&nbsp;</Td>");
                HttpContext.Current.Response.Write("<Td>" + Math.Round(Convert.ToDouble(row["Amount"].ToString()), 2).ToString() + "</Td>");
            }
            else
            {
                HttpContext.Current.Response.Write("<Td>" + row["BOrderNo"].ToString() + ", " + Convert.ToDateTime(row["BOrderDate"].ToString()).ToString("dd/MM/yyyy") + "</Td>");
                HttpContext.Current.Response.Write("<Td>" + row["DespNo"].ToString() + ", " + Convert.ToDateTime(row["DespDate"].ToString()).ToString("dd/MM/yyyy") + "</Td>");
                HttpContext.Current.Response.Write("<Td>" + row["Destination"].ToString() + "</Td>");
                HttpContext.Current.Response.Write("<Td>" + row["DespVNo"].ToString() + "</Td>");
                HttpContext.Current.Response.Write("<Td>" + Math.Round(Convert.ToDouble(row["CD"].ToString()), 2).ToString() + "</Td>");
                HttpContext.Current.Response.Write("<Td>" + Math.Round(Convert.ToDouble(row["Amount"].ToString()), 2).ToString() + "</Td>");
                HttpContext.Current.Response.Write("<Td>&nbsp;</Td>");
            }

            HttpContext.Current.Response.Write("</TR>");
        }

        HttpContext.Current.Response.Write("</Table>");
        HttpContext.Current.Response.Write("</font>");
        HttpContext.Current.Response.Flush();
        HttpContext.Current.Response.End();
    }

    public string GenInvoiceNoSale(string a, string b)
    {
        int mon = Convert.ToDateTime(b).Month;
        int yr = Convert.ToDateTime(b).Year;
        int yr1 = 0;
        int yr2 = 0;

        if (mon <= 3) { yr1 = yr - 1; yr2 = yr; }
        else { yr1 = yr; yr2 = yr + 1; }

        string invoiceNo = "";
        if (a.Length == 1) invoiceNo = "RR/RV/" + yr1 + "-" + yr2 + "/000" + a;
        else if (a.Length == 2) invoiceNo = "RR/RV/" + yr1 + "-" + yr2 + "/00" + a;
        else if (a.Length == 3) invoiceNo = "RR/RV/" + yr1 + "-" + yr2 + "/0" + a;
        else invoiceNo = "RR/RV/" + yr1 + "-" + yr2 + "/" + a;

        return invoiceNo;
    }
}
