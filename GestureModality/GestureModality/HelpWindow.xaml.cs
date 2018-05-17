using Microsoft.Kinect;
using Microsoft.Kinect.Wpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GestureModality
{
    /// <summary>
    /// Interaction logic for HelpWindow.xaml
    /// </summary>
    public partial class HelpWindow : Window
    {

        private KinectSensor kinectSensor;

        public HelpWindow(KinectSensor kinectSensor)
        {
            InitializeComponent();

            this.kinectSensor = kinectSensor;

            KinectRegion.SetKinectRegion(this, kinectRegionHelp);

            App app = ((App)Application.Current);
            app.KinectRegionHelp = kinectRegionHelp;

            // Use the default sensor
            this.kinectRegionHelp.KinectSensor = this.kinectSensor;
        }

        private void closeHelpWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
