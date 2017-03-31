using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;





namespace Dapper的基本使用
{
    public class CommonHelp : dbConfig
    {
        #region comm
        /// <summary>
        /// 设置更新值Str
        /// </summary>
        /// <param name="updFiledValue"></param>
        /// <returns></returns>
        private string GetSetValuesStr(Dictionary<string, string> updFiledValue)
        {
            string retStr = string.Empty;
            foreach (var item in updFiledValue)
            {
                retStr = retStr + " [" + item.Key + "] =@" + item.Key + ",";
            }

            if (!String.IsNullOrEmpty(retStr))
            {
                retStr = retStr.TrimEnd(',');
            }
            return retStr;
        }

        /// <summary>
        /// 设置条件值Str(防注入啊)
        /// </summary>
        /// <param name="conditionFiledValue"></param>
        /// <returns></returns>
        private string GetSetConditionsStr(Dictionary<string, string> conditionFiledValue)
        {
            string retStr = string.Empty;
            foreach (var item in conditionFiledValue)
            {
                retStr = retStr + " [" + item.Key + "] =@" + item.Key + " and ";
            }

            if (!String.IsNullOrEmpty(retStr))
            {
                retStr = retStr.Substring(0, retStr.LastIndexOf(" and "));
            }

            return retStr;
        }
        /// <summary>
        /// 设置条件值Str(防注入啊)
        /// </summary>
        /// <param name="conditionFiledValue"></param>
        /// <returns></returns>
        private string GetSetConditionsLikeStr(Dictionary<string, string> conditionFiledValue)
        {
            string retStr = string.Empty;
            foreach (var item in conditionFiledValue)
            {
                retStr = retStr + " [" + item.Key + "] like @" + item.Key + " and ";
            }

            if (!String.IsNullOrEmpty(retStr))
            {
                retStr = retStr.Substring(0, retStr.LastIndexOf(" and "));
            }

            return retStr;
        }
        /// <summary>
        /// 设置条件值Str防止注入
        /// </summary>
        /// <param name="conditionFiledValue"></param>
        /// <returns></returns>
        private DynamicParameters GetSetParam(Dictionary<string, string> conditionFiledValue)
        {
            var parameters = new DynamicParameters();
            foreach (var item in conditionFiledValue)
            {
                parameters.Add(item.Key, item.Value);
            }
            return parameters;
        }
        #endregion

        #region 增
        /// <summary>
        /// 新增实体表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public long AddNewEntity<T>(object obj) where T : class,new()
        {

            long id = 0;
            try
            {
                using (IDbConnection conn = OpenConnection())
                {
                    id = conn.Insert<T>((T)obj);

                }
            }
            catch (Exception ex)
            {

            }
            return id;
        }

        /// <summary>
        /// 新增实体表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string AddNewEntityString<T>(object obj) where T : class,new()
        {

            string id = "";
            try
            {
                using (IDbConnection conn = OpenConnection())
                {
                    id = conn.Insert<T>((T)obj);

                }
            }
            catch (Exception ex)
            {

            }
            return id;
        }
        /// <summary>
        /// 新增实体表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool AddIEnumerable<T>(IEnumerable<T> obj) where T : class,new()
        {


            try
            {
                using (IDbConnection conn = OpenConnection())
                {
                    conn.Insert<T>(obj);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        #endregion


        #region 删

        /// <summary>
        /// 通过id删除某一条信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteInfoById(string tableName, long id)
        {
            try
            {
                using (IDbConnection conn = OpenConnection())
                {
                    string sqlStr = @"DELETE FROM " + tableName + " WHERE Id=@id";
                    int n = conn.Execute(sqlStr, new { id = id });
                    return n > 0;
                    //使用事物
                    //IDbTransaction transaction = conn.BeginTransaction();
                    //int row = conn.Execute(sqlStr, new { id = id }, transaction, null, null);
                    //row += conn.Execute(sqlStr, new { id = id }, transaction, null, null);
                    //transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                //log
                return false;
            }
        }
        /// <summary>
        /// 通过某个属性删除某一条信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="field"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool DeleteInfoByConditionField(string tableName, string field, string values)
        {
            bool flag = true;
            try
            {
                using (IDbConnection conn = OpenConnection())
                {
                    string sqlStr = @"DELETE " + tableName + " WHERE " + field + "=@values";
                    int n = conn.Execute(sqlStr, new { values = values });

                    return n > 0;
                }
            }
            catch (Exception ex)
            {

                flag = false;
            }
            return flag;
        }

        /// <summary>
        /// 根据多个条件删除
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="conditionFiledValue"></param>
        /// <returns></returns>
        public bool DeleteInfoByConditionFields(string tableName, Dictionary<string, string> conditionFiledValue)
        {
            try
            {
                using (IDbConnection conn = OpenConnection())
                {
                    string strSetConditions = this.GetSetConditionsStr(conditionFiledValue);
                    string sqlStr = @"DELETE FROM [" + tableName + "] where " + strSetConditions;
                    int n = conn.Execute(sqlStr, this.GetSetParam(conditionFiledValue));
                    return n > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 根据sql删除
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool DeleteTableBySql(string tableName, string sql)
        {
            bool bl = true;
            try
            {
                using (IDbConnection conn = OpenConnection())
                {
                    conn.Execute(sql);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

                bl = false;
            }
            return bl;
        }
        #endregion


        #region 改
        /// <summary>
        /// 更新实体表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool UpdateEntity<T>(object obj) where T : class,new()
        {
            bool flag = false;
            try
            {

                using (IDbConnection conn = OpenConnection())
                {
                    conn.Update<T>((T)obj);
                }
            }
            catch (Exception ex)
            {
                flag = false;
            }
            return flag;

        }

        /// <summary>
        /// 更新某个字段
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="field">字段</param>
        /// <param name="values">字段值</param>
        /// <returns></returns>
        public bool UpdateInfoOneAttribute(string tableName, string field, string values, string conditionFied, string conditionValues)
        {
            try
            {
                using (IDbConnection conn = OpenConnection())
                {
                    string sqlStr = @"update " + tableName + " set " + field + "=" + values + " where " + conditionFied + "=" + conditionValues;

                    bool result = conn.Update(sqlStr);
                    conn.Close();
                    return result;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        /// <summary>
        /// 根据多个条件，更新多个值
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="updFiledValue"></param>
        /// <param name="conditionFiledValue"></param>
        /// <returns></returns>
        public bool UpdateInfoByAttributes(string tableName, Dictionary<string, string> updFiledValue, Dictionary<string, string> conditionFiledValue)
        {
            try
            {
                using (IDbConnection conn = OpenConnection())
                {
                    string strSetValues = this.GetSetValuesStr(updFiledValue);
                    string strSetConditions = this.GetSetConditionsStr(conditionFiledValue);
                    string sqlStr = @"update [" + tableName + "] set " + strSetValues + " where " + strSetConditions;
                    foreach (var item in updFiledValue)
                    {
                        conditionFiledValue.Add(item.Key, item.Value);
                    }
                    int result = conn.Execute(sqlStr, GetSetParam(conditionFiledValue));
                    conn.Close();
                    return result > 0;
                }
            }
            catch (Exception ex)
            {

                return false;
            }
        }


        /// <summary>
        /// 根据sql更新
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool UpDateTableBySql(string tableName, string sql)
        {
            bool bl = true;
            try
            {
                using (IDbConnection conn = OpenConnection())
                {
                    conn.Execute(sql);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

                bl = false;
            }
            return bl;
        }
        #endregion


        #region 查
        /// <summary>
        /// 通过id获得某个表的所有信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<T> GetEntytyInfoById<T>(string tableName, long id) where T : new()
        {
            IList<T> comList = new List<T>();
            try
            {
                using (IDbConnection conn = OpenConnection())
                {
                    string sql = @"select * from [" + tableName + "]";
                    if (id > 0)
                    {
                        sql = @"select * from [" + tableName + "]   where Id=@id";
                    }

                    if (id > 0)
                    {
                        comList = conn.Query<T>(sql, new { id = id }).ToList();
                    }
                    else
                    {
                        comList = conn.Query<T>(sql).ToList();
                    }

                    conn.Close();

                }
            }
            catch (Exception ex)
            {
                //log
            }
            return comList;
        }

        /// <summary>
        /// 通过某个字段获得表的所有信息 
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<T> GetEntytyInfoByQuery<T>(string tableName, string field, string values) where T : new()
        {
            IList<T> comList = new List<T>();
            try
            {
                using (IDbConnection conn = OpenConnection())
                {
                    string sql = @"select * from [" + tableName + "]   where [" + field + "] =@field order by " + field + " desc";
                    comList = conn.Query<T>(sql, new { field = values }).ToList();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //log
            }
            return comList;
        }

        /// <summary>
        /// 根据Id 获得实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        public T GetEntityById<T>(string tableName, long Id) where T : new()
        {
            try
            {
                using (IDbConnection conn = OpenConnection())
                {
                    string sql = @"select * from [" + tableName + "]   where Id =@id order by Id desc";
                    T p = conn.Query<T>(sql, new { id = Id }).SingleOrDefault();
                    conn.Close();
                    return p;
                }

            }
            catch (Exception ex)
            {

                return new T();
            }

        }

        /// <summary>
        /// 根据Id查询实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public T GetEntityByConditionField<T>(string tableName, string field, string values) where T : new()
        {
            IList<T> comList = new List<T>();
            try
            {
                using (IDbConnection conn = OpenConnection())
                {
                    string sql = @"select * from [" + tableName + "]   where " + field + "=@field";
                    T p = conn.Query<T>(sql, new { field = field }).SingleOrDefault();
                    conn.Close();
                    return p;
                }
            }
            catch (Exception ex)
            {
                //
            }
            if (comList.Count > 0)
            {
                return comList[0];
            }
            else
            {
                return new T();
            }
        }
        /// <summary>
        /// 根据sql 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IList<T> GetEntytyInfoBySQL<T>(string sql) where T : new()
        {
            IList<T> comList = new List<T>();
            try
            {
                using (IDbConnection conn = OpenConnection())
                {

                    comList = conn.Query<T>(sql).ToList();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //log
            }
            return comList;
        }


        /// <summary>
        /// 获得实体类的单个值
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="selectValue">要查询的值</param>
        /// <param name="field">条件字段</param>
        /// <param name="values">条件值</param>
        /// <returns></returns>
        public string GetEntyOneValue(string tableName, string selectValue, string field, string values)
        {
            string str = string.Empty;
            try
            {
                using (IDbConnection conn = OpenConnection())
                {
                    string sql = @"select " + selectValue + " cs from [" + tableName + "]   where " + field + "=@field";


                    str = conn.QueryFirst(sql, new { field = values });
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //log
            }
            return str;
        }


        /// <summary>
        /// 查询表中数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="orderby"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public IList<T> GetEntityInfoList<T>(string tableName)
        {
            return this.GetEntityInfoList<T>(tableName, string.Empty, true);
        }

        /// <summary>
        /// 查询表中数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="orderby"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        public IList<T> GetEntityInfoList<T>(string tableName, string orderby, bool desc)
        {
            IList<T> comList = new List<T>();
            try
            {
                using (IDbConnection conn = OpenConnection())
                {
                    string sql = @"select * from [" + tableName + "]";
                    if (!String.IsNullOrEmpty(orderby))
                    {
                        sql = @" select * from [" + tableName + "]   Order by [" + orderby + "] ";
                        if (desc == true)
                        {
                            sql = sql + " Desc";
                        }
                    }

                    comList = conn.Query<T>(sql).ToList();


                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //log
            }
            return comList;
        }

        /// <summary>
        /// 根据字段按照In查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tableName"></param>
        /// <param name="filed"></param>
        /// <param name="inValues"></param>
        /// <returns></returns>
        public IList<T> GetEntityInfoByFiledIn<T>(string tableName, string filed, string inValues) where T : new()
        {
            IList<T> comList = new List<T>();
            try
            {
                using (IDbConnection conn = OpenConnection())
                {
                    string sql = @"select * from [" + tableName + "]   where [" + filed + "] in (" + inValues + ") order by id desc";

                    comList = conn.Query<T>(sql).ToList();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //log
            }
            return comList;
        }


        /// <summary>
        /// 获得实体类的单个值--sql中必须将要查到的值重命名为cs
        /// </summary>
        /// <returns></returns>
        public string GetEntyOneValue(string sql, string tableName)
        {
            string str = string.Empty;
            try
            {
                using (IDbConnection conn = OpenConnection())
                {

                    str = conn.QueryFirst(sql);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                //log
            }
            return str;
        }



        /// <summary>
        /// 分页根据条件获得表的所有信息
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IList<T> GetEntityInfoByQueryWithPager<T>(string tableName, Dictionary<string, string> conditionFiledValue, Dictionary<string, string> conditionFiledLikeValue, int currentPage, int pageSize, string orderByFiled, bool desc) where T : new()
        {
            IList<T> comList = new List<T>();
            try
            {
                using (IDbConnection conn = OpenConnection())
                {
                    int startNum = (currentPage - 1) * pageSize + 1;
                    int endNum = (startNum + pageSize) - 1;
                    string orderStr = " ORDER BY [" + orderByFiled + "] ";
                    if (desc == true)
                    {
                        orderStr = orderStr + " DESC ";
                    }
                    string strSetConditions = "";
                    string strSetConditionsLike = "";
                    if (conditionFiledValue != null)
                    {
                        strSetConditions = this.GetSetConditionsStr(conditionFiledValue);
                    }
                    else
                    {
                        strSetConditions = " 1 = 1 ";
                        conditionFiledValue = new Dictionary<string, string>();
                    }
                    if (strSetConditionsLike != null)
                    {
                        strSetConditionsLike = this.GetSetConditionsLikeStr(conditionFiledLikeValue);
                        foreach (var item in conditionFiledLikeValue)
                        {
                            conditionFiledValue.Add(item.Key, item.Value);
                        }
                    }
                    else
                    {
                        strSetConditionsLike = " 1 = 1 ";
                    }



                    string sql = @"select * from  (
			                       SELECT ROW_NUMBER() OVER(" + orderStr + ") as [row_number],* FROM [" + tableName + "] Where " + strSetConditions + " and " + strSetConditionsLike + " )  ";
                    sql = sql + " t where [row_number] BETWEEN " + startNum.ToString() + " and " + endNum.ToString();
                    if (conditionFiledValue != null)
                    {
                        comList = conn.Query<T>(sql, GetSetParam(conditionFiledValue)).ToList();
                    }
                    else
                    {
                        comList = conn.Query<T>(sql).ToList();

                    }
                    conn.Close();

                }
            }
            catch (Exception ex)
            {
                //
            }
            return comList;
        }




        #endregion
    }
}
