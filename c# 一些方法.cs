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

        