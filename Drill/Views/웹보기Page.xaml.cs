using Drill.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Drill.Views;

// To learn more about WebView2, see https://docs.microsoft.com/microsoft-edge/webview2/.
public sealed partial class 웹보기Page : Page
{
    public 웹보기ViewModel ViewModel
    {
        get;
    }

    public 웹보기Page()
    {
        ViewModel = App.GetService<웹보기ViewModel>();
        InitializeComponent();

        ViewModel.WebViewService.Initialize(WebView);
    }
}
