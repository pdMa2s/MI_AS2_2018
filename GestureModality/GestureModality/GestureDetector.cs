using Microsoft.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using System;
using System.Collections.Generic;

namespace GestureModality
{
    class GestureDetector : IDisposable
    {
        private readonly string gestureDatabase = @"DiscordGestures.gbd";
        private VisualGestureBuilderFrameSource vgbFrameSource;
        private VisualGestureBuilderFrameReader vgbFrameReader;
        private readonly string muteGestureName = "mute";
        private readonly string deafGestureName = "deaf";
        private readonly string deleteMessageGestureName = "deleteMessage";

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
            VisualGestureBuilderDatabase database = new VisualGestureBuilderDatabase(gestureDatabase);

            if(database == null)
            {
                Console.WriteLine("No gesture database!");
                Environment.Exit(1);
            }

            this.vgbFrameSource.AddGestures(database.AvailableGestures);

            // para fazer disable de gestos que não queremos
            /*foreach (Gesture gesture in this.vgbFrameSource.Gestures)
            {
                if (gesture.Name.Equals(this.deleteMessageGestureName))
                {
                    this.vgbFrameSource.SetIsEnabled(gesture, false);
                }
            }*/
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

                            if (result != null)
                            {
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
