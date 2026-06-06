using System;
using System.Web;
using System.Web.UI;

public partial class Home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Login nahi hai to wapas bhejo
        if (Session["User"] == null)
        {
            Response.Redirect("Login.aspx");
            return;
        }

        // Admin yahan na aaye
        if (Session["UserType"] != null && Session["UserType"].ToString() == "Admin")
        {
            Response.Redirect("Admin/Dashboard.aspx");
            return;
        }

        // SuperAdmin yahan na aaye
        if (Session["UserType"] != null && Session["UserType"].ToString() == "SuperAdmin")
        {
            Response.Redirect("superadmin/Dashboard.aspx");
            return;
        }
    }
}
