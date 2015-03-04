using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SQLite;

/// <summary>
/// Summary description for SqliteDb
/// </summary>
public class SqliteDbProvider : IDbProvider 
{

    public SqliteDbProvider()
    {

    }


    public IDbConnection GetConnection()
    {
        String connStr = System.Configuration.ConfigurationManager.AppSettings["DatabaseConnectionString"];
        return new SQLiteConnection(connStr); //"Data Source=//App_Data/test.db;Version=3");
    }

    private DataTable ExecuteReader(String sql)
    {
        DataTable table = new DataTable();
        using (SQLiteConnection conn = (GetConnection() as SQLiteConnection))
        {
            conn.Open();
            SQLiteCommand selectCmd = conn.CreateCommand();
            selectCmd.CommandText = sql;
            selectCmd.CommandType = CommandType.Text;
            SQLiteDataReader reader = selectCmd.ExecuteReader();
            table.Load(reader, LoadOption.OverwriteChanges);
        }

        return table;
    }
    private Object ExecuteScalar(String sql)
    {
        Object o = null;
        using (SQLiteConnection conn = (GetConnection() as SQLiteConnection))
        {
            conn.Open();
            SQLiteCommand selectCmd = conn.CreateCommand();
            selectCmd.CommandText = sql;
            selectCmd.CommandType = CommandType.Text;
            o = selectCmd.ExecuteScalar();
        }

        return o;
    }
    private int ExecuteNonQuery(String sql)
    {
        int res = 0;

        DataTable table = new DataTable();
        using (SQLiteConnection conn = (GetConnection() as SQLiteConnection))
        {
            conn.Open();
            SQLiteCommand cmd = conn.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            res = cmd.ExecuteNonQuery();
        }
        return res;
    }


    public DataTable ExecuteReader(String sql, params object[] args)
    {
        sql = (args == null) ? sql : String.Format(sql, args);
        return ExecuteReader(sql);
    }
    public Object ExecuteScalar(String sql, params object[] args)
    {
        sql = (args == null) ? sql : String.Format(sql, args);
        return ExecuteScalar(sql);
    }
    public int ExecuteNonQuery(String sql, params object[] args)
    {
        sql = (args == null) ? sql : String.Format(sql, args);
        return ExecuteNonQuery(sql);

    }


}