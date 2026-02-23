using CefSharp;
using CefSharp.Wpf;
using System;
using System.IO;
using System.Windows;

namespace TEXTA_web
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var settings = new CefSettings
            {
                CachePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "TEXTA_Cache")
            };

            Cef.Initialize(settings);

            var mainWindow = new Views.MainWindow();
            mainWindow.Show();
        }
    }
}