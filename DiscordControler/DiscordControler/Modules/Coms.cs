using Discord.WebSocket;
using mmisharp;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private readonly string userNick;

        public Coms(string userNick) {
            mmiC = new MmiCommunication("localhost", 8000, "User1", "GUI");
            _guildInfoPipeClient = new NamedPipeClientStream("guildInfo");
            this.userNick = userNick;
        }
        public MmiCommunication GetMmic() {
            return mmiC;
        }

        public void SendGuildInfo(IReadOnlyCollection<SocketGuild> guilds)
        {

            Task.Factory.StartNew(() =>
            {
                _guildInfoPipeClient.Connect();
                StreamWriter writer = new StreamWriter(_guildInfoPipeClient);
                writer.AutoFlush = true;

                Regex regex = new Regex(@"\s+");

                writer.WriteLine(guilds.Count);

                foreach (SocketGuild g in guilds) {

                    StringBuilder sbChannels = new StringBuilder();
                    StringBuilder sbUsers = new StringBuilder();

                    foreach (SocketTextChannel s in g.TextChannels)
                    {
                        Match match = regex.Match(s.ToString());

                        if (!match.Success)
                        {
                            sbChannels.Append(s.ToString() + "|");

                        }

                    }

                    foreach (SocketUser u in g.Users)
                    {
                        Match match = regex.Match(u.ToString());

                        if (!match.Success && !u.Username.Equals("wally") && !u.Username.Equals(userNick))
                        {
                            sbUsers.Append(u.Username + "|");
                        }
                    }
                    writer.WriteLine(g.Name);
                    if (sbChannels.Length != 0 || sbUsers.Length != 0)
                    {
                        writer.WriteLine(sbChannels.ToString().Substring(0, sbChannels.Length - 1));
                        writer.WriteLine(sbUsers.ToString().Substring(0, sbUsers.Length - 1));
                    }

                }


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
