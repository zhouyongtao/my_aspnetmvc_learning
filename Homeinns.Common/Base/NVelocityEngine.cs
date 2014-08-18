using System.Web;
using System.IO;
using NVelocity;
using NVelocity.App;
using NVelocity.Context;
using NVelocity.Runtime;
using Commons.Collections;
using Homeinns.Common.Util;

/**
 * 版权所有 All Rights Reserved
 *
 * @author Irving_Zhou
 * @description NVelocity模板工具类 NVelocityEngine
 * @date 2013年10月24日15:11:29
 * @version 1.0.0
 * @email zhouyongtao@outlook.com
 * @blog http://www.cnblogs.com/Irving/
 */
namespace Homeinns.Common.Base
{
    public class NVelocityEngine
    {
        private VelocityEngine velocity = null;
        private IContext context = null;

        /// <summary>
        /// 无参数构造函数(使用默认虚拟路径)
        /// </summary>
        public NVelocityEngine()
        {
            Init(DBSetting.getAppText("templateDir"), false);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="templatDir">模板文件夹路径</param>
        /// <param name="isPhyPath">是否为物理路径</param>
        public NVelocityEngine(string templatDir, bool isPhyPath)
        {
            Init(templatDir, isPhyPath);
        }

        /// <summary>
        /// 初始话NVelocity模块
        /// </summary>
        /// <param name="templatDir">模板文件夹路径</param>
        private void Init(string templatDir, bool isPhyPath)
        {
            //创建VelocityEngine实例对象
            velocity = new VelocityEngine();

            //使用设置初始化VelocityEngine
            ExtendedProperties props = new ExtendedProperties();
            string path = string.Empty;
            if (isPhyPath)
            {
                path = templatDir;
            }
            else
            {
                path = HttpContext.Current.Server.MapPath(templatDir);
            }
            props.AddProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, path);
            props.AddProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            props.AddProperty(RuntimeConstants.INPUT_ENCODING, "gb2312");
            props.AddProperty(RuntimeConstants.OUTPUT_ENCODING, "gb2312");
            velocity.Init(props);

            //为模板变量赋值
            context = new VelocityContext();
        }
        /// <summary>
        /// 给模板变量赋值
        /// </summary>
        /// <param name="key">模板变量</param>
        /// <param name="value">模板变量值</param>
        public void Put(string key, object value)
        {
            if (context == null)
                context = new VelocityContext();
            context.Put(key, value);
        }

        /// <summary>
        /// HttpContext输出模板
        /// </summary>
        /// <param name="templatFileName">模板名</param>
        public void Response(string templatName)
        {
            using (StringWriter writer = new StringWriter())
            {
                //从文件中读取模板
                Template template = velocity.GetTemplate(templatName);
                //合并模板
                template.Merge(context, writer);
                //输出内容
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(writer.ToString());
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// 获得模板内容
        /// </summary>
        /// <param name="templatName">模板名</param>
        /// <returns></returns>
        public string GetTemplateBody(string templatName)
        {
            using (StringWriter writer = new StringWriter())
            {
                Template template = velocity.GetTemplate(templatName);
                template.Merge(context, writer);
                return writer.ToString();
            }
        }
    }
}
