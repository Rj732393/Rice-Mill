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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null || Session["User"].ToString() != "admin")
        {
            Response.Redirect("../Login.aspx");
            return;
        }
    }

    // ---------------------------------------------------------------
    // SEARCH by Sauda No.
    // ---------------------------------------------------------------
    protected void btnSearch_ServerClick(object sender, EventArgs e)
    {
        string saudaNo = txtSaudaNo.Text.Trim();
        if (string.IsNullOrEmpty(saudaNo))
        {
            ShowMessage("Sauda Number daalo.", false);
            return;
        }

        dac = new DataAccessLayer();
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@saudaNo", saudaNo));

        // Sauda fetch karo
        DataTable dtSauda = dac.GetDataTable(
            "SELECT * FROM prabha.Purchase_Sauda_Info WHERE MNo=@saudaNo OR " +
            "CAST(No AS NVARCHAR)=@saudaNo", param);

        // MNo format se bhi try karo
        if (dtSauda.Rows.Count == 0)
        {
            param = new List<SqlParameter>();
            param.Add(new SqlParameter("@saudaNo", "%" + saudaNo + "%"));
            dtSauda = dac.GetDataTable(
                "SELECT * FROM prabha.Purchase_Sauda_Info WHERE MNo LIKE @saudaNo", param);
        }

        if (dtSauda.Rows.Count == 0)
        {
            ShowMessage("Koi Sauda nahi mila: " + saudaNo, false);
            return;
        }

        DataRow s = dtSauda.Rows[0];
        Session["SaudaID"] = s["ID"].ToString();
        Session["SaudaNo"] = saudaNo;

        // Sauda fields fill karo
        hdnSaudaID.Value = s["ID"].ToString();
        lblSaudaNo.Text = s["MNo"].ToString();
        saudaDate.Value = s["DataDate"] != DBNull.Value
            ? Convert.ToDateTime(s["DataDate"]).ToString("yyyy-MM-dd") : "";
        saudaParty.Value = s["PartyName"].ToString();
        saudaBroker.Value = s["BrokerName"].ToString();
        saudaMNo.Value = s["MNo"].ToString();

        // Paddy rates fill karo
        sRupaliWt.Value = s["RupaliWt"].ToString();
        sRupaliRate.Value = s["RupaliRate"].ToString();
        sMansuriWt.Value = s["MansuriWt"].ToString();
        sMansuriRate.Value = s["MansuriRate"].ToString();
        sSonamWt.Value = s["SonamWt"].ToString();
        sSonamRate.Value = s["SonamRate"].ToString();
        sHybridWt.Value = s["HybridWt"].ToString();
        sHybridRate.Value = s["HybridRate"].ToString();

        // Linked Purchase Entries load karo
        LoadPurchaseEntries(s["MNo"].ToString());

        editPanel.Visible = true;
        ShowMessage("Sauda mila: " + s["MNo"].ToString() + " | Party: " + s["PartyName"].ToString(), true);
    }

    // ---------------------------------------------------------------
    // Load linked Purchase Master + Items
    // ---------------------------------------------------------------
    private void LoadPurchaseEntries(string saudaNo)
    {
        dac = new DataAccessLayer();
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@saudaNo", saudaNo));

        DataTable dtMaster = dac.GetDataTable(
            "SELECT * FROM prabha.Purchase_Master_Data WHERE SaudaNo=@saudaNo ORDER BY ID", param);

        StringBuilder html = new StringBuilder();

        if (dtMaster == null || dtMaster.Rows.Count == 0)
        {
            html.Append("<div style='color:#888;padding:10px;'>Is Sauda ke liye koi Purchase Entry nahi mili abhi tak.</div>");
            phPurchaseEntries.Controls.Add(new LiteralControl(html.ToString()));
            return;
        }

        foreach (DataRow master in dtMaster.Rows)
        {
            string masterID = master["ID"].ToString();
            string purNo = master["MPurNo"].ToString();
            string purDate = master["DataDate"] != DBNull.Value
                ? Convert.ToDateTime(master["DataDate"]).ToString("dd-MMM-yyyy") : "";

            html.Append("<div class='purchase-entry-block'>");
            html.Append("<div class='purchase-entry-title'>🚛 Purchase No: " + purNo
                + " | Date: " + purDate
                + " | Truck: " + master["TruckNo"].ToString() + "</div>");

            param = new List<SqlParameter>();
            param.Add(new SqlParameter("@masterID", Convert.ToDecimal(masterID)));
            DataTable dtItems = dac.GetDataTable(
                "SELECT * FROM prabha.Purchase_Item_Info WHERE Master_ID=@masterID ORDER BY ID", param);

            if (dtItems != null && dtItems.Rows.Count > 0)
            {
                html.Append("<table class='paddy-table'>");
                html.Append("<thead><tr><th>Item ID</th><th>Paddy Type</th><th>Avg Wt (KG)</th>"
                    + "<th>Rate (₹)</th><th>Fresh Qty</th><th>Moisture%</th></tr></thead><tbody>");

                foreach (DataRow item in dtItems.Rows)
                {
                    string itemID = item["ID"].ToString();
                    html.Append("<tr>");
                    html.Append("<td>" + itemID
                        + "<input type='hidden' name='itemID_" + itemID + "' value='" + itemID + "' /></td>");
                    html.Append("<td>" + item["PaddyType"].ToString() + "</td>");
                    html.Append("<td><input type='text' name='itemAvgWt_" + itemID + "' value='"
                        + item["AvgWt"].ToString() + "' style='width:100px;' /></td>");
                    html.Append("<td><input type='text' name='itemRate_" + itemID + "' value='"
                        + item["Rate"].ToString() + "' style='width:100px;' /></td>");
                    html.Append("<td><input type='text' name='itemFreshQty_" + itemID + "' value='"
                        + item["FreshQuantity"].ToString() + "' style='width:80px;' /></td>");
                    html.Append("<td><input type='text' name='itemMoisture_" + itemID + "' value='"
                        + item["Moisture"].ToString() + "' style='width:80px;' /></td>");
                    html.Append("</tr>");
                }
                html.Append("</tbody></table>");
            }
            else
            {
                html.Append("<div style='color:#888;font-size:12px;padding:6px;'>Koi item nahi mila is entry mein.</div>");
            }
            html.Append("</div>");
        }

        phPurchaseEntries.Controls.Add(new LiteralControl(html.ToString()));
    }

    // ---------------------------------------------------------------
    // SAVE ALL CHANGES
    // ---------------------------------------------------------------
    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        string saudaID = Session["SaudaID"] != null ? Session["SaudaID"].ToString() : "";
        if (string.IsNullOrEmpty(saudaID))
        {
            ShowMessage("Session expire ho gaya. Dobara search karo.", false);
            return;
        }

        try
        {
            dac = new DataAccessLayer();

            // -------------------------------------------------------
            // 1. Purchase_Sauda_Info update karo
            // -------------------------------------------------------
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
                DataDate=@DataDate, PartyName=@PartyName, BrokerName=@BrokerName,
                MNo=@MNo, RupaliWt=@RupaliWt, RupaliRate=@RupaliRate,
                MansuriWt=@MansuriWt, MansuriRate=@MansuriRate,
                SonamWt=@SonamWt, SonamRate=@SonamRate,
                HybridWt=@HybridWt, HybridRate=@HybridRate
                WHERE ID=@ID", param);

            // -------------------------------------------------------
            // 2. Purchase_Master_Data mein PartyName/BrokerName update
            // -------------------------------------------------------
            string saudaNo = saudaMNo.Value.Trim();
            param = new List<SqlParameter>();
            param.Add(new SqlParameter("@PartyName", saudaParty.Value.Trim()));
            param.Add(new SqlParameter("@BrokerName", saudaBroker.Value.Trim()));
            param.Add(new SqlParameter("@SaudaNo", saudaNo));
            dac.update(@"UPDATE prabha.Purchase_Master_Data SET
                PartyName=@PartyName, BrokerName=@BrokerName
                WHERE SaudaNo=@SaudaNo", param);

            // -------------------------------------------------------
            // 3. Purchase_Item_Info — har item update karo
            // -------------------------------------------------------
            foreach (string key in Request.Form.AllKeys)
            {
                if (key != null && key.StartsWith("itemRate_"))
                {
                    string itemID = key.Substring("itemRate_".Length);
                    string newRate = Request.Form["itemRate_" + itemID];
                    string newAvgWt = Request.Form["itemAvgWt_" + itemID];
                    string newFreshQty = Request.Form["itemFreshQty_" + itemID];
                    string newMoisture = Request.Form["itemMoisture_" + itemID];

                    param = new List<SqlParameter>();
                    param.Add(new SqlParameter("@Rate", ToDecimal(newRate)));
                    param.Add(new SqlParameter("@AvgWt", ToDecimal(newAvgWt)));
                    param.Add(new SqlParameter("@FreshQuantity", ToDecimal(newFreshQty)));
                    param.Add(new SqlParameter("@Moisture", ToDecimal(newMoisture)));
                    param.Add(new SqlParameter("@ID", Convert.ToDecimal(itemID)));

                    dac.update(@"UPDATE prabha.Purchase_Item_Info SET
                        Rate=@Rate, AvgWt=@AvgWt,
                        FreshQuantity=@FreshQuantity, Moisture=@Moisture
                        WHERE ID=@ID", param);
                }
            }

            Session["SaudaID"] = null;
            editPanel.Visible = false;
            txtSaudaNo.Text = "";
            ShowMessage("✅ Sauda aur saari linked entries successfully update ho gayi!", true);
        }
        catch (Exception ex)
        {
            ShowMessage("❌ Error: " + ex.Message, false);
        }
    }

    protected void btnCancel_ServerClick(object sender, EventArgs e)
    {
        editPanel.Visible = false;
        Session["SaudaID"] = null;
        txtSaudaNo.Text = "";
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
}