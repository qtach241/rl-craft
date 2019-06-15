using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameInput;
using System.Diagnostics;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(5000);

            //Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.H);
            //Thread.Sleep(200);
            //Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.E);
            //Thread.Sleep(200);
            //Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.L);
            //Thread.Sleep(200);
            //Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.L);
            //Thread.Sleep(200);
            //Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.O);
            //Thread.Sleep(200);
            //Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.Space);
            //Thread.Sleep(200);
            //Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.W);
            //Thread.Sleep(200);
            //Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.O);
            //Thread.Sleep(200);
            //Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.R);
            //Thread.Sleep(200);
            //Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.L);
            //Thread.Sleep(200);
            //Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.D);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.A, 500);
            Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.W, 500);
            Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.D, 500);
            Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.S, 500);
            Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.A, 500);
            Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.W, 500);
            Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.D, 500);

            //Keyboard.SendKeyDownAsInput(System.Windows.Forms.Keys.W);
            //Thread.Sleep(5000);
            //Keyboard.SendKeyUpAsInput(System.Windows.Forms.Keys.W);

            Console.WriteLine($"Blocked: {sw.ElapsedMilliseconds}");

            Console.ReadKey();
        }
    }
}
