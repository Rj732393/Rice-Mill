
using System;

public partial class Sale : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            pMN.Attributes["type"] = "number";
            pMN.Attributes["step"] = "1";

            SaudaNo.Attributes["type"] = "text";

            SaudaDate.Attributes["type"] = "date";

            DespatchNo.Attributes["type"] = "text";
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string saudaNo = SaudaNo.Text;
        string saudaDate = SaudaDate.Text;
        string despatchNo = DespatchNo.Text;
        string pmn = pMN.Text;

        Response.Write(
            "<script>alert('Sale Saved Successfully');</script>");
    }
}