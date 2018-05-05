using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Enums;
namespace ImageService.Infrastructure.Classes
{
    public class Log
    {
        public string status;

        public string Type { get; private set; }
        public string Message { get; private set; }

        public Log(int type, string message)
        {
            switch (type) {
                case (int)MessageTypeEnum.INFO:
                    Type = "Info";
                    break;
                case (int)MessageTypeEnum.WARNING:
                    Type = "Warning";
                    break;
                case (int)MessageTypeEnum.FAIL:
                    Type = "Fail";
                    break;
                default:
                    Type = "Wrong type";
                    break;

            }
            this.Message = message;
        }
    }
}
