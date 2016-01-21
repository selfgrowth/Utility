/*
 源码己托管:https://github.com/v5bep7/Utility
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace System.Data.SqlClient
{
    /// <summary>
    /// SQL Server数据库访问类
    /// 本类为静态类,不可被实例化,需要使用直接调用即可
    /// </summary>
    /// <remarks>
    /// 2016/1/15 10:32:40 Created By Devin
    /// </remarks>
    public class SqlHelper
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private readonly static string _connStr;

        #region 静态构造方法

        /// <summary>
        /// 读取连接字符串,配置文件中连接字符串的键为connStr
        /// </summary>
        static SqlHelper()
        {
            var conn = ConfigurationManager.ConnectionStrings["connStr"];
            if (conn != null)
            {
                _connStr = conn.ConnectionString;
            }
        }

        #endregion

        #region 执行方法

        #region ExecuteNonQuery

        /// <summary>
        /// 执行一个非查询的T-SQL语句，返回受影响行数，如果执行的是非增、删、改操作，返回-1
        /// </summary>
        /// <param name="cmdText">要执行的T-SQL语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>受影响行数</returns>
        public static int ExecuteNonQuery(string cmdText, CommandType cmdType, params SqlParameter[] parameters)
        {
            int result = -1;
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                {
                    cmd.CommandType = cmd.CommandType;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    result = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
            return result;
        }

        /// <summary>
        /// 执行一个非查询的T-SQL语句，返回受影响行数，如果执行的是非增、删、改操作，返回-1
        /// </summary>
        /// <param name="cmdText">要执行的T-SQL语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>受影响行数</returns>
        public static int ExecuteNonQuery(string cmdText, params SqlParameter[] parameters)
        {
            return ExecuteNonQuery(cmdText, CommandType.Text, parameters);
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// 执行一个查询的T-SQL语句，返回查询结果的首行首列数据
        /// </summary>
        /// <param name="cmdText">要执行的T-SQL语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回的查询结果的首行首列数据</returns>
        public static object ExecuteScalar(string cmdText, CommandType cmdType, params SqlParameter[] parameters)
        {
            object result = null;
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, conn))
                {
                    cmd.CommandType = cmdType;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                    conn.Open();
                    result = cmd.ExecuteScalar();
                    cmd.Parameters.Clear();
                }
            }
            return result;
        }

        /// <summary>
        /// 执行一个查询的T-SQL语句，返回查询结果的首行首列数据
        /// </summary>
        /// <param name="cmdText">要执行的T-SQL语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回的查询结果的首行首列数据</returns>
        public static object ExecuteScalar(string cmdText, params SqlParameter[] parameters)
        {
            return ExecuteScalar(cmdText, CommandType.Text, parameters);
        }

        #endregion

        #region ExecuteReader

        /// <summary>
        /// 执行一个查询的查询语句,返回一个SqlDataReader对象
        /// </summary>
        /// <param name="cmdText">要执行的T-SQL语句</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回SqlDataReader对象</returns>
        public static SqlDataReader ExecuteReader(string cmdText, CommandType cmdType, params SqlParameter[] parameters)
        {
            SqlDataReader reader = null;
            SqlConnection conn = new SqlConnection(_connStr);
            using (SqlCommand cmd = new SqlCommand(cmdText, conn))
            {
                cmd.CommandType = cmdType;
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                try
                {
                    conn.Open();
                    reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    cmd.Parameters.Clear();
                }
                catch (Exception)
                {
                    conn.Close();
                    conn.Dispose();
                    throw;
                }
            }
            return reader;
        }

        /// <summary>
        /// 执行一个查询的查询语句,返回一个SqlDataReader对象
        /// </summary>
        /// <param name="cmdText">要执行的T-SQL语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>返回SqlDataReader对象</returns>
        public static SqlDataReader ExecuteReader(string cmdText, params SqlParameter[] parameters)
        {
            return ExecuteReader(cmdText, CommandType.Text, parameters);
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// 执行一个查询的T-SQL语句, 返回一个离线数据集DataSet
        /// </summary>
        /// <param name="cmdText">要执行的T-SQL参数</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>填充了查询结果的DataSet</returns>
        public static DataSet ExecuteDataSet(string cmdText, CommandType cmdType, params SqlParameter[] parameters)
        {
            DataSet result = new DataSet();
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmdText, _connStr))
            {
                adapter.SelectCommand.CommandType = cmdType;
                if (parameters != null)
                {
                    adapter.SelectCommand.Parameters.Clear();
                    adapter.SelectCommand.Parameters.AddRange(parameters);
                }
                adapter.Fill(result);
                adapter.SelectCommand.Parameters.Clear();
                return result;

            }
        }

        /// <summary>
        /// 执行一个查询的T-SQL语句, 返回一个离线数据集DataSet
        /// </summary>
        /// <param name="cmdText">要执行的T-SQL参数</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>填充了查询结果的DataSet</returns>
        public static DataSet ExecuteDataSet(string cmdText, params SqlParameter[] parameters)
        {
            return ExecuteDataSet(cmdText, CommandType.Text, parameters);
        }

        #endregion

        #endregion

        #region 公共方法

        #region 获取指定表中指定字段的最大值
        /// <summary>
        /// 获取指定表中指定字段的最大值, 确保字段为INT类型
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="fieldName">字段名</param>
        /// <returns>最大值</returns>
        public static int QueryMaxId(string tableName, string fieldName)
        {
            string sql = string.Format("select max([{0}]) from [{1}];", fieldName, tableName);
            object res = ExecuteScalar(sql);
            if (res == null)
            {
                return 0;
            }
            return Convert.ToInt32(res);
        }
        #endregion

        #region 生成查询语句

        /// <summary>
        /// 生成分页数据库的查询语句
        /// </summary>
        /// <param name="table">要查询的表</param>
        /// <param name="columns">要查询的列集合</param>
        /// <param name="page">页码.如果小于0,则默认为1</param>
        /// <param name="size">每页的条数.如果小于0,则默认为10</param>
        /// <param name="where">查询条件,不需要加where</param>
        /// <param name="orderField">排序的字段.如果为空,则抛出异常</param>
        /// <param name="isDesc">是否降序,默认为false</param>
        /// <returns></returns>
        public static string GeneratePageQuerySql(string table, string[] columns, int page, int size, string where, string orderField, bool isDesc = false)
        {
            //规范页码和每页条数
            page = page < 1 ? 1 : page;
            size = size < 1 ? 10 : size;

            //检测排序字段
            if (string.IsNullOrEmpty(orderField))
            {
                throw new ArgumentNullException("orderField");
            }

            //规范要查询的列
            string column = " * ";
            if (columns != null && columns.Any())
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    columns[i] = string.Format("[{0}]", columns[i]);
                }
                column = string.Join(" , ", columns);
            }

            //规范条件
            where = string.IsNullOrEmpty(where) ? string.Empty : " where " + where;
            //规范排序
            string sort = isDesc ? " desc " : string.Empty;

            if (page == 1) //如果是第一页的,返回该条SQL语句(效率高)
            {
                return GenerateFirstPageQuerySql(table, column, size, where, orderField, isDesc);
            }

            //定义SQL语句模板
            string sqlFormat = @"select {0} from
                                    (
                                        select ROW_NUMBER() over(order by [{1}] {2}) as num, {0} from [{3}] {4}
                                    )
                                    as tbl
                                    where
                                        tbl.num between ({5}-1)*{6} + 1 and {5}*{6};";
            return string.Format(sqlFormat, column, orderField, sort, table, where, page, size);
        }

        /// <summary>
        /// 生成查询第一页数据的T-SQL语句
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="columns">列集合</param>
        /// <param name="size">每页的条数</param>
        /// <param name="where">条件语句(忽略则传入null)</param>
        /// <param name="orderField">排序字段(即根据那个字段排序)(忽略则传入null)</param>
        /// <param name="isDesc">排序方式.默认为false</param>
        /// <returns>生成的T-SQL语句</returns>
        private static string GenerateFirstPageQuerySql(string table, string column, int size, string where, string orderField, bool isDesc = false)
        {

            //规范排序字段
            orderField = string.IsNullOrEmpty(orderField) ? string.Empty : " order by " + orderField;
            //规范排序的类型
            string sort = (isDesc && !string.IsNullOrEmpty(orderField)) ? " desc " : string.Empty;

            const string format = "select top {0} {1} from [{2}] {3} {4} {5}";
            return string.Format(format, size, column, table, where, orderField, sort);
        }

        /// <summary>
        /// 生成无分页的查询语句
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="columns">列集合</param>
        /// <param name="where">条件语句,不需要where 关键字</param>
        /// <param name="orderField">排序字段(即根据那个字段排序)(忽略则传入null)</param>
        /// <param name="isDesc">排序方式,默认为false</param>
        /// <returns>生成的T-SQL语句</returns>
        public static string GenerateQuerySql(string table, string[] columns, string where, string orderField, bool isDesc = false)
        {
            // where语句组建
            where = string.IsNullOrEmpty(where) ? string.Empty : " where " + where;
            // 查询字段拼接
            string column = columns != null && columns.Any()
                ? string.Join(" , ", (columns.Select(col => "[" + col + "]").ToArray()))
                : "*";
            const string format = "select {0} from [{1}] {2} {3} {4}";
            return string.Format(format, column, table, where,
                string.IsNullOrEmpty(orderField) ? string.Empty : "order by " + orderField,
                isDesc && !string.IsNullOrEmpty(orderField) ? "desc" : string.Empty);
        }

        #endregion

        #region 将SqlDataReader对象转换为List<Entity>对象

        /// <summary>
        /// 将SqlDataReader对象转换为实体集合
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <param name="reader">SqlDataReader</param>
        /// <returns>实体集合</returns>
        public List<TEntity> MapEntity<TEntity>(SqlDataReader reader)
            where TEntity : class,new()
        {
            //定义容器
            List<TEntity> list = null;
            //如果SqlDataReader为空,或者没有数据,直接返回null
            if (reader == null || !reader.HasRows)
            {
                return list;
            }
            //创建容器
            list = new List<TEntity>();
            //获取类型的公共属性
            var props = typeof(TEntity).GetProperties();
            //循环读取数据
            while (reader.Read())
            {
                //创建实体对象
                var entity = new TEntity();
                //遍历属性集合
                foreach (var prop in props)
                {
                    try
                    {
                        //如果属性不能写
                        if (!prop.CanWrite)
                        {
                            continue;
                        }
                        //获取当前属性的索引
                        var index = reader.GetOrdinal(prop.Name);
                        //如果当前字段不是DBNull,表示可以赋值
                        if (!reader.IsDBNull(index))
                        {
                            //获取字段对应的值
                            var value = reader.GetValue(index);
                            //给Entity对象的该属性赋值
                            prop.SetValue(entity, value, null);
                        }
                    }
                    catch (Exception)
                    {
                        //如果异常,跳过该属性
                        continue;
                    }
                }
                list.Add(entity);
            }


            return list;
        }

        #endregion

        #endregion

        #region 检测方法

        private static readonly string[] _localhost = new[] { "localhost", ".", "(local)" };

        #region 测试数据库服务器连接

        /// <summary> 
        /// 采用Socket方式，测试数据库服务器能否建立连接 
        /// </summary> 
        /// <param name="host">服务器主机名或IP</param> 
        /// <param name="port">端口号</param> 
        /// <param name="millisecondsTimeout">等待时间，单位：毫秒</param> 
        /// <returns>返回一个结果表示该地址能否建立连接</returns> 
        public static bool TestConnection(string host, int port, int millisecondsTimeout)
        {
            //判断是不是本机
            host = _localhost.Contains(host) ? "127.0.0.1" : host;
            //创建TcpClient对象
            using (var client = new TcpClient())
            {
                try
                {
                    //开始连接
                    var ar = client.BeginConnect(host, port, null, null);
                    //设置等待时间
                    ar.AsyncWaitHandle.WaitOne(millisecondsTimeout);
                    return client.Connected;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        #endregion

        #region 检测表是否存在

        /// <summary>
        /// 检测表是否存在
        /// </summary>
        /// <param name="table">要检测的表名</param>
        /// <returns>检测的结果.true表示存在,false表示不存在</returns>
        public static bool ExistsTable(string table)
        {
            string sql = "select count(1) from sysobjects where id = object_id(N'[" + table + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1";
            //string strsql = "SELECT count(*) FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[" + TableName + "]') AND type in (N'U')";
            object res = ExecuteScalar(sql);
            if (res == null)
            {
                return false;
            }
            return Convert.ToInt32(res) > 0;
        }

        #endregion

        #region 判断表是否存在某个字段

        /// <summary>
        /// 判断是否存在某张表的某个字段
        /// </summary>
        /// <param name="table">表名称</param>
        /// <param name="column">列名称</param>
        /// <returns>是否存在</returns>
        public static bool ExistsColumn(string table, string column)
        {
            //string sql = "select count(1) from syscolumns where [id]=object_id('N[" + table + "]') and [name]='" + column + "'";
            string sql =
                string.Format(
                    "select   count(1)   from   sysobjects as tbl left join syscolumns as  col  on tbl.id=col.id where  tbl.name='{0}' and col.name='{1}'"
                    , table
                    , column);
            object res = ExecuteScalar(sql);
            if (res == null)
            {
                return false;
            }
            return Convert.ToInt32(res) > 0;
        }

        #endregion

        #region 判断某张表的某个字段中是否存在某个值
        /// <summary>
        /// 判断某张表的某个字段中是否存在某个值
        /// </summary>
        /// <param name="table">表名称</param>
        /// <param name="column">列名称</param>
        /// <param name="value">要判断的值</param>
        /// <returns>是否存在</returns>
        public static bool ColumnExistsValue(string table, string column, string value)
        {
            string sql = "SELECT count(1) FROM [" + table + "] WHERE [" + column + "]=@Value;";
            object res = ExecuteScalar(sql, new SqlParameter("@Value", value));
            if (res == null)
                return false;
            return Convert.ToInt32(res) > 0;
        }
        #endregion

        #endregion

    }
}
