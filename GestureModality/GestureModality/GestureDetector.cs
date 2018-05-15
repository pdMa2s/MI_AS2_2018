using Microsoft.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using mmisharp;
using System;
using System.Collections.Generic;

namespace GestureModality
{
    class GestureDetector : IDisposable
    {
        private readonly string gestureDatabasePath = "DiscordGestures.gbd";
        private VisualGestureBuilderFrameSource vgbFrameSource;
        private VisualGestureBuilderFrameReader vgbFrameReader;
        private const string muteGestureName = "mute_Right";
        private const string deafGestureName = "deaf_Left";
        private const string deleteMessageGestureName = "deleteBothArms";
        private LifeCycleEvents lce;
        private MmiCommunication mmic;
        private const int fpsDelay = 60;
        private int fpsCounter;
        private bool gestureWasDetected;
        private ComModule coms;
        private String userSelected;
        private String channelSelected;

        public GestureDetector(KinectSensor kinectSensor, ComModule coms)
        {
            this.coms = coms;
            if (kinectSensor == null)
            {
                throw new ArgumentNullException("kinectSensor");
            }

            this.vgbFrameSource = new VisualGestureBuilderFrameSource(kinectSensor, 0);
            this.vgbFrameReader = this.vgbFrameSource.OpenReader();

            if (this.vgbFrameReader != null)
            {
                this.vgbFrameReader.IsPaused = true;
                this.vgbFrameReader.FrameArrived += this.Reader_GestureFrameArrived;
            }
            VisualGestureBuilderDatabase database = new VisualGestureBuilderDatabase(gestureDatabasePath);

            if(database == null)
            {
                Console.WriteLine("No gesture database!");
                Environment.Exit(1);
            }

            lce = new LifeCycleEvents("GESTURES", "FUSION", "gesture-1", "acoustic", "command"); // LifeCycleEvents(string source, string target, string id, string medium, string mode)
            mmic = new MmiCommunication("localhost", 8000, "User2", "GESTURES"); // MmiCommunication(string IMhost, int portIM, string UserOD, string thisModalityName)
            mmic.Send(lce.NewContextRequest());

            this.vgbFrameSource.AddGestures(database.AvailableGestures);
            fpsCounter = 0;
            gestureWasDetected = false;
        }



        private void Reader_GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
        {
            if (coms.IsTtsSpeaking())
                return;

            if (this.gestureWasDetected)
            {
                this.fpsCounter++;
                if (fpsCounter == fpsDelay)
                {
                    this.fpsCounter = 0;
                    this.gestureWasDetected = false;
                }
                return;
            }

            VisualGestureBuilderFrameReference frameReference = e.FrameReference;
            using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    IReadOnlyDictionary<Gesture, DiscreteGestureResult> discreteResults = frame.DiscreteGestureResults;

                    if (discreteResults != null)
                    {
                        
                        Gesture toSend = null;
                        double toSendConfidence = -1;

                        foreach (Gesture gesture in this.vgbFrameSource.Gestures)
                        {
                            DiscreteGestureResult result = null;
                            discreteResults.TryGetValue(gesture, out result);

                            if (result != null && result.Confidence > .70)// && result.Detected)
                            {
                                toSend = gesture;
                                toSendConfidence = result.Confidence;
                            }
                        }

                        if(toSend != null)
                        {
                            SendDetectedGesture(toSend, toSendConfidence);
                            this.gestureWasDetected = true;
                            Console.WriteLine("Detected: "+ toSend.Name + " " + toSendConfidence);
                            coms.KeepServerAlive();
                        }

                    }
                }
            }
        }
        
        private void SendDetectedGesture(Gesture gesture, double confidence)
        {
            MainWindow.main.ChangeDetectedGesture = gesture.Name + "detected";
            MainWindow.main.ChangeConfidence = "Confidence: "+confidence.ToString();
            string json = "{ \"recognized\": { \"action\" : ";

            switch (gesture.Name)
            {
                case deafGestureName:
                    json += "\"SELF_DEAF\" ";
                    break;
                case muteGestureName:
                    json += "\"SELF_MUTE\" ";
                    break;
                case deleteMessageGestureName:
                    json += "\"DELETE_LAST_MESSAGE\" ";
                    break;
                        
            }

            if (channelSelected != null)
                json += ", \"channelName\" : \""+channelSelected+"\" ";
            if (userSelected != null)
                json += ", \"userName\" : \""+userSelected+"\" ";

            json += ", \"confidence\":\"implicit confirmation\" } }";

            var exNot = lce.ExtensionNotification("", "", (float) confidence, json);
            mmic.Send(exNot);
            channelSelected = null;
            userSelected = null;
        }

        public ulong TrackingId
        {
            get
            {
                return this.vgbFrameSource.TrackingId;
            }

            set
            {
                if (this.vgbFrameSource.TrackingId != value)
                    this.vgbFrameSource.TrackingId = value;
            }
        }

        public bool IsPaused
        {
            get
            {
                return this.vgbFrameReader.IsPaused;
            }

            set
            {
                if (this.vgbFrameReader.IsPaused != value)
                    this.vgbFrameReader.IsPaused = value;
            }
        }

        public void SetUserSelected(String userSelected)
        {
            this.userSelected = userSelected;
        }

        public void SetChannelSelected(String channelSelected)
        {
            this.channelSelected = channelSelected;
        }

        // Disposes the VisualGestureBuilderFrameSource and VisualGestureBuilderFrameReader objects
        public void Dispose()
        {
            if (this.vgbFrameReader != null)
            {
                this.vgbFrameReader.FrameArrived -= this.Reader_GestureFrameArrived;
                this.vgbFrameReader.Dispose();
                this.vgbFrameReader = null;
            }

            if (this.vgbFrameSource != null)
            {
                this.vgbFrameSource.Dispose();
                this.vgbFrameSource = null;
            }
        }
    }
}
