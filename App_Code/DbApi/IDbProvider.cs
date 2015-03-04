using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Interface for the database API
/// </summary>
public interface IDbProvider
{
    IDbConnection GetConnection();
    DataTable ExecuteReader(String sql, params object[] args);
    Object ExecuteScalar(String sql, params object[] args);
    int ExecuteNonQuery(String sql, params object[] args);
}