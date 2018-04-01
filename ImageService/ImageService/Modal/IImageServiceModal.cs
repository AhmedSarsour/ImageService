using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    // <summary>
    /// An imageservice model which has function that will handle of handling file.
    /// </summary>
    public interface IImageServiceModal
    {
        /// <summary>
        /// The Function Adds A file to the system and do whatever the class which implements the interface want.
        /// </summary>
        /// <param name="path">The Path of the Image from the file</param>
        /// <param name="result">It will be set to indication if the Addition Was Successful</param>
        /// <returns>A string the class implements it decide it meaning</returns>
        string AddFile(string path, out bool result);
    }
}
