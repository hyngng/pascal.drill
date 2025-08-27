using CommunityToolkit.WinUI.UI.Animations;

using Drill.Contracts.Services;
using Drill.ViewModels;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace Drill.Views;

public sealed partial class 콘텐츠표DetailPage : Page
{
    public 콘텐츠표DetailViewModel ViewModel
    {
        get;
    }

    public 콘텐츠표DetailPage()
    {
        ViewModel = App.GetService<콘텐츠표DetailViewModel>();
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        this.RegisterElementForConnectedAnimation("animationKeyContentGrid", itemHero);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
        if (e.NavigationMode == NavigationMode.Back)
        {
            var navigationService = App.GetService<INavigationService>();

            if (ViewModel.Item != null)
            {
                navigationService.SetListDataItemForNextConnectedAnimation(ViewModel.Item);
            }
        }
    }
}
