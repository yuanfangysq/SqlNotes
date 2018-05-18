using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Xml;
using Regent.Report.Business;

namespace Regentsoft
{
    
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public partial class PluginService
    {
        #region --公用类
        /// <summary>
        /// 得到过滤条件类数据
        /// </summary>
        /// <param name="locale">本地化</param>
        /// <param name="module">模块</param>
        /// <param name="returnId">返回错误编号</param>
        /// <param name="returnMessage">返回错误信息</param> 
        /// <returns>数据结果</returns>
        [WebMethod(Description = "得到过滤条件类数据。")]
        [SoapHeader("Credentials")]
        public DataSet GetFilterDataSet(string locale, string module, ref int returnId, ref string returnMessage)
        {
            returnId = 0;
            returnMessage = "";
            DataSet item = null;
            try
            {
                SecurityHelper.VerifyCredentials(this);
                item = new Util().GetFilterDataSet(locale, module);
                if (item != null) returnId = 1;
            }
            catch (Exception ex)
            {
                returnId = -1;
                returnMessage = ex.Message;
            }
            return item;
        }

        /// <summary>
        /// 保存输出栏位信息
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="user">用户</param>
        /// <param name="dataSet">需要保存的内容</param>
        /// <param name="returnId">返回错误编号</param>
        /// <param name="returnMessage">返回错误信息</param> 
        [WebMethod(Description = "保存输出栏位信息。")]
        [SoapHeader("Credentials")]
        public void SaveExportItem(string module, string user, DataSet dataSet, ref int returnId, ref string returnMessage)
        {
            returnId = 0;
            returnMessage = "";
            //DataSet item = null;
            try
            {
                SecurityHelper.VerifyCredentials(this);
                new Util().SaveExportItem(module, user, dataSet);
            }
            catch (Exception ex)
            {
                returnId = -1;
                returnMessage = ex.Message;
            }
        }

        #endregion

        #region --货品类
        /// <summary>
        /// 得到货品过滤条件数据
        /// </summary>
        /// <param name="locale">本地化</param>
        /// <param name="user">用户编号</param>
        /// <param name="onlySchema">只得到数据结构</param>
        /// <param name="right">是否判断权限</param>
        /// <param name="returnId">返回错误编号</param>
        /// <param name="returnMessage">返回错误信息</param> 
        /// <returns>数据结果</returns>
        [WebMethod(Description = "得到货品过滤条件数据。")]
        [SoapHeader("Credentials")]
        public DataSet GetGoodsFilterDataSet(string locale, string user, bool onlySchema, bool right, ref int returnId, ref string returnMessage)
        {
            returnId = 0;
            returnMessage = "";
            DataSet item = null;
            try
            {
                SecurityHelper.VerifyCredentials(this);
                item = new Base().GetGoodsFilterDataSet(locale, user, onlySchema, right);
                if (item != null) returnId = 1;
            }
            catch (Exception ex)
            {
                returnId = -1;
                returnMessage = ex.Message;
            }
            return item;
        }

        /// <summary>
        /// 得到货品基础资料数据
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="locale">本地化</param>
        /// <param name="user">用户编号</param>
        /// <param name="onlySchema">只得到数据结构</param>
        /// <param name="right">是否判断权限</param>
        /// <param name="returnId">返回错误编号</param>
        /// <param name="returnMessage">返回错误信息</param> 
        /// <returns>数据结果</returns>
        [WebMethod(Description = "得到货品基础资料数据。")]
        [SoapHeader("Credentials")]
        public DataSet GetModuleBaseDataSet(string module, string locale, string user, bool onlySchema, bool right, ref int returnId, ref string returnMessage)
        {
            returnId = 0;
            returnMessage = "";
            DataSet item = null;
            try
            {
                SecurityHelper.VerifyCredentials(this);
                item = new Base().GetModuleBaseDataSet(module, locale, user, onlySchema, right);
                if (item != null) returnId = 1;
            }
            catch (Exception ex)
            {
                returnId = -1;
                returnMessage = ex.Message;
            }
            return item;
        }

        #endregion

        #region --得到报表数据
        /// <summary>
        /// 得到报表数据
        /// </summary>
        /// <param name="locale">本地化</param>
        /// <param name="module">模块</param>
        /// <param name="user">用户编号</param>
        /// <param name="onlySchema">只得到数据结构</param>
        /// <param name="filter">过滤</param>
        /// <param name="right">是否判断权限</param>
        /// <param name="returnId">返回错误编号</param>
        /// <param name="returnMessage">返回错误信息</param> 
        /// <returns>数据结果</returns>
        [WebMethod(Description = "得到报表数据。")]
        [SoapHeader("Credentials")]
        public DataSet GetReportDataSet(string locale, string module, string user, bool onlySchema, DataSet filter, bool right, ref int returnId, ref string returnMessage)
        {
            return GetReportDataSetEx(locale, module, user, onlySchema, filter, right, string.Empty, ref returnId, ref returnMessage);
        }

        /// <summary>
        /// 2011-02-15 王明洋 增加扩展Where
        /// </summary>
        /// <param name="locale"></param>
        /// <param name="module"></param>
        /// <param name="user"></param>
        /// <param name="onlySchema"></param>
        /// <param name="filter"></param>
        /// <param name="right"></param>
        /// <param name="extWhere"></param>
        /// <param name="returnId"></param>
        /// <param name="returnMessage"></param>
        /// <returns></returns>
        [WebMethod(Description = "得到报表数据。")]
        [SoapHeader("Credentials")]
        public DataSet GetReportDataSetEx(string locale, string module, string user, bool onlySchema, DataSet filter, bool right, string extWhere, ref int returnId, ref string returnMessage)
        {
            returnId = 0;
            returnMessage = "";
            DataSet item = null;
            try
            {
                SecurityHelper.VerifyCredentials(this);
                item = new Util().GetReportDataSet(locale, module, user, onlySchema, filter, right, extWhere);
                if (item != null) returnId = 1;
            }
            catch (Exception ex)
            {
                returnId = -1;
                returnMessage = ex.Message;
            }

            return item;
        }

        [WebMethod(Description = "得到报表数据。")]
        [SoapHeader("Credentials")]
        public byte[] GetReportData(string locale, string module, string user, bool onlySchema, byte[] btsFilter, bool right, string extWhere)
        {
            SecurityHelper.VerifyCredentials(this);
            DataSet dsFilter = btsFilter == null ? null : RegentZip.ZIP.DecompressDS(btsFilter);
            DataSet ds = new Util().GetReportDataSet(locale, module, user, onlySchema, dsFilter, right, extWhere);
            return RegentZip.ZIP.CompressDS(ds);
        }
        #endregion

        #region --得到输出项目数据
        /// <summary>
        /// 得到输出项目数据
        /// </summary>
        /// <param name="locale">本地化</param>
        /// <param name="module">模块</param>
        /// <param name="user">用户编号</param>
        /// <param name="onlySchema">只得到数据结构</param>
        /// <param name="right">是否判断权限</param>
        /// <param name="returnId">返回错误编号</param>
        /// <param name="returnMessage">返回错误信息</param> 
        /// <returns>数据结果</returns>
        [WebMethod(Description = "得到输出项目数据。")]
        [SoapHeader("Credentials")]
        public DataSet GetExportItemDataSet(string locale, string module, string user, bool onlySchema, bool right, ref int returnId, ref string returnMessage)
        {
            returnId = 0;
            returnMessage = "";
            DataSet item = null;
            try
            {
                //SecurityHelper.VerifyCredentials(this);
                item = new Util().GetExportItemDataSet(locale, module, user, onlySchema, right);
                if (item != null) returnId = 1;
            }
            catch (Exception ex)
            {
                returnId = -1;
                returnMessage = ex.Message;
                //if (GlobalResource.IoLogWebService)
                //    IoLog.WriteLog(GlobalResource.IoLogFile, this.ToString(), ex);
            }
            //if (GlobalResource.IoLogWebService)
            //    IoLog.WriteLog(GlobalResource.IoLogFile, this.ToString(), "成功执行GetExportItemDataSet()。");

            return item;
        }

        #endregion

        #region --其他
        /// <summary>
        /// 得到货品尺码,FieldName,Size
        /// </summary>
        /// <param name="goodsID">货号</param>
        /// <param name="returnId">返回错误编号</param>
        /// <param name="returnMessage">返回错误信息</param> 
        /// <returns>数据结果</returns>
        [WebMethod(Description = "得到货品尺码,FieldName,Size。")]
        [SoapHeader("Credentials")]
        public DataSet GetSizeOfGoods(string goodsID, ref int returnId, ref string returnMessage)
        {
            returnId = 0;
            returnMessage = "";
            DataSet item = null;
            try
            {
                SecurityHelper.VerifyCredentials(this);
                item = new Util().GetSizeOfGoods(goodsID);
                if (item != null) returnId = 1;
            }
            catch (Exception ex)
            {
                returnId = -1;
                returnMessage = ex.Message;
            }
            return item;
        }

        /// <summary>
        /// 得到货品尺码列表
        /// </summary>
        /// <param name="goodsTable">货品表</param>
        /// <param name="sizeCount">尺码列数</param>
        /// <param name="returnId">返回错误编号</param>
        /// <param name="returnMessage">返回错误信息</param> 
        /// <returns>数据结果</returns>
        [WebMethod(Description = "得到货品尺码列表。")]
        [SoapHeader("Credentials")]
        public DataSet GetSizeList(string goodsTable, ref int sizeCount, ref int returnId, ref string returnMessage)
        {
            returnId = 0;
            returnMessage = "";
            DataSet item = null;
            try
            {
                SecurityHelper.VerifyCredentials(this);
                item = new Util().GetSizeList(goodsTable, ref sizeCount);
                if (item != null) returnId = 1;
            }
            catch (Exception ex)
            {
                returnId = -1;
                returnMessage = ex.Message;
            }
            return item;
        }


        #endregion

        #region 得到报表数据

        [WebMethod(Description = "得到报表数据")]
        [SoapHeader("Credentials")]
        public DataSet GetRptData(string moduleName, DataSet dsCondition, string userNo)
        {
            DataSet ds = new DataSet();
            Type type = Type.GetType("Regentsoft.Plugin.Report.Bll." + moduleName + ",Regentsoft.Plugin.Report.Bll");
            if (type == null)
                return ds;

            object dObj = Activator.CreateInstance(type);
            //属性
            System.Reflection.PropertyInfo property = type.GetProperty("UserNo");
            property.SetValue(dObj, userNo, null);
            System.Reflection.PropertyInfo property1 = type.GetProperty("DsCondition");
            property1.SetValue(dObj, dsCondition, null);

            //获取方法的信息
            System.Reflection.MethodInfo method = type.GetMethod("Calculate");
            //调用方法的一些标志位，这里的含义是Public并且是实例方法，这也是默认的值
            System.Reflection.BindingFlags flag = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;
            //调用方法，用一个object接收返回值
            object obj = method.Invoke(dObj, flag, null, null, null);

            if (obj != null)
            {
                ds = (DataSet)obj;
            }

            return ds;
        }


        /// <summary>
        /// 得到保存的报表输出项目
        /// </summary>
        /// <param name="userNo">用户</param>
        /// <param name="moduleName">报表moduleName</param>
        /// <returns></returns>
        [WebMethod(Description = "得到保存的报表输出项目")]
        [SoapHeader("Credentials")]
        public DataTable ReportGetSavedOutItem(string userNo, string moduleName)
        {
            return Regent.Business.ReportBusiness.Generic.GetSavedOutItem(userNo, moduleName);
        }


        [WebMethod(Description = "判断用户是否拥有指定模块指定类型权限。")]
        [SoapHeader("Credentials")]
        public bool IsHasRight(string userNo, string moduleID, Regent.Business.ReportBusiness.RightType rightType)
        {
            return Regent.Business.ReportBusiness.Generic.IsHasRight(userNo, moduleID, rightType);
        }

        #endregion

        /// <summary>
        /// 获取用户模块权限
        /// </summary>
        /// <param name="userID">用户编号</param>
        /// <returns>用户模块权限</returns>
        [WebMethod(Description = "获取用户模块AllowToExcel权限。")]
        [SoapHeader("Credentials")]
        public bool AllowToExcel(string userID)
        {
            SecurityHelper.VerifyCredentials(this);
            Regent.Business.Base.RightAdd ra = new Regent.Business.UserBusiness().GetRightAdd(userID, "AllowToExcel");
            return ra.AddIn | ra.AddNew | ra.DeleteRec | ra.Modify | ra.Post | ra.ReadRec;
        }

        [WebMethod(Description = "得到双击后的数据")]
        [SoapHeader("Credentials")]
        public DataSet GetRptDetailData(string moduleName, object[] parameters)
        {
            DataSet ds = null;
            DataTable dt = new DataTable("detail");

            Type type = null;

            if (moduleName.Contains("."))
            {
                type = Type.GetType(moduleName);
            }
            else
            {
                type = Type.GetType("Regentsoft.Plugin.Report.Bll." + moduleName + ",Regentsoft.Plugin.Report.Bll");
            }

            if (type == null)
                return ds;

            object dObj = Activator.CreateInstance(type);
            ////属性
            //System.Reflection.PropertyInfo property = type.GetProperty("UserNo");
            //property.SetValue(dObj, userNo, null);
            //System.Reflection.PropertyInfo property1 = type.GetProperty("DsCondition");
            //property1.SetValue(dObj, dsCondition, null);

            //获取方法的信息
            System.Reflection.MethodInfo method = type.GetMethod("GetSecondDetail");
            //调用方法的一些标志位，这里的含义是Public并且是实例方法，这也是默认的值
            System.Reflection.BindingFlags flag = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;
            //调用方法，用一个object接收返回值
            //object obj = method.Invoke(dObj, flag, null, null, null);
            object obj = method.Invoke(dObj, flag, null, parameters, null);

            if (obj != null)
            {
                dt = (DataTable)obj;
            }
            ds = new DataSet();
            if (dt.DataSet != null)
                dt.DataSet.Tables.Remove(dt);
            ds.Tables.Add(dt);

            return ds;
        }

        [WebMethod(Description = "得到报表数据")]
        [SoapHeader("Credentials")]
        public byte[] GetRptDataZip(string moduleName, byte[] btCondition, string userNo)
        {
            DataSet ds = new DataSet();
            Type type = Type.GetType("Regentsoft.Plugin.Report.Bll." + moduleName + ",Regentsoft.Plugin.Report.Bll");
            if (type == null)
                return null;

            DataSet dsCondition = btCondition == null ? null : RegentZip.ZIP.DecompressDS(btCondition);

            object dObj = Activator.CreateInstance(type);
            //属性
            System.Reflection.PropertyInfo property = type.GetProperty("UserNo");
            property.SetValue(dObj, userNo, null);
            System.Reflection.PropertyInfo property1 = type.GetProperty("DsCondition");
            property1.SetValue(dObj, dsCondition, null);

            //获取方法的信息
            System.Reflection.MethodInfo method = type.GetMethod("Calculate");
            //调用方法的一些标志位，这里的含义是Public并且是实例方法，这也是默认的值
            System.Reflection.BindingFlags flag = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance;
            //调用方法，用一个object接收返回值
            object obj = method.Invoke(dObj, flag, null, null, null);

            if (obj != null)
            {
                ds = (DataSet)obj;
            }
            byte[] btDetailResult = null;
            if (ds != null)
                btDetailResult = RegentZip.ZIP.CompressDS(ds);

            return btDetailResult;
        }

        /// <summary>
        /// 取得指定页的数据
        /// </summary>
        /// <param name="reportTable"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageRecordCount"></param>
        /// <returns></returns>
        [WebMethod(Description = "取得指定页的数据")]
        [SoapHeader("Credentials")]
        public byte[] GetPageDataTable(string reportTable, int pageIndex, int pageRecordCount)
        {
            DataSet dsResult = Regent.Business.ReportBusiness.Generic.GetPageDataTable(reportTable, pageIndex, pageRecordCount);
            if (dsResult == null) return null;

            return RegentZip.ZIP.CompressDS(dsResult);
        }

        [WebMethod(Description = "删除实体临时表")]
        [SoapHeader("Credentials")]
        public void DropSolidTempTable(string reportTable)
        {
            Regent.Business.ReportBusiness.Generic.DropSolidTempTable(reportTable);
        }
    }
}
