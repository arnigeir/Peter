using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Factorizes database access objects
/// </summary>
public static class DbProviderFactory
{
    
    private static IDbProvider sqlliteInstance = new SqliteDbProvider();
    private static IDbProvider oracleInstance = new OracleDbProvider();
    private static IDbProvider mssqlInstance = new MsSqlDbProvider();
    public static class Names
    {
        public const String OracleProvider = "oracle";
        public const String SqLiteProvider = "sqlite";
        public const String MsSqlProvider = "mssql";
    }

    /// <summary>
    /// Gets the provider specified in the application key parameter "DbProviderName" in web.config
    /// </summary>
    /// <returns></returns>
    public static  IDbProvider Get()
    {
        String providerName = System.Configuration.ConfigurationManager.AppSettings["DbProviderName"];
        return Get(providerName);
    }

    /// <summary>
    /// Gets the named provider
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public static  IDbProvider Get(String name)
    {
        IDbProvider provider = null;
        switch (name){
            case DbProviderFactory.Names.SqLiteProvider:
                provider = sqlliteInstance;
                break;
            case DbProviderFactory.Names.OracleProvider:
                provider = oracleInstance;
                break;
            case DbProviderFactory.Names.MsSqlProvider:
                provider = mssqlInstance;
                break;
        }
        return provider;
    }
}