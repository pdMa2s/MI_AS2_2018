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
        
        private bool _isTtsSpeaking;
        public MainWindow()
        {
            InitializeComponent();
            _pipeServer = new NamedPipeServerStream("ttsCommands");
            
            
            _isTtsSpeaking = false;

            _sm = new SpeechMod();
            _sm.Recognized += _sm_Recognized;
        }

        private void _sm_Recognized(object sender, SpeechEventArg e)
        {
            if (!_isTtsSpeaking)
            {
                result.Text = e.Text;
                confidence.Text = e.Confidence + "";
                if (e.Final) result.FontWeight = FontWeights.Bold;
                else result.FontWeight = FontWeights.Normal;
            }
            
        }

        private void _listenTts() {
            Task.Factory.StartNew(() =>
            {
                _pipeServer.WaitForConnection();
                StreamReader reader = new StreamReader(_pipeServer);
                while (true)
                {
                    var line = reader.ReadLine();
                    _processCommand(line);
                }
            });
        }

        private void _processCommand(string command) {
            switch (command) {
                case "ttsSpeaking":
                    _isTtsSpeaking = true;
                    _sm.listen = false;
                    break;
                case "ttsNotSpeaking":
                    _isTtsSpeaking = false;
                    _sm.listen = true;
                    break;
                default:
                    Console.WriteLine("Invalid command!");
                    break;
            }
        }
    }
}
