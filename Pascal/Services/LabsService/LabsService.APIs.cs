using Pascal.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pascal.Services.LabsService
{
    public partial class LabsService : ILabsService
    {
        public void GetToggleSwitchStatus(ToggleSwitch toggleSwitch)
        {
            toggleSwitch.IsOn = isLabsEnabled;
        }

        public void SetLabsEnabledStatus(bool isEnabled)
        {
            SetLabsConfiguration(isEnabled);
            ToggleLabsEnabledStatus();
        }
    }
}
