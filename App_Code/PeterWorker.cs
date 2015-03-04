/*
Copyright (c) 2011 , Arni G Sigurdsson  (arni.geir.sigurdsson@gmail.is)

All rights reserved.

Redistribution of source or binary forms of software, 
with or without modification, is not permitted.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using fastJSON;



/// <summary>
/// Summary description for PeterWorker
/// </summary>
public class DBAccess
{
    public static string PeterAdminRole = "SYS_ADMIN";
    //SQLite syntax
    private static string createTableSql = @"CREATE TABLE IF NOT EXISTS PETER  ( 
                                        APPID   	VARCHAR2(256) NULL,
                                        USERNAME	VARCHAR2(256) NULL,
                                        ROLENAME	VARCHAR2(256) NULL)";
 
    private static string insertSql = "INSERT INTO PETER(APPID,USERNAME,ROLENAME) VALUES('{0}','{1}','{2}')";
    private static string deleteApplicationSql = "DELETE FROM PETER WHERE APPID = '{0}'";
    private static string deleteRoleSql = "DELETE FROM PETER WHERE APPID='{0}' AND ROLENAME='{1}'";
    private static string deleteUserSql = "DELETE FROM PETER WHERE APPID='{0}' AND USERNAME='{1}'";
    private static string deleteUserRoleSql = "DELETE FROM PETER WHERE APPID='{0}' AND ROLENAME='{2}' AND USERNAME='{1}'";
    private static string selectApplicationsSql = "SELECT DISTINCT APPID FROM PETER ORDER BY APPID";
    private static string selectApplicationUsersSql = "SELECT DISTINCT USERNAME FROM PETER WHERE APPID='{0}' ORDER BY USERNAME";
    private static string selectApplicationRolesSql = "SELECT DISTINCT ROLENAME FROM PETER WHERE APPID='{0}' ORDER BY ROLENAME";
    private static string selectApplicationUserRolesSql = "SELECT DISTINCT ROLENAME FROM PETER WHERE APPID='{0}' AND USERNAME='{1}' ORDER BY ROLENAME";


    private static QueryValue ExecuteQuery(string sql)
    {
        QueryValue value = new QueryValue();
        String s;
        try
        {
            //if exist then do nothing
            DataTable table = DbProviderFactory.Get().ExecuteReader(sql);
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    if (row[0] == DBNull.Value) continue;
                    s = Convert.ToString(row[0]);
                    if (String.IsNullOrEmpty(s)) continue;
                    value.Items.Add(s);
                }
            }
        }
        catch (Exception ex)
        {
            value.Status = 0;
            value.Message = ex.Message;
            return value;
        }

        return value;
    }



    private static QueryValue ExecuteNonQuery(string sql)
    {
        QueryValue value = new QueryValue();
        try
        {
            value.Status = DbProviderFactory.Get().ExecuteNonQuery(sql);
        }
        catch (Exception ex)
        {
            value.Status = 0;
            value.Message = ex.Message;
            return value;
        }

        return value;
    }

    //r
    public static QueryValue InitializeDatabase()
    {
        return ExecuteNonQuery(createTableSql);
    }

    public static QueryValue GetApplications()
    {
        return ExecuteQuery(selectApplicationsSql);
    }
    //add application and default system administrator role to it
    public static QueryValue AddApplication(string appId)
    {
        QueryValue qv = ExecuteNonQuery(String.Format(insertSql, appId, null, null));
        if (qv.Status != 0)
        {
            //add default admin role to the application
            String username = HttpContext.Current.Request.ServerVariables["AUTH_USER"]; //Finding with name
            if(!String.IsNullOrEmpty(username)) 
                qv = AddUserRole(appId, username.ToUpper(), PeterAdminRole);
        }
        return qv;
    }

    public static QueryValue RemoveApplication(string appId)
    {
        return ExecuteNonQuery(String.Format(deleteApplicationSql, appId));
    }


    public static QueryValue GetRoles(string appId)
    {
        return ExecuteQuery(String.Format(selectApplicationRolesSql,appId));
    }

    public static QueryValue GetUsers(string appId)
    {
        return ExecuteQuery(String.Format(selectApplicationUsersSql,appId));
    }

    public static QueryValue GetUserRoles(string appId, string username)
    {
        return ExecuteQuery(String.Format(selectApplicationUserRolesSql,appId,username));
    }

    public static QueryValue AddRole(string appId, string role)
    {
        return ExecuteNonQuery(String.Format(insertSql,appId,null,role));
    }

    public static QueryValue RemoveRole(string appId, string role)
    {
        return ExecuteNonQuery(String.Format(deleteRoleSql, appId, role));
    }


    public static QueryValue AddUserRole(string appId, string username, string role)
    {
        return ExecuteNonQuery(String.Format(insertSql, appId, username, role));
    }

    public static QueryValue RemoveUserRole(string appId, string username, string role)
    {
        return ExecuteNonQuery(String.Format(deleteUserRoleSql, appId, username, role));
    }

    public static QueryValue AddUser(string appId, string username)
    {
        return ExecuteNonQuery(String.Format(insertSql, appId, username,""));
    }

    public static QueryValue RemoveUser(string appId, string username)
    {
        return ExecuteNonQuery(String.Format(deleteUserSql, appId, username));
    }
}