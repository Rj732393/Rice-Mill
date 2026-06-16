using System;
using System.Web.UI;

public partial class Includes_WebUserControl : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["CompanyName"] != null)
            {
                lblCompany.Text = Session["CompanyName"].ToString() + " Management System";
            }
            else
            {
                lblCompany.Text = "Rice Mill Management System";
            }
        }
    }
}