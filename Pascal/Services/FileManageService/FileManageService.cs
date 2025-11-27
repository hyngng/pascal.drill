using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.Storage;
using Windows.Storage.Pickers;
using Pascal.Models;

namespace Pascal.Services.FileManageService
{
    internal sealed partial class FileManageService : IFileManageService
    {
        public event EventHandler<bool>? BusyChanged;
        private int busyCount;

        private void BeginBusy()
        {
            if (Interlocked.Increment(ref busyCount) == 1)
                BusyChanged?.Invoke(this, true);
        }

        private void EndBusy()
        {
            if (Interlocked.Decrement(ref busyCount) == 0)
                BusyChanged?.Invoke(this, false);
        }

        #region 내부 유틸
        private static FolderPicker CreateFolderPicker(PickerLocationId location, PickerViewMode mode)
        {
            var picker = new FolderPicker
            {
                SuggestedStartLocation = location,
                ViewMode = mode
            };
            picker.FileTypeFilter.Add("*");
            var window = App.MainWindow;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);
            return picker;
        }

        private static FileOpenPicker CreateOpenPicker(PickerLocationId location, PickerViewMode mode)
        {
            var picker = new FileOpenPicker
            {
                SuggestedStartLocation = location,
                ViewMode = mode
            };
            var window = App.MainWindow;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);
            return picker;
        }

        private static FileSavePicker CreateSavePicker(PickerLocationId location)
        {
            var picker = new FileSavePicker
            {
                SuggestedStartLocation = location
            };
            var window = App.MainWindow;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);
            return picker;
        }
        #endregion
    }
}
