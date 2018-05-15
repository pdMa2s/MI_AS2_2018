using Discord.WebSocket;
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
        private NamedPipeClientStream _ttsPipeClient;
        private NamedPipeClientStream _guildInfoPipeClient;
        private StreamWriter writer;

        public Coms() {
            mmiC = new MmiCommunication("localhost", 8000, "User2", "GUI");
            _guildInfoPipeClient = new NamedPipeClientStream("guildInfo");
        }
        public MmiCommunication GetMmic() {
            return mmiC;
        }

        public void SendGuildInfo(IReadOnlyCollection<SocketTextChannel> channels)
        {

            Task.Factory.StartNew(() =>
            {
                _guildInfoPipeClient.Connect();
                StreamWriter writer = new StreamWriter(_guildInfoPipeClient);
                writer.AutoFlush = true;

                StringBuilder sbChannels = new StringBuilder();
                foreach(SocketTextChannel s in channels)
                {
                    sbChannels.Append(s.ToString() + "|");
                }
               
                writer.WriteLine(sbChannels.ToString());

                _guildInfoPipeClient.Close();
                
                Console.WriteLine("Guild info sent");
            });
        }

        public void SendCommandToTts(string command) {
            if (_ttsPipeClient == null)
            {
                _ttsPipeClient = new NamedPipeClientStream("ttsCommands");
                _ttsPipeClient.Connect();
                writer = new StreamWriter(_ttsPipeClient);
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
            _ttsPipeClient.Close();
        }
        
        private void _retry(string command) {
            _ttsPipeClient.Close();
            _ttsPipeClient = new NamedPipeClientStream("ttsCommands");
            _ttsPipeClient.Connect();
            writer = new StreamWriter(_ttsPipeClient);
            writer.AutoFlush = true;
            SendCommandToTts(command);
        }

    }
}
