using System;

public partial class Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        // Session Clear

        Session.Clear();

        Session.Abandon();

        // Cache Clear

        Response.Cache.SetCacheability(
            System.Web.HttpCacheability.NoCache);

        Response.Cache.SetNoStore();

        Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));

        // Redirect Login Page

        Response.Redirect("../Login.aspx");
    }
}