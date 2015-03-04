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
using System.Data;
using System.Web;
using System.Data.OracleClient;
using System.Collections.Generic;
using System.Data.Common;

    /// <summary>
    /// Misc utility methods for accessing a ORACLE database
    /// </summary>
    public class OracleDbProvider: IDbProvider
    {
        public  IDbConnection GetConnection()
        {
            String connStr = System.Configuration.ConfigurationManager.AppSettings["DatabaseConnectionString"];
            return new System.Data.OracleClient.OracleConnection(connStr); //"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521)))(CONNECT_DATA=(SERVICE_NAME=)));User ID=PETER;Password=PETER");
        }
        private  DataTable ExecuteReader(String sql)
        {
            DataTable table = new DataTable();
            using (System.Data.OracleClient.OracleConnection conn = (GetConnection() as OracleConnection))
            {
                conn.Open();               
                System.Data.OracleClient.OracleCommand selectCmd = conn.CreateCommand();

                selectCmd.CommandText = sql;

                selectCmd.CommandType = CommandType.Text;
                System.Data.OracleClient.OracleDataReader reader = selectCmd.ExecuteReader();                
                table.Load(reader, LoadOption.OverwriteChanges);
            }

            return table;
        }
        private  Object ExecuteScalar(String sql)
        {
            Object o = null;
            using (System.Data.OracleClient.OracleConnection conn = (GetConnection() as OracleConnection))
            {
                conn.Open();
                System.Data.OracleClient.OracleCommand selectCmd = conn.CreateCommand();
                selectCmd.CommandText = sql;
                selectCmd.CommandType = CommandType.Text;
                o = selectCmd.ExecuteScalar();
            }

            return o;
        }
        private  int ExecuteNonQuery(String sql)
        {
            int res = 0;

            DataTable table = new DataTable();
            using (System.Data.OracleClient.OracleConnection conn = (GetConnection() as OracleConnection))
            {
                conn.Open();
                

                System.Data.OracleClient.OracleCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                
                res = cmd.ExecuteNonQuery();
            }
            return res;
        }
        private  int ExecuteNonQuery(IDbConnection conn, IDbTransaction transaction, String sql)
        {
            DataTable table = new DataTable();
            System.Data.OracleClient.OracleConnection oraConnection = conn as OracleConnection;
            System.Data.OracleClient.OracleCommand cmd = oraConnection.CreateCommand();
            cmd.Transaction = transaction as OracleTransaction;
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;

            return cmd.ExecuteNonQuery();
        }
        private   Object ExecuteScalar(IDbConnection conn, IDbTransaction transaction, String sql)
        {
            System.Data.OracleClient.OracleConnection oraConnection = conn as OracleConnection;
            System.Data.OracleClient.OracleCommand selectCmd = oraConnection.CreateCommand();
            selectCmd.Transaction = transaction as OracleTransaction;
            selectCmd.CommandText = sql;
            selectCmd.CommandType = CommandType.Text;
            return selectCmd.ExecuteScalar();
        }
        private static DataTable ExecuteReader(IDbConnection conn, IDbTransaction transaction, String sql)
        {
            DataTable table = new DataTable();
            System.Data.OracleClient.OracleConnection oraConnection = conn as OracleConnection;
            System.Data.OracleClient.OracleCommand selectCmd = oraConnection.CreateCommand();
            selectCmd.Transaction = transaction as OracleTransaction;
            selectCmd.CommandText = sql;
            selectCmd.CommandType = CommandType.Text;
            System.Data.OracleClient.OracleDataReader reader = selectCmd.ExecuteReader();
            table.Load(reader, LoadOption.OverwriteChanges);
            return table;
        }

        #region Non-transaction based methods
        public  DataTable ExecuteReader(String sql, params object[] args)
        {
            sql = (args == null) ? sql : String.Format(sql, args);
            return ExecuteReader(sql);
        }
        public  Object ExecuteScalar(String sql, params object[] args)
        {
            sql = (args == null) ? sql : String.Format(sql, args);
            return ExecuteScalar(sql);
        }
        public  int ExecuteNonQuery(String sql, params object[] args)
        {
            sql = (args == null) ? sql : String.Format(sql, args);
            return ExecuteNonQuery(sql);

        }
        #endregion

        #region Transaction based methods
        public  int ExecuteNonQuery(IDbConnection conn, IDbTransaction transaction, String sql, params object[] args)
        {
            sql = (args == null) ? sql : String.Format(sql, args);
            return ExecuteNonQuery(conn,transaction,sql);
        }
        /// <summary>
        /// Executes a scalar command - ie a command that returns a single row object
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="transaction"></param>
        /// <param name="command">A parametrized SQL command</param>
        /// <param name="args">An array of objects</param>
        /// <returns></returns>
        public  Object ExecuteScalar(IDbConnection conn, IDbTransaction transaction, String sql, params object[] args)
        {
            sql = (args == null) ? sql : String.Format(sql, args);
            return ExecuteScalar(conn, transaction, sql);
        }
        /// <summary>
        /// Executes a select query and returns a datatable.
        /// </summary>
        public  DataTable ExecuteReader(IDbConnection conn, IDbTransaction transaction, String sql, params object[] args)
        {
            sql = (args == null) ? sql : String.Format(sql, args);
            return ExecuteReader(conn, transaction, sql);
        }
        #endregion
    }

