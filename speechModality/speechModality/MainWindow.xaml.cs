using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace speechModality
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private SpeechMod _sm;
        private NamedPipeServerStream _pipeServer;
        private bool serverThreadRunning;
        public MainWindow()
        {
            InitializeComponent();
            _pipeServer = new NamedPipeServerStream("ttsCommands");
            _sm = new SpeechMod();
            serverThreadRunning = true;
            _listenTts();
            _sm.Recognized += _sm_Recognized;
        }

        private void _sm_Recognized(object sender, SpeechEventArg e)
        {
            if (serverThreadRunning == false)
            {
                _pipeServer = new NamedPipeServerStream("ttsCommands");

                _listenTts();
            }
                
            result.Text = e.Text;
            confidence.Text = e.Confidence + "";
            if (e.Final) result.FontWeight = FontWeights.Bold;
            else result.FontWeight = FontWeights.Normal;
            
        }

        private void _listenTts() {
            
            Task.Factory.StartNew(() =>
            {
                serverThreadRunning = true;

                _pipeServer.WaitForConnection();
                StreamReader reader = new StreamReader(_pipeServer);
                
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    _processCommand(line);
                }
                _pipeServer.Close();
                serverThreadRunning = false;

            });
        }

        private void _processCommand(string command) {
            Console.WriteLine("------------"+command+"------------------------");
            
                
            switch (command) {
                case "ttsSpeaking":
                    _sm.stopListening();
                    break;
                case "ttsNotSpeaking":
                    _sm.startListening();
                    break;
                default:
                    Console.WriteLine("Invalid command!");
                    break;
            }
        }
    }
}
