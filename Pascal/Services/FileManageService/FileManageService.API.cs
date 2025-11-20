using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Storage;
using Windows.Storage.Pickers;
using Pascal.Models;

namespace Pascal.Services.FileManageService;
internal partial class FileManageService
{
    #region 범용 Picker API
    public async Task<IReadOnlyList<StorageFile>> PickMultipleFilesAsync(
        IEnumerable<string>? fileTypeFilters,
        PickerLocationId startLocation = PickerLocationId.DocumentsLibrary,
        PickerViewMode viewMode = PickerViewMode.List)
    {
        var picker = CreateOpenPicker(startLocation, viewMode);

        bool any = false;
        foreach (var ext in fileTypeFilters ?? Array.Empty<string>())
        {
            if (string.IsNullOrWhiteSpace(ext)) continue;
            var normalized = ext.StartsWith('.') ? ext : "." + ext;
            picker.FileTypeFilter.Add(normalized);
            any = true;
        }

        if (!any)
            picker.FileTypeFilter.Add(".dat");

        return await picker.PickMultipleFilesAsync();
    }

    public async Task<StorageFile?> PickSaveFileAsync(
        string description,
        IEnumerable<string>? fileTypeChoices,
        string? suggestedFileName,
        PickerLocationId startLocation = PickerLocationId.DocumentsLibrary)
    {
        var picker = CreateSavePicker(startLocation);

        var list = new List<string>();
        foreach (var ext in fileTypeChoices ?? Array.Empty<string>())
        {
            if (string.IsNullOrWhiteSpace(ext)) continue;
            list.Add(ext.StartsWith('.') ? ext : "." + ext);
        }
        if (list.Count == 0)
            list.Add(".dat");

        picker.FileTypeChoices.Add(description, list);
        picker.SuggestedFileName = string.IsNullOrWhiteSpace(suggestedFileName) ? "untitled" : suggestedFileName;

        return await picker.PickSaveFileAsync();
    }
    #endregion

    #region 제네릭 아이템 생성/조작
    public async Task<IReadOnlyList<T>> PickAndCreateItemsAsync<T>(
        int baseCount,
        IEnumerable<string>? fileTypeFilters,
        Func<StorageFile, int, int, Task<T?>> factory,
        CancellationToken ct = default)
        where T : class
    {
        var files = await PickMultipleFilesAsync(fileTypeFilters);
        return await CreateItemsFromFilesAsync(files, baseCount, factory, ct);
    }

    public async Task<IReadOnlyList<T>> CreateItemsFromFilesAsync<T>(
        IReadOnlyList<StorageFile> files,
        int baseCount,
        Func<StorageFile, int, int, Task<T?>> factory,
        CancellationToken ct = default)
        where T : class
    {
        var result = new List<T>();
        if (files == null || files.Count == 0) return result;

        for (int i = 0; i < files.Count; i++)
        {
            ct.ThrowIfCancellationRequested();
            var file = files[i];
            var item = await factory(file, baseCount, i);
            if (item != null)
                result.Add(item);
        }
        return result;
    }

    public void OpenFiles<T>(IEnumerable<T>? items, Func<T, string> pathSelector)
    {
        if (items == null) return;
        var first = items.Where(i => i != null).FirstOrDefault();
        if (first == null) return;

        var path = pathSelector(first);
        if (string.IsNullOrWhiteSpace(path)) return;

        var psi = new ProcessStartInfo
        {
            FileName = path,
            UseShellExecute = true
        };
        Process.Start(psi);
    }

    public void ReorderItems<T>(ObservableCollection<T>? items, Action<T, int> setOrder)
    {
        if (items == null) return;
        for (int i = 0; i < items.Count; i++)
            setOrder(items[i], i + 1);
    }

    public void DeleteItems<T>(
        IEnumerable<T>? toDelete,
        ObservableCollection<T>? source,
        Action<T, int>? setOrderAfter = null)
    {
        if (toDelete == null || source == null) return;
        var list = toDelete.Where(i => i != null).Distinct().ToList();
        if (list.Count == 0) return;

        foreach (var it in list)
            source.Remove(it);

        if (setOrderAfter != null)
            ReorderItems(source, setOrderAfter);
    }
    #endregion
}
