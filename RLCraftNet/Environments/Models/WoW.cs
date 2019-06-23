using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Environments.Models
{
    public class WoW : IEnvironment
    {
        #region Imports

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        #endregion

        private const string WINDOW_NAME = "World of Warcraft";

        private readonly IntPtr hGameWindow;
        private RECT rect;

        public struct RECT
        {
            public int left, top, right, bottom;
        }

        public WoW()
        {
            hGameWindow = FindWindow(null, WINDOW_NAME);
            GetWindowRect(hGameWindow, out rect);
        }

        public void Observe()
        {

        }

        public void GetReward()
        {

        }

        public void Act()
        {

        }
    }
}
