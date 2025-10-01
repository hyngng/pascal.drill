using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Pascal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pascal.Views.Pages.PdfEditPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PdfSplitPage : Page
    {
        public PdfSplitPageViewModel ViewModel { get; } = new();

        public PdfSplitPage()
        {
            InitializeComponent();
        }

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
                                   .OfType<PdfItemToMerge>()
                                   .ToList() ?? new List<PdfItemToMerge>();

                List<PdfItemToMerge> selectedItems;
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

                    var item = listViewItem.Content as PdfItemToMerge;
                    selectedItems = item is not null
                                  ? new List<PdfItemToMerge> { item }
                                  : new List<PdfItemToMerge>();
                }

                menuFlyOutOpenItem.CommandParameter = selectedItems;
                menuFlyOutOpenItem.Command = ViewModel.OpenFilesCommand;
                menuFlyOutDeleteItem.CommandParameter = selectedItems;
                menuFlyOutDeleteItem.Command = ViewModel.DeleteFilesCommand;
            }
        }
        #endregion
    }
}
