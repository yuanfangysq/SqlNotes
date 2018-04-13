using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Regent.DB;
using Regentsoft.Plugin.Report.Dal;
using Regent.Business;
//using Regent.Business;

namespace Regentsoft.Plugin.Report.Dal
{
    /// <summary>
    /// 文件名称：
    /// 功能摘要：看板报表
    /// 编写日期：2018/4/10
    /// 使用说明：请参照各个方法函数的定义
    /// 修改记录：
    /// 功能            日期            修改人
    /// </summary>
    public class RptCheckMb
    {
        #region 属性
        private DataSet _DsCondition;
        /// <summary>
        /// 过滤条件和输出项目
        /// </summary>
        public DataSet DsCondition
        {
            set { _DsCondition = value; }
        }

        private string _UserNo;
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserNo
        {
            set { _UserNo = value; }
        }
        #endregion 属性

        #region 初始化设定

        StringBuilder sb = new StringBuilder();
        string tbCustomerFilter;
        DataAccess da;
        string yyyy = "", MM = "", datebegin = "", dateend = "";
        string Smonthbegin = "", Smonthend = "", Smonth = "";
        string Syearbegin = "", Syearend = "", Syear = "";

        string DaysInMonth;
        DataTable ZhuanguiTable;
        DataTable DaixiaoTable;
        DataTable ZhiyingTable;
        DataTable Zong;

        string ZhuanguiTable2 = Generic.GetTmpTableName();
        string DaixiaoTable2 = Generic.GetTmpTableName();

        string resultTable = Generic.GetTmpTableName();
        /// <summary>
        /// 数据初始化
        /// </summary>
        private void InitData()
        {
            da = new DataAccess();
            da.ManualConnection = true;
            da.Connection = da.NewConnection();
            if (da.Connection.State == ConnectionState.Closed)
            {
                da.Open();
            }

            #region 得到店铺过滤条件
            if (_DsCondition.Tables.Contains("CustomerFilter"))
            {
                tbCustomerFilter = Generic.GetCustomerFilterByRight(da, _DsCondition.Tables["CustomerFilter"], _UserNo,true);
            }
            else
            {
                tbCustomerFilter = Generic.GetCustomerFilterByRight(da, null, _UserNo,true);
            }
            da.ExecuteNonQuery(string.Format("CREATE NONCLUSTERED INDEX [ID] ON [{0}](ID ASC) ON [PRIMARY]", tbCustomerFilter));
            #endregion 得到店铺过滤条件



           DataRow[]   drs = _DsCondition.Tables["OtherFilter"].Select("ID = 'yyyy'");
            if (drs.Length > 0)
            {
                yyyy = drs[0]["Data"].ToString();
                Syear = (Convert.ToInt32(drs[0]["Data"]) - 1).ToString();
              
            }
            drs = _DsCondition.Tables["OtherFilter"].Select("ID = 'MM'");
            if (drs.Length > 0)
            {
                MM = drs[0]["Data"].ToString();
                Smonth = (Convert.ToInt32(drs[0]["Data"]) - 1).ToString();
            }

            //这个月的开始时间和结束时间
            DaysInMonth = DateTime.DaysInMonth(Convert.ToInt32(yyyy), Convert.ToInt32(MM)).ToString();
            datebegin = yyyy + "-" + MM + "-" + "1"; dateend = yyyy + "-" + MM + "-" + DaysInMonth;

            //上个月的开始时间和结束时间
            Smonthbegin = yyyy + "-" + Smonth + "-" + "1"; Smonthend = yyyy + "-" + Smonth + "-" + DateTime.DaysInMonth(Convert.ToInt32(yyyy), Convert.ToInt32(Smonth)).ToString();

            //上年的开始时间和结束时间
            Syearbegin = Syear + "-" + MM + "-" + "1"; Syearend = Syear + "-" + MM + "-" + DateTime.DaysInMonth(Convert.ToInt32(Syear), Convert.ToInt32(MM)).ToString();

        }

        #endregion 初始化设定


        #region 统计处理

        //得到报表数据
        public DataSet Calculate()
        {
            InitData();

            return pssOpenData();
        }

        //统计方法
        private DataSet pssOpenData()
        {
            //创建临时表
            string tbDelivery = CreateTempTable_Customer();
            //dlg.Caption = "汇总输出";
            DataSet dsReportData = null;
            dsReportData = GetResultData(tbDelivery);


            da.Close();
            da.ManualConnection = false;

            dsReportData.AcceptChanges();
            return dsReportData;

        }

        private string CreateTempTable_Customer()
        {
            #region --分开处理数据
            // 代销临时表
            da.ExecuteNonQuery(
            string.Format("select B.customer_id as id , '{1}' + B.Abbrev as name  into [{2}] from [{0}] A INNER JOIN Customer B ON A.ID = B.customer_id"
                        + " WHERE B.DealInPattern = '代销'"
                         , tbCustomerFilter, "代销$", DaixiaoTable2)
                                                );
            //专柜临时表
            da.ExecuteNonQuery(
            string.Format("select B.customer_id as id , '{1}' + B.Abbrev as name into [{2}]  from [{0}] A INNER JOIN Customer B ON A.ID = B.customer_id"
                  + " WHERE B.DealInPattern = '专柜'"
                   , tbCustomerFilter, "专柜$", ZhuanguiTable2)
           );

            int rowcount = int.Parse(da.ExecuteQueryDataTable(
            string.Format("select count(B.customer_id) as [rowcount]  from {0}  A INNER JOIN Customer B ON A.ID = B.customer_id"
                             + " WHERE B.DealInPattern in ('专柜','代销','自营')", tbCustomerFilter
                     )
           ).Rows[0]["rowcount"].ToString());

            // ----------------------  ------------------------ //
            StringBuilder sb = new StringBuilder();
            string sql = "";
            sb.Length = 0;

            DaixiaoTable = da.ExecuteQueryDataTable(
                string.Format("select B.customer_id as id , '{1}' + B.Abbrev as name  from [{0}] A INNER JOIN Customer B ON A.ID = B.customer_id"
                            + " WHERE B.DealInPattern = '代销'"
                             , tbCustomerFilter, "代销$")
                                                    );


            ZhuanguiTable = da.ExecuteQueryDataTable(
                       string.Format("select B.customer_id as id , '{1}' + B.Abbrev as name  from [{0}] A INNER JOIN Customer B ON A.ID = B.customer_id"
                            + " WHERE B.DealInPattern = '专柜'"
                             , tbCustomerFilter, "专柜$")
                );



            ZhiyingTable = da.ExecuteQueryDataTable(
                   string.Format("select B.customer_id as id , '{1}' + B.Abbrev as name  from [{0}] A INNER JOIN Customer B ON A.ID = B.customer_id"
                            + " WHERE B.DealInPattern = '自营'"
                             , tbCustomerFilter, "本月销售点数量：" + rowcount + "个$")
                );


            DaixiaoTable.Merge(ZhuanguiTable);
            DaixiaoTable.Merge(ZhiyingTable);
            Zong = DaixiaoTable.Copy();
            #endregion

            #region --动态创建临时表

            string tbTempResult = Generic.GetTmpTableName();
            sb.Length = 0;
            sb.AppendLine(" SELECT TOP 0  CAST(0 AS VARCHAR(MAX)) as [销售点名称] , ");

            //输出的时候再as name
            foreach (DataRow item in Zong.Rows)
            {
                sql += string.Format(" cast(0 AS decimal(30, 2)) as [{0}], ", item["id"]);
            }

            sb.Append(sql);
            sb.Append("  CAST(0 AS DECIMAL(30, 2)) AS  [本月销售点数量：_个$专柜合计]");
            sb.Append(" ,CAST(0 AS DECIMAL(30, 2)) AS  [本月销售点数量：_个$代销合计]");
            sb.Append(" ,CAST(0 AS DECIMAL(30, 2)) AS  [本月销售点数量：_个$合计]");
            sb.AppendLine("into " + tbTempResult);
            sb.AppendLine(" FROM GOODS ");
            da.ExecuteNonQuery(sb.ToString());
            return tbTempResult;

            #endregion
        }

      

        //统计销售数据
        private string GetCheck(string tbDelivery)
        {

            int intSizeCount = new GenericBusiness().GetMaxSizeCount();
            sb.Length = 0;
            sb.AppendLine(" SELECT A.Customer_ID, A.CheckDate, B.DiscountPrice ,");
            sb.AppendLine( SizeField.GetSizeSumField(intSizeCount, "c.") + " AS Quantity");
            sb.AppendLine(" into " + tbDelivery);
            sb.AppendLine(" FROM [vn_Check] A ");
            sb.AppendLine("   INNER JOIN vn_CheckGoods B ON A.CheckID = B.CheckID ");
            sb.AppendLine("   INNER JOIN vn_CheckDetail C ON B.CheckGoodsID = C.CheckGoodsID ");
            sb.AppendLine(" WHERE 1 = 1 " );
            sb.AppendLine("  AND Exists (SELECT 1 FROM " + tbCustomerFilter + " WHERE ID = A.Customer_ID) \n");
            sb.AppendFormat("  AND A.CheckDate>='{0}' AND A.CheckDate<='{1} 23:59:59' ", datebegin, dateend);
            sb.AppendLine(" GROUP BY A.Customer_ID,A.CheckDate,B.DiscountPrice");
            da.ExecuteNonQuery(sb.ToString());
            return tbDelivery;
        }


        private string GetCheck2(string tbDelivery)
        {

            int intSizeCount = new GenericBusiness().GetMaxSizeCount();
            sb.Length = 0;
            sb.AppendLine("select a.Customer_ID,a.CheckDate,B.DiscountPrice ,");
            sb.AppendLine(SizeField.GetSizeSumField(intSizeCount, "c.") + " AS Quantity");
            sb.AppendLine("into " + tbDelivery);
            sb.AppendLine(" FROM [vn_Check] A ");
            sb.AppendLine("   INNER JOIN vn_CheckGoods B ON A.CheckID = B.CheckID ");
            sb.AppendLine("   INNER JOIN vn_CheckDetail C ON B.CheckGoodsID = C.CheckGoodsID ");
            sb.AppendLine("   WHERE 1 = 1 ");
            sb.AppendLine("  AND Exists (SELECT 1 FROM " + tbCustomerFilter + " WHERE ID = A.Customer_ID) \n");
            sb.AppendFormat("  and a.CheckDate>='{0}' and a.CheckDate<='{1} 23:59:59' ", Smonthbegin, Smonthend);
            sb.AppendLine("group by a.Customer_ID,a.CheckDate,B.DiscountPrice");
            da.ExecuteNonQuery(sb.ToString());
            return tbDelivery;
        }


        private string GetCheck3(string tbDelivery)
        {

            int intSizeCount = new GenericBusiness().GetMaxSizeCount();
            sb.Length = 0;
            sb.AppendLine("select a.Customer_ID,a.CheckDate,B.DiscountPrice ,");
            sb.AppendLine(SizeField.GetSizeSumField(intSizeCount, "c.") + " AS Quantity");
            sb.AppendLine("into " + tbDelivery);
            sb.AppendLine(" FROM [vn_Check] A ");
            sb.AppendLine("   INNER JOIN vn_CheckGoods B ON A.CheckID = B.CheckID ");
            sb.AppendLine("   INNER JOIN vn_CheckDetail C ON B.CheckGoodsID = C.CheckGoodsID ");
            sb.AppendLine("   WHERE 1 = 1 ");
            sb.AppendLine("  AND Exists (SELECT 1 FROM " + tbCustomerFilter + " WHERE ID = A.Customer_ID) \n");
            sb.AppendFormat("  and a.CheckDate>='{0}' and a.CheckDate<='{1} 23:59:59' ", Syearbegin, Syearend);
            sb.AppendLine("group by a.Customer_ID,a.CheckDate,B.DiscountPrice");
            da.ExecuteNonQuery(sb.ToString());
            return tbDelivery;
        }

        private void initRows(string tbDelivery)
        {

            //当前合计日均 本月预计 上月销售 上月同比  上年同期  上年同比 当月计划预计完成
            sb.Length = 0;
            sb.AppendLine(" INSERT INTO " + tbDelivery);
            sb.AppendLine(" (销售点名称) values ('当前合计') ");
            sb.AppendLine();
            sb.AppendLine(" INSERT INTO " + tbDelivery);
            sb.AppendLine(" (销售点名称) values ('日均') ");
            sb.AppendLine();
            sb.AppendLine(" INSERT INTO " + tbDelivery);
            sb.AppendLine(" (销售点名称) values ('本月预计') ");
            sb.AppendLine();
            sb.AppendLine(" INSERT INTO " + tbDelivery);
            sb.AppendLine(" (销售点名称) values ('上月销售') ");
            sb.AppendLine();
            sb.AppendLine(" INSERT INTO " + tbDelivery);
            sb.AppendLine(" (销售点名称) values ('上月同比') ");
            sb.AppendLine();
            sb.AppendLine(" INSERT INTO " + tbDelivery);
            sb.AppendLine(" (销售点名称) values ('上年同期') ");
            sb.AppendLine();
            sb.AppendLine(" INSERT INTO " + tbDelivery);
            sb.AppendLine(" (销售点名称) values ('当月计划') ");

            sb.AppendLine();
            sb.AppendLine(" INSERT INTO " + tbDelivery);
            sb.AppendLine(" (销售点名称) values ('预计完成') ");

            da.ExecuteNonQuery(sb.ToString());

            //插入某月的各个天数
            for (int i = 1; i <= Convert.ToInt32(DaysInMonth); i++)//先把某月的天数插上
            {
                sb.Length = 0;
                sb.AppendLine(" INSERT INTO " + tbDelivery);
                sb.AppendLine(" (销售点名称) ");
                sb.AppendFormat(" SELECT '{0}日'  \n", i);
                da.ExecuteNonQuery(sb.ToString());

            }

            sb.Length = 0;
            sb.AppendLine(" INSERT INTO " + tbDelivery);
            sb.AppendLine(" (销售点名称) values ('单日') ");
            sb.AppendLine();
            sb.AppendLine(" INSERT INTO " + tbDelivery);
            sb.AppendLine(" (销售点名称) values ('双日') ");
            sb.AppendLine();
            sb.AppendLine(" INSERT INTO " + tbDelivery);
            sb.AppendLine(" (销售点名称) values ('1到7号') ");
            sb.AppendLine();
            sb.AppendLine(" INSERT INTO " + tbDelivery);
            sb.AppendLine(" (销售点名称) values ('8号到14号') ");
            sb.AppendLine();
            sb.AppendLine(" INSERT INTO " + tbDelivery);
            sb.AppendLine(" (销售点名称) values ('15到21号') ");
            sb.AppendLine();
            sb.AppendLine(" INSERT INTO " + tbDelivery);
            sb.AppendLine(" (销售点名称) values ('22到28号') ");
            sb.AppendLine();
            sb.AppendLine(" INSERT INTO " + tbDelivery);
            sb.AppendLine(" (销售点名称) values ('29到31号') ");
            da.ExecuteNonQuery(sb.ToString());

        }
        private void UpdateEveryday(string tbDelivery)
        {
            string datebegin = "";
            datebegin = yyyy + "-" + MM + "-";

            for (int i = 1; i <= 31; i++)
            {
                foreach (DataRow item in Zong.Rows)//更新金额
                {
                    if (i <= Convert.ToInt32(DaysInMonth))//控制月的天数要不然月数条件填大会蹦
                    {
                        sb.Length = 0;
                        sb.AppendLine(" UPDATE " + tbDelivery);
                        sb.AppendLine("        ");
                        sb.AppendFormat(" SET [{0}] = B.金额  \n", item["id"].ToString());
                        sb.AppendFormat(" FROM (");
                        sb.Append("         SELECT  SUM(isnull(A.Quantity,0) * ISNULL(A.DiscountPrice,0)) as [金额] \n");
                        sb.AppendLine("     FROM [" + resultTable + "] a ");
                        sb.AppendFormat("   WHERE A.Customer_ID = '{0}' ", item["id"].ToString());
                        sb.AppendFormat("   AND A.CheckDate>='{0}' AND A.CheckDate<='{0} 23:59:59' ", datebegin + i.ToString());
                        sb.AppendFormat("      ) B");
                        sb.AppendFormat(" WHERE 销售点名称= '{0}日' ", i);//控制一行的N列也就是店铺编号
                        da.ExecuteNonQuery(sb.ToString());

                    }
                }
            }
        }

        private void UpdateOneTwo(string tbDelivery)
        {
            List<int> one = new List<int>();
            string oneTable = Generic.GetTmpTableName();

            List<int> two = new List<int>();
            string twoTable = Generic.GetTmpTableName();

            string datebegin = "";
            datebegin = yyyy + "-" + MM + "-";

            //单双日先分开
            for (int i = 1; i <= Convert.ToInt32(DaysInMonth); i++)
            {
                if (i % 2 == 0)
                {
                    two.Add(i);
                }
                else
                {
                    one.Add(i);
                }
            }
            da.ExecuteNonQuery("SELECT cast(0 as decimal(10,2) ) as [金额] ,cast ('' as varchar(max)) as Customer_ID  into " + oneTable);
            da.ExecuteNonQuery("SELECT cast(0 as decimal(10,2) ) as [金额] ,cast ('' as varchar(max)) as Customer_ID  into " + twoTable);

            foreach (int item in one)
            {
                sb.Length = 0;
                sb.AppendLine("INSERT INTO " + oneTable);
                sb.AppendLine(" SELECT  isnull(a.Quantity,0) * isnull(a.DiscountPrice,0) AS [金额], a.Customer_ID  ");
                sb.AppendLine(" FROM [" + resultTable + "] a ");
                sb.AppendLine(" WHERE 1=1");
                sb.AppendFormat(" AND a.CheckDate>='{0}' and a.CheckDate<='{0} 23:59:59' ", datebegin + item.ToString());
                da.ExecuteNonQuery(sb.ToString());

            }

            foreach (int item in two)
            {
                sb.Length = 0;
                sb.AppendLine("INSERT INTO " + twoTable);
                sb.AppendLine(" SELECT  isnull(a.Quantity,0) * isnull(a.DiscountPrice,0) AS [金额], a.Customer_ID  ");
                sb.AppendLine(" FROM [" + resultTable + "] a ");
                sb.AppendLine(" WHERE 1=1");
                sb.AppendFormat(" AND a.CheckDate>='{0}' and a.CheckDate<='{0} 23:59:59' ", datebegin + item.ToString());
                da.ExecuteNonQuery(sb.ToString());
            }

            // 更新单双日数据
            foreach (DataRow item in Zong.Rows)//更新金额
            {

                sb.Length = 0;
                sb.AppendLine(" UPDATE " + tbDelivery);
                sb.AppendLine("        ");
                sb.AppendFormat(" SET [{0}] = b.金额  \n", item["id"].ToString());
                sb.AppendFormat(" FROM ( \n");
                sb.AppendLine("   SELECT  sum([金额])  as [金额] \n");
                sb.AppendLine("   FROM [" + oneTable + "] a ");
                sb.AppendFormat(" WHERE A.Customer_ID = '{0}' \n", item["id"].ToString());
                sb.AppendLine("   GROUP BY A.Customer_ID ");
                sb.AppendLine(" ) B");
                sb.AppendLine(" WHERE 销售点名称= '单日' ");//控制一行的N列也就是店铺编号
                da.ExecuteNonQuery(sb.ToString());

            }

            foreach (DataRow item in Zong.Rows)//更新金额
            {

                sb.Length = 0;
                sb.AppendLine(" UPDATE " + tbDelivery);
                sb.AppendLine("        ");
                sb.AppendFormat(" SET [{0}] = b.金额  \n", item["id"].ToString());
                sb.AppendFormat(" FROM ( \n");
                sb.AppendLine("   SELECT  sum([金额])  AS [金额] \n");
                sb.AppendLine("   FROM [" + twoTable + "] a ");
                sb.AppendFormat(" WHERE A.Customer_ID = '{0}' \n", item["id"].ToString());
                sb.AppendLine("   GROUP BY A.Customer_ID ");
                sb.AppendLine(" ) b");
                sb.AppendLine(" WHERE 销售点名称= '双日' ");//控制一行的N列也就是店铺编号
                da.ExecuteNonQuery(sb.ToString());

            }

       
        }

        private void UpdateTimeRange(string tbDelivery)
        {
            string datebegin = "";
            datebegin = yyyy + "-" + MM + "-";
            Dictionary<string, int> dict = new Dictionary<string, int>();
            dict.Add("1到7号",   1);
            dict.Add("8号到14号",8);
            dict.Add("15到21号", 15);
            dict.Add("22到28号", 22);
            dict.Add("29到31号", 29);

            foreach (var dictnum in dict)
            {
                foreach (DataRow item in Zong.Rows)//更新金额
                {
                    //算出结束时间
                    int mm = dictnum.Value + 6;
                    string dateend = datebegin + mm.ToString();

                    if (dictnum.Value ==  29) //某月29天结束日期只要到月底
                    {
                        dateend = datebegin + DaysInMonth;
                    }

                    sb.Length = 0;
                    sb.AppendLine(" UPDATE " + tbDelivery);
                    sb.AppendLine("        ");
                    sb.AppendFormat(" SET [{0}] = B.金额  \n", item["id"].ToString());
                    sb.AppendFormat(" FROM (");
                    sb.Append("              SELECT  SUM(isnull(A.Quantity,0) * ISNULL(A.DiscountPrice,0)) as [金额] \n");
                    sb.AppendLine("          FROM [" + resultTable + "] a ");
                    sb.AppendFormat("        WHERE A.Customer_ID = '{0}' ", item["id"].ToString());
                    sb.AppendFormat("        AND A.CheckDate>='{0}' AND A.CheckDate<='{1} 23:59:59' ", datebegin + dictnum.Value.ToString(), dateend);
                    sb.AppendFormat("      ) B");
                    sb.AppendFormat(" WHERE 销售点名称= '{0}' ", dictnum.Key.ToString());
                    da.ExecuteNonQuery(sb.ToString());

                }
            }           
        }

        private void UpdateCurrentSum(string tbDelivery)
        {
            foreach (DataRow item in Zong.Rows)//更新金额
            {

                sb.Length = 0;
                sb.AppendLine(" UPDATE " + tbDelivery);
                sb.AppendLine("        ");
                sb.AppendFormat(" SET [{0}] = B.金额  \n", item["id"].ToString());
                sb.AppendFormat(" FROM (");
                sb.Append("                SELECT  SUM(isnull(A.Quantity,0) * ISNULL(A.DiscountPrice,0)) AS [金额] \n");
                sb.AppendLine("            FROM [" + resultTable + "] a ");
                sb.AppendFormat("           WHERE A.Customer_ID = '{0}' ", item["id"].ToString());          
                sb.AppendFormat("      ) B");
                sb.AppendFormat(" WHERE 销售点名称= '{0}' ", "当前合计");
                da.ExecuteNonQuery(sb.ToString());

            }


            int mm = Convert.ToInt32(da.ExecuteQueryDataTable("SELECT DAY(GETDATE()) as day").Rows[0]["day"]);
           
            foreach (DataRow item in Zong.Rows)//更新金额
            {
                
                sb.Length = 0;
                sb.AppendLine(" UPDATE " + tbDelivery);
                sb.AppendLine("        ");
                sb.AppendFormat(" SET [{0}] = B.金额  \n", item["id"].ToString());
                sb.AppendFormat(" FROM (");
                sb.AppendFormat(          @"SELECT  SUM(isnull(A.Quantity,0) * ISNULL(A.DiscountPrice,0))  / {0} AS [金额]  ", mm);
                sb.AppendLine("            \n FROM [" + resultTable + "] a ");
                sb.AppendFormat(           " WHERE A.Customer_ID = '{0}' ", item["id"].ToString());
                sb.AppendFormat("   ) B");
                sb.AppendFormat(" WHERE 销售点名称= '{0}' ", "日均");
                da.ExecuteNonQuery(sb.ToString());

            }

        }

        private void UpdateUpperMonth(string tbDelivery)
        {
            int mm = Convert.ToInt32(DaysInMonth);

            foreach (DataRow item in Zong.Rows)
            {

                sb.Length = 0;
                sb.AppendLine(" UPDATE " + tbDelivery);
                sb.AppendLine("        ");
                sb.AppendFormat(" SET [{0}] = B.金额  \n", item["id"].ToString());
                sb.AppendFormat(" FROM (");
                sb.AppendFormat(@"         SELECT  SUM(isnull(A.Quantity,0) * ISNULL(A.DiscountPrice,0))  * {0} as [金额]  ", mm);
                sb.AppendLine("            \n FROM [" + resultTable + "] a ");
                sb.AppendFormat("          WHERE A.Customer_ID = '{0}' ", item["id"].ToString());
                sb.AppendFormat("      ) B");
                sb.AppendFormat(" WHERE 销售点名称= '{0}' ", "本月预计");
                da.ExecuteNonQuery(sb.ToString());

            }


            string resultTable2 = Generic.GetTmpTableName();
            GetCheck2(resultTable2);

            foreach (DataRow item in Zong.Rows)
            {
                sb.Length = 0;
                sb.AppendLine(" UPDATE " + tbDelivery);
                sb.AppendLine("        ");
                sb.AppendFormat(" SET [{0}] = B.金额  \n", item["id"].ToString());
                sb.AppendFormat(" FROM (");
                sb.AppendFormat(@"         SELECT  SUM(isnull(A.Quantity,0) * ISNULL(A.DiscountPrice,0))  * {0} as [金额]  ", mm);
                sb.AppendLine("            \n FROM [" + resultTable2 + "] a ");
                sb.AppendFormat("           WHERE A.Customer_ID = '{0}' ", item["id"].ToString());
                sb.AppendLine("       ) B");
                sb.AppendFormat(" WHERE 销售点名称= '{0}' ", "上月销售");
                da.ExecuteNonQuery(sb.ToString());

            }

            //现在上月同步是本月预计的数据 (本月预计 － 上月销售）/上月销售)          
            foreach (DataRow item in Zong.Rows)
            {

                sb.Length = 0;
                sb.AppendLine(" UPDATE " + tbDelivery);
                sb.AppendLine("        ");
                sb.AppendFormat(" SET [{0}] = B.金额  \n", item["id"].ToString());
                sb.AppendFormat(" FROM (");
                sb.AppendFormat(@"         SELECT  SUM(isnull(A.Quantity,0) * ISNULL(A.DiscountPrice,0))  * {0} as [金额]  ", mm);
                sb.AppendLine("            \n FROM [" + resultTable + "] a ");
                sb.AppendFormat("          WHERE A.Customer_ID = '{0}' ", item["id"].ToString());
                sb.AppendFormat("      ) B");
                sb.AppendFormat(" WHERE 销售点名称= '{0}' ", "上月同比");
                da.ExecuteNonQuery(sb.ToString());
            }

            //再次更新上月同比
            foreach (DataRow item in Zong.Rows)
            {

                sb.Length = 0;
                sb.AppendLine(" UPDATE " + tbDelivery);
                sb.AppendLine("        ");
                sb.AppendFormat(" SET [{0}] =  ([{0}] - isnull(B.金额,0)) / nullif(B.金额,0) \n", item["id"].ToString());
                sb.AppendFormat(" FROM (");
                sb.AppendFormat("         SELECT  isnull([{0}],0) as  金额 ", item["id"].ToString());
                sb.AppendLine("           FROM [" + tbDelivery + "] a ");
                sb.AppendFormat("         WHERE 销售点名称= '{0}' ", "上月销售");
                sb.AppendFormat("       ) B");
                sb.AppendFormat(" WHERE 销售点名称= '{0}' ", "上月同比");
                da.ExecuteNonQuery(sb.ToString());

            }

        }

        private void GetCustomerPerformanceGuide(string tbDelivery)
        {
            string Month = MM.Replace("0", "");
            sb.Length = 0;
            sb.AppendFormat(" SELECT a.Customer_ID, m{0} as 金额", Month);
            sb.AppendLine(" INTO " +tbDelivery);
            sb.AppendLine(" FROM CustomerPerformanceGuide A ");
            sb.AppendFormat(" WHERE  a. Year='{0}' \n",yyyy);
            sb.AppendLine("         And Exists(Select 1 from " + tbCustomerFilter + " CT where A.Customer_ID = CT.ID)");
            da.ExecuteNonQuery(sb.ToString());

        }

        private void UpdatePlan(string tbDelivery)
        {
            int mm = Convert.ToInt32(DaysInMonth);

            string resultTable4 = Generic.GetTmpTableName();
            GetCustomerPerformanceGuide(resultTable4);

            foreach (DataRow item in Zong.Rows)
            {
                sb.Length = 0;
                sb.AppendLine(" UPDATE " + tbDelivery);
                sb.AppendLine("        ");
                sb.AppendFormat(" SET [{0}] = B.金额  \n", item["id"].ToString());
                sb.AppendFormat(" FROM (");
                sb.AppendFormat(" SELECT  SUM(金额) as [金额]  ");
                sb.AppendLine(" \n FROM [" + resultTable4 + "] a ");
                sb.AppendFormat(" WHERE A.Customer_ID = '{0}' ", item["id"].ToString());
                sb.AppendFormat(" ) B");
                sb.AppendFormat(" WHERE 销售点名称= '{0}' ", "当月计划");
                da.ExecuteNonQuery(sb.ToString());

            }


            // 预计完成 当月指标
            foreach (DataRow item in Zong.Rows)
            {

                sb.Length = 0;
                sb.AppendLine(" UPDATE " + tbDelivery);
                sb.AppendLine("        ");
                sb.AppendFormat(" SET [{0}] = B.金额  \n", item["id"].ToString());
                sb.AppendFormat(" FROM (");
                sb.AppendFormat(" SELECT  SUM(金额) as [金额]  ");
                sb.AppendLine(" \n FROM [" + resultTable4 + "] a ");
                sb.AppendFormat(" WHERE A.Customer_ID = '{0}' ", item["id"].ToString());
                sb.AppendFormat(" ) B");
                sb.AppendFormat(" WHERE 销售点名称= '{0}' ", "预计完成");
                da.ExecuteNonQuery(sb.ToString());

            }

            //再次更新上年同比    
            foreach (DataRow item in Zong.Rows)
            {

                sb.Length = 0;
                sb.AppendLine(" UPDATE " + tbDelivery);
                sb.AppendLine("        ");
                sb.AppendFormat(" SET [{0}] =  B.金额 / nullif([{0}],0) ", item["id"].ToString());
                sb.AppendFormat(" FROM (");
                sb.AppendFormat("         SELECT  isnull([{0}],0) as  金额 ", item["id"].ToString());
                sb.AppendLine("           FROM [" + tbDelivery + "] a ");
                sb.AppendFormat("         WHERE 销售点名称= '{0}' ", "当前合计");
                sb.AppendFormat("       ) B");
                sb.AppendFormat(" WHERE 销售点名称= '{0}' ", "预计完成");
                da.ExecuteNonQuery(sb.ToString());
            }

        }


        private void UpdeSum(string tbDelivery)
        {

            sb.Length = 0;
            sb.AppendLine(" UPDATE " + tbDelivery);
            sb.AppendLine("        ");
            sb.AppendLine(" SET [本月销售点数量：_个$专柜合计] = B.金额 ");
            sb.AppendLine(" FROM ( ");
            sb.AppendLine("  SELECT  SUM(isnull(A.Quantity,0) * ISNULL(A.DiscountPrice,0))   as [金额] ");
            sb.AppendLine("  FROM [" + resultTable + "] a ");
            sb.AppendLine("  WHERE Exists(Select 1 from " + ZhuanguiTable2 + " ct where a.customer_id = ct.ID)");
            sb.AppendFormat(" ) B");
            sb.AppendFormat(" WHERE 销售点名称= '{0}' ", "当前合计");
            da.ExecuteNonQuery(sb.ToString());


            sb.Length = 0;
            sb.AppendLine(" UPDATE " + tbDelivery);
            sb.AppendLine("        ");
            sb.AppendLine(" SET [本月销售点数量：_个$代销合计] = B.金额  ");
            sb.AppendLine(" FROM ( ");
            sb.AppendLine("  SELECT   SUM(isnull(A.Quantity,0) * ISNULL(A.DiscountPrice,0))   as [金额]  ");
            sb.AppendLine("  FROM [" + resultTable + "] a ");
            sb.AppendLine("  WHERE Exists(Select 1 from " + DaixiaoTable2 + " ct where a.customer_id = ct.ID)");
            sb.AppendFormat(" ) B");
            sb.AppendFormat(" WHERE 销售点名称= '{0}' ", "当前合计");
            da.ExecuteNonQuery(sb.ToString());


            sb.Length = 0;
            sb.AppendLine(" UPDATE " + tbDelivery);
            sb.AppendLine("        ");
            sb.AppendLine(" SET [本月销售点数量：_个$合计] = B.金额  ");
            sb.AppendLine(" FROM ( ");
            sb.AppendLine("  SELECT   SUM(isnull(A.Quantity,0) * ISNULL(A.DiscountPrice,0))   as [金额]  ");
            sb.AppendLine("  FROM [" + resultTable + "] a INNER JOIN CUSTOMER CU ON A.CUSTOMER_ID = CU.CUSTOMER_ID ");
            sb.AppendLine("  WHERE Exists(Select 1 from " + tbCustomerFilter + " ct  where a.customer_id = ct.ID)");
            sb.AppendLine("  AND CU.DealInPattern IN ('代销','专柜','自营') ");
            sb.AppendFormat(" ) B");
            sb.AppendFormat(" WHERE 销售点名称= '{0}' ", "当前合计");
            da.ExecuteNonQuery(sb.ToString());

        }

        private void UpdateUpperYear(string tbDelivery)
        {
            int mm = Convert.ToInt32(DaysInMonth);



            string resultTable3 = Generic.GetTmpTableName();
            GetCheck3(resultTable3);

            foreach (DataRow item in Zong.Rows)
            {
                sb.Length = 0;
                sb.AppendLine(" UPDATE " + tbDelivery);
                sb.AppendLine("        ");
                sb.AppendFormat(" SET [{0}] = B.金额  \n", item["id"].ToString());
                sb.AppendFormat(" FROM (");
                sb.AppendFormat(@" SELECT  SUM(isnull(A.Quantity,0) * ISNULL(A.DiscountPrice,0))  * {0} as [金额]  ", mm);
                sb.AppendLine(" \n FROM [" + resultTable3 + "] a ");
                sb.AppendFormat(" WHERE A.Customer_ID = '{0}' ", item["id"].ToString());
                sb.AppendFormat(" ) B");
                sb.AppendFormat(" WHERE 销售点名称= '{0}' ", "上年同期");
                da.ExecuteNonQuery(sb.ToString());

            }


            //现在上月同比是本月预计的数据
            foreach (DataRow item in Zong.Rows)
            {

                sb.Length = 0;
                sb.AppendLine(" UPDATE " + tbDelivery);
                sb.AppendLine("        ");
                sb.AppendFormat(" SET [{0}] = B.金额  \n", item["id"].ToString());
                sb.AppendFormat(" FROM (");
                sb.AppendFormat(@"         SELECT  SUM(isnull(A.Quantity,0) * ISNULL(A.DiscountPrice,0))  * {0} as [金额]  ", mm);
                sb.AppendLine("            \n FROM [" + resultTable + "] a ");
                sb.AppendFormat("          WHERE A.Customer_ID = '{0}' ", item["id"].ToString());
                sb.AppendFormat("      ) B");
                sb.AppendFormat(" WHERE 销售点名称= '{0}' ", "上年同比");
                da.ExecuteNonQuery(sb.ToString());

            }

            //再次更新上年同比    
            foreach (DataRow item in Zong.Rows)
            {

                sb.Length = 0;
                sb.AppendLine(" UPDATE " + tbDelivery);
                sb.AppendLine("        ");
                sb.AppendFormat(" SET [{0}] =  ([{0}] - isnull(B.金额,0)) / nullif(B.金额,0) \n", item["id"].ToString());
                sb.AppendFormat(" FROM (");
                sb.AppendFormat("         SELECT  isnull([{0}],0) as  金额 ", item["id"].ToString());
                sb.AppendLine("           FROM [" + tbDelivery + "] a ");
                sb.AppendFormat("         WHERE 销售点名称= '{0}' ", "上年同期");
                sb.AppendFormat("       ) B");
                sb.AppendFormat(" WHERE 销售点名称= '{0}' ", "上月同比");
                da.ExecuteNonQuery(sb.ToString());
            }

        }

        private DataSet GetResultData(string tbDelivery)
        {
              
            initRows(tbDelivery); //初始化化行

            GetCheck(resultTable);//获取当月销售总数据

            UpdateEveryday(tbDelivery);//更新某月每天的数据

            UpdateOneTwo(tbDelivery);//更新单双日期数据

            UpdateTimeRange(tbDelivery);//更新时间区域的数据

            UpdateCurrentSum(tbDelivery); //更新当前合计 和 日均

            UpdateUpperMonth(tbDelivery); //更新本月预计 上月销售  上月同比

            UpdateUpperYear(tbDelivery); //更新上年同期 上年同比 

            UpdatePlan(tbDelivery);  //当月计划   预计完成

            UpdeSum(tbDelivery);  //更新汇总数据

            sb.Length = 0; 

            string rows =  initColumn();

            sb.AppendLine(" select "+ rows);
            sb.AppendFormat(" from [{0}]  ", tbDelivery);
            DataSet dsResult = da.ExecuteQueryDataSet(sb.ToString());   
            dsResult.Tables[0].TableName = "ReportData";
            return dsResult;
            
        }

        //把店铺id转换成店铺简称
        private string initColumn()
        {
            string rows = "";
            string rowssql = "";
            rows = "[销售点名称],";

            foreach (DataRow item in Zong.Rows)
            {
                rowssql += string.Format("  [{0}] as [{1}], ", item["id"], item["name"]);
            }

            rows += rowssql;

            rows += string.Format(" [本月销售点数量：_个$专柜合计] as [本月销售点数量：{0}个$专柜合计]", Zong.Rows.Count);
            rows += string.Format(",[本月销售点数量：_个$代销合计] as [本月销售点数量：{0}个$代销合计] ",Zong.Rows.Count);
            rows += string.Format(",[本月销售点数量：_个$合计]     as [本月销售点数量：{0}个$合计] ", Zong.Rows.Count);

            return rows;
        }
        #endregion 函数处理
    }
}