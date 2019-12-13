# WPF_CEF_Test
使用cefsharp.wpf开发的基本功能
安装 PM> Install-Package CefSharp.Wpf 
解决方案->属性->配置属性->活动解决方案平台—新建-x64

在需要使用的窗体上引用xmlns:cefSharp="clr-namespace:CefSharp.Wpf;assembly=CefSharp.Wpf"
重新生成解决方案
引用
<cefSharp:ChromiumWebBrowser Name="browser" Grid.Row="0" Address="http://www.baidu.com" Margin="10,43,10,10"/>

加载页面
var value="http://cnglogs.com/ives";
browser.Load(value);

获取IBrowser对象，对页面的前进、后退操作需要此对象
```
private IBrowser IBrowser
{
    get
    {
        return browser.GetBrowser();
    }
}

IBrowser.GoForward();//前进
IBrowser.Reload();//重载
IBrowser.StopLoad();//停止
IBrowser.GoBack();//后退
```
加载svg
安装：Install-Package SharpVectors -Version 1.0.0 
```
<Button  x:Name="btnPrev" HorizontalAlignment="Left" Width="32" Height="32" Margin="29,10,0,10"  Click="btnPrev_Click_1"  VerticalAlignment="Top">
            <svgc:SvgViewbox IsHitTestVisible="False"  Source="pack://application:,,,/resources/svg/arrow-left-2.svg"/>
</Button>
```
需要注意引用方式 Source="pack://application:,,,/1.svg" ，这样才能正确的引用资源。
svg 文件的属性默认是内容，务必改为  Resource

在同一个页面中打开链接
CefSharp中控制弹窗的接口是 ILifeSpanHandler，对OnBeforePopup进行重写
首先定义操作类
```
public class OpenPage : ILifeSpanHandler
    {
        public bool DoClose(IWebBrowser browserControl, IBrowser browser)
        {
            return false;
        }

        public void OnAfterCreated(IWebBrowser browserControl, IBrowser browser)
        {

        }

        public void OnBeforeClose(IWebBrowser browserControl, IBrowser browser)
        {

        }

        public bool OnBeforePopup(IWebBrowser browserControl, IBrowser browser, IFrame frame, string targetUrl,
            string targetFrameName, WindowOpenDisposition targetDisposition, bool userGesture, IPopupFeatures popupFeatures,
            IWindowInfo windowInfo, IBrowserSettings browserSettings, ref bool noJavascriptAccess, out IWebBrowser newBrowser)
        {
            newBrowser = null;
            var chromiumWebBrowser = (ChromiumWebBrowser)browserControl;
            chromiumWebBrowser.Load(targetUrl);
            return true; //Return true to cancel the popup creation copyright by codebye.com.
        }
    }
```
然后添加处理事件 browser.LifeSpanHandler = new OpenPage();

wpf与js互相调用、传值
首先创建测试类来处理前台js请求
```
public class TestClass
    {
        public int testFunc(string msg="test")
        {
            MessageBox.Show($"Get msg:{msg}");
            return 1;
        }
    }
```
将处理方法注入容器
```
private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            InjectMessages(); 
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
```
在前台绑定
```
<script>
            // 注入
            CefSharp.BindObjectAsync("TestObj");
            function callBackEnd(msg) {
                TestObj.testFunc(msg).then((r) => {
                    alert("Get result:"+r);
                })
            }
        </script>
```
