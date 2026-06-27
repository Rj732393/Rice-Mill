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
        string saudaNo = SaudaNo.Text.Trim();
        string saudaDate = SaudaDate.Text.Trim();
        string despatchNo = DespatchNo.Text.Trim();
        string pmn = pMN.Text.Trim();

        // Server-side validation -- mirrors validateSale() in Sale.aspx
        // so frontend aur backend rules kabhi mismatch na ho.
        DateTime parsedDate;
        int pmnValue;
        string errorMsg = ValidateSaleEntry(saudaNo, saudaDate, despatchNo, pmn, out parsedDate, out pmnValue);

        if (!string.IsNullOrEmpty(errorMsg))
        {
            // Error ab popup alert() mein nahi -- same topAlert box mein dikhega (red)
            ClientScript.RegisterStartupScript(
                this.GetType(),
                "saveError",
                "$(document).ready(function(){ showAlert('" + errorMsg.Replace("'", "\\'") + "', 'error'); });",
                true);
            return;
        }

        // Sab validation pass -- ab safely save karo.
        // TODO: Apna DB insert/update logic yahan likhein
        // (saudaNo, parsedDate, despatchNo, pmnValue use karein)

        // Success message bhi topAlert box mein hi dikhega (green) -- popup alert() nahi
        ClientScript.RegisterStartupScript(
            this.GetType(),
            "saveSuccess",
            "$(document).ready(function(){ showAlert('Sale Saved Successfully', 'success'); });",
            true);
    }

    /// <summary>
    /// Same rules as JS validateSale() -- required fields, valid date,
    /// numeric positive PMN. Returns null when everything is valid.
    /// </summary>
    private string ValidateSaleEntry(
        string saudaNo,
        string saudaDate,
        string despatchNo,
        string pmn,
        out DateTime parsedDate,
        out int pmnValue)
    {
        parsedDate = DateTime.MinValue;
        pmnValue = 0;

        if (string.IsNullOrEmpty(saudaNo))
            return "Please enter Sauda No.";

        if (string.IsNullOrEmpty(saudaDate))
            return "Please select Sauda Date.";

        if (!DateTime.TryParse(saudaDate, out parsedDate))
            return "Please enter a valid Sauda Date.";

        if (string.IsNullOrEmpty(despatchNo))
            return "Please enter Despatch No.";

        if (string.IsNullOrEmpty(pmn))
            return "Please enter PMN.";

        if (!int.TryParse(pmn, out pmnValue))
            return "PMN must be a valid number.";

        if (pmnValue <= 0)
            return "PMN must be greater than zero.";

        return null; // koi error nahi
    }
}
