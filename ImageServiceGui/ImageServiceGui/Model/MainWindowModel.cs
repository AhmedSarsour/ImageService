using ImageServiceGui.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGui
{
    class MainWindowModel
    {
        private IModelCommunication communicate;
        /// <summary>
        /// MainWindowModel constructor.
        /// </summary>
        public MainWindowModel()
        {
            //getting the communication instance in order to know if failed or not.
            communicate = ModelCommunication.GetInstance();
        }
        /// <summary>
        /// only checking whether the connection is true or false.
        /// </summary>
        /// <returns></returns>
        public bool IsConnected()
        {
            return communicate.IsConnected();
        }

    }
}
