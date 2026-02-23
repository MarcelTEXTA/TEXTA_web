using System.Collections.ObjectModel;

namespace TEXTA_web.ViewModels
{
    public class MainWindowViewModel
    {
        public ObservableCollection<TabViewModel> Tabs { get; set; }
        public TabViewModel SelectedTab { get; set; }

        public MainWindowViewModel()
        {
            Tabs = new ObservableCollection<TabViewModel>();
            var firstTab = new TabViewModel();
            Tabs.Add(firstTab);
            SelectedTab = firstTab;
        }
    }
}