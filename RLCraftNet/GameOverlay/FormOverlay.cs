using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using GameOverlay.Models;
using Environments.Game;

namespace GameOverlay
{
    public partial class FormOverlay : Form
    {
        #region Imports

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        #endregion

        Graphics g;
        Pen redPen = new Pen(Color.Red);
        Pen bluPen = new Pen(Color.Blue);
        Pen grnPen = new Pen(Color.Green);

        Overlay overlay;

        public FormOverlay()
        {
            InitializeComponent();
        }

        private void FormOverlay_Load(object sender, EventArgs e)
        {
            // Game API we will interacting with.
            WoW WoW = new WoW();

            // Setup the overlay to place over the game window.
            this.BackColor = Color.Wheat;
            this.TransparencyKey = Color.Wheat;
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;

            // Make the transparent overlay window click-through.
            // https://stackoverflow.com/questions/2798245/click-through-in-c-sharp-form
            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);

            // Match the overlay window with the game window.
            this.Size = new Size(WoW.Window.Right - WoW.Window.Left, WoW.Window.Bottom - WoW.Window.Top);
            this.Top = WoW.Window.Top;
            this.Left = WoW.Window.Left;

            overlay = new Overlay(WoW.Window.Top, WoW.Window.Left, (WoW.Window.Right - WoW.Window.Left), (WoW.Window.Bottom - WoW.Window.Top));
        }

        private void FormOverlay_Paint(object sender, PaintEventArgs e)
        {
            g = e.Graphics;
            g.DrawRectangle(redPen, 200, 200, 300, 300);

            e.Graphics.DrawRectangle(grnPen, overlay.SelfFrame.GetRect());
            e.Graphics.DrawRectangle(grnPen, overlay.TankFrame.GetRect());
            e.Graphics.DrawRectangle(grnPen, overlay.DpsFrames[0].GetRect());
            e.Graphics.DrawRectangle(grnPen, overlay.DpsFrames[1].GetRect());
            e.Graphics.DrawRectangle(grnPen, overlay.DpsFrames[2].GetRect());
        }

        private void ActionTimer_Tick(object sender, EventArgs e)
        {
            // Observe the game state

            // Act on the game state

            // Obtain the reward from the action
        }
    }
}
