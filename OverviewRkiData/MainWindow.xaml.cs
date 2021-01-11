using OverviewRkiData.Commands;
using OverviewRkiData.Components.LegacyData;
using OverviewRkiData.Components.Ui.EventBus;
using OverviewRkiData.Components.UserSettings;
using OverviewRkiData.Views.Base;
using OverviewRkiData.Views.Main;
using System;
using System.Windows;
using System.Windows.Input;

namespace OverviewRkiData
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            
            new ShiftDataToSubFolder().MoveRkiDataFilesFromCurrentApplicationFolderToSubFolder();

            var setting = UserSettingsLoader.GetInstance().Load();

            this.LoadApplicationSize(setting);
            this.LoadApplicationWindowState(setting);
            this.LoadApplicationStartLocation(setting);            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Start the main view
            EventBusManager.OpenView<MainView>(0);
            EventBusManager.Send<MainView, BaseMessage>(new BaseMessage(BaseMessageOptions.LoadActualData), 0);
        }

        /// <summary>
        /// Load the last size of the application
        /// </summary>
        private void LoadApplicationSize(SettingsFile setting)
        {
            var size = new System.Drawing.Size(setting.SizeX, setting.SizeY);

            if (size.Width < 100)
            {
                size.Width = 800;
            }

            if (size.Height < 600)
            {
                size.Height = 600;
            }

            this.Width = size.Width;
            this.Height = size.Height;
        }

        /// <summary>
        /// Load the window state
        /// </summary>
        private void LoadApplicationWindowState(SettingsFile setting)
        {
            if (string.IsNullOrEmpty(setting.ApplicationWindowState))
            {
                this.WindowState = WindowState.Normal;
                return;
            }

            this.WindowState = Enum
                .TryParse(setting.ApplicationWindowState, 
                    out WindowState windowState) ? windowState : WindowState.Normal;
        }

        /// <summary>
        /// Load the location and place the application to the position.
        /// </summary>
        private void LoadApplicationStartLocation(SettingsFile setting)
        {
            var point = new System.Drawing.Point(setting.ApplicationPositionX, setting.ApplicationPositionY);

            if (point.X < 0 && point.Y < 0)
            {
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                return;
            }

            this.Left = point.X;
            this.Top = point.Y;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var usl = UserSettingsLoader.GetInstance();

            var file = usl.Load();

            file.ApplicationPositionX = (int)this.Left;
            file.ApplicationPositionY = (int)this.Top;
            file.SizeX = (int)this.Width;
            file.SizeY = (int)this.Height;
            file.ApplicationWindowState = this.WindowState.ToString();

            usl.Save(file);
        }

        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UIElement_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2)
            {
                if (this.WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                }
            }
        }
    }
}
