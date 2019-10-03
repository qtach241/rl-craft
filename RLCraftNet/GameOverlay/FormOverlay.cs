using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using GameOverlay.Models;
using Environments.Game;
using System.ComponentModel;
using System.Diagnostics;
using GameInput;
using System.Threading;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace GameOverlay
{
    public partial class FormOverlay : Form
    {
        #region Imports

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        static Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);

        #endregion

        Graphics g;
        Pen redPen = new Pen(Color.Red);
        Pen bluPen = new Pen(Color.Blue);
        Pen grnPen = new Pen(Color.Green);

        Overlay overlay;

        BoundingBox drawBox = new BoundingBox();

        public FormOverlay()
        {
            InitializeComponent();
            InitializeBackgroundWorkers();
        }

        private void InitializeBackgroundWorkers()
        {
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker1_RunWorkerCompleted);
            backgroundWorker2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker2_RunWorkerCompleted);
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
            //this.Size = new Size(WoW.Window.Right - WoW.Window.Left, WoW.Window.Bottom - WoW.Window.Top);
            //this.Top = WoW.Window.Top;
            //this.Left = WoW.Window.Left;

            this.Size = new Size(2560, 1440);
            this.Top = 0;
            this.Left = 0;

            //overlay = new Overlay(WoW.Window.Top, WoW.Window.Left, (WoW.Window.Right - WoW.Window.Left), (WoW.Window.Bottom - WoW.Window.Top));

            backgroundWorker1.RunWorkerAsync(3000);
        }

        private void FormOverlay_Paint(object sender, PaintEventArgs e)
        {
            //g = e.Graphics;
            //g.DrawRectangle(redPen, 200, 200, 300, 300);

            //e.Graphics.DrawRectangle(grnPen, overlay.SelfFrame.GetRect());
            //e.Graphics.DrawRectangle(grnPen, overlay.TankFrame.GetRect());
            //e.Graphics.DrawRectangle(grnPen, overlay.DpsFrames[0].GetRect());
            //e.Graphics.DrawRectangle(grnPen, overlay.DpsFrames[1].GetRect());
            //e.Graphics.DrawRectangle(grnPen, overlay.DpsFrames[2].GetRect());

            e.Graphics.DrawRectangle(grnPen, (float)drawBox.left * 2560, (float)drawBox.top * 1440, (float)drawBox.width * 2560, (float)drawBox.height * 1440);
        }

        private void ActionTimer_Tick(object sender, EventArgs e)
        {
            // Observe the game state

            // Act on the game state

            // Obtain the reward from the action
        }

        /// <summary>
        /// Fires when backgroundWorker1 completes (python script has finished
        /// executing and returned the tag coordinates). This handler obtains 
        /// the bounding box from the returned tags and refreshes the screen so 
        /// that it may be drawn. Finally, it kicks off backgroundWorker2 which 
        /// is responsible for detecting when the lure drops.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                Debug.WriteLine("Worker1 completed: error");
            }
            else if (e.Cancelled)
            {
                Debug.WriteLine("Worker1 completed: cancelled");
            }
            else
            {
                Debug.WriteLine("Worker1 completed: " + e.Result?.ToString());
            }

            // Move the mouse out of the way.
            Keyboard.MouseMoveTo(0, 0);

            LureTag lureTag = e.Result as LureTag;

            if (lureTag == null)
            {
                Debug.WriteLine("lureTag is null");
            }
            else
            {
                drawBox = lureTag.boundingBox;

                // Refresh the screen (triggers a repaint).
                this.Refresh();
            }

            // Kick off backgroundWorker2.
            backgroundWorker2.RunWorkerAsync(1000);
        }

        /// <summary>
        /// Fires when backgroundWorker2 completes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                Debug.WriteLine("Worker2 completed: error");
            }
            else if (e.Cancelled)
            {
                Debug.WriteLine("Worker2 completed: cancelled");
            }
            else
            {
                Debug.WriteLine("Worker2 completed: " + e.Result?.ToString());
            }

            // Shift click in the center of the bounding box to bring the fish in.
            double x_normalized = (drawBox.left + drawBox.width / 2);
            double y_normalized = (drawBox.top + drawBox.height / 2);

            Keyboard.SendKeyDownAsInput(System.Windows.Forms.Keys.ShiftKey);
            Keyboard.MouseLeftClick((int)((x_normalized) * 65536.0), (int)((y_normalized) * 65536.0));
            Keyboard.SendKeyUpAsInput(System.Windows.Forms.Keys.ShiftKey);

            // Remove the bounding box drawn on screen.
            drawBox.top = 0;
            drawBox.left = 0;
            drawBox.height = 0;
            drawBox.width = 0;
            this.Refresh();

            // Kick off backgroundWorker 1 and repeat.
            backgroundWorker1.RunWorkerAsync(3000);
        }

        /// <summary>
        /// Finds the coordinates of the lure, which involves invoking a python script.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker worker = sender as BackgroundWorker;

            // Assign the result of the computation to the Result property 
            // of the DoWorkEventArgs object. This is will be available to 
            // the RunWorkerCompleted eventhandler.
            e.Result = FindLure((int)e.Argument, worker, e);
        }

        /// <summary>
        /// Monitors the bounding box where the lure was found to see when it "drops".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker2_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker worker = sender as BackgroundWorker;

            // TODO: Algo needs to be implemented.

            double x = (drawBox.left + drawBox.width / 2) * 2560;
            double y = (drawBox.top + drawBox.height / 2) * 1440;
            Point pt = new Point((int)x, (int)y);
            Color prev_c = new Color();

            for (int i = 0; i <= 200; i++)
            {
                //GetCursorPos(ref cursor);

                //Console.WriteLine($"Cursor: {cursor.X} , {cursor.Y}");

                Color c = GetColorAt(pt);

                //Debug.WriteLine($"Color: {c.R}, {c.G}, {c.B}, ({i})");

                if (i > 0)
                {
                    int delta = Math.Abs(c.G - prev_c.G);
                    Debug.WriteLine("Iteration: " + i + ", Delta: " + delta);
                    if (delta >= 20)
                    {
                        e.Result = "detected";
                        Thread.Sleep(1000);
                        return;
                    }
                }

                prev_c = c;
                Thread.Sleep(100);
            }

            // When the algo triggers, this background worker simply returns which
            // lets the completed handler take care of the rest.
            e.Result = "expired";
        }

        /// <summary>
        /// Helper function to find the location of the lure.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="worker"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private LureTag FindLure(int n, BackgroundWorker worker, DoWorkEventArgs e)
        {
            // Allow delay for us to switch to game window.
            Thread.Sleep(n);

            // Press "C" to cast line. Hold a few seconds for picture to settle.
            Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.C, 2000);

            // Take picture of the screen
            string image_filename = DateTime.Now.ToString("yyyy_MM_ddTHH_mm_ss_fffffffZ");

            Rectangle bounds = Screen.GetBounds(Point.Empty);
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                }
                bitmap.Save(image_filename + ".jpg", ImageFormat.Jpeg);
            }

            string result;

            // Send picture to python script
            // NOTE: This requires python along with a bunch of packages to be installed. Make sure to run:
            // >>pip install tensorflow
            // >>pip install pillow
            // >>pip install numpy
            // >>pip install opencv-python
            result = exec_python("predict.py", image_filename + ".jpg");

            // Attempt to deserialize the output as Json.
            List<LureTag> lureTags = new List<LureTag>();
            lureTags = JsonConvert.DeserializeObject<List<LureTag>>(result);

            // Return the first tag (the most confident).
            return lureTags.FirstOrDefault();
        }

        /// <summary>
        /// Executes python script as a process and returns stdout as string to caller.
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private string exec_python(string cmd, string args)
        {
            ProcessStartInfo start = new ProcessStartInfo();

            // run 'where python' in the command line if python is added to PATH.
            start.FileName = "C:/Users/Frank/AppData/Local/Programs/Python/Python37/python.exe";

            start.Arguments = string.Format("{0} {1}", cmd, args);
            start.UseShellExecute = false;
            start.CreateNoWindow = true;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Debug.Write(result);

                    return result;
                }
            }
        }

        public class BoundingBox
        {
            [JsonProperty]
            public double left { get; set; }

            [JsonProperty]
            public double top { get; set; }

            [JsonProperty]
            public double width { get; set; }

            [JsonProperty]
            public double height { get; set; }
        }

        public class LureTag
        {
            [JsonProperty]
            public double probability { get; set; }

            [JsonProperty]
            public int tagId { get; set; }

            [JsonProperty]
            public string tagName { get; set; }

            [JsonProperty]
            public BoundingBox boundingBox { get; set; }
        }

        static public Color GetColorAt(Point location)
        {
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }

            return screenPixel.GetPixel(0, 0);
        }
    }
}
