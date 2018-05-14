using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestureModality
{
    class ComModule
    {
        private NamedPipeServerStream _pipeServer;
        private bool serverRunning;
        private readonly string comChannel = "ttsCommands";
        private bool ttsSpeaking = true;
        public ComModule()
        {
            this._pipeServer = new NamedPipeServerStream(comChannel);
            serverRunning = false;
            _listenCommands();
        }

        public void KeepServerAlive() {
            if (!serverRunning)
            {
                _pipeServer = new NamedPipeServerStream(comChannel);
                _listenCommands();
            }
        }
        public bool IsTtsSpeaking() {
            return ttsSpeaking;
        }

        private void _listenCommands()
        {

            Task.Factory.StartNew(() =>
            {
                serverRunning = true;

                _pipeServer.WaitForConnection();
                StreamReader reader = new StreamReader(_pipeServer);

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    _processCommand(line);
                }
                _pipeServer.Close();
                serverRunning = false;

            });
        }

        private void _processCommand(string command)
        {
            Console.WriteLine("------------" + command + "------------------------");


            switch (command)
            {
                case "ttsSpeaking":
                    ttsSpeaking = true;
                    break;
                case "ttsNotSpeaking":
                    ttsSpeaking = false;
                    break;
                default:
                    Console.WriteLine("Invalid command!");
                    break;
            }
        }
    }
}
