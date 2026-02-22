namespace TEXTA_web.Models
{
    public class BrowserTabModel
    {
        public string Title { get; set; } = "Nouvel onglet";
        public string Url { get; set; } = "file:///Assets/newtab.html";
        public bool IsPrivate { get; set; } = false;
    }
}
