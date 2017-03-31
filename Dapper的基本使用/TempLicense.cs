using DapperExtensions.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace Dapper的基本使用.Model
{
	/// <summary>
	/// TempLicense:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class TempLicense
	{
		public TempLicense()
		{
        }
		#region Model
		private string _id;
		private string _keyword;
		private string _pkeyword;
		private string _spiderdate;
		private string _urlstr;
		private string _urltype;
		private string _para;
		private string _license;
		private string _cerdate;
		private string _proname;
		private string _address;
		private string _batchurl;
		/// <summary>
		/// 
		/// </summary>
		public string ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string KeyWord
		{
			set{ _keyword=value;}
			get{return _keyword;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PKeyWord
		{
			set{ _pkeyword=value;}
			get{return _pkeyword;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SpiderDate
		{
			set{ _spiderdate=value;}
			get{return _spiderdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UrlStr
		{
			set{ _urlstr=value;}
			get{return _urlstr;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UrlType
		{
			set{ _urltype=value;}
			get{return _urltype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Para
		{
			set{ _para=value;}
			get{return _para;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string License
		{
			set{ _license=value;}
			get{return _license;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CerDate
		{
			set{ _cerdate=value;}
			get{return _cerdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ProName
		{
			set{ _proname=value;}
			get{return _proname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BatchUrl
		{
			set{ _batchurl=value;}
			get{return _batchurl;}
		}

        #region 新加临时字段
       [System.Xml.Serialization.XmlIgnore]

       [SoapIgnore]
        public string New_id { get; set; }

        #endregion
        #endregion Model

        /// <summary>
        /// TempLicense：实体对象映射关系  
        /// </summary>
        [Serializable]
        public class UsersEntityORMMapper : ClassMapper<TempLicense>
        {

            public UsersEntityORMMapper()
            {

                base.Table("TempLicense");

                Map(f => f.New_id).Ignore();//设置忽略

                //Map(f => f.Name).Key(KeyType.Identity);//设置主键  (如果主键名称不包含字母“ID”，请设置)      

                AutoMap();

            }

        }

		

	}
}

