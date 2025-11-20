using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pascal.Models;
using Pascal.Services.FileManageService;

namespace Pascal.ViewModels
{
    public partial class PdfMergePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<PdfItem> pdfItems = new();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy;

        public bool IsNotBusy => !IsBusy;

        private readonly IFileManageService fileManager;

        public PdfMergePageViewModel(IFileManageService fileManager)
        {
            this.fileManager = fileManager;
            this.fileManager.BusyChanged += (_, busy) => IsBusy = busy;
        }

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
        public async Task SavePdfFile()
        {
            if (IsBusy || PdfItems.Count == 0)
                return;

            await fileManager.SaveMergedPdfAsync(PdfItems);
        }

        [RelayCommand]
        public void OpenPdfFiles(IList<PdfItem>? selectedItems)
        {
            var list = (selectedItems?.Count > 0 ? selectedItems : new List<PdfItem> { PdfItems.FirstOrDefault()! })
                       .Where(x => x != null);

            fileManager.OpenFiles(list, i => i.FilePath);
        }

        [RelayCommand]
        public void DeletePdfFiles(IList<PdfItem>? selectedItems)
        {
            if (selectedItems is null || selectedItems.Count == 0)
                return;

            fileManager.DeleteItems(selectedItems, PdfItems, (it, order) => it.FileOrder = order);
        }

        [RelayCommand]
        public void ReorderPdfFiles()
        {
            fileManager.ReorderItems(PdfItems, (it, order) => it.FileOrder = order);
        }
    }
}
