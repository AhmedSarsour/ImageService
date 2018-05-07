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
using ImageService.Infrastructure.Classes;
using System.Configuration;
namespace TestClient
{
    class Program
    {
        public static  void UpdateSetting(string key, string value)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.Settings[key].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }


        static void Main(string[] args)
        {


            TcpClientChannel client = TcpClientChannel.GetInstance(8000);
            client.Connect();
            string str = "";

         //   Console.WriteLine("Result: " + client.sendCommand((int)CommandEnum.CloseCommand, new string[] { @"C:\aa", "true" }));

            //while (true)
            //{
            //    Task t = new Task(() =>
            //    {
            //        str = Console.ReadLine();
            //        int c = int.Parse(str[0] + "");
            //        Console.WriteLine("Result: " + client.sendCommand(c, new string[] { "ab", "cd" }));
            //    });


            //    t.Start();
            //    t.Wait();
            //    Console.WriteLine("Server sent: " + client.recieveMessage());


            //    // t.Wait();
            //    //    t.Wait();
            //}


            client.close();


        }
    }
}
