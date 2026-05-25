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

public partial class BillofSupply : System.Web.UI.Page
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
        q = "select * from [prabha].[Sale_Master_Data] where ID=@ID";
        dac = new DataAccessLayer();
        Session["DataMain"] = dac.GetDataTable(q, param);

        q = "";
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@ID", Request.QueryString["ID"].ToString()));
        q = "select * from [prabha].[Sale_Item_Info] where Master_ID=@ID";
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

            htmlTable.Append("<tr><td colspan='6' align='center'><span style='display:table-cell; vertical-align:top;'><img src='http://prabhasoftware.com/Rashmi Rice Logo (1).png' height='100px'/></span><span style='display:table-cell; vertical-align:top;'><span style='font-size:16pt; font-weight:bold;'> Rashmi Rice Mills Pvt. Ltd. </span></br><span style='font-size:8pt;'>Daniyawan Chandi Road, Hasanpur, Patna- 801304 </br>Mob.: 9304052349, 9334280057</br>Email: srirajbhog@gmail.com</br>CIN: U15312BR2014PTC022237</br>PAN No.: AAGCR9497P</br>GSTIN: 10AAGCR9497P1ZK</span></span></td></tr>");
            htmlTable.Append("<tr><td colspan='6' align='center'><span style='font-size:10pt; font-weight:bold;'>Bill of Supply </span></td></tr>");

            /*dtMain.Columns.Add("No", typeof(string));
            dtMain.Columns.Add("DataDate", typeof(string));
            dtMain.Columns.Add("PartyName", typeof(string));
            dtMain.Columns.Add("PMobile", typeof(string));
            dtMain.Columns.Add("PAddress", typeof(string));
            dtMain.Columns.Add("PGSTIN", typeof(string));
            dtMain.Columns.Add("PPAN", typeof(string));
            dtMain.Columns.Add("BOrderNo", typeof(string));
            dtMain.Columns.Add("BOrderDate", typeof(string));
            dtMain.Columns.Add("DespNo", typeof(string));
            dtMain.Columns.Add("DespDate", typeof(string));
            dtMain.Columns.Add("DespVNo", typeof(string));
            dtMain.Columns.Add("Destination", typeof(string));
            dtMain.Columns.Add("TareWt", typeof(string));

            dtMain.Columns.Add("BrokerName", typeof(string));*/
            string CInvNo = "";
            CInvNo = GenInvoiceNo(dtMain.Rows[0]["No"].ToString(), dtMain.Rows[0]["DataDate"].ToString());

            htmlTable.Append("<tr><td rowspan='6' align='left' valign='top'><b>Our Bank Details:</b></br>Federal Bank, Jamal Road, Patna</br>A/C No.: 12205500005021</br>IFSC Code: FDRL0001220</td>");
            if (dtMain.Rows[0]["ManualInvoice"].ToString() == "")
            {
                htmlTable.Append("<td colspan='5' align='left'>Invoice No.: <b>" + CInvNo + "</b> Dated: " + Convert.ToDateTime(dtMain.Rows[0]["DataDate"].ToString()).ToString("dd/MM/yyyy") + "</td>");
            }
            else
            {
                htmlTable.Append("<td colspan='5' align='left'>Invoice No.: <b>" + CInvNo + "</b> Dated: " + Convert.ToDateTime(dtMain.Rows[0]["DataDate"].ToString()).ToString("dd/MM/yyyy") + "</br>Manual Invoice No.: <b>" + dtMain.Rows[0]["ManualInvoice"].ToString() + "</b></td>");
            }


            htmlTable.Append("<tr><td colspan='5' align='left'>Supplier's Ref: " + dtMain.Rows[0]["BrokerName"].ToString() + "</td></tr>");
            htmlTable.Append("<tr><td colspan='5' align='left'>Buyer's Order No.: " + dtMain.Rows[0]["BOrderNo"].ToString() + " Dated: " + Convert.ToDateTime(dtMain.Rows[0]["BOrderDate"].ToString()).ToString("dd/MM/yyyy") + "</td>");

            htmlTable.Append("<tr><td colspan='5' align='left'>Despatch Doc. No.: " + dtMain.Rows[0]["DespNo"].ToString() + " Dated: " + Convert.ToDateTime(dtMain.Rows[0]["DespDate"].ToString()).ToString("dd/MM/yyyy") + "</td>");

            htmlTable.Append("<tr><td colspan='5' align='left'>Despatched Through: " + dtMain.Rows[0]["DespVNo"].ToString() + "</td></tr>");
            htmlTable.Append("<tr><td colspan='5' align='left'>Destination: " + dtMain.Rows[0]["Destination"].ToString() + "</td></tr>");

            htmlTable.Append("<tr><td align='left' valign='top'>Consignee: <b>" + dtMain.Rows[0]["PartyName"].ToString() + "</b></br>" + dtMain.Rows[0]["PAddress"].ToString() + "</br>GSTIN: " + dtMain.Rows[0]["PGSTIN"].ToString() + "</br>PAN No.: " + dtMain.Rows[0]["PPAN"].ToString() + "</br>Mob.: " + dtMain.Rows[0]["PMobile"].ToString() + "</td>");
            htmlTable.Append("<td colspan='5' align='left' valign='top'>Terms of Delivery:  </br>" + dtMain.Rows[0]["TOD"].ToString() + "</td></tr>");

            htmlTable.Append("<tr><td align='center'><b>Description of Goods</b> </td>");
            htmlTable.Append("<td align='center'><b>HSN/SAC</b></td>");
            htmlTable.Append("<td align='center'><b>Qty. In KG</b></td>");
            htmlTable.Append("<td colspan='2' align='center'><b>Rate/KG</b></td>");
            //htmlTable.Append("<td align='center'><b>Per</b></td>");
            htmlTable.Append("<td align='center'><b>Amount (In Rs.)</b></td></tr>");

            double am = 0;
            double wt = 0;

            double CGST = 0;
            double IGST = 0;
            double SGST = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                /*
                 dtData.Columns.Add("RiceType", typeof(string));
            dtData.Columns.Add("Rate", typeof(string));
                        
            dtData.Columns.Add("Quantity", typeof(string));
            dtData.Columns.Add("AvgWt", typeof(string));
                 */
                htmlTable.Append("<tr><td align='left'>" + dt.Rows[i]["RiceType"].ToString() + " (" + dt.Rows[i]["Quantity"].ToString() + "X" + dt.Rows[i]["AvgWt"].ToString() + ")</td>");
                if (dt.Rows[i]["RiceType"].ToString() == "Steam Bran")
                {
                    htmlTable.Append("<td align='center'>230240</td>");
                }
                else
                {
                    htmlTable.Append("<td align='center'>100610</td>");
                }
                htmlTable.Append("<td align='right'>" + (Convert.ToDouble(dt.Rows[i]["Quantity"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString())) + "</td>");
                wt = wt + (Convert.ToDouble(dt.Rows[i]["Quantity"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()));
                htmlTable.Append("<td colspan='2' align='right'>" + dt.Rows[i]["Rate"].ToString() + "</td>");
                //htmlTable.Append("<td align='left'>&nbsp;</td>");
                htmlTable.Append("<td align='right'>" + (Convert.ToDouble(dt.Rows[i]["Quantity"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * Convert.ToDouble(dt.Rows[i]["Rate"].ToString())) + "</td></tr>");
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


            htmlTable.Append("<td colspan='5' align='right'>CD (" + dtMain.Rows[0]["CD"].ToString() + "%)</td>");
            //htmlTable.Append("<td align='left'>&nbsp;</td>");
            htmlTable.Append("<td align='right'>(-)" + Math.Round((am * Convert.ToDouble(dtMain.Rows[0]["CD"].ToString()) / 100), 0) + "</td></tr>");


            double amwgst = 0;
            amwgst = am - Math.Round((am * Convert.ToDouble(dtMain.Rows[0]["CD"].ToString()) / 100), 0);

            htmlTable.Append("<td colspan='5' align='right'><b>Total</b></td>");


            htmlTable.Append("<td align='right'><b>" + amwgst.ToString() + "</b></td></tr>");

            
            htmlTable.Append("<td colspan='5' align='right'>CGST @2.5%</td>");

            htmlTable.Append("<td align='right'>" + CGST.ToString() + "</td></tr>");




            htmlTable.Append("<td colspan='5' align='right'>SGST @2.5%</td>");

            htmlTable.Append("<td align='right'>" + SGST.ToString() + "</td></tr>");


            htmlTable.Append("<td colspan='5' align='right'>IGST @5%</td>");

            htmlTable.Append("<td align='right'>" + IGST.ToString() + "</td></tr>");


            htmlTable.Append("<td colspan='5' align='right' style='white-space: nowrap;'>Freight (Adv.)</td>");
            //htmlTable.Append("<td align='left'>&nbsp;</td>");
            htmlTable.Append("<td align='right'>" + Math.Round(Convert.ToDouble(dtMain.Rows[0]["Freight"].ToString()),0) + "</td></tr>");

            double GT = 0;
            GT = Math.Round(amwgst + IGST + CGST + SGST + Convert.ToDouble(dtMain.Rows[0]["Freight"].ToString()), 0);

            htmlTable.Append("<td colspan='5' align='right'><b>Grand Total</b></td>");

            htmlTable.Append("<td align='right'><b>" + GT.ToString() + "</b></td></tr>");
            htmlTable.Append("<tr><td colspan='4' valign='top' align='left'><span style='font-size:8pt; font-weight:bold;'>RUPEES " + ConvertNumbertoWords(Convert.ToInt64(Math.Round(GT, 0))) + " ONLY</span></td>");//convert amount in words

            double GW = 0;
            double TW = 0;
            double NW = 0;
            TW = Convert.ToDouble(dtMain.Rows[0]["TareWt"].ToString());
            NW = wt;
            GW = TW + NW;

            htmlTable.Append("<td colspan='4' align='left'>G.W.: " + GW.ToString() + " KG</br>T.W.: " + TW.ToString() + " KG</br>N.W.: " + NW.ToString() + " KG</td></tr>");

            htmlTable.Append("<tr><td colspan='2' align='left'><span style='font-size:8pt;'><b>Declaration:</b></br>We declare that this invoice shows the actual price of the goods described and that all particulars are true and correct.</span></td>");
            htmlTable.Append("<td colspan='4' align='center'><b>For Rashmi Rice Mills Pvt. Ltd.</br></br>Authorised Signatory</b></td>");

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