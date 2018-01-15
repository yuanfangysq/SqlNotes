       /// <summary>
        /// 取得选中的记录
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="selectFieldName">选择字段名</param>
        /// <returns></returns>
        public static DataTable GetDataTableSelectData(DataTable dt, string selectFieldName)
        {
            if (dt == null)
                return null;

            DataView dv = new DataView(dt);
            dv.RowFilter = string.Format("{0} = 1", selectFieldName);

            return dv.ToTable();
        }

        //反射
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

        