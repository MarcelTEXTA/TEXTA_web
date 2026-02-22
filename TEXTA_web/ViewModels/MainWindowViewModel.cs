using System.Collections.ObjectModel;
using System.Windows.Input;
using TEXTA_web.Models;

namespace TEXTA_web.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ObservableCollection<BrowserTabViewModel> Tabs { get; }

        private BrowserTabViewModel _selectedTab;
        public BrowserTabViewModel SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;
                OnPropertyChanged();
            }
        }

        private string _addressBar;
        public string AddressBar
        {
            get => _addressBar;
            set
            {
                _addressBar = value;
                OnPropertyChanged();
            }
        }

        // COMMANDES
        public ICommand NewTabCommand { get; }
        public ICommand CloseTabCommand { get; }
        public ICommand NavigateCommand { get; }
        public ICommand RefreshCommand { get; }

        public MainWindowViewModel()
        {
            Tabs = new ObservableCollection<BrowserTabViewModel>();

            NewTabCommand = new RelayCommand(AddNewTab);
            CloseTabCommand = new RelayCommand(CloseCurrentTab, () => SelectedTab != null);
            NavigateCommand = new RelayCommand(Navigate);
            RefreshCommand = new RelayCommand(Refresh);

            AddNewTab();
        }

        private void AddNewTab()
        {
            var model = new BrowserTabModel
            {
                Title = "Nouvel onglet",
                Url = "https://www.bing.com"
            };

            var tab = new BrowserTabViewModel(model);

            Tabs.Add(tab);
            SelectedTab = tab;
        }

        private void CloseCurrentTab()
        {
            if (SelectedTab != null)
                Tabs.Remove(SelectedTab);
        }

        private void Navigate()
        {
            if (SelectedTab == null || string.IsNullOrWhiteSpace(AddressBar))
                return;

            string url = AddressBar;

            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
                url = "http://" + url;

            SelectedTab.Url = url;
        }

        private void Refresh()
        {
            if (SelectedTab != null)
            {
                SelectedTab.Url = SelectedTab.Url;
            }
        }
    }
}
