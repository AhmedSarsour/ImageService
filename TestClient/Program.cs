using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure.Interfaces;
using ImageService.Communication;
using ImageService.Infrastructure.Enums;
using ImageService.Communication.Interfaces;
using System.Threading;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClientChannel client = new TcpClientChannel(8000);
            client.Connect();
            string str = "";
            Task mainTask = new Task(() =>
            {
                while (true)
                {
              

                    Console.WriteLine("Write command: ");
                    if ((str = Console.ReadLine()) != "e")
                    {
                        Task task = new Task(() => {

                            int c = int.Parse(str[0] + "");
                   
                        Console.WriteLine("Result: " + client.sendCommand(c, new string[] { "ab", "cd" }));
                        });
                        task.Start();
                        task.Wait();
                    }
                      
                     //   task.Wait();
                

                    Task t = new Task(() =>
                    {
                            Console.WriteLine("Server sent: " + client.recieveMessage());
                        
                    });
                    t.Start();
                   // t.Wait();
                    //    t.Wait();
                }
            });
            mainTask.Start();
            mainTask.Wait();

                client.close();

         
        }
    }
}
