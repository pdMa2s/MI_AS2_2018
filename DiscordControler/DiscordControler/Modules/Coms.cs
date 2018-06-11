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
        private NamedPipeClientStream _ttsPipeClientSpeech;
        private NamedPipeClientStream _ttsPipeClientGesture;

        private NamedPipeClientStream _guildInfoPipeClient;
        private StreamWriter writerSpeech;
        private StreamWriter writerGesture;

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
            if (_ttsPipeClientSpeech == null)
            {
                _ttsPipeClientSpeech = new NamedPipeClientStream("ttsCommandsSpeech");
                _ttsPipeClientSpeech.Connect();
                _ttsPipeClientGesture = new NamedPipeClientStream("ttsCommandsGesture");
                _ttsPipeClientGesture.Connect();

                writerSpeech = new StreamWriter(_ttsPipeClientSpeech);
                writerSpeech.AutoFlush = true;
                writerGesture = new StreamWriter(_ttsPipeClientGesture);
                writerGesture.AutoFlush = true;

            }

            try
            {
                writerSpeech.WriteLine(command);
            }
            catch (IOException e) {
                _retrySpeech(command);
            }

            try
            {
                writerGesture.WriteLine(command);
            }
            catch (IOException e)
            {
                _retryGesture(command);
            }


        }

        public void ClosePipe()
        {
            _ttsPipeClientSpeech.Close();
        }
        
        private void _retrySpeech(string command) {
            _ttsPipeClientSpeech.Close();
            _ttsPipeClientSpeech = new NamedPipeClientStream("ttsCommandsSpeech");
            _ttsPipeClientSpeech.Connect();

            
            writerSpeech = new StreamWriter(_ttsPipeClientSpeech);
            writerSpeech.AutoFlush = true;
            SendCommandToTts(command);
        }

        private void _retryGesture(string command)
        {
            
            _ttsPipeClientGesture.Close();
            _ttsPipeClientGesture = new NamedPipeClientStream("ttsCommandsGesture");
            _ttsPipeClientGesture.Connect();

            writerGesture = new StreamWriter(_ttsPipeClientSpeech);
            writerGesture.AutoFlush = true;
            SendCommandToTts(command);
        }

    }
}
