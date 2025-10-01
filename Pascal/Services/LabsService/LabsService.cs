using Microsoft.UI.Xaml;
using System;
using Windows.Storage;
using Pascal.Internal;

namespace Pascal.Services.LabsService;
public partial class LabsService : ILabsService
{
    private readonly string ConfigFilePath = "CoreAppConfigV9.0.0.json";
    private string? userDefinedFileName = null;
    private bool useAutoSave = true;
    private bool isLabsEnabled;

    public event Action<bool>? LabsChanged;

    public bool IsLabsEnabled
    {
        get => isLabsEnabled;
        private set
        {
            if (isLabsEnabled != value)
            {
                isLabsEnabled = value;
                LabsChanged?.Invoke(isLabsEnabled);
            }
        }
    }

    public LabsService Initialize()
    {
        string RootPath = Path.Combine(PathHelper.GetAppDataFolderPath(), ProcessInfoHelper.ProductNameAndVersion);
        string AppConfigPath = Path.Combine(RootPath, ConfigFilePath);

        if (useAutoSave)
        {
            if (!string.IsNullOrEmpty(userDefinedFileName))
                AppConfigPath = userDefinedFileName;

            GlobalData.SavePath = AppConfigPath;

            if (!Directory.Exists(RootPath))
                Directory.CreateDirectory(RootPath);

            GlobalData.Init();
        }

        // 여기부터는 커스텀 로직
        IsLabsEnabled = GetLabsConfiguration();

        return this;
    }
}