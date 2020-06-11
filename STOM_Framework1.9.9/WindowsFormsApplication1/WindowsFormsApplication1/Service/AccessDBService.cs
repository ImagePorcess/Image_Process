using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Windows.Forms;
using Helper;
using System.Data;


namespace STOM.Service
{
    class AccessDBService
    {
        private string _connectionString;
        /// <summary>
        /// 数据量链接字符串,数据库密码为stomDB
        /// </summary>
        public string ConnectionString
        {
            get 
            {
                string path = System.Environment.CurrentDirectory + @"\DBStom.mdb";
                return _connectionString = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;jet oledb:database password={0};Data Source={1}", "stomDB", path);
            }
            
        }

        /// <summary>
        /// 执行SQL操作,并返回影响了多少行数据
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="cmdParameters"></param>
        /// <returns></returns>
        private int ExecuteNonQuery(string cmdText, params OleDbParameter[] cmdParameters)
        {
            return OleDbHelper.ExecuteNonQuery(ConnectionString, CommandType.Text, cmdText, cmdParameters);
        }

        /// <summary>
        /// 通过select查询并生成一个DataSet对象,用于绑定在DataView
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="cmdParameters"></param>
        /// <returns></returns>
        private DataSet ExecuteDataSet(string cmdText, params OleDbParameter[] cmdParameters )
        {
            return OleDbHelper.ExecuteDataset(ConnectionString, CommandType.Text, cmdText, cmdParameters);
        }

        /// <summary>
        /// 重载用户没有输入限制条件的查询
        /// </summary>
        /// <param name="cmdText"></param>
        /// <returns></returns>
        private DataSet ExecuteDataSet(string cmdText)
        {
            return OleDbHelper.ExecuteDataset(ConnectionString, CommandType.Text, cmdText, (OleDbParameter[])null);
        }
        

        /// <summary>
        /// 查询配置表
        /// </summary>
        /// <param name="table">表名</param>
        /// <returns>对应table</returns>
        public DataTable QueryTable(string table)
        {
            DataTable resTable = new DataTable();
            try
            {
                string cmdText = string.Format(@"SELECT * FROM {0}", table);
                resTable = ExecuteDataSet(cmdText).Tables[0];
            }
            catch(Exception ex)
            {
                MessageBox.Show( "配置表查询失败" + ex.Message);
            }

            return resTable;
        }
        
     
        
        /// <summary>
        /// 查询某表的某项属性值
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="restrictions">限制条件</param>
        /// <param name="restrictionsValue">限制条件值</param>
        /// <param name="attribute">要查询的属性名</param>
        /// <returns>只返回第一条数据</returns>
        public string QueryTableAttributeValues(string table, string attribute,string restrictions = null, string restrictionsValue = null)
        {
            string res = "";
            string cmdText = "";
            try
            {
                OleDbParameter[] param = new OleDbParameter[]{  
                                                           new OleDbParameter("restrictionsValue",restrictionsValue),
                                                          };

                if (restrictions == null)
                {
                    cmdText = string.Format(@"SELECT {0} FROM {1}", attribute, table);
                    res = ExecuteDataSet(cmdText).Tables[0].Rows[0][0].ToString();
                }
                else
                {
                    cmdText = string.Format(@"SELECT {0} FROM {1} WHERE `{2}`=@restrictionsValue", attribute, table, restrictions);
                    res = ExecuteDataSet(cmdText, param).Tables[0].Rows[0][0].ToString();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format(" 查询{0}表出错:{1}", table, ex.Message));
            }
            return res;
            
        }

        /// <summary>
        /// 更新某表的某项属性值
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="attribute">要修改的属性名</param>
        /// <param name="value">要修改的属性值</param>
        /// <param name="restrictions">约束条件</param>
        /// <param name="restrictionsValue">约束条件值</param>
        public void UpdateTableAttribute(string table, string attribute, object value, string restrictions = null, string restrictionsValue = null)
        {
            string cmdText = "";
            OleDbParameter[] param = new OleDbParameter[]{ 
                                                           new OleDbParameter("value",value),
                                                           new OleDbParameter("restrictions",restrictions),
                                                           new OleDbParameter("restrictionsValue",restrictionsValue)
                                                         };
            
            try
            {
                if (restrictions == null)
                {
                    cmdText = string.Format(@"UPDATE {0} SET `{1}` = {2}", table, attribute, value);
                    ExecuteNonQuery(cmdText);
                }
                else
                {
                    cmdText = string.Format(@"UPDATE {0} SET `{1}` = {2} WHERE `@restrictions`= @restrictionsValue", table, attribute, value);
                    ExecuteNonQuery(cmdText, param);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format(" 更新{0}表出错:{1}", table, ex.Message));
            }
           
           
        }


        /// <summary>
        /// 条件删除某表中某行数据
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="restriction">条件名</param>
        /// <param name="restrictionValue">条件值</param>
        public void DeleteTableData(string table, string restriction, string restrictionValue)
        {
            string cmdText = "";
            try
            {
                OleDbParameter[] param = new OleDbParameter[]{ 
                                                            new OleDbParameter("restriction",restriction),
                                                            new OleDbParameter("restrictionValue",restrictionValue)
                                                         };

                cmdText = string.Format(@"DELETE FROM `{0}` WHERE `@restriction`=@restrictionValue", table);
                ExecuteNonQuery(cmdText, param);
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format("从{0}删除数据失败,{1}", table, ex.Message));
            }

        }

        /// <summary>
        /// 往表中插入一条数据
        /// </summary>
        /// <param name="table">表名</param>
        /// <param name="attributeParam">属性名</param>
        /// <param name="valueParam">数据值</param>
        /// <returns>返回 -1 代表失败, 返回1代表成功</returns>
        public int InsertTableData(string table, string[] attributeParam ,string[] valueParam)
        {
            string cmdText =  @"INSERT TO " + table;
            int res = 0;

            try
            {
                if (attributeParam.Length != valueParam.Length)
                {
                    res = -1;
                }
                else
                {
                    for (int i = 0; i < attributeParam.Length; i++)
                    {
                        if (i == 0)
                        {
                            cmdText += "(`" + attributeParam[i] + "`,";
                        }
                        else if (i == attributeParam.Length - 1)
                        {
                            cmdText += "`" + attributeParam[i] + "`)";
                        }
                        else
                        {
                            cmdText += "`" + attributeParam[i] + "`,";
                        }

                    }

                    cmdText += "values";

                    for (int i = 0; i < valueParam.Length; i++)
                    {
                        if (i == 0)
                        {
                            cmdText += "(`" + valueParam[i] + "`,";
                        }
                        else if (i == valueParam.Length - 1)
                        {
                            cmdText += "`" + valueParam[i] + "`)";
                        }
                        else
                        {
                            cmdText += "`" + valueParam[i] + "`,";
                        }

                    }

                    res = ExecuteNonQuery(cmdText);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(string.Format("{0}插入一条数据出错,{1}", table, ex.Message));
            }
            

            return res;
           
        }

    }
}
