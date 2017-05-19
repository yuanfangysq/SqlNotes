     /// <summary>
        /// 合拼数据
        /// </summary>
        /// <param name="ReportPath"></param>
        /// <param name="procName"></param>
        /// <param name="pars"></param>
        /// <param name="ShowPrintDialog"></param>
        /// <param name="a"></param>
        public void ydPrint(string ReportPath, string procName, SqlParameter[] pars, bool ShowPrintDialog)
        {
            //要合拼这种格式
            DataTable dt = new DataTable();
            dt.Columns.Add("unit1", typeof(Int32));
            dt.Columns.Add("accdaishou1", typeof(decimal));
            dt.Columns.Add("accarrived1", typeof(decimal));

            dt.Columns.Add("unit2", typeof(Int32));
            dt.Columns.Add("accdaishou2", typeof(decimal));
            dt.Columns.Add("accarrived2", typeof(decimal));

            dt.Columns.Add("unit3", typeof(Int32));
            dt.Columns.Add("accdaisho3", typeof(decimal));
            dt.Columns.Add("accarrived3", typeof(decimal));

            dt.Columns.Add("unit4", typeof(Int32));
            dt.Columns.Add("accdaishou4", typeof(decimal));
            dt.Columns.Add("accarrived4", typeof(decimal));
            
            try
            {
                DataSet ds = new DataSet();

                ds = cs.FILLDS_LOAD(procName, pars).Copy();
                if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    return;
                }

                int count = ds.Tables[0].Rows.Count;

                int f = 4;
                int e = 1;

                int m;
                for (int j = 0; j < count; j=j)
                {

                    m = f * e;
                    try
                    {
                        string a = ds.Tables[0].Rows[m]["unit"].ToString(); //看最大的数是否蹦
                    }
                    catch (Exception)
                    {

                        if (count - j == 3)
                        {

                            dt.Rows.Add(new object[] {
                                      ds.Tables[0].Rows[j]["unit"],ds.Tables[0].Rows[j]["accdaishou"], ds.Tables[0].Rows[j]["accarrived"],
                                      ds.Tables[0].Rows[j+1]["unit"],ds.Tables[0].Rows[j+1]["accdaishou"], ds.Tables[0].Rows[j+1]["accarrived"],
                                      ds.Tables[0].Rows[j+2]["unit"],ds.Tables[0].Rows[j+2]["accdaishou"], ds.Tables[0].Rows[j+2]["accarrived"]
                                       });
                        }
                        else if (count - j == 2)
                        {
                            dt.Rows.Add(new object[] {
                                      ds.Tables[0].Rows[j]["unit"],ds.Tables[0].Rows[j]["accdaishou"], ds.Tables[0].Rows[j]["accarrived"],
                                      ds.Tables[0].Rows[j+1]["unit"],ds.Tables[0].Rows[j+1]["accdaishou"], ds.Tables[0].Rows[j+1]["accarrived"]
                                       });
                        }
                        else if (count - j == 1)
                        {
                            dt.Rows.Add(new object[] {
                                      ds.Tables[0].Rows[j]["unit"],ds.Tables[0].Rows[j]["accdaishou"], ds.Tables[0].Rows[j]["accarrived"]
                                    
                                       });
                        }
                        else
                        {
                            dt.Rows.Add(new object[] {
                                      ds.Tables[0].Rows[j]["unit"],ds.Tables[0].Rows[j]["accdaishou"], ds.Tables[0].Rows[j]["accarrived"],
                                      ds.Tables[0].Rows[j+1]["unit"],ds.Tables[0].Rows[j+1]["accdaishou"], ds.Tables[0].Rows[j+1]["accarrived"],
                                      ds.Tables[0].Rows[j+2]["unit"],ds.Tables[0].Rows[j+2]["accdaishou"], ds.Tables[0].Rows[j+2]["accarrived"],
                                      ds.Tables[0].Rows[j+3]["unit"],ds.Tables[0].Rows[j+3]["accdaishou"], ds.Tables[0].Rows[j+3]["accarrived"]

                                       });

                        }

                    }
                  
                   
                    dt.Rows.Add(new object[] {
                                                    
                            ds.Tables[0].Rows[j]["unit"],ds.Tables[0].Rows[j]["accdaishou"], ds.Tables[0].Rows[j]["accarrived"],
              
                            ds.Tables[0].Rows[j+1]["unit"],ds.Tables[0].Rows[j+1]["accdaishou"], ds.Tables[0].Rows[j+1]["accarrived"],

                            ds.Tables[0].Rows[j+2]["unit"],ds.Tables[0].Rows[j+2]["accdaishou"], ds.Tables[0].Rows[j+2]["accarrived"],

                            ds.Tables[0].Rows[j+3]["unit"],ds.Tables[0].Rows[j+3]["accdaishou"], ds.Tables[0].Rows[j+3]["accarrived"]
 
                        });


                    j = j + 4; //记录插入了多少行数据

                    e = e + 1; //记录循环次数

                }
               

            }
            catch (Exception ex)
            {
               // XtraMessageBox.Show(ex.Message, "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Print(ReportPath, dt, ShowPrintDialog);

                string a = "11";
            }
        }
