using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using System.Configuration;

public partial class Caller : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Result_Literal.Text = "";
        if (!IsPostBack)
        {
            App_DropDownList.DataSource = HttpApi.GetApplications();
            App_DropDownList.DataBind();
            User_DropDownList.DataSource = HttpApi.GetUsers(this.App_DropDownList.SelectedValue);
            User_DropDownList.DataBind();
        }
    }

    private void UpdateRoles()
    {
        this.Result_Literal.Text = "";
        List<String> items = HttpApi.GetUserRoles(this.App_DropDownList.SelectedValue, this.User_DropDownList.SelectedValue);
        foreach (String item in items)
            this.Result_Literal.Text += item + ",";
        
        this.Result_Literal.Text = (items.Count == 0)? "Does not have any roles" : "Has the roles: "+this.Result_Literal.Text.TrimEnd(',');
    }


    protected void App_DropDownList_SelectedIndexChanged(object sender, EventArgs e)
    {
        User_DropDownList.DataSource = HttpApi.GetUsers(this.App_DropDownList.SelectedValue);
        User_DropDownList.DataBind();
        UpdateRoles();
    }


    protected void Refresh_Button_Click(object sender, EventArgs e)
    {
        UpdateRoles();
    }
}