using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using substitute;

public partial class Login : System.Web.UI.Page
{
    DataTable dt;
    List<SqlParameter> param;
    DataAccessLayer dac;
    SaaSHelper saas = new SaaSHelper();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Session.Clear();
        }
    }

    protected void login_Click(object sender, EventArgs e)
    {
        try
        {
            string uname = Request.Form["userName"] != null ? Request.Form["userName"].Trim() : "";
            string upass = Request.Form["pwd"] != null ? Request.Form["pwd"].Trim() : "";

            if (string.IsNullOrEmpty(uname) || string.IsNullOrEmpty(upass))
            {
                lblMsg.Text = "Please enter Username and Password!";
                return;
            }

            dac = new DataAccessLayer();

            // CHECK 1: SuperAdmin
            param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserName", uname));
            param.Add(new SqlParameter("@Password", upass));

            dt = dac.GetDataTable(
                "SELECT * FROM prabha.SuperAdmin WHERE UserName=@UserName AND UPassword=@Password",
                param);

            if (dt != null && dt.Rows.Count > 0)
            {
                Session["User"] = uname;
                Session["UserType"] = "SuperAdmin";
                Session["CompanyID"] = 0;
                Session["CompanyName"] = "Prabha Software Technologies";
                Session["RoleID"] = 1;

                saas.LogAction(null, uname, "SuperAdmin", "Login", "Auth", "SuperAdmin logged in");

                Response.Redirect("superadmin/Dashboard.aspx");
                return;
            }

            // CHECK 2: Company Admin
            param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserName", uname));
            param.Add(new SqlParameter("@Password", upass));

            dt = dac.GetDataTable(@"
                SELECT c.*, sp.PlanName
                FROM prabha.Companies c
                LEFT JOIN prabha.SubscriptionPlans sp ON sp.PlanID = c.PlanID
                WHERE c.AdminUserName=@UserName AND c.AdminPassword=@Password",
                param);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow company = dt.Rows[0];
                int companyID = Convert.ToInt32(company["CompanyID"]);
                string companyName = company["CompanyName"].ToString();

                string status = saas.GetSubscriptionStatus(companyID);

                if (status == "Suspended" || status == "Blocked")
                {
                    lblMsg.Text = "Your company has been " + (status == "Blocked" ? "blocked" : "suspended") +
                        ". Please contact Super Admin.";
                    saas.LogAction(companyID, uname, "Admin", "LoginBlocked", "Auth",
                        "Login attempt blocked - company status: " + status);
                    return;
                }

                if (status == "Expired")
                {
                    lblMsg.Text = "Your subscription has expired. Please contact Super Admin to renew your plan.";
                    saas.LogAction(companyID, uname, "Admin", "LoginBlocked", "Auth",
                        "Login attempt blocked - subscription expired");
                    return;
                }

                Session["User"] = uname;
                Session["UserType"] = "Admin";
                Session["CompanyID"] = companyID;
                Session["CompanyName"] = companyName;
                Session["RoleID"] = 2;

                saas.LogAction(companyID, uname, "Admin", "Login", "Auth", "Company Admin logged in");

                Response.Redirect("Admin/Dashboard.aspx");
                return;
            }

            // CHECK 3: Normal User
            param = new List<SqlParameter>();
            param.Add(new SqlParameter("@UserName", uname));
            param.Add(new SqlParameter("@Password", upass));

            dt = dac.GetDataTable(@"
                SELECT u.*, c.Status, c.SubscriptionEnd, c.CompanyName, r.RoleName
                FROM prabha.UserInfo u
                INNER JOIN prabha.Companies c ON u.CompanyID = c.CompanyID
                LEFT JOIN prabha.Roles r ON r.RoleID = u.RoleID
                WHERE u.UserName=@UserName AND u.UPassword=@Password",
                param);

            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow user = dt.Rows[0];
                int companyID = Convert.ToInt32(user["CompanyID"]);
                bool userActive = user["IsActive"] != DBNull.Value ? Convert.ToBoolean(user["IsActive"]) : true;

                if (!userActive)
                {
                    lblMsg.Text = "Your account has been deactivated. Please contact your Company Admin.";
                    return;
                }

                string status = saas.GetSubscriptionStatus(companyID);

                if (status == "Suspended" || status == "Blocked")
                {
                    lblMsg.Text = "Your company has been " + (status == "Blocked" ? "blocked" : "suspended") +
                        ". Please contact Super Admin.";
                    saas.LogAction(companyID, uname, "User", "LoginBlocked", "Auth",
                        "Login attempt blocked - company status: " + status);
                    return;
                }

                if (status == "Expired")
                {
                    lblMsg.Text = "Your company's subscription has expired. Please contact your Admin.";
                    saas.LogAction(companyID, uname, "User", "LoginBlocked", "Auth",
                        "Login attempt blocked - subscription expired");
                    return;
                }

                Session["User"] = uname;
                Session["UserType"] = "User";
                Session["CompanyID"] = companyID;
                Session["CompanyName"] = user["CompanyName"].ToString();
                Session["RoleID"] = user["RoleID"] != DBNull.Value ? Convert.ToInt32(user["RoleID"]) : 5;
                Session["RoleName"] = user["RoleName"] != DBNull.Value ? user["RoleName"].ToString() : "Operator";

                saas.LogAction(companyID, uname, "User", "Login", "Auth", "User logged in");

                Response.Redirect("Home.aspx");
                return;
            }

            lblMsg.Text = "Invalid Username or Password!";
        }
        catch (Exception)
        {
            lblMsg.Text = "Unable to connect to server. Please try again later.";
        }
    }
}