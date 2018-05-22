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
        private readonly string comChannel = "ttsCommands";
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

                string channels = reader.ReadLine();
                string users = reader.ReadLine();
                _processGuildInfo(channels, users);
                
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

        private void _processGuildInfo(string channels, string users)
        {
            Console.WriteLine(users);
            string[] parsedChannels = channels.Split('|');
            string[] parsedUsers = users.Split('|');

            Application.Current.Dispatcher.Invoke((Action)delegate {
                // your code
                //window.AddChannelsToGUI(parsedChannels);
                //window.AddUsersToGUI(parsedUsers);
                Guild guild = new Guild("cenas");
                guild.Channels = parsedChannels;
                guild.Users = parsedUsers;
                window.AddGuild(guild);
            });
            
        }
    }
}
