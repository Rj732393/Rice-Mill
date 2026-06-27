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

public partial class SaleSauda : System.Web.UI.Page
{
    DataTable dt;
    DataTable dtMain;
    DataRow dtRow;
    DataRow rmain;
    List<SqlParameter> param;
    DataAccessLayer dac;
    string script = "";
    DataRow companyRow;   // current logged-in company ki details yaha store hongi

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["User"] == null)
            {
                Response.Redirect("Login.aspx");
            }

            sdate.Attributes["type"] = "date";
            sdate.Attributes["max"] = DateTime.Today.ToString("yyyy-MM-dd");

            pMN.Attributes["type"] = "number";
            pMN.Attributes["step"] = "1";

            QIKG.Attributes["type"] = "number";
            QIKG.Attributes["step"] = ".001";

            avgrate.Attributes["type"] = "number";
            avgrate.Attributes["step"] = ".01";

            Party();

            Panel1.Visible = sPartyName.SelectedItem != null &&
                             sPartyName.SelectedItem.Text.Trim() == "Other";

            Session["Data"] = null;
            Session["DataMain"] = null;

            dataDisplay();
        }
    }

    // Session["CompanyID"] se current company ki details DB se uthata hai
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

    // agar column DB table me exist nahi karta, crash nahi hoga, khali string aayega
    private string SafeCol(DataRow row, string colName)
    {
        if (row == null) return "";
        if (!row.Table.Columns.Contains(colName)) return "";
        return row[colName] == DBNull.Value ? "" : row[colName].ToString();
    }

    /* ============================================================
       SERVER-SIDE VALIDATION
       ============================================================ */
    private string ValidateContinueEntry(
        string date,
        string supplierRef,
        string partyText,
        string pNameVal,
        string pMNVal,
        string pGSTVal,
        string pPANVal,
        string pAddrVal,
        string qikg,
        string rate,
        out DateTime parsedDate,
        out double parsedQty,
        out double parsedRate)
    {
        parsedDate = DateTime.MinValue;
        parsedQty = 0;
        parsedRate = 0;

        if (string.IsNullOrEmpty(date))
            return "Please select Date.";
        if (!DateTime.TryParse(date, out parsedDate))
            return "Please enter a valid Date.";
        if (parsedDate.Date > DateTime.Today)
            return "Future date is not allowed.";

        if (partyText == "Other")
        {
            if (string.IsNullOrEmpty(pNameVal)) return "Please enter Party Name.";
            if (string.IsNullOrEmpty(pMNVal)) return "Please enter Mobile No.";
            if (string.IsNullOrEmpty(pGSTVal)) return "Please enter GSTIN.";
            if (string.IsNullOrEmpty(pPANVal)) return "Please enter PAN.";
            if (string.IsNullOrEmpty(pAddrVal)) return "Please enter Address.";
        }

        if (string.IsNullOrEmpty(qikg))
            return "Please enter Quantity.";
        if (!double.TryParse(qikg, out parsedQty) || parsedQty <= 0)
            return "Quantity must be a positive number.";

        if (string.IsNullOrEmpty(rate))
            return "Please enter Rate.";
        if (!double.TryParse(rate, out parsedRate) || parsedRate <= 0)
            return "Rate must be a positive number.";

        return null;
    }

    /* ============================================================
       RESET BUTTON
       ============================================================ */
    public void Submit1_ServerClick(object sender, EventArgs e)
    {
        Session["Data"] = null;
        Session["DataMain"] = null;
        dataDisplay();
    }

    /* ============================================================
       SAVE BUTTON
       ============================================================ */
    public void btnSave_ServerClick(object sender, EventArgs e)
    {
        if (Session["Data"] == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "saveErr",
                "$(document).ready(function(){ showAlert('Please add at least one item first!', 'error'); });", true);
        }
        else if (Session["User"] == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "sessErr",
                "$(document).ready(function(){ showAlert('Your session has expired!', 'error'); });", true);
        }
        else
        {
            insertPurchaseData();
            dataDisplay();
            CallPrint("prntContent");
        }
    }

    /* ============================================================
       CONTINUE BUTTON
       ============================================================ */
    public void btnContinue_ServerClick(object sender, EventArgs e)
    {
        DateTime parsedDate;
        double parsedQty;
        double parsedRate;

        string partyText = sPartyName.SelectedItem != null
            ? sPartyName.SelectedItem.Text.Trim()
            : "";

        string errorMsg = ValidateContinueEntry(
            sdate.Value.Trim(),
            txtEmpName.Text.Trim(),
            partyText,
            pName.Value.Trim(),
            pMN.Value.Trim(),
            pGST.Value.Trim(),
            pPAN.Value.Trim(),
            pAddress.Value.Trim(),
            QIKG.Value.Trim(),
            avgrate.Value.Trim(),
            out parsedDate,
            out parsedQty,
            out parsedRate
        );

        if (!string.IsNullOrEmpty(errorMsg))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "valErr",
                "$(document).ready(function(){ showAlert('" + errorMsg.Replace("'", "\\'") + "', 'error'); });", true);
            dataDisplay();
            return;
        }

        int chk = PartyValidation();
        if (chk != 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "partyErr",
                "$(document).ready(function(){ showAlert('Please fill all Party fields.', 'error'); });", true);
            dataDisplay();
            return;
        }

        dtMain = new DataTable();
        dtMain.Columns.Add("No", typeof(string));
        dtMain.Columns.Add("DataDate", typeof(string));
        dtMain.Columns.Add("MNo", typeof(string));
        dtMain.Columns.Add("PartyName", typeof(string));
        dtMain.Columns.Add("PMobile", typeof(string));
        dtMain.Columns.Add("PAddress", typeof(string));
        dtMain.Columns.Add("PGSTIN", typeof(string));
        dtMain.Columns.Add("PPAN", typeof(string));
        dtMain.Columns.Add("BrokerName", typeof(string));

        rmain = dtMain.NewRow();
        rmain[0] = GenInvoiceNo();
        rmain[1] = parsedDate.ToString("dd-MMM-yyyy");
        rmain[2] = MPNo.Value.Trim();

        if (partyText == "Other")
        {
            rmain[3] = pName.Value.Trim();
            rmain[4] = pMN.Value.Trim();
            rmain[5] = pAddress.Value.Trim();
            rmain[6] = pGST.Value.Trim();
            rmain[7] = pPAN.Value.Trim();
        }
        else
        {
            string[] sep = new string[] { " (Mobile No.: " };
            var result = partyText.Split(sep, StringSplitOptions.None);
            string Pname = result[0];
            string PMob = result[1].Substring(0, result[1].Length - 1);

            DataTable dtPartyDetails = checkData(Pname, PMob);
            rmain[3] = dtPartyDetails.Rows[0]["PartyName"].ToString();
            rmain[4] = dtPartyDetails.Rows[0]["PMobile"].ToString();
            rmain[5] = dtPartyDetails.Rows[0]["PAddress"].ToString();
            rmain[6] = dtPartyDetails.Rows[0]["PGSTIN"].ToString();
            rmain[7] = dtPartyDetails.Rows[0]["PPAN"].ToString();
        }
        rmain[8] = txtEmpName.Text.Trim();

        dtMain.Rows.Add(rmain);
        Session["DataMain"] = null;
        Session["DataMain"] = dtMain;

        if (Session["Data"] == null)
        {
            DataTable dtData = new DataTable();
            dtData.Columns.Add("ItemType", typeof(string));
            dtData.Columns.Add("QIKG", typeof(string));
            dtData.Columns.Add("AvgRate", typeof(string));

            dtRow = dtData.NewRow();
            dtRow[0] = sPaddyType.Value.Trim();
            dtRow[1] = QIKG.Value.Trim();
            dtRow[2] = avgrate.Value.Trim();
            dtData.Rows.Add(dtRow);

            Session["Data"] = dtData;
        }
        else
        {
            DataTable dtData = (DataTable)Session["Data"];

            for (int i = dtData.Rows.Count - 1; i >= 0; i--)
            {
                if (dtData.Rows[i]["ItemType"].ToString() == sPaddyType.Value.Trim())
                    dtData.Rows[i].Delete();
            }
            dtData.AcceptChanges();

            dtRow = dtData.NewRow();
            dtRow[0] = sPaddyType.Value.Trim();
            dtRow[1] = QIKG.Value.Trim();
            dtRow[2] = avgrate.Value.Trim();
            dtData.Rows.Add(dtRow);

            Session["Data"] = dtData;
        }

        dataDisplay();
    }

    /* ============================================================
       DATA DISPLAY
       ============================================================ */
    public void dataDisplay()
    {
        StringBuilder htmlTable;

        try
        {
            LoadCompanyDetails();

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

                string compName = SafeCol(companyRow, "CompanyName");
                if (string.IsNullOrWhiteSpace(compName)) compName = "Company";
                string compAddress = (SafeCol(companyRow, "Address") + ", " + SafeCol(companyRow, "City") + ", " + SafeCol(companyRow, "State")).Trim(',', ' ');
                string compPhone = SafeCol(companyRow, "Phone");
                string compEmail = SafeCol(companyRow, "Email");
                string compCIN = SafeCol(companyRow, "CINNumber");
                string compPAN = SafeCol(companyRow, "PANNumber");
                string compGST = SafeCol(companyRow, "GSTNumber");  

                htmlTable = new StringBuilder();
                htmlTable.Append("<table class='table' runat='server' style='font-size:10pt; noWrap' id='printTable' cellspacing='0' border='1px'>");

                htmlTable.Append("<tr><td colspan='3' align='center'><span style='display:table-cell; vertical-align:top;'><span style='font-size:16pt; font-weight:bold;'> " + compName + " </span></br><span style='font-size:8pt;'>" + compAddress + " </br>Mob.: " + compPhone + "</br>Email: " + compEmail + "</br>CIN: " + compCIN + "</br>PAN No.: " + compPAN + "</br>GSTIN: " + compGST + "</span></span></td></tr>");
                htmlTable.Append("<tr><td colspan='3' align='center'><span style='font-size:10pt; font-weight:bold;'>SALE SAUDA REPORT </span></td></tr>");

                htmlTable.Append("<tr><td align='left' rowspan='3' valign='top'><b>Party Details:</b></br>" + dtMain.Rows[0]["PartyName"].ToString() + "</br>" + dtMain.Rows[0]["PAddress"].ToString() + "</br>");
                htmlTable.Append("Mobile No.: " + dtMain.Rows[0]["PMobile"].ToString() + "</br>GSTIN: " + dtMain.Rows[0]["PGSTIN"].ToString() + "</br>PAN: " + dtMain.Rows[0]["PPAN"].ToString() + "</td>");

                if (dtMain.Rows[0]["MNo"].ToString() == "")
                    htmlTable.Append("<td colspan='2' align='left'>Sauda No.: <b>" + dtMain.Rows[0]["No"].ToString() + "</b></td></tr>");
                else
                    htmlTable.Append("<td colspan='2' align='left'>Sauda No.: <b>" + dtMain.Rows[0]["No"].ToString() + "</b></br>Manual Sauda No.: <b>" + dtMain.Rows[0]["MNo"].ToString() + "</b></td></tr>");

                htmlTable.Append("<tr><td colspan='2' align='left'>Sauda Date: " + Convert.ToDateTime(dtMain.Rows[0]["DataDate"].ToString()).ToString("dd/MM/yyyy") + "</td></tr>");
                htmlTable.Append("<tr><td colspan='2' align='left'>Supplier's Ref.: " + dtMain.Rows[0]["BrokerName"].ToString() + "</td></tr>");

                htmlTable.Append("<tr><td align='center' width='60%'><b>Description of Goods</b></td>");
                htmlTable.Append("<td align='center' width='20%'><b>Qty. In KG</b></td>");
                htmlTable.Append("<td align='center' width='20%'><b>Rate /KG</b></td></tr>");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    htmlTable.Append("<tr><td align='left'>" + dt.Rows[i]["ItemType"].ToString() + "</td>");
                    htmlTable.Append("<td align='right'>" + Math.Round(Convert.ToDouble(dt.Rows[i]["QIKG"].ToString()), 3).ToString() + "</td>");
                    htmlTable.Append("<td align='right'>" + Math.Round(Convert.ToDouble(dt.Rows[i]["AvgRate"].ToString()), 2).ToString() + "</td>");
                    htmlTable.Append("</tr>");
                }

                htmlTable.Append("<tr><td align='left'><span style='font-size:7pt;'><b>Note:</b></br>All claim disputes will be resolved within 2 working days from the date of issue of this Purchase Order and receipt of a copy of this Order to you.</br>दावे से सम्बंधित सभी विवादों का समाधान इस खरीद आदेश के जारी होने और इस आदेश की प्रति आपको प्राप्त होने की तारीख से 2 कार्य दिवसों के भीतर किया जाएगा।</span></td>");
                htmlTable.Append("<td colspan='2' align='center'><b>For " + compName + "</br></br></br>Authorised Signatory</b></td>");
                htmlTable.Append("</table>");
            }

            DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
        }
        catch (Exception)
        {
            DBDataPlaceHolder.Controls.Add(new Literal { Text = "<table class='table'><tr><td align='center'>Data load karne mein error aaya, page refresh karein.</td></tr></table>" });
        }
    }

    /* ============================================================
       PRINT
       ============================================================ */
    public void CallPrint(string strid)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<script type='text/javascript'>");
        sb.Append("var prtContent = document.getElementById('" + strid + "');");
        sb.Append("var WinPrint = window.open('','','left=50,top=40,width=400,height=400,toolbar=0,scrollbars=0,status=0');");
        sb.Append("WinPrint.document.write(prtContent.innerHTML);");
        sb.Append("WinPrint.document.close();");
        sb.Append("WinPrint.focus();");
        sb.Append("setTimeout(function(){");
        sb.Append("WinPrint.print();");
        sb.Append("WinPrint.close();");
        sb.Append("}, 250);");
        sb.Append("</" + "script>");
        ClientScript.RegisterStartupScript(this.GetType(), "Print", sb.ToString());
    }

    /* ============================================================
       PARTY VALIDATION
       ============================================================ */
    public int PartyValidation()
    {
        if (sPartyName.SelectedItem != null &&
            sPartyName.SelectedItem.Text.Trim() == "Other")
        {
            if (pName.Value.Trim() == "" ||
                pMN.Value.Trim() == "" ||
                pAddress.Value.Trim() == "" ||
                pGST.Value.Trim() == "" ||
                pPAN.Value.Trim() == "")
                return 0;
        }
        return 1;
    }

    /* ============================================================
       PARTY DROPDOWN
       ============================================================ */
    public void Party()
    {
        dt = new DataTable();
        param = new List<SqlParameter>();
        string q = "select distinct concat(PartyName,' (Mobile No.: ',PMobile,')') as PartyName "
                 + "from prabha.Sale_Sauda_Master order by PartyName";
        dac = new DataAccessLayer();
        dt = dac.GetDataTable(q, param);

        sPartyName.DataSource = dt;
        sPartyName.DataTextField = "PartyName";
        sPartyName.DataValueField = "PartyName";
        sPartyName.DataBind();
        sPartyName.Items.Add("Other");
    }

    /* ============================================================
       CHECK PARTY DETAILS
       ============================================================ */
    public DataTable checkData(string PName, string PMobile)
    {
        dt = new DataTable();
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@Party_Name", PName));
        param.Add(new SqlParameter("@PMobile", PMobile));

        string q = "select distinct PartyName,PMobile,PAddress,PGSTIN,PPAN "
                 + "from prabha.Sale_Sauda_Master "
                 + "where PartyName=@Party_Name and PMobile=@PMobile";
        dac = new DataAccessLayer();
        dt = dac.GetDataTable(q, param);
        return dt;
    }

    /* ============================================================
       AUTOCOMPLETE WEB METHOD
       ============================================================ */
    [WebMethod]
    public static List<string> GetEmployeeName(string empName)
    {
        List<string> empResult = new List<string>();
        using (SqlConnection con = new SqlConnection(
            @"Server=ws241.win.arvixe.com;Database=sati1983_farming;User ID=prabha;Password=prabha@#*2022;Trusted_Connection=False;"))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "SELECT [BrokerName] FROM [prabha].Sale_Sauda_Master "
                                + "where [BrokerName] like ''+@SearchEmpName+'%'";
                cmd.Connection = con;
                con.Open();
                cmd.Parameters.AddWithValue("@SearchEmpName", empName);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                    empResult.Add(dr["BrokerName"].ToString());
                con.Close();
                return empResult;
            }
        }
    }

    /* ============================================================
       PARTY DROPDOWN CHANGE
       ============================================================ */
    protected void sPartyName_SelectedIndexChanged(object sender, EventArgs e)
    {
        Panel1.Visible = sPartyName.SelectedItem != null &&
                         sPartyName.SelectedItem.Text.Trim() == "Other";
    }

    /* ============================================================
       GENERATE INVOICE NO  (new entry)
       ============================================================ */
    public string GenInvoiceNo()
    {
        int mon = Convert.ToDateTime(sdate.Value.Trim()).Month;
        int yr = Convert.ToDateTime(sdate.Value.Trim()).Year;
        int yr1, yr2;
        string dtFrom, dtTo;

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

        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@DataDate1", Convert.ToDateTime(dtFrom).ToString("dd-MMM-yyyy")));
        param.Add(new SqlParameter("@DataDate2", Convert.ToDateTime(dtTo).ToString("dd-MMM-yyyy")));

        string q = "select max([No]) from prabha.Sale_Sauda_Master "
                 + "where DataDate>=@DataDate1 and DataDate<=@DataDate2";
        dac = new DataAccessLayer();
        object test = dac.Scalar(q, param);

        string invoiceNo;
        if (test == DBNull.Value || test == null)
            invoiceNo = "RR/SS/" + yr1 + "-" + yr2 + "/0001";
        else
        {
            int next = Convert.ToInt32(test) + 1;
            invoiceNo = "RR/SS/" + yr1 + "-" + yr2 + "/" + next.ToString("D4");
        }
        return invoiceNo;
    }

    /* ============================================================
       GENERATE INVOICE NO  (display list)
       ============================================================ */
    public string GenInvoiceNo(string a, string b)
    {
        int mon = Convert.ToDateTime(b).Month;
        int yr = Convert.ToDateTime(b).Year;
        int yr1, yr2;

        if (mon <= 3) { yr1 = yr - 1; yr2 = yr; }
        else { yr1 = yr; yr2 = yr + 1; }

        int num;
        string suffix = int.TryParse(a, out num) ? num.ToString("D4") : a;
        return "RR/SS/" + yr1 + "-" + yr2 + "/" + suffix;
    }

    /* ============================================================
       INSERT DATA
       ============================================================ */
    public void insertPurchaseData()
    {
        dtMain = (DataTable)Session["DataMain"];
        dt = (DataTable)Session["Data"];

        int chkValid = chkData(
            dtMain.Rows[0]["DataDate"].ToString(),
            dtMain.Rows[0]["PartyName"].ToString());

        if (chkValid == 0)
        {
            string q = "";
            param = new List<SqlParameter>();

            string[] Inv = dtMain.Rows[0]["No"].ToString().Split('/');

            param.Add(new SqlParameter("@No", Inv[3]));
            param.Add(new SqlParameter("@DataDate", Convert.ToDateTime(dtMain.Rows[0]["DataDate"].ToString()).ToString("dd-MMM-yyyy")));
            param.Add(new SqlParameter("@MNo", dtMain.Rows[0]["MNo"].ToString()));
            param.Add(new SqlParameter("@PartyName", dtMain.Rows[0]["PartyName"].ToString()));
            param.Add(new SqlParameter("@PMobile", dtMain.Rows[0]["PMobile"].ToString()));
            param.Add(new SqlParameter("@PAddress", dtMain.Rows[0]["PAddress"].ToString()));
            param.Add(new SqlParameter("@PGSTIN", dtMain.Rows[0]["PGSTIN"].ToString()));
            param.Add(new SqlParameter("@PPAN", dtMain.Rows[0]["PPAN"].ToString()));
            param.Add(new SqlParameter("@BrokerName", dtMain.Rows[0]["BrokerName"].ToString()));
            param.Add(new SqlParameter("@OperatorName", Session["User"].ToString()));
            param.Add(new SqlParameter("@EntryDate", DateTime.Now.ToString("dd-MMM-yyyy")));
            param.Add(new SqlParameter("@IsActive", "1"));

            q = "insert into prabha.Sale_Sauda_Master([No],DataDate,MNo,PartyName,PMobile,PAddress,PGSTIN,PPAN,BrokerName,OperatorName,EntryDate,IsActive) ";
            q += "values(@No,@DataDate,@MNo,@PartyName,@PMobile,@PAddress,@PGSTIN,@PPAN,@BrokerName,@OperatorName,@EntryDate,@IsActive) select @@IDENTITY";

            dac = new DataAccessLayer();
            int msg = Convert.ToInt32(dac.Scalar(q, param));

            if (msg > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    q = "";
                    param = new List<SqlParameter>();
                    param.Add(new SqlParameter("@MasterID", msg));
                    param.Add(new SqlParameter("@ItemType", dt.Rows[i]["ItemType"].ToString()));
                    param.Add(new SqlParameter("@QIKG", dt.Rows[i]["QIKG"].ToString()));
                    param.Add(new SqlParameter("@AvgRate", dt.Rows[i]["AvgRate"].ToString()));

                    q = "insert into prabha.Sale_Sauda_Item_Info(MasterID,ItemType,QIKG,AvgRate) ";
                    q += "values(@MasterID,@ItemType,@QIKG,@AvgRate)";

                    dac = new DataAccessLayer();
                    dac.update(q, param);
                }

                ClientScript.RegisterStartupScript(this.GetType(), "saveSuccess",
                    "$(document).ready(function(){ showAlert('Sale Sauda saved successfully!', 'success'); });", true);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "insertErr",
                    "$(document).ready(function(){ showAlert('Error occurred while saving. Please try again.', 'error'); });", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "dupErr",
                "$(document).ready(function(){ showAlert('Data Already Exists!', 'error'); });", true);
        }
    }

    /* ============================================================
       DUPLICATE CHECK
       ============================================================ */
    public int chkData(string DDate, string PN)
    {
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@DataDate", Convert.ToDateTime(DDate).ToString("dd-MMM-yyyy")));
        param.Add(new SqlParameter("@PartyName", PN));

        string q = "select * from prabha.Sale_Sauda_Master "
                 + "where DataDate=@DataDate and PartyName=@PartyName";
        dac = new DataAccessLayer();
        DataTable dtOut = dac.GetDataTable(q, param);
        return dtOut.Rows.Count > 0 ? 1 : 0;
    }

    /* ============================================================
       SAUDA LIST (LinkButton)
       ============================================================ */
    protected void lBtnSaudaParty_Click(object sender, EventArgs e)
    {
        dt = new DataTable();
        param = new List<SqlParameter>();
        string q = "";

        string partyText = sPartyName.SelectedItem != null
            ? sPartyName.SelectedItem.Text.Trim()
            : "";

        if (partyText == "Other")
        {
            param.Add(new SqlParameter("@PartyName", partyText));
            q = "select ID,[No],DataDate,PartyName,BrokerName from prabha.Sale_Sauda_Master "
              + "where PartyName=@PartyName order by DataDate desc";
        }
        else
        {
            string[] sep = new string[] { " (Mobile No.: " };
            var result = partyText.Split(sep, StringSplitOptions.None);
            string Pname = result[0];
            string PMob = result[1].Substring(0, result[1].Length - 1);

            param.Add(new SqlParameter("@PartyName", Pname));
            param.Add(new SqlParameter("@PMobile", PMob));

            q = "select ID,[No],DataDate,PartyName,BrokerName from prabha.Sale_Sauda_Master "
              + "where PartyName=@PartyName and PMobile=@PMobile order by DataDate desc";
        }

        dac = new DataAccessLayer();
        dt = dac.GetDataTable(q, param);

        StringBuilder htmlTable = new StringBuilder();
        htmlTable.Append("<table class='table table-bordered' id='dataTable' cellspacing='0'>");
        htmlTable.Append("<thead><tr>"
            + "<th>Sl. No.</th>"
            + "<th>Sauda No. &amp; Date</th>"
            + "<th>Party Name</th>"
            + "<th>Supplier's Ref.</th>"
            + "<th></th>"
            + "</tr></thead><tbody>");

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string INVNo = GenInvoiceNo(
                dt.Rows[i]["No"].ToString(),
                dt.Rows[i]["DataDate"].ToString());

            htmlTable.Append("<tr>");
            htmlTable.Append("<td>" + (i + 1) + "</td>");
            htmlTable.Append("<td><a href='SO.aspx?ID=" + dt.Rows[i]["ID"]
                + "' target='_blank'>" + INVNo + ", "
                + Convert.ToDateTime(dt.Rows[i]["DataDate"].ToString()).ToString("dd/MM/yyyy")
                + "</a></td>");
            htmlTable.Append("<td>" + dt.Rows[i]["PartyName"] + "</td>");
            htmlTable.Append("<td>" + dt.Rows[i]["BrokerName"] + "</td>");
            htmlTable.Append("<td><a href='Sale.aspx?ID=" + dt.Rows[i]["ID"]
                + "' target='_blank'>Sale Entry</a></td>");
            htmlTable.Append("</tr>");
        }

        htmlTable.Append("</tbody></table>");
        DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
    }
}
