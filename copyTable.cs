
        public void yd_Move(GridView gv, DataSet ds, DataSet ds2, int tbid)
        {
            ds.AcceptChanges();
            int unit = 0;
            int[] r = gv.GetSelectedRows();
            try
            {
                for (int i = 0; i < r.Length; i++)
                {
                    unit = Convert.ToInt32(gv.GetRowCellValue(r[i], "unit"));
                    for (int j = 0; j < ds.Tables[tbid].Rows.Count; j++)
                    {
                        if (ds.Tables[tbid].Rows[j].RowState != DataRowState.Deleted)
                        {
                            if (unit == Convert.ToInt32(ds.Tables[tbid].Rows[j]["unit"]))
                            {
                                //  ds2.Tables[0].ImportRow(ds.Tables[tbid].Rows[j]);
                                //数据永远在第一个方便用法查看

                                DataRow dr = ds2.Tables[0].NewRow(); //创建一个新行
                                dr.ItemArray = ds.Tables[tbid].Rows[j].ItemArray;
                                ds2.Tables[0].Rows.InsertAt(dr, 0);
                                break;
                            }
                        }
                    }
                }




            }
            catch (Exception)
            {
                return;
            }
            gv.DeleteSelectedRows();
        }