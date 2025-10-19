using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Web.WebView2.Wpf;

namespace TEXTA_web
{
    public partial class MainWindow : Window
    {
        private bool _isChangingTab = false;

        public MainWindow()
        {
            InitializeComponent();
            InitTabs();
        }

        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e) => this.WindowState = WindowState.Minimized;

        private void Maximize_Click(object sender, RoutedEventArgs e) =>
            this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

        private void Close_Click(object sender, RoutedEventArgs e) => this.Close();

        private void InitTabs()
        {
            MainTabControl.Items.Clear();
            AddNewTab(); // Premier onglet
            AddPlusTab(); // Onglet "+"
        }

        private void AddPlusTab()
        {
            var plusTab = new TabItem { Header = "+", IsEnabled = true };
            MainTabControl.Items.Add(plusTab);
        }

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isChangingTab) return;

            if (MainTabControl.SelectedItem is TabItem selected &&
                selected.Header is string header && header == "+")
            {
                _isChangingTab = true;

                MainTabControl.Items.Remove(selected);
                AddNewTab();
                AddPlusTab();

                _isChangingTab = false;
            }
        }

        private void AddNewTab()
        {
            var tabItem = new TabItem();

            // Création de l'en-tête avec bouton de fermeture
            var headerPanel = new StackPanel { Orientation = Orientation.Horizontal };
            var title = new TextBlock { Text = "Nouvel onglet", Margin = new Thickness(0, 0, 5, 0) };
            var closeButton = new Button
            {
                Content = "x",
                Width = 16,
                Height = 16,
                FontSize = 10,
                Padding = new Thickness(0),
                Margin = new Thickness(2, 0, 0, 0),
                Background = null,
                BorderBrush = null,
                Cursor = Cursors.Hand
            };

            closeButton.Click += (s, e) =>
            {
                if (MainTabControl.Items.Contains(tabItem))
                    MainTabControl.Items.Remove(tabItem);
            };

            headerPanel.Children.Add(title);
            headerPanel.Children.Add(closeButton);
            tabItem.Header = headerPanel;

            // Création du WebView2
            var browser = new WebView2();
            string localFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "newtab.html");

            browser.Loaded += async (s, e) =>
            {
                try
                {
                    await browser.EnsureCoreWebView2Async(null);

                    string localUri = "file:///" + localFile.Replace("\\", "/");
                    browser.Source = new Uri(localUri);

                    // Debug optionnel
                    // browser.CoreWebView2.OpenDevToolsWindow();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur de chargement WebView2 : " + ex.Message);
                }
            };

            tabItem.Content = browser;

            // Insère le nouvel onglet juste avant l'onglet "+"
            MainTabControl.Items.Insert(MainTabControl.Items.Count, tabItem);
            MainTabControl.SelectedItem = tabItem;
        }
    }
}
