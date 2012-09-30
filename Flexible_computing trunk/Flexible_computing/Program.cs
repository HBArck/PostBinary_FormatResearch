using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
//using System.Threading;

namespace Flexible_computing
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>

        //static Thread thr1;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            //thr1 = new Thread(() => Application.Run(new Form1()));
           // thr1.Start();
        }
    }
}
