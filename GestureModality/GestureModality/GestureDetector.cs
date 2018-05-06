using Microsoft.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using mmisharp;
using System;
using System.Collections.Generic;

namespace GestureModality
{
    class GestureDetector : IDisposable
    {
        private readonly string gestureDatabasePath = Environment.CurrentDirectory + "\\DiscordGestures.gbd";
        private VisualGestureBuilderFrameSource vgbFrameSource;
        private VisualGestureBuilderFrameReader vgbFrameReader;
        private readonly string muteGestureName = "mute";
        private readonly string deafGestureName = "deaf";
        private readonly string deleteMessageGestureName = "deleteMessage";
        private MmiCommunication mmic;

        public GestureDetector(KinectSensor kinectSensor)
        {
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

            mmic = new MmiCommunication("localhost", 8000, "User1", "GES"); // MmiCommunication(string IMhost, int portIM, string UserOD, string thisModalityName)


        }

        private void Reader_GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
        {
            VisualGestureBuilderFrameReference frameReference = e.FrameReference;
            using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    IReadOnlyDictionary<Gesture, DiscreteGestureResult> discreteResults = frame.DiscreteGestureResults;

                    if (discreteResults != null)
                    {
                        bool muteGesture = false;
                        bool deafGesture = false;
                        bool deleteMessageGesture = false;
                        double muteGestureConfidence = 0;
                        double deafGestureConfidence = 0;
                        double deleteMessageGestureConfidence = 0;

                        foreach (Gesture gesture in this.vgbFrameSource.Gestures)
                        {
                            DiscreteGestureResult result = null;
                            discreteResults.TryGetValue(gesture, out result);

                            if (result != null && result.Detected)
                            {
                                SendDetectedGesture(gesture);
                                if (gesture.Name.Equals(this.muteGestureName))
                                {
                                    muteGesture = result.Detected;
                                    muteGestureConfidence = result.Confidence;
                                }
                                else if (gesture.Name.Equals(this.deafGestureName))
                                {
                                    deafGesture = result.Detected;
                                    deafGestureConfidence = result.Confidence;
                                }
                                else if (gesture.Name.Equals(this.deleteMessageGestureName))
                                {
                                    deleteMessageGesture = result.Detected;
                                    deleteMessageGestureConfidence = result.Confidence;
                                }

                            }
                        }

                        Console.WriteLine("Mute: "+muteGesture + " "+ muteGestureConfidence);
                        Console.WriteLine("Deaf: " + deafGesture + " " + deafGestureConfidence);
                        Console.WriteLine("Delete Message: " + deleteMessageGesture + " " + deleteMessageGestureConfidence);
                        
                    }
                }
            }
        }

        private void SendDetectedGesture(Gesture gesture)
        {
            string json = "{ \"recognized\": {";

            switch (gesture.Name)
            {
                case "deaf":
                    json += "SELF_DEAF";
                    break;
                case "mute":
                    json += "SELF_MUTE";
                    break;
                case "deleteMessage":
                    json += "DELETE_LAST_MESSAGE";
                    break;
                        
            }
            json += ", \"confidence\":\"implicit confirmation\" } }";

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
