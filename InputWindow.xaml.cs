using System.Windows;
using System.Windows.Input;

namespace InspirationRecorder
{
    public partial class InputWindow : Window, IDisposable
    {
        private readonly Config _config;
        private bool _disposed = false;

        public InputWindow(Config config)
        {
            InitializeComponent();
            _config = config;
            
            // 注册按键事件
            this.KeyDown += InputWindow_KeyDown;
            
            // 设置焦点到输入框
            this.Loaded += (s, e) => InputTextBox.Focus();
        }

        private void InputWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !Keyboard.IsKeyDown(Key.LeftShift))
            {
                // 普通回车保存并关闭
                this.Close();
            }
            else if (e.Key == Key.Escape)
            {
                // ESC取消
                this.Close();
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            // 暂时隐藏输入窗口
            this.Hide();
            
            var settingsWindow = new SettingsWindow(_config);
            settingsWindow.ShowDialog();
            
            // 设置窗口关闭后重新显示输入窗口
            this.Show();
            // 重新获取焦点
            this.Activate();
            InputTextBox.Focus();
        }

        public string InputText
        {
            get { return InputTextBox.Text; }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                this.KeyDown -= InputWindow_KeyDown;
                _disposed = true;
            }
        }
    }
} 