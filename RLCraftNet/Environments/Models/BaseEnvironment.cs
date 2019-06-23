using System;
using System.Runtime.InteropServices;

namespace Environments.Models
{
    public abstract class BaseEnvironment : IEnvironment
    {
        #region Imports

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        #endregion

        private readonly IntPtr hGameWindow;
        public RECT Window;

        public BaseEnvironment(string name)
        {
            hGameWindow = FindWindow(null, name);
            GetWindowRect(hGameWindow, out Window);
        }

        public struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        public enum Resolution
        {
            _800_x_600,
            _1280_x_720,
            _1600_x_900,
            _1920_x_1080,
            _2560_x_1440,
            None,
        }

        public Resolution WindowResolution
        {
            get
            {
                switch (Window.Bottom - Window.Top)
                {
                    case 600:
                        return Resolution._800_x_600;
                    case 720:
                        return Resolution._1280_x_720;
                    case 900:
                        return Resolution._1600_x_900;
                    case 1080:
                        return Resolution._1920_x_1080;
                    case 1440:
                        return Resolution._2560_x_1440;
                    default:
                        return Resolution.None;
                }       
            }
        }

        public abstract void Observe();
        public abstract void Act();
        public abstract void GetReward();
    }
}
