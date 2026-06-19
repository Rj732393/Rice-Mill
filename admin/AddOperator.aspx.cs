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
        string uname = txtUser.Text.Trim();
        string pass = txtPass.Text.Trim();

        if (string.IsNullOrEmpty(uname) || string.IsNullOrEmpty(pass))
        {
            lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Text = "Username aur Password dono bharna zaroori hai.";
            return;
        }

        int companyID = Session["CompanyID"] != null ? Convert.ToInt32(Session["CompanyID"]) : 0;

        con.Open();

        // Duplicate username check (same company ke andar)
        SqlCommand chkCmd = new SqlCommand(
            "SELECT COUNT(*) FROM prabha.UserInfo WHERE UserName=@u AND CompanyID=@cid", con);
        chkCmd.Parameters.AddWithValue("@u", uname);
        chkCmd.Parameters.AddWithValue("@cid", companyID);

        int existingCount = (int)chkCmd.ExecuteScalar();

        if (existingCount > 0)
        {
            con.Close();
            lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Text = "Ye username already maujood hai. Doosra username try karo.";
            return;
        }

        SqlCommand cmd = new SqlCommand(
            "INSERT INTO prabha.UserInfo (UserName,UPassword,CreatedDate,UserType,IsActive,CompanyID) " +
            "VALUES (@u,@p,GETDATE(),'Operator',1,@cid)", con);
        cmd.Parameters.AddWithValue("@u", uname);
        cmd.Parameters.AddWithValue("@p", pass);
        cmd.Parameters.AddWithValue("@cid", companyID);
        cmd.ExecuteNonQuery();

        con.Close();

        txtUser.Text = "";
        txtPass.Text = "";
        lblMsg.ForeColor = System.Drawing.Color.Green;
        lblMsg.Text = "Operator added successfully!";
    }
}