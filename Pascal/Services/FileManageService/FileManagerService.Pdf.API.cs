using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Storage;
using Windows.Storage.Pickers;
using Pascal.Models;

namespace Pascal.Services.FileManageService;
internal partial class FileManageService
{
    #region 기본(기존) PDF 전용 Picker
    public async Task<IReadOnlyList<StorageFile>> PickMultiplePdfFilesAsync()
    {
        var picker = CreateOpenPicker(PickerLocationId.DocumentsLibrary, PickerViewMode.List);
        picker.FileTypeFilter.Add(".pdf");
        return await picker.PickMultipleFilesAsync();
    }

    public async Task<StorageFile?> PickSavePdfFileAsync()
    {
        var picker = CreateSavePicker(PickerLocationId.DocumentsLibrary);
        picker.FileTypeChoices.Add("PDF Document", new List<string> { ".pdf" });
        picker.SuggestedFileName = "merged-document";
        return await picker.PickSaveFileAsync();
    }
    #endregion

    #region PDF 특화 래퍼
    public async Task<IReadOnlyList<PdfItem>> PickAndCreatePdfItemsAsync(int baseCount, CancellationToken ct = default)
    {
        BeginBusy();
        try
        {
            var files = await PickMultiplePdfFilesAsync();
            var list = new List<PdfItem>();
            for (int i = 0; i < files.Count; i++)
            {
                ct.ThrowIfCancellationRequested();
                var f = files[i];
                var props = await f.GetBasicPropertiesAsync();
                var pageCount = App.Current.PdfService.FindPageRanges(f.Path);
                list.Add(new PdfItem
                {
                    FileOrder = baseCount + i + 1,
                    PageCount = pageCount,
                    FilePath = f.Path,
                    FileName = f.Name,
                    FileSize = $"{props.Size / 1024:N0} KB",
                });
            }
            return list;
        }
        finally
        {
            EndBusy();
        }
    }

    public async Task<string?> SaveMergedPdfAsync(IReadOnlyList<PdfItem> items, CancellationToken ct = default)
    {
        if (items == null || items.Count == 0) return null;

        var oc = items as ObservableCollection<PdfItem> ?? new ObservableCollection<PdfItem>(items);

        App.Current.ParseService.ParsePageRange(oc);

        var file = await PickSavePdfFileAsync();
        if (file == null) return null;

        ct.ThrowIfCancellationRequested();
        App.Current.PdfService.MergePdf(file.Path, oc);
        return file.Path;
    }
    #endregion
}
