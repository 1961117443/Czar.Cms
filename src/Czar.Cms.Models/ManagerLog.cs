﻿// 本代码由代码生成器生成请勿随意改动
// 生成时间  2018-12-20 23:36:21
using System;

namespace Czar.Cms.Models
{
	/// <summary>
	/// suxiangnian
	/// 2018-12-20 23:36:21
	/// 操作日志
	/// </summary>
	public partial class ManagerLog
	{
		public Int32 Id {get;set;}

		/// <summary>
		/// 操作类型
		/// </summary>
		public String ActionType {get;set;}

		/// <summary>
		/// 主键
		/// </summary>
		public Int32 AddManageId {get;set;}

		/// <summary>
		/// 操作人名称
		/// </summary>
		public String AddManagerNickName {get;set;}

		/// <summary>
		/// 操作时间
		/// </summary>
		public DateTime AddTime {get;set;}

		/// <summary>
		/// 操作IP
		/// </summary>
		public String AddIp {get;set;}

		/// <summary>
		/// 备注
		/// </summary>
		public String Remark {get;set;}


	}
}