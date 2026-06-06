using System;
using System.Web;
using System.Web.UI;

public partial class admin_Dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Session check - sirf Admin aa sake
        if (Session["User"] == null || Session["UserType"] == null ||
            Session["UserType"].ToString() != "Admin")
        {
            Response.Redirect("../Login.aspx");
            return;
        }
    }
}
