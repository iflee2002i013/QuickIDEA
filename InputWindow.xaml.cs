using System.Windows;
using System.Windows.Input;

namespace InspirationRecorder
{
    public partial class InputWindow : Window
    {
        private readonly Config _config;

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
            var settingsWindow = new SettingsWindow(_config);
            settingsWindow.ShowDialog();
        }

        public string InputText
        {
            get { return InputTextBox.Text; }
        }
    }
} 