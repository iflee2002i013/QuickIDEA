using System;
using System.Windows;
using GlobalHotKey;
using System.Windows.Input;

namespace InspirationRecorder
{
    public partial class MainWindow : Window
    {
        private HotKeyManager hotKeyManager;

        public MainWindow()
        {
            InitializeComponent();
            InitializeHotKey();
        }

        private void InitializeHotKey()
        {
            hotKeyManager = new HotKeyManager();
            
            // 注册Ctrl + Shift + I作为快捷键
            hotKeyManager.Register(Key.I, ModifierKeys.Control | ModifierKeys.Shift);
            
            hotKeyManager.KeyPressed += HotKeyManager_KeyPressed;
        }

        private void HotKeyManager_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            // 检查是否有全屏应用
            if (IsAnyWindowFullScreen())
                return;

            // 显示输入窗口
            var inputWindow = new InputWindow();
            inputWindow.ShowDialog();

            if (!string.IsNullOrWhiteSpace(inputWindow.InputText))
            {
                // TODO: 处理输入的文本
                MessageBox.Show(inputWindow.InputText);
            }
        }

        private bool IsAnyWindowFullScreen()
        {
            // 获取主屏幕尺寸
            var screenBounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            
            // 获取前台窗口句柄
            var foregroundWindow = NativeMethods.GetForegroundWindow();
            
            if (foregroundWindow != IntPtr.Zero)
            {
                NativeMethods.RECT rect;
                if (NativeMethods.GetWindowRect(foregroundWindow, out rect))
                {
                    // 检查窗口是否占据整个屏幕
                    return rect.Left <= 0 && rect.Top <= 0 &&
                           rect.Right >= screenBounds.Width &&
                           rect.Bottom >= screenBounds.Height;
                }
            }
            return false;
        }

        protected override void OnClosed(EventArgs e)
        {
            hotKeyManager?.Dispose();
            base.OnClosed(e);
        }
    }
}