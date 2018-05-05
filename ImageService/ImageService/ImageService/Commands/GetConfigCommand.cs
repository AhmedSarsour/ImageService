using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Classes;
using ImageService.Infrastructure.Interfaces;
namespace ImageService.Commands
{
    public class GetConfigCommand:ICommand
    {
        /// <summary>
        /// The closeCommand constructor.
        /// </summary>
        /// <param name="handler">The handler we want to close.</param>
        public GetConfigCommand()
        {

        }
        /// <summary>
        /// we get the path of the directory, and call the onClose function by the handler.
        /// </summary>
        /// <param name="args">Arguments to the command.</param>
        /// <param name="result">Set to true if succeed and false otherwise</param>
        /// <returns>Message to the logger</returns>
        public string Execute(string[] args, out bool result)
        {
            //Our convention - the path will be stored on args[0]
            result = true;
            //The file path will be stored in args[0] so we will run the add file.
            Configure cnfg = new Configure(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath);
            return cnfg.ToJSON();
        }
    }
}
