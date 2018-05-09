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


            TcpClientChannel client = TcpClientChannel.GetInstance();
            TcpClientChannel.Connect(8000);
            //    string result = client.sendCommand((int)CommandEnum.LogCommand, new string[] { "a" });

            //   Console.WriteLine("Result: " + client.sendCommand((int)CommandEnum.CloseCommand, new string[] { @"C:\aa", "true" }));
            //Console.WriteLine("Result: " + result );

            //LogCollection logs = new LogCollection();
            //logs.FromJson(result);
            //Console.WriteLine(logs.ToJSON());
            //Configure config = Configure.GetInstance();
            //config.FromJson(result);

            //Console.WriteLine("Now");
            //Console.WriteLine(config.ToJSON());


            //Task t = new Task(() =>
            //{
            //    while (true)
            //    {
            //        Console.WriteLine(client.recieveMessage());
            //    }
            //});
            //t.Start();


            ////    t.Start();
            //    t.Wait();
            //    Console.WriteLine("Server sent: " + client.recieveMessage());


            //    // t.Wait();
            //    //    t.Wait();
            //}


            client.close();


        }
    }
}
