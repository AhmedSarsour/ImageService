using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.Enums
{
    /// <summary>
    /// CommandEnum : Instead of numbers for each command that we will foget we will do enum.
    /// newFileCommand and CloseCommand currently
    /// </summary>
    public enum JsonEnum : int
    {
        SettingsJson,
        LogsJson
        
    }
}
