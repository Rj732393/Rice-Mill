using System;
using System.Web;
using System.Web.UI;

public partial class admin_Dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Session check - SaaS wala
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

        if (!IsPostBack)
        {
            // Company naam session se lo
            string companyName = Session["CompanyName"] != null
                ? Session["CompanyName"].ToString()
                : "Rice Mills";

            lblDashboardCompany.Text = companyName;
        }
    }
}