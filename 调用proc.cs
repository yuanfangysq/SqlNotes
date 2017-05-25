using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ajax_分页.sqlhelp
{
    public class SqlHelper
    {
        private static string connStr = "Data Source=.;Initial Catalog=text;Integrated Security=True";

        public static DataTable ExecuteProcPageList(int pageSize, int currentPage, out int rowCount, out int pageCount)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "proc_location_paging"; //存储过程的名字  
                    cmd.CommandType = CommandType.StoredProcedure; //设置命令为存储过程类型(即：指明我们执行的是一个存储过程)  

                    rowCount = 0;
                    pageCount = 0;


                    SqlParameter[] parameters ={  
                             new SqlParameter("@pageSize",pageSize),  
                             new SqlParameter("@currentpage",currentPage),  
                             new SqlParameter("@rowCount",rowCount),  
                             new SqlParameter("@pageCount",pageCount)  
                                                 
                                               };

                    //如果是T-sql防止SQL注入漏洞攻击参数查询
                    //cmd.CommandText = "select * from t_user where name=@us andpassword=@pw";
                    //cmd.Parameters.Add(new SqlParameter("@us", "王二小"));
                    //cmd.Parameters.Add(new SqlParameter("@pw", 123456));
                    //SqlDataReader dr = cmd.ExecuteReader();

      
                    
                    //因为在存储过程中@rowCount 与@pageCount 是一个输出参数(output), 而parameters这个数组里，第三，和第四个参数就是要用来替换掉这两个输出参数的，所以这里要将parameters这个数组里的这两个参数设为输出参数。  
                    parameters[2].Direction = ParameterDirection.Output;
                    parameters[3].Direction = ParameterDirection.Output;

                    cmd.Parameters.AddRange(parameters); //将参数传递给我们的cmd命令对象  


                   
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);//到数据库去执行存储过程，并将结果填充到dt表中  
                    }

                  
                    rowCount = Convert.ToInt32(parameters[2].Value);
                    pageCount = Convert.ToInt32(parameters[3].Value);

                    return dt;

                }
            }

        }
    }


}