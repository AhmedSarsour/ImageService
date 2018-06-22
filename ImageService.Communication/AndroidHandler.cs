using ImageService.Communication.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Drawing;
using ImageService.Infrastructure.Classes;
using System.Threading;

namespace ImageService.Communication
{
    public class AndroidHandler : IClientHandler
    {
        public event Excecute HandlerExcecute;
        public AndroidHandler()
        {

        }
        public void HandleClient(TcpClient client, object locker)
        {

            //creating a task that will handle the client's commands.
            Task t = new Task(() =>
            {
                NetworkStream stream = client.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                BinaryWriter writer = new BinaryWriter(stream);
                while (true)
                {
                    //Getting the command.
                    //readMutex.WaitOne();
                    /**
                       * first byte has the size of the picture.
                       * Than we get the size of the size of the image in the byte array.
                       * Than we get the size of the image.
                       * after the firsty comes the picture as bytes.
                       * after building the picture from its bytes, we move it to one of the handled folders (haha, aa, ...)
                       * and thats it.
                       * */
                    byte[] check = reader.ReadBytes(1);

                    if (check[0] == 1)
                    {
                        Console.WriteLine("\nStarting to transfer new picture!!");
                        string picName = ReadString(reader);
                        Console.WriteLine("Pic name is " + picName);
                        int nBytes;
                    
                   
                        int sizeSizeImage  = reader.ReadBytes(1)[0];
                        byte[] sizeImage = reader.ReadBytes(sizeSizeImage);
                        var str = System.Text.Encoding.Default.GetString(sizeImage);
                        nBytes = int.Parse(str);

                        Console.WriteLine("Picture size is " + nBytes + " bytes");



                        //Reading the image
                        Image photo;
                        MemoryStream ms = new MemoryStream(reader.ReadBytes(nBytes));
                        photo = Image.FromStream(ms);

                        string date = ReadString(reader);

                        Configure config = Configure.GetInstance();
                        string path = config.Handlers[0];
                        string picPath = Path.Combine(path, picName);
                        photo.Save(picPath);
                        Console.WriteLine("Date is " + date);
                        DateTime taken = DateTime.ParseExact(date, "MM/dd/yyyy HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);
                        try
                        {
                            File.SetLastWriteTime(picPath, taken);
                        } catch(Exception)
                        {

                        }
                        Thread.Sleep(2000);
                        continue;

                    }
                    //Finish connection
                    if (check[0] == 0)
                    {
                        Console.WriteLine("\nFinished");
                        break;
                    }
                    //Locking this critical place
                    //lock (locker)
                    //{
                    //    writer.Write(message.ToJSON());
                    //}
                }
                //client.Close();
            });
            t.Start();




        }
        //Reading string by bytes
        private string ReadString(BinaryReader reader) {
            int sizeSizeString = reader.ReadBytes(1)[0];

            byte[] sizeString = reader.ReadBytes(sizeSizeString);
            var s = System.Text.Encoding.Default.GetString(sizeString);

            int nBytes = int.Parse(s);


            byte[] readString = reader.ReadBytes(nBytes);
            string str = System.Text.Encoding.Default.GetString(readString);

            return str;
        }
    }
}
