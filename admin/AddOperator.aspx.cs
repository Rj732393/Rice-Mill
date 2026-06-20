using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using substitute;

public partial class admin_AddOperator : System.Web.UI.Page
{
    DataAccessLayer dac = new DataAccessLayer();

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

        // Duplicate username check - GLOBALLY unique (Login query CompanyID ke bina match karti hai,
        // isliye alag companies me bhi same username clash karega)
        var chkParam = new List<SqlParameter>();
        chkParam.Add(new SqlParameter("@UserName", uname));

        object existingCount = dac.Scalar(
            "SELECT COUNT(*) FROM prabha.UserInfo WHERE UserName=@UserName",
            chkParam);

        if (Convert.ToInt32(existingCount) > 0)
        {
            lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Text = "Ye username already kisi aur company me bhi maujood hai. Doosra username try karo.";
            return;
        }

        var param = new List<SqlParameter>();
        param.Add(new SqlParameter("@UserName", uname));
        param.Add(new SqlParameter("@UPassword", pass));
        param.Add(new SqlParameter("@CompanyID", companyID));
        // RoleID 5 = Operator (1=SuperAdmin, 2=CompanyAdmin, 3=Manager, 4=Accountant, 5=Operator)
        param.Add(new SqlParameter("@RoleID", 5));

        dac.update(
            @"INSERT INTO prabha.UserInfo (UserName, UPassword, CreatedDate, UserType, IsActive, CompanyID, RoleID)
              VALUES (@UserName, @UPassword, GETDATE(), 'Operator', 1, @CompanyID, @RoleID)",
            param);

        txtUser.Text = "";
        txtPass.Text = "";
        lblMsg.ForeColor = System.Drawing.Color.Green;
        lblMsg.Text = "Operator add ho gaya!";
    }
}