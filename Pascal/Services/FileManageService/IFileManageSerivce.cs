using System.Collections.ObjectModel;
using Windows.Storage;
using Pascal.Models;

namespace Pascal.Services.FileManageService
{
    public interface IFileManageService
    {
        event EventHandler<bool>? BusyChanged;

        Task<IReadOnlyList<StorageFile>> PickMultiplePdfFilesAsync();
        Task<StorageFile?> PickSavePdfFileAsync();

        Task<IReadOnlyList<PdfItem>> PickAndCreatePdfItemsAsync(int baseCount, CancellationToken ct = default);
        Task<string?> SaveMergedPdfAsync(IReadOnlyList<PdfItem> items, CancellationToken ct = default);

        void OpenFiles<T>(IEnumerable<T>? items, Func<T, string> pathSelector);
        void ReorderItems<T>(ObservableCollection<T>? items, Action<T, int> setOrder);
        void DeleteItems<T>(IEnumerable<T>? toDelete, ObservableCollection<T>? source, Action<T, int>? setOrderAfter = null);
    }
}
