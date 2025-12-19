using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pascal.Models;
using Pascal.Services.FileManageService;

namespace Pascal.ViewModels
{
    public partial class PdfSplitPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<PdfItem> pdfItems = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy = false;
        public bool IsNotBusy => !isBusy;

        private readonly IFileManageService fileManager;

        public PdfSplitPageViewModel(IFileManageService fileManager)
        {
            this.fileManager = fileManager;
            this.fileManager.BusyChanged += (_, busy) => IsBusy = busy;
        }

        //[RelayCommand]
        //private async Task AddPdfFilesAsync()
        //{
        //    IsBusy = true;
        //    try
        //    {
        //        var files = await App.Current.FileManageService.PickMultiplePdfFilesAsync();
        //        if (files != null && files.Count > 0)
        //        {
        //            int baseCount = pdfItems.Count;
        //            for (int i = 0; i < files.Count; i++)
        //            {
        //                var file = files[i];
        //                var properties  = await file.GetBasicPropertiesAsync();
        //                var pageCount   = App.Current.PdfService.FindPageRanges(file.Path);
        //                // var processUnit = 

        //                var newItem = new PdfItem
        //                {
        //                    FileOrder      = baseCount + i + 1,
        //                    PageCount      = pageCount,
        //                    // ProcessUnit    = processUnit,
        //                    FilePath       = file.Path,
        //                    FileName       = file.Name,
        //                    FileSize       = $"{properties.Size / 1024:N0} KB",
        //                    // PagesToProcess = new List<int>()
        //                };
        //                pdfItems.Add(newItem);
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}

        [RelayCommand]
        public async Task AddPdfFiles()
        {
            if (IsBusy) return;
            var added = await fileManager.PickAndCreatePdfItemsAsync(PdfItems.Count);
            if (added is null || added.Count == 0) return;

            foreach (var item in added)
                PdfItems.Add(item);
        }

        [RelayCommand]
        private async Task SaveFileAsync()
        {
            if (IsBusy || PdfItems.Count == 0)
                return;

            await fileManager.SaveMergedPdfAsync(PdfItems);
        }

        [RelayCommand]
        private void OpenFiles(IList<PdfItem>? selectedItems)
        {
            var list = (selectedItems?.Count > 0 ? selectedItems : new List<PdfItem> { PdfItems.FirstOrDefault()! })
                       .Where(x => x != null);

            fileManager.OpenFiles(list, i => i.FilePath);
        }

        [RelayCommand]
        private void DeleteFiles(IList<PdfItem>? selectedItems)
        {
            if (selectedItems is null || selectedItems.Count == 0)
                return;

            fileManager.DeleteItems(selectedItems, PdfItems, (it, order) => it.FileOrder = order);
        }
    }
}
