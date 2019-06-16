using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace GameOverlay
{
    public partial class FormOverlay : Form
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

        public const string WINDOW_NAME = "World of Warcraft";
        IntPtr handle = FindWindow(null, WINDOW_NAME);
        RECT rect;

        Graphics g;
        Pen redPen = new Pen(Color.Red);
        Pen bluPen = new Pen(Color.Blue);
        Pen grnPen = new Pen(Color.Green);

        private struct RECT
        {
            public int left, top, right, bottom;
        }

        public FormOverlay()
        {
            InitializeComponent();
        }

        private void FormOverlay_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.Wheat;
            this.TransparencyKey = Color.Wheat;
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;

            // Make the transparent overlay window click-through.
            // https://stackoverflow.com/questions/2798245/click-through-in-c-sharp-form
            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);

            GetWindowRect(handle, out rect);
            this.Size = new Size(rect.right - rect.left, rect.bottom - rect.top);
            this.Top = rect.top;
            this.Left = rect.left;
        }

        private void FormOverlay_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            g.DrawRectangle(redPen, 200, 200, 300, 300);

            e.Graphics.DrawRectangle(grnPen, 400, 400, 200, 200);
        }
    }
}
