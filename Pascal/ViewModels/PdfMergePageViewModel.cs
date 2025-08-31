using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pascal.Models;
using Pascal.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Pascal.ViewModels
{
    public partial class PdfMergePageViewModel : ObservableObject
    {
        private readonly IFilePickerService filePickerService;

        [ObservableProperty]
        private ObservableCollection<PdfItemToMerge> items = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy = false;

        public bool IsNotBusy => !isBusy;

        public PdfMergePageViewModel(IFilePickerService filePickerService)
        {
            this.filePickerService = filePickerService;
        }

        [RelayCommand]
        private async Task PickFilesAsync()
        {
            IsBusy = true;
            try
            {
                var files = await filePickerService.PickMultiplePdfFilesAsync();
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        var properties = await file.GetBasicPropertiesAsync();
                        var newItem = new PdfItemToMerge
                        {
                            IsChecked = true,
                            FileName = file.Name,
                            FileSizeText = $"{properties.Size / 1024:N0} KB",
                            PageCount = 100, // TODO: 여기에 실제 로직 구현
                            RangeStart = 0, // TODO: 여기에 실제 로직 구현
                            RangeEnd = 100 // TODO: 여기에 실제 로직 구현
                        };
                        Items.Add(newItem);
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SaveFileAsync()
        {
            IsBusy = true;
            try
            {
                var file = await filePickerService.PickSavePdfFileAsync();
                if (file != null)
                {
                    // TODO: 여기에 실제 병합 및 저장 로직 구현
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
