using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
/// <summary>
/// The Peter page implements the HTTP API.  Calls to this page returns a JSON string representation of the ResultValue object.
/// Page request parameters :
///     action  : 
///         getusers - returns a list of usernames
///         getuserroles - returns a list of roles
///     appid : the id of the application
///     username : the username (AD or other authentication name)
///     role: the name of role 
///     
///     NOTE: parameter names should be lower case but parameter values are not case sensitive.
/// </summary>
public partial class Peter : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        

        String action = (Page.Request.QueryString["action"] == null) ? "" : Page.Request.QueryString["action"].ToUpper();
        String username = (Page.Request.QueryString["username"] == null) ? "" : Page.Request.QueryString["username"].ToUpper();
        String role = (Page.Request.QueryString["role"] == null) ? "" : Page.Request.QueryString["role"].ToUpper(); ;
        String appId = (Page.Request.QueryString["appid"] == null) ? "" : Page.Request.QueryString["appid"].ToUpper();
        String responseFormat = (Page.Request.QueryString["format"] == null) ? "JSON" : Page.Request.QueryString["format"].ToUpper();
        
        QueryValue result = new QueryValue();

        if (action != null && appId != null)
        {
            if ("GETAPPS".Equals(action))
                result = DBAccess.GetApplications();
            else if("GETUSERROLES".Equals(action))
                result = DBAccess.GetUserRoles(appId, username);
            else if ("GETUSERS".Equals(action))
                result = DBAccess.GetUsers(appId);  
        }

        String response = (result.Status == 1) ? result.ToJSONString() : "";

        Response.ClearHeaders();
        Response.Write(response);
        Response.Flush();
        Response.Close();

    }
}