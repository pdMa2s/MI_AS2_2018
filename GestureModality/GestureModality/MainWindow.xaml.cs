using Microsoft.Kinect;
using System;
using System.ComponentModel;
using System.Windows;

namespace GestureModality
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged, IDisposable
    {
        private KinectSensor kinectSensor;
        private Body[] bodies;
        private int activeBodyIndex;
        private BodyFrameReader bodyFrameReader;
        private GestureDetector gestureDetector;
        internal static MainWindow main;
        private ComModule coms;
        // INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            main = this;
            
            this.kinectSensor = KinectSensor.GetDefault();
            this.kinectSensor.Open();
            Console.WriteLine(kinectSensor.IsAvailable);

            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();
            this.bodyFrameReader.FrameArrived += this.Reader_BodyFrameArrived;
            coms = new ComModule();

            this.gestureDetector = new GestureDetector(kinectSensor,coms);
            this.activeBodyIndex = -1;
            /*if (!kinectSensor.IsAvailable)
            {
                Console.WriteLine("Kinect Sensor is not available!");
                Environment.Exit(1);
            }*/
        }

        private void Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            
            bool bodyInFrame = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (bodies == null)
                    {
                        // creates an array of 6 bodies, which is the max number of bodies that Kinect can track simultaneously
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }
                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    VerifyActiveBody();
                    bodyInFrame = this.activeBodyIndex != -1;
                }
            }

            if (bodyInFrame)
            {
                Console.WriteLine(this.activeBodyIndex);
                Body body = this.bodies[this.activeBodyIndex];

                // if the current body TrackingId changed, update the corresponding gesture detector with the new value
                if (body.TrackingId != this.gestureDetector.TrackingId)
                    this.gestureDetector.TrackingId = body.TrackingId;

                // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
                this.gestureDetector.IsPaused = (body.TrackingId == 0);
            }

        }

        private void VerifyActiveBody()
        {
            this.activeBodyIndex = -1;

            int maxBodies = this.kinectSensor.BodyFrameSource.BodyCount;

            float minZPoint = float.MaxValue; // Default to impossible value
            for (int i = 0; i < maxBodies; i++)
            {
                Body body = this.bodies[i];
                if (body.IsTracked)
                {
                    float zMeters = body.Joints[JointType.SpineBase].Position.Z;
                    if (zMeters < minZPoint)
                    {
                        minZPoint = zMeters;
                        this.activeBodyIndex = i;
                    }
                }
            }
        }

        internal string ChangeDetectedGesture
        {
            get { return this.gestureDetected.Text.ToString(); }
            set { Dispatcher.Invoke(new Action(() => { this.gestureDetected.Text = value; })); }
        }

        internal string ChangeConfidence
        {
            get { return this.confidence.Text.ToString(); }
            set { Dispatcher.Invoke(new Action(() => { this.confidence.Text = value; })); }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Disposes the GestureDetector object
        // True if Dispose was called directly, false if the GC handles the disposing
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.gestureDetector != null)
                {
                    this.gestureDetector.Dispose();
                    this.gestureDetector = null;
                }
            }
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (this.bodyFrameReader != null)
            {
                this.bodyFrameReader.FrameArrived -= this.Reader_BodyFrameArrived;
                this.bodyFrameReader.Dispose();
                this.bodyFrameReader = null;
            }

            if (this.gestureDetector != null)
            {
                this.gestureDetector.Dispose();
                this.gestureDetector = null;
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }

        }
    }
}
