using System;
using System.Collections.Generic;
using System.Text;

namespace Czar.Cms.ViewModels
{
    public class ManagerRoleAddOrModifyModel
    {
        /// <summary>
		/// 主键
		/// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// 角色名称
        public String RoleName { get; set; }

        /// <summary>
        /// 角色类型1超管2系管
        /// </summary>
        public Int32 RoleType { get; set; }

        /// <summary>
        /// 是否系统默认
        /// </summary>
        public Boolean IsSystem { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; set; }
    }
}
