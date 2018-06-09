using Microsoft.Kinect;
using Microsoft.Kinect.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
        private List<Guild> guildList;
        // INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            this.InitializeComponent();
            main = this;

            KinectRegion.SetKinectRegion(this, kinectRegion);

            App app = ((App)Application.Current);
            app.KinectRegion = kinectRegion;

            this.kinectSensor = KinectSensor.GetDefault();

            // Use the default sensor
            this.kinectRegion.KinectSensor = this.kinectSensor;

            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();
            this.bodyFrameReader.FrameArrived += this.Reader_BodyFrameArrived;
            guildList = new List<Guild>();
            coms = new ComModule(this);

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
            get { return this.confidencePercentage.Text.ToString(); }
            set { Dispatcher.Invoke(new Action(() => {
                double confidenceValue = Convert.ToDouble(value);
                this.confidencePercentage.Text = String.Format("{0:P2}", confidenceValue);
                if (value.Equals("0"))
                {
                    this.confidencePercentage.Foreground = Brushes.Black;
                }
                else
                {
                    this.confidencePercentage.Foreground = Brushes.Green;
                }
                }));
            }
        }

        private Button CreateButton(string name, double marginWidth, double marginHeight)
        {
            Button newButton = new Button();
            newButton.Content = name;
            string nameButton = name.Replace(" ", "_");
            newButton.Name = nameButton + "BTN";
            newButton.Width = 14 * name.Length;
            newButton.Height = 50;
            newButton.HorizontalAlignment = HorizontalAlignment.Left;
            newButton.VerticalAlignment = VerticalAlignment.Top;
            newButton.Style = FindResource("buttonStyle") as Style;
            Thickness margin = newButton.Margin;
            margin.Left = marginWidth;
            margin.Top = marginHeight;
            newButton.Margin = margin;
            return newButton;
        }

        private void AddChannelsToGUI(string[] channelsName)
        {
            double marginWidth = 10;
            double marginHeight = 10;
            for (int i = 0; i < channelsName.Length; i++)
            {
                Button button = CreateButton(channelsName[i], marginWidth, marginHeight);
                button.Click += channelsButtonClicked;
                gridChannels.Children.Add(button);
                marginWidth += button.Width + 25;
            }
        }

        private void channelsButtonClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string channelNameSelectedPrev = this.gestureDetector.ChannelName;
            string userNameSelectedPrev = this.gestureDetector.UserName;
            string channelNameSelectedNow = button.Content as string;
            Console.WriteLine("Channel Selected: " + this.gestureDetector.ChannelName);
            Console.WriteLine("User Selected: " + this.gestureDetector.UserName);
            if (channelNameSelectedNow.Equals(channelNameSelectedPrev))
            {
                button.Background = Brushes.DarkTurquoise;
                this.gestureDetector.ChannelName = null;
                
                return;
            }
            this.gestureDetector.ChannelName = button.Content as string;
            this.gestureDetector.UserName = null;
            if (channelNameSelectedPrev != null)
            {
                for (int i = 0; i < gridChannels.Children.Count; i++)
                {
                    Button children = gridChannels.Children[i] as Button;
                    string content = children.Content as string;
                    if (content.Equals(channelNameSelectedPrev))
                    {
                        children.Background = Brushes.DarkTurquoise;
                        break;
                    }
                }
            }
            if (userNameSelectedPrev != null)
            {
                for (int i = 0; i < gridUsers.Children.Count; i++)
                {
                    Button children = gridUsers.Children[i] as Button;
                    string content = children.Content as string;
                    if (content.Equals(userNameSelectedPrev))
                    {
                        children.Background = Brushes.DarkTurquoise;
                        break;
                    }
                }
            }
            button.Background = Brushes.LimeGreen;
            Console.WriteLine("Channel Selected: " + this.gestureDetector.ChannelName);
            Console.WriteLine("User Selected: " + this.gestureDetector.UserName);
        }

        private void AddUsersToGUI(string[] usersName)
        {
            double marginWidth = 10;
            double marginHeight = 10;
            for (int i = 0; i < usersName.Length; i++)
            {
                Button newButton = CreateButton(usersName[i], marginWidth, marginHeight);
                newButton.Click += usersButtonClicked;
                gridUsers.Children.Add(newButton);
                marginWidth += newButton.Width + 25;
            }
        }

        private void usersButtonClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string userNameSelectedPrev = this.gestureDetector.UserName;
            string channelNameSelectedPrev = this.gestureDetector.ChannelName;
            string userNameSelectedNow = button.Content as string;
            Console.WriteLine("User Selected: " + this.gestureDetector.UserName);
            Console.WriteLine("Channel Selected: " + this.gestureDetector.ChannelName);
            if (userNameSelectedNow.Equals(userNameSelectedPrev))
            {
                button.Background = Brushes.DarkTurquoise;
                this.gestureDetector.UserName = null;
                Console.WriteLine("User Selected: " + this.gestureDetector.UserName);
                Console.WriteLine("Channel Selected: " + this.gestureDetector.ChannelName);
                return;
            }
            this.gestureDetector.UserName = button.Content as string;
            this.gestureDetector.ChannelName = null;
            if (userNameSelectedPrev != null)
            {
                for (int i = 0; i < gridUsers.Children.Count; i++)
                {
                    Button children = gridUsers.Children[i] as Button;
                    string content = children.Content as string;
                    if (content.Equals(userNameSelectedPrev))
                    {
                        children.Background = Brushes.DarkTurquoise;
                        break;
                    }
                }
            }
            if (channelNameSelectedPrev != null)
            {
                for (int i = 0; i < gridChannels.Children.Count; i++)
                {
                    Button children = gridChannels.Children[i] as Button;
                    string content = children.Content as string;
                    if (content.Equals(channelNameSelectedPrev))
                    {
                        children.Background = Brushes.DarkTurquoise;
                        break;
                    }
                }
            }
            button.Background = Brushes.LimeGreen;
            Console.WriteLine("User Selected: " + this.gestureDetector.UserName);
            Console.WriteLine("Channel Selected: " + this.gestureDetector.ChannelName);
        }

        public void ChangeColorBTNUserSelected(string userName)
        {
            for (int i = 0; i < gridUsers.Children.Count; i++)
            {
                Button children = gridUsers.Children[i] as Button;
                string content = children.Content as string;
                if (content.Equals(userName))
                {
                    children.Background = Brushes.DarkTurquoise;
                    break;
                }
            }
        }

        public void ChangeColorBTNChannelSelected(string channelName)
        {
            for (int i = 0; i < gridChannels.Children.Count; i++)
            {
                Button children = gridChannels.Children[i] as Button;
                string content = children.Content as string;
                if (content.Equals(channelName))
                {
                    children.Background = Brushes.DarkTurquoise;
                    break;
                }
            }
        }

        public void AddGuild(Guild guild)
        {
            this.guildList.Add(guild);
            if (this.gridGuilds.Children.Count == 0)
            {
                Button button = CreateButton(guild.Name, 10.0, 10.0);
                button.Click += guildsButtonClicked;
                this.gridGuilds.Children.Add(button);
            }
            else
            {
                int numChildren = this.gridGuilds.Children.Count;
                Button button = this.gridGuilds.Children[numChildren-1] as Button;
                Button newButton = CreateButton(guild.Name, button.Margin.Left+button.Width+25.0, button.Margin.Top*1.0);
                newButton.Click += guildsButtonClicked;
                this.gridGuilds.Children.Add(newButton);
            }
        }

        private void guildsButtonClicked(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Background == Brushes.LimeGreen)
                return;
            string guildName = button.Content.ToString();
            Guild guild = guildList.Find(x => x.Name == guildName);
            this.gridChannels.Children.Clear();
            Console.WriteLine(this.gridChannels.Children.Count);
            this.gridUsers.Children.Clear();
            AddChannelsToGUI(guild.Channels);
            AddUsersToGUI(guild.Users);
            button.Background = Brushes.LimeGreen;
            string lastGuildNameSelected = this.gestureDetector.GuildName;
            this.gestureDetector.GuildName = guild.Name;
            for (int i = 0; i < gridGuilds.Children.Count; i++)
            {
                Button children = gridGuilds.Children[i] as Button;
                string content = children.Content as string;
                if (content.Equals(lastGuildNameSelected))
                {
                    children.Background = Brushes.DarkTurquoise;
                    break;
                }
            }
        }

        private void helpButtonClicked(object sender, RoutedEventArgs e)
        {
            HelpWindow helpWindow = new HelpWindow(kinectSensor);
            helpWindow.Show();
            this.Deactivated -= Window_Deactivated;
        }

        public void AddDeactivatedHandler()
        {
            this.Deactivated += Window_Deactivated;
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

        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
            this.Activate();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            this.Topmost = true;
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Top = desktopWorkingArea.Bottom - this.Height;
            this.Left = desktopWorkingArea.Right - this.Width;
        }
    }
}
