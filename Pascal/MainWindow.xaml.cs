using Microsoft.UI.Windowing;
using Pascal.Views.Pages;

namespace Pascal.Views;
public sealed partial class MainWindow : Window
{
    public MainViewModel ViewModel { get; }
    public MainWindow()
    {
        ViewModel = App.GetService<MainViewModel>();
        this.InitializeComponent();
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
        AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;

        mainPage_Loaded();
    }

    private void mainPage_Loaded()
    {
        // DevWinUI의 ConfigureDefaultPage와 ConfigureSettingsPage 사용
        App.Current.NavigationService
            .Initialize(NavView, NavFrame)
            .ConfigureDefaultPage(typeof(HomeLandingPage))
            .ConfigureSettingsPage(typeof(SettingsPage));
    }

    #region 고든램지 햄버거 버튼
    private void TitleBar_PaneToggleRequested(TitleBar sender, object args)
    {
        NavView.IsPaneOpen = !NavView.IsPaneOpen;
    }
    #endregion 고든램지 햄버거 버튼
}
