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

        [RelayCommand]
        private void ReorderFiles()
        {
            System.Diagnostics.Debug.WriteLine($"=== ReorderFiles 실행 ===");
            System.Diagnostics.Debug.WriteLine($"Items.Count: {Items.Count}");
            
            IsBusy = true;
            try
            {
                // 현재 상태 로깅
                for (int i = 0; i < Items.Count; i++)
                {
                    System.Diagnostics.Debug.WriteLine($"BEFORE - Index {i}: {Items[i].FileName} (FileOrder: {Items[i].FileOrder})");
                }
                
                // 순번 재설정
                for (int i = 0; i < Items.Count; i++)
                {
                    Items[i].FileOrder = i + 1;
                }
                
                // 변경 후 상태 로깅
                for (int i = 0; i < Items.Count; i++)
                {
                    System.Diagnostics.Debug.WriteLine($"AFTER  - Index {i}: {Items[i].FileName} (FileOrder: {Items[i].FileOrder})");
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
