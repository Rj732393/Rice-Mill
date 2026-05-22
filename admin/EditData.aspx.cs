using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using substitute;

public partial class admin_EditData : System.Web.UI.Page
{
    DataTable dt;
    List<SqlParameter> param;
    DataAccessLayer dac;

    private string GetPrimaryKeyColumn(string tableName)
    {
        if (tableName == "prabha.Purchase_Party_Info") return "Party_ID";
        return "ID";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null || Session["User"].ToString() != "admin")
        {
            Response.Redirect("../Login.aspx");
            return;
        }
        if (!Page.IsPostBack)
        {
            Session["EditTable"] = null;
            Session["EditID"] = null;
        }
    }

    protected void btnLoad_ServerClick(object sender, EventArgs e)
    {
        string tableName = ddlTable.SelectedValue.Trim();
        if (string.IsNullOrEmpty(tableName))
        {
            ShowMessage("Please select a table first.", false);
            return;
        }
        if (!IsAllowedTable(tableName))
        {
            ShowMessage("This table is not allowed.", false);
            return;
        }
        Session["EditTable"] = tableName;
        Session["EditID"] = null;
        editFormPanel.Visible = false;
        LoadTableData(tableName);
    }

    private void LoadTableData(string tableName)
    {
        try
        {
            dac = new DataAccessLayer();
            param = new List<SqlParameter>();
            string pkCol = GetPrimaryKeyColumn(tableName);
            dt = dac.GetDataTable("SELECT * FROM " + tableName + " ORDER BY " + pkCol + " DESC", param);

            lblTableTitle.Text = ddlTable.SelectedItem.Text + " (" + dt.Rows.Count + " records)";
            dataPanel.Visible = true;

            StringBuilder html = new StringBuilder();
            html.Append("<table class='table table-bordered table-hover tbl-edit' style='font-size:12px;'>");
            html.Append("<thead style='background:#8B0000;color:#fff;'><tr><th>Action</th>");
            foreach (DataColumn col in dt.Columns)
                html.Append("<th>" + col.ColumnName + "</th>");
            html.Append("</tr></thead><tbody>");

            if (dt.Rows.Count == 0)
            {
                html.Append("<tr><td colspan='" + (dt.Columns.Count + 1) + "' style='text-align:center;'>No records found.</td></tr>");
            }
            else
            {
                foreach (DataRow row in dt.Rows)
                {
                    string pkVal = row[pkCol].ToString();
                    html.Append("<tr><td style='white-space:nowrap;'>");
                    html.Append("<input type='submit' name='editBtn_" + pkVal + "' value='Edit' class='btn-edit-row' />");
                    html.Append("</td>");
                    foreach (DataColumn col in dt.Columns)
                    {
                        string val = row[col].ToString();
                        string display = val.Length > 40 ? val.Substring(0, 37) + "..." : val;
                        html.Append("<td class='view-cell' title='" + HttpUtility.HtmlEncode(val) + "'>"
                            + HttpUtility.HtmlEncode(display) + "</td>");
                    }
                    html.Append("</tr>");
                }
            }

            html.Append("</tbody></table>");
            phTable.Controls.Add(new LiteralControl(html.ToString()));

            foreach (string key in Request.Form.AllKeys)
            {
                if (key != null && key.StartsWith("editBtn_"))
                {
                    string pkVal = key.Substring("editBtn_".Length);
                    OpenEditForm(tableName, pkVal);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage("Error loading data: " + ex.Message, false);
        }
    }

    private void OpenEditForm(string tableName, string pkVal)
    {
        try
        {
            string pkCol = GetPrimaryKeyColumn(tableName);
            dac = new DataAccessLayer();
            param = new List<SqlParameter>();
            param.Add(new SqlParameter("@pkval", pkVal));
            dt = dac.GetDataTable("SELECT * FROM " + tableName + " WHERE " + pkCol + "=@pkval", param);

            if (dt.Rows.Count == 0) { ShowMessage("Record not found.", false); return; }

            DataRow row = dt.Rows[0];
            hdnEditID.Value = pkVal;
            hdnTableName.Value = tableName;

            // Cascade warning message
            string warningMsg = GetCascadeWarning(tableName);
            if (!string.IsNullOrEmpty(warningMsg))
            {
                ShowMessage("⚠️ Cascade Warning: " + warningMsg, false);
            }

            lblEditID.Text = "(" + pkCol + " = " + pkVal + ")";
            editFormPanel.Visible = true;
            Session["EditID"] = pkVal;
            Session["EditTable"] = tableName;

            StringBuilder html = new StringBuilder();
            html.Append("<div style='column-count:2; column-gap:30px;'>");
            foreach (DataColumn col in dt.Columns)
            {
                string fieldVal = row[col] == DBNull.Value ? "" : row[col].ToString();
                string inputType = (col.DataType == typeof(decimal) || col.DataType == typeof(int) ||
                                    col.DataType == typeof(long) || col.DataType == typeof(double) ||
                                    col.DataType == typeof(float)) ? "number" : "text";

                html.Append("<div style='break-inside:avoid; margin-bottom:12px;'>");
                html.Append("<label style='font-weight:bold;font-size:12px;color:#555;'>" + col.ColumnName + "</label>");

                if (col.ColumnName == pkCol)
                {
                    html.Append("<input type='text' name='ef_" + col.ColumnName + "' value='"
                        + HttpUtility.HtmlEncode(fieldVal) + "' readonly "
                        + "style='width:100%;padding:4px 8px;border:1px solid #ccc;border-radius:3px;background:#f5f5f5;color:#999;' />");
                }
                else
                {
                    html.Append("<input type='" + inputType + "' name='ef_" + col.ColumnName + "' value='"
                        + HttpUtility.HtmlEncode(fieldVal) + "' "
                        + "style='width:100%;padding:4px 8px;border:1px solid #aaa;border-radius:3px;' />");
                }
                html.Append("</div>");
            }
            html.Append("</div>");
            phEditForm.Controls.Add(new LiteralControl(html.ToString()));
        }
        catch (Exception ex)
        {
            ShowMessage("Error opening edit form: " + ex.Message, false);
        }
    }

    protected void btnSave_ServerClick(object sender, EventArgs e)
    {
        string tableName = Session["EditTable"] != null ? Session["EditTable"].ToString() : "";
        string pkVal = Session["EditID"] != null ? Session["EditID"].ToString() : "";

        if (string.IsNullOrEmpty(tableName) || string.IsNullOrEmpty(pkVal))
        {
            ShowMessage("Session expired. Please reload and try again.", false);
            return;
        }
        if (!IsAllowedTable(tableName)) { ShowMessage("Table not allowed.", false); return; }

        try
        {
            string pkCol = GetPrimaryKeyColumn(tableName);
            dac = new DataAccessLayer();
            param = new List<SqlParameter>();
            param.Add(new SqlParameter("@pkval", pkVal));

            // Purana data fetch karo (cascade ke liye old values chahiye)
            dt = dac.GetDataTable("SELECT * FROM " + tableName + " WHERE " + pkCol + "=@pkval", param);
            if (dt.Rows.Count == 0) { ShowMessage("Record not found.", false); return; }

            DataRow oldRow = dt.Rows[0];

            // Old values save karo cascade ke liye
            Dictionary<string, string> oldValues = new Dictionary<string, string>();
            Dictionary<string, string> newValues = new Dictionary<string, string>();

            foreach (DataColumn col in dt.Columns)
            {
                oldValues[col.ColumnName] = oldRow[col] == DBNull.Value ? "" : oldRow[col].ToString();
                string formKey = "ef_" + col.ColumnName;
                string newVal = Request.Form[formKey];
                newValues[col.ColumnName] = newVal ?? oldValues[col.ColumnName];
            }

            // Main table update
            StringBuilder setClauses = new StringBuilder();
            param = new List<SqlParameter>();

            foreach (DataColumn col in dt.Columns)
            {
                if (col.ColumnName == pkCol) continue;
                string newVal = newValues[col.ColumnName];

                if (setClauses.Length > 0) setClauses.Append(", ");
                setClauses.Append("[" + col.ColumnName + "]=@p_" + col.ColumnName);

                SqlParameter sp = new SqlParameter("@p_" + col.ColumnName, GetSqlDbType(col.DataType));
                if (string.IsNullOrWhiteSpace(newVal))
                    sp.Value = DBNull.Value;
                else
                {
                    try
                    {
                        if (col.DataType == typeof(DateTime)) sp.Value = Convert.ToDateTime(newVal);
                        else if (col.DataType == typeof(decimal)) sp.Value = Convert.ToDecimal(newVal);
                        else if (col.DataType == typeof(int)) sp.Value = Convert.ToInt32(newVal);
                        else if (col.DataType == typeof(long)) sp.Value = Convert.ToInt64(newVal);
                        else if (col.DataType == typeof(double) || col.DataType == typeof(float))
                            sp.Value = Convert.ToDouble(newVal);
                        else sp.Value = newVal;
                    }
                    catch { sp.Value = newVal; }
                }
                param.Add(sp);
            }

            if (setClauses.Length == 0) { ShowMessage("No fields to update.", false); return; }

            param.Add(new SqlParameter("@pkval", pkVal));
            string updateQ = "UPDATE " + tableName + " SET " + setClauses + " WHERE [" + pkCol + "]=@pkval";
            dac = new DataAccessLayer();
            dac.update(updateQ, param);

            // ✅ CASCADE UPDATES
            int cascadeCount = RunCascadeUpdates(tableName, oldValues, newValues);

            Session["EditID"] = null;
            editFormPanel.Visible = false;
            ddlTable.SelectedValue = tableName;
            Session["EditTable"] = tableName;
            LoadTableData(tableName);

            string cascadeMsg = cascadeCount > 0 ? " + " + cascadeCount + " linked table(s) bhi update hue." : "";
            ShowMessage("✅ Record successfully updated!" + cascadeMsg, true);
        }
        catch (Exception ex)
        {
            ShowMessage("Error saving: " + ex.Message, false);
        }
    }

    // ================================================================
    // CASCADE UPDATE LOGIC — Yahan sari linking hai
    // ================================================================
    private int RunCascadeUpdates(string tableName, Dictionary<string, string> oldVal, Dictionary<string, string> newVal)
    {
        int count = 0;
        dac = new DataAccessLayer();

        // -----------------------------------------------------------
        // 1. Purchase_Party_Info → Party_Name change
        //    Cascade: Purchase_Sauda_Info, Purchase_Master_Data, Purchase_Payment_Info
        // -----------------------------------------------------------
        if (tableName == "prabha.Purchase_Party_Info")
        {
            string oldName = oldVal.ContainsKey("Party_Name") ? oldVal["Party_Name"] : "";
            string newName = newVal.ContainsKey("Party_Name") ? newVal["Party_Name"] : "";

            if (oldName != newName && !string.IsNullOrEmpty(oldName))
            {
                // Purchase_Sauda_Info
                CascadeUpdate("prabha.Purchase_Sauda_Info", "PartyName", oldName, newName);
                // Purchase_Master_Data
                CascadeUpdate("prabha.Purchase_Master_Data", "PartyName", oldName, newName);
                // Purchase_Payment_Info (PName column)
                CascadeUpdate("prabha.Purchase_Payment_Info", "PName", oldName, newName);
                count += 3;
            }
        }

        // -----------------------------------------------------------
        // 2. PartyInfo → PartyName change (Sale side)
        //    Cascade: Sale_Sauda_Master, Sale_Master_Data, Sale_Payment_Info
        // -----------------------------------------------------------
        if (tableName == "prabha.PartyInfo")
        {
            string oldName = oldVal.ContainsKey("PartyName") ? oldVal["PartyName"] : "";
            string newName = newVal.ContainsKey("PartyName") ? newVal["PartyName"] : "";

            if (oldName != newName && !string.IsNullOrEmpty(oldName))
            {
                CascadeUpdate("prabha.Sale_Sauda_Master", "PartyName", oldName, newName);
                CascadeUpdate("prabha.Sale_Master_Data", "PartyName", oldName, newName);
                CascadeUpdate("prabha.Sale_Payment_Info", "PName", oldName, newName);
                count += 3;
            }
        }

        // -----------------------------------------------------------
        // 3. BrokerInfo → BrokerName change
        //    Cascade: Purchase_Sauda_Info, Purchase_Master_Data,
        //             Sale_Sauda_Master, Sale_Master_Data
        // -----------------------------------------------------------
        if (tableName == "prabha.BrokerInfo")
        {
            string oldName = oldVal.ContainsKey("BrokerName") ? oldVal["BrokerName"] : "";
            string newName = newVal.ContainsKey("BrokerName") ? newVal["BrokerName"] : "";

            if (oldName != newName && !string.IsNullOrEmpty(oldName))
            {
                CascadeUpdate("prabha.Purchase_Sauda_Info", "BrokerName", oldName, newName);
                CascadeUpdate("prabha.Purchase_Master_Data", "BrokerName", oldName, newName);
                CascadeUpdate("prabha.Sale_Sauda_Master", "BrokerName", oldName, newName);
                CascadeUpdate("prabha.Sale_Master_Data", "BrokerName", oldName, newName);
                count += 4;
            }
        }

        // -----------------------------------------------------------
        // 4. Purchase_Master_Data → ID change
        //    Cascade: Purchase_Item_Info (Master_ID)
        // -----------------------------------------------------------
        if (tableName == "prabha.Purchase_Master_Data")
        {
            string oldID = oldVal.ContainsKey("ID") ? oldVal["ID"] : "";
            string newID = newVal.ContainsKey("ID") ? newVal["ID"] : "";
            if (oldID != newID && !string.IsNullOrEmpty(oldID))
            {
                CascadeUpdateNumeric("prabha.Purchase_Item_Info", "Master_ID", oldID, newID);
                count += 1;
            }

            // Agar PartyName change hua
            string oldParty = oldVal.ContainsKey("PartyName") ? oldVal["PartyName"] : "";
            string newParty = newVal.ContainsKey("PartyName") ? newVal["PartyName"] : "";
            if (oldParty != newParty && !string.IsNullOrEmpty(oldParty))
            {
                CascadeUpdate("prabha.Purchase_Payment_Info", "PName", oldParty, newParty);
                count += 1;
            }
        }

        // -----------------------------------------------------------
        // 5. Sale_Master_Data → ID change
        //    Cascade: Sale_Item_Info (Master_ID)
        // -----------------------------------------------------------
        if (tableName == "prabha.Sale_Master_Data")
        {
            string oldID = oldVal.ContainsKey("ID") ? oldVal["ID"] : "";
            string newID = newVal.ContainsKey("ID") ? newVal["ID"] : "";
            if (oldID != newID && !string.IsNullOrEmpty(oldID))
            {
                CascadeUpdateNumeric("prabha.Sale_Item_Info", "Master_ID", oldID, newID);
                count += 1;
            }

            // Agar PartyName change hua
            string oldParty = oldVal.ContainsKey("PartyName") ? oldVal["PartyName"] : "";
            string newParty = newVal.ContainsKey("PartyName") ? newVal["PartyName"] : "";
            if (oldParty != newParty && !string.IsNullOrEmpty(oldParty))
            {
                CascadeUpdate("prabha.Sale_Payment_Info", "PName", oldParty, newParty);
                count += 1;
            }
        }

        // -----------------------------------------------------------
        // 6. Sale_Sauda_Master → ID change
        //    Cascade: Sale_Sauda_Item_Info (MasterID)
        // -----------------------------------------------------------
        if (tableName == "prabha.Sale_Sauda_Master")
        {
            string oldID = oldVal.ContainsKey("ID") ? oldVal["ID"] : "";
            string newID = newVal.ContainsKey("ID") ? newVal["ID"] : "";
            if (oldID != newID && !string.IsNullOrEmpty(oldID))
            {
                CascadeUpdateNumeric("prabha.Sale_Sauda_Item_Info", "MasterID", oldID, newID);
                count += 1;
            }
        }

        return count;
    }

    // Text field cascade (PartyName, BrokerName etc.)
    private void CascadeUpdate(string table, string column, string oldVal, string newVal)
    {
        try
        {
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@newVal", newVal));
            p.Add(new SqlParameter("@oldVal", oldVal));
            dac.update("UPDATE " + table + " SET [" + column + "]=@newVal WHERE [" + column + "]=@oldVal", p);
        }
        catch { }
    }

    // Numeric field cascade (Master_ID etc.)
    private void CascadeUpdateNumeric(string table, string column, string oldVal, string newVal)
    {
        try
        {
            var p = new List<SqlParameter>();
            p.Add(new SqlParameter("@newVal", Convert.ToDecimal(newVal)));
            p.Add(new SqlParameter("@oldVal", Convert.ToDecimal(oldVal)));
            dac.update("UPDATE " + table + " SET [" + column + "]=@newVal WHERE [" + column + "]=@oldVal", p);
        }
        catch { }
    }

    protected void btnCancelEdit_ServerClick(object sender, EventArgs e)
    {
        string tableName = Session["EditTable"] != null ? Session["EditTable"].ToString() : "";
        Session["EditID"] = null;
        editFormPanel.Visible = false;
        if (!string.IsNullOrEmpty(tableName) && IsAllowedTable(tableName))
        {
            ddlTable.SelectedValue = tableName;
            LoadTableData(tableName);
        }
    }

    private string GetCascadeWarning(string tableName)
    {
        switch (tableName)
        {
            case "prabha.Purchase_Party_Info":
                return "Party Name badalne se Purchase Sauda, Purchase Bill aur Purchase Payment bhi update honge.";
            case "prabha.PartyInfo":
                return "Party Name badalne se Sale Sauda, Sale Bill aur Sale Payment bhi update honge.";
            case "prabha.BrokerInfo":
                return "Broker Name badalne se Purchase Sauda, Purchase Bill, Sale Sauda aur Sale Bill bhi update honge.";
            case "prabha.Purchase_Master_Data":
                return "Is record se linked Purchase Items bhi affect honge.";
            case "prabha.Sale_Master_Data":
                return "Is record se linked Sale Items bhi affect honge.";
            case "prabha.Sale_Sauda_Master":
                return "Is record se linked Sale Sauda Items bhi affect honge.";
            default:
                return "";
        }
    }

    private bool IsAllowedTable(string t)
    {
        string[] allowed = {
            "prabha.Purchase_Party_Info", "prabha.Purchase_Sauda_Info",
            "prabha.Purchase_Master_Data", "prabha.Purchase_Item_Info",
            "prabha.Purchase_Payment_Info", "prabha.Sale_Sauda_Master",
            "prabha.Sale_Master_Data", "prabha.Sale_Item_Info",
            "prabha.Sale_Payment_Info", "prabha.Sale_Sauda_Item_Info",
            "prabha.PaddyProcessing", "prabha.PaddyStock",
            "prabha.RiceStock", "prabha.Expense_Info",
            "prabha.PartyInfo", "prabha.BrokerInfo"
        };
        return Array.Exists(allowed, x => x == t);
    }

    private SqlDbType GetSqlDbType(Type t)
    {
        if (t == typeof(int)) return SqlDbType.Int;
        if (t == typeof(long)) return SqlDbType.BigInt;
        if (t == typeof(decimal)) return SqlDbType.Decimal;
        if (t == typeof(double)) return SqlDbType.Float;
        if (t == typeof(float)) return SqlDbType.Real;
        if (t == typeof(DateTime)) return SqlDbType.DateTime;
        if (t == typeof(bool)) return SqlDbType.Bit;
        return SqlDbType.NVarChar;
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