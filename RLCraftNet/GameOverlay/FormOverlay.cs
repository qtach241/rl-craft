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

        //Overlay overlay;

        BoundingBox drawBox = new BoundingBox();

        Stopwatch sw = new Stopwatch();

        private const int SCREEN_WIDTH_PX = 2560;
        private const int SCREEN_HEIGHT_PX = 1440;

        private int castsUntilBreak;
        Random rng = new Random();

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
            //WoW WoW = new WoW();

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

            this.Size = new Size(SCREEN_WIDTH_PX, SCREEN_HEIGHT_PX);
            this.Top = 0;
            this.Left = 0;

            //overlay = new Overlay(WoW.Window.Top, WoW.Window.Left, (WoW.Window.Right - WoW.Window.Left), (WoW.Window.Bottom - WoW.Window.Top));

            castsUntilBreak = rng.Next(100, 500);

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

            e.Graphics.DrawRectangle(grnPen, (float)drawBox.left * SCREEN_WIDTH_PX, (float)drawBox.top * SCREEN_HEIGHT_PX, (float)drawBox.width * SCREEN_WIDTH_PX, (float)drawBox.height * SCREEN_HEIGHT_PX);
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
            //Keyboard.MouseMoveTo(0, 0);

            // Grab the result of the background worker.
            LureTag lureTag = e.Result as LureTag;

            if (lureTag == null)
            {
                // If we didn't find the lure, nothing else to do.
                Debug.WriteLine("lureTag is null");
            }
            else
            {
                // Otherwise, draw a green rectangle over the lure bounding box.
                drawBox = lureTag.boundingBox;

                // Refresh the screen (triggers a repaint).
                this.Refresh();
            }

            // Kick off backgroundWorker2.
            backgroundWorker2.RunWorkerAsync();
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

            //Keyboard.SendKeyDownAsInput(System.Windows.Forms.Keys.ShiftKey);
            Keyboard.MouseLeftClick((int)((x_normalized) * 65536.0), (int)((y_normalized) * 65536.0));
            //Keyboard.SendKeyUpAsInput(System.Windows.Forms.Keys.ShiftKey);

            // Remove the bounding box drawn on screen.
            drawBox.top = 0;
            drawBox.left = 0;
            drawBox.height = 0;
            drawBox.width = 0;
            this.Refresh();

            castsUntilBreak--;
            Debug.WriteLine($"Casts remaining until break: {castsUntilBreak}");

            if(castsUntilBreak == 0)
            {
                int breakTime = rng.Next(1, 15); // 1 to 15 minutes.
                Debug.WriteLine($"Taking a break for {breakTime} minutes...");

                // Toggle the UI, take a screenshot, save it, then toggle back.
                Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.F12);

                Thread.Sleep(3000);

                // Click to "refresh" whispers tab (makes them show up)
                double general_tab_x_normalized = (100.0 / SCREEN_WIDTH_PX);
                double general_tab_y_normalized = (960.0 / SCREEN_HEIGHT_PX);
                double whisper_tab_x_normalized = (400.0 / SCREEN_WIDTH_PX);
                double whisper_tab_y_normalized = (960.0 / SCREEN_HEIGHT_PX);

                Keyboard.MouseLeftClick((int)((general_tab_x_normalized) * 65536.0), (int)((general_tab_y_normalized) * 65536.0));
                Thread.Sleep(1000);
                Keyboard.MouseLeftClick((int)((whisper_tab_x_normalized) * 65536.0), (int)((whisper_tab_y_normalized) * 65536.0));
                Thread.Sleep(1000);

                // Take picture of the screen
                string image_filename = DateTime.Now.ToString("yyyy_MM_ddTHH_mm_ss_fffffffZ");

                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                    }
                    bitmap.Save("break_" + image_filename + ".jpg", ImageFormat.Jpeg);
                }

                Thread.Sleep(3000);

                // Toggle UI back off.
                Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.F12);

                for (int i = 0; i < breakTime; i++)
                {
                    Thread.Sleep(60000);
                    Debug.WriteLine($"Minutes passed while on break: {i+1}");
                }
                //Thread.Sleep(breakTime * 60 * 1000);

                castsUntilBreak = rng.Next(10, 200);
                Debug.WriteLine($"Number of casts until next break: {castsUntilBreak}");

                // Jump to get rid of AFK status
                Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.Space);

                // Kick off backgroundWorker 1 and repeat.
                backgroundWorker1.RunWorkerAsync(3000);
            }
            else
            {
                // Kick off backgroundWorker 1 and repeat.
                backgroundWorker1.RunWorkerAsync(rng.Next(3000, 10000));
            }
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

            double x_left = (drawBox.left + 0.25 * drawBox.width) * SCREEN_WIDTH_PX;
            double x_center = (drawBox.left + 0.5 * drawBox.width) * SCREEN_WIDTH_PX;
            double x_right = (drawBox.left + 0.75 * drawBox.width) * SCREEN_WIDTH_PX;

            double y_top = (drawBox.top + 0.25 * drawBox.height) * SCREEN_HEIGHT_PX;
            double y_center = (drawBox.top + 0.5 * drawBox.height) * SCREEN_HEIGHT_PX;
            double y_bot = (drawBox.top + 0.75 * drawBox.height) * SCREEN_HEIGHT_PX;

            Point[] pts = new Point[3]
            {
                new Point((int) x_left, (int) y_top),
                //new Point((int) x_center, (int) y_top),
                //new Point((int) x_right, (int) y_top),
                //new Point((int) x_left, (int) y_center),
                new Point((int) x_center, (int) y_center),
                //new Point((int) x_right, (int) y_center),
                //new Point((int) x_left, (int) y_bot),
                //new Point((int) x_center, (int) y_bot),
                new Point((int) x_right, (int) y_bot),
            };

            Color[] prev_c = new Color[3];
            Color curr_c = new Color();
            int delta, i = 0;
            long ms = 0;

            sw.Restart();

            // 200 iterations, 100 ms per iteration, for a total of 20 seconds
            // monitoring the screen (21 seconds fishing channel time).
            while (true)
            {
                ms = sw.ElapsedMilliseconds;
                if (ms > 20000)
                    break;

                Debug.Write($"[{ms}] Deltas: ");

                for (int j = 0; j < 3; j++)
                {
                    curr_c = GetColorAt(pts[j]);

                    if (i > 0)
                    {
                        delta = Math.Abs(curr_c.G - prev_c[j].G);
                        Debug.Write($"[{j}]: {delta} ");

                        if (delta >= 50)
                        {
                            // When the algo triggers, this background worker simply returns which
                            // lets the completed handler take care of the rest.
                            e.Result = "detected";
                            Debug.Write("\n");
                            Thread.Sleep(1000);
                            return;
                        }
                    }

                    prev_c[j] = curr_c;
                }

                i++;
                Debug.Write("\n");
                //Thread.Sleep(100);
            }

            // Return if no detection after ~20 seconds.
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
            Debug.WriteLine($"Casting a new line in: {n} ms");
            Thread.Sleep(n);

            // Press "C" to cast line.
            Keyboard.SendKeyAsInput(System.Windows.Forms.Keys.C);

            // Wait a few seconds for lure to settle.
            Thread.Sleep(3000);

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
            // >>pip install tensorflow==1.15
            // >>pip install pillow
            // >>pip install numpy
            // >>pip install opencv-python

            // NOTES FROM 2ND BRING-UP:
            // - Tensor flow often times does not support the latest python, may have to install a previous version.
            // - Add python to path when installing to make everything easier.
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
