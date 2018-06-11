using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GestureModality
{
    class ComModule
    {
        private NamedPipeServerStream _ttsPipeServer;
        private NamedPipeServerStream _guildInfoServer;
        private bool ttsServerRunning;
        private readonly string comChannel = "ttsCommandsGesture";
        private bool ttsSpeaking = true;
        private MainWindow window;

        public ComModule(MainWindow window)
        {
            this._ttsPipeServer = new NamedPipeServerStream(comChannel);
            ttsServerRunning = false;
            _guildInfoServer = new NamedPipeServerStream("guildInfo");
            this.window = window;
            _listenCommands();
            _listenGuildInfoCommands();
        }

        public void KeepServerAlive() {
            if (!ttsServerRunning)
            {
                _ttsPipeServer = new NamedPipeServerStream(comChannel);
                _listenCommands();
            }
        }
        public bool IsTtsSpeaking() {
            return ttsSpeaking;
        }

        private void _listenGuildInfoCommands()
        {

            Task.Factory.StartNew(() =>
            {

                _guildInfoServer.WaitForConnection();
                StreamReader reader = new StreamReader(_guildInfoServer);
                int numberOfGuilds = -1; 
                int.TryParse(reader.ReadLine(), out numberOfGuilds);

                for (int i = 0; i < numberOfGuilds; i++) {
                    string guild = reader.ReadLine();
                    string channels = reader.ReadLine();
                    string users = reader.ReadLine();
                    _processGuildInfo(guild, channels, users);

                }
                _guildInfoServer.Close();
            });
        }

        private void _listenCommands()
        {

            Task.Factory.StartNew(() =>
            {
                ttsServerRunning = true;

                _ttsPipeServer.WaitForConnection();
                StreamReader reader = new StreamReader(_ttsPipeServer);

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    _processCommand(line);
                }
                _ttsPipeServer.Close();
                ttsServerRunning = false;

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

        private void _processGuildInfo(string guildName, string channels, string users)
        {
          
            string[] parsedChannels = channels.Split('|');
            string[] parsedUsers = users.Split('|');

            Application.Current.Dispatcher.Invoke((Action)delegate {
                
                Guild guild = new Guild(guildName);
                guild.Channels = parsedChannels;
                guild.Users = parsedUsers;

                Console.WriteLine(guild);
                window.AddGuild(guild);
            });
            
        }
    }
}
