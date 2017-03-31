using Dapper的基本使用.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dapper的基本使用
{
    class Program
    {

        static void Main(string[] args)
        {
            CommonHelp comm = new CommonHelp();
            IList<TempLicense> temlist = comm.GetEntytyInfoByQuery<TempLicense>("TempLicense", "ProName", "恒大林溪郡御苑");
            temlist = comm.GetEntytyInfoBySQL<TempLicense>("select * from TempLicense");
            TempLicense te = new TempLicense();
            te.ID = Guid.NewGuid().ToString();
            te.KeyWord = "12";
            comm.AddNewEntityString<TempLicense>(te);
            te.KeyWord = "1111111111111";
            comm.UpdateEntity<TempLicense>(te);

            comm.DeleteInfoByConditionField("TempLicense", "Id", "2f9561d3-8941-48ca-8454-4b317c282ece");

            comm.GetEntityInfoList<TempLicense>("TempLicense");
            Dictionary<string, string> dc1 = new Dictionary<string, string>();
            dc1.Add("KeyWord", "1111111111111");
            comm.DeleteInfoByConditionFields("TempLicense", dc1);
            //删除
            Dictionary<string, string> dc = new Dictionary<string, string>();
            dc.Add("ProName", "%当代滨江苑%");
            IList<TempLicense> temlist1 = comm.GetEntityInfoByQueryWithPager<TempLicense>("TempLicense", null,dc, 1, 10, "SpiderDate",true);
            IList<TempLicense> aa = new List<TempLicense>();
            TempLicense a = new TempLicense();
            for (int i = 0; i < 999999; i++)
            {
                a.ID = Guid.NewGuid().ToString();
                aa.Add(a);
            }

            comm.AddIEnumerable<TempLicense>(aa);

        }

    }
}
