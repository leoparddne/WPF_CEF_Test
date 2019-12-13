using CefSharp;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFChrome.Common
{
    public static class CEFSharpEx
    {
        /// <summary>
        /// 执行js代码
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="code"></param>
        public static void ExcuteJS(this IBrowser browser, string code)
        {
            browser.MainFrame.ExecuteJavaScriptAsync(code);
        }

        /// <summary>
        /// 获取网页源码
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="result"></param>
        public static void GetSource(this IBrowser browser,out string result)
        {
            result = browser.MainFrame.GetSourceAsync().Result;
        }
    }
}
