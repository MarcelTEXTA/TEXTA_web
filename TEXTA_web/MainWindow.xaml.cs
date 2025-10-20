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

        public void CloseCurrentTab()
        {
            if (MainTabControl.SelectedItem is TabItem selected &&
                selected.Header is StackPanel headerPanel &&
                headerPanel.Children[0] is TextBlock titleBlock &&
                titleBlock.Text != "+")
            {
                MainTabControl.Items.Remove(selected);
            }
        }

        public void OpenNewTab()
        {
            _isChangingTab = true;
            // Supprime l'onglet "+"
            if (MainTabControl.Items[MainTabControl.Items.Count - 1] is TabItem plusTab &&
                plusTab.Header is string header && header == "+")
            {
                MainTabControl.Items.Remove(plusTab);
            }
            AddNewTab();
            AddPlusTab();
            _isChangingTab = false;
        }

        public void RefreshCurrentTab()
        {
            if (MainTabControl.SelectedItem is TabItem selected &&
                selected.Content is WebView2 browser)
            {
                browser.Reload();
            }
        }

        public void NavigateCurrentTab(string url)
        {
            if (MainTabControl.SelectedItem is TabItem selected &&
                selected.Content is WebView2 browser)
            {
                try
                {
                    if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                    {
                        url = "http://" + url;
                    }
                    browser.Source = new Uri(url);
                }
                catch (UriFormatException)
                {
                    MessageBox.Show("URL invalide : " + url);
                }
            }
        }

        public void GoBackCurrentTab()
        {
            if (MainTabControl.SelectedItem is TabItem selected &&
                selected.Content is WebView2 browser)
            {
                if (browser.CanGoBack)
                {
                    browser.GoBack();
                }
            }
        }

        public void GoForwardCurrentTab()
        {
            if (MainTabControl.SelectedItem is TabItem selected &&
                selected.Content is WebView2 browser)
            {
                if (browser.CanGoForward)
                {
                    browser.GoForward();
                }
            }
        }

        public string GetCurrentTabUrl()
        {
            if (MainTabControl.SelectedItem is TabItem selected &&
                selected.Content is WebView2 browser)
            {
                return browser.Source?.ToString() ?? string.Empty;
            }
            return string.Empty;
        }

        // Ajoutez d'autres méthodes selon les besoins

        public int GetTabCount()
        {
            // Exclure l'onglet "+"
            return MainTabControl.Items.Count - 1;
        }

        public int GetCurrentTabIndex()
        {
            // définit l'index de l'onglet actuel
            return MainTabControl.SelectedIndex;
        }

        public void SwitchToTab(int index)
        {
            // bascule vers l'onglet spécifié par l'index
            if (index >= 0 && index < GetTabCount())
            {
                MainTabControl.SelectedIndex = index;
            }
        }

        public void CloseAllTabs()
        {
            _isChangingTab = true;
            MainTabControl.Items.Clear();
            AddNewTab();
            AddPlusTab();
            _isChangingTab = false;
        }

        public void CloseOtherTabs()
        {
            if (MainTabControl.SelectedItem is TabItem selected)
            {
                _isChangingTab = true;
                var currentIndex = MainTabControl.Items.IndexOf(selected);
                MainTabControl.Items.Clear();
                MainTabControl.Items.Add(selected);
                AddPlusTab();
                MainTabControl.SelectedIndex = 0;
                _isChangingTab = false;
            }
            else
            {
                CloseAllTabs();
            }
        }

        // fonctions de sécurité
        public void ClearCookies()
        {
            if (MainTabControl.SelectedItem is TabItem selected &&
                selected.Content is WebView2 browser)
            {
                var cookieManager = browser.CoreWebView2.CookieManager;
                cookieManager.DeleteAllCookies();
            }
        }

        public void ClearCache()
        {
            if (MainTabControl.SelectedItem is TabItem selected &&
                selected.Content is WebView2 browser &&
                browser.CoreWebView2 != null)
            {
                // Pour vider le cache, il faut passer par le Profile associé.
                var profile = browser.CoreWebView2.Profile;
                if (profile != null)
                {
                    // Efface le cache et les données de navigation
                    profile.ClearBrowsingDataAsync(CoreWebView2BrowsingDataKinds.All);
                }
            }
        }

        public void ClearHistory()
        {
            // WebView2 ne fournit pas d'API directe pour effacer l'historique de navigation.
            // Cependant, vous pouvez réinitialiser le contrôle WebView2 pour effacer l'historique.
            if (MainTabControl.SelectedItem is TabItem selected &&
                selected.Content is WebView2 browser)
            {
                browser.Dispose();
                var newBrowser = new WebView2();
                selected.Content = newBrowser;
                newBrowser.Loaded += async (s, e) =>
                {
                    await newBrowser.EnsureCoreWebView2Async(null);
                };
            }
        }
    }
}
