using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Pascal.Models;
using Pascal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;

namespace Pascal.Views.Pages.PdfEditPages
{
    public sealed partial class PdfMergePage : Page
    {
        public PdfMergePageViewModel ViewModel { get; }

        public PdfMergePage()
        {
            ViewModel = App.GetService<PdfMergePageViewModel>();
            this.InitializeComponent();
        }

        #region 파일 드래그&드롭
        private void PdfListView_OnDragOver(object sender, DragEventArgs e)
        {
            if (ViewModel.IsNotBusy && e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                e.AcceptedOperation               = DataPackageOperation.Copy;
                e.DragUIOverride.Caption          = "PDF 추가";
                e.DragUIOverride.IsCaptionVisible = true;
                e.Handled                         = true;
            }
        }

        private async void PdfListView_OnDrop(object sender, DragEventArgs e)
        {
            if (ViewModel.IsBusy || !e.DataView.Contains(StandardDataFormats.StorageItems))
                return;

            var storageItems = await e.DataView.GetStorageItemsAsync();
            var pdfPaths = storageItems.OfType<StorageFile>()
                .Where(file => file.FileType.Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                .Select(file => file.Path)
                .Where(path => !string.IsNullOrEmpty(path))
                .ToList();

            if (pdfPaths.Count == 0)
                return;

            await ViewModel.AddPdfFilesFromPathsAsync(pdfPaths);
        }
        #endregion 파일 드래그&드롭

        #region 항목 우클릭 메뉴
        private void ItemMenuFlyout_Opening(object sender, object e)
        {
            var menuFlyout = sender as MenuFlyout;
            if (menuFlyout?.Target is ListViewItem listViewItem)
            {
                var openItem = menuFlyout.Items.OfType<MenuFlyoutItem>().FirstOrDefault(mi => (mi.Tag as string) == "OpenMenu");
                var deleteItem = menuFlyout.Items.OfType<MenuFlyoutItem>().FirstOrDefault(mi => (mi.Tag as string) == "DeleteMenu");
                if (deleteItem is null || openItem is null) return;

                openItem.Command = null;
                openItem.CommandParameter = null;
                deleteItem.Command = null;
                deleteItem.CommandParameter = null;

                var selectedList = PdfListView.SelectedItems?.OfType<PdfItem>().ToList() ?? new List<PdfItem>();
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
                    selectedItems = item is not null ? new List<PdfItem> { item } : new List<PdfItem>();
                }

                openItem.CommandParameter = selectedItems;
                openItem.Command = ViewModel.OpenPdfFilesCommand;
                deleteItem.CommandParameter = selectedItems;
                deleteItem.Command = ViewModel.DeletePdfFilesCommand;
            }
        }
        #endregion 항목 우클릭 메뉴

        #region 드래그 앤 드롭으로 항목 순서 변경
        private void ListView_OnDragCompleted(ListViewBase sender, DragItemsCompletedEventArgs e)
        {
            DispatcherQueue.TryEnqueue(Microsoft.UI.Dispatching.DispatcherQueuePriority.Low, () =>
            {
                ViewModel.ReorderPdfFilesCommand.Execute(null);
            });
        }
        #endregion 드래그 앤 드롭으로 항목 순서 변경
    }
}
