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
                        int sizeSizeName = reader.ReadBytes(1)[0];

                        byte[] sizeName = reader.ReadBytes(sizeSizeName);
                        var str = System.Text.Encoding.Default.GetString(sizeName);
                 
                        int nBytes = int.Parse(str);
                        Console.WriteLine("Picture name has " + nBytes + " bytes");


                        byte[] readName = reader.ReadBytes(nBytes);
                        string picName = System.Text.Encoding.Default.GetString(readName);
                        Console.WriteLine("Picture name is " + picName);
                   
                        int sizeSizeImage  = reader.ReadBytes(1)[0];
                        byte[] sizeImage = reader.ReadBytes(sizeSizeImage);
                        str = System.Text.Encoding.Default.GetString(sizeImage);
                        nBytes = int.Parse(str);

                        Console.WriteLine("Picture size is " + nBytes + " bytes");



                        //Reading the image
                        Image photo;
                        MemoryStream ms = new MemoryStream(reader.ReadBytes(nBytes));
                        photo = Image.FromStream(ms);
                        Configure config = Configure.GetInstance();
                        string path = config.Handlers[0];
                        photo.Save(Path.Combine(path, picName));
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
    }
}
