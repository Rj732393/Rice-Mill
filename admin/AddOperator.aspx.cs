using System;
using System.Data.SqlClient;
using System.Configuration;

public partial class admin_AddOperator : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(
        ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null ||
            Session["UserType"] == null ||
            Session["UserType"].ToString() != "Admin")
        {
            Response.Redirect("../Login.aspx");
            return;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        con.Open();

        int companyID = Session["CompanyID"] != null ? Convert.ToInt32(Session["CompanyID"]) : 0;

        SqlCommand cmd = new SqlCommand(
            "INSERT INTO prabha.UserInfo (UserName,UPassword,CreatedDate,UserType,IsActive,CompanyID) " +
            "VALUES (@u,@p,GETDATE(),'Operator',1,@cid)", con);
        cmd.Parameters.AddWithValue("@u", txtUser.Text);
        cmd.Parameters.AddWithValue("@p", txtPass.Text);
        cmd.Parameters.AddWithValue("@cid", companyID);
        cmd.ExecuteNonQuery();

        con.Close();

        txtUser.Text = "";
        txtPass.Text = "";
        lblMsg.Text = "Operator added successfully!";
    }
}