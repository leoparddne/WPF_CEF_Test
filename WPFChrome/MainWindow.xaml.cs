using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFChrome.Common;
using WPFChrome.Inject;

namespace WPFChrome
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            browser.LifeSpanHandler = new OpenPage();
        }
        
        //private string[] wewbsites = new string[] { "http://127.0.0.1" };
        private string[] wewbsites = new string[] { "http://cnblogs.com/ives", "http://www.baidu.com", "http://www.csdn.net", "http://www.51cto.com" };
        private string getTestSite()
        {
            var index = (int)new Random().Next(0, wewbsites.Count());
            return wewbsites[index];
        }
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            StopLoadding();

            browser.Load(getTestSite());
        }
        /// <summary>
        /// 后退
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrev_Click_1(object sender, RoutedEventArgs e)
        {
            StopLoadding();

            if (IBrowser.CanGoBack)
            {
                IBrowser.GoBack();
            }
        }
        /// <summary>
        /// 前进
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            StopLoadding();

            if (IBrowser.CanGoForward)
            {
                IBrowser.GoForward();
            }
        }
        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            StopLoadding();
            IBrowser.Reload();
        }

        /// <summary>
        /// 停止搜索
        /// </summary>
        private void StopLoadding()
        {
            if (IBrowser.IsLoading)
            {
                IBrowser.StopLoad();
            }
        }
        /// <summary>
        /// 回车搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtRUL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var value = txtRUL.Text;
                if (value.Trim() == "")
                {
                    return;
                }
                if (!value.Contains("http") && !value.Contains("https"))
                {
                    value = "http://" + value;
                    txtRUL.Text = value;
                }
                StopLoadding();
                browser.Load(value);
            }
        }

        private IBrowser IBrowser
        {
            get
            {
                return browser.GetBrowser();
            }
        }
        private void ExecuteJS(string code)
        {
            IBrowser.ExcuteJS(code);
        }

        private void btnExecCode_Click(object sender, RoutedEventArgs e)
        {
            //IBrowser.GetSource(out string result); 
            //MessageBox.Show(result);
            var code = txtCode.Text.Trim();
            if (string.IsNullOrEmpty(code))
            {
                return;
            }
            ExecuteJS(code);
            //browser.regi
        }
        public void show(string mes)
        {
            MessageBox.Show(mes);
        }
        /// <summary>
        /// 窗口加载完毕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InjectMessages(); // 新版本的注入方式
        }

        /// <summary>
        /// 注入事件消息
        /// </summary>
        public void InjectMessages()
        {
            browser.JavascriptObjectRepository.ResolveObject += (s, eve) =>
            {
                var repo = eve.ObjectRepository;
                if (eve.ObjectName == "TestObj")
                {
                    repo.Register("TestObj", new TestClass(), isAsync: true, options: BindingOptions.DefaultBinder);
                }
            };
        }
        /// <summary>
        /// 处理全局事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.F12)
            //{
            //    // 页面加载完毕后打开开发者工具
            //    browser.FrameLoadEnd += (s, eve) =>
            //    {
            //        browser.ShowDevTools();
            //    };
            //}
            switch (e.Key)
            {
                case Key.F12:
                    browser.ShowDevTools();
                    break;
                default:
                    break;
            }
        }
    }
}
