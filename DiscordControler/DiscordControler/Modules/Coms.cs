using mmisharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DiscordControler
{
    class Coms
    {
        private MmiCommunication mmiC;
        private NamedPipeClientStream _speechmodalityPipeClient;
        private StreamWriter writer;

        public Coms() {
            mmiC = new MmiCommunication("localhost", 8000, "User2", "GUI");
        }
        public MmiCommunication GetMmic() {
            return mmiC;
        }

        public void SendCommandToTts(string command) {
            if (_speechmodalityPipeClient == null)
            {
                _speechmodalityPipeClient = new NamedPipeClientStream("ttsCommands");
                _speechmodalityPipeClient.Connect();
                writer = new StreamWriter(_speechmodalityPipeClient);
                writer.AutoFlush = true;

            }
            try
            {
                writer.WriteLine(command);
            }
            catch (IOException e) {
                _retry(command);
            }


        }

        public void ClosePipe()
        {
            _speechmodalityPipeClient.Close();
        }
        
        private void _retry(string command) {
            _speechmodalityPipeClient.Close();
            _speechmodalityPipeClient = new NamedPipeClientStream("ttsCommands");
            _speechmodalityPipeClient.Connect();
            writer = new StreamWriter(_speechmodalityPipeClient);
            writer.AutoFlush = true;
            SendCommandToTts(command);
        }

    }
}
