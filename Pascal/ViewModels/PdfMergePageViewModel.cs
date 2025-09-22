using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pascal.Models;
using Pascal.Services.FilePickerService;
using Pascal.Services.PdfService;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Pascal.ViewModels
{
    public partial class PdfMergePageViewModel : ObservableObject
    {
        private readonly IFilePickerService filePickerService;

        [ObservableProperty]
        private ObservableCollection<PdfItemToMerge> pdfItems = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy = false;

        public bool IsNotBusy => !isBusy;

        public PdfMergePageViewModel()
        {
            this.filePickerService = App.Current.FilePickerService;
        }

        [RelayCommand]
        private async Task AddPdfFilesAsync()
        {
            IsBusy = true;
            try
            {
                var files = await filePickerService.PickMultiplePdfFilesAsync();
                if (files != null && files.Count > 0)
                {
                    int baseCount = pdfItems.Count;
                    for (int i = 0; i < files.Count; i++)
                    {
                        var file = files[i];
                        var properties = await file.GetBasicPropertiesAsync();

                        var pageCount = App.Current.PdfService.FindPageRanges(file.Path);

                        var newItem = new PdfItemToMerge
                        {
                            FileOrder = baseCount + i + 1,
                            FilePath = file.Path,
                            FileName = file.Name,
                            FileSize = $"{properties.Size / 1024:N0} KB",
                            PageCount = pageCount,
                            PagesToExtract = new List<int>()
                        };
                        pdfItems.Add(newItem);
                    }
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private void OpenFiles(IEnumerable<PdfItemToMerge> pdfItemsToOpen)
        {
            var list = pdfItemsToOpen.Where(i => i != null).Distinct().ToList();
            var psi = new ProcessStartInfo
            {
                FileName = list[0].FilePath,
                UseShellExecute = true
            };
            Process.Start(psi);
        }

        [RelayCommand]
        private void ReorderFiles()
        {
            for (int i = 0; i < pdfItems.Count; i++)
                pdfItems[i].FileOrder = i + 1;
        }

        [RelayCommand]
        private void DeleteFiles(IEnumerable<PdfItemToMerge> pdfItemsToDelete)
        {
            if (pdfItemsToDelete == null)
                return;

            var list = pdfItemsToDelete.Where(i => i != null).Distinct().ToList();
            if (list.Count == 0)
                return;

            foreach (var it in list)
                pdfItems.Remove(it);

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
                    App.Current.PdfService.MergePdf(await file.OpenStreamForWriteAsync(), pdfItems);
                }
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
