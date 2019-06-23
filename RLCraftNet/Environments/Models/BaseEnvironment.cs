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

        public struct RECT
        {
            public int Left, Top, Right, Bottom;
        }

        public BaseEnvironment(string name)
        {
            hGameWindow = FindWindow(null, name);
            GetWindowRect(hGameWindow, out Window);
        }

        public abstract void Observe();
        public abstract void Act();
        public abstract void GetReward();
    }
}
