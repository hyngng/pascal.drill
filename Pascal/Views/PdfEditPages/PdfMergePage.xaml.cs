using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Pascal.Models;
using Pascal.Services;
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

namespace Pascal.Views.PdfEditPages
{
    public sealed partial class PdfMergePage : Page, IFilePickerService
    {
        public PdfMergePageViewModel ViewModel { get; }

        public PdfMergePage()
        {
            this.InitializeComponent();
            ViewModel = new PdfMergePageViewModel(this);
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // ...
        }

        private void ItemMenuFlyout_Opening(object sender, object e)
        {
            System.Diagnostics.Debug.WriteLine($"ㅋㅋ2.4 {sender.GetType()}");

            var menuFlyout = sender as MenuFlyout;

            // 1. 메뉴가 연결된 대상(즉, ListViewItem)을 가져옵니다.
            if (menuFlyout?.Target is FrameworkElement targetElement)
            {
                // 2. ListViewItem의 DataContext (즉, PdfItemToMerge 객체)를 가져옵니다.
                var dataContext = targetElement.DataContext;

                // 3. 메뉴에 포함된 모든 항목(MenuFlyoutItem 등)을 순회합니다.
                foreach (var item in menuFlyout.Items)
                {
                    // 4. 각 항목의 DataContext를 ListViewItem의 DataContext로 설정합니다.
                    //    이것이 바로 끊어졌던 DataContext 상속을 수동으로 이어주는 과정입니다.
                    if (item is FrameworkElement flyoutItem)
                    {
                        flyoutItem.DataContext = dataContext;
                    }
                }
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"ㅋㅋ2 {sender.GetType()}");

            var menuItem = sender as MenuFlyoutItem;

            if (menuItem?.Parent is MenuFlyout menuFlyout)
            {
                if (menuFlyout.Target is FrameworkElement targetItem)
                {
                    if (targetItem.DataContext is PdfItemToMerge itemToDelete)
                    {
                        if (ViewModel.DeleteFileCommand.CanExecute(itemToDelete))
                        {
                            ViewModel.DeleteFileCommand.Execute(itemToDelete);
                        }

                        return;
                    }
                }
            }
        }

        private void ListView_OnDragCompleted(ListViewBase sender, DragItemsCompletedEventArgs e)
        {
            DispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Low, () =>
            {
                ViewModel.ReorderFilesCommand.Execute(null);
            });
        }

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
            savePicker.SuggestedFileName = "merged-document";

            return await savePicker.PickSaveFileAsync();
        }
    }
}
