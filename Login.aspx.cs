using System;
using System.Collections.Generic;
using System.Linq;
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
            Session["User"] = null;
        }
    }

    protected void login_Click(object sender, EventArgs e)
    {
        dt = new DataTable();
        string q = "";
        param = new List<SqlParameter>();

        // ✅ ONLY FIX IS HERE (.Value → .Text)
        param.Add(new SqlParameter("@UserName", userName.Text.Trim()));
        param.Add(new SqlParameter("@Password", pwd.Text.Trim()));

        q = "select * from prabha.UserInfo where UserName=@UserName and UPassword=@Password";

        dac = new DataAccessLayer();
        dt = dac.GetDataTable(q, param);

        if (dt.Rows.Count > 0)
        {
            Session["User"] = userName.Text.Trim();

            if (userName.Text == "admin")
            {
                Response.Redirect("Admin/Dashboard.aspx");
            }
            else
            {
                Response.Redirect("Home.aspx");
            }
        }
        else
        {
            lblMsg.Text = "Wrong credentials!!";
        }
    }
}