using System;
using System.Windows;
using GlobalHotKey;
using System.Windows.Input;

namespace InspirationRecorder
{
    public partial class MainWindow : Window
    {
        private HotKeyManager hotKeyManager;
        private readonly Config _config;
        private readonly IdeaService _ideaService;

        public MainWindow()
        {
            InitializeComponent();
            
            _config = Config.Load();
            _ideaService = new IdeaService(_config);
            
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
            if (IsAnyWindowFullScreen())
                return;

            var inputWindow = new InputWindow();
            inputWindow.ShowDialog();

            if (!string.IsNullOrWhiteSpace(inputWindow.InputText))
            {
                try
                {
                    _ideaService.SaveIdea(inputWindow.InputText);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"保存失败：{ex.Message}", "错误", 
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
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