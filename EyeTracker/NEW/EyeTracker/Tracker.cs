using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading.Tasks;
using System.Threading;


//DEBUG
using System.Diagnostics;



using Emgu.CV;                  //
using Emgu.CV.CvEnum;           // usual Emgu CV imports
using Emgu.CV.Structure;        //
using Emgu.CV.UI;               //

namespace EyeTracker
{
    public partial class Tracker : Form
    {

        public Tracker()
        {
            InitializeComponent();
        }

        private void Tracker_Load(object sender, EventArgs e)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.WorkerReportsProgress = true;
        }



        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Capture camera;
            CascadeClassifier haar_cascade_face = new CascadeClassifier("haarcascade_frontalface_default.xml"); // Trained face classifier from OpenCV
            CascadeClassifier haar_cascade_eyes = new CascadeClassifier("haarcascade_eye.xml"); // Trained eye classifier from OpenCV
            
            Mat imgOriginal = new Mat();
            Mat gray_equalized = new Mat();
            Mat eyes = new Mat();


            try
            {
                //Console.WriteLine("Starting capture from thread: " + Thread.CurrentThread.ManagedThreadId);
                camera = new Capture(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("unable to read from webcam, error: " + Environment.NewLine + Environment.NewLine + ex.Message + Environment.NewLine + Environment.NewLine + "exiting program");
                this.Close();
                return;
            }
            

            for (; ; ) // Forever loop - worker's job
            {
                //Console.WriteLine("Proccessing image from thread: " + Thread.CurrentThread.ManagedThreadId);

                imgOriginal = camera.QueryFrame();

                if (imgOriginal == null) // If the current frame is damaged - stop
                {
                    MessageBox.Show("unable to read frame from webcam" + Environment.NewLine + Environment.NewLine + "exiting program");
                    this.Close();
                    return;
                }


                //("Camera resolution: " + imgOriginal.Width + "x" + imgOriginal.Height);
                
                //Image<Gray, Byte> currentFrame = imgOriginal.ToImage<Gray, Byte>();
                //var detected_faces = haar_cascade_face.DetectMultiScale(currentFrame, 1.1, 10, Size.Empty);
                //Image<Bgr, Byte> original = imgOriginal.ToImage<Bgr, Byte>();

                CvInvoke.CvtColor(imgOriginal, gray_equalized, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray); // Convert camera's frame to gray scale
                CvInvoke.EqualizeHist(gray_equalized, gray_equalized); // Normalize the brightness and increase contrast of the gray frame
                
                Rectangle[] faces_detect = haar_cascade_face.DetectMultiScale(gray_equalized, 1.1, 10, new Size(30, 30)); // Find faces inside the gray frame

                foreach (Rectangle face in faces_detect)
                {
                    CvInvoke.Rectangle(imgOriginal, face, new MCvScalar(255, 0 , 0), 3); // Draw a rectangle around the face
                    
                    eyes = new Mat(gray_equalized, face);
                    Rectangle[] eyes_detect = haar_cascade_eyes.DetectMultiScale(eyes, 1.1, 20, new Size(30,30));
                    Console.WriteLine(eyes_detect.Length);
                    foreach(Rectangle eye in eyes_detect)
                    {
                        Point eye_center = new Point(face.X + eye.X + (eye.Width / 2), face.Y + eye.Y + (eye.Height / 2)); // Don't ask, it just works..
                        var radious = Math.Ceiling((eye.Height + eye.Width)*0.25);
                        CvInvoke.Circle(imgOriginal, eye_center, (int)radious, new MCvScalar(100, 0, 0));
                    }
                    //original.Draw(face, new Bgr(Color.BurlyWood), 3);
                }
                imageBox1.Image = imgOriginal;

                this.backgroundWorker1.ReportProgress(1, imgOriginal);

                if (this.backgroundWorker1.CancellationPending)
                {
                    camera.Stop();
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //imageBox1.Image = (Image<Bgr, Byte>)e.UserState;
           // Console.WriteLine("Displaying image from thread: " + Thread.CurrentThread.ManagedThreadId);
            CvInvoke.Imshow("Window", (Mat)e.UserState);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Somehow detection stops
        }

        private void startRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.backgroundWorker1.RunWorkerAsync();
        }

        private void stopRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.backgroundWorker1.CancelAsync();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.backgroundWorker1.CancelAsync();
            Thread.Sleep(200);
            this.Close();
        }

    }
}
