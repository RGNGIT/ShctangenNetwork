using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

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
            Console.WriteLine("Введите адрес сервера-моста");
            URL = Console.ReadLine();
            if (new Network(null, null).Ping(URL))
            {
                Console.WriteLine("Ожидание реквеста с клиента...");
                while (true) // Главный прослушивающий цикл
                {
                    Listen();
                    Thread.Sleep(1000);
                }
            }
        }

        static string URL;

        static System.Net.NetworkCredential credential = new System.Net.NetworkCredential()
        {
            UserName = "testuser",
            Password = "12345678"
        };

        static void Listen()
        {
            try
            {
                File.WriteAllBytes("GetInput.shc", new Network(credential, URL).GetInput(new Uri($"ftp://{URL}/files/ShctangenNetwork/Input.shc")));
                Thread.Sleep(1000);
                SetDB();
                StartGauge();
                Console.WriteLine("Вроде прогнал циклы. Пусть клиент зачекает\nОжидание реквеста с клиента...");
                CleanInput();
                CleanServer();
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
            new Network(credential, URL).SendOutput(File.ReadAllBytes("SetOutput.shc"));
        }

        static void CleanInput()
        {
            if (File.Exists("GetInput.shc"))
            {
                File.Delete("GetInput.shc");
            }
        }

        static void CleanServer()
        {
            new Network(credential, URL).Delete(new Uri($"ftp://{URL}/files/ShctangenNetwork/Input.shc"));
        }

        static BinaryFormatter binaryFormatter = new BinaryFormatter();

        static CodenameShctangencircle.DataBlock GetBlock()
        {
           CodenameShctangencircle.DataBlock DeserializeBlock;
            using (FileStream fileStream = new FileStream("GetInput.shc", FileMode.OpenOrCreate))
            {
                DeserializeBlock = binaryFormatter.Deserialize(fileStream) as CodenameShctangencircle.DataBlock;
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
