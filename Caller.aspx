<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Caller.aspx.cs" Inherits="Caller" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .labelcolumn
        {
            width: 90px;
            text-align: right;
            float: left;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="font-size: large; color: Gray; border-bottom: solid 2px silver; margin-bottom: 20px;">
            Testing Peter's http interface
        </div>
        <table>
            <tr>
                <td style="color: #888;">
                    Application
                </td>
                <td style="color: #888;">
                    User
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="App_DropDownList" runat="server" OnSelectedIndexChanged="App_DropDownList_SelectedIndexChanged"
                        AutoPostBack="True">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:DropDownList ID="User_DropDownList" runat="server">
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Button ID="Refresh_Button" runat="server" Text="Fetch roles" AutoPostBack="True"
                        OnClick="Refresh_Button_Click" />
                </td>
            </tr>
        </table>
        &nbsp;
        <asp:Literal ID="Result_Literal" runat="server"></asp:Literal>
        <br />
    </form>
</body>
</html>
