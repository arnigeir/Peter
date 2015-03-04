using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Web.Services;

public partial class _Default : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            BindControls();
            

    }

    private void BindControls()
    {
        
        //check if current user has permission to view this page
        String username = Page.Request.ServerVariables["AUTH_USER"]; 
        PeterWorker
        String superAdmin = System.Configuration.ConfigurationManager.AppSettings["SuperAdmin"];
        List<string> apps = DBAccess.GetApplications().Items;
        //not super => get only apps that user has persmission 

        if (username != superAdmin)
        {
            //check if user has PETER_ADMIN role in any of the registered apps
            apps = new List<string>();

            QueryValue qv = DBAccess.GetApplications();
            foreach (String appId in qv.Items)
            {
                List<string> roles = DBAccess.GetUserRoles(appId, username).Items;
                if (roles != null && roles.Find(delegate(string s) { return s == DBAccess.PeterAdminRole; }) != null)
                {
                    apps.Add(appId);
                }
            }
        }
        //hide the add/delete application buttons from non admin users

        //AddApplicationSubmit_Button.Visible = username == superAdmin;
        //DeleteApplication_Button.Visible = username == superAdmin;

        AuthenticatedUser_Literal.Text = username;

        //QueryValue appsRV = PeterWorker.GetApplications();
        Applications_DropDownList.DataSource = apps;
        Applications_DropDownList.DataBind();
    }
    //add new application
    protected void AddApplicationSubmit_Button_Click(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(AddApplication_TextBox.Text))
        {
            QueryValue apps = DBAccess.GetApplications();
            String appId = AddApplication_TextBox.Text.ToUpper();
            if (!apps.Items.Contains(appId))
            {
                DBAccess.AddApplication(appId);

                BindControls();
                Applications_DropDownList.SelectedValue = appId;
            }
        }
        AddApplication_TextBox.Text = "";

    }

    protected void DeleteApplication_Button_Click(object sender, EventArgs e)
    {
        String appId = Applications_DropDownList.SelectedValue;
        DBAccess.RemoveApplication(appId);
        BindControls();
    }

    [WebMethod]
    public static void DeleteRole(string appId, string role)
    {
        if (String.IsNullOrEmpty(appId)) throw new Exception("Application id  is null");
        if (String.IsNullOrEmpty(role)) throw new Exception("Role  is null");
        DBAccess worker = new DBAccess();
        DBAccess.RemoveRole(appId, role);

    }
    [WebMethod]
    public static void DeleteUser(string appId, string username)
    {
        if (String.IsNullOrEmpty(appId)) throw new Exception("Application id  is null");
        if (String.IsNullOrEmpty(username)) throw new Exception("Username  is null");
        DBAccess worker = new DBAccess();
        DBAccess.RemoveUser(appId, username);

    }

    [WebMethod]
    public static void AddRole(string appId, string role)
    {
        if (String.IsNullOrEmpty(appId)) throw new Exception("Application id  is null");
        if (String.IsNullOrEmpty(role)) throw new Exception("Role  is null");
        DBAccess worker = new DBAccess();
        DBAccess.AddRole(appId.ToUpper(), role.ToUpper());
    }

    [WebMethod]
    public static void AddUser(string appId, string username)
    {
        if (String.IsNullOrEmpty(appId) ) throw new Exception("Application id  is null");
        if (String.IsNullOrEmpty(username)) throw new Exception("Username  is null");

        DBAccess.AddUser(appId.ToUpper(), username.ToUpper());
    }

    [WebMethod]
    public static void UserRoleClick(string id,string value)
    {
        if (id == null) return;
        String[] l = id.Split('|');
        if (l.Length != 4) return;
        String appId = l[1];
        String username = l[2];
        String role = l[3];
        bool isChecked = value != null && "true".Equals(value.ToLower());

        DBAccess.RemoveUserRole(appId, username, role);
        if (isChecked) DBAccess.AddUserRole(appId, username, role);
    }
    [WebMethod]
    public static string LoadTable(string appId)
    {
        //if user has edit rights then display editable user-role matrix

        QueryValue usersRV = DBAccess.GetUsers(appId);
        QueryValue rolesRV = DBAccess.GetRoles(appId);

        String s = "<table id='UserRolesTable'>";
        s += "<tr>";
        s += "<th>USERNAME</th>";
        foreach (String r in rolesRV.Items)
        {
            s += (r == DBAccess.PeterAdminRole)?   
                String.Format("<th>{0}</th>", r)
                :
                String.Format("<th>{0}<img src='images/delete.png' class='DeleteColumnImg DeleteRoleAction'  id='Role|{0}'></th>", r);
        }
        s += "</tr>";
        foreach (String u in usersRV.Items)
        {


            s += "<tr><td>" + u + "</td>";
            QueryValue userRoles = DBAccess.GetUserRoles(appId, u);
            foreach (String r in rolesRV.Items)
            {
                string template = (userRoles.Items.Contains(r)) ? "<td><input id='{0}' runat='server' type='checkbox' checked='checked'/>" : "<td><input id='{0}' type='checkbox'/>";
                string id = String.Format("Check|{0}|{1}|{2}", appId, u, r);
                s += String.Format(template, id);
            }
            s += String.Format("<td><img src='images/delete.png' class='DeleteRowImg DeleteUserAction' id='User|{0}'></td>", u);
            s += "</tr>";
        }
        s += "</table>";
        return s;
    }
}