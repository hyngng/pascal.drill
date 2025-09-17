using System;
using System.Reflection.Metadata.Ecma335;

namespace Pascal.Services.LabsService;
public interface ILabsService
{
    event Action<bool> LabsChanged;

    bool IsLabsEnabled { get; }
    LabsService Initialize();
    void GetToggleSwitchStatus(ToggleSwitch toggleSwitch);
    void SetLabsEnabledStatus(bool isEnabled);
}
