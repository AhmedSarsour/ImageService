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
 
    public class ImageController : IImageController
    {
        private IImageServiceModal m_modal;                      // The Modal Object
        private Dictionary<int, ICommand> commands;

        private class ThreadResult
        {
            public string ExcecuteResult { get; set; }
            public bool BoolResult { get; set; }
        }


        //We want to pass via the thread the boolean result and the string of the result so we need to define a struct.



        public ImageController(IImageServiceModal modal)
        {
            m_modal = modal;                    // Storing the Modal Of The System
            commands = new Dictionary<int, ICommand>();
            //First put into the dictionary the new file command.
            //We have the command id on CommandEnum
            commands.Add((int)CommandEnum.NewFileCommand, new NewFileCommand(m_modal));
        }

    
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            //First checks if our command exists
            if (commands.ContainsKey(commandID))
            {
                Task <ThreadResult> t = new Task<ThreadResult>(() =>
                {
                    bool b;
                    //Sleep to synchronyze between the threads.
                    Thread.Sleep(1000);
                    string ret = commands[commandID].Execute(args, out b);

                    ThreadResult r = new ThreadResult();
                    r.ExcecuteResult = ret;
                    r.BoolResult = b;
                    return r;
                });
                //Excecute the command from the dictionary.
                t.Start();
                ThreadResult result =  t.Result;
                resultSuccesful = result.BoolResult;
                return result.ExcecuteResult;

                
            }
            //The command does not exists so we will return a message for it.
            else
            {
                resultSuccesful = false;
                return "Command does not exists";
            }
        }
    }
}
