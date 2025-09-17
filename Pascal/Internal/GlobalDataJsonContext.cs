using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Pascal.Internal;
[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(AppConfig))]
internal partial class GlobalDataJsonContext : JsonSerializerContext
{
    /// <summary>
    /// DevWinUI.GlobalDataJsonContext인데, internal로 되어있어서 Pascal.Internal로 복사해옴.
    /// </summary>
}
