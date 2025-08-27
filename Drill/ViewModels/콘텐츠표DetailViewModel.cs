using CommunityToolkit.Mvvm.ComponentModel;

using Drill.Contracts.ViewModels;
using Drill.Core.Contracts.Services;
using Drill.Core.Models;

namespace Drill.ViewModels;

public partial class 콘텐츠표DetailViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;

    [ObservableProperty]
    private SampleOrder? item;

    public 콘텐츠표DetailViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is long orderID)
        {
            var data = await _sampleDataService.GetContentGridDataAsync();
            Item = data.First(i => i.OrderID == orderID);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
