using System.Windows;
using System.Windows.Input;

namespace InspirationRecorder
{
    public partial class InputWindow : Window
    {
        public InputWindow()
        {
            InitializeComponent();
            
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

        public string InputText
        {
            get { return InputTextBox.Text; }
        }
    }
} 