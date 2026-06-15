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
    DataRow dtRow;
    DataRow rmain;
    List<SqlParameter> param;
    DataAccessLayer dac;
    string script = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Session["User"] == null || Session["CompanyID"] == null)
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
        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
        q = "select distinct PartyName from prabha.Purchase_Master_Data where CompanyID=@CompanyID order by PartyName";
        dac = new DataAccessLayer();
        dt = dac.GetDataTable(q, param);

        sPartyName.DataSource = dt;
        sPartyName.DataTextField = "PartyName";
        sPartyName.DataValueField = "PartyName";
        sPartyName.DataBind();
        sPartyName.Items.Insert(0,"--Select One--");
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
    
    public void checkData()
    {
        DataTable DtData = new DataTable();
        string q = "";
        param = new List<SqlParameter>();//Emp_Id
        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));

        
        if (sPartyName.SelectedItem.Text.Trim() == "--Select One--")
        {
            param.Add(new SqlParameter("@DataDate1", Convert.ToDateTime(fdate.Value.Trim()).ToString("dd-MMM-yyyy")));
            param.Add(new SqlParameter("@DataDate2", Convert.ToDateTime(tdate.Value.Trim()).ToString("dd-MMM-yyyy")));

            q = "select ID,[No],MPurNo,DataDate,PartyName,BrokerName,SaudaNo,SaudaDate,TruckNo,KantaNo,Advance from prabha.Purchase_Master_Data where CompanyID=@CompanyID and DataDate>=@DataDate1 and DataDate<=@DataDate2 order by [No],DataDate";
        }
        else
        {
            param.Add(new SqlParameter("@DataDate1", Convert.ToDateTime(fdate.Value.Trim()).ToString("dd-MMM-yyyy")));
            param.Add(new SqlParameter("@DataDate2", Convert.ToDateTime(tdate.Value.Trim()).ToString("dd-MMM-yyyy")));
            param.Add(new SqlParameter("@PartyName", sPartyName.SelectedItem.Text.Trim()));

            q = "select ID,[No],MPurNo,DataDate,PartyName,BrokerName,SaudaNo,SaudaDate,TruckNo,KantaNo,Advance from prabha.Purchase_Master_Data where CompanyID=@CompanyID and DataDate>=@DataDate1 and DataDate<=@DataDate2 and PartyName=@PartyName order by [No],DataDate";
        }
        dac = new DataAccessLayer();
        DtData = dac.GetDataTable(q, param);
        DtData.Columns.Add("Amount",typeof(string));
        DtData.Columns.Add("CD", typeof(string));
        DtData.Columns.Add("GK", typeof(string));
        StringBuilder htmlTable = new StringBuilder();
        string INVNo = "";
        //TruckNo,KantaNo,Advance
        htmlTable.Append("<table class='table table-bordered' id='dataTable' cellspacing='0'>");
        htmlTable.Append("<tr><td colspan='12' align='center'><span style='display:table-cell; vertical-align:top;'><img src='http://prabhasoftware.com/Rashmi Rice Logo (1).png' height='100px'/></span><span style='display:table-cell; vertical-align:top;'><span style='font-size:16pt; font-weight:bold;'> Rashmi Rice Mills Pvt. Ltd. </span></br><span style='font-size:8pt;'>Daniyawan Chandi Road, Hasanpur, Patna- 801304 </br>Mob.: 9304052349, 9334280057</br>Email: srirajbhog@gmail.com</br>CIN: U15312BR2014PTC022237</br>PAN No.: AAGCR9497P</br>GSTIN: 10AAGCR9497P1ZK</span></span></td></tr>");
        htmlTable.Append("<tr><td colspan='12' align='center'><span style='font-size:10pt; font-weight:bold;'> PURCHASE & UNLOADING REPORT </span></td></tr>");
        htmlTable.Append("<tr><td>Sl. No.</td><td>Invoice No. & Date</td><td>Party Name</td><td>Broker Name</td><td>Sauda No. & Date</td><td>Truck No.</td><td>Kanta No.</td><td>Freight Adv.</td><td>CD</td><td>GK</td><td>Amount</td><td></td></tr><tbody>");
        for (int i = 0; i < DtData.Rows.Count; i++)
        {
            htmlTable.Append("<tr>");
            htmlTable.Append("<td>" + (i + 1) + "</td>");
            INVNo = GenInvoiceNo(DtData.Rows[i]["No"].ToString(), DtData.Rows[i]["DataDate"].ToString());
            htmlTable.Append("<td>" + INVNo + ", " + Convert.ToDateTime(DtData.Rows[i]["DataDate"].ToString()).ToString("dd/MM/yyyy") + "</td>");
            htmlTable.Append("<td>" + DtData.Rows[i]["PartyName"].ToString() + "</td>");
            htmlTable.Append("<td>" + DtData.Rows[i]["BrokerName"].ToString() + "</td>");
            htmlTable.Append("<td>" + DtData.Rows[i]["SaudaNo"].ToString() + ", " + Convert.ToDateTime(DtData.Rows[i]["SaudaDate"].ToString()).ToString("dd/MM/yyyy") + "</td>");
            htmlTable.Append("<td>" + DtData.Rows[i]["TruckNo"].ToString() + "</td>");
            htmlTable.Append("<td>" + DtData.Rows[i]["KantaNo"].ToString() + "</td>");
            htmlTable.Append("<td>" + DtData.Rows[i]["Advance"].ToString() + "</td>");
            string[] calc = dataDisplay(DtData.Rows[i]["ID"].ToString()).Split('-');
            if (calc[0].ToString().Trim() == "")
            {
                DtData.Rows[i]["Amount"] = "0";
            }
            else
            {
                DtData.Rows[i]["Amount"] = Math.Round(Convert.ToDouble(calc[0].ToString()), 0);
            }
            DtData.Rows[i]["CD"] = Math.Round(Convert.ToDouble(calc[1].ToString()), 0);
            DtData.Rows[i]["GK"] = Math.Round(Convert.ToDouble(calc[2].ToString()), 0);
            htmlTable.Append("<td>" + DtData.Rows[i]["CD"].ToString() + "</td>");
            htmlTable.Append("<td>" + DtData.Rows[i]["GK"].ToString() + "</td>");
            htmlTable.Append("<td>" + DtData.Rows[i]["Amount"].ToString() + "</td>");
            htmlTable.Append("<td><a href='PurchaseBill.aspx?ID=" + DtData.Rows[i]["ID"].ToString() + "' target='_blank'>Purchase Bill</a></td></tr>");
        }
        Session["Export"] = null;
        Session["Export"] = DtData;
        htmlTable.Append("</tbody></table>");
        DBDataPlaceHolder.Controls.Add(new Literal { Text = htmlTable.ToString() });
    }
    public string dataDisplay(string id)
    {
        string q = "";
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@ID", id));
        q = "select * from [prabha].[Purchase_Master_Data] where ID=@ID";
        dac = new DataAccessLayer();
        Session["DataMain"] = dac.GetDataTable(q, param);

        q = "";
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@ID", id));
        q = "select * from [prabha].[Purchase_Item_Info] where Master_ID=@ID";
        dac = new DataAccessLayer();
        Session["Data"] = dac.GetDataTable(q, param);
        double FAmount = 0;
        double LCD = 0;
        double LGK = 0;
        if (Session["Data"] == null)
        {
            FAmount = 0;
        }
        else
        {
            dtMain = (DataTable)Session["DataMain"];
            dt = (DataTable)Session["Data"];


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
            

            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                
                am = Math.Round(Convert.ToDouble(dt.Rows[i]["FreshQuantity"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * Convert.ToDouble(dt.Rows[i]["Rate"].ToString()), 2);

                if (Convert.ToDouble(dt.Rows[i]["KhakhriPer"].ToString()) <= 2)
                {
                    
                    KhAmount = Math.Round(Convert.ToDouble(dt.Rows[i]["KhakhriBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * Convert.ToDouble(dt.Rows[i]["Rate"].ToString()), 2);
                }
                else
                {
                    KhRate = Math.Round(Convert.ToDouble(dt.Rows[i]["Rate"].ToString()) - (Convert.ToDouble(dt.Rows[i]["Rate"].ToString()) * (Convert.ToDouble(dt.Rows[i]["KhakhriPer"].ToString()) - 2) / 100), 2);
                    
                    KhAmount = Math.Round(Convert.ToDouble(dt.Rows[i]["KhakhriBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * KhRate, 2);
                }

                if (Convert.ToDouble(dt.Rows[i]["MittiPer"].ToString()) <= 0)
                {
                    
                    MAmount = Math.Round(Convert.ToDouble(dt.Rows[i]["MittiBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * Convert.ToDouble(dt.Rows[i]["Rate"].ToString()), 2);
                }
                else
                {
                    MRate = Math.Round(Convert.ToDouble(dt.Rows[i]["Rate"].ToString()) - (Convert.ToDouble(dt.Rows[i]["Rate"].ToString()) * (Convert.ToDouble(dt.Rows[i]["MittiPer"].ToString()) - 0) / 100), 2);
                    
                    MAmount = Math.Round(Convert.ToDouble(dt.Rows[i]["MittiBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * MRate, 2);
                }

                if (Convert.ToDouble(dt.Rows[i]["DaagiPer"].ToString()) <= 0)
                {
                    
                    DAmount = Math.Round(Convert.ToDouble(dt.Rows[i]["DaagiBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * Convert.ToDouble(dt.Rows[i]["Rate"].ToString()), 2);
                }
                else
                {
                    DRate = Math.Round(Convert.ToDouble(dt.Rows[i]["Rate"].ToString()) - (Convert.ToDouble(dt.Rows[i]["Rate"].ToString()) * (Convert.ToDouble(dt.Rows[i]["DaagiPer"].ToString()) - 0) / 100), 2);
                    
                    DAmount = Math.Round(Convert.ToDouble(dt.Rows[i]["DaagiBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * DRate, 2);
                }


                if (Convert.ToDouble(dt.Rows[i]["MixPer"].ToString()) <= 0)
                {
                    
                    DMixAmount = Math.Round(Convert.ToDouble(dt.Rows[i]["MixBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * Convert.ToDouble(dt.Rows[i]["Rate"].ToString()), 2);
                }
                else
                {
                    DMixRate = Math.Round(Convert.ToDouble(dt.Rows[i]["Rate"].ToString()) - (Convert.ToDouble(dt.Rows[i]["Rate"].ToString()) * (Convert.ToDouble(dt.Rows[i]["MixPer"].ToString()) - 0) / 100), 2);
                    
                    DMixAmount = Math.Round(Convert.ToDouble(dt.Rows[i]["MixBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * DMixRate, 2);
                }

                if (Convert.ToDouble(dt.Rows[i]["OtherPer"].ToString()) <= 0)
                {
                    
                    OAmount = Math.Round(Convert.ToDouble(dt.Rows[i]["OtherBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * Convert.ToDouble(dt.Rows[i]["Rate"].ToString()), 2);
                }
                else
                {
                    ORate = Math.Round(Convert.ToDouble(dt.Rows[i]["Rate"].ToString()) - (Convert.ToDouble(dt.Rows[i]["Rate"].ToString()) * (Convert.ToDouble(dt.Rows[i]["OtherPer"].ToString()) - 0) / 100), 2);
                    
                    OAmount = Math.Round(Convert.ToDouble(dt.Rows[i]["OtherBags"].ToString()) * Convert.ToDouble(dt.Rows[i]["AvgWt"].ToString()) * ORate, 2);
                }

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
                
                if (i == (dt.Rows.Count - 1))
                {
                    LCD = Math.Round(tAmount * Convert.ToDouble(dtMain.Rows[0]["CD"].ToString()) / 100);

                    LGK = Math.Round(tQuantity / 1000 * 25, 2);
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

                    

                }
                

            }
            
        }
        return FAmount + "-" + LCD + "-" + LGK;
    }
    public string GenInvoiceNo(string a,string b)
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
    public void Export_ServerClick(object sender, EventArgs e)
    {

        if (Session["Export"] == null)
        {

        }
        else
        {
            DataTable EData = (DataTable) Session["Export"];

            DataTable DtDataF = new DataTable();
            string q = "";
            param = new List<SqlParameter>();//Emp_Id
        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));


            if (sPartyName.SelectedItem.Text.Trim() == "--Select One--")
            {
                param.Add(new SqlParameter("@DataDate1", Convert.ToDateTime(fdate.Value.Trim()).ToString("dd-MMM-yyyy")));
                param.Add(new SqlParameter("@DataDate2", Convert.ToDateTime(tdate.Value.Trim()).ToString("dd-MMM-yyyy")));
                //ID,[No],MPurNo,DataDate,PartyName,BrokerName,SaudaNo,SaudaDate,TruckNo,KantaNo,Advance,Amount,CD,GK
                q = "select ID,[No],MPVNo as MPurNo,DataDate,PName as PartyName,PaymentMode as BrokerName,'' as SaudaNo,convert(smalldatetime,'01/01/1990') as SaudaDate,Bank as TruckNo,[Transaction] as KantaNo,CAST('0' AS DECIMAL(10, 2)) AS Advance,convert(varchar,AmountPaid) as Amount,'' as CD,'' as GK from prabha.[Purchase_Payment_Info] where CompanyID=@CompanyID and DataDate>=@DataDate1 and DataDate<=@DataDate2 order by DataDate";
            }
            else
            {
                param.Add(new SqlParameter("@DataDate1", Convert.ToDateTime(fdate.Value.Trim()).ToString("dd-MMM-yyyy")));
                param.Add(new SqlParameter("@DataDate2", Convert.ToDateTime(tdate.Value.Trim()).ToString("dd-MMM-yyyy")));
                param.Add(new SqlParameter("@PartyName", sPartyName.SelectedItem.Text.Trim()));

                q = "select ID,[No],MPVNo as MPurNo,DataDate,PName as PartyName,PaymentMode as BrokerName,'' as SaudaNo,convert(smalldatetime,'01/01/1990') as SaudaDate,Bank as TruckNo,[Transaction] as KantaNo,CAST('0' AS DECIMAL(10, 2)) AS Advance,convert(varchar,AmountPaid) as Amount,'' as CD,'' as GK from prabha.[Purchase_Payment_Info] where CompanyID=@CompanyID and DataDate>=@DataDate1 and DataDate<=@DataDate2 and PName=@PartyName order by DataDate";
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
        //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
          "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
          "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR>");
        //am getting my grid's column headers
         //write in new column
            HttpContext.Current.Response.Write("<Td>");
            //Get column headers  and make it as bold in excel columns
            HttpContext.Current.Response.Write("<B>");
            HttpContext.Current.Response.Write("Sl. No.");
            HttpContext.Current.Response.Write("</B>");
            HttpContext.Current.Response.Write("</Td>");

            HttpContext.Current.Response.Write("<Td><B>Purchase/Payment No.</B></Td>");
            HttpContext.Current.Response.Write("<Td><B>Manual No.</B></Td>");
            HttpContext.Current.Response.Write("<Td><B>Invoice/Payment Date</B></Td>");
            HttpContext.Current.Response.Write("<Td><B>Party Name</B></Td>");
            HttpContext.Current.Response.Write("<Td><B>Sauda No. & Date</B></Td>");
            HttpContext.Current.Response.Write("<Td><B>Vehicle No.</B></Td>");
            HttpContext.Current.Response.Write("<Td><B>Kanta No.</B></Td>");
            HttpContext.Current.Response.Write("<Td><B>Freight Adv.</B></Td>");
            HttpContext.Current.Response.Write("<Td><B>CD</B></Td>");
            HttpContext.Current.Response.Write("<Td><B>GK</B></Td>");
            HttpContext.Current.Response.Write("<Td><B>Bill Amount</B></Td>");
            HttpContext.Current.Response.Write("<Td><B>Paid Amount</B></Td>");

        HttpContext.Current.Response.Write("</TR>");
        int i = 0;
        string InvoiceNo = "";
        foreach (DataRow row in table.Rows)
        {//write in new row
            i = i + 1;
            if (row["SaudaNo"].ToString() == "")
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
            if (row["SaudaNo"].ToString() == "")
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
            HttpContext.Current.Response.Write(row["MPurNo"].ToString());
            HttpContext.Current.Response.Write("</Td>");
            HttpContext.Current.Response.Write("<Td>");
            HttpContext.Current.Response.Write(Convert.ToDateTime(row["DataDate"].ToString()).ToString("dd/MM/yyyy"));
            HttpContext.Current.Response.Write("</Td>");
            HttpContext.Current.Response.Write("<Td>");
            HttpContext.Current.Response.Write(row["PartyName"].ToString());
            HttpContext.Current.Response.Write("</Td>");
            
            if (row["SaudaNo"].ToString() == "")
            {
                HttpContext.Current.Response.Write("<Td colspan='3'>");
                HttpContext.Current.Response.Write(row["BrokerName"].ToString() + " (" + row["KantaNo"].ToString() + ")");
                HttpContext.Current.Response.Write("</Td>");
            }
            else
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row["SaudaNo"].ToString() + ", " + Convert.ToDateTime(row["SaudaDate"].ToString()).ToString("dd/MM/yyyy"));
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row["TruckNo"].ToString());
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(row["KantaNo"].ToString());
                HttpContext.Current.Response.Write("</Td>");
            }

            if (row["SaudaNo"].ToString() == "")
            {
                HttpContext.Current.Response.Write("<Td colspan='3'>");
                HttpContext.Current.Response.Write(row["TruckNo"].ToString());
                HttpContext.Current.Response.Write("</Td>");
            }
            else
            {
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(Math.Round(Convert.ToDouble(row["Advance"].ToString()), 0));
                
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(Math.Round(Convert.ToDouble(row["CD"].ToString()), 2).ToString());
                
                HttpContext.Current.Response.Write("</Td>");
                HttpContext.Current.Response.Write("<Td>");
                HttpContext.Current.Response.Write(Math.Round(Convert.ToDouble(row["GK"].ToString()), 2).ToString());
                
                HttpContext.Current.Response.Write("</Td>");
            }
            if (row["SaudaNo"].ToString() == "")
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
