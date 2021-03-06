using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace ShctangenNetwork
{
    class Network
    {

        public Network(NetworkCredential credential, string URL)
        {
            this.credential = credential;
            this.URL = URL;
        }

        NetworkCredential credential;
        string URL;

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

        public void Delete(Uri ServerUri) 
        {
            try
            {
                FtpWebRequest Request = WebRequest.Create(ServerUri) as FtpWebRequest;
                Request.Credentials = credential;
                Request.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse Response = Request.GetResponse() as FtpWebResponse;
                Response.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Произошло исключение: {e.Message}");
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
                if (e.HResult != -2146233079)
                {
                    Console.WriteLine($"Произошло исключение: {e.Message}");
                }
                return null;
            }
        }

        public void SendOutput(byte[] OutputData, string ID)
        {
            try
            {
                string PFN = new FileInfo("Output.wshc").Name;
                string UploadURL = $"ftp://{URL}/files/ShctangenNetwork/{ID}/{PFN}";
                FtpWebRequest request = WebRequest.Create(UploadURL) as FtpWebRequest;
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = credential;
                request.Proxy = null;
                request.KeepAlive = true;
                request.UseBinary = true;
                byte[] fileContents = OutputData;
                request.ContentLength = fileContents.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Console.WriteLine(response.StatusDescription);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Произошло исключение: {e.Message}");
            }
        }

        public string CreateSessionDir(string ID)
        {
            try
            {
                FtpWebRequest Request = WebRequest.Create($"ftp://{URL}/files/ShctangenNetwork/{ID}") as FtpWebRequest;
                Request.Method = WebRequestMethods.Ftp.MakeDirectory;
                Request.Credentials = credential;
                FtpWebResponse Response = Request.GetResponse() as FtpWebResponse;
                return $"Директория сессии успешно создана (ShctangenNetwork/{ID})!";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

        public string RemoveSessionDir(string ID)
        {
            try
            {
                FtpWebRequest Request = WebRequest.Create($"ftp://{URL}/files/ShctangenNetwork/{ID}/") as FtpWebRequest;
                Request.Method = WebRequestMethods.Ftp.RemoveDirectory;
                Request.Credentials = credential;
                FtpWebResponse Response = Request.GetResponse() as FtpWebResponse;
                return $"Директория сессии успешно очищена!";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }

    }
}
