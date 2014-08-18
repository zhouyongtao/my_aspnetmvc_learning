using System;
using System.Web;
/*
 IIS6
 <httpModules>
    <add name="ErrorHttpModule" type="Homeinns.Common.Web.HttpModule.ErrorHttpModule"/>
</httpModules>

 IIS7
   <!--兼容集成与经典模式-->
    <validation validateIntegratedModeConfiguration="false"/>
    <!--自定义HttpModule  -->
    <modules runAllManagedModulesForAllRequests="true">
      <add name="UrlRoutingModule" type="System.Web.Routing.UrlRoutingModule, System.Web, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" />
      <add name="ErrorHttpModule"  type="Homeinns.Common.Web.HttpModule.ErrorHttpModule"/>
    </modules>
 */
namespace Homeinns.Common.Web.HttpModule
{
    /// <summary>
    /// 处理全局错误
    /// </summary>
    public class ErrorHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
            context.Error += context_Error;
        }
        /// <summary>
        /// 开始请求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_BeginRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;
            string url = app.Request.RawUrl;
        }
        /// <summary>
        /// 全局错误信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_Error(object sender, EventArgs e)
        {
            HttpException ex = HttpContext.Current.Server.GetLastError() as HttpException;
            string errorMsg = ex.ToString();
            if (ex != null && ex.GetHttpCode() == 404)
            {
                // HttpContext.Current.Server.ClearError();

            }

        }

        public void Dispose()
        {
           
        }
    }
}
