﻿using System;
using System.Windows;
using GlobalHotKey;
using System.Windows.Input;
using System.Windows.Forms;
using Drawing = System.Drawing;

namespace InspirationRecorder
{
    public partial class MainWindow : Window
    {
        private HotKeyManager hotKeyManager;
        private readonly Config _config;
        private readonly IdeaService _ideaService;
        private NotifyIcon trayIcon;
        private ContextMenuStrip contextMenu;

        public MainWindow()
        {
            InitializeComponent();
            
            _config = Config.Load();
            _ideaService = new IdeaService(_config);
            
            InitializeHotKey();
            InitializeTrayIcon();
            
            this.WindowState = WindowState.Minimized;
            this.ShowInTaskbar = false;

            System.Windows.Application.Current.Exit += Current_Exit;
        }

        private void InitializeHotKey()
        {
            hotKeyManager = new HotKeyManager();
            
            hotKeyManager.Register(Key.I, ModifierKeys.Control | ModifierKeys.Shift);
            
            hotKeyManager.KeyPressed += HotKeyManager_KeyPressed;
        }

        private void HotKeyManager_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            if (IsAnyWindowFullScreen())
                return;

            var inputWindow = new InputWindow(_config);
            inputWindow.ShowDialog();

            if (!string.IsNullOrWhiteSpace(inputWindow.InputText))
            {
                try
                {
                    _ideaService.SaveIdea(inputWindow.InputText);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"保存失败：{ex.Message}", "错误", 
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool IsAnyWindowFullScreen()
        {
            var screenBounds = Screen.PrimaryScreen.Bounds;
            
            var foregroundWindow = NativeMethods.GetForegroundWindow();
            
            if (foregroundWindow != IntPtr.Zero)
            {
                NativeMethods.RECT rect;
                if (NativeMethods.GetWindowRect(foregroundWindow, out rect))
                {
                    return rect.Left <= 0 && rect.Top <= 0 &&
                           rect.Right >= screenBounds.Width &&
                           rect.Bottom >= screenBounds.Height;
                }
            }
            return false;
        }

        private void InitializeTrayIcon()
        {
            contextMenu = new ContextMenuStrip();
            
            contextMenu.Items.Add("快速记录", null, (s, e) => ShowInputWindow());
            contextMenu.Items.Add("设置", null, (s, e) => ShowSettingsWindow());
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add("退出", null, (s, e) => System.Windows.Application.Current.Shutdown());

            trayIcon = new NotifyIcon
            {
                Icon = new Drawing.Icon("D:\\Cursor_prj\\Windows\\QuickIDEA\\Resources\\app.ico"),
                Visible = true,
                Text = "QuickIDEA",
                ContextMenuStrip = contextMenu
            };

            trayIcon.DoubleClick += (s, e) => ShowInputWindow();
        }

        private void ShowSettingsWindow()
        {
            using (var settingsWindow = new SettingsWindow(_config))
            {
                settingsWindow.ShowDialog();
            }
        }

        private void ShowInputWindow()
        {
            using (var inputWindow = new InputWindow(_config))
            {
                inputWindow.ShowDialog();

                if (!string.IsNullOrWhiteSpace(inputWindow.InputText))
                {
                    try
                    {
                        _ideaService.SaveIdea(inputWindow.InputText);
                    }
                    catch (Exception ex)
                    {
                        System.Windows.MessageBox.Show($"保存失败：{ex.Message}", "错误", 
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            Cleanup();
        }

        private void Cleanup()
        {
            if (trayIcon != null)
            {
                trayIcon.Visible = false;
                trayIcon.Dispose();
                trayIcon = null;
            }

            if (contextMenu != null)
            {
                contextMenu.Dispose();
            }

            if (hotKeyManager != null)
            {
                hotKeyManager.Dispose();
                hotKeyManager = null;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            Cleanup();
            base.OnClosed(e);
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                this.Hide();
            }
            base.OnStateChanged(e);
        }
    }
}