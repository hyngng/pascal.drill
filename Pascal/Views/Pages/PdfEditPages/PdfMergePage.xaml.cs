using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Pascal.Models;
using Pascal.Services;
using Pascal.Services.FilePickerService;
using Pascal.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;

namespace Pascal.Views.Pages.PdfEditPages
{
    public sealed partial class PdfMergePage : Page, IFilePickerService
    {
        public PdfMergePageViewModel ViewModel { get; }

        public PdfMergePage()
        {
            this.InitializeComponent();
            ViewModel = new PdfMergePageViewModel();
        }

        #region 파일 선택 및 저장 버튼 관련 로직
        public async Task<IReadOnlyList<StorageFile>> PickMultiplePdfFilesAsync()
        {
            var openPicker = new FileOpenPicker();
            var window = App.MainWindow;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".pdf");

            return await openPicker.PickMultipleFilesAsync();
        }

        public async Task<StorageFile?> PickSavePdfFileAsync()
        {
            var savePicker = new FileSavePicker();
            var window = App.MainWindow;
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hWnd);

            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("PDF Document", new List<string>() { ".pdf" });
            savePicker.SuggestedFileName = ViewModel.PdfItems.FirstOrDefault()?.FileName ?? "merged-document";

            return await savePicker.PickSaveFileAsync();
        }
        #endregion

        #region ListView 관련 로직
        private void ItemMenuFlyout_Opening(object sender, object e)
        {
            var menuFlyout = sender as MenuFlyout;
            if (menuFlyout?.Target is ListViewItem listViewItem)
            {
                var menuFlyOutOpenItem = menuFlyout.Items
                                                     .OfType<MenuFlyoutItem>()
                                                     .FirstOrDefault(mi => (mi.Tag as string) == "OpenMenu");
                var menuFlyOutDeleteItem = menuFlyout.Items
                                                     .OfType<MenuFlyoutItem>()
                                                     .FirstOrDefault(mi => (mi.Tag as string) == "DeleteMenu");

                if (menuFlyOutDeleteItem is null)
                    return;

                menuFlyOutOpenItem.Command = null;
                menuFlyOutOpenItem.CommandParameter = null;
                menuFlyOutDeleteItem.Command = null;
                menuFlyOutDeleteItem.CommandParameter = null;

                var selectedList = PdfListView.SelectedItems?
                                   .OfType<PdfItem>()
                                   .ToList() ?? new List<PdfItem>();

                List<PdfItem> selectedItems;
                if (selectedList.Count > 1)
                {
                    OpenMenu.Visibility = Visibility.Collapsed;
                    FlyoutSeperator.Visibility = Visibility.Collapsed;

                    selectedItems = selectedList;
                }
                else
                {
                    OpenMenu.Visibility = Visibility.Visible;
                    FlyoutSeperator.Visibility = Visibility.Visible;

                    var item = listViewItem.Content as PdfItem;
                    selectedItems = item is not null
                                  ? new List<PdfItem> { item }
                                  : new List<PdfItem>();
                }

                menuFlyOutOpenItem.CommandParameter = selectedItems;
                menuFlyOutOpenItem.Command = ViewModel.OpenFilesCommand;
                menuFlyOutDeleteItem.CommandParameter = selectedItems;
                menuFlyOutDeleteItem.Command = ViewModel.DeleteFilesCommand;
            }
        }

        private void ListView_OnDragCompleted(ListViewBase sender, DragItemsCompletedEventArgs e)
        {
            DispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Low, () =>
            {
                ViewModel.ReorderFilesCommand.Execute(null);
            });
        }
        #endregion
    }
}
