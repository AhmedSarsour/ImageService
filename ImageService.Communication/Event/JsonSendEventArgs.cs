using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication.Event
{
    public class JsonSendEventArgs:EventArgs
    {
        public int Id { get; set; }      // The Command ID
        /// <summary>
        /// the Constructor, setting the values.
        /// </summary>
        /// <param name="id">The id of the command we recieved</param>
        public JsonSendEventArgs(int id)
        {
            Id = id;
        }
    }
}
