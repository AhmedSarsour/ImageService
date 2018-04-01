using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    /// <summary>
    /// This class in charge of excecute the commands for now it holds a dictionary that contains only new file command.
    /// </summary>
    public class ImageController : IImageController
    {
        private IImageServiceModal m_modal;                      // The Modal Object
        private Dictionary<int, ICommand> commands;
        /// <summary>
        /// inner clase that has 2 properties
        /// </summary>
        private class ThreadResult
        {
            //The result after excecuting the command.
            public string ExcecuteResult { get; set; }
            //The out boolean value.
            public bool BoolResult { get; set; }
        }
        //We want to pass via the thread the boolean result and the string of the result so we need to define a struct.
        /// <summary>
        /// the ImageController constructor, creates the commands dictionary.
        /// </summary>
        /// <param name="modal">An image modal</param>
        public ImageController(IImageServiceModal modal)
        {
            m_modal = modal;                    // Storing the Modal Of The System
            commands = new Dictionary<int, ICommand>();
            //First put into the dictionary the new file command.
            //We have the command id on CommandEnum
            commands.Add((int)CommandEnum.NewFileCommand, new NewFileCommand(m_modal));
        }
        /// <summary>
        /// Executing the command according the ID with the help of the Command Dictionary.
        /// </summary>
        /// <param name="commandID">The id of the command</param>
        /// <param name="args">Arguments to the command</param>
        /// <param name="resultSuccesful">Set to true if the we succesed and false otherwise</param>
        /// <returns>The command result</returns>
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            //First checks if our command exists
            if (commands.ContainsKey(commandID))
            {
                //Creating task for each command to do it with threads.
                Task <ThreadResult> t = new Task<ThreadResult>(() =>
                {
                    bool b;
                    //Sleep to synchronyze between the threads.
                    Thread.Sleep(1000);
                    string ret = commands[commandID].Execute(args, out b);
                    ThreadResult r = new ThreadResult();
                    //The arguments we return to know the result.
                    r.ExcecuteResult = ret;
                    r.BoolResult = b;
                    return r;
                });
                //Excecute the command from the dictionary.(executing the above lines).
                t.Start();
                ThreadResult result =  t.Result;
                resultSuccesful = result.BoolResult;
                return result.ExcecuteResult;  
            }
            //The command does not exists so we will return a message for it.
            else
            {
                resultSuccesful = false;
                return "Command does not exist";
            }
        }
    }
}
