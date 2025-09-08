using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pascal.Internal
{
    /// <summary>
    /// DevWinUI.AppConfig인데, internal로 되어있어서 Pascal.Internal로 복사해옴.
    /// </summary>
    internal partial class AppConfig
    {
        public ElementTheme ElementTheme { get; set; } = ElementTheme.Default;
        public BackdropType BackdropType { get; set; } = BackdropType.Mica;

        // 여기부터는 커스텀
        public bool IsLabsEnabled { get; set; } = false;
    }
}
