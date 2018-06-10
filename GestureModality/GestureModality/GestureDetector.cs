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
        private const string deleteGestureName = "deleteContinous";
        private const string kickGestureName = "kick";
        private const string banGestureName = "ban";

        private LifeCycleEvents lce;
        private MmiCommunication mmic;
        private const int fpsDelay = 60;
        private int fpsCounter;
        private bool gestureWasDetected;
        private ComModule coms;
        private String userSelected;
        private String channelSelected;
        private String guildSelected;

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
            mmic = new MmiCommunication("localhost", 9876, "User1", "GESTURES"); // MmiCommunication(string IMhost, int portIM, string UserOD, string thisModalityName)
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
                    MainWindow.main.ChangeDetectedGesture = "No gestures detected";
                    MainWindow.main.ChangeConfidence = "0";
                }
                return;
            }

            VisualGestureBuilderFrameReference frameReference = e.FrameReference;
            using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    IReadOnlyDictionary<Gesture, DiscreteGestureResult> discreteResults = frame.DiscreteGestureResults;
                    IReadOnlyDictionary<Gesture, ContinuousGestureResult> continousResults = frame.ContinuousGestureResults;

                    if (discreteResults != null)
                    {
                        
                        string toSend = null;
                        double toSendConfidence = -1;

                        foreach (Gesture gesture in this.vgbFrameSource.Gestures)
                        {
                            if (gesture.GestureType == GestureType.Discrete) {
                                DiscreteGestureResult result = null;
                                discreteResults.TryGetValue(gesture, out result);

                                if (result != null)
                                {
                                    if(result.Confidence > .10)
                                    {
                                        Console.WriteLine(gesture.Name);
                                        Console.WriteLine(result.Confidence);

                                    }
                                    Tuple<string, double> t = ProcessDiscreteGesture(result, gesture.Name);
                                    double confidence = t.Item2;
                                    if (confidence > toSendConfidence)
                                    {
                                        toSend = t.Item1;
                                        toSendConfidence = t.Item2;
                                    }
                                }
                            }

                            if (continousResults != null) {
                                if (gesture.GestureType == GestureType.Continuous && gesture.Name.Equals(deleteGestureName)) {
                                    ContinuousGestureResult result = null;
                                    continousResults.TryGetValue(gesture, out result);

                                    if (result != null) {
                                        var progress = result.Progress;
                                        if (progress > .80 && progress > toSendConfidence) {
                                            toSend = deleteGestureName;
                                            toSendConfidence = progress;
                                        }
                                    }
                                }
                            }
                            


                        }

                        if(toSend != null)
                        {
                            SendDetectedGesture(toSend, toSendConfidence);
                            this.gestureWasDetected = true;
                            Console.WriteLine("Detected: "+ toSend + " " + toSendConfidence);
                            coms.KeepServerAlive();
                        }

                    }
                }
            }
        }

        private Tuple<string, double> ProcessDiscreteGesture(DiscreteGestureResult detected, string gestureName) {
            
            if (((gestureName.Equals(muteGestureName) || gestureName.Equals(kickGestureName) || gestureName.Equals(banGestureName)) && detected.Confidence > .30) || detected.Confidence > .70)
                return Tuple.Create<string, double>(gestureName, detected.Confidence);
            return Tuple.Create<string, double>(null, -1);
        }
        
        private void SendDetectedGesture(string gesture, double confidence)
        {
            MainWindow.main.ChangeDetectedGesture = gesture + " detected";
            MainWindow.main.ChangeConfidence = confidence.ToString();
            string json = "{\"recognized\": [\"action\", ";

            switch (gesture)
            {
                case deafGestureName:
                    if (userSelected != null)
                        json += "\"DEAF_USER\"]";
                    else
                        json += "\"SELF_DEAF\"]";
                    break;
                case muteGestureName:
                    if (userSelected != null)
                        json += "\"MUTE_USER\"]";
                    else
                        json += "\"SELF_MUTE\"]";
                    break;
                case deleteGestureName:
                    json += "\"DELETE_LAST_MESSAGE\"]";
                    break;
                case kickGestureName:
                    json += "\"REMOVE_USER\"]";
                    break;
                case banGestureName:
                    json += "\"BAN_USER\"]";
                    break;
                default:
                    Console.WriteLine("Invalid action");
                    break;

            }

            if (channelSelected != null)
            {
                json = json.Substring(0, json.Length - 1);
                json += ", \"channelName\", \""+channelSelected+"\"]";
            }
            if (userSelected != null)
            {
                json = json.Substring(0, json.Length - 1);
                json += ", \"userName\", \"" + userSelected + "\"]";
            }
            if (guildSelected != null)
            {
                json = json.Substring(0, json.Length - 1);
                json += ", \"guildName\", \"" + guildSelected + "\"]";
            }

            json += ", \"confidence\":\"implicit confirmation\", \"modality\":\"gesture\"}";
            Console.WriteLine(json);
            var exNot = lce.ExtensionNotification("", "", (float) confidence, json);
            mmic.Send(exNot);
            if (channelSelected != null)
                MainWindow.main.ChangeColorBTNChannelSelected(channelSelected);
            if (userSelected != null)
                MainWindow.main.ChangeColorBTNUserSelected(userSelected);

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

        public string ChannelName
        {
            get
            {
                return this.channelSelected;
            }

            set
            {
                if (this.channelSelected != value)
                    this.channelSelected = value;
            }
        }

        public string UserName
        {
            get
            {
                return this.userSelected;
            }

            set
            {
                if (this.userSelected != value)
                    this.userSelected = value;
            }
        }

        public string GuildName
        {
            get
            {
                return this.guildSelected;
            }

            set
            {
                if (this.guildSelected != value)
                    this.guildSelected = value;
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
