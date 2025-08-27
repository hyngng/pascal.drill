﻿using CommunityToolkit.WinUI.UI.Controls;

using Drill.ViewModels;

using Microsoft.UI.Xaml.Controls;

namespace Drill.Views;

public sealed partial class 목록세부정보Page : Page
{
    public 목록세부정보ViewModel ViewModel
    {
        get;
    }

    public 목록세부정보Page()
    {
        ViewModel = App.GetService<목록세부정보ViewModel>();
        InitializeComponent();
    }

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}
