using Microsoft.UI.Composition.SystemBackdrops;
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
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Drill.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private const string ThemeKey = "AppTheme";

        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void SettingsPage_Loaded(object sender, RoutedEventArgs e)
        {
            var savedTheme = ApplicationData.Current.LocalSettings.Values[ThemeKey]?.ToString();
            var targetTag = !string.IsNullOrEmpty(savedTheme) ? savedTheme : "Default";

            var selectedItem = themeMode.Items
                .OfType<ComboBoxItem>()
                .FirstOrDefault(item => item.Tag.ToString() == targetTag);

            if (selectedItem != null)
                themeMode.SelectedItem = selectedItem;
        }

        private void themeMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (themeMode.SelectedItem is ComboBoxItem selectedItem)
            {
                string tag = selectedItem.Tag?.ToString() ?? "Default";

                if (this.XamlRoot.Content is FrameworkElement rootElement)
                {
                    switch (tag)
                    {
                        case "Light":
                            rootElement.RequestedTheme = ElementTheme.Light;
                            break;
                        case "Dark":
                            rootElement.RequestedTheme = ElementTheme.Dark;
                            break;
                        case "Default":
                        default:
                            rootElement.RequestedTheme = ElementTheme.Default;
                            break;
                    }
                }

                ApplicationData.Current.LocalSettings.Values[ThemeKey] = tag;
            }
        }
    }
}
