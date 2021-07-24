using System;
using ShctangenNetLib;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using WindowsInput;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;

namespace ShctangenNetwork
{
    class Calc
    {

        double NoRepeatAmount, LongestStep, LowerBorder, UpperBorder, KE = 0;

        void GetFileVars(int iterator)
        {
            string[] arr = File.ReadAllLines($"Составленные{iterator}_0.txt");
            StringBuilder sb;
            string Temp = string.Empty;
            foreach (char i in arr[1])
            {
                if (Char.IsDigit(i))
                {
                    Temp += i;
                }
            }
            NoRepeatAmount = Convert.ToDouble(Temp);
            Temp = string.Empty;
            foreach (char i in arr[3])
            {
                if (Char.IsDigit(i))
                {
                    Temp += i;
                }
            }
            LongestStep = Convert.ToDouble(Temp);
            Temp = string.Empty;
            foreach (char i in arr[5])
            {
                if (Char.IsDigit(i) || i == ',')
                {
                    Temp += i;
                }
            }
            sb = new StringBuilder(Temp);
            sb.Remove(0, 1);
            LowerBorder = Convert.ToDouble(sb.ToString());
            Temp = string.Empty;
            foreach (char i in arr[7])
            {
                if (Char.IsDigit(i) || i == ',')
                {
                    Temp += i;
                }
            }
            sb = new StringBuilder(Temp);
            sb.Remove(0, 1);
            UpperBorder = Convert.ToDouble(sb.ToString());
            Temp = string.Empty;
            foreach (char i in arr[10])
            {
                if (Char.IsDigit(i) || i == ',')
                {
                    Temp += i;
                }
            }
            KE = Convert.ToDouble(Temp);
        }

        void GetArr(int iterator, out string[] arr)
        {
            try
            {
                if (File.Exists($"Составленные{iterator}_0.txt"))
                {
                    arr = File.ReadAllLines($"Составленные{iterator}_0.txt");
                }
                else
                {
                    while (!File.Exists($"Составленные{iterator}_0.txt"))
                    {
                        Thread.Sleep(1000);
                    }
                    arr = File.ReadAllLines($"Составленные{iterator}_0.txt");
                }
            }
            catch (Exception)
            {
                GetArr(iterator, out arr);
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string IpClassName, string IpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool BringWindowToTop(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetActiveWindow(IntPtr hwnd);
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);


        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern bool ShowWindow(IntPtr handle, int nCmdShow);
        InputSimulator simulator = new InputSimulator();
        public string Entry;
        int iterator = 1;

        public void ProgramCycles()
        {
            Thread.Sleep(3000);
            GridBlock gridBlock = new GridBlock();
            IntPtr w = FindWindow(null, "Поиск лучшего набора и расчет характеристик");
            // IntPtr frm1 = FindWindow(null, "Form1");
            // f = frm1;
            // if (w.ToInt32() == 0) MessageBox.Show("Окно не найдено :c");
            ShowWindow(w, 9);
            SetForegroundWindow(w);
            simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RIGHT);
            for (int i = 0; i < 4; i++)
            {
                simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.TAB);
            }
            for (int i = 0; i < Database.l1.Count; i++)
            {

                simulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_A);
                Thread.Sleep(200);
                simulator.Keyboard.TextEntry(Entry);
                Thread.Sleep(200);
                simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.TAB);
                simulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_A);
                Thread.Sleep(200);
                simulator.Keyboard.TextEntry(Database.l1[i] + Database.l2[i] + Database.l3[i] + Database.l4[i] + Database.l5[i]);
                Thread.Sleep(200);
                simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.TAB);
                simulator.Keyboard.ModifiedKeyStroke(WindowsInput.Native.VirtualKeyCode.CONTROL, WindowsInput.Native.VirtualKeyCode.VK_A);
                Thread.Sleep(200);
                SendKeys.SendWait(Database.o[i].ToString());
                for (int m = 0; m < 3; m++)
                {
                    simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.TAB);
                }
                Thread.Sleep(200);
                simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
                for (int m = 0; m < 7; m++)
                {
                    simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.TAB);
                }
                Thread.Sleep(200);
                simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
                simulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.TAB);
                Thread.Sleep(1000);

                string[] arr;

                GetArr(iterator, out arr);

                Thread.Sleep(200);
                GetFileVars(iterator);
                Thread.Sleep(200);


                if (iterator % 10 == 0 && iterator != 0)
                {
                    iterator = 0;
                    for (int j = 1; j < 11; j++) File.Delete($"Составленные{j}_0.txt");
                }
                iterator++;
                string Ns = ("n1 = " + Database.p1[i] + " n2 = " + Database.p2[i] + " n3 = " + Database.p3[i] + " n4 = " + Database.p4[i] + " n5 = " + Database.p5[i]).ToString();
                gridBlock.Summ.Add(Database.l1[i] + Database.l2[i] + Database.l3[i] + Database.l4[i] + Database.l5[i]);
                gridBlock.KE.Add(KE.ToString());
                gridBlock.LowerBorder.Add(LowerBorder.ToString());
                gridBlock.UpperBorder.Add(UpperBorder.ToString());
                gridBlock.o.Add(Database.o[i].ToString());
                gridBlock.LongestStep.Add(LongestStep.ToString());
                gridBlock.NoRepeatAmount.Add(NoRepeatAmount.ToString());
                gridBlock.Ns.Add(Ns);
                Console.WriteLine($"Итерация {i}: \n" +
                    $"Сумма = '{Database.l1[i] + Database.l2[i] + Database.l3[i] + Database.l4[i] + Database.l5[i]}', \n" +
                    $"KE = '{KE}', \n" +
                    $"LowerBorder = '{LowerBorder}', \n" +
                    $"UpperBorder = '{UpperBorder}', \n" +
                    $"o = '{Database.o[i]}', \n" +
                    $"LongestStep = '{LongestStep}', \n" +
                    $"NoRepeatAmount = '{NoRepeatAmount}', \n" +
                    $"Ns = '{Ns}'\n");
            }
            SendKeys.SendWait("%{F4}");
            Serializer(gridBlock);
        }

        BinaryFormatter binaryFormatter = new BinaryFormatter();

        void Serializer(GridBlock data)
        {
            using (FileStream fileStream = new FileStream("SetOutput.shc", FileMode.OpenOrCreate))
            {
                binaryFormatter.Serialize(fileStream, data);
            }
        }

    }
}
