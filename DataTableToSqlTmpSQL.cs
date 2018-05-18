using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace help
{
    class SqlHelep
    {
        private static string connStr = "Data Source=.;Initial Catalog=text;Integrated Security=True";
        /// <summary>  
        /// 对连接执行 Transact-SQL 语句并返回受影响的行数  
        /// </summary>  
        /// <param name="sql">对数据库表进行Insert，Update和Delete语句</param>  
        /// <param name="parameters">sql语句参数</param>  
        /// <returns>返回值为该命令所影响的行数</returns>  
        public static int ExecuteNonQuery(CommandType cmdTye, string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = cmdTye;

                    if (parameters!=null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }
                   

                    int i = cmd.ExecuteNonQuery();

                    return i;


                }
            }
        }


        /// <summary>  
        /// DataSet是将查询结果填充到本地内存中，这样即使与连接断开，服务器的连接断开，都不会影响数据的读取。但是它也有一个坏处，就是只有小数据量才能往里面存放，数据量大了就给你的本地内存冲爆了。电脑会卡死去。大数据量的话还得用SqlDataReader    
        /// </summary>  
        /// <param name="sql">要执行的sql语句</param>  
        /// <param name="parameters">sql语句的参数</param>  
        /// <returns>返回值是一个DataSet</returns>  
        public static DataSet ExecuteDataSet(CommandType cmdTye, string sql, params SqlParameter[] parameters)
        
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = cmdTye;

                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    DataSet ds = new DataSet();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                       
                        //将cmd的执行结果填充到ds里面  
                        adapter.Fill(ds);
                        return ds;
                    }

                }
            }
        }

        /// <summary>
        /// 转换成sql语句
        /// </summary>
        /// <param name="da">原数据表</param>
        /// <param name="TbTemb">数据库临时表</param>
        /// <returns>返回sql语句</returns>
        public static string DataTableToSqlTmpSQL(DataTable da, string TbTemb)
        {
            StringBuilder sb = new StringBuilder();
            bool isComplete = false;
            string Columns= "";
            string sql="";

            if (da == null || da.Rows.Count <= 0)
            {
                return "";
            }

            #region --创建临时表
            sb.AppendLine(" CREATE  TABLE " + TbTemb +" (");

            foreach (DataColumn item in da.Columns)
            {
                string type= Gettype(item);

                if (string.IsNullOrWhiteSpace(type))
                {
                    isComplete = true;
                    break;
                }

                sb.AppendLine(item.ColumnName + " " + type +",");

                Columns += ","+ item.ColumnName;
            }


            sb.AppendLine("  ) ");

            #endregion

            sb.AppendLine();

            #region --插入数据

            string Value="";
            //插入数据
             if (!isComplete)
             {

                 foreach (DataRow item in da.Rows)
                 {
                     object[] ItemArray = item.ItemArray;

                     for (int i = 0; i < ItemArray.Length; i++)
                     {
                         if (ItemArray[i].GetType().FullName == "System.Boolean")
                         {
                             int istrue = Convert.ToBoolean(ItemArray[i]) ? 1 : 0;
                             Value += "," + istrue.ToString();
                         }
                         else
                         {
                             if (Isint(ItemArray[i]))
                             {

                                 Value += "," + ItemArray[i].ToString();
                             }
                             else
                             {
                                 Value += ",'" + ItemArray[i].ToString() + " '";
                             }
                         }
                     }

                     sb.AppendLine("INSERT INTO " + TbTemb + " (" + Columns.Remove(0,1) + ") ");
                     sb.AppendLine(" SELECT " + Value.Remove(0,1));
                     Value = "";
                 }
             }

            #endregion



             if (!isComplete)
             {
                 sql = sb.ToString();               
             }

             return sql;
        }

        static Dictionary<string, string> dic = new Dictionary<string, string>()
        {
             {"System.Int32","INT"},
             {"System.Int16","INT"},
             {"System.String","VARCHAR(MAX)"},
             {"System.string","VARCHAR(MAX)"},
             {"System.char","VARCHAR(MAX)"},
             
             {"System.Boolean","BIT"},
             {"System.DateTime","Datetime"},
             {"System.Decimal","DECIMAL(10,4)"},
             {"System.Double","DECIMAL(10,4)"},
             {"float","DECIMAL(10,4)"},
             

        };
        public static string Gettype(DataColumn item)
        {
            string itemType = item.DataType.FullName;

            string str = "";
            try
            {
                str = dic[itemType];
            }
            catch (Exception)
            {

                throw;
            }
            return str;

        }

        private static bool Isint(object isint)
        {
            bool isbool = true;
            try
            {
               int a= Convert.ToInt32(isint);
            }
            catch (Exception)
            {
                isbool = false;
            }

            return isbool;
        }



    }
}
