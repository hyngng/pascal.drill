using Pascal.Services.FileManageService;
using Pascal.Services.LabsService;
using Pascal.Services.ParseService;
using Pascal.Services.PdfService;
using Pascal.Services.WindowService;

namespace Pascal;

public partial class App : Application
{
    public new static App Current => (App)Application.Current;
    public static Window MainWindow = Window.Current;
    public static IntPtr Hwnd => WinRT.Interop.WindowNative.GetWindowHandle(MainWindow);
    public IServiceProvider Services { get; }
    public IThemeService ThemeService => GetService<IThemeService>();
    public ILabsService LabsService => GetService<ILabsService>();
    public IFileManageService FileManageService => GetService<IFileManageService>();
    public IPdfService PdfService => GetService<IPdfService>();
    public IParseService ParseService => GetService<IParseService>();
    public INavigationServiceEx NavigationService => GetService<INavigationServiceEx>();

    public static T GetService<T>() where T : class
    {
        if ((App.Current as App)!.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public App()
    {
        Services = ConfigureServices();
        this.InitializeComponent();
    }

    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // ViewModels
        services.AddTransient<MainViewModel>();
        services.AddTransient<PdfMergePageViewModel>();
        services.AddTransient<PdfSplitPageViewModel>();
        services.AddTransient<WindowsUpdatePageViewModel>();

        // Services
        services.AddSingleton<IThemeService, ThemeService>();
        services.AddSingleton<ILabsService, LabsService>();
        services.AddSingleton<IPdfService, PdfService>();
        services.AddSingleton<IParseService, ParseService>();
        services.AddSingleton<INavigationServiceEx, NavigationServiceEx>();
        services.AddSingleton<IWindowService, WindowService>();

        services.AddTransient<IFileManageService, FileManageService>();

        return services.BuildServiceProvider();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        MainWindow = new MainWindow();

        MainWindow.Title = MainWindow.AppWindow.Title = ProcessInfoHelper.ProductNameAndVersion;
        MainWindow.AppWindow.SetIcon("Assets/AppIcon.ico");

        ThemeService.Initialize(MainWindow);
        LabsService.Initialize();

        MainWindow.Activate();

        InitializeApp();
    }

    private void InitializeApp()
    {

    }
}

