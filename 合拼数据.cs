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
            DataTable dt = new DataTable();
       
            //要 合拼的列
            dt.Columns.Add("unit1", typeof(string));//货号
            dt.Columns.Add("accdaishou1", typeof(decimal));//代收
            dt.Columns.Add("accarrived1", typeof(string));//运费
            dt.Columns.Add("acctf1",typeof(decimal));//提付
            dt.Columns.Add("accqt1", typeof(decimal));  //其他运费
             

            dt.Columns.Add("unit2", typeof(string));
            dt.Columns.Add("accdaishou2", typeof(decimal));
            dt.Columns.Add("accarrived2", typeof(string));
            dt.Columns.Add("acctf2", typeof(decimal));//提付
            dt.Columns.Add("accqt2", typeof(decimal));  //其他运费

            dt.Columns.Add("unit3", typeof(string));
            dt.Columns.Add("accdaishou3", typeof(decimal));
            dt.Columns.Add("accarrived3", typeof(string));
            dt.Columns.Add("acctf3", typeof(decimal));//提付
            dt.Columns.Add("accqt3", typeof(decimal));  //其他运费


            dt.Columns.Add("unit4", typeof(string));
            dt.Columns.Add("accdaishou4", typeof(decimal));
            dt.Columns.Add("accarrived4", typeof(string));
            dt.Columns.Add("acctf4", typeof(decimal));//提付
            dt.Columns.Add("accqt4", typeof(decimal));  //其他运费

            //固定的列

            dt.Columns.Add("esite", typeof(string));//到站
            dt.Columns.Add("fetchdate", typeof(DateTime));//提货日期
            dt.Columns.Add("fetchmadeby", typeof(string));//提货人


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
                        string a = ds.Tables[0].Rows[m]["unit"].ToString(); //看最大数是否蹦
                    }
                    catch (Exception)
                    {
                     

                       if (count-j==3)
                        {

                            dt.Rows.Add(new object[] {
                                      ds.Tables[0].Rows[j]["unit"],ds.Tables[0].Rows[j]["accdaishou"], ds.Tables[0].Rows[j]["accarrived"], ds.Tables[0].Rows[j]["acctf"],ds.Tables[0].Rows[j]["accqt"],
                                      ds.Tables[0].Rows[j+1]["unit"],ds.Tables[0].Rows[j+1]["accdaishou"], ds.Tables[0].Rows[j+1]["accarrived"], ds.Tables[0].Rows[j+1]["acctf"],ds.Tables[0].Rows[j+1]["accqt"],
                                      ds.Tables[0].Rows[j+2]["unit"],ds.Tables[0].Rows[j+2]["accdaishou"], ds.Tables[0].Rows[j+2]["accarrived"],ds.Tables[0].Rows[j+2]["acctf"],ds.Tables[0].Rows[j+2]["accqt"],
                                      //占个位(保持位置不变)
                               
                                       "",0,"",0,0,
                                      //增加固定列

                                      ds.Tables[0].Rows[0]["esite"],ds.Tables[0].Rows[0]["fetchdate"],ds.Tables[0].Rows[0]["fetchmadeby"] 
                                   


                                       });
                        }
                       else if (count - j == 2)
                       {
                           dt.Rows.Add(new object[] {
                                      ds.Tables[0].Rows[j]["unit"],ds.Tables[0].Rows[j]["accdaishou"], ds.Tables[0].Rows[j]["accarrived"], ds.Tables[0].Rows[j]["acctf"],ds.Tables[0].Rows[j]["accqt"],
                                      ds.Tables[0].Rows[j+1]["unit"],ds.Tables[0].Rows[j+1]["accdaishou"], ds.Tables[0].Rows[j+1]["accarrived"], ds.Tables[0].Rows[j+1]["acctf"],ds.Tables[0].Rows[j+1]["accqt"],
                                      //占个位(保持位置不变)
                                      "",0,"",0,0,
                                      "",0,"",0,0,
                                      //增加固定列
                                      ds.Tables[0].Rows[0]["esite"],ds.Tables[0].Rows[0]["fetchdate"],ds.Tables[0].Rows[0]["fetchmadeby"]  
                                       });
                       }
                       else if (count - j == 1)
                       {
                           dt.Rows.Add(new object[] {
                                      ds.Tables[0].Rows[j]["unit"],ds.Tables[0].Rows[j]["accdaishou"], ds.Tables[0].Rows[j]["accarrived"], ds.Tables[0].Rows[j]["acctf"],ds.Tables[0].Rows[j]["accqt"],
                                      //占个位(保持位置不变)
                                      "",0,"",0,0,
                                      "",0,"",0,0,
                                      "",0,"",0,0,
                                        //增加固定列
                                      ds.Tables[0].Rows[0]["esite"],ds.Tables[0].Rows[0]["fetchdate"],ds.Tables[0].Rows[0]["fetchmadeby"]  
                                    
                                       });
                       }
                       else
                       {
                           dt.Rows.Add(new object[] {
                                      ds.Tables[0].Rows[j]["unit"],ds.Tables[0].Rows[j]["accdaishou"], ds.Tables[0].Rows[j]["accarrived"], ds.Tables[0].Rows[j]["acctf"],ds.Tables[0].Rows[j]["accqt"],
                                      ds.Tables[0].Rows[j+1]["unit"],ds.Tables[0].Rows[j+1]["accdaishou"], ds.Tables[0].Rows[j+1]["accarrived"], ds.Tables[0].Rows[j+1]["acctf"],ds.Tables[0].Rows[j+1]["accqt"],
                                      ds.Tables[0].Rows[j+2]["unit"],ds.Tables[0].Rows[j+2]["accdaishou"], ds.Tables[0].Rows[j+2]["accarrived"],ds.Tables[0].Rows[j+2]["acctf"],ds.Tables[0].Rows[j+2]["accqt"],
                                      ds.Tables[0].Rows[j+3]["unit"],ds.Tables[0].Rows[j+3]["accdaishou"], ds.Tables[0].Rows[j+3]["accarrived"],ds.Tables[0].Rows[j+3]["acctf"],ds.Tables[0].Rows[j+3]["accqt"],

                                      //增加固定列
                                       ds.Tables[0].Rows[0]["esite"],ds.Tables[0].Rows[0]["fetchdate"],ds.Tables[0].Rows[0]["fetchmadeby"]  
                                       });

                       }
                            
                    }
                  
                   
                    dt.Rows.Add(new object[] {
                                                    
                            ds.Tables[0].Rows[j]["unit"],ds.Tables[0].Rows[j]["accdaishou"], ds.Tables[0].Rows[j]["accarrived"],ds.Tables[0].Rows[j]["acctf"],ds.Tables[0].Rows[j]["accqt"],
              
                            ds.Tables[0].Rows[j+1]["unit"],ds.Tables[0].Rows[j+1]["accdaishou"], ds.Tables[0].Rows[j+1]["accarrived"],ds.Tables[0].Rows[j+1]["acctf"],ds.Tables[0].Rows[j+1]["accqt"],

                            ds.Tables[0].Rows[j+2]["unit"],ds.Tables[0].Rows[j+2]["accdaishou"], ds.Tables[0].Rows[j+2]["accarrived"],ds.Tables[0].Rows[j+2]["acctf"],ds.Tables[0].Rows[j+2]["accqt"],

                            ds.Tables[0].Rows[j+3]["unit"],ds.Tables[0].Rows[j+3]["accdaishou"], ds.Tables[0].Rows[j+3]["accarrived"],ds.Tables[0].Rows[j+3]["acctf"],ds.Tables[0].Rows[j+3]["accqt"],


                            //增加固定列

                            ds.Tables[0].Rows[0]["esite"],ds.Tables[0].Rows[0]["fetchdate"],ds.Tables[0].Rows[0]["fetchmadeby"]  
 
                        });



                    j = j + 4;
                    e = e + 1;

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