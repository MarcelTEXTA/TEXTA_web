using System.ComponentModel;
using System.Runtime.CompilerServices;

public class TabViewModel : INotifyPropertyChanged
{
    private string _address = "https://www.google.com";
    private string _title = "Nouvel onglet";

    public string Address
    {
        get => _address;
        set
        {
            _address = value;
            OnPropertyChanged();
        }
    }

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}