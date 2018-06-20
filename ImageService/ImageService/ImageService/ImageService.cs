using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ImageService.Logging;
using ImageService.Server;
using ImageService.Modal;
using ImageService.Controller;
using ImageService.Infrastructure.Classes;
using ImageService.Infrastructure.Enums;
using ImageService.Communication;
using ImageService.Commands;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Interfaces;

namespace ImageService
{
 
    /// <summary>
    /// Our image service - in charge of what to do when we do functions with the service
    /// </summary>
    public partial class ImageService : ServiceBase
    {
        public void RunAsConsole(string[] args)
        {
            OnStart(args);
            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
            OnStop();
        }


        //The states the service can be in.
        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };
        //Our classes that incharge of doing the functions of the service.
        private ImageServer m_imageServer;         
        private IImageServiceModal modal;
        private IImageController controller;
        //Our logging system
        private ILoggingService logging;
        private LogCollection logs;

        //The dictionary of handlers
        private Dictionary<string, IDirectoryHandler> handlers;

        private int eventId = 1;

        private TcpServer tcpServer;
        private TcpServer androidServer;

        /// <summary>
        /// The constructor of our class
        /// </summary>
        /// <param name="args">Arguments that contain the source and log name we want</param>
        /// <param name="iInput2"></param>
        public ImageService(string[] args)
        {
            InitializeComponent();
            //Getting the source name and log name via our arguments.
            string eventSourceName = "MySource";
            string logName = "MyNewLog";
            if (args.Count() > 0)
            {
                eventSourceName = args[0];
            }
            if (args.Count() > 1)
            {
                logName = args[1];
            }
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
            this.logs = new LogCollection();
  
        }

      
    

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }
        /// <summary>
        /// When we get invoked from the logger we do this - writes entry into the service.
        /// </summary>
        /// <param name="sender">Who called the event</param>
        /// <param name="e">Aruments of message recieved which contain status and message</param>
        private void OnMsg(object sender, MessageRecievedEventArgs e)
        {
            //This is another event id so i increase it
            this.eventId++;
            //After get invoking from the logging service we will write to the logger
            eventLog1.WriteEntry(e.Message + "\n\nWith status: " + e.Status, EventLogEntryType.Information, this.eventId);
            Console.WriteLine("Log: " +e.Message + "With status: " + e.Status);
            //Adding the log to the log collection
            logs.AddLog(new Log((int)e.Status, e.Message));
 
        }

        private void AddCloseCommand(object sender, int id)
        {
            if (id == (int)CommandEnum.CloseCommand)
            {
                if (sender is Dictionary<int, ICommand>)
                {
                    Dictionary<int, ICommand> commands = (Dictionary<int, ICommand>)sender;
                    commands.Add(id, new CloseCommand(ref handlers));

                    foreach (int key in commands.Keys)
                    {
                        Console.WriteLine("Key is " + key);
                    }

                }
            }

        }

        private void AddLogCommand(object sender, int id)
        {
            if (id == (int)CommandEnum.LogCommand)
            {
                if (sender is Dictionary<int, ICommand>)
                {
                    Dictionary<int, ICommand> commands = (Dictionary<int, ICommand>)sender;
                    commands.Add(id, new LogCommand(logs));

                }
            }
        }

        private void SendLogToClients(object sender, MessageRecievedEventArgs e)
        {
            Log log = new Log((int)e.Status, e.Message);
          //  tcpServer.SendToAllClients((int)SendClientEnum.AddLog, log.ToJSON());
        }

     
        /// <summary> 
        /// When we start the service this method is called.
        /// </summary>
        /// <param name="args">Arguments</param>
        protected override void OnStart(string[] args)
        {
            //Log for start
            eventLog1.WriteEntry("In OnStart", EventLogEntryType.Information, this.eventId);
            this.logs.AddLog(new Log((int)MessageTypeEnum.INFO, "In OnStart"));
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);


            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            //The logging service we made
            logging = new LoggingService();
            //Creating the log collection
            LogCollection logs = new LogCollection();
            //  tcpServer = TcpServer.GetInstance(8000, new AndroidHandler());
            androidServer = TcpServer.GetInstance(9222, new AndroidHandler());
            // Adding functions to the event when we get new log
            logging.MessageRecieved += OnMsg;
            //    logging.MessageRecieved += logs.AddLog;
            logging.MessageRecieved += SendLogToClients;

            //Creating our all system members - the model server and controller
            this.modal = new ImageServiceModal();
            //We will create the controller
            this.controller = new ImageController(this.modal);

            //  tcpServer.ExcecuteCommand += controller.ExecuteCommand;
            //The server is in task because we want it background
            //   Task t = new Task(tcpServer.Start);
            Task t = new Task(androidServer.Start);
            t.Start();
            this.m_imageServer = new ImageServer(this.controller, this.logging);
            Configure configs = Configure.GetInstance();

             handlers = new Dictionary<string, IDirectoryHandler>();
            //Creating the handlers to each folder than configured in the app config.
            foreach (string handler in configs.AllHandlers)
            {
                IDirectoryHandler h = m_imageServer.createHandler(handler);

                if (h != null)
                {
                    Console.WriteLine("Adding the handler " + handler);
                    handlers.Add(handler, h);
                }
            }
            //Adding to the controller the command of close command
            controller.AddCommand += AddCloseCommand;
            controller.AddCommand += AddLogCommand;
            controller.AddAditionalCommands((int)CommandEnum.CloseCommand);
            //Adding the log comand
            controller.AddAditionalCommands((int)CommandEnum.LogCommand);


        }
        /// <summary>
        /// When we stop the service this method is called.
        /// </summary>
        protected override void OnStop()
        {
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            //Closes all the handlers by our server.
            this.m_imageServer.sendCommand();

            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLog1.WriteEntry("In onStop.", EventLogEntryType.Information, ++this.eventId);
            this.logs.AddLog(new Log((int)MessageTypeEnum.INFO, "In onStop"));
            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }
        protected override void OnContinue()
        {
            eventLog1.WriteEntry("In OnContinue.");
        }
        protected override void OnPause()
        {
            eventLog1.WriteEntry("In OnPause.");
        }

        protected override void OnShutdown()
        {
            eventLog1.WriteEntry("In OnShutdown.");
        }

        private void eventLog1_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }
    }
}
