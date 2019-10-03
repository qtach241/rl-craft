﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameInput;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

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

            //Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.A, 500);
            //Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.W, 500);
            //Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.D, 500);
            //Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.S, 500);
            //Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.A, 500);
            //Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.W, 500);
            //Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.D, 500);

            //Keyboard.SendKeyDownAsInput(System.Windows.Forms.Keys.W);
            //Thread.Sleep(5000);
            //Keyboard.SendKeyUpAsInput(System.Windows.Forms.Keys.W);

            //string image_filename = DateTime.Now.ToString("yyyy_MM_ddTHH_mm_ss_fffffffZ");

            //Rectangle bounds = Screen.GetBounds(Point.Empty);
            //using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            //{
            //    using (Graphics g = Graphics.FromImage(bitmap))
            //    {
            //        g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
            //    }
            //    bitmap.Save(image_filename + ".jpg", ImageFormat.Jpeg);
            //}

            //Keyboard.MouseMoveTo((int)((1280.0 / 2560.0) * 65536.0), (int)((720.0 / 1440.0) * 65536.0));
            Keyboard.MouseLeftClick((int)((1280.0 / 2560.0) * 65536.0), (int)((720.0 / 1440.0) * 65536.0));

            Console.WriteLine($"Blocked: {sw.ElapsedMilliseconds}");

            Console.ReadKey();
        }
    }
}
