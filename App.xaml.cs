using System.Configuration;
using System.Data;
using System.Windows;

namespace QuickIDEA
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // 设置进程的工作集限制
            using (var process = System.Diagnostics.Process.GetCurrentProcess())
            {
                process.MaxWorkingSet = new IntPtr(100 * 1024 * 1024); // 限制最大工作集为100MB
            }
        }
    }

}
