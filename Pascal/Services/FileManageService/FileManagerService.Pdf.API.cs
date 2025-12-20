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

    public async Task<StorageFolder?> PickSavePdfFolderAsync()
    {
        var picker = CreateFolderPicker(PickerLocationId.DocumentsLibrary, PickerViewMode.List);
        return await picker.PickSingleFolderAsync();
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

    public async Task<IReadOnlyList<PdfItem>?> CreatePdfItemsFromFilePathsAsync(int startOrder, IReadOnlyList<string> filePaths)
    {
        if (filePaths.Count == 0)
            return Array.Empty<PdfItem>();

        BeginBusy();
        try
        {
            var list = new List<PdfItem>();
            for (int i = 0; i < filePaths.Count; i++)
            {
                var path = filePaths[i];
                if (string.IsNullOrWhiteSpace(path))
                    continue;

                StorageFile? file;
                try
                {
                    file = await StorageFile.GetFileFromPathAsync(path);
                }
                catch (Exception)
                {
                    continue;
                }

                var props = await file.GetBasicPropertiesAsync();
                var pageCount = App.Current.PdfService.FindPageRanges(path);

                list.Add(new PdfItem
                {
                    FileOrder = startOrder + list.Count + 1,
                    PageCount = pageCount,
                    FilePath = path,
                    FileName = file.Name,
                    FileSize = $"{props.Size / 1024:N0} KB",
                });
            }

            return list.Count > 0 ? list : null;
        }
        finally
        {
            EndBusy();
        }
    }
    #endregion
}
