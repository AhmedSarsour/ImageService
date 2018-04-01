using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ImageService.Commands
{
    /// <summary>
    /// This interface represents a command with id and argument we can give
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// The Function That will Execute The command according to args.
        /// </summary>
        /// <param name="args">Arguments to the command.</param>
        /// <param name="result">Set to true if succeed and false otherwise</param>
        /// <returns>Message</returns>
        string Execute(string[] args, out bool result);          // 
    }
}
