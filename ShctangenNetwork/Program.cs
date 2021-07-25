using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using ShctangenNetLib;

namespace ShctangenNetwork
{
    class Program
    {

        public Program()
        {
            CleanInput();
        }

        static void Main(string[] args)
        {
            Console.CancelKeyPress += Console_CancelKeyPress;
            Console.WriteLine("Введите адрес сервера-моста");
            URL = Console.ReadLine();
            Console.WriteLine("Придумайте идентификатор сессии");
            ID += Console.ReadLine();
            ControlSession(true);
            if (new Network(null, null).Ping(URL))
            {
                Console.WriteLine(
                    "Для завершения работы сервера нажмите Ctrl + C или Ctrl + Break\n" +
                    "Ожидание реквеста с клиента...");
                while (true) // Главный прослушивающий цикл
                {
                    Listen();
                    Thread.Sleep(1000);
                }
            }
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            if (ID != "_shctangenNetworkSessionId_") {
                ControlSession(false);
                Console.WriteLine("Завершение...");
                Environment.Exit(-1);
            }
        }

        static string URL, ID = "_shctangenNetworkSessionId_";

        static System.Net.NetworkCredential credential = new System.Net.NetworkCredential()
        {
            UserName = "testuser",
            Password = "12345678"
        };

        static void Listen()
        {
            try
            {
                File.WriteAllBytes("GetInput.wshc", new Network(credential, URL).GetInput(new Uri($"ftp://{URL}/files/ShctangenNetwork/{ID}/Input.wshc")));
                Thread.Sleep(1000);
                SetDB();
                StartGauge();
                Console.WriteLine(
                    "Вроде прогнал циклы. Пусть клиент зачекает\n" +
                    "Для завершения работы сервера нажмите Ctrl + C или Ctrl + Break\n" +
                    "Ожидание реквеста с клиента...");
                CleanInput();
                CleanServer();
                Thread.Sleep(3000);
            }
            catch(Exception e)
            {
                // Console.WriteLine(e);
            }
        }

        static void StartGauge()
        {
            Console.WriteLine("Получили базу первичных расчетов. Стартуем прогу школьников и запускаем циклы...");
            Calc calc = new Calc();
            calc.Entry = GetBlock().Entry;
            Process.Start("GaugeBlockv3-1.exe");
            calc.ProgramCycles();
            new Network(credential, URL).SendOutput(File.ReadAllBytes("SetOutput.wshc"), ID);
        }

        static void CleanInput()
        {
            if (File.Exists("GetInput.wshc"))
            {
                File.Delete("GetInput.wshc");
            }
        }

        static void CleanServer()
        {
            new Network(credential, URL).Delete(new Uri($"ftp://{URL}/files/ShctangenNetwork/{ID}/Input.wshc"));
        }

        static void ControlSession(bool OnCreate)
        {
            if(OnCreate)
            {
                Console.WriteLine(new Network(credential, URL).CreateSessionDir(ID));
            }
            else
            {
                Console.WriteLine(new Network(credential, URL).RemoveSessionDir(ID));
            }
        }

        static BinaryFormatter binaryFormatter = new BinaryFormatter();

        static DataBlock GetBlock()
        {
            DataBlock DeserializeBlock;
            using (FileStream fileStream = new FileStream("GetInput.wshc", FileMode.OpenOrCreate))
            {
                DeserializeBlock = binaryFormatter.Deserialize(fileStream) as DataBlock;
            }
            return DeserializeBlock;
        }

        static void SetDB()
        {
            Database.o = GetBlock().o;
            Database.o1 = GetBlock().o1;
            Database.o2 = GetBlock().o2;
            Database.o3 = GetBlock().o3;
            Database.o4 = GetBlock().o4;
            Database.o5 = GetBlock().o5;
            Database.p1 = GetBlock().p1;
            Database.p2 = GetBlock().p2;
            Database.p3 = GetBlock().p3;
            Database.p4 = GetBlock().p4;
            Database.p5 = GetBlock().p5;
            Database.GridCount = GetBlock().GridCount;
            Database.Count = GetBlock().Count;
            Database.l1 = GetBlock().l1;
            Database.l2 = GetBlock().l2;
            Database.l3 = GetBlock().l3;
            Database.l4 = GetBlock().l4;
            Database.l5 = GetBlock().l5;
            Database.KE = GetBlock().KE;
            Database.KSR = GetBlock().KSR;
            Database.SDM = GetBlock().SDM;
            Database.VGU = GetBlock().VGU;
            Database.NGU = GetBlock().NGU;
        }

    }
}
