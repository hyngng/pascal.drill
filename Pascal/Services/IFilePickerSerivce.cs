using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Pascal.Services
{
    public interface IFilePickerService
    {
        Task<IReadOnlyList<StorageFile>> PickMultiplePdfFilesAsync();
        Task<StorageFile?> PickSavePdfFileAsync();
    }
}
