using Drill.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.ApplicationSettings;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Drill
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private const string ThemeKey = "AppTheme";

        public MainWindow()
        {
            InitializeComponent();

            // TitleBar 비활성화
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(CustomTitleBar);

            // Theme 불러오기
            LoadAndApplyTheme();

            // 화면 전환 효과
            // ContentFrame.Navigate(typeof(SamplePage), null, new DrillInNavigationTransitionInfo());
        }

        private void NavSelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem item)
            {
                switch (item.Tag.ToString())
                {
                    case "Settings":
                        RootFrame.Navigate(typeof(SettingsPage));
                        break;
                    case "PdfEdit":
                        RootFrame.Navigate(typeof(PdfEditPage));
                        break;
                    case "MaybeLater":
                        RootFrame.Navigate(typeof(MaybeLaterPage));
                        break;
                    case "Credit":
                        RootFrame.Navigate(typeof(CreditPage));
                        break;
                }
            }
        }

        #region TitleBar
        private void TitleBar_BackRequested(TitleBar sender, object args)
        {
            if (RootFrame.CanGoBack)
                RootFrame.GoBack();
        }

        private void TitleBar_PaneToggleRequested(TitleBar sender, object args)
        {
            RootNavigationView.IsPaneOpen = !RootNavigationView.IsPaneOpen;
        }
        #endregion TitleBar

        private void LoadAndApplyTheme()
        {
            var savedTheme = ApplicationData.Current.LocalSettings.Values[ThemeKey]?.ToString() ?? "Default";

            if (this.Content is FrameworkElement rootElement)
            {
                switch (savedTheme)
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
        }
    }
}
