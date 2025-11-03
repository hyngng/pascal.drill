using CommunityToolkit.Mvvm.Input;
using Pascal.Services.WindowService;

namespace Pascal.ViewModels;

public partial class WindowsUpdatePageViewModel : ObservableObject
{
    private readonly IWindowService windowService;

    public WindowsUpdatePageViewModel(IWindowService windowService)
    {
        this.windowService = windowService;
    }

    [RelayCommand]
    private void ShowWindowsUpdate()
    {
        windowService.ShowWindowsUpdateSimulation();
    }
}
