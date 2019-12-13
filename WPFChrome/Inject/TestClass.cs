using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFChrome.Inject
{
    public class TestClass
    {
        public int testFunc(string msg="test")
        {
            MessageBox.Show($"Get msg:{msg}");
            return 1;
        }
    }
}
