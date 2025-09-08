using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Pascal.Views;
/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SettingsPage : Page
{
    public SettingsPage()
    {
        InitializeComponent();

        pageLoaded();
    }

    private void pageLoaded()
    {
        loadThemeMode();
        loadBackdropMode();
        loadLabsState();
    }

#region 테마 모드, 재질 효과 변환
    private void themeMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        App.Current.ThemeService.OnThemeComboBoxSelectionChanged(sender);
    }

    private void loadThemeMode()
    {
        App.Current.ThemeService.SetThemeComboBoxDefaultItem(themeMode);
    }

    private void backdrop_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        App.Current.ThemeService.OnBackdropComboBoxSelectionChanged(sender);
    }

    private void loadBackdropMode()
    {
        App.Current.ThemeService.SetBackdropComboBoxDefaultItem(backdropMode);
    }
#endregion

#region 실험실 기능 On/Off
    
    private void loadLabsState()
    {
        App.Current.LabsService.SetLabsStateToggleSwitchDefaultState(labsToggleSwitch);
    }

    private void labsToggleSwitch_Toggled(object sender, RoutedEventArgs e)
    {
        if (labsToggleSwitch.IsOn != App.Current.LabsService.IsLabsEnabled)
            App.Current.LabsService.SetLabsEnabledStatus(labsToggleSwitch.IsOn);
    }
#endregion
}
