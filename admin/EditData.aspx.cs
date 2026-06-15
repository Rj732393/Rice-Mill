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
        if (Session["User"] == null || Session["UserType"] == null)
        {
            Response.Redirect("../Login.aspx");
            return;
        }

        string userType = Session["UserType"].ToString();
        if (userType != "Admin" && userType != "SuperAdmin")
        {
            Response.Redirect("../Login.aspx");
            return;
        }

        if (!Page.IsPostBack)
        {
            Session["EditTable"] = null;
            Session["EditID"] = null;

            // Company naam session se set karo
            string companyName = Session["CompanyName"] != null
                ? Session["CompanyName"].ToString()
                : "Rice Mills";
            lblCompanyName.Text = companyName;
            return;
        }

        // PostBack pe — pehle check karo kaunsa button click hua
        string savedTable = Session["EditTable"] != null ? Session["EditTable"].ToString() : "";

        // Edit button click check
        string editPkVal = "";
        foreach (string key in Request.Form.AllKeys)
        {
            if (key != null && key.StartsWith("editBtn_"))
            {
                editPkVal = key.Substring("editBtn_".Length);
                break;
            }
        }

        if (!string.IsNullOrEmpty(editPkVal) && !string.IsNullOrEmpty(savedTable))
        {
            // Edit button click hua — table load karo + form open karo
            ddlTable.SelectedValue = savedTable;
            LoadTableData(savedTable);
            OpenEditForm(savedTable, editPkVal);
        }
        else if (!string.IsNullOrEmpty(savedTable) && IsAllowedTable(savedTable))
        {
            // Save ya Cancel ke baad table reload
            ddlTable.SelectedValue = savedTable;
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

            lblTableTitle.Text = ddlTable.SelectedItem != null
                ? ddlTable.SelectedItem.Text + " (" + dt.Rows.Count + " records)"
                : tableName + " (" + dt.Rows.Count + " records)";
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
                    html.Append("<input type='submit' name='editBtn_" + pkVal + "' value='Edit' class='btn-edit-row' style='background:#27ae60;color:#fff;border:none;border-radius:3px;padding:4px 12px;cursor:pointer;font-size:12px;' />");
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
            lblEditID.Text = "(" + pkCol + " = " + pkVal + ")";
            editFormPanel.Visible = true;
            Session["EditID"] = pkVal;

            string warningMsg = GetCascadeWarning(tableName);
            if (!string.IsNullOrEmpty(warningMsg))
                ShowMessage("⚠️ " + warningMsg, false);

            StringBuilder html = new StringBuilder();
            html.Append("<div style='display:flex;flex-wrap:wrap;gap:12px;'>");
            foreach (DataColumn col in dt.Columns)
            {
                string fieldVal = row[col] == DBNull.Value ? "" : row[col].ToString();
                string inputType = (col.DataType == typeof(decimal) || col.DataType == typeof(int) ||
                                    col.DataType == typeof(long) || col.DataType == typeof(double) ||
                                    col.DataType == typeof(float)) ? "number" : "text";

                html.Append("<div style='flex:0 0 calc(50% - 12px);margin-bottom:10px;'>");
                html.Append("<label style='font-weight:bold;font-size:12px;color:#555;display:block;'>" + col.ColumnName + "</label>");

                if (col.ColumnName == pkCol)
                {
                    html.Append("<input type='text' name='ef_" + col.ColumnName + "' value='"
                        + HttpUtility.HtmlEncode(fieldVal) + "' readonly "
                        + "style='width:100%;padding:4px 8px;border:1px solid #ccc;border-radius:3px;background:#f5f5f5;color:#999;box-sizing:border-box;' />");
                }
                else
                {
                    html.Append("<input type='" + inputType + "' name='ef_" + col.ColumnName + "' value='"
                        + HttpUtility.HtmlEncode(fieldVal) + "' "
                        + "style='width:100%;padding:4px 8px;border:1px solid #aaa;border-radius:3px;box-sizing:border-box;' />");
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
            dt = dac.GetDataTable("SELECT * FROM " + tableName + " WHERE " + pkCol + "=@pkval", param);
            if (dt.Rows.Count == 0) { ShowMessage("Record not found.", false); return; }

            DataRow oldRow = dt.Rows[0];
            Dictionary<string, string> oldValues = new Dictionary<string, string>();
            Dictionary<string, string> newValues = new Dictionary<string, string>();

            foreach (DataColumn col in dt.Columns)
            {
                oldValues[col.ColumnName] = oldRow[col] == DBNull.Value ? "" : oldRow[col].ToString();
                string formKey = "ef_" + col.ColumnName;
                string newVal = Request.Form[formKey];
                newValues[col.ColumnName] = newVal ?? oldValues[col.ColumnName];
            }

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
            dac = new DataAccessLayer();
            dac.update("UPDATE " + tableName + " SET " + setClauses + " WHERE [" + pkCol + "]=@pkval", param);

            int cascadeCount = RunCascadeUpdates(tableName, oldValues, newValues);

            Session["EditID"] = null;
            editFormPanel.Visible = false;
            ddlTable.SelectedValue = tableName;
            LoadTableData(tableName);

            string cascadeMsg = cascadeCount > 0 ? " + " + cascadeCount + " linked table(s) bhi update hue." : "";
            ShowMessage("✅ Record successfully updated!" + cascadeMsg, true);
        }
        catch (Exception ex)
        {
            ShowMessage("Error saving: " + ex.Message, false);
        }
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

    private int RunCascadeUpdates(string tableName, Dictionary<string, string> oldVal, Dictionary<string, string> newVal)
    {
        int count = 0;
        dac = new DataAccessLayer();

        if (tableName == "prabha.Purchase_Party_Info")
        {
            string oldName = oldVal.ContainsKey("Party_Name") ? oldVal["Party_Name"] : "";
            string newName = newVal.ContainsKey("Party_Name") ? newVal["Party_Name"] : "";
            if (oldName != newName && !string.IsNullOrEmpty(oldName))
            {
                CascadeUpdate("prabha.Purchase_Sauda_Info", "PartyName", oldName, newName);
                CascadeUpdate("prabha.Purchase_Master_Data", "PartyName", oldName, newName);
                CascadeUpdate("prabha.Purchase_Payment_Info", "PName", oldName, newName);
                count += 3;
            }
        }
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
        if (tableName == "prabha.Purchase_Master_Data")
        {
            string oldID = oldVal.ContainsKey("ID") ? oldVal["ID"] : "";
            string newID = newVal.ContainsKey("ID") ? newVal["ID"] : "";
            if (oldID != newID && !string.IsNullOrEmpty(oldID))
            {
                CascadeUpdateNumeric("prabha.Purchase_Item_Info", "Master_ID", oldID, newID);
                count++;
            }
            string oldParty = oldVal.ContainsKey("PartyName") ? oldVal["PartyName"] : "";
            string newParty = newVal.ContainsKey("PartyName") ? newVal["PartyName"] : "";
            if (oldParty != newParty && !string.IsNullOrEmpty(oldParty))
            {
                CascadeUpdate("prabha.Purchase_Payment_Info", "PName", oldParty, newParty);
                count++;
            }
        }
        if (tableName == "prabha.Sale_Master_Data")
        {
            string oldID = oldVal.ContainsKey("ID") ? oldVal["ID"] : "";
            string newID = newVal.ContainsKey("ID") ? newVal["ID"] : "";
            if (oldID != newID && !string.IsNullOrEmpty(oldID))
            {
                CascadeUpdateNumeric("prabha.Sale_Item_Info", "Master_ID", oldID, newID);
                count++;
            }
            string oldParty = oldVal.ContainsKey("PartyName") ? oldVal["PartyName"] : "";
            string newParty = newVal.ContainsKey("PartyName") ? newVal["PartyName"] : "";
            if (oldParty != newParty && !string.IsNullOrEmpty(oldParty))
            {
                CascadeUpdate("prabha.Sale_Payment_Info", "PName", oldParty, newParty);
                count++;
            }
        }
        if (tableName == "prabha.Sale_Sauda_Master")
        {
            string oldID = oldVal.ContainsKey("ID") ? oldVal["ID"] : "";
            string newID = newVal.ContainsKey("ID") ? newVal["ID"] : "";
            if (oldID != newID && !string.IsNullOrEmpty(oldID))
            {
                CascadeUpdateNumeric("prabha.Sale_Sauda_Item_Info", "MasterID", oldID, newID);
                count++;
            }
        }
        return count;
    }

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

    private string GetCascadeWarning(string tableName)
    {
        switch (tableName)
        {
            case "prabha.Purchase_Party_Info": return "Party Name badalne se Purchase Sauda, Purchase Bill aur Purchase Payment bhi update honge.";
            case "prabha.PartyInfo": return "Party Name badalne se Sale Sauda, Sale Bill aur Sale Payment bhi update honge.";
            case "prabha.BrokerInfo": return "Broker Name badalne se Purchase Sauda, Purchase Bill, Sale Sauda aur Sale Bill bhi update honge.";
            case "prabha.Purchase_Master_Data": return "Is record se linked Purchase Items bhi affect honge.";
            case "prabha.Sale_Master_Data": return "Is record se linked Sale Items bhi affect honge.";
            case "prabha.Sale_Sauda_Master": return "Is record se linked Sale Sauda Items bhi affect honge.";
            default: return "";
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