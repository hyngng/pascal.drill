using Drill.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Drill.Views;

public sealed partial class 콘텐츠표Page : Page
{
    public 콘텐츠표ViewModel ViewModel
    {
        get;
    }

    public 콘텐츠표Page()
    {
        ViewModel = App.GetService<콘텐츠표ViewModel>();
        InitializeComponent();
    }
}
