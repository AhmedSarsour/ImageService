using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure.Enums
{
    public enum SendClientEnum : int
    {
        AddLog,
        //Same ass commandenum
        RemoveHandler = 1,
        GetConfig = 2,
        GetLogs = 3,
    }
}
