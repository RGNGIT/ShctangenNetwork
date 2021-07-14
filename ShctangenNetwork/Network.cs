using System;
using System.Net;
using System.Net.NetworkInformation;

namespace ShctangenNetwork
{
    class Network
    {

        public Network(NetworkCredential credential)
        {
            this.credential = credential;
        }

        NetworkCredential credential;

        public bool Ping(string URL)
        {
            Ping ping = new Ping();
            PingReply reply = ping.Send(URL);
            if (reply.Status == IPStatus.Success)
            {
                Console.WriteLine($"Сервер активен: {reply.RoundtripTime}мс");
                return true;
            }
            else
            {
                Console.WriteLine("Нет соединения!");
                return false;
            }
        }

        public byte[] GetInput(Uri InputUri)
        {
            WebClient Request = new WebClient()
            {
                Credentials = credential
            };
            try
            {
                byte[] InputData = Request.DownloadData(InputUri.ToString());
                return InputData;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошло исключение: {e}");
                return null;
            }
        }

        public void SendOutput(byte[] OutputData)
        {

        }

    }
}
