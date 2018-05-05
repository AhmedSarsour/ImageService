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
                    Type = "INFO";
                    break;
                case (int)MessageTypeEnum.WARNING:
                    Type = "WARRNING";
                    break;
                case (int)MessageTypeEnum.FAIL:
                    Type = "FAIL";
                    break;
                default:
                    Type = "Wrong type";
                    break;

            }
            this.Message = message;
        }
    }
}
