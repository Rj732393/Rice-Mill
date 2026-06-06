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

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Session.Clear();
        }
    }

    protected void login_Click(object sender, EventArgs e)
    {
        // Request.Form se lenge — kisi bhi control type ke saath kaam karega
        string uname = Request.Form["userName"] != null ? Request.Form["userName"].Trim() : "";
        string upass = Request.Form["pwd"] != null ? Request.Form["pwd"].Trim() : "";

        if (string.IsNullOrEmpty(uname) || string.IsNullOrEmpty(upass))
        {
            lblMsg.Text = "Username aur Password daalein!";
            return;
        }

        dac = new DataAccessLayer();

        // CHECK 1: SuperAdmin hai kya?
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@UserName", uname));
        param.Add(new SqlParameter("@Password", upass));

        dt = dac.GetDataTable(
            "SELECT * FROM prabha.SuperAdmin WHERE UserName=@UserName AND UPassword=@Password",
            param);

        if (dt.Rows.Count > 0)
        {
            Session["User"] = uname;
            Session["UserType"] = "SuperAdmin";
            Session["CompanyID"] = 0;
            Session["CompanyName"] = "Prabha Software Technologies";
            Response.Redirect("superadmin/Dashboard.aspx");
            return;
        }

        // CHECK 2: Company Admin hai kya?
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@UserName", uname));
        param.Add(new SqlParameter("@Password", upass));

        dt = dac.GetDataTable(
            "SELECT * FROM prabha.Companies WHERE AdminUserName=@UserName AND AdminPassword=@Password",
            param);

        if (dt.Rows.Count > 0)
        {
            DataRow company = dt.Rows[0];
            bool isActive = Convert.ToBoolean(company["IsActive"]);
            DateTime subEnd = Convert.ToDateTime(company["SubscriptionEnd"]);

            if (!isActive || subEnd < DateTime.Today)
            {
                lblMsg.Text = "Aapki subscription khatam ho gayi hai. Kripya Prabha Software se sampark karein.";
                return;
            }

            Session["User"] = uname;
            Session["UserType"] = "Admin";
            Session["CompanyID"] = Convert.ToInt32(company["CompanyID"]);
            Session["CompanyName"] = company["CompanyName"].ToString();
            Response.Redirect("Admin/Dashboard.aspx");
            return;
        }

        // CHECK 3: Normal User hai kya?
        param = new List<SqlParameter>();
        param.Add(new SqlParameter("@UserName", uname));
        param.Add(new SqlParameter("@Password", upass));

        dt = dac.GetDataTable(
            @"SELECT u.*, c.IsActive, c.SubscriptionEnd, c.CompanyName
              FROM prabha.UserInfo u
              INNER JOIN prabha.Companies c ON u.CompanyID = c.CompanyID
              WHERE u.UserName=@UserName AND u.UPassword=@Password",
            param);

        if (dt.Rows.Count > 0)
        {
            DataRow user = dt.Rows[0];
            bool isActive = Convert.ToBoolean(user["IsActive"]);
            DateTime subEnd = Convert.ToDateTime(user["SubscriptionEnd"]);

            if (!isActive || subEnd < DateTime.Today)
            {
                lblMsg.Text = "Aapki company ki subscription khatam ho gayi hai. Admin se sampark karein.";
                return;
            }

            Session["User"] = uname;
            Session["UserType"] = "User";
            Session["CompanyID"] = Convert.ToInt32(user["CompanyID"]);
            Session["CompanyName"] = user["CompanyName"].ToString();
            Response.Redirect("Home.aspx");
            return;
        }

        lblMsg.Text = "Username ya Password galat hai!";
    }
}
