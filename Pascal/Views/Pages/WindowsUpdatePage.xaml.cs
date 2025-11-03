using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Pascal.Models;
using Pascal.Views.SubPages;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics;
using Pascal.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pascal.Views.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WindowsUpdatePage : Page
    {
        public WindowsUpdatePageViewModel ViewModel { get; }

        public WindowsUpdatePage()
        {
            ViewModel = App.GetService<WindowsUpdatePageViewModel>();
            InitializeComponent();
        }

        private void ActivateWindowsUpdateWindow(object sender, RoutedEventArgs e)
        {
            var monitors = Models.Monitor.All.ToArray();

            if (monitors.Length > 1)
            {
                for (int i = 0; i < monitors.Length; i++)
                {
                    WindowsUpdateWindow window = new WindowsUpdateWindow();

                    window.UIContent.Visibility = monitors[i].IsPrimary ? Visibility.Visible : Visibility.Collapsed;
                    
                    window.Activate();
                    
                    // 각 모니터의 WorkingArea를 직접 사용
                    window.AppWindow.Move(new PointInt32(monitors[i].WorkingArea.X, monitors[i].WorkingArea.Y));
                }
            }
            else
            {
                WindowsUpdateWindow window = new WindowsUpdateWindow();
                window.UIContent.Visibility = Visibility.Visible;
                window.Activate();
            }
        }
    }
}
