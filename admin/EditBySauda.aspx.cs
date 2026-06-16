using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using substitute;

public partial class admin_EditBySauda : System.Web.UI.Page
{
    DataAccessLayer dac;
    List<SqlParameter> param;

    // ---------------------------------------------------------------
    // Page Load — only admin allowed
    // ---------------------------------------------------------------
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null || Session["User"].ToString() != "admin")
        {
            Response.Redirect("../Login.aspx");
            return;
        }

        // Current financial year auto-select karo (first load par)
        if (!IsPostBack)
        {
            int curMonth = DateTime.Now.Month;
            int curYear = DateTime.Now.Year;
            string currentFY = curMonth > 3
                ? curYear + "-" + (curYear + 1)
                : (curYear - 1) + "-" + curYear;

            ListItem liCurrent = ddlFinancialYear.Items.FindByValue(currentFY);
            if (liCurrent != null)
                liCurrent.Selected = true;
        }
    }

    // ---------------------------------------------------------------
    // SEARCH button click
    // ---------------------------------------------------------------
    protected void btnSearch_ServerClick(object sender, EventArgs e)
    {
        string saudaNo = txtSaudaNo.Text.Trim();
        string saudaType = hdnSaudaType.Value; // "Purchase" or "Sale"

        if (string.IsNullOrEmpty(saudaNo))
        {
            ShowMessage("Sauda Number daalo pehle.", false);
            return;
        }

        // -------------------------------------------------------
        // Full Sauda Number parse karo — e.g. RR/PS/2026-2027/0010
        // Format: PREFIX/TYPE/FY/NUMBER
        // -------------------------------------------------------
        string[] inputParts = saudaNo.Split('/');
        if (inputParts.Length == 4)
        {
            // TYPE detect karo: PS=Purchase, SS=Sale
            string typePart = inputParts[1].Trim().ToUpper();
            if (typePart == "PS")
            {
                saudaType = "Purchase";
                hdnSaudaType.Value = "Purchase";
            }
            else if (typePart == "SS")
            {
                saudaType = "Sale";
                hdnSaudaType.Value = "Sale";
            }

            // FY part set karo dropdown mein — 2026-2027
            string fyPart = inputParts[2].Trim();
            ListItem liMatch = ddlFinancialYear.Items.FindByValue(fyPart);
            if (liMatch != null)
            {
                ddlFinancialYear.ClearSelection();
                liMatch.Selected = true;
            }
            else
            {
                // Dropdown mein nahi tha — dynamically add karke select karo
                ddlFinancialYear.Items.Add(new ListItem(fyPart, fyPart));
                ddlFinancialYear.Items.FindByValue(fyPart).Selected = true;
            }

            // Numeric part — leading zeros strip karo (0010 → "10")
            saudaNo = inputParts[3].TrimStart('0');
            if (string.IsNullOrEmpty(saudaNo)) saudaNo = "0";
        }

        dac = new DataAccessLayer();
        DataTable dtSauda = null;
        string saudaID = "";

        // --- Financial Year filter calculate karo ---
        string fyStr = ddlFinancialYear.SelectedValue; // e.g. "2026-2027"
        int fyStart = 0, fyEnd = 0;
        bool hasFY = false;
        if (!string.IsNullOrEmpty(fyStr) && fyStr != "0" && fyStr.Contains("-"))
        {
            string[] parts = fyStr.Split('-');
            fyStart = int.Parse(parts[0]);
            fyEnd = int.Parse(parts[1]);
            hasFY = true;
        }

        if (saudaType == "Sale")
        {
            // --- Sale Sauda search ---
            // Try exact numeric No with optional FY filter
            param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
            param.Add(new SqlParameter("@no", saudaNo));
            string fyWhere = hasFY
                ? @" AND (
                        (MONTH(DataDate) > 3 AND YEAR(DataDate) = @fyStart)
                     OR (MONTH(DataDate) <= 3 AND YEAR(DataDate) = @fyEnd)
                   )"
                : "";
            if (hasFY)
            {
                param.Add(new SqlParameter("@fyStart", fyStart));
                param.Add(new SqlParameter("@fyEnd", fyEnd));
            }
            dtSauda = dac.GetDataTable(
                "SELECT * FROM prabha.Sale_Sauda_Master WHERE CompanyID=@CompanyID AND (CAST(No AS NVARCHAR)=@no OR MNo=@no)" + fyWhere, param);

            // Try LIKE on MNo
            if (dtSauda == null || dtSauda.Rows.Count == 0)
            {
                param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
                param.Add(new SqlParameter("@no", "%" + saudaNo + "%"));
                if (hasFY)
                {
                    param.Add(new SqlParameter("@fyStart", fyStart));
                    param.Add(new SqlParameter("@fyEnd", fyEnd));
                }
                dtSauda = dac.GetDataTable(
                    "SELECT * FROM prabha.Sale_Sauda_Master WHERE CompanyID=@CompanyID AND MNo LIKE @no" + fyWhere, param);
            }

            if (dtSauda == null || dtSauda.Rows.Count == 0)
            {
                ShowMessage("Sale Sauda nahi mila: " + saudaNo + (hasFY ? " (FY: " + fyStr + ")" : ""), false);
                return;
            }

            DataRow s = dtSauda.Rows[0];
            saudaID = s["ID"].ToString();
            Session["SaudaID"] = saudaID;
            Session["SaudaType"] = "Sale";

            // Fill common fields
            hdnSaudaID.Value = saudaID;
            saudaDate.Value = s["DataDate"] != DBNull.Value
                ? Convert.ToDateTime(s["DataDate"]).ToString("yyyy-MM-dd") : "";
            saudaParty.Value = s["PartyName"].ToString();
            saudaBroker.Value = s["BrokerName"].ToString();
            saudaMNo.Value = s["MNo"].ToString();

            string invNo = GenSaleInvoiceNo(s["No"].ToString(), s["DataDate"].ToString());
            lblSaudaHeading.Text = invNo + " | Party: " + s["PartyName"].ToString();

            // Load Sale items (editable)
            LoadSaleItems(saudaID);

            // Load linked Sale bills
            LoadLinkedSaleBills(saudaID);

            pnlPurchaseRates.Visible = false;
            pnlSaleItems.Visible = true;
        }
        else
        {
            // --- Purchase Sauda search ---
            param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
            param.Add(new SqlParameter("@no", saudaNo));
            string fyWhere = hasFY
                ? @" AND (
                        (MONTH(DataDate) > 3 AND YEAR(DataDate) = @fyStart)
                     OR (MONTH(DataDate) <= 3 AND YEAR(DataDate) = @fyEnd)
                   )"
                : "";
            if (hasFY)
            {
                param.Add(new SqlParameter("@fyStart", fyStart));
                param.Add(new SqlParameter("@fyEnd", fyEnd));
            }
            dtSauda = dac.GetDataTable(
                "SELECT * FROM prabha.Purchase_Sauda_Info WHERE CompanyID=@CompanyID AND (CAST(No AS NVARCHAR)=@no OR MNo=@no)" + fyWhere, param);

            if (dtSauda == null || dtSauda.Rows.Count == 0)
            {
                param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
                param.Add(new SqlParameter("@no", "%" + saudaNo + "%"));
                if (hasFY)
                {
                    param.Add(new SqlParameter("@fyStart", fyStart));
                    param.Add(new SqlParameter("@fyEnd", fyEnd));
                }
                dtSauda = dac.GetDataTable(
                    "SELECT * FROM prabha.Purchase_Sauda_Info WHERE CompanyID=@CompanyID AND MNo LIKE @no" + fyWhere, param);
            }

            if (dtSauda == null || dtSauda.Rows.Count == 0)
            {
                ShowMessage("Purchase Sauda nahi mila: " + saudaNo + (hasFY ? " (FY: " + fyStr + ")" : ""), false);
                return;
            }

            DataRow s = dtSauda.Rows[0];
            saudaID = s["ID"].ToString();
            Session["SaudaID"] = saudaID;
            Session["SaudaType"] = "Purchase";

            // Fill fields
            hdnSaudaID.Value = saudaID;
            saudaDate.Value = s["DataDate"] != DBNull.Value
                ? Convert.ToDateTime(s["DataDate"]).ToString("yyyy-MM-dd") : "";
            saudaParty.Value = s["PartyName"].ToString();
            saudaBroker.Value = s["BrokerName"].ToString();
            saudaMNo.Value = s["MNo"].ToString();

            // Paddy rates
            sRupaliWt.Value = s["RupaliWt"].ToString();
            sRupaliRate.Value = s["RupaliRate"].ToString();
            sMansuriWt.Value = s["MansuriWt"].ToString();
            sMansuriRate.Value = s["MansuriRate"].ToString();
            sSonamWt.Value = s["SonamWt"].ToString();
            sSonamRate.Value = s["SonamRate"].ToString();
            sHybridWt.Value = s["HybridWt"].ToString();
            sHybridRate.Value = s["HybridRate"].ToString();

            string invNo = GenPurchaseInvoiceNo(s["No"].ToString(), s["DataDate"].ToString());
            lblSaudaHeading.Text = invNo + " | Party: " + s["PartyName"].ToString();

            // Load linked purchase bills (unloading entries)
            // invNo = RR/PS/2026-2027/0010 format
            LoadLinkedPurchaseBills(s["MNo"].ToString(), saudaID, invNo);

            pnlPurchaseRates.Visible = true;
            pnlSaleItems.Visible = false;
        }

        editPanel.Visible = true;
        ShowMessage("✅ Sauda mila! Ab neeche edit karo aur Save karo.", true);
    }

    // ---------------------------------------------------------------
    // Load Sale Sauda Items (editable)
    // ---------------------------------------------------------------
    private void LoadSaleItems(string masterID)
    {
        dac = new DataAccessLayer();
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@MasterID", Convert.ToDecimal(masterID)));
        DataTable dt = dac.GetDataTable(
            "SELECT * FROM prabha.Sale_Sauda_Item_Info WHERE MasterID=@MasterID ORDER BY ID", param);

        StringBuilder html = new StringBuilder();
        if (dt == null || dt.Rows.Count == 0)
        {
            html.Append("<div style='color:#888;padding:8px;'>Koi Sale item nahi mila is Sauda mein.</div>");
        }
        else
        {
            html.Append("<table class='item-table'>");
            html.Append("<thead><tr><th>Item ID</th><th>Item Type</th><th>Qty (KG)</th><th>Rate (₹/KG)</th></tr></thead><tbody>");
            foreach (DataRow row in dt.Rows)
            {
                string itemID = row["ID"].ToString();
                html.Append("<tr>");
                html.Append("<td>" + itemID + "<input type='hidden' name='saleItemID_" + itemID + "' value='" + itemID + "' /></td>");
                html.Append("<td>" + row["ItemType"].ToString() + "</td>");
                html.Append("<td><input type='text' name='saleItemQty_" + itemID + "' value='" + row["QIKG"].ToString() + "' /></td>");
                html.Append("<td><input type='text' name='saleItemRate_" + itemID + "' value='" + row["AvgRate"].ToString() + "' /></td>");
                html.Append("</tr>");
            }
            html.Append("</tbody></table>");
        }
        phSaleItems.Controls.Add(new LiteralControl(html.ToString()));
    }

    // ---------------------------------------------------------------
    // Load Linked Purchase Bills (Purchase_Master_Data + Purchase_Item_Info)
    // ---------------------------------------------------------------
    private void LoadLinkedPurchaseBills(string mno, string saudaIDFallback, string saudaFormattedNo)
    {
        dac = new DataAccessLayer();
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
        DataTable dtMaster = null;

        // Try 1: SaudaNo = formatted number (RR/PS/2026-2027/0010)
        if (!string.IsNullOrEmpty(saudaFormattedNo))
        {
            param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
            param.Add(new SqlParameter("@sno", saudaFormattedNo));
            dtMaster = dac.GetDataTable(
                "SELECT * FROM prabha.Purchase_Master_Data WHERE CompanyID=@CompanyID AND SaudaNo=@sno ORDER BY ID", param);
        }

        // Try 2: SaudaNo = MNo (numeric/manual number like 7377)
        if ((dtMaster == null || dtMaster.Rows.Count == 0) && !string.IsNullOrEmpty(mno))
        {
            param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
            param.Add(new SqlParameter("@mno", mno));
            dtMaster = dac.GetDataTable(
                "SELECT * FROM prabha.Purchase_Master_Data WHERE CompanyID=@CompanyID AND SaudaNo=@mno ORDER BY ID", param);
        }

        // Try 3: SaudaNo LIKE %mno% (partial match)
        if ((dtMaster == null || dtMaster.Rows.Count == 0) && !string.IsNullOrEmpty(mno))
        {
            param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
            param.Add(new SqlParameter("@mno", "%" + mno + "%"));
            dtMaster = dac.GetDataTable(
                "SELECT * FROM prabha.Purchase_Master_Data WHERE CompanyID=@CompanyID AND SaudaNo LIKE @mno ORDER BY ID", param);
        }

        // Try 4: SaudaID column se match (agar koi aisa column ho)
        if ((dtMaster == null || dtMaster.Rows.Count == 0) && !string.IsNullOrEmpty(saudaIDFallback))
        {
            param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
            param.Add(new SqlParameter("@sid", saudaIDFallback));
            dtMaster = dac.GetDataTable(
                "SELECT * FROM prabha.Purchase_Master_Data WHERE CompanyID=@CompanyID AND SaudaID=@sid ORDER BY ID", param);
        }

        StringBuilder html = new StringBuilder();
        if (dtMaster == null || dtMaster.Rows.Count == 0)
        {
            html.Append("<div style='color:#888;padding:8px;font-size:12px;'>"
                + "Is Sauda ke liye koi Purchase Bill nahi mila."
                + "<br/><small style='color:#aaa;'>(Search: SaudaNo='" + saudaFormattedNo + "' | MNo='" + mno + "' | SaudaID='" + saudaIDFallback + "')</small>"
                + "</div>");
            phLinkedEntries.Controls.Add(new LiteralControl(html.ToString()));
            return;
        }

        foreach (DataRow master in dtMaster.Rows)
        {
            string masterID = master["ID"].ToString();
            string purNo = master["MPurNo"] != DBNull.Value ? master["MPurNo"].ToString() : "";
            string purDateV = master["DataDate"] != DBNull.Value ? Convert.ToDateTime(master["DataDate"]).ToString("yyyy-MM-dd") : "";
            string purDateD = master["DataDate"] != DBNull.Value ? Convert.ToDateTime(master["DataDate"]).ToString("dd-MMM-yyyy") : "";

            html.Append("<div class='linked-block'>");
            html.Append("<div class='linked-title'>🚛 Purchase Bill No: " + purNo + " &nbsp;|&nbsp; Date: " + purDateD + "</div>");

            // ── Master fields (collapsible) ──────────────────────────────
            html.Append("<div style='background:#fff;border:1px solid #c8dff5;border-radius:4px;padding:10px;margin-bottom:8px;'>");
            html.Append("<b style='font-size:12px;color:#1a6496;'>📋 Bill Master Details</b>");
            html.Append("<div style='display:flex;flex-wrap:wrap;gap:8px;margin-top:8px;'>");

            // Row 1 — Date, MPurNo, TruckNo, KantaNo
            html.Append(MasterField("Bill Date", "purDate_" + masterID, "date", purDateV));
            html.Append(MasterField("Bill No (MPurNo)", "purBillNo_" + masterID, "text", purNo));
            html.Append(MasterField("Truck No", "purTruck_" + masterID, "text", SafeCol(master, "TruckNo")));
            html.Append(MasterField("Kanta No", "purKanta_" + masterID, "text", SafeCol(master, "KantaNo")));

            // Row 2 — Unloaded At, Tare Wt, Avg Wt/Bag
            html.Append(MasterField("Unloaded At", "purUnload_" + masterID, "text", SafeCol(master, "UnloadedAt")));
            html.Append(MasterField("Tare Wt (KG)", "purTareWt_" + masterID, "text", SafeCol(master, "TareWt")));
            html.Append(MasterField("Avg Wt/Bag (KG)", "purAvgWtBag_" + masterID, "text", SafeCol(master, "AvgWtPerBag")));

            // Row 3 — Bags
            html.Append(MasterField("Plastic Bags", "purPBags_" + masterID, "text", SafeCol(master, "PlasticBags")));
            html.Append(MasterField("Plastic Torn Bags", "purPTBags_" + masterID, "text", SafeCol(master, "PlasticTornBags")));
            html.Append(MasterField("Jute Bags", "purJBags_" + masterID, "text", SafeCol(master, "JuteBags")));
            html.Append(MasterField("Jute Torn Bags", "purJTBags_" + masterID, "text", SafeCol(master, "JuteTornBags")));

            // Row 4 — Freight, CD, Advance, Brokerage
            html.Append(MasterField("CD (%)", "purCD_" + masterID, "text", SafeCol(master, "CD")));
            html.Append(MasterField("Total Freight", "purFreight_" + masterID, "text", SafeCol(master, "TotalFreight")));
            html.Append(MasterField("Freight (Own)", "purFrOwn_" + masterID, "text", SafeCol(master, "FreightOwn")));
            html.Append(MasterField("Freight (By Party)", "purFrParty_" + masterID, "text", SafeCol(master, "FreightByParty")));
            html.Append(MasterField("Advance", "purAdvance_" + masterID, "text", SafeCol(master, "Advance")));
            html.Append(MasterField("Brokerage (Party)", "purBrokerage_" + masterID, "text", SafeCol(master, "BrokerageParty")));

            html.Append("</div></div>"); // end master fields div

            // ── Item rows ──────────────────────────────────────────────
            param = new List<SqlParameter>();
            param.Add(new SqlParameter("@masterID", Convert.ToDecimal(masterID)));
            DataTable dtItems = dac.GetDataTable(
                "SELECT * FROM prabha.Purchase_Item_Info WHERE Master_ID=@masterID ORDER BY ID", param);

            if (dtItems != null && dtItems.Rows.Count > 0)
            {
                html.Append("<b style='font-size:12px;color:#1a6496;'>🌾 Paddy Items</b>");
                html.Append("<table class='item-table' style='margin-top:6px;'>");
                html.Append("<thead><tr>" +
                    "<th>ID</th><th>Paddy Type</th><th>Avg Wt</th>" +
                    "<th style='background:#b8860b;'>Rate (₹) [Auto]</th>" +
                    "<th>Fresh Qty</th><th>Moisture%</th>" +
                    "<th>KhakhriPer</th><th>KhakhriBags</th>" +
                    "<th>MittiPer</th><th>MittiBags</th>" +
                    "<th>DaagiPer</th><th>DaagiBags</th>" +
                    "<th>MixPer</th><th>MixBags</th>" +
                    "<th>OtherName</th><th>OtherPer</th><th>OtherBags</th>" +
                    "</tr></thead><tbody>");
                foreach (DataRow item in dtItems.Rows)
                {
                    string iid = item["ID"].ToString();
                    string pt = item["PaddyType"].ToString();
                    html.Append("<tr>");
                    // ID + hidden fields (PaddyType for server-side rate lookup)
                    html.Append("<td>" + iid
                        + "<input type='hidden' name='purItemID_" + iid + "' value='" + iid + "' />"
                        + "<input type='hidden' name='purItemPaddyType_" + iid + "' value='" + pt + "' /></td>");
                    html.Append("<td>" + pt + "</td>");
                    html.Append("<td><input type='text' style='width:70px' name='purItemAvgWt_" + iid + "' value='" + SafeColDef(item, "AvgWt") + "' /></td>");
                    // Rate — readonly, yellow — auto-fills from Sauda rate on Save
                    html.Append("<td><input type='text' style='width:70px;background:#fffde7;color:#b8860b;font-weight:bold;cursor:not-allowed;' " +
                        "name='purItemRate_" + iid + "' value='" + SafeColDef(item, "Rate") + "' readonly " +
                        "title='Sauda mein Rate badlo — Save karte hi yahan bhi update ho jaayega' /></td>");
                    html.Append("<td><input type='text' style='width:70px' name='purItemFreshQty_" + iid + "' value='" + SafeColDef(item, "FreshQuantity") + "' /></td>");
                    html.Append("<td><input type='text' style='width:60px' name='purItemMoisture_" + iid + "' value='" + SafeColDef(item, "Moisture") + "' /></td>");
                    html.Append("<td><input type='text' style='width:60px' name='purItemKhakhriPer_" + iid + "' value='" + SafeColDef(item, "KhakhriPer") + "' /></td>");
                    html.Append("<td><input type='text' style='width:60px' name='purItemKhakhriBags_" + iid + "' value='" + SafeColDef(item, "KhakhriBags") + "' /></td>");
                    html.Append("<td><input type='text' style='width:60px' name='purItemMittiPer_" + iid + "' value='" + SafeColDef(item, "MittiPer") + "' /></td>");
                    html.Append("<td><input type='text' style='width:60px' name='purItemMittiBags_" + iid + "' value='" + SafeColDef(item, "MittiBags") + "' /></td>");
                    html.Append("<td><input type='text' style='width:60px' name='purItemDaagiPer_" + iid + "' value='" + SafeColDef(item, "DaagiPer") + "' /></td>");
                    html.Append("<td><input type='text' style='width:60px' name='purItemDaagiBags_" + iid + "' value='" + SafeColDef(item, "DaagiBags") + "' /></td>");
                    html.Append("<td><input type='text' style='width:60px' name='purItemMixPer_" + iid + "' value='" + SafeColDef(item, "MixPer") + "' /></td>");
                    html.Append("<td><input type='text' style='width:60px' name='purItemMixBags_" + iid + "' value='" + SafeColDef(item, "MixBags") + "' /></td>");
                    html.Append("<td><input type='text' style='width:80px' name='purItemOtherName_" + iid + "' value='" + SafeColDef(item, "OtherName") + "' /></td>");
                    html.Append("<td><input type='text' style='width:60px' name='purItemOtherPer_" + iid + "' value='" + SafeColDef(item, "OtherPer") + "' /></td>");
                    html.Append("<td><input type='text' style='width:60px' name='purItemOtherBags_" + iid + "' value='" + SafeColDef(item, "OtherBags") + "' /></td>");
                    html.Append("</tr>");
                }
                html.Append("</tbody></table>");
            }
            else
            {
                html.Append("<div style='color:#888;font-size:12px;padding:5px;'>Is bill mein koi item nahi.</div>");
            }
            html.Append("</div>"); // linked-block
        }
        phLinkedEntries.Controls.Add(new LiteralControl(html.ToString()));
    }

    // Helper — ek labeled input box banata hai master fields ke liye
    private string MasterField(string label, string name, string type, string val)
    {
        return "<div style='flex:0 0 160px;min-width:140px;'>"
             + "<label style='font-size:11px;color:#555;font-weight:bold;display:block;margin-bottom:2px;'>" + label + "</label>"
             + "<input type='" + type + "' name='" + name + "' value='" + val + "' "
             + "style='width:100%;padding:4px 6px;border:1px solid #aaa;border-radius:3px;font-size:12px;box-sizing:border-box;' />"
             + "</div>";
    }

    // ---------------------------------------------------------------
    // Load Linked Sale Bills (Sale_Master_Data + Sale_Item_Info)
    // ---------------------------------------------------------------
    private void LoadLinkedSaleBills(string masterSaudaID)
    {
        // Sale_Master_Data mein SaudaID ya SaudaNo se link hota hai
        dac = new DataAccessLayer();
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CompanyID", Convert.ToInt32(Session["CompanyID"])));
        param.Add(new SqlParameter("@SaudaID", Convert.ToDecimal(masterSaudaID)));
        DataTable dtMaster = null;

        try
        {
            dtMaster = dac.GetDataTable(
                "SELECT * FROM prabha.Sale_Master_Data WHERE CompanyID=@CompanyID AND SaudaID=@SaudaID ORDER BY ID", param);
        }
        catch { dtMaster = null; }

        StringBuilder html = new StringBuilder();
        if (dtMaster == null || dtMaster.Rows.Count == 0)
        {
            html.Append("<div style='color:#888;padding:8px;font-size:12px;'>Is Sauda ke liye abhi tak koi Sale Bill nahi bana. Future mein jo bill banega wo updated rate se banega.</div>");
            phLinkedEntries.Controls.Add(new LiteralControl(html.ToString()));
            return;
        }

        foreach (DataRow master in dtMaster.Rows)
        {
            string masterID = master["ID"].ToString();
            string saleNo = master["MSaleNo"] != DBNull.Value ? master["MSaleNo"].ToString() : "";
            string saleDate = master["DataDate"] != DBNull.Value
                ? Convert.ToDateTime(master["DataDate"]).ToString("dd-MMM-yyyy") : "";

            html.Append("<div class='linked-block'>");
            html.Append("<div class='linked-title'>💰 Sale Bill No: " + saleNo + "  |  Date: " + saleDate + "</div>");

            param = new List<SqlParameter>();
            param.Add(new SqlParameter("@masterID", Convert.ToDecimal(masterID)));
            DataTable dtItems = dac.GetDataTable(
                "SELECT * FROM prabha.Sale_Item_Info WHERE Master_ID=@masterID ORDER BY ID", param);

            if (dtItems != null && dtItems.Rows.Count > 0)
            {
                html.Append("<table class='item-table'>");
                html.Append("<thead><tr><th>Item ID</th><th>Item Type</th><th>Qty (KG)</th><th>Rate (₹)</th></tr></thead><tbody>");
                foreach (DataRow item in dtItems.Rows)
                {
                    string itemID = item["ID"].ToString();
                    html.Append("<tr>");
                    html.Append("<td>" + itemID + "<input type='hidden' name='saleLinkedItemID_" + itemID + "' value='" + itemID + "' /></td>");
                    html.Append("<td>" + item["ItemType"].ToString() + "</td>");
                    html.Append("<td><input type='text' name='saleLinkedQty_" + itemID + "' value='" + item["QIKG"].ToString() + "' /></td>");
                    html.Append("<td><input type='text' name='saleLinkedRate_" + itemID + "' value='" + item["AvgRate"].ToString() + "' /></td>");
                    html.Append("</tr>");
                }
                html.Append("</tbody></table>");
            }
            html.Append("</div>");
        }
        phLinkedEntries.Controls.Add(new LiteralControl(html.ToString()));
    }

    // ---------------------------------------------------------------
    // SAVE ALL CHANGES
    // ---------------------------------------------------------------
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        string saudaID = Session["SaudaID"] != null ? Session["SaudaID"].ToString() : "";
        string saudaType = Session["SaudaType"] != null ? Session["SaudaType"].ToString() : "Purchase";

        if (string.IsNullOrEmpty(saudaID))
        {
            ShowMessage("Session expire ho gaya. Dobara search karo.", false);
            return;
        }

        try
        {
            dac = new DataAccessLayer();

            if (saudaType == "Sale")
            {
                SaveSaleSauda(saudaID);
            }
            else
            {
                SavePurchaseSauda(saudaID);
            }

            Session["SaudaID"] = null;
            Session["SaudaType"] = null;
            editPanel.Visible = false;
            txtSaudaNo.Text = "";
            ShowMessage("✅ Sauda aur saari linked bills successfully update ho gayi! Future ke naye bills mein bhi updated rate aayega.", true);
        }
        catch (Exception ex)
        {
            ShowMessage("❌ Error: " + ex.Message, false);
        }
    }

    // ---------------------------------------------------------------
    // Save Purchase Sauda + linked Purchase_Item_Info
    // ---------------------------------------------------------------
    private void SavePurchaseSauda(string saudaID)
    {
        // 1. Purchase_Sauda_Info update
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@DataDate", saudaDate.Value.Trim()));
        param.Add(new SqlParameter("@PartyName", saudaParty.Value.Trim()));
        param.Add(new SqlParameter("@BrokerName", saudaBroker.Value.Trim()));
        param.Add(new SqlParameter("@MNo", saudaMNo.Value.Trim()));
        param.Add(new SqlParameter("@RupaliWt", ToDecimal(sRupaliWt.Value)));
        param.Add(new SqlParameter("@RupaliRate", ToDecimal(sRupaliRate.Value)));
        param.Add(new SqlParameter("@MansuriWt", ToDecimal(sMansuriWt.Value)));
        param.Add(new SqlParameter("@MansuriRate", ToDecimal(sMansuriRate.Value)));
        param.Add(new SqlParameter("@SonamWt", ToDecimal(sSonamWt.Value)));
        param.Add(new SqlParameter("@SonamRate", ToDecimal(sSonamRate.Value)));
        param.Add(new SqlParameter("@HybridWt", ToDecimal(sHybridWt.Value)));
        param.Add(new SqlParameter("@HybridRate", ToDecimal(sHybridRate.Value)));
        param.Add(new SqlParameter("@ID", Convert.ToDecimal(saudaID)));

        dac.update(@"UPDATE prabha.Purchase_Sauda_Info SET
            DataDate=@DataDate, PartyName=@PartyName, BrokerName=@BrokerName, MNo=@MNo,
            RupaliWt=@RupaliWt, RupaliRate=@RupaliRate,
            MansuriWt=@MansuriWt, MansuriRate=@MansuriRate,
            SonamWt=@SonamWt, SonamRate=@SonamRate,
            HybridWt=@HybridWt, HybridRate=@HybridRate
            WHERE ID=@ID", param);

        // 2. Purchase_Master_Data — saari fields update (by masterID)
        foreach (string key in Request.Form.AllKeys)
        {
            if (key != null && key.StartsWith("purDate_"))
            {
                string mid = key.Substring("purDate_".Length);
                param = new List<SqlParameter>();
                param.Add(new SqlParameter("@DataDate", Request.Form["purDate_" + mid]));
                param.Add(new SqlParameter("@MPurNo", Request.Form["purBillNo_" + mid]));
                param.Add(new SqlParameter("@TruckNo", Request.Form["purTruck_" + mid]));
                param.Add(new SqlParameter("@KantaNo", Request.Form["purKanta_" + mid]));
                param.Add(new SqlParameter("@UnloadedAt", Request.Form["purUnload_" + mid]));
                param.Add(new SqlParameter("@TareWt", ToDecimal(Request.Form["purTareWt_" + mid])));
                param.Add(new SqlParameter("@AvgWtPerBag", ToDecimal(Request.Form["purAvgWtBag_" + mid])));
                param.Add(new SqlParameter("@PlasticBags", ToDecimal(Request.Form["purPBags_" + mid])));
                param.Add(new SqlParameter("@PlasticTornBags", ToDecimal(Request.Form["purPTBags_" + mid])));
                param.Add(new SqlParameter("@JuteBags", ToDecimal(Request.Form["purJBags_" + mid])));
                param.Add(new SqlParameter("@JuteTornBags", ToDecimal(Request.Form["purJTBags_" + mid])));
                param.Add(new SqlParameter("@CD", ToDecimal(Request.Form["purCD_" + mid])));
                param.Add(new SqlParameter("@TotalFreight", ToDecimal(Request.Form["purFreight_" + mid])));
                param.Add(new SqlParameter("@FreightOwn", ToDecimal(Request.Form["purFrOwn_" + mid])));
                param.Add(new SqlParameter("@FreightByParty", ToDecimal(Request.Form["purFrParty_" + mid])));
                param.Add(new SqlParameter("@Advance", ToDecimal(Request.Form["purAdvance_" + mid])));
                param.Add(new SqlParameter("@BrokerageParty", ToDecimal(Request.Form["purBrokerage_" + mid])));
                param.Add(new SqlParameter("@PartyName", saudaParty.Value.Trim()));
                param.Add(new SqlParameter("@BrokerName", saudaBroker.Value.Trim()));
                param.Add(new SqlParameter("@ID", Convert.ToDecimal(mid)));
                try
                {
                    dac.update(@"UPDATE prabha.Purchase_Master_Data SET
                        DataDate=@DataDate, MPurNo=@MPurNo, TruckNo=@TruckNo, KantaNo=@KantaNo,
                        UnloadedAt=@UnloadedAt, TareWt=@TareWt, AvgWtPerBag=@AvgWtPerBag,
                        PlasticBags=@PlasticBags, PlasticTornBags=@PlasticTornBags,
                        JuteBags=@JuteBags, JuteTornBags=@JuteTornBags,
                        CD=@CD, TotalFreight=@TotalFreight, FreightOwn=@FreightOwn,
                        FreightByParty=@FreightByParty, Advance=@Advance,
                        BrokerageParty=@BrokerageParty,
                        PartyName=@PartyName, BrokerName=@BrokerName
                        WHERE ID=@ID", param);
                }
                catch { }
            }
        }

        // 3. Purchase_Item_Info — update sab items
        //    Rate: Sauda level se automatically (PaddyType ke hisab se)
        decimal rRupali = ToDecimal(sRupaliRate.Value);
        decimal rMansuri = ToDecimal(sMansuriRate.Value);
        decimal rSonam = ToDecimal(sSonamRate.Value);
        decimal rHybrid = ToDecimal(sHybridRate.Value);

        foreach (string key in Request.Form.AllKeys)
        {
            if (!string.IsNullOrEmpty(key) && key.StartsWith("purItemID_"))
            {
                string itemID = key.Replace("purItemID_", "");
                string paddyType = (Request.Form["purItemPaddyType_" + itemID] ?? "").ToLower().Trim();

                // Auto-select rate from Sauda based on PaddyType
                decimal autoRate;
                if (paddyType.Contains("mansuri") || paddyType.Contains("mnsr")) autoRate = rMansuri;
                else if (paddyType.Contains("sonam") || paddyType.Contains("sonm")) autoRate = rSonam;
                else if (paddyType.Contains("hybrid") || paddyType.Contains("hybrd")) autoRate = rHybrid;
                else autoRate = rRupali; // Rupali or default

                param = new List<SqlParameter>();
                param.Add(new SqlParameter("@ID", Convert.ToDecimal(itemID)));
                param.Add(new SqlParameter("@Rate", autoRate));
                param.Add(new SqlParameter("@AvgWt", ToDecimal(Request.Form["purItemAvgWt_" + itemID])));
                param.Add(new SqlParameter("@FreshQuantity", ToDecimal(Request.Form["purItemFreshQty_" + itemID])));
                param.Add(new SqlParameter("@Moisture", ToDecimal(Request.Form["purItemMoisture_" + itemID])));
                param.Add(new SqlParameter("@KhakhriPer", ToDecimal(Request.Form["purItemKhakhriPer_" + itemID])));
                param.Add(new SqlParameter("@KhakhriBags", ToDecimal(Request.Form["purItemKhakhriBags_" + itemID])));
                param.Add(new SqlParameter("@MittiPer", ToDecimal(Request.Form["purItemMittiPer_" + itemID])));
                param.Add(new SqlParameter("@MittiBags", ToDecimal(Request.Form["purItemMittiBags_" + itemID])));
                param.Add(new SqlParameter("@DaagiPer", ToDecimal(Request.Form["purItemDaagiPer_" + itemID])));
                param.Add(new SqlParameter("@DaagiBags", ToDecimal(Request.Form["purItemDaagiBags_" + itemID])));
                param.Add(new SqlParameter("@MixPer", ToDecimal(Request.Form["purItemMixPer_" + itemID])));
                param.Add(new SqlParameter("@MixBags", ToDecimal(Request.Form["purItemMixBags_" + itemID])));
                param.Add(new SqlParameter("@OtherName", Request.Form["purItemOtherName_" + itemID] ?? ""));
                param.Add(new SqlParameter("@OtherPer", ToDecimal(Request.Form["purItemOtherPer_" + itemID])));
                param.Add(new SqlParameter("@OtherBags", ToDecimal(Request.Form["purItemOtherBags_" + itemID])));

                try
                {
                    dac.update(@"UPDATE prabha.Purchase_Item_Info SET
                        Rate=@Rate, AvgWt=@AvgWt, FreshQuantity=@FreshQuantity, Moisture=@Moisture,
                        KhakhriPer=@KhakhriPer, KhakhriBags=@KhakhriBags,
                        MittiPer=@MittiPer, MittiBags=@MittiBags,
                        DaagiPer=@DaagiPer, DaagiBags=@DaagiBags,
                        MixPer=@MixPer, MixBags=@MixBags,
                        OtherName=@OtherName, OtherPer=@OtherPer, OtherBags=@OtherBags
                        WHERE ID=@ID", param);
                }
                catch (Exception ex)
                {
                    ShowMessage("Purchase Item Update Error: " + ex.Message, false);
                }
            }
        }
    }

    // ---------------------------------------------------------------
    // Save Sale Sauda + linked Sale_Sauda_Item_Info + Sale_Item_Info
    // ---------------------------------------------------------------
    private void SaveSaleSauda(string saudaID)
    {
        // 1. Sale_Sauda_Master update
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@DataDate", saudaDate.Value.Trim()));
        param.Add(new SqlParameter("@PartyName", saudaParty.Value.Trim()));
        param.Add(new SqlParameter("@BrokerName", saudaBroker.Value.Trim()));
        param.Add(new SqlParameter("@MNo", saudaMNo.Value.Trim()));
        param.Add(new SqlParameter("@ID", Convert.ToDecimal(saudaID)));
        dac.update(@"UPDATE prabha.Sale_Sauda_Master SET
            DataDate=@DataDate, PartyName=@PartyName,
            BrokerName=@BrokerName, MNo=@MNo
            WHERE ID=@ID", param);

        // 2. Sale_Sauda_Item_Info (Sauda items — future bills ka base)
        foreach (string key in Request.Form.AllKeys)
        {
            if (key != null && key.StartsWith("saleItemRate_"))
            {
                string itemID = key.Substring("saleItemRate_".Length);
                param = new List<SqlParameter>();
                param.Add(new SqlParameter("@QIKG", ToDecimal(Request.Form["saleItemQty_" + itemID])));
                param.Add(new SqlParameter("@AvgRate", ToDecimal(Request.Form["saleItemRate_" + itemID])));
                param.Add(new SqlParameter("@ID", Convert.ToDecimal(itemID)));
                try
                {
                    dac.update("UPDATE prabha.Sale_Sauda_Item_Info SET QIKG=@QIKG, AvgRate=@AvgRate WHERE ID=@ID", param);
                }
                catch { }
            }
        }

        // 3. Linked Sale_Item_Info (already bane bills)
        foreach (string key in Request.Form.AllKeys)
        {
            if (key != null && key.StartsWith("saleLinkedRate_"))
            {
                string itemID = key.Substring("saleLinkedRate_".Length);
                param = new List<SqlParameter>();
                param.Add(new SqlParameter("@QIKG", ToDecimal(Request.Form["saleLinkedQty_" + itemID])));
                param.Add(new SqlParameter("@AvgRate", ToDecimal(Request.Form["saleLinkedRate_" + itemID])));
                param.Add(new SqlParameter("@ID", Convert.ToDecimal(itemID)));
                try
                {
                    dac.update("UPDATE prabha.Sale_Item_Info SET QIKG=@QIKG, AvgRate=@AvgRate WHERE ID=@ID", param);
                }
                catch { }
            }
        }
    }

    // ---------------------------------------------------------------
    // CANCEL
    // ---------------------------------------------------------------
    protected void btnCancel_ServerClick(object sender, EventArgs e)
    {
        editPanel.Visible = false;
        Session["SaudaID"] = null;
        Session["SaudaType"] = null;
        txtSaudaNo.Text = "";
    }

    // ---------------------------------------------------------------
    // Helper: Generate Invoice Numbers
    // ---------------------------------------------------------------
    private string GenPurchaseInvoiceNo(string no, string dateStr)
    {
        int mon = Convert.ToDateTime(dateStr).Month;
        int yr = Convert.ToDateTime(dateStr).Year;
        int yr1 = mon <= 3 ? yr - 1 : yr;
        int yr2 = mon <= 3 ? yr : yr + 1;
        string pad = no.Length == 1 ? "000" : no.Length == 2 ? "00" : no.Length == 3 ? "0" : "";
        return "RR/PS/" + yr1 + "-" + yr2 + "/" + pad + no;
    }

    private string GenSaleInvoiceNo(string no, string dateStr)
    {
        int mon = Convert.ToDateTime(dateStr).Month;
        int yr = Convert.ToDateTime(dateStr).Year;
        int yr1 = mon <= 3 ? yr - 1 : yr;
        int yr2 = mon <= 3 ? yr : yr + 1;
        string pad = no.Length == 1 ? "000" : no.Length == 2 ? "00" : no.Length == 3 ? "0" : "";
        return "RR/SS/" + yr1 + "-" + yr2 + "/" + pad + no;
    }

    private decimal ToDecimal(string val)
    {
        if (string.IsNullOrWhiteSpace(val)) return 0;
        try { return Convert.ToDecimal(val); }
        catch { return 0; }
    }

    private void ShowMessage(string msg, bool success)
    {
        string cls = success ? "alert-success" : "alert-error";
        string icon = success ? "&#10003;" : "&#9888;";
        phMessage.Controls.Add(new LiteralControl(
            "<div class='alert-msg " + cls + "'>" + icon + " " + msg + "</div>"
        ));
    }
    // ---------------------------------------------------------------
    // Helper: Safe column read — returns "" if column missing or null
    // ---------------------------------------------------------------
    private string SafeCol(DataRow row, string col)
    {
        if (row == null) return "";
        if (!row.Table.Columns.Contains(col)) return "";
        return row[col] == DBNull.Value ? "" : row[col].ToString();
    }

    // Helper: Safe column read with default "0"
    private string SafeColDef(DataRow row, string col)
    {
        if (row == null) return "0";
        if (!row.Table.Columns.Contains(col)) return "0";
        return row[col] == DBNull.Value ? "0" : row[col].ToString();
    }


}