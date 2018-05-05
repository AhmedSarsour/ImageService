using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.Enums
{
    /// <summary>
    /// Message enum, the Ifno and warning and fail statuses.
    /// </summary>
    public enum MessageTypeEnum : int
    {
        INFO,
        WARNING,
        FAIL
    }
}
