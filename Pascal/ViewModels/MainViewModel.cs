namespace Pascal.ViewModels;

using CommunityToolkit.Mvvm.ComponentModel;
using Pascal.Services.LabsService;

public partial class MainViewModel : ObservableObject
{
    private readonly ILabsService labsService;

    public MainViewModel(ILabsService labsService)
    {
        this.labsService = labsService;
        this.labsService.LabsChanged += OnLabsChanged;

        IsLabsVisible = this.labsService.IsLabsEnabled;
    }

    [ObservableProperty]
    private bool isLabsVisible;

    private void OnLabsChanged(bool isEnabled)
    {
        IsLabsVisible = isEnabled;
    }
}
