using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using notify.Properties;
using System.IO;
using System.Resources;
using System.Drawing.Drawing2D;
using System.Media;
/*******************************************************************************
 *  Code contributed to the webinos project
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Copyright 2012-2013 André Paul
 *
 ******************************************************************************/
namespace notify
{
    public partial class Form1 : Form
    {
        Thread workerThread;

        public Form1()
        {
            InitializeComponent();

            newAlert();


            
            workerThread = new Thread(DoWork);
            workerThread.Start();

            this.TopMost = true;
            this.Focus();
            this.BringToFront();

        }

        private void newAlert()
        {

            

            SystemSounds.Beep.Play();
            string[] args = Environment.GetCommandLineArgs();

            if (args.Length == 1)
            {
                this.labelNotification.Text = "Usage: notify-send <\"Headline\"> <\"body\"> -i <warntype> where warntype is absolute file uri";
            }

            
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;

            this.Location = new Point(screenWidth - 350, 50);

            foreach (string arg in Environment.GetCommandLineArgs())
            {
                Console.Out.WriteLine("Log: " + arg);
            }

            if (args.Length >= 3) labelNotification.Text = args[2];
            if (args.Length >= 2) label1.Text = args[1];


            if (args.Length >= 5 && args[3] == "-i")
            {
                Image img = null;

                if (File.Exists(args[4]))
                {

                    img = new Bitmap(args[4]);

                    if (img != null)
                    {
                        img = resizeImage(img, new Size(51, 51));
                        this.pictureBox1.Image = img;
                    }
                }
            }
        }

 
        private void Form1_Load(object sender, EventArgs e)
        {

        }

 
        public void DoWork()
        {
           Thread.Sleep(7000);
           Console.Out.WriteLine("CLOSED");
           Application.Exit();
        }

        private static Image resizeImage(Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Console.Out.WriteLine("CLICKED");
            workerThread.Abort();
            Application.Exit();
        }

        private void labelNotification_Click(object sender, EventArgs e)
        {
            Console.Out.WriteLine("CLICKED");
            workerThread.Abort();
            Application.Exit();
        }


    }
}
