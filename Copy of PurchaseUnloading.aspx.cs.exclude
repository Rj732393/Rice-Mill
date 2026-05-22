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

            katano.Attributes["type"] = "number";
            pMN.Attributes["type"] = "number";
            pMN.Attributes["step"] = "1";
            
            truckno.Attributes["type"] = "text";
                        
            PBags.Attributes["type"] = "number";
            
            PTBags.Attributes["type"] = "number";
            PTBags.Value = "0";
            JBags.Attributes["type"] = "number";
            JBags.Value = "0";
            JTBags.Attributes["type"] = "number";
            JTBags.Value = "0";
            SaudaNo.Attributes["type"] = "text";
            SaudaDate.Attributes["type"] = "text";
            QIB.Attributes["type"] = "number";
            TWeight.Attributes["type"] = "number";
            TWeight.Attributes["step"] = ".001";
            QIK.Attributes["type"] = "number";
            QIK.Attributes["step"] = ".001";
            avgrate.Attributes["type"] = "number";
            avgrate.Attributes["step"] = ".01";
            moisture.Attributes["type"] = "number";
            moisture.Attributes["step"] = ".01";
            KhakhriPer.Attributes["type"] = "number";
            KhakhriPer.Attributes["step"] = ".01";
            KhakhriPer.Value = "0";
            KhakhriBag.Attributes["type"] = "number";
            KhakhriBag.Value = "0";
            
            MittiPer.Attributes["type"] = "number";
            MittiPer.Attributes["step"] = ".01";
            MittiPer.Value = "0";
            MittiBag.Attributes["type"] = "number";
            MittiBag.Value = "0";

            DaagiPer.Attributes["type"] = "number";
            DaagiPer.Attributes["step"] = ".01";
            DaagiPer.Value = "0";
            DaagiBag.Attributes["type"] = "number";
            DaagiBag.Value = "0";

            MixRicePer.Attributes["type"] = "number";
            MixRicePer.Attributes["step"] = ".01";
            MixRicePer.Value = "0";
            MixRiceBag.Attributes["type"] = "number";
            MixRiceBag.Value = "0";

            txtOthers.Value = "NA";
            OtherPer.Attributes["type"] = "number";
            OtherPer.Attributes["step"] = ".01";
            OtherPer.Value = "0";
            OtherBag.Attributes["type"] = "number";
            OtherBag.Value = "0";

            CD.Attributes["type"] = "number";
            CD.Attributes["step"] = ".01";
            CD.Value = "0";

            TFreight.Attributes["type"] = "number";
            TFreight.Attributes["step"] = ".01";
            Freight.Attributes["type"] = "number";
            Freight.Attributes["step"] = ".01";
            PFreight.Attributes["type"] = "number";
            PFreight.Attributes["step"] = ".01";
            PFreight.Value = "0";
            Advance.Attributes["type"] = "number";
            Advance.Attributes["step"] = ".01";
            Advance.Value = "0";
            brokerage.Attributes["type"] = "number";
            brokerage.Attributes["step"] = ".01";
            brokerage.Value = "0";

            //Party();
            PreData();
            calBalance();

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
        }
    }
    public void PreData()
    {
        try
        {
            string ID = Request.QueryString["ID"].ToString();
            if (ID == "")
            {
                Response.Redirect("PurchaseSauda.aspx");
            }
            else
            {
                string q = "";
                param = new List<SqlParameter>();
                param.Add(new SqlParameter("@ID", Request.QueryString["ID"].ToString()));
                q = "select * from [prabha].[Purchase_Sauda_Info] where ID=@ID";
                dac = new DataAccessLayer();
                dt = dac.GetDataTable(q, param);

                if (dt.Rows.Count > 0)
                {
                    sPartyName.Items.Clear();
                    sPartyName.Items.Add(dt.Rows[0]["PartyName"].ToString());
                    txtEmpName.Text = dt.Rows[0]["BrokerName"].ToString();
                    txtEmpName.Enabled = false;
                    SaudaNo.Value = GenInvoiceNo(dt.Rows[0]["No"].ToString(),dt.Rows[0]["DataDate"].ToString());
                    SaudaNo.Disabled = true;
                    SaudaDate.Value = Convert.ToDateTime(dt.Rows[0]["DataDate"].ToString()).ToString("dd-MMM-yyyy");
                    SaudaDate.Disabled = true;

                    if (Convert.ToDouble(dt.Rows[0]["RupaliWt"].ToString()) > 0)
                    {
                        string Value="Rupali-"+dt.Rows[0]["RupaliWt"].ToString()+"-"+dt.Rows[0]["RupaliRate"].ToString();
                        sPaddyType.Items.Add(new ListItem("Rupali", Value));
                    }
                    if (Convert.ToDouble(dt.Rows[0]["MansuriWt"].ToString()) > 0)
                    {
                        string Value = "Mansuri-" + dt.Rows[0]["MansuriWt"].ToString() + "-" + dt.Rows[0]["MansuriRate"].ToString();
                        sPaddyType.Items.Add(new ListItem("Mansuri", Value));
                    }
                    if (Convert.ToDouble(dt.Rows[0]["SonamWt"].ToString()) > 0)
                    {
                        string Value = "Sonam-" + dt.Rows[0]["SonamWt"].ToString() + "-" + dt.Rows[0]["SonamRate"].ToString();
                        sPaddyType.Items.Add(new ListItem("Sonam", Value));
                    }
                    if (Convert.ToDouble(dt.Rows[0]["HybridWt"].ToString()) > 0)
                    {
                        string Value = "Hybrid-" + dt.Rows[0]["HybridWt"].ToString() + "-" + dt.Rows[0]["HybridRate"].ToString();
                        sPaddyType.Items.Add(new ListItem("Hybrid", Value));
                    }

                }
                else
                {
                    Response.Redirect("PurchaseSauda.aspx");
                }
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("PurchaseSauda.aspx");
        }
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
    public void calBalance()
    {
        double OrderQ = 0;
        double Rate = 0;
        double RBalance = 0;
        double SQ = 0;
        string[] dataB = sPaddyType.SelectedValue.ToString().Split('-');
        Rate = Convert.ToDouble(dataB[2].ToString());
        avgrate.Value = Rate.ToString();
        avgrate.Disabled = true;
        OrderQ = Convert.ToDouble(dataB[1].ToString());

        string q = "";
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@SaudaNo", SaudaNo.Value.Trim()));
        param.Add(new SqlParameter("@SaudaDate", Convert.ToDateTime(SaudaDate.Value.Trim()).ToString("dd-MMM-yyyy")));
        q = "select ID from [prabha].[Purchase_Master_Data] where SaudaNo=@SaudaNo and SaudaDate=@SaudaDate";
        dac = new DataAccessLayer();
        DataTable dtID = dac.GetDataTable(q, param);

        if (dtID.Rows.Count > 0)
        {
            q = "";
            param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PaddyType", dataB[0].ToString()));
            q = "select FreshQuantity,KhakhriBags,MittiBags,DaagiBags,MixBags,OtherBags,AvgWt from [prabha].[Purchase_Item_Info] where PaddyType=@PaddyType and Master_ID in (";
            for (int i = 0; i < dtID.Rows.Count; i++)
            {
                q = q + dtID.Rows[i]["ID"].ToString() + ",";
            }
            q = q.Substring(0, (q.Length - 1));
            q += ")";

            dac = new DataAccessLayer();
            dtID = new DataTable();
            dtID = dac.GetDataTable(q, param);
            double tst;
            for (int i = 0; i < dtID.Rows.Count; i++)
            {
                tst = 0;
                tst += Convert.ToDouble(dtID.Rows[i]["FreshQuantity"].ToString()) + Convert.ToDouble(dtID.Rows[i]["KhakhriBags"].ToString());
                tst += Convert.ToDouble(dtID.Rows[i]["MittiBags"].ToString()) + Convert.ToDouble(dtID.Rows[i]["DaagiBags"].ToString());
                tst += Convert.ToDouble(dtID.Rows[i]["MixBags"].ToString()) + Convert.ToDouble(dtID.Rows[i]["OtherBags"].ToString());
                SQ += (tst * Convert.ToDouble(dtID.Rows[i]["AvgWt"].ToString()));

            }
        }
        else
        {
            SQ = 0;
        }
        RBalance = OrderQ - SQ;
        lblRBalance.Text = "Balance: "+ RBalance.ToString()+" KG";
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
        int chk=PartyValidation();
        if (chk == 1)
        {
            dtMain = new DataTable();
            dtMain.Columns.Add("No", typeof(string));
            dtMain.Columns.Add("DataDate", typeof(string));
            dtMain.Columns.Add("TruckNo", typeof(string));
            dtMain.Columns.Add("KantaNo", typeof(string));
            dtMain.Columns.Add("UnloadedAt", typeof(string));
            dtMain.Columns.Add("TareWt", typeof(string));
            dtMain.Columns.Add("PartyName", typeof(string));
            dtMain.Columns.Add("BrokerName", typeof(string));
            dtMain.Columns.Add("PBags", typeof(string));
            dtMain.Columns.Add("PTBags", typeof(string));
            dtMain.Columns.Add("JBags", typeof(string));
            dtMain.Columns.Add("JTBags", typeof(string));
            dtMain.Columns.Add("SaudaNo", typeof(string));
            dtMain.Columns.Add("SaudaDate", typeof(string));
            dtMain.Columns.Add("CD", typeof(string));
            dtMain.Columns.Add("TFreight", typeof(string));
            dtMain.Columns.Add("FreightOwn", typeof(string));
            dtMain.Columns.Add("FreightParty", typeof(string));
            dtMain.Columns.Add("Advance", typeof(string));
            dtMain.Columns.Add("Brokerage", typeof(string));
            dtMain.Columns.Add("MPurNo", typeof(string));

            rmain = dtMain.NewRow();
            rmain[0] = GenInvoiceNo();
            rmain[1] = Convert.ToDateTime(sdate.Value.Trim()).ToString("dd-MMM-yyyy");
            rmain[2] = truckno.Value.Trim();
            rmain[3] = katano.Value.Trim();
            rmain[4] = UAt.Value.Trim();
            rmain[5] = TWeight.Value.Trim();
            if (sPartyName.SelectedItem.Text.Trim() == "Other")
            {
                rmain[6] = pName.Value.Trim() + " (Mobile No.: " + pMN.Value.Trim() + ")";
            }
            else
            {
                rmain[6] = sPartyName.SelectedItem.Text.Trim();
            }
            rmain[7] = txtEmpName.Text.Trim();
            rmain[8] = PBags.Value.Trim();
            rmain[9] = PTBags.Value.Trim();
            rmain[10] = JBags.Value.Trim();
            rmain[11] = JTBags.Value.Trim();
            rmain[12] = SaudaNo.Value.Trim();
            rmain[13] = Convert.ToDateTime(SaudaDate.Value.Trim()).ToString("dd-MMM-yyyy");
            rmain[14] = CD.Value.Trim();
            rmain[15] = TFreight.Value.Trim();
            rmain[16] = Freight.Value.Trim();
            rmain[17] = PFreight.Value.Trim();
            rmain[18] = Advance.Value.Trim();
            rmain[19] = brokerage.Value.Trim();
            rmain[20] = MPNo.Value.Trim();

            dtMain.Rows.Add(rmain);
            Session["DataMain"] = null;
            Session["DataMain"] = dtMain;

            if (Session["Data"] == null)
            {
                DataTable dtData = new DataTable();
                dtData.Columns.Add("PaddyType", typeof(string));
                dtData.Columns.Add("AvgWt", typeof(string));
                dtData.Columns.Add("Rate", typeof(string));
                dtData.Columns.Add("FreshQuantity", typeof(string));
                dtData.Columns.Add("Moisture", typeof(string));
                dtData.Columns.Add("KhakhriPer", typeof(string));
                dtData.Columns.Add("KhakhriBags", typeof(string));
                dtData.Columns.Add("MittiPer", typeof(string));
                dtData.Columns.Add("MittiBags", typeof(string));
                dtData.Columns.Add("DaagiPer", typeof(string));
                dtData.Columns.Add("DaagiBags", typeof(string));
                dtData.Columns.Add("MixPer", typeof(string));
                dtData.Columns.Add("MixBags", typeof(string));
                dtData.Columns.Add("OtherName", typeof(string));
                dtData.Columns.Add("OtherPer", typeof(string));
                dtData.Columns.Add("OtherBags", typeof(string));

                dtRow = dtData.NewRow();
                dtRow[0] = sPaddyType.SelectedItem.Text.Trim();
                dtRow[1] = QIK.Value.Trim();
                dtRow[2] = avgrate.Value.Trim();
                dtRow[3] = QIB.Value.Trim();
                dtRow[4] = moisture.Value.Trim();
                dtRow[5] = KhakhriPer.Value.Trim();
                dtRow[6] = KhakhriBag.Value.Trim();
                dtRow[7] = MittiPer.Value.Trim();
                dtRow[8] = MittiBag.Value.Trim();
                dtRow[9] = DaagiPer.Value.Trim();
                dtRow[10] = DaagiBag.Value.Trim();
                dtRow[11] = MixRicePer.Value.Trim();
                dtRow[12] = MixRiceBag.Value.Trim();
                dtRow[13] = txtOthers.Value.Trim();
                dtRow[14] = OtherPer.Value.Trim();
                dtRow[15] = OtherBag.Value.Trim();
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
                    if (dr["PaddyType"] == sPaddyType.SelectedItem.Text.Trim())
                        dr.Delete();
                }
                dtData.AcceptChanges();

                dtRow = dtData.NewRow();
                dtRow[0] = sPaddyType.SelectedItem.Text.Trim();
                dtRow[1] = QIK.Value.Trim();
                dtRow[2] = avgrate.Value.Trim();
                dtRow[3] = QIB.Value.Trim();
                dtRow[4] = moisture.Value.Trim();
                dtRow[5] = KhakhriPer.Value.Trim();
                dtRow[6] = KhakhriBag.Value.Trim();
                dtRow[7] = MittiPer.Value.Trim();
                dtRow[8] = MittiBag.Value.Trim();
                dtRow[9] = DaagiPer.Value.Trim();
                dtRow[10] = DaagiBag.Value.Trim();
                dtRow[11] = MixRicePer.Value.Trim();
                dtRow[12] = MixRiceBag.Value.Trim();
                dtRow[13] = txtOthers.Value.Trim();
                dtRow[14] = OtherPer.Value.Trim();
                dtRow[15] = OtherBag.Value.Trim();
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

            htmlTable.Append("<tr><td colspan='8' align='center'><span style='display:table-cell; vertical-align:top;'><img src='http://prabhasoftware.com/Rashmi Rice Logo (1).png' height='100px'/></span><span style='display:table-cell; vertical-align:top;'><span style='font-size:16pt; font-weight:bold;'> Rashmi Rice Mills Pvt. Ltd. </span></br><span style='font-size:8pt;'>Daniyawan Chandi Road, Hasanpur, Patna- 801304 </br>Mob.: 9304052349, 9334280057</br>Email: srirajbhog@gmail.com</br>CIN: U15312BR2014PTC022237</br>PAN No.: AAGCR9497P</br>GSTIN: 10AAGCR9497P1ZK</span></span></td></tr>");
            htmlTable.Append("<tr><td colspan='8' align='center'><span style='font-size:10pt; font-weight:bold;'> PURCHASE VOUCHER CUM UNLOADING REPORT </span></td></tr>");
            
            /*dtMain.Columns.Add("No", typeof(string));
                dtMain.Columns.Add("DataDate", typeof(string));
                dtMain.Columns.Add("TruckNo", typeof(string));
                dtMain.Columns.Add("KantaNo", typeof(string));
                dtMain.Columns.Add("UnloadedAt", typeof(string));
                dtMain.Columns.Add("TareWt", typeof(string));
                dtMain.Columns.Add("PartyName", typeof(string));
                dtMain.Columns.Add("BrokerName", typeof(string));
                dtMain.Columns.Add("PBags", typeof(string));
                dtMain.Columns.Add("PTBags", typeof(string));
                dtMain.Columns.Add("JBags", typeof(string));
                dtMain.Columns.Add("JTBags", typeof(string));
                dtMain.Columns.Add("SaudaNo", typeof(string));
                dtMain.Columns.Add("SaudaDate", typeof(string));
                dtMain.Columns.Add("CD", typeof(string));
                dtMain.Columns.Add("TFreight", typeof(string));
                dtMain.Columns.Add("FreightOwn", typeof(string));
                dtMain.Columns.Add("FreightParty", typeof(string));
                dtMain.Columns.Add("Advance", typeof(string));
                dtMain.Columns.Add("Brokerage", typeof(string));*/
            if (dtMain.Rows[0]["MPurNo"].ToString() == "")
            {
                htmlTable.Append("<tr><td colspan='8' align='left'>Purchase No.: <b>" + dtMain.Rows[0]["No"].ToString() + "</b></td></tr>");
            }
            else
            {
                htmlTable.Append("<tr><td colspan='8' align='left'>Purchase No.: <b>" + dtMain.Rows[0]["No"].ToString() + "</b></br>Manual Purchase No.: <b>" + dtMain.Rows[0]["MPurNo"].ToString() + "</b></td></tr>");
            }
            htmlTable.Append("<tr><td colspan='6' align='left'>Date.: " + Convert.ToDateTime(dtMain.Rows[0]["DataDate"].ToString()).ToString("dd/MM/yyyy") + "</td>");
            htmlTable.Append("<td align='left'>Plastic Bags: " + dtMain.Rows[0]["PBags"].ToString() + "</td>");
            htmlTable.Append("<td align='left'>Plastic Torn Bags: " + dtMain.Rows[0]["PTBags"].ToString() + "</td></tr>");
            htmlTable.Append("<tr><td colspan='6' align='left'>Truck No.: " + dtMain.Rows[0]["TruckNo"].ToString() + "</td>");
            htmlTable.Append("<td align='left'>Jute Bags: " + dtMain.Rows[0]["JBags"].ToString() + "</td>");
            htmlTable.Append("<td align='left'>Jute Torn Bags: " + dtMain.Rows[0]["JTBags"].ToString() + "</td></tr>");
            htmlTable.Append("<tr><td colspan='6' align='left'>Kanta No.: " + dtMain.Rows[0]["KantaNo"].ToString() + "</td>");
            htmlTable.Append("<td colspan='2' align='left'>Unloaded at: " + dtMain.Rows[0]["UnloadedAt"].ToString() + "</td></tr>");
            htmlTable.Append("<tr><td colspan='8' align='left'>Party Name: " + dtMain.Rows[0]["PartyName"].ToString() + "</td></tr>");
            htmlTable.Append("<tr><td colspan='6' align='left'>Broker's Name: " + dtMain.Rows[0]["BrokerName"].ToString() + "</td>");
            htmlTable.Append("<td align='left'>Sauda No.: " + dtMain.Rows[0]["SaudaNo"].ToString() + "</td>");
            htmlTable.Append("<td align='left'>Sauda Date: " + Convert.ToDateTime(dtMain.Rows[0]["SaudaDate"].ToString()).ToString("dd/MM/yyyy") + "</td></tr>");
            htmlTable.Append("<tr><td align='left'><b>Paddy Report:</b> </td>");
            htmlTable.Append("<td align='center'><b>Qty. In Bags</b></td>");
            htmlTable.Append("<td align='center'><b>Qty. In KG</b></td>");
            htmlTable.Append("<td align='center'><b>%</b></td>");
            htmlTable.Append("<td align='center'><b>Rate</b></td>");
            htmlTable.Append("<td align='center'><b>Amount (In Rs.)</b></td>");
            htmlTable.Append("<td colspan='2' align='center'><b>Calculation</b></td></tr>");

            int tBags = 0;
            double KhRate = 0;
            double MRate = 0;
            double DRate = 0;
            double DMixRate = 0;
            double ORate = 0;

            double am = 0;
            double KhAmount = 0;
            double MAmount = 0;
            double DAmount = 0;
            double DMixAmount = 0;
            double OAmount = 0;

            double tQuantity = 0;
            double tAmount = 0;
            double LClaim = 0;

            double FAmount = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                /*dt.Columns.Add("PaddyType", typeof(string));
                dt.Columns.Add("AvgWt", typeof(string));
                dt.Columns.Add("Rate", typeof(string));
                dt.Columns.Add("FreshQuantity", typeof(string));
                dt.Columns.Add("Moisture", typeof(string));
                dt.Columns.Add("KhakhriPer", typeof(string));
                dt.Columns.Add("KhakhriBags", typeof(string));
                dt.Columns.Add("MittiPer", typeof(string));
                dt.Columns.Add("MittiBags", typeof(string));
                dt.Columns.Add("DaagiPer", typeof(string));
                dt.Columns.Add("DaagiBags", typeof(string));
                dt.Columns.Add("MixPer", typeof(string));
                dt.Columns.Add("MixBags", typeof(string));
                dt.Columns.Add("OtherPer", typeof(string));
                dt.Columns.Add("OtherBags", typeof(string));*/
                htmlTable.Append("<tr><td align='left' style='vertical-align:top;'>" + (i + 1).ToString() + " " + dt.Rows[i]["PaddyType"].ToString() + " (Avg Wt.: " + dt.Rows[i]["AvgWt"].ToString() + " KG)</br>&nbsp;&nbsp;&nbsp;&nbsp;" + (i + 1).ToString() + ".1 Fresh");
                htmlTable.Append("</br>&nbsp;&nbsp;&nbsp;&nbsp;" + (i + 1).ToString() + ".2 Moisture");
                htmlTable.Append("</br>&nbsp;&nbsp;&nbsp;&nbsp;" + (i + 1).ToString() + ".3 Khakhri");
                htmlTable.Append("</br>&nbsp;&nbsp;&nbsp;&nbsp;" + (i + 1).ToString() + ".4 Mitti");
                htmlTable.Append("</br>&nbsp;&nbsp;&nbsp;&nbsp;" + (i + 1).ToString() + ".5 Daagi");
                htmlTable.Append("</br>&nbsp;&nbsp;&nbsp;&nbsp;" + (i + 1).ToString() + ".6 Mix Rice");
                htmlTable.Append("</br>&nbsp;&nbsp;&nbsp;&nbsp;" + (i + 1).ToString() + ".7 Other (" + dt.Rows[i]["OtherName"].ToString() + ")</td>");
                
                

                htmlTable.Append("<td align='right' style='vertical-align:top;'></br>" + dt.Rows[i]["FreshQuantity"].ToString() + "</br>");
                htmlTable.Append("</br>" + dt.Rows[i]["KhakhriBags"].ToString());
                htmlTable.Append("</br>" + dt.Rows[i]["MittiBags"].ToString());
                htmlTable.Append("</br>" + dt.Rows[i]["DaagiBags"].ToString());
                htmlTable.Append("</br>" + dt.Rows[i]["MixBags"].ToString());
                htmlTable.Append("</br>" + dt.Rows[i]["OtherBags"].ToString() + "</td>");

                htmlTable.Append("<td align='right' style='vertical-align:top;'></br>" + Math.Round(Convert.ToDouble(dt.Rows[i]["FreshQuantity"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()), 2).ToString() + "</br>");
                htmlTable.Append("</br>" + Math.Round(Convert.ToDouble(dt.Rows[i]["KhakhriBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()), 2).ToString());
                htmlTable.Append("</br>" + Math.Round(Convert.ToDouble(dt.Rows[i]["MittiBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()), 2).ToString());
                htmlTable.Append("</br>" + Math.Round(Convert.ToDouble(dt.Rows[i]["DaagiBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()), 2).ToString());
                htmlTable.Append("</br>" + Math.Round(Convert.ToDouble(dt.Rows[i]["MixBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()), 2).ToString());
                htmlTable.Append("</br>" + Math.Round(Convert.ToDouble(dt.Rows[i]["OtherBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()), 2).ToString() + "</td>");


                htmlTable.Append("<td align='right' style='vertical-align:top;'></br></br>" + dt.Rows[i]["Moisture"].ToString());
                htmlTable.Append("</br>" + dt.Rows[i]["KhakhriPer"].ToString());
                htmlTable.Append("</br>" + dt.Rows[i]["MittiPer"].ToString());
                htmlTable.Append("</br>" + dt.Rows[i]["DaagiPer"].ToString());
                htmlTable.Append("</br>" + dt.Rows[i]["MixPer"].ToString());
                htmlTable.Append("</br>" + dt.Rows[i]["OtherPer"].ToString() + "</td>");


                htmlTable.Append("<td align='right' style='vertical-align:top;'></br>" + Math.Round(Convert.ToDouble(dt.Rows[i]["Rate"].ToString()), 2).ToString() + "</br>");
                am = Math.Round(Convert.ToDouble(dt.Rows[i]["FreshQuantity"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * Convert.ToDouble(dt.Rows[i]["Rate"].ToString()), 2);

                if (Convert.ToDouble(dt.Rows[i]["KhakhriPer"].ToString()) <= 2)
                {
                    htmlTable.Append("</br>" + Math.Round(Convert.ToDouble(dt.Rows[i]["Rate"].ToString()), 2).ToString());
                    KhAmount = Math.Round(Convert.ToDouble(dt.Rows[i]["KhakhriBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * Convert.ToDouble(dt.Rows[i]["Rate"].ToString()), 2);
                }
                else
                {
                    KhRate = Math.Round(Convert.ToDouble(dt.Rows[i]["Rate"].ToString()) - (Convert.ToDouble(dt.Rows[i]["Rate"].ToString()) * (Convert.ToDouble(dt.Rows[i]["KhakhriPer"].ToString()) - 2) / 100), 2);
                    htmlTable.Append("</br>" + KhRate.ToString());
                    KhAmount = Math.Round(Convert.ToDouble(dt.Rows[i]["KhakhriBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * KhRate, 2);
                }

                if (Convert.ToDouble(dt.Rows[i]["MittiPer"].ToString()) <= 0)
                {
                    htmlTable.Append("</br>" + Math.Round(Convert.ToDouble(dt.Rows[i]["Rate"].ToString()), 2).ToString());
                    MAmount = Math.Round(Convert.ToDouble(dt.Rows[i]["MittiBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * Convert.ToDouble(dt.Rows[i]["Rate"].ToString()), 2);
                }
                else
                {
                    MRate = Math.Round(Convert.ToDouble(dt.Rows[i]["Rate"].ToString()) - (Convert.ToDouble(dt.Rows[i]["Rate"].ToString()) * (Convert.ToDouble(dt.Rows[i]["MittiPer"].ToString()) - 0) / 100), 2);
                    htmlTable.Append("</br>" + MRate.ToString());
                    MAmount = Math.Round(Convert.ToDouble(dt.Rows[i]["MittiBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * MRate, 2);
                }

                if (Convert.ToDouble(dt.Rows[i]["DaagiPer"].ToString()) <= 0)
                {
                    htmlTable.Append("</br>" + Math.Round(Convert.ToDouble(dt.Rows[i]["Rate"].ToString()), 2).ToString());
                    DAmount = Math.Round(Convert.ToDouble(dt.Rows[i]["DaagiBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * Convert.ToDouble(dt.Rows[i]["Rate"].ToString()), 2);
                }
                else
                {
                    DRate = Math.Round(Convert.ToDouble(dt.Rows[i]["Rate"].ToString()) - (Convert.ToDouble(dt.Rows[i]["Rate"].ToString()) * (Convert.ToDouble(dt.Rows[i]["DaagiPer"].ToString()) - 0) / 100), 2);
                    htmlTable.Append("</br>" + DRate.ToString());
                    DAmount = Math.Round(Convert.ToDouble(dt.Rows[i]["DaagiBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * DRate, 2);
                }


                if (Convert.ToDouble(dt.Rows[i]["MixPer"].ToString()) <= 0)
                {
                    htmlTable.Append("</br>" + Math.Round(Convert.ToDouble(dt.Rows[i]["Rate"].ToString()), 2).ToString());
                    DMixAmount = Math.Round(Convert.ToDouble(dt.Rows[i]["MixBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * Convert.ToDouble(dt.Rows[i]["Rate"].ToString()), 2);
                }
                else
                {
                    DMixRate = Math.Round(Convert.ToDouble(dt.Rows[i]["Rate"].ToString()) - (Convert.ToDouble(dt.Rows[i]["Rate"].ToString()) * (Convert.ToDouble(dt.Rows[i]["MixPer"].ToString()) - 0) / 100), 2);
                    htmlTable.Append("</br>" + DMixRate.ToString());
                    DMixAmount = Math.Round(Convert.ToDouble(dt.Rows[i]["MixBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * DMixRate, 2);
                }

                if (Convert.ToDouble(dt.Rows[i]["OtherPer"].ToString()) <= 0)
                {
                    htmlTable.Append("</br>" + Math.Round(Convert.ToDouble(dt.Rows[i]["Rate"].ToString()), 2).ToString());
                    OAmount = Math.Round(Convert.ToDouble(dt.Rows[i]["OtherBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * Convert.ToDouble(dt.Rows[i]["Rate"].ToString()), 2);
                }
                else
                {
                    ORate = Math.Round(Convert.ToDouble(dt.Rows[i]["Rate"].ToString()) - (Convert.ToDouble(dt.Rows[i]["Rate"].ToString()) * (Convert.ToDouble(dt.Rows[i]["OtherPer"].ToString()) - 0) / 100), 2);
                    htmlTable.Append("</br>" + ORate.ToString());
                    OAmount = Math.Round(Convert.ToDouble(dt.Rows[i]["OtherBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * ORate, 2);
                }

                htmlTable.Append("<td align='right' style='vertical-align:top;'></br>" + am.ToString());
                htmlTable.Append("</br></br>" + KhAmount.ToString());
                htmlTable.Append("</br>" + MAmount.ToString());
                htmlTable.Append("</br>" + DAmount.ToString());
                htmlTable.Append("</br>" + DMixAmount.ToString());
                htmlTable.Append("</br>" + OAmount.ToString() + "</td>");

                tBags += Convert.ToInt32(dt.Rows[i]["FreshQuantity"].ToString()) + Convert.ToInt32(dt.Rows[i]["KhakhriBags"].ToString()) + Convert.ToInt32(dt.Rows[i]["MittiBags"].ToString()) + Convert.ToInt32(dt.Rows[i]["DaagiBags"].ToString()) + Convert.ToInt32(dt.Rows[i]["MixBags"].ToString()) + Convert.ToInt32(dt.Rows[i]["OtherBags"].ToString());
                tQuantity = Math.Round(tBags * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()), 2);
                tAmount += am + KhAmount + MAmount + DAmount + DMixAmount + OAmount;
                if (Convert.ToDouble(dt.Rows[i]["Moisture"].ToString()) <= 17)
                {
                    LClaim += 0;
                }
                else
                {
                    if (dtMain.Rows[0]["PartyName"].ToString() == "SHIVAM BHANDAR (SUBODH JEE (Mobile No.: 9334280057)")
                    {
                        LClaim += Math.Round(tAmount * (Convert.ToDouble(dt.Rows[i]["Moisture"].ToString()) - 18) / 100, 2);
                    }
                    else
                    {
                        LClaim += Math.Round(tAmount * (Convert.ToDouble(dt.Rows[i]["Moisture"].ToString()) - 17) / 100, 2);
                    }
                }
                if (i == 0 & dt.Rows.Count > 1)
                {
                    htmlTable.Append("<td colspan='2' style='vertical-align:bottom; border-bottom:none;' align='left'></td>");
                }
                if (i > 0 & i < (dt.Rows.Count - 1))
                {
                    htmlTable.Append("<td colspan='2' style='vertical-align:bottom; border-top:none; border-bottom:none;' align='left'></td>");
                }
                
                if (i == (dt.Rows.Count-1))
                {
                    double LCD = Math.Round(tAmount * Convert.ToDouble(CD.Value.Trim()) / 100);
                    
                    double LGK = Math.Round(tQuantity / 1000 * 25, 2);
                    if (LGK <= 100)
                    {
                        LGK = 100;
                    }
                    double Frt = Convert.ToDouble(Freight.Value.Trim());
                    LClaim += (Convert.ToDouble(PTBags.Value.Trim()) * 0) + (Convert.ToDouble(JTBags.Value.Trim()) * 0);
                    double PAmount = tAmount - LCD - LGK - LClaim - Frt;
                    double LAdvance = Convert.ToDouble(Advance.Value.Trim());
                    double Brok = Convert.ToDouble(brokerage.Value.Trim());
                    //double AGK = Math.Round(tQuantity / 1000 * 15, 2);
                    //double pb = 0;

                    FAmount = PAmount - LAdvance - Brok;

                    if (i == 0)
                    {
                        htmlTable.Append("<td colspan='2' style='vertical-align:bottom;' align='left'>");
                    }
                    else
                    {
                        htmlTable.Append("<td colspan='2' style='vertical-align:bottom; border-top:none;' align='left'>");
                    }
                    htmlTable.Append("Gross Wt.: " + (tQuantity + Convert.ToDouble(TWeight.Value.Trim())) + " KG</br>Tare Wt.: " + Convert.ToDouble(TWeight.Value.Trim()) + " KG</br>Net Wt.: " + tQuantity.ToString() + " KG");
                    htmlTable.Append("</br></br></br>Less: CD (" + dtMain.Rows[0]["CD"].ToString() + "%) Rs. " + LCD.ToString());
                    htmlTable.Append("</br>Less: GK (@25/Ton) Rs. " + LGK.ToString());
                    htmlTable.Append("</br>Less: Claim-(Moist./Others) Rs. " + LClaim.ToString());
                    htmlTable.Append("</br>Less: Freight(Own) Rs.  " + Frt.ToString());
                    htmlTable.Append("</br></br><b>Purchase Amount: Rs.  " + PAmount.ToString()+"</b>");
                    htmlTable.Append("</br></br>Less: Advance Rs. " + LAdvance.ToString());
                    htmlTable.Append("</br>Less: Brokerage (of party) Rs. " + Brok.ToString());
                    //htmlTable.Append("</br>Add: Previous Bal. (Party) Rs. " + pb.ToString());
                    //htmlTable.Append("</br>Add: GK (Recd in Mill) Rs. " + AGK.ToString());

                    htmlTable.Append("</td>");
                    
                }
                htmlTable.Append("</tr>");

            }
            htmlTable.Append("<tr><td align='left'>Total</td>");



            htmlTable.Append("<td align='right'>" + tBags.ToString() + "</td>");
            htmlTable.Append("<td align='right'>" + tQuantity.ToString() + "</td>");
            htmlTable.Append("<td align='right'></td>");
            htmlTable.Append("<td align='right'></td>");
            htmlTable.Append("<td align='right'>" + tAmount.ToString() + "</td>");
            htmlTable.Append("<td colspan='2' rowspan='3' align='left'  style='vertical-align:top;'>Remarks: </td></tr>");
            htmlTable.Append("<tr><td align='left' colspan='5'>G. Total</td>");
            //htmlTable.Append("<td align='left'></td>");
            //htmlTable.Append("<td align='left'></td>");
            //htmlTable.Append("<td align='left'></td>");
            //htmlTable.Append("<td align='left'></td>");
            htmlTable.Append("<td align='right'><b>" + Math.Round(FAmount, 0).ToString() + "</b></td></tr>");
            
            htmlTable.Append("<tr><td colspan='6' align='center'><span style='font-size:8pt; font-weight:bold;'>RUPEES " + ConvertNumbertoWords(Convert.ToInt64(Math.Round(FAmount, 0))) + " ONLY</span></td></tr>");//convert amount in words

            htmlTable.Append("<tr><td colspan='4' align='left'><span style='font-size:7pt;'><b>Note:</b></br>All claim disputes will be resolved within 2 working days from the date of issue of this Purchase Order and receipt of a copy of this Order to you.</br>दावे से सम्बंधित सभी विवादों का समाधान इस खरीद आदेश के जारी होने और इस आदेश की प्रति आपको प्राप्त होने की तारीख से 2 कार्य दिवसों के भीतर किया जाएगा।</span></td>");
            htmlTable.Append("<td colspan='4' align='center'><b>For Rashmi Rice Mills Pvt. Ltd.</br></br>Authorised Signatory</b></td>");
            htmlTable.Append("</table>");
            
        }
        DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
    }
    public void CallPrint(string strid) {
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
                cmd.CommandText = "SELECT [BrokerName] FROM [prabha].[BrokerInfo] where [BrokerName] like ''+@SearchEmpName+'%'";
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

        q = "select max([No]) from prabha.Purchase_Master_Data where DataDate>=@DataDate1 and DataDate<=@DataDate2";
        dac = new DataAccessLayer();
        object test = dac.Scalar(q, param);
        if (test == DBNull.Value)
        {
            invoiceNo = "RR/PUR/" + yr1 + "-" + yr2 + "/0001";
        }
        else
        {
            if ((Convert.ToInt32(test) + 1).ToString().Length == 1)
            {
                invoiceNo = "RR/PUR/" + yr1 + "-" + yr2 + "/000" + (Convert.ToInt32(test) + 1);
            }
            else if ((Convert.ToInt32(test) + 1).ToString().Length == 2)
            {
                invoiceNo = "RR/PUR/" + yr1 + "-" + yr2 + "/00" + (Convert.ToInt32(test) + 1);
            }
            else if ((Convert.ToInt32(test) + 1).ToString().Length == 3)
            {
                invoiceNo = "RR/PUR/" + yr1 + "-" + yr2 + "/0" + (Convert.ToInt32(test) + 1);
            }
            else
            {
                invoiceNo = "RR/PUR/" + yr1 + "-" + yr2 + "/" + Convert.ToInt32(test) + 1;
            }

        }
        return invoiceNo;
    }
    public void insertPurchaseData()
    {
        
        dtMain = (DataTable)Session["DataMain"];
        dt = (DataTable)Session["Data"];

        int chkValid = chkData(dtMain.Rows[0]["DataDate"].ToString(), dtMain.Rows[0]["TruckNo"].ToString(), dtMain.Rows[0]["PartyName"].ToString());

        if (chkValid == 0)
        {
            int msg = 0;
            string q = "";
            param = new List<SqlParameter>();//Emp_Id

            string[] Inv = dtMain.Rows[0]["No"].ToString().Split('/');
            param.Add(new SqlParameter("@No", Inv[3]));
            param.Add(new SqlParameter("@MPurNo", dtMain.Rows[0]["MPurNo"].ToString()));
            param.Add(new SqlParameter("@DataDate", Convert.ToDateTime(dtMain.Rows[0]["DataDate"].ToString()).ToString("dd-MMM-yyyy")));
            param.Add(new SqlParameter("@TruckNo", dtMain.Rows[0]["TruckNo"].ToString()));
            param.Add(new SqlParameter("@KantaNo", dtMain.Rows[0]["KantaNo"].ToString()));
            param.Add(new SqlParameter("@UnloadedAt", dtMain.Rows[0]["UnloadedAt"].ToString()));
            param.Add(new SqlParameter("@TareWt", dtMain.Rows[0]["TareWt"].ToString()));
            param.Add(new SqlParameter("@PartyName", dtMain.Rows[0]["PartyName"].ToString()));
            param.Add(new SqlParameter("@BrokerName", dtMain.Rows[0]["BrokerName"].ToString()));
            param.Add(new SqlParameter("@PBags", dtMain.Rows[0]["PBags"].ToString()));
            param.Add(new SqlParameter("@PTBags", dtMain.Rows[0]["PTBags"].ToString()));
            param.Add(new SqlParameter("@JBags", dtMain.Rows[0]["JBags"].ToString()));
            param.Add(new SqlParameter("@JTBags", dtMain.Rows[0]["JTBags"].ToString()));
            param.Add(new SqlParameter("@SaudaNo", dtMain.Rows[0]["SaudaNo"].ToString()));
            param.Add(new SqlParameter("@SaudaDate", dtMain.Rows[0]["SaudaDate"].ToString()));
            param.Add(new SqlParameter("@CD", dtMain.Rows[0]["CD"].ToString()));
            param.Add(new SqlParameter("@TFreight", dtMain.Rows[0]["TFreight"].ToString()));
            param.Add(new SqlParameter("@FreightOwn", dtMain.Rows[0]["FreightOwn"].ToString()));
            param.Add(new SqlParameter("@FreightParty", dtMain.Rows[0]["FreightParty"].ToString()));
            param.Add(new SqlParameter("@Advance", dtMain.Rows[0]["Advance"].ToString()));
            param.Add(new SqlParameter("@Brokerage", dtMain.Rows[0]["Brokerage"].ToString()));

            param.Add(new SqlParameter("@OperatorName", Session["User"].ToString()));
            param.Add(new SqlParameter("@EntryDate", Convert.ToDateTime(System.DateTime.Now).ToString("dd-MMM-yyyy")));

            q = "insert into prabha.Purchase_Master_Data([No],MPurNo,DataDate,TruckNo,KantaNo,UnloadedAt,TareWt,PartyName,BrokerName,";
            q += "PBags,PTBags,JBags,JTBags,SaudaNo,SaudaDate,CD,TFreight,FreightOwn,FreightParty,Advance,Brokerage,OperatorName,EntryDate) ";
            q += " values(@No,@MPurNo,@DataDate,@TruckNo,@KantaNo,@UnloadedAt,@TareWt,@PartyName,@BrokerName,";
            q += "@PBags,@PTBags,@JBags,@JTBags,@SaudaNo,@SaudaDate,@CD,@TFreight,@FreightOwn,@FreightParty,@Advance,@Brokerage,@OperatorName,@EntryDate) select @@IDENTITY";
            dac = new DataAccessLayer();

            msg = Convert.ToInt32(dac.Scalar(q, param));


            if (msg > 0)
            {
                int OutMsg = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    q = "";
                    param = new List<SqlParameter>();
                    param.Add(new SqlParameter("@Master_ID", msg));
                    param.Add(new SqlParameter("@PaddyType", dt.Rows[i]["PaddyType"].ToString()));
                    param.Add(new SqlParameter("@AvgWt", dt.Rows[i]["AvgWt"].ToString()));
                    param.Add(new SqlParameter("@Rate", dt.Rows[i]["Rate"].ToString()));
                    param.Add(new SqlParameter("@FreshQuantity", dt.Rows[i]["FreshQuantity"].ToString()));
                    param.Add(new SqlParameter("@Moisture", dt.Rows[i]["Moisture"].ToString()));
                    param.Add(new SqlParameter("@KhakhriPer", dt.Rows[i]["KhakhriPer"].ToString()));
                    param.Add(new SqlParameter("@KhakhriBags", dt.Rows[i]["KhakhriBags"].ToString()));
                    param.Add(new SqlParameter("@MittiPer", dt.Rows[i]["MittiPer"].ToString()));
                    param.Add(new SqlParameter("@MittiBags", dt.Rows[i]["MittiBags"].ToString()));
                    param.Add(new SqlParameter("@DaagiPer", dt.Rows[i]["DaagiPer"].ToString()));
                    param.Add(new SqlParameter("@DaagiBags", dt.Rows[i]["DaagiBags"].ToString()));
                    param.Add(new SqlParameter("@MixPer", dt.Rows[i]["MixPer"].ToString()));
                    param.Add(new SqlParameter("@MixBags", dt.Rows[i]["MixBags"].ToString()));
                    param.Add(new SqlParameter("@OtherName", dt.Rows[i]["OtherName"].ToString()));
                    param.Add(new SqlParameter("@OtherPer", dt.Rows[i]["OtherPer"].ToString()));
                    param.Add(new SqlParameter("@OtherBags", dt.Rows[i]["OtherBags"].ToString()));

                    q = "insert into prabha.Purchase_Item_Info(Master_ID,PaddyType,AvgWt,Rate,FreshQuantity,Moisture,KhakhriPer,";
                    q += "KhakhriBags,MittiPer,MittiBags,DaagiPer,DaagiBags,MixPer,MixBags,OtherName,OtherPer,OtherBags) ";
                    q += "values(@Master_ID,@PaddyType,@AvgWt,@Rate,@FreshQuantity,@Moisture,@KhakhriPer,";
                    q += "@KhakhriBags,@MittiPer,@MittiBags,@DaagiPer,@DaagiBags,@MixPer,@MixBags,@OtherName,@OtherPer,@OtherBags) ";

                    dac = new DataAccessLayer();
                    OutMsg = dac.update(q, param);

                }

                if (sPartyName.SelectedItem.Text.Trim() == "Other")
                {
                    string source = dtMain.Rows[0]["PartyName"].ToString();
                    string[] stringSeparators = new string[] { " (Mobile No.: " };
                    var result = source.Split(stringSeparators, StringSplitOptions.None);

                    string Pname = result[0];
                    string PMobile = result[1].Substring(0, (result[1].Length - 1));

                    q = "";
                    param = new List<SqlParameter>();
                    param.Add(new SqlParameter("@Party_Name", Pname));
                    param.Add(new SqlParameter("@Party_Mobile", PMobile));
                    q = "insert into prabha.Purchase_Party_Info(Party_Name,Party_Mobile) values(@Party_Name,@Party_Mobile)";
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
    public int chkData(string DDate,string TNo,string PN)
    {
        int tst = 0;
        DataTable dtOut = new DataTable();
        string q = "";
        param = new List<SqlParameter>();//Emp_Id

        param.Add(new SqlParameter("@DataDate", Convert.ToDateTime(DDate).ToString("dd-MMM-yyyy")));
        param.Add(new SqlParameter("@TruckNo", TNo));
        param.Add(new SqlParameter("@PartyName", PN));

        q = "select * from prabha.Purchase_Master_Data where DataDate=@DataDate and TruckNo=@TruckNo and PartyName=@PartyName";
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
    protected void sPaddyType_SelectedIndexChanged(object sender, EventArgs e)
    {
        calBalance();
        dataDisplay();
    }
}  
