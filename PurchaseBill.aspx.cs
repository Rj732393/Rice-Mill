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

public partial class PurchaseBill : System.Web.UI.Page
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
        // ============ DYNAMIC COMPANY DETAILS - SIRF YE BLOCK ADD HUA HAI ============
        string companyName = "";
        string companyAddress = "";
        string companyPhone = "";
        string companyEmail = "";
        string companyGST = "";
        string companyCIN = "";
        string companyPAN = "";
        string companyLogo = "";

        if (Session["CompanyID"] != null && Session["CompanyID"].ToString() != "0")
        {
            var cparam = new List<SqlParameter>();
            cparam.Add(new SqlParameter("@CompanyID", Session["CompanyID"].ToString()));
            DataAccessLayer cdac = new DataAccessLayer();
            DataTable dtComp = cdac.GetDataTable(
                "SELECT * FROM prabha.Companies WHERE CompanyID=@CompanyID", cparam);

            if (dtComp != null && dtComp.Rows.Count > 0)
            {
                companyName = dtComp.Rows[0]["CompanyName"].ToString();
                companyAddress = dtComp.Rows[0]["Address"].ToString();
                companyPhone = dtComp.Rows[0]["Phone"].ToString();
                companyEmail = dtComp.Rows[0]["Email"].ToString();
                companyGST = dtComp.Rows[0]["GSTNumber"] != DBNull.Value ? dtComp.Rows[0]["GSTNumber"].ToString() : "";
                companyCIN = dtComp.Rows[0]["CIN"] != DBNull.Value ? dtComp.Rows[0]["CIN"].ToString() : "";
                companyPAN = dtComp.Rows[0]["PAN"] != DBNull.Value ? dtComp.Rows[0]["PAN"].ToString() : "";
                companyLogo = dtComp.Rows[0]["LogoUrl"] != DBNull.Value ? dtComp.Rows[0]["LogoUrl"].ToString() : "";
            }
        }
        // ============ END COMPANY DETAILS ============

        string q = "";
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@ID", Request.QueryString["ID"].ToString()));
        q = "select * from [prabha].[Purchase_Master_Data] where ID=@ID";
        dac = new DataAccessLayer();
        Session["DataMain"] = dac.GetDataTable(q, param);

        q = "";
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@ID", Request.QueryString["ID"].ToString()));
        q = "select * from [prabha].[Purchase_Item_Info] where Master_ID=@ID";
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
            dtMain = (DataTable)Session["DataMain"];
            dt = (DataTable)Session["Data"];

            htmlTable = new StringBuilder();
            htmlTable.Append("<table class='table' runat='server' style='font-size:10pt; noWrap' id='printTable' cellspacing='0' border='1px'>");

            // ============ CHANGE 1: HARDCODED HEADER HATA KE DYNAMIC KIYA ============
            string logoHtml = !string.IsNullOrEmpty(companyLogo)
                ? "<span style='display:table-cell; vertical-align:top;'><img src='" + companyLogo + "' height='100px'/></span>"
                : "";
            string cinHtml = !string.IsNullOrEmpty(companyCIN) ? "</br>CIN: " + companyCIN : "";
            string panHtml = !string.IsNullOrEmpty(companyPAN) ? "</br>PAN No.: " + companyPAN : "";
            string gstHtml = !string.IsNullOrEmpty(companyGST) ? "</br>GSTIN: " + companyGST : "";
            string emailHtml = !string.IsNullOrEmpty(companyEmail) ? "</br>Email: " + companyEmail : "";

            htmlTable.Append("<tr><td colspan='8' align='center'>"
                + logoHtml
                + "<span style='display:table-cell; vertical-align:top;'>"
                + "<span style='font-size:16pt; font-weight:bold;'> " + companyName + " </span>"
                + "</br><span style='font-size:8pt;'>" + companyAddress
                + "</br>Mob.: " + companyPhone
                + emailHtml + cinHtml + panHtml + gstHtml
                + "</span></span></td></tr>");
            // ============ END CHANGE 1 ============

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

            string PurInv = GenInvoiceNo(dtMain.Rows[0]["No"].ToString(), dtMain.Rows[0]["DataDate"].ToString());
            if (dtMain.Rows[0]["MPurNo"].ToString() == "")
            {
                htmlTable.Append("<tr><td colspan='8' align='left'>Purchase No.: <b>" + PurInv + "</b></td></tr>");
            }
            else
            {
                htmlTable.Append("<tr><td colspan='8' align='left'>Purchase No.: <b>" + PurInv + "</b></br>Manual Purchase No.: <b>" + dtMain.Rows[0]["MPurNo"].ToString() + "</b></td></tr>");
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

                int lt = 0;
                if (dtMain.Rows[0]["PartyName"].ToString() == "SHIVAM BHANDAR (SUBODH JEE (Mobile No.: 9334280057)" || dtMain.Rows[0]["PartyName"].ToString() == "PRACHI TRADERS (MASURHI) (Mobile No.: 9334280057)" || dtMain.Rows[0]["PartyName"].ToString() == "Sankat Mochan Traders(Kunil jee) jahanabad (Mobile No.: 9334280057)")
                {
                    lt = 18;
                }
                else
                {
                    lt = 17;
                }

                if (Convert.ToDouble(dt.Rows[i]["Moisture"].ToString()) <= lt)
                {
                    LClaim += 0;
                }
                else
                {
                    LClaim += Math.Round((am + KhAmount + MAmount + DAmount + DMixAmount + OAmount) * (Convert.ToDouble(dt.Rows[i]["Moisture"].ToString()) - lt) / 100, 2);
                }

                if (i == 0 & dt.Rows.Count > 1)
                {
                    htmlTable.Append("<td colspan='2' style='vertical-align:bottom; border-bottom:none;' align='left'></td>");
                }
                if (i > 0 & i < (dt.Rows.Count - 1))
                {
                    htmlTable.Append("<td colspan='2' style='vertical-align:bottom; border-top:none; border-bottom:none;' align='left'></td>");
                }

                if (i == (dt.Rows.Count - 1))
                {
                    double LCD = Math.Round(tAmount * Convert.ToDouble(dtMain.Rows[0]["CD"].ToString()) / 100);

                    double LGK = Math.Round(tQuantity / 1000 * 25, 2);
                    if (LGK <= 100)
                    {
                        LGK = 100;
                    }
                    double Frt = Convert.ToDouble(dtMain.Rows[0]["FreightOwn"].ToString());
                    LClaim += (Convert.ToDouble(dtMain.Rows[0]["PTBags"].ToString()) * 0) + (Convert.ToDouble(dtMain.Rows[0]["JTBags"].ToString()) * 0);
                    double PAmount = 0;
                    PAmount = tAmount - LCD - LClaim - Frt;

                    double LAdvance = Convert.ToDouble(dtMain.Rows[0]["Advance"].ToString());
                    double OAdvance = 0;
                    if (LAdvance == 0)
                    {
                        OAdvance = LGK;
                    }
                    else
                    {
                        OAdvance = LAdvance;
                    }

                    double Brok = Convert.ToDouble(dtMain.Rows[0]["Brokerage"].ToString());
                    //double AGK = Math.Round(tQuantity / 1000 * 15, 2);
                    //double pb = 0;

                    FAmount = PAmount - OAdvance - Brok;

                    if (i == 0)
                    {
                        htmlTable.Append("<td colspan='2' style='vertical-align:bottom;' align='left'>");
                    }
                    else
                    {
                        htmlTable.Append("<td colspan='2' style='vertical-align:bottom; border-top:none;' align='left'>");
                    }
                    htmlTable.Append("Gross Wt.: " + (tQuantity + Convert.ToDouble(dtMain.Rows[0]["TareWt"].ToString())) + " KG</br>Tare Wt.: " + Convert.ToDouble(dtMain.Rows[0]["TareWt"].ToString()) + " KG</br>Net Wt.: " + tQuantity.ToString() + " KG");
                    htmlTable.Append("</br></br></br>Less: CD (" + dtMain.Rows[0]["CD"].ToString() + "%) Rs. " + LCD.ToString());
                    htmlTable.Append("</br>Less: GK (@25/Ton) Rs. " + LGK.ToString());
                    htmlTable.Append("</br>Less: Claim-(Moist./Others) Rs. " + LClaim.ToString());
                    htmlTable.Append("</br>Less: Freight(Own) Rs.  " + Frt.ToString());
                    htmlTable.Append("</br></br><b>Purchase Amount: Rs.  " + PAmount.ToString() + "</b>");
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

            htmlTable.Append("<tr><td colspan='6' align='center'><span style='font-size:8pt; font-weight:bold;'>RUPEES " + ConvertNumbertoWords(Convert.ToInt64(Math.Round(FAmount, 0))) + " ONLY</span></td></tr>");

            htmlTable.Append("<tr><td colspan='4' align='left'><span style='font-size:7pt;'><b>Note:</b></br>All claim disputes will be resolved within 2 working days from the date of issue of this Purchase Order and receipt of a copy of this Order to you.</br>दावे से सम्बंधित सभी विवादों का समाधान इस खरीद आदेश के जारी होने और इस आदेश की प्रति आपको प्राप्त होने की तारीख से 2 कार्य दिवसों के भीतर किया जाएगा।</span></td>");

            // ============ CHANGE 2: FOOTER MEIN BHI DYNAMIC COMPANY NAME ============
            htmlTable.Append("<td colspan='4' align='center'><b>For " + companyName + "</br></br>Authorised Signatory</b></td>");
            // ============ END CHANGE 2 ============

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
            invoiceNo = "RR/PUR/" + yr1 + "-" + yr2 + "/000" + a;
        }
        else if (a.Length == 2)
        {
            invoiceNo = "RR/PUR/" + yr1 + "-" + yr2 + "/00" + a;
        }
        else if (a.Length == 3)
        {
            invoiceNo = "RR/PUR/" + yr1 + "-" + yr2 + "/0" + a;
        }
        else
        {
            invoiceNo = "RR/PUR/" + yr1 + "-" + yr2 + "/" + a;
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