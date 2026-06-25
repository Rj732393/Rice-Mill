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

public partial class PurchaseUnloading : System.Web.UI.Page
{
    DataTable dt;
    DataTable dtMain;
    DataRow dtRow;
    DataRow rmain;
    List<SqlParameter> param;
    DataAccessLayer dac;
    string script = "";
    DataRow companyRow;   // <-- NAYA: current logged-in company ki details yaha store hongi

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
            }


            fdate.Attributes["type"] = "date";
            tdate.Attributes["type"] = "date";
            Party();
            Session["Data"] = null;
            Session["DataMain"] = null;

        }
    }

    // ===== NAYA METHOD: Session["CompanyID"] se current company ki details DB se uthata hai =====
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
            {
                companyRow = dtC.Rows[0];
            }
            else
            {
                companyRow = null;
            }
        }
        catch (Exception)
        {
            companyRow = null;
        }
    }

    // ===== NAYA HELPER: agar column DB table me exist nahi karta, crash nahi hoga, khali string aayega =====
    private string SafeCol(DataRow row, string colName)
    {
        if (row == null) return "";
        if (!row.Table.Columns.Contains(colName)) return "";
        return row[colName] == DBNull.Value ? "" : row[colName].ToString();
    }

    public void btnContinue_ServerClick(object sender, EventArgs e)
    {

        checkData();


    }

    public int chkDate()
    {
        int i = 0;
        try
        {
            string dat = Convert.ToDateTime(fdate.Value.Trim()).ToString("dd-MMM-yyyy");
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
    public void Party()
    {
        dt = new DataTable();
        string q = "";
        param = new List<SqlParameter>();//Emp_Id
        q = "select distinct PartyName from prabha.Sale_Master_Data order by PartyName";
        dac = new DataAccessLayer();
        dt = dac.GetDataTable(q, param);

        sPartyName.DataSource = dt;
        sPartyName.DataTextField = "PartyName";
        sPartyName.DataValueField = "PartyName";
        sPartyName.DataBind();
        sPartyName.Items.Insert(0, "--Select One--");
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

    public void checkData()
    {
        try
        {
            LoadCompanyDetails();   // <-- NAYA: report banane se pehle current company load karo

            DataTable DtData = new DataTable();
            string q = "";
            param = new List<SqlParameter>();//Emp_Id


            if (sPartyName.SelectedItem.Text.Trim() == "--Select One--")
            {
                param.Add(new SqlParameter("@DataDate1", Convert.ToDateTime(fdate.Value.Trim()).ToString("dd-MMM-yyyy")));
                param.Add(new SqlParameter("@DataDate2", Convert.ToDateTime(tdate.Value.Trim()).ToString("dd-MMM-yyyy")));

                q = "select ID,[No],ManualInvoice,DataDate,PartyName,BOrderNo,BOrderDate,DespNo,DespDate,DespVNo,Destination from prabha.Sale_Master_Data where DataDate>=@DataDate1 and DataDate<=@DataDate2 order by [No],DataDate";
            }
            else
            {
                param.Add(new SqlParameter("@DataDate1", Convert.ToDateTime(fdate.Value.Trim()).ToString("dd-MMM-yyyy")));
                param.Add(new SqlParameter("@DataDate2", Convert.ToDateTime(tdate.Value.Trim()).ToString("dd-MMM-yyyy")));
                param.Add(new SqlParameter("@PartyName", sPartyName.SelectedItem.Text.Trim()));

                q = "select ID,[No],ManualInvoice,DataDate,PartyName,BOrderNo,BOrderDate,DespNo,DespDate,DespVNo,Destination from prabha.Sale_Master_Data where DataDate>=@DataDate1 and DataDate<=@DataDate2 and PartyName=@PartyName order by [No],DataDate";
            }
            dac = new DataAccessLayer();
            DtData = dac.GetDataTable(q, param);
            DtData.Columns.Add("Amount", typeof(string));
            DtData.Columns.Add("CD", typeof(string));

            // ===== NAYA: company ki saari details safely uthayi ja rahi hain (missing column se crash nahi hoga) =====
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

            // ===== CHANGE: hardcoded "Rashmi Rice Mills" hata kar company ki dynamic details daali gayi hain (logo hata diya - 404 fix) =====
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
        catch (Exception)
        {
            DBDataPlaceHolder.Controls.Add(new Literal { Text = "<table class='table'><tr><td align='center'>Data load karne mein error aaya, page refresh karein.</td></tr></table>" });
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
        if (Session["Data"] == null)
        {

        }
        else
        {
            dtMain = (DataTable)Session["DataMain"];
            dt = (DataTable)Session["Data"];



            //string CInvNo = "";
            //CInvNo = GenInvoiceNo(dtMain.Rows[0]["No"].ToString(), dtMain.Rows[0]["DataDate"].ToString());


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

            double amwgst = 0;
            amwgst = am - Math.Round((am * Convert.ToDouble(dtMain.Rows[0]["CD"].ToString()) / 100), 0);

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
            invoiceNo = "RR/INV/" + yr1 + "-" + yr2 + "/000" + a;
        }
        else if (a.Length == 2)
        {
            invoiceNo = "RR/INV/" + yr1 + "-" + yr2 + "/00" + a;
        }
        else if (a.Length == 3)
        {
            invoiceNo = "RR/INV/" + yr1 + "-" + yr2 + "/0" + a;
        }
        else
        {
            invoiceNo = "RR/INV/" + yr1 + "-" + yr2 + "/" + a;
        }


        return invoiceNo;
    }
    public void Export_ServerClick(object sender, EventArgs e)
    {

        if (Session["Export"] == null)
        {

        }
        else
        {
            DataTable EData = (DataTable)Session["Export"];

            DataTable DtDataF = new DataTable();
            string q = "";
            param = new List<SqlParameter>();//Emp_Id


            if (sPartyName.SelectedItem.Text.Trim() == "--Select One--")
            {
                param.Add(new SqlParameter("@DataDate1", Convert.ToDateTime(fdate.Value.Trim()).ToString("dd-MMM-yyyy")));
                param.Add(new SqlParameter("@DataDate2", Convert.ToDateTime(tdate.Value.Trim()).ToString("dd-MMM-yyyy")));
                //ID,[No],ManualInvoice,DataDate,PartyName,BOrderNo,BOrderDate,DespNo,DespDate,Destination,CD,Amount
                q = "select ID,[No],MRVNo as ManualInvoice,DataDate,PName as PartyName,PaymentMode as BOrderNo,convert(smalldatetime,'01/01/1990') as BOrderDate,[Transaction] as DespNo,convert(smalldatetime,'01/01/1990') as DespDate,'' as DespVNo,'' as CD,convert(varchar,AmountPaid) as Amount from prabha.[Sale_Payment_Info] where DataDate>=@DataDate1 and DataDate<=@DataDate2 order by DataDate";
            }
            else
            {
                param.Add(new SqlParameter("@DataDate1", Convert.ToDateTime(fdate.Value.Trim()).ToString("dd-MMM-yyyy")));
                param.Add(new SqlParameter("@DataDate2", Convert.ToDateTime(tdate.Value.Trim()).ToString("dd-MMM-yyyy")));
                param.Add(new SqlParameter("@PartyName", sPartyName.SelectedItem.Text.Trim()));

                q = "select ID,[No],MRVNo as ManualInvoice,DataDate,PName as PartyName,PaymentMode as BOrderNo,convert(smalldatetime,'01/01/1990') as BOrderDate,[Transaction] as DespNo,convert(smalldatetime,'01/01/1990') as DespDate,'' as DespVNo,'' as CD,convert(varchar,AmountPaid) as Amount from prabha.[Sale_Payment_Info] where DataDate>=@DataDate1 and DataDate<=@DataDate2 and PName=@PartyName order by DataDate";
            }

            dac = new DataAccessLayer();
            DtDataF = dac.GetDataTable(q, param);

            EData.Merge(DtDataF);

            //string script = "alert('" + EData.Rows.Count + "');";
            //ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
            EData.DefaultView.Sort = "DataDate";
            EData = EData.DefaultView.ToTable();
            ExporttoExcel(EData);
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
        //sets font
        HttpContext.Current.Response.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        HttpContext.Current.Response.Write("<BR><BR><BR>");

        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:10.0pt; font-family:Calibri; background:white;'>");
        //am getting my grid's column headers
        //write in new column
        //HttpContext.Current.Response.Write("<TR><Td colspan='12' align='center'><span style='display:table-cell; vertical-align:top;'><img src='http://prabhasoftware.com/Rashmi Rice Logo (1).png' height='100px'/></span><span style='display:table-cell; vertical-align:top;'><span style='font-size:16pt; font-weight:bold;'> Rashmi Rice Mills Pvt. Ltd. </span></br><span style='font-size:8pt;'>Daniyawan Chandi Road, Hasanpur, Patna- 801304 </br>Mob.: 9304052349, 9334280057</br>Email: srirajbhog@gmail.com</br>CIN: U15312BR2014PTC022237</br>PAN No.: AAGCR9497P</br>GSTIN: 10AAGCR9497P1ZK</span></span></Td></TR>");
        //HttpContext.Current.Response.Write("<TR><Td colspan='12' align='center'><span style='font-size:10pt; font-weight:bold;'> SALE REPORT </span></Td></TR>");
        HttpContext.Current.Response.Write("<TR><Td>");
        //Get column headers  and make it as bold in excel columns
        HttpContext.Current.Response.Write("<B>");
        HttpContext.Current.Response.Write("Sl. No.");
        HttpContext.Current.Response.Write("</B>");
        HttpContext.Current.Response.Write("</Td>");

        HttpContext.Current.Response.Write("<Td><B>Invoice/Payment No.</B></Td>");
        HttpContext.Current.Response.Write("<Td><B>Manual No.</B></Td>");
        HttpContext.Current.Response.Write("<Td><B>Invoice/Payment Date</B></Td>");
        HttpContext.Current.Response.Write("<Td><B>Party Name</B></Td>");
        HttpContext.Current.Response.Write("<Td><B>Sauda No. & Date</B></Td>");
        HttpContext.Current.Response.Write("<Td><B>Despatch Doc No. & Date</B></Td>");
        HttpContext.Current.Response.Write("<Td><B>Destination</B></Td>");
        HttpContext.Current.Response.Write("<Td><B>Vehicle No.</B></Td>");
        HttpContext.Current.Response.Write("<Td><B>CD</B></Td>");

        HttpContext.Current.Response.Write("<Td><B>Bill Amount</B></Td>");
        HttpContext.Current.Response.Write("<Td><B>Paid Amount</B></Td>");

        HttpContext.Current.Response.Write("</TR>");
        int i = 0;
        string InvoiceNo = "";
        foreach (DataRow row in table.Rows)
        {//write in new row
            //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
            /*htmlTable.Append("<thead><tr><th>Sl. No.</th><th>Invoice No. & Date</th><th>Party Name</th><th>Buyer's Order No. & Date</th><th>Despatch Doc No. & Date</th><th>Destination</th><th>CD</th><th>Amount (In Rs.)</th><th></th></tr></thead><tbody>");
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
                htmlTable.Append("<td>" + calc[0].ToString() + "</td>");
                htmlTable.Append("<td>" + calc[1].ToString() + "</td>");
                htmlTable.Append("<td><a href='BillofSupply.aspx?ID=" + DtData.Rows[i]["ID"].ToString() + "' target='_blank'>Bill of Supply</a></td></tr>");
            }
            htmlTable.Append("</tbody></table>");*/
            i = i + 1;
            if (Convert.ToDateTime(row["BOrderDate"].ToString()).ToString("dd-MMM-yyyy") == "01-Jan-1990")
            {
                HttpContext.Current.Response.Write("<TR>");
            }
            else
            {
                HttpContext.Current.Response.Write("<TR>");
            }
            HttpContext.Current.Response.Write("<Td>");
            HttpContext.Current.Response.Write(i.ToString());
            HttpContext.Current.Response.Write("<Td>");
            if (Convert.ToDateTime(row["BOrderDate"].ToString()).ToString("dd-MMM-yyyy") == "01-Jan-1990")
            {
                InvoiceNo = GenInvoiceNoSale(row["No"].ToString(), row["DataDate"].ToString());
            }
            else
            {
                InvoiceNo = GenInvoiceNo(row["No"].ToString(), row["DataDate"].ToString());
            }
            HttpContext.Current.Response.Write(InvoiceNo);
            HttpContext.Current.Response.Write("</Td>");
            HttpContext.Current.Response.Write("<Td>");
            HttpContext.Current.Response.Write(row["ManualInvoice"].ToString());
            HttpContext.Current.Response.Write("</Td>");
            HttpContext.Current.Response.Write("<Td>");
            HttpContext.Current.Response.Write(Convert.ToDateTime(row["DataDate"].ToString()).ToString("dd/MM/yyyy"));
            HttpContext.Current.Response.Write("</Td>");
            HttpContext.Current.Response.Write("<Td>");
            HttpContext.Current.Response.Write(row["PartyName"].ToString());
            HttpContext.Current.Response.Write("</Td>");

            if (Convert.ToDateTime(row["BOrderDate"].ToString()).ToString("dd-MMM-yyyy") == "01-Jan-1990")
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row["BOrderNo"].ToString());
                HttpContext.Current.Response.Write("</Td>");
            }
            else
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row["BOrderNo"].ToString() + ", " + Convert.ToDateTime(row["BOrderDate"].ToString()).ToString("dd/MM/yyyy"));
                HttpContext.Current.Response.Write("</Td>");

            }

            if (Convert.ToDateTime(row["BOrderDate"].ToString()).ToString("dd-MMM-yyyy") == "01-Jan-1990")
            {
                HttpContext.Current.Response.Write("<Td colspan='3'>");
                HttpContext.Current.Response.Write(row["DespNo"].ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("0");

                HttpContext.Current.Response.Write("</Td>");
            }
            else
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row["DespNo"].ToString() + ", " + Convert.ToDateTime(row["DespDate"].ToString()).ToString("dd/MM/yyyy"));
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row["Destination"].ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row["DespVNo"].ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(Math.Round(Convert.ToDouble(row["CD"].ToString()), 2).ToString());

                HttpContext.Current.Response.Write("</Td>");

            }
            if (Convert.ToDateTime(row["BOrderDate"].ToString()).ToString("dd-MMM-yyyy") == "01-Jan-1990")
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("&nbsp;");
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(Math.Round(Convert.ToDouble(row["Amount"].ToString()), 2).ToString());
                HttpContext.Current.Response.Write("</Td>");
            }
            else
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(Math.Round(Convert.ToDouble(row["Amount"].ToString()), 2).ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write("&nbsp;");
                HttpContext.Current.Response.Write("</Td>");
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
            invoiceNo = "RR/RV/" + yr1 + "-" + yr2 + "/000" + a;
        }
        else if (a.Length == 2)
        {
            invoiceNo = "RR/RV/" + yr1 + "-" + yr2 + "/00" + a;
        }
        else if (a.Length == 3)
        {
            invoiceNo = "RR/RV/" + yr1 + "-" + yr2 + "/0" + a;
        }
        else
        {
            invoiceNo = "RR/RV/" + yr1 + "-" + yr2 + "/" + a;
        }


        return invoiceNo;
    }
}
