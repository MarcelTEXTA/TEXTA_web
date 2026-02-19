using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Web.WebView2.Core;
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
            AddNewTab(); // Premier onglet (par défaut)
            AddPlusTab(); // Onglet "+"
        }

        private void AddPlusTab()
        {
            var plusTab = new TabItem { Header = "Nouvel onglet", IsEnabled = true };
            MainTabControl.Items.Add(plusTab);
            // Style optionnel pour centrer le "+" (pas de background ni bordure
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
                //AddPlusTab();

                _isChangingTab = false;
            }
        }

        private void Show_Web_Menu(object sender, RoutedEventArgs e)
        {
            // afficher le menu contextuel
            if (sender is FrameworkElement)
            {
                ContextMenu menuBoutonActif = new ContextMenu();

                MenuItem newTabItem = new MenuItem { Header = "Nouvel onglet" };
                newTabItem.Click += (s, args) => OpenNewTab();
                menuBoutonActif.Items.Add(newTabItem);
                MenuItem newWin = new MenuItem { Header = "Nouvelle fenêtre" };
                newWin.Click += (s, args) =>
                {
                    MainWindow newWindow = new MainWindow();
                    newWindow.Show();
                };
                menuBoutonActif.Items.Add(newWin);
                MenuItem NewPrivateWin = new MenuItem { Header = "Nouvelle fenêtre navigation privée" };
                NewPrivateWin.Click += (s, args) =>
                {
                    MainWindow newWindow = new MainWindow();
                    newWindow.Show();
                };
                menuBoutonActif.Items.Add(NewPrivateWin);

                MenuItem separator = new MenuItem { IsEnabled = false };
                menuBoutonActif.Items.Add(separator);

                MenuItem closeTabItem = new MenuItem { Header = "Fermer l'onglet" };
                closeTabItem.Click += (s, args) => CloseCurrentTab();
                menuBoutonActif.Items.Add(closeTabItem);

                MenuItem closeOtherTabsItem = new MenuItem { Header = "Fermer les autres onglets" };
                closeOtherTabsItem.Click += (s, args) => CloseOtherTabs();
                menuBoutonActif.Items.Add(closeOtherTabsItem);

                menuBoutonActif.IsOpen = true;
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
                BorderBrush = null
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
                    profile.ClearBrowsingDataAsync(CoreWebView2BrowsingDataKinds.AllSite);
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

        public void ClearAllData()
        {
            ClearCookies();
            ClearCache();
            ClearHistory();
        }

        public void EnablePrivateBrowsingMode()
        {
            // WebView2 ne supporte pas directement le mode navigation privée.
            // Cependant, vous pouvez créer un profil temporaire pour simuler ce comportement.
            if (MainTabControl.SelectedItem is TabItem selected &&
                selected.Content is WebView2 browser)
            {
                browser.Dispose();
                var newBrowser = new WebView2();
                selected.Content = newBrowser;
                newBrowser.Loaded += async (s, e) =>
                {
                    var env = await CoreWebView2Environment.CreateAsync(null, null, new CoreWebView2EnvironmentOptions("--incognito"));
                    await newBrowser.EnsureCoreWebView2Async(env);
                };
            }
        }

        public void DisablePrivateBrowsingMode()
        {
            // Cette fonctionnalité n'est pas directement supportée par WebView2.
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

        public void ClearPrivateData()
        {
            ClearAllData();
        }

        public void SetTrackingProtection(bool enable)
        {
            if (MainTabControl.SelectedItem is TabItem selected &&
                selected.Content is WebView2 browser)
            {
                browser.Dispose();
                var newBrowser = new WebView2();
                selected.Content = newBrowser;
                newBrowser.Loaded += async (s, e) =>
                {
                    var options = enable ? "--enable-features=TrackingPrevention" : "--disable-features=TrackingPrevention";
                    var env = await CoreWebView2Environment.CreateAsync(null, null, new CoreWebView2EnvironmentOptions(options));
                    await newBrowser.EnsureCoreWebView2Async(env);
                };
            }
        }

        public void ClearSiteData(string url)
        {
            if (MainTabControl.SelectedItem is TabItem selected &&
                selected.Content is WebView2 browser &&
                browser.CoreWebView2 != null)
            {
                var uri = new Uri(url);
                browser.CoreWebView2.Profile.ClearBrowsingDataAsync(CoreWebView2BrowsingDataKinds.AllSite);
            }
        }

        public void SetDoNotTrack(bool enable)
        {
            if (MainTabControl.SelectedItem is TabItem selected &&
                selected.Content is WebView2 browser)
            {
                browser.Dispose();
                var newBrowser = new WebView2();
                selected.Content = newBrowser;
                newBrowser.Loaded += async (s, e) =>
                {
                    var options = enable ? "--enable-features=DoNotTrack" : "--disable-features=DoNotTrack";
                    var env = await CoreWebView2Environment.CreateAsync(null, null, new CoreWebView2EnvironmentOptions(options));
                    await newBrowser.EnsureCoreWebView2Async(env);
                };
            }
        }

        public void ClearDownloads()
        {
            // WebView2 ne fournit pas d'API pour gérer les téléchargements.
            // Pour l'instant, cette fonctionnalité n'est pas implémentée.
        }

        public void ClearFormData()
        {
            // WebView2 ne fournit pas d'API directe pour effacer les données de formulaire.
            // Cette fonctionnalité n'est pas implémentée pour l'instant.
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

        public void ClearPasswords()
        {
            // WebView2 ne fournit pas d'API directe pour effacer les mots de passe enregistrés.
            // Cette fonctionnalité n'est pas implémentée pour l'instant.
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

        public void ClearAutofillData()
        {
            // WebView2 ne fournit pas d'API directe pour effacer les données de saisie automatique.
            // Cette fonctionnalité n'est pas implémentée pour l'instant.
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
