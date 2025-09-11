using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pascal.Models;
using Pascal.Services;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
                    int baseCount = Items.Count;
                    for (int i = 0; i < files.Count; i++)
                    {
                        var file = files[i];
                        var properties = await file.GetBasicPropertiesAsync();
                        var newItem = new PdfItemToMerge
                        {
                            FileOrder = baseCount + i + 1,
                            FileName = file.Name,
                            FileSize = $"{properties.Size / 1024:N0} KB",
                            PageCount = 100, // TODO: 실제 페이지 수
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
            for (int i = 0; i < Items.Count; i++)
                Items[i].FileOrder = i + 1;
        }

        [RelayCommand]
        private void DeleteFile(PdfItemToMerge item)
        {
            if (item != null && Items.Contains(item))
            {
                Items.Remove(item);
                ReorderFiles();
            }
        }

        // 새로 추가: 다중 삭제
        [RelayCommand]
        private void DeleteFiles(IEnumerable<PdfItemToMerge> itemsToDelete)
        {
            if (itemsToDelete == null)
                return;

            // 복사본 만들어 안전하게 삭제
            var list = itemsToDelete.Where(i => i != null).Distinct().ToList();
            if (list.Count == 0)
                return;

            foreach (var it in list)
            {
                Items.Remove(it);
            }
            ReorderFiles();
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
                    // TODO: 병합 저장
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
