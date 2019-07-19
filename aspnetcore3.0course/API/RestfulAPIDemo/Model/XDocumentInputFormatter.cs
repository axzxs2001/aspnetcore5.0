using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RestfulAPIDemo.Model
{
    /// <summary>
    /// xml 转换
    /// </summary>
    public class XDocumentInputFormatter : InputFormatter, IInputFormatter, IApiRequestFormatMetadataProvider
    {
        /// <summary>
        /// 构造
        /// </summary>
        public XDocumentInputFormatter()
        {
            SupportedMediaTypes.Add("application/xml");
        }
        /// <summary>
        /// 是否可读类型
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns></returns>
        protected override bool CanReadType(Type type)
        {
            if (type.IsAssignableFrom(typeof(XDocument)))
            {
                return true;
            }
            return base.CanReadType(type);
        }
        /// <summary>
        /// 读取请求体
        /// </summary>
        /// <param name="context">输入上下文</param>
        /// <returns></returns>
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var xmlDoc = await XDocument.LoadAsync(context.HttpContext.Request.Body, LoadOptions.None, CancellationToken.None);
            return InputFormatterResult.Success(xmlDoc);
        }
    }
}
