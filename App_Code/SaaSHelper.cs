using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace substitute
{
    /// <summary>
    /// Helper class for SaaS cross-cutting concerns:
    /// audit logging, permission checks, subscription status.
    /// </summary>
    public class SaaSHelper
    {
        DataAccessLayer dac = new DataAccessLayer();

        /// <summary>
        /// Writes an entry to prabha.AuditLogs.
        /// </summary>
        public void LogAction(int? companyID, string userName, string userType,
                               string action, string module, string description)
        {
            try
            {
                var param = new List<SqlParameter>();
                param.Add(new SqlParameter("@CompanyID", companyID.HasValue ? (object)companyID.Value : DBNull.Value));
                param.Add(new SqlParameter("@UserName", userName ?? ""));
                param.Add(new SqlParameter("@UserType", userType ?? ""));
                param.Add(new SqlParameter("@Action", action ?? ""));
                param.Add(new SqlParameter("@Module", module ?? (object)DBNull.Value));
                param.Add(new SqlParameter("@Description", description ?? (object)DBNull.Value));

                string ip = "";
                try { ip = HttpContext.Current.Request.UserHostAddress; } catch { }
                param.Add(new SqlParameter("@IPAddress", string.IsNullOrEmpty(ip) ? (object)DBNull.Value : ip));

                dac.update(@"INSERT INTO prabha.AuditLogs
                    (CompanyID, UserName, UserType, Action, Module, Description, IPAddress, CreatedDate)
                    VALUES (@CompanyID, @UserName, @UserType, @Action, @Module, @Description, @IPAddress, GETDATE())",
                    param);
            }
            catch
            {
                // Audit logging must never break the main flow.
            }
        }

        /// <summary>
        /// Checks if the logged-in user (by RoleID) has a given permission key.
        /// </summary>
        public bool HasPermission(int roleID, string permissionKey)
        {
            var param = new List<SqlParameter>();
            param.Add(new SqlParameter("@RoleID", roleID));
            param.Add(new SqlParameter("@PermissionKey", permissionKey));

            object result = dac.Scalar(@"
                SELECT COUNT(*) FROM prabha.RolePermissions rp
                INNER JOIN prabha.Permissions p ON rp.PermissionID = p.PermissionID
                WHERE rp.RoleID = @RoleID AND p.PermissionKey = @PermissionKey", param);

            return Convert.ToInt32(result) > 0;
        }

        /// <summary>
        /// Returns the current subscription status for a company:
        /// "Active", "Expired", "Suspended", "Blocked"
        /// Also auto-updates Companies.Status to "Expired" if EndDate has passed.
        /// </summary>
        public string GetSubscriptionStatus(int companyID)
        {
            var param = new List<SqlParameter>();
            param.Add(new SqlParameter("@CompanyID", companyID));

            DataTable dt = dac.GetDataTable(@"
                SELECT TOP 1 c.Status, cs.EndDate
                FROM prabha.Companies c
                LEFT JOIN prabha.CompanySubscriptions cs ON cs.CompanyID = c.CompanyID
                WHERE c.CompanyID = @CompanyID
                ORDER BY cs.EndDate DESC", param);

            if (dt.Rows.Count == 0) return "Blocked";

            string status = dt.Rows[0]["Status"].ToString();

            if (status == "Suspended" || status == "Blocked") return status;

            if (dt.Rows[0]["EndDate"] != DBNull.Value)
            {
                DateTime endDate = Convert.ToDateTime(dt.Rows[0]["EndDate"]);
                if (endDate < DateTime.Today)
                {
                    // Auto-mark expired
                    var updParam = new List<SqlParameter>();
                    updParam.Add(new SqlParameter("@CompanyID", companyID));
                    dac.update("UPDATE prabha.Companies SET Status = N'Expired' WHERE CompanyID = @CompanyID", updParam);
                    return "Expired";
                }
            }

            return status; // Active
        }

        /// <summary>
        /// Creates notifications for companies whose subscription is expiring
        /// in 7 / 3 / 0 days, or already inactive. Call this from a scheduled
        /// task (e.g. Application_Start timer or a Windows Task calling a
        /// dedicated endpoint once a day).
        /// </summary>
        public void RunSubscriptionNotificationCheck()
        {
            DataTable dt = dac.GetDataTable(@"
                SELECT c.CompanyID, c.CompanyName, c.Status,
                       MAX(cs.EndDate) AS EndDate
                FROM prabha.Companies c
                LEFT JOIN prabha.CompanySubscriptions cs ON cs.CompanyID = c.CompanyID
                GROUP BY c.CompanyID, c.CompanyName, c.Status", null);

            foreach (DataRow row in dt.Rows)
            {
                if (row["EndDate"] == DBNull.Value) continue;

                int companyID = Convert.ToInt32(row["CompanyID"]);
                string companyName = row["CompanyName"].ToString();
                string status = row["Status"].ToString();
                DateTime endDate = Convert.ToDateTime(row["EndDate"]);
                int daysLeft = (endDate.Date - DateTime.Today).Days;

                if (status == "Suspended" || status == "Blocked")
                {
                    CreateNotificationIfNotExists(companyID, "CompanyInactive",
                        "Company Inactive",
                        companyName + " is currently " + status + ".");
                    continue;
                }

                if (daysLeft == 7)
                    CreateNotificationIfNotExists(companyID, "SubExpiring7",
                        "Subscription Expiring Soon",
                        companyName + "'s subscription will expire in 7 days.");
                else if (daysLeft == 3)
                    CreateNotificationIfNotExists(companyID, "SubExpiring3",
                        "Subscription Expiring Soon",
                        companyName + "'s subscription will expire in 3 days.");
                else if (daysLeft == 0)
                    CreateNotificationIfNotExists(companyID, "SubExpiringToday",
                        "Subscription Expires Today",
                        companyName + "'s subscription expires today.");
                else if (daysLeft < 0 && status == "Active")
                {
                    var updParam = new List<SqlParameter>();
                    updParam.Add(new SqlParameter("@CompanyID", companyID));
                    dac.update("UPDATE prabha.Companies SET Status = N'Expired' WHERE CompanyID = @CompanyID", updParam);

                    CreateNotificationIfNotExists(companyID, "SubExpired",
                        "Subscription Expired",
                        companyName + "'s subscription has expired.");
                }
            }
        }

        private void CreateNotificationIfNotExists(int companyID, string type, string title, string message)
        {
            // Avoid duplicate notifications for the same type on the same day
            var checkParam = new List<SqlParameter>();
            checkParam.Add(new SqlParameter("@CompanyID", companyID));
            checkParam.Add(new SqlParameter("@Type", type));

            object existing = dac.Scalar(@"
                SELECT COUNT(*) FROM prabha.Notifications
                WHERE CompanyID = @CompanyID AND NotificationType = @Type
                AND CAST(CreatedDate AS DATE) = CAST(GETDATE() AS DATE)", checkParam);

            if (Convert.ToInt32(existing) > 0) return;

            var param = new List<SqlParameter>();
            param.Add(new SqlParameter("@CompanyID", companyID));
            param.Add(new SqlParameter("@Title", title));
            param.Add(new SqlParameter("@Message", message));
            param.Add(new SqlParameter("@Type", type));

            dac.update(@"INSERT INTO prabha.Notifications
                (CompanyID, Title, Message, NotificationType, IsRead, CreatedDate)
                VALUES (@CompanyID, @Title, @Message, @Type, 0, GETDATE())", param);
        }
    }
}
