using System;

public partial class Includes_AdminMenu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Session se Company ka naam lo
        string companyName = "Rashmi Rice"; // default

        if (Session["CompanyName"] != null &&
            !string.IsNullOrWhiteSpace(Session["CompanyName"].ToString()))
        {
            companyName = Session["CompanyName"].ToString();
        }

        // Sidebar logo aur navbar title set karo
        lblSidebarCompany.Text = companyName;
        lblNavbarCompany.Text = companyName + " Management";
    }
}