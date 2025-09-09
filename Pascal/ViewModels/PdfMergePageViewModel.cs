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
                    int baseCount = Items.Count; // 기존 항목 수(뒤에 이어붙이기용)
                    for (int i = 0; i < files.Count; i++)
                    {
                        var file = files[i];
                        var properties = await file.GetBasicPropertiesAsync();
                        var newItem = new PdfItemToMerge
                        {
                            FileOrder = baseCount + i + 1, // 1부터 시작하는 순번 부여
                            FileName = file.Name,
                            FileSizeText = $"{properties.Size / 1024:N0} KB",
                            PageCount = 100, // TODO: 실제 페이지 수 로직
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

        // (선택) 재정렬 후 순번 재부여가 필요하면 호출
        private async Task RenumberFilesAsync()
        {
            IsBusy = true;
            try
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].FileOrder != i + 1)
                        Items[i].FileOrder = i + 1;
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
