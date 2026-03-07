using CefSharp;
using TEXTA_web.AdBlock;

public class AdBlockRequestHandler : IRequestHandler
{
    public bool OnBeforeBrowse(IWebBrowser chromiumWebBrowser,
                               IBrowser browser,
                               IFrame frame,
                               IRequest request,
                               bool userGesture,
                               bool isRedirect)
    {
        var url = request.Url;

        if (AdBlockService.IsAd(url))
            return true;

        return false;
    }
}