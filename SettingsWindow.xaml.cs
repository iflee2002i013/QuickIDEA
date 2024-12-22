using Microsoft.Win32;
using System.Windows;

namespace InspirationRecorder
{
    public partial class SettingsWindow : Window
    {
        private readonly Config _config;

        public SettingsWindow(Config config)
        {
            InitializeComponent();
            _config = config;
            
            // 显示当前配置
            PathTextBox.Text = _config.MarkdownFilePath;
        }

        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Markdown 文件|*.md|所有文件|*.*",
                FileName = System.IO.Path.GetFileName(_config.MarkdownFilePath),
                InitialDirectory = System.IO.Path.GetDirectoryName(_config.MarkdownFilePath)
            };

            if (dialog.ShowDialog() == true)
            {
                PathTextBox.Text = dialog.FileName;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _config.MarkdownFilePath = PathTextBox.Text;
            _config.Save();
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
} 