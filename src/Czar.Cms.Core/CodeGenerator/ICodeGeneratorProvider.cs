using Czar.Cms.Core.Models;
using Czar.Cms.Core.Options;
using System.Collections.Generic;

namespace Czar.Cms.Core.CodeGenerator
{
    public interface ICodeGeneratorProvider
    {
        /// <summary>
        /// 从数据库获取表列表以及生成实体对象
        /// </summary>
        /// <param name="options"></param>
        /// <param name="isCoveredExsited"></param>
        /// <returns></returns>
        List<DbTable> GetModelFromDatabase(CodeGenerateOption options, bool isCoveredExsited = true);
    }
}