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
            System.Diagnostics.Debug.WriteLine($"����2.4 {sender.GetType()}");

            var menuFlyout = sender as MenuFlyout;

            // 1. �޴��� ����� ���(��, ListViewItem)�� �����ɴϴ�.
            if (menuFlyout?.Target is FrameworkElement targetElement)
            {
                // 2. ListViewItem�� DataContext (��, PdfItemToMerge ��ü)�� �����ɴϴ�.
                var dataContext = targetElement.DataContext;

                // 3. �޴��� ���Ե� ��� �׸�(MenuFlyoutItem ��)�� ��ȸ�մϴ�.
                foreach (var item in menuFlyout.Items)
                {
                    // 4. �� �׸��� DataContext�� ListViewItem�� DataContext�� �����մϴ�.
                    //    �̰��� �ٷ� �������� DataContext ����� �������� �̾��ִ� �����Դϴ�.
                    if (item is FrameworkElement flyoutItem)
                    {
                        flyoutItem.DataContext = dataContext;
                    }
                }
            }
        }

        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"����2 {sender.GetType()}");

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
