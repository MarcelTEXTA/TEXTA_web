using TEXTA_web.Models;

namespace TEXTA_web.ViewModels
{
    public class BrowserTabViewModel : BaseViewModel
    {
        private readonly BrowserTabModel _model;

        public BrowserTabViewModel(BrowserTabModel model)
        {
            _model = model;
        }

        public string Title
        {
            get => _model.Title;
            set
            {
                _model.Title = value;
                OnPropertyChanged();
            }
        }

        public string Url
        {
            get => _model.Url;
            set
            {
                _model.Url = value;
                OnPropertyChanged();
            }
        }

        public bool IsPrivate
        {
            get => _model.IsPrivate;
            set
            {
                _model.IsPrivate = value;
                OnPropertyChanged();
            }
        }
    }
}
