﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Keyboard;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(3000);

            Keyboard.Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.H);
            Thread.Sleep(1000);
            Keyboard.Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.E);
            Thread.Sleep(1000);
            Keyboard.Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.L);
            Thread.Sleep(1000);
            Keyboard.Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.L);
            Thread.Sleep(1000);
            Keyboard.Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.O);
            Thread.Sleep(1000);
            Keyboard.Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.Space);
            Thread.Sleep(1000);
            Keyboard.Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.W);
            Thread.Sleep(1000);
            Keyboard.Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.O);
            Thread.Sleep(1000);
            Keyboard.Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.R);
            Thread.Sleep(1000);
            Keyboard.Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.L);
            Thread.Sleep(1000);
            Keyboard.Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.D);

            Console.ReadKey();
        }
    }
}