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
        #region 클래스 외부 API
        public void SetLabsStateToggleSwitchDefaultState(ToggleSwitch toggleSwitch)
        {
            toggleSwitch.IsOn = isLabsEnabled;
        }

        public void SetLabsEnabledStatus(bool isEnabled)
        {
            ToggleLabsEnabledStatus();
            SetLabsConfiguration(isEnabled);
        }
        #endregion 클래스 외부 API

        #region 클래스 내부 API
        private void SetLabsConfiguration(bool isEnabled)
        {
            try
            {
                GlobalData.Config.IsLabsEnabled = isEnabled;
                GlobalData.Save();
            }
            catch { }
        }

        private bool GetLabsConfiguration()
        {
            return GlobalData.Config?.IsLabsEnabled ?? false;
        }

        private void ToggleLabsEnabledStatus()
        {
            IsLabsEnabled = !IsLabsEnabled;
        }
        #endregion 클래스 내부 API
    }
}
