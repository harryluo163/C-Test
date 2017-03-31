using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dapper的基本使用
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class DapperIgnoreAttribute : System.Attribute
    {
        public string _DBTableName { get; set; }//数据表名  
        public string _DBPropety { get; set; }//字段
        public DapperIgnoreAttribute(string _DBTableName)
        { 
        
        }
    }
}
