using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TEXTA_web.ViewModels
{
    public class TabViewModel : INotifyPropertyChanged
    {
        private string _address = "file:///Assets/welcome.html";
        private string _title = "Nouvel onglet";

        public string Address
        {
            get => _address;
            set { _address = value; OnPropertyChanged(); }
        }

        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
             => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}