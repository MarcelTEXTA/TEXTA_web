using CefSharp;
using System.IO;
using System.Windows;


namespace TEXTA_web.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Browser.Address = "https://www.google.com";
        }
    }
}
