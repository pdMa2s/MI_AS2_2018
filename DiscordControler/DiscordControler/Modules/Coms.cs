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
            
            mmiC = new MmiCommunication("localhost", 8000, "User1", "GUI");
            //mmiC.Message += MmiC_Message;
            //mmiC.Start();
            _speechmodalityPipeClient = new NamedPipeClientStream("ttsCommands");
            _speechmodalityPipeClient.Connect();
            writer = new StreamWriter(_speechmodalityPipeClient);

        }
        public MmiCommunication GetMmic() {
            return mmiC;
        }

        public void SendCommandToTts(string command) {
            writer.WriteLine(command);
        }

        public void ClosePipe()
        {
            _speechmodalityPipeClient.Close();
        }
        
    }
}
