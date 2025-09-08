using System;

namespace Pascal.Services.LabsService;
public interface ILabsService
{
    event Action<bool> LabsChanged;
    bool IsLabsEnabled { get; }
    LabsService Initialize();
    void SetLabsStateToggleSwitchDefaultState(ToggleSwitch toggleSwitch);
    void SetLabsEnabledStatus(bool isEnabled);
}
