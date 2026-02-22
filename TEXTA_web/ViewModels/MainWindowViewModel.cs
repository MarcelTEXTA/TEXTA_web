using System.Collections.ObjectModel;

public class MainWindowViewModel
{
    public ObservableCollection<TabViewModel> Tabs { get; set; }

    public TabViewModel SelectedTab { get; set; }

    public MainWindowViewModel()
    {
        Tabs = new ObservableCollection<TabViewModel>();
        AddNewTab();
    }

    public void AddNewTab()
    {
        var tab = new TabViewModel();
        Tabs.Add(tab);
        SelectedTab = tab;
    }
}