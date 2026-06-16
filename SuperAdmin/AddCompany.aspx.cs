using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using substitute;

public partial class superadmin_AddCompany : System.Web.UI.Page
{
    DataAccessLayer dac = new DataAccessLayer();
    SaaSHelper saas = new SaaSHelper();

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

        string query = "SELECT * FROM prabha.Companies WHERE CompanyID=@CompanyID";
        System.Diagnostics.Debug.WriteLine("QUERY: " + query);
        System.Diagnostics.Debug.WriteLine("CONN: " + System.Configuration.ConfigurationManager.ConnectionStrings["dbcon"].ConnectionString);

        DataTable dt = dac.GetDataTable(query, param);

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
            txtGST.Text = r["GSTNumber"] != DBNull.Value ? r["GSTNumber"].ToString() : "";
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

        DateTime startDate, endDate;
        if (!DateTime.TryParse(txtStartDate.Text, out startDate) ||
            !DateTime.TryParse(txtEndDate.Text, out endDate))
        {
            lblMsg.Text = "Dates ka format sahi nahi hai!";
            return;
        }

        if (endDate < startDate)
        {
            lblMsg.Text = "End Date, Start Date se pehle nahi ho sakti!";
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
        param.Add(new SqlParameter("@GSTNumber", string.IsNullOrWhiteSpace(txtGST.Text) ? (object)DBNull.Value : txtGST.Text.Trim()));
        param.Add(new SqlParameter("@SubStart", startDate));
        param.Add(new SqlParameter("@SubEnd", endDate));

        int result;
        bool isNew = (companyID == 0);

        if (isNew)
        {
            // ID seedha INSERT se lo
            object newIdObj = dac.Scalar(@"
        INSERT INTO prabha.Companies
        (CompanyName, AdminUserName, AdminPassword, OwnerName, Phone, Email,
         Address, City, State, GSTNumber, SubscriptionStart, SubscriptionEnd,
         Status, IsActive, CreatedDate)
        OUTPUT INSERTED.CompanyID
        VALUES
        (@CompanyName, @AdminUserName, @AdminPassword, @OwnerName, @Phone, @Email,
         @Address, @City, @State, @GSTNumber, @SubStart, @SubEnd,
         N'Active', 1, GETDATE())", param);

            result = 1; // OUTPUT matlab success
            companyID = (newIdObj != null && newIdObj != DBNull.Value)
                        ? Convert.ToInt32(newIdObj)
                        : 0;
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
                    City=@City, State=@State, GSTNumber=@GSTNumber,
                    SubscriptionStart=@SubStart, SubscriptionEnd=@SubEnd
                WHERE CompanyID=@CompanyID", param);
        }

        if (result > 0)
        {
            if (isNew)
            {
                saas.LogAction(companyID, Session["User"].ToString(), "SuperAdmin",
                    "CompanyCreated", "Company", "New company '" + txtCompanyName.Text.Trim() + "' created");

                // Notification for new company
                var notifParam = new List<SqlParameter>();
                notifParam.Add(new SqlParameter("@CompanyID", companyID));
                notifParam.Add(new SqlParameter("@Title", "New Company Created"));
                notifParam.Add(new SqlParameter("@Message", "Company '" + txtCompanyName.Text.Trim() + "' has been onboarded."));
                dac.update(@"INSERT INTO prabha.Notifications
                    (CompanyID, Title, Message, NotificationType, IsRead, CreatedDate)
                    VALUES (@CompanyID, @Title, @Message, N'CompanyCreated', 0, GETDATE())", notifParam);
            }
            else
            {
                saas.LogAction(companyID, Session["User"].ToString(), "SuperAdmin",
                    "CompanyUpdated", "Company", "Company '" + txtCompanyName.Text.Trim() + "' details updated");
            }

            string msg = isNew ? "saved" : "updated";
            Response.Redirect("Dashboard.aspx?msg=" + msg);
        }
        else
        {
            lblMsg.Text = "Kuch galat hua, dobara try karein.";
        }
    }
}