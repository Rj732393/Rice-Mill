using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using substitute;

public partial class superadmin_AddCompany : System.Web.UI.Page
{
    DataAccessLayer dac = new DataAccessLayer();

    protected void Page_Load(object sender, EventArgs e)
    {
        // Sirf SuperAdmin aa sake
        if (Session["User"] == null || Session["UserType"] == null ||
            Session["UserType"].ToString() != "SuperAdmin")
        {
            Response.Redirect("../Login.aspx");
            return;
        }

        if (!IsPostBack)
        {
            // Default dates set karo
            txtStartDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
            txtEndDate.Text = DateTime.Today.AddYears(1).ToString("yyyy-MM-dd");

            // Edit mode check
            if (Request.QueryString["id"] != null)
            {
                int cid = Convert.ToInt32(Request.QueryString["id"]);
                hfCompanyID.Value = cid.ToString();
                btnSave.Text = "Update Karo";
                LoadCompany(cid);
            }
        }
    }

    private void LoadCompany(int cid)
    {
        var param = new List<SqlParameter>();
        param.Add(new SqlParameter("@CompanyID", cid));

        DataTable dt = dac.GetDataTable(
            "SELECT * FROM prabha.Companies WHERE CompanyID=@CompanyID", param);

        if (dt.Rows.Count > 0)
        {
            DataRow r = dt.Rows[0];
            txtCompanyName.Text = r["CompanyName"].ToString();
            txtOwnerName.Text = r["OwnerName"].ToString();
            txtPhone.Text = r["Phone"].ToString();
            txtEmail.Text = r["Email"].ToString();
            txtAddress.Text = r["Address"].ToString();
            txtCity.Text = r["City"].ToString();
            txtState.Text = r["State"].ToString();
            txtUserName.Text = r["AdminUserName"].ToString();
            txtPassword.Text = r["AdminPassword"].ToString();
            txtStartDate.Text = Convert.ToDateTime(r["SubscriptionStart"]).ToString("yyyy-MM-dd");
            txtEndDate.Text = Convert.ToDateTime(r["SubscriptionEnd"]).ToString("yyyy-MM-dd");
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        // Validation
        if (string.IsNullOrWhiteSpace(txtCompanyName.Text) ||
            string.IsNullOrWhiteSpace(txtUserName.Text) ||
            string.IsNullOrWhiteSpace(txtPassword.Text) ||
            string.IsNullOrWhiteSpace(txtStartDate.Text) ||
            string.IsNullOrWhiteSpace(txtEndDate.Text))
        {
            lblMsg.Text = "Company Naam, Username, Password aur Dates zaroori hain!";
            return;
        }

        int companyID = Convert.ToInt32(hfCompanyID.Value);
        var param = new List<SqlParameter>();

        param.Add(new SqlParameter("@CompanyName", txtCompanyName.Text.Trim()));
        param.Add(new SqlParameter("@AdminUserName", txtUserName.Text.Trim()));
        param.Add(new SqlParameter("@AdminPassword", txtPassword.Text.Trim()));
        param.Add(new SqlParameter("@OwnerName", txtOwnerName.Text.Trim()));
        param.Add(new SqlParameter("@Phone", txtPhone.Text.Trim()));
        param.Add(new SqlParameter("@Email", txtEmail.Text.Trim()));
        param.Add(new SqlParameter("@Address", txtAddress.Text.Trim()));
        param.Add(new SqlParameter("@City", txtCity.Text.Trim()));
        param.Add(new SqlParameter("@State", txtState.Text.Trim()));
        param.Add(new SqlParameter("@SubStart", txtStartDate.Text));
        param.Add(new SqlParameter("@SubEnd", txtEndDate.Text));

        int result;

        if (companyID == 0)
        {
            // Naya record
            result = dac.update(@"
                INSERT INTO prabha.Companies
                (CompanyName, AdminUserName, AdminPassword, OwnerName, Phone, Email,
                 Address, City, State, SubscriptionStart, SubscriptionEnd, IsActive)
                VALUES
                (@CompanyName, @AdminUserName, @AdminPassword, @OwnerName, @Phone, @Email,
                 @Address, @City, @State, @SubStart, @SubEnd, 1)", param);
        }
        else
        {
            // Update existing
            param.Add(new SqlParameter("@CompanyID", companyID));
            result = dac.update(@"
                UPDATE prabha.Companies SET
                    CompanyName=@CompanyName, AdminUserName=@AdminUserName,
                    AdminPassword=@AdminPassword, OwnerName=@OwnerName,
                    Phone=@Phone, Email=@Email, Address=@Address,
                    City=@City, State=@State,
                    SubscriptionStart=@SubStart, SubscriptionEnd=@SubEnd
                WHERE CompanyID=@CompanyID", param);
        }

        if (result > 0)
        {
            string msg = companyID == 0 ? "saved" : "updated";
            Response.Redirect("Dashboard.aspx?msg=" + msg);
        }
        else
        {
            lblMsg.Text = "Kuch galat hua, dobara try karein.";
        }
    }
}
