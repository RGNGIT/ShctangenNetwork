using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShctangenNetwork
{
    public static class Database
    {
        // База результатов цикла
        public static List<int> o = new List<int>();
        public static List<int> o1 = new List<int>();
        public static List<int> o2 = new List<int>();
        public static List<int> o3 = new List<int>();
        public static List<int> o4 = new List<int>();
        public static List<int> o5 = new List<int>();
        public static List<double> p1 = new List<double>();
        public static List<double> p2 = new List<double>();
        public static List<double> p3 = new List<double>();
        public static List<double> p4 = new List<double>();
        public static List<double> p5 = new List<double>();
        public static List<int> GridCount = new List<int>();
        // База результатов подбора
        public static List<string> Count = new List<string>();
        public static List<string> l1 = new List<string>();
        public static List<string> l2 = new List<string>();
        public static List<string> l3 = new List<string>();
        public static List<string> l4 = new List<string>();
        public static List<string> l5 = new List<string>();
        public static List<string> KE = new List<string>();
        public static List<string> KSR = new List<string>();
        public static List<string> SDM = new List<string>();
        public static List<string> VGU = new List<string>();
        public static List<string> NGU = new List<string>();

        public static void ClearStuff()
        {
            o.Clear();
            o1.Clear();
            o2.Clear();
            o3.Clear();
            o4.Clear();
            o5.Clear();
            p1.Clear();
            p2.Clear();
            p3.Clear();
            p4.Clear();
            p5.Clear();
            GridCount.Clear();
            Count.Clear();
            l1.Clear();
            l2.Clear();
            l3.Clear();
            l4.Clear();
            l5.Clear();
            KE.Clear();
            KSR.Clear();
            SDM.Clear();
            VGU.Clear();
            NGU.Clear();
        }

    }
}
