using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ramount.Attributes["type"] = "number";
        ramount.Attributes["step"] = ".01";

        bamount.Attributes["type"] = "number";
        bamount.Attributes["step"] = ".01";
        
    }
    protected void ramount_TextChanged(object sender, EventArgs e)
    {
        bamount.Value = "20000";
    }
}