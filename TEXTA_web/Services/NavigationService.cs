using TEXTA_web.ViewModels;

public interface INavigationService
{
    void Navigate(string url);
    void GoBack();
    void GoForward();
    void Reload();
    void GoHome();
}

public class NavigationService : INavigationService
{
    private readonly TabViewModel _tab;

    public NavigationService(TabViewModel tab)
    {
        _tab = tab;
    }

    public void Navigate(string url)
    {
        _tab.Address = url;
    }

    public void GoBack()
    {
        _tab.Browser?.Back();
    }

    public void GoForward()
    {
        _tab.Browser?.Forward();
    }

    public void Reload()
    {
        _tab.Browser?.Reload();
    }

    public void GoHome()
    {
        _tab.Address = "https://www.google.com";
    }
}