using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using mmisharp;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Xml.Linq;
using Discord.Rest;

namespace DiscordControler
{
    class Controler
    {
        static void Main(string[] args) => new Controler().RunBotAsync().GetAwaiter().GetResult();

        private readonly ulong _defaultChannelId = 426423137073364995;
        private readonly ulong _defaultGuildId = 426423137073364993;
        private readonly ulong _userID = 414166532143316992;

        
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _service;
        private MmiCommunication _comModule;
        public async Task RunBotAsync() {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _comModule = new Coms().GetMmic();
            _service = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();
           
            string botToken = "NDI2NDI1OTUwMTU1MTc3OTk0.DZXWHA.0j3R_PJEqWgpdd2iiYVq7dP6jN0";

            _client.Log += Log;

            //await RegisterCommandsAsync(); // handle text commands
            _comModule.Message += MmiC_Message; // subscribe to the messages that come from the comMudole
            _comModule.Start();

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
            var channel = guild.GetTextChannel(_defaultChannelId);
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

        private async void MmiC_Message(object sender, MmiEventArgs e)
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
                    var guildName = json.recognized.guildName.ToString() as String;
                    await CreateGuild(guildName);
                    break;
                case "CREATE_CHANNEL":
                    var channelName = json.recognized.channelName.ToString() as String;
                    var guildNameToAddChannel = json.recognized.guildName.ToString() as String;
                    await CreateChannel(channelName, guildNameToAddChannel);
                    break;
                case "ADD_USER":
                    var usernameToAdd = json.recognized.userName.ToString() as String;
                    var guildNameToAddUser = json.recognized.guildName.ToString() as String;
                    break;
                case "REMOVE_USER":
                    var usernameToRemove = json.recognized.userName.ToString() as String;
                    var guildNameToRemoveUser = json.recognized.guildName.ToString() as String;
                    break;
                case "BAN_USER":
                    var usernameToBan = json.recognized.userName.ToString() as String;
                    var guildNameToBanUser = json["guildName"] == null ? null : json.recognized.guildName as String;
                    var reason = json["reason"] == null ? null : json.recognized.reason.ToString() as String;
                    await BanUser(usernameToBan, guildNameToBanUser, reason);
                    break;
                case "SEND_MESSAGE":
                    var channelNameToSendMsg = json.recognized.channelName.ToString() as String;
                    var messageToAdd = json.recognized.message.ToString() as String;
                    break;
                case "EDIT_MESSAGE":
                    var channelNameToEditMsg = json.recognized.channelName.ToString() as String;
                    var messageEdited = json.recognized.message.ToString() as String;
                    break;
                case "DELETE_LAST_MESSAGE":
                    var channelNameToDeleteMsg = json.recognized.channelName.ToString() as String;
                    break;
                case "DELETE_CHANNEL":
                    var channelNameToDelete = json.recognized.channelName.ToString() as String;
                    break;
                case "LEAVE_GUILD":
                    var guildNameToLeave = json.recognized.guildName.ToString() as String;
                    break;
            }
        }

        private async Task CreateGuild(string guildName)
        {
            string regionID = "eu-west";
            IVoiceRegion region = _client.GetVoiceRegion(regionID);
            var guildCreated = await _client.CreateGuildAsync(guildName, region);

            var channels = await guildCreated.GetChannelsAsync();
            var channelId = channels.First().Id;
            var channelDefault = await guildCreated.GetChannelAsync(channelId);
            var inviteObject = await channelDefault.CreateInviteAsync();
            var urlInvite = inviteObject.Url;

            var user = _client.GetUser(_userID);
            var channelPrivate = await user.GetOrCreateDMChannelAsync();
            await channelPrivate.SendMessageAsync("Guild criada com sucesso!");
            await channelPrivate.SendMessageAsync($"Link {urlInvite}");
            Console.WriteLine("guild criada com sucesso");
        }

        private async Task CreateChannel(string channelName, string guildNameToAddChannel)
        {
            var guildsOfClient = _client.Guilds;
            var guildsFiltered = guildsOfClient.Where(guild => guild.Name.Equals(guildNameToAddChannel));
            Console.WriteLine(guildsFiltered.Count());
            if (guildsFiltered.Count() == 0)
            {
                Console.WriteLine("Não existe nenhuma guild com esse nome!");
            }
            else
            {
                var guild = guildsFiltered.First();
                var response = await guild.CreateTextChannelAsync(channelName);
              
                Console.WriteLine("Channel criado com sucesso!");
            }
        }

        private async Task BanUser(string userName, string guildName, string banReason) {
            var guild = FindGuild(guildName);
            var user = FindUser(guild, userName);

            if (user == null) {
                await guild.GetTextChannel(_defaultChannelId).SendMessageAsync("Esse utilizador não existe!");
                Console.WriteLine("O utilizador não existe!");
                return;
            }

            if (banReason == null)
                await guild.AddBanAsync(user.Id);
            else
                await guild.AddBanAsync(user.Id, reason:banReason);

            await guild.GetTextChannel(_defaultChannelId).SendMessageAsync($"Já podes dizer adeus ao {userName}!");
            
            Console.WriteLine($"Já podes dizer adeus ao {userName}!");
        }
        private SocketGuild FindGuild(string guildName) {
            var guildsOfClient = _client.Guilds;
            var guildsFiltered = guildsOfClient.Where(g => g.Name.Equals(guildName));
            var guild = (SocketGuild)null;
            if (guildsFiltered.Count() == 0)
            {
                guild = _client.GetGuild(_defaultGuildId);
            }
            else
            {
                guild = guildsFiltered.First();
            }
            return guild;
        }
        private SocketUser FindUser(SocketGuild guild , string userName) {
            foreach (var u in guild.Users)
            {
                if (u.Username.Equals(userName))
                    return u;
            }
            return null;
        }
    }
}
