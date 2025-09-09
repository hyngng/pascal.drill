using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using System.Windows.Input;
using Pascal.Models;
using System.Collections.ObjectModel;
using Pascal.Services;
using Pascal.ViewModels;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pascal.Views.PdfEditPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class PdfMergePage : Page, IFilePickerService
    {
        public PdfMergePageViewModel ViewModel { get; }

        public PdfMergePage()
        {
            this.InitializeComponent();
            ViewModel = new PdfMergePageViewModel(this);
        }

        private void ListView_OnDragStarted(object sender, DragStartedEventArgs e)
        {
            //
        }

        private void ListView_OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            //ViewModel.RenumberFilesCommand.Execute(null);
        }

        // MVVM 모델에 따라 코드비하인드 대신 뷰모델에 작성하는게 좋을듯?
        private async void PickPdfFilesButton_Click(object sender, RoutedEventArgs e)
        {
            //disable the button to avoid double-clicking
            var senderButton = sender as Button;
            if (senderButton != null)
                senderButton.IsEnabled = false;

            // Clear previous returned file name, if it exists, between iterations of this scenario
            // PickFilesOutputTextBlock.Text = "";

            // Create a file picker
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            // See the sample code below for how to make the window accessible from the App class.
            var window = App.MainWindow;

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

            // Set options for your file picker
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".pdf");

            // Open the picker for the user to pick a file
            IReadOnlyList<StorageFile> files = await openPicker.PickMultipleFilesAsync();
            if (files.Count > 0)
            {
                StringBuilder output = new StringBuilder("Picked files:\n");
                foreach (StorageFile file in files)
                    output.Append(file.Name + "\n");
                // PickFilesOutputTextBlock.Text = output.ToString();
            }
            else
            // PickFilesOutputTextBlock.Text = "Operation cancelled.";

            //re-enable the button
            if (senderButton != null)
                senderButton.IsEnabled = true;
        }

        private async void SaveAMergedPdfFileButton_Click(object sender, RoutedEventArgs e)
        {
            //disable the button to avoid double-clicking
            var senderButton = sender as Button;
            if (senderButton != null) 
                senderButton.IsEnabled = false;

            // Clear previous returned file name, if it exists, between iterations of this scenario
            // SaveFileOutputTextBlock.Text = "";

            // Create a file picker
            FileSavePicker savePicker = new Windows.Storage.Pickers.FileSavePicker();

            // See the sample code below for how to make the window accessible from the App class.
            var window = App.MainWindow;

            // Retrieve the window handle (HWND) of the current WinUI 3 window.
            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(window);

            // Initialize the file picker with the window handle (HWND).
            WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hWnd);

            // Set options for your file picker
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("PDF Document", new List<string>() { ".pdf" });
            savePicker.SuggestedFileName = "merged-document";

            // Open the picker for the user to pick a file
            StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until we finish making changes and call CompleteUpdatesAsync.
                CachedFileManager.DeferUpdates(file);

                // Another way to write a string to the file is to use this instead:
                // await FileIO.WriteTextAsync(file, "Example file contents.");

                // Let Windows know that we're finished changing the file so the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                FileUpdateStatus status = await CachedFileManager.CompleteUpdatesAsync(file);
                if (status == FileUpdateStatus.Complete)
                {
                    // SaveFileOutputTextBlock.Text = "File " + file.Name + " was saved.";
                }
                else if (status == FileUpdateStatus.CompleteAndRenamed)
                {
                    // SaveFileOutputTextBlock.Text = "File " + file.Name + " was renamed and saved.";
                }
                else
                {
                    // SaveFileOutputTextBlock.Text = "File " + file.Name + " couldn't be saved.";
                }
            }
            else
            {
                // SaveFileOutputTextBlock.Text = "Operation cancelled.";
            }

            //re-enable the button
            if (senderButton != null) 
                senderButton.IsEnabled = true;
        }

        // IFilePickerService 구현은 그대로 둡니다.
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
