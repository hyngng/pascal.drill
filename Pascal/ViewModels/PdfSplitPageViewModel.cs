using Pascal.Models;
using Pascal.Services.FilePickerService;
using Pascal.Services.ParseService;
using Pascal.Services.PdfService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pascal.ViewModels
{
    public partial class PdfSplitPageViewModel : ObservableObject
    {
        private readonly IFilePickerService filePickerService;
        private readonly IPdfService pdfService;
        private readonly IParseService parseService;

        [ObservableProperty]
        private ObservableCollection<PdfItem> pdfItems = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy = false;
        public bool IsNotBusy => !isBusy;

        public PdfSplitPageViewModel()
        {
            this.filePickerService = App.Current.FilePickerService;
            this.pdfService = App.Current.PdfService;
            this.parseService = App.Current.ParseService;
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

                        var pageCount = pdfService.FindPageRanges(file.Path);

                        var newItem = new PdfItem
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
        private async Task SaveFileAsync()
        {
            if (pdfItems.Count != 0)
            {
                IsBusy = true;
                parseService.ParsePageRange(pdfItems);
                try
                {
                    var file = await filePickerService.PickSavePdfFileAsync();
                    if (file != null)
                    {
                        pdfService.MergePdf(file.Path, pdfItems);
                    }
                }
                finally
                {
                    IsBusy = false;
                }
            }
        }

        [RelayCommand]
        private void OpenFiles(IEnumerable<PdfItem> pdfItemsToOpen)
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
        private void DeleteFiles(IEnumerable<PdfItem> pdfItemsToDelete)
        {
            if (pdfItemsToDelete == null)
                return;

            var list = pdfItemsToDelete.Where(i => i != null).Distinct().ToList();
            if (list.Count == 0)
                return;

            foreach (var it in list)
                pdfItems.Remove(it);
        }
    }
}
