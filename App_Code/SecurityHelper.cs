using System;
using System.Web;

public static class SecurityHelper
{
    public static int CurrentRoleID
    {
        get
        {
            if (HttpContext.Current.Session["RoleID"] == null)
                return 0;

            return Convert.ToInt32(HttpContext.Current.Session["RoleID"]);
        }
    }

    public static bool HasAccess(params int[] allowedRoles)
    {
        int currentRole = CurrentRoleID;

        foreach (int role in allowedRoles)
        {
            if (currentRole == role)
                return true;
        }

        return false;
    }
}