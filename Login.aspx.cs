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
        param = new List<SqlParameter>();//Emp_Id

        param.Add(new SqlParameter("@UserName", userName.Value.Trim()));
        param.Add(new SqlParameter("@Password", pwd.Value.Trim()));
        q = "select * from prabha.UserInfo where UserName=@UserName and UPassword=@Password";
        dac = new DataAccessLayer();
        dt = dac.GetDataTable(q, param);

        if (dt.Rows.Count > 0)
        {
            Session["User"] = userName.Value.Trim();
            if (userName.Value == "admin")
            {
                Response.Redirect("Admin/RiceStock.aspx");
            }
            else
            {
                Response.Redirect("PurchaseUnloading.aspx");
            }
            

        }
        else
        {
            lblMsg.Text = "Wrong credentials!!";
        }
    }
}