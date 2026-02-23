using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TEXTA_web.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public ObservableCollection<TabViewModel> Tabs { get; set; }

        private TabViewModel _selectedTab;
        public TabViewModel SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(AddressBar));
            }
        }

        public string AddressBar
        {
            get => SelectedTab?.Address;
            set
            {
                if (SelectedTab != null)
                {
                    SelectedTab.Address = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand NewTabCommand { get; }
        public ICommand CloseTabCommand { get; }
        public ICommand NavigateCommand { get; }

        public MainWindowViewModel()
        {
            Tabs = new ObservableCollection<TabViewModel>();

            NewTabCommand = new RelayCommand(AddTab);
            CloseTabCommand = new RelayCommand<TabViewModel>(CloseTab);
            NavigateCommand = new RelayCommand(Navigate);

            AddTab();
        }

        private void AddTab()
        {
            var tab = new TabViewModel();
            Tabs.Add(tab);
            SelectedTab = tab;
        }

        private void CloseTab(TabViewModel tab)
        {
            if (tab != null && Tabs.Contains(tab))
                Tabs.Remove(tab);
        }

        private void Navigate()
        {
            if (SelectedTab != null)
                SelectedTab.Address = AddressBar;
        }
    }
}