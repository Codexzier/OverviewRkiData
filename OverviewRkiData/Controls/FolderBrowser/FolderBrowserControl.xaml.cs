using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using OverviewRkiData.Views.Base;

namespace OverviewRkiData.Controls.FolderBrowser
{
    /// <summary>
    /// Interaction logic for FolderBrowserControl.xaml
    /// </summary>
    public partial class FolderBrowserControl : UserControl
    {
        public static readonly DependencyProperty SelectedDirectoryProperty = DependencyProperty.Register(
            "SelectedDirectory", typeof(SelectedDirectory), typeof(FolderBrowserControl), new PropertyMetadata(default(SelectedDirectory)));

        public SelectedDirectory SelectedDirectory
        {
            get => (SelectedDirectory)this.GetValue(SelectedDirectoryProperty);
            set => this.SetValue(SelectedDirectoryProperty, value);
        }

        public FolderBrowserControl() => this.InitializeComponent();

        public override void OnApplyTemplate() => this.LoadCurrentFolder(Environment.CurrentDirectory);

        private void LoadCurrentFolder(string currentDirectory)
        {
            var list = new List<FolderBrowserItem>();

            if (!this._filters.All(a => a.Invoke(new DirectoryInfo(currentDirectory))))
            {
                var d = new FolderBrowserItem(currentDirectory, true);
                if (!string.IsNullOrEmpty(d.FolderName))
                {
                    list.Add(d);
                }
            };

            try
            {
                var folderNames = this.FilterFolders(
                    Directory.GetDirectories(currentDirectory));

                list.AddRange(folderNames.Select(item => new FolderBrowserItem(item)).Where(ddd => !string.IsNullOrEmpty(ddd.FolderName)));

                this.ListBoxFolder.ItemsSource = list;
            }
            catch (Exception e)
            {
                SimpleStatusOverlays.Show("Fehler", e.Message);
            }
        }

        private readonly IList<Func<DirectoryInfo, bool>> _filters = new List<Func<DirectoryInfo, bool>>
        {
            diInfo => string.IsNullOrEmpty(diInfo.Name),
            diInfo => diInfo.Name.StartsWith("$"),
            diInfo => diInfo.Name.ToLower().EndsWith(".bin"),
            diInfo => diInfo.Parent == null
        };

        private IEnumerable<string> FilterFolders(IEnumerable<string> folders) =>
            folders.Where(w =>
            {
                var directoryInfo = new DirectoryInfo(w);
                return this._filters.All(filter => !filter.Invoke(directoryInfo));
            });

        private void ListBoxFolder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!(this.ListBoxFolder.SelectedItem is FolderBrowserItem item))
            {
                return;
            }

            if (item.ReturnFolderItem)
            {
                var di = new DirectoryInfo(item.CompletePath);
                if (di.Parent == null)
                {
                    return;
                }

                this.LoadCurrentFolder(di.Parent.FullName);
                return;
            }

            this.LoadCurrentFolder(item.CompletePath);
        }

        private void ListBoxFolder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(this.ListBoxFolder.SelectedItem is FolderBrowserItem item))
            {
                return;
            }

            this.TextBoxCompleteFolderPath.Text = item.CompletePath;
            this.SelectedDirectory.FolderName = item.CompletePath;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var str = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            this.LoadCurrentFolder(str);
        }
    }
}
