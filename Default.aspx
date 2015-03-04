<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Peter</title>
    <link rel="icon" type="image/ico" href="favicon.ico" />
    <script src="scripts/jquery.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui.js" type="text/javascript"></script>
    <link href="scripts/jquery-ui.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">




        function onLoadTableSuccess(result) {
            $("#tableDiv").html(result);
        }
        function onLoadTableError(error) {
            alert(error.get_message());
        }

        function onAddDeleteSuccess(result) {
            loadTable();
        }
        function onAddDeleteError(error) {
            alert(error.get_message());
        }

        //refreshes the application listbox
        function loadApplicationIds() {
            //store currently selected application 
            var appId = $("#Applications_DropDownList :selected").val();
        }


        function loadTable() {
            var appId = $("#Applications_DropDownList :selected").val();
            PageMethods.LoadTable(appId, onLoadTableSuccess, onLoadTableError);
            DisplayMainPanel();
        }


        $(document).ready(function () {




            $(".DeleteUserAction").live("click", function () {
                var id = $(this).attr("id");
                var a = id.split('|');
                if (a.length != 2) {
                    alert("Userid not defined in '" + id + "'");
                }
                else {
                    var appId = $("#Applications_DropDownList :selected").val();
                    var username = a[1];
                    if (confirm("Are you sure you want to delete user '" + username + "' in application '" + appId + "'")) {
                        PageMethods.DeleteUser(appId, username, onAddDeleteSuccess, onAddDeleteError);
                    }
                }
            });

            $(".DeleteRoleAction").live("click", function () {
                var id = $(this).attr("id");
                var a = id.split('|');
                if (a.length != 2) {
                    alert("Role not defined in '" + id + "'");
                }
                else {
                    var appId = $("#Applications_DropDownList :selected").val();
                    var role = a[1];
                    if (confirm("Are you sure you want to delete role '" + role + "' in application '" + appId + "'")) {
                        PageMethods.DeleteRole(appId, role, onAddDeleteSuccess, onAddDeleteError);
                    }
                }
            });

            $(":checkbox").live("click", function () {
                PageMethods.UserRoleClick($(this).attr("id"), $(this).attr("checked"));
            });

            $("#Applications_DropDownList").change(function (event) {
                loadTable();
            });

            $("#AddUser_Button").click(function (event) {
                $("#Username_TextBox").val("");
                DisplayPanel($("#AddUserPanel"));
            });
            $("#AddRole_Button").click(function (event) {
                $("#Role_TextBox").val("");
                DisplayPanel($("#AddRolePanel"));
            });

            $("#AddApplication_Button").click(function (event) {
                DisplayPanel($("#AddApplicationPanel"));
            });


            $(".CancelButton").click(function () {
                $(".SubPanel").hide();
                $(".MainPanel").show();
            });

            //add new user
            $("#AddUserSubmit_Button").click(function () {
                var appId = $("#Applications_DropDownList :selected").val();
                var username = $("#Username_TextBox").val();
                PageMethods.AddUser(appId, username, onAddDeleteSuccess, onAddDeleteError);
            });
            //add new role
            $("#AddRoleSubmit_Button").click(function () {
                var appId = $("#Applications_DropDownList :selected").val();
                var role = $("#Role_TextBox").val();
                PageMethods.AddRole(appId, role, onAddDeleteSuccess, onAddDeleteError);
            });


            $(".MenuPanel").hide();
            $(".MenuButton").click(function () {
                var panelid = '#' + $(this).attr('data-panel');
                $(".MenuPanel").hide();
                if ($(panelid).is(':visible')) {
                    $(panelid).hide();
                }
                else {
                    var offset = $(TaskBar).offset();
                    var height = $(TaskBar).height();
                    $(panelid).css("top", offset.top + height - 1);
                    $(panelid).css("left", offset.left);
                    $(panelid).show();
                }
            });
            $(".DialogClose").click(function () {
                $(".MenuPanel").hide();
            });


            loadTable();
        });

    
    </script>
    <style type="text/css">
        .AppRolesPanel
        {
            margin-top: 5px;
        }
        .DeleteRowImg
        {
            vertical-align: top;
            margin-left: 5px;
            cursor: pointer;
        }
        .DeleteColumnImg
        {
            vertical-align: top;
            margin-left: 5px;
            cursor: pointer;
        }
        .MainPanel
        {
            margin-top: 5px;
            padding: 5px;
        }
        .SubPanel
        {
            display: none;
        }
        .DialogTitle
        {
            font-size: larger;
            font-weight: bold;
            margin: 10px 0 5px 0;
            color: Gray;
        }
        .DialogHelpMessage
        {
            font-size: small;
            color: Gray;
            padding: 5px;
        }
        
        /* table styles */
        table
        {
            border-collapse: collapse;
        }
        table, th, td
        {
            border: 1px solid silver;
            padding: 2px;
        }
        th
        {
            background-color: #efefef;
        }
        .AuthenticatedUser
        {
            position: absolute;
            top: 0px;
            right: 0px;
            z-index: 10;
            padding: 2px 5px 0px 0px;
            color: Gray;
        }
        
        /* *********************** */
        .MenuPanel
        {
            position: absolute;
            border: solid 1px gray;
            box-shadow: 3px 3px 5px #888;
            width: 300px;
            padding:0px;
            background-color: White;
            
        }
        
        .MenuButton
        {
            border: none;
            background-color: #efefef;
            border:solid 1px #efefef;
            margin-left: 0px;
            margin-right: 0px;
            cursor: pointer;
            color: gray;

        }
        .MenuButton:hover
        {
            color: #006699;
        }
        .MenuPanelHeader
        {
            margin: 0px 0px 5px 0px;
            padding: 1px 0px 1px 5px;
            background-color: #efefef;
        }
        .MenuPanelBody
        {
            padding: 5px 5px 10px 5px;
        }
        
        .DialogButton
        {
            border: none;
            background-color: transparent;
            margin: 0px;
            cursor: pointer;
            color: navy;
            
        }
        .DialogButton:hover
        {
            background-color: #efefef;
            color: black;
        }
        .Label
        {
            color:#888;
            margin-right:3px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server">
    </asp:ScriptManager>
    <div id="AuthenticatedUser" class="AuthenticatedUser">
        <asp:Literal ID="AuthenticatedUser_Literal" runat="server"></asp:Literal>
    </div>
    <div style="min-width: 400px;">
        <div id="TaskBar">
            <input type="button" value="Add application" data-panel="NewApplicationDialog" class="MenuButton">
            <input type="button" value="Add user" data-panel="NewUserDialog" id="NewUser" class="MenuButton">
            <input type="button" value="Add role" data-panel="NewRoleDialog" id="NewRole" class="MenuButton">
        </div>
        <div id="AppRolesPanel" class="MainPanel">

            <div class="AppSelectionPanel">
                <span class="Label">Application: </span>
                <asp:DropDownList ID="Applications_DropDownList" runat="server">
                </asp:DropDownList>
                <asp:Button ID="DeleteApplication_Button" runat="server" OnClientClick="if(!confirm('Do want to delete this application ?')){ return false;}"
                    Text="Delete this application" OnClick="DeleteApplication_Button_Click" class="DialogButton" />
            </div>
            <!-- [username,role1,role2,...,roleN,[delete]] -->
            <div id="tableDiv" style="margin-top:10px;">
            </div>
        </div>
        <div id="NewUserDialog" class="MenuPanel">
            <div class="MenuPanelHeader">
                Add a new user</div>
            <div class="MenuPanelBody" >
                <span class="Label">Username:</span>
                <input type="text" id="Username_TextBox" />
            </div>
            <div style="text-align: right;">
                <input type="button" value="Cancel" class="DialogClose DialogButton">
                <input type="button" id="AddUserSubmit_Button" value="OK" class="DialogClose DialogButton">
            </div>
        </div>
        <div id="NewRoleDialog" class="MenuPanel">
            <div class="MenuPanelHeader">
                Add a new role</div>
            <div class="MenuPanelBody" >
                <span class="Label">Role:</span>
                <input type="text" id="Role_TextBox" />
            </div>
            <div style="text-align: right;">
                <input type="button" value="Cancel" class="DialogClose DialogButton">
                <input type="button" value="OK" id="AddRoleSubmit_Button" class="DialogClose DialogButton">
            </div>
        </div>
        <div id="NewApplicationDialog" class="MenuPanel">
            <div class="MenuPanelHeader">
                Add a new application</div>
            <div class="MenuPanelBody" >
                <span class="Label">Application id:</span>
                <asp:TextBox ID="AddApplication_TextBox" runat="server"></asp:TextBox>
            </div>
            <div style="text-align: right;">
                <input type="button" value="Cancel" class="DialogClose DialogButton">
                <asp:Button ID="AddApplicationSubmit_Button" runat="server" 
                    Text="Ok" OnClick="AddApplicationSubmit_Button_Click" CssClass="DialogClose DialogButton" />
            </div>
        </div>
    </div>
    </form>
</body>
</html>
