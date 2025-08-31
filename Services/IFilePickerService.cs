using Windows.Storage;

namespace Pascal.Services
{
    public interface IFilePickerService
    {
        Task<IReadOnlyList<StorageFile>> PickMultiplePdfFilesAsync();
        Task<StorageFile?> PickSavePdfFileAsync();
    }
}