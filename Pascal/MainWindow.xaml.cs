using Microsoft.UI.Windowing;

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

        var navService = App.GetService<IJsonNavigationService>() as JsonNavigationService;
        if (navService != null)
        {
            navService.Initialize(NavView, NavFrame, NavigationPageMappings.PageDictionary)
                .ConfigureDefaultPage(typeof(HomeLandingPage))
                .ConfigureSettingsPage(typeof(SettingsPage))
                .ConfigureJsonFile("Assets/NavViewMenu/AppData.json")
                .ConfigureTitleBar(AppTitleBar)
                .ConfigureBreadcrumbBar(BreadCrumbNav, BreadcrumbPageMappings.PageDictionary);
        }

        mainPage_Loaded();
    }

    private void ThemeButton_Click(object sender, RoutedEventArgs e)
    {
        ThemeService.ChangeThemeWithoutSave(App.MainWindow);
    }

    private void OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        AutoSuggestBoxHelper.OnITitleBarAutoSuggestBoxTextChangedEvent(sender, args, NavFrame);
    }

    private void OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        AutoSuggestBoxHelper.OnITitleBarAutoSuggestBoxQuerySubmittedEvent(sender, args, NavFrame);
    }

    private void mainPage_Loaded()
    {
        App.Current.LabsService.LabsChanged += OnLabsEnabledToggled; 
    }

    #region 실험실 기능 숨기기/보이기 => 모델뷰로 옮길 것
    private void OnLabsEnabledToggled(bool isEnabled)
    {
        UpdateNavigationViewItemVisibility("LabsPage", !isEnabled);
    }

    private void UpdateNavigationViewItemVisibility(string uniqueId, bool hide)
    {
        var item = FindNavigationViewItem(uniqueId);
        if (item != null)
            item.Visibility = hide ? Visibility.Collapsed : Visibility.Visible;
    }

    private NavigationViewItem FindNavigationViewItem(string identifier)
    {
        var allCollections = new[] { NavView.MenuItems, NavView.FooterMenuItems };
        
        return allCollections
              .SelectMany(collection => GetAllNavigationItems(collection))
              .FirstOrDefault(item => item.Tag?.ToString() == identifier || item.Name == identifier);
    }

    private IEnumerable<NavigationViewItem> GetAllNavigationItems(IList<object> items)
    {
        foreach (var item in items)
        {
            if (item is NavigationViewItem navItem)
            {
                yield return navItem;
                
                if (navItem.MenuItems?.Count > 0)
                    foreach (var childItem in GetAllNavigationItems(navItem.MenuItems))
                        yield return childItem;
            }
        }
    }
    #endregion 실험실 기능 숨기기/보이기
}

