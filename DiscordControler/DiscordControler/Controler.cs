using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using mmisharp;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;
using System.Xml.Linq;

namespace DiscordControler
{
    class Controler
    {
        static void Main(string[] args) => new Controler().RunBotAsync().GetAwaiter().GetResult();
        
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _service;

        public async Task RunBotAsync() {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _service = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            string botToken = "NDI2NDI1OTUwMTU1MTc3OTk0.DZXWHA.0j3R_PJEqWgpdd2iiYVq7dP6jN0";


            _client.Log += Log;

            //oawait RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, botToken);

            await _client.StartAsync();

            await Task.Delay(-1);
        }

        
        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);

            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync() {

            _client.MessageReceived += HandleCommandAsync;
            _client.UserJoined += AnnouceUserJoined;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task AnnouceUserJoined(SocketGuildUser user)
        {
            var guild = user.Guild;
            var channel = guild.GetTextChannel(426423137073364995);
            await channel.SendMessageAsync($"Seja bem vindo, {user.Mention}");
        }


        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;

            if (message == null || message.Author.IsBot) return;

            int argumentPosition = 0;

            if (message.HasStringPrefix("amb!", ref argumentPosition) || message.HasMentionPrefix(_client.CurrentUser, ref argumentPosition)) {
                var context = new SocketCommandContext(_client, message);
                var result = await _commands.ExecuteAsync(context, argumentPosition, _service);

                if (!result.IsSuccess)
                    Console.WriteLine(result.ErrorReason);
                
            }

        }

        private void MmiC_Message(object sender, MmiEventArgs e)
        {
            Console.WriteLine(e.Message);
            var doc = XDocument.Parse(e.Message);
            var com = doc.Descendants("command").FirstOrDefault().Value;
            dynamic json = JsonConvert.DeserializeObject(com);
            Console.WriteLine(json);
            var action = json.recognized.action;
            Console.WriteLine(action);

            switch ((string)action)
            {
                case "CREATE_GUILD":
                    var guildName = json.recognized.guildName;
                    break;
                case "CREATE_CHANNEL":
                    var channelName = json.recognized.channelName;
                    var guildNameToAddChannel = json.recognized.guildName;
                    break;
                case "ADD_USER":
                    var usernameToAdd = json.recognized.userName;
                    var guildNameToAddUser = json.recognized.guildName;
                    break;
                case "REMOVE_USER":
                    var usernameToRemove = json.recognized.userName;
                    var guildNameToRemoveUser = json.recognized.guildName;
                    break;
                case "BAN_USER":
                    var usernameToBan = json.recognized.userName;
                    var guildNameToBanUser = json.recognized.guildName;
                    var reason = json.recognized.reason;
                    break;
                case "SEND_MESSAGE":
                    var channelNameToSendMsg = json.recognized.channelName;
                    var messageToAdd = json.recognized.message;
                    break;
                case "EDIT_MESSAGE":
                    var channelNameToEditMsg = json.recognized.channelName;
                    var messageEdited = json.recognized.message;
                    break;
                case "DELETE_LAST_MESSAGE":
                    var channelNameToDeleteMsg = json.recognized.channelName;
                    break;
                case "DELETE_CHANNEL":
                    var channelNameToDelete = json.recognized.channelName;
                    break;
                case "LEAVE_GUILD":
                    var guildNameToLeave = json.recognized.guildName;
                    break;
            }
        }
    }
}
