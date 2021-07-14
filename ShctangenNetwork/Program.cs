using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ShctangenNetwork
{
    class Program
    {

        public Program()
        {

        }

        static void Main(string[] args)
        {
            Console.WriteLine("Введите адрес сервера");
            URL = Console.ReadLine();
            if (new Network(null).Ping(URL))
            {
                while (true) // Главный прослушивающий цикл
                {
                    IsCalculating();
                    ReadInput();
                    Thread.Sleep(1000);
                }
            }
        }

        static string URL;

        /*
         * Структура флагов
         * line 0 - Состояние вычислений
         * line 1 - Готовность сторон (0 - Сервер готов, 1 - Сервер считает)
        */

        static bool GaugeStarted = false;

        static void IsCalculating()
        {
            try
            {
                Network network = new Network(new System.Net.NetworkCredential("testuser", "12345678"));
                File.WriteAllBytes("GetFlags.shc", network.GetInput(new Uri($"ftp://{URL}/files/ShctangenNetwork/Flags.shc")));
                if (!GaugeStarted && File.ReadAllLines("GetFlags.shc")[0] == "1")
                {
                    Process.Start(@"GaugeBlockv3-1.exe");
                    GaugeStarted = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        static void ReadInput()
        {
            if (File.Exists("GetInput.shc"))
            {
                File.Delete("GetInput.shc");
            }
            try
            {
                Network network = new Network(new System.Net.NetworkCredential("testuser", "12345678"));
                File.WriteAllBytes("GetInput.shc", network.GetInput(new Uri($"ftp://{URL}/files/ShctangenNetwork/Input.shc")));
                foreach (string i in File.ReadAllLines("GetInput.shc"))
                {
                    Console.WriteLine(i); // Вызов расчета туть
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e); // Ожидание
            }
        }

        static void WriteOutput()
        {

        }

    }
}
