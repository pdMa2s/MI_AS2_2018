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
using Discord.Rest;
using DiscordControler.Modules;

namespace DiscordControler
{
    class Controller
    {
        static void Main(string[] args) => new Controller().RunBotAsync().GetAwaiter().GetResult();

        private readonly ulong _defaultChannelId = 426423137073364995;
        private readonly ulong _defaultGuildId = 426423137073364993;
        private readonly ulong _userID = 414166532143316992;
        private readonly string _userNick = "IMStudent2018";
        
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _service;
        private MmiCommunication _comModule;
        private Tts _tts;
        private dynamic lastJsonMessage;
        public async Task RunBotAsync() {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _comModule = new Coms().GetMmic();
            _service = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();
            _tts = new Tts();
            _tts.Speak("otorrinolaringologista");
            string botToken = "NDMxNTg4NTczMTI5NjA1MTIw.Dag8Yw.lW9VrG3H8cJLiFv8rg0eUBkvwBY";

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
            var confidence = json.recognized.condidence.ToString() as String;
            Console.WriteLine(action);

            if (confidence.Equals("low confidence"))
                _tts.Speak("Desculpa, não percebi! Podes repetir se faz favor?");
            else if (confidence.Equals("explicit confimation"))
                lastJsonMessage = json;
            else
            {
                switch ((string)action)
                {
                    case "REMOVE_USER":
                        var usernameToRemove = json.recognized.userName.ToString() as String;
                        var guildNameToRemoveUser = json["guildName"] == null ? null : json.recognized.guildName.ToString() as String;
                        var kickReason = json["reason"] == null ? null : json.recognized.reason.ToString() as String;
                        await KickUser(usernameToRemove, guildNameToRemoveUser, kickReason, confidence);
                        break;
                    case "BAN_USER": //fica
                        var usernameToBan = json.recognized.userName.ToString() as String;
                        var guildNameToBanUser = json["guildName"] == null ? null : json.recognized.guildName as String;
                        var banReason = json["reason"] == null ? null : json.recognized.reason.ToString() as String;
                        await BanUser(usernameToBan, guildNameToBanUser, banReason, confidence);
                        break;
                    case "DELETE_LAST_MESSAGE": //fica
                        var channelNameToDeleteMsg = json.recognized.channelName.ToString() as String;
                        var guildNameToDeleteMsg = json["guildName"] == null ? null : json.recognized.guildName as String;
                        await DeleteLastMessage(channelNameToDeleteMsg, guildNameToDeleteMsg, confidence);
                        break;
                    case "DELETE_CHANNEL":  //fica
                        var channelNameToDelete = json.recognized.channelName.ToString() as String;
                        var guildNameToDeleteChannel = json["guildName"] == null ? null : json.recognized.guildName as String;
                        await DeleteChanel(channelNameToDelete, guildNameToDeleteChannel, confidence);
                        break;
                    case "LEAVE_GUILD": //fica
                        var guildNameToLeave = json.recognized.guildName.ToString() as String;
                        await LeaveGuild(guildNameToLeave, confidence);
                        break;
                    case "REMOVE_BAN": //fica
                        var userNameToRemBan = json.recognized.userName.ToString() as String;
                        var guildNameToRemBan = json["guildName"] == null ? null : json.recognized.guildName as String;
                        await RemoveBan(userNameToRemBan, guildNameToRemBan, confidence);
                        break;
                    case "USER_STATUS":
                        var userNameToKnowStatus = json.recognized.userName.ToString() as String;
                        var guildNameToKnowStatus = json["guildName"] == null ? null : json.recognized.guildName as String;
                        UserStatus(userNameToKnowStatus, guildNameToKnowStatus, confidence);
                        break;
                    case "MUTE_USER":
                        var userNameToMute = json.recognized.userName.toString() as String;
                        var guildNameToMuteUser = json["guildName"] == null ? null : json.recognized.guildName as String;
                        await ChangeMuteUser(userNameToMute, guildNameToMuteUser, true, confidence);
                        break;
                    case "DEAF_USER":
                        var userNameToDeaf = json.recognized.userName.toString() as String;
                        var guildNameToDeafUser = json["guildName"] == null ? null : json.recognized.guildName as String;
                        await ChangeDeafUser(userNameToDeaf, guildNameToDeafUser, true, confidence);
                        break;
                    case "UNMUTE_USER":
                        var userNameToUnMute = json.recognized.userName.toString() as String;
                        var guildNameToUnMuteUser = json["guildName"] == null ? null : json.recognized.guildName as String;
                        await ChangeMuteUser(userNameToUnMute, guildNameToUnMuteUser, false, confidence);
                        break;
                    case "UNDEAF_USER":
                        var userNameToUnDeaf = json.recognized.userName.toString() as String;
                        var guildNameToUnDeafUser = json["guildName"] == null ? null : json.recognized.guildName as String;
                        await ChangeDeafUser(userNameToUnDeaf, guildNameToUnDeafUser, false, confidence);
                        break;
                    case "UNMUTE_UNDEAF_USER":
                        var userNameToUnMuteUnDeaf = json.recognized.userName.toString() as String;
                        var guildNameToUnMuteUnDeafUser = json["guildName"] == null ? null : json.recognized.guildName as String;
                        await ChangeMuteDeafUser(userNameToUnMuteUnDeaf, guildNameToUnMuteUnDeafUser, false, confidence);
                        break;
                    case "MUTE_DEAF_USER":
                        var userNameToMuteDeaf = json.recognized.userName.toString() as String;
                        var guildNameToMuteDeafUser = json["guildName"] == null ? null : json.recognized.guildName as String;
                        await ChangeMuteDeafUser(userNameToMuteDeaf, guildNameToMuteDeafUser, true, confidence);
                        break;
                }
            }
        }

        private async Task KickUser(string userName, string guildName, string kickReason, string confidence)
        {
            var guild = FindGuild(guildName);
            var user = FindUser(guild, userName);

            if (user == null)
            {
                Console.WriteLine("Não sei de quem falas!");
                return;
            }

            await user.KickAsync(reason: kickReason);
            Console.WriteLine($"O {userName} vai lá fora apanhar ar.");
        }

        private async Task BanUser(string userName, string guildName, string banReason, string confidence)
        {
            var guild = FindGuild(guildName);
            var user = FindUser(guild, userName);

            if (user == null)
            {
                Console.WriteLine("O utilizador não existe!");
                return;
            }
            await guild.AddBanAsync(user.Id, reason: banReason);

            Console.WriteLine($"Já podes dizer adeus ao {userName}!");
        }

        private async Task DeleteLastMessage(string channelName, string guildNameToDeleteMsg, string confidence)
        {
            var guild = FindGuild(guildNameToDeleteMsg);
            var channel = (SocketTextChannel)FindChannel(guild, channelName);
            var message = await channel.GetMessagesAsync(1).First();
            Console.WriteLine(message);
        }

        private async Task DeleteChanel(string channelName, string guildName, string confidence)
        {
            var guild = FindGuild(guildName);
            var channel = FindChannel(guild, channelName);
            await channel.DeleteAsync();
            Console.WriteLine("Canal apagado!");
        }

        private async Task LeaveGuild(string guildName, string confidence)
        {
            var guild = FindGuild(guildName);
            var user = FindUser(guild, _userNick);

            if (user == null)
            {
                Console.WriteLine("Não estás nessa guild!");
                return;
            }

            await user.KickAsync();

            Console.WriteLine("O pessoal da guild manda abraços.");
        }

        private async Task RemoveBan(string userNameToRemBan, string guildNameToRemBan, string confidence)
        {
            var guild = FindGuild(guildNameToRemBan);
            var user = FindUser(guild, userNameToRemBan);

            if (user == null)
            {
                Console.WriteLine("O utilizador não existe!");
                return;
            }
            var bans = await guild.GetBansAsync();
            var banToRemove = (RestBan)null;
            foreach (RestBan ban in bans)
            {
                if (ban.User.Username.Equals(userNameToRemBan))
                {
                    banToRemove = ban;
                    break;
                }
            }

            if (banToRemove == null)
            {
                Console.WriteLine($"Não existe nenhum ban ao utilizador {userNameToRemBan}");

            }
            else
            {
                await guild.RemoveBanAsync(user.Id);
                Console.WriteLine($"Foi removido o ban ao utilizador {userNameToRemBan}");
            }

        }

        private void UserStatus(string userName, string guildName, string confidence)
        {
            var guild = FindGuild(guildName);
            var user = FindUser(guild, userName);

            if (user == null)
            {
                Console.WriteLine("Desconheço essa pessoa.");
                return;
            }
            var status = user.Status;
            Console.WriteLine(status);
        }

        private async Task ChangeMuteUser(string userNameToMute, string guildNameToMuteUser, bool mute, string confidence)
        {
            var guild = FindGuild(guildNameToMuteUser);
            var user = FindUser(guild, userNameToMute);

            if (user == null)
            {
                Console.WriteLine("Desconheço essa pessoa.");
                return;
            }

            await user.ModifyAsync(x => x.Mute = mute);
            if (mute)
                Console.WriteLine("Fui tirada a voz ao user " + userNameToMute);
            else
                Console.WriteLine("Fui retomada a voz ao user "+userNameToMute);
        }

        private async Task ChangeDeafUser(string userNameToDeaf, string guildNameToDeafUser, bool deaf, string confidence)
        {
            var guild = FindGuild(guildNameToDeafUser);
            var user = FindUser(guild, userNameToDeaf);

            if (user == null)
            {
                Console.WriteLine("Desconheço essa pessoa.");
                return;
            }

            await user.ModifyAsync(x => x.Deaf = deaf);
            if (deaf)
                Console.WriteLine("Fui tirado os ouvidos ao user " + userNameToDeaf);
            else
                Console.WriteLine("Fui retomado os ouvidos ao user " + userNameToDeaf);
        }

        private async Task ChangeMuteDeafUser(string userNameToMuteDeaf, string guildNameToMuteDeafUser, bool muteDeaf, string confidence)
        {
            var guild = FindGuild(guildNameToMuteDeafUser);
            var user = FindUser(guild, userNameToMuteDeaf);

            if (user == null)
            {
                Console.WriteLine("Desconheço essa pessoa.");
                return;
            }

            await user.ModifyAsync(x => x.Deaf = muteDeaf);
            await user.ModifyAsync(x => x.Mute = muteDeaf);
            if (muteDeaf)
                Console.WriteLine("Fui tirado os ouvidos e a voz ao user " + userNameToMuteDeaf);
            else
                Console.WriteLine("Fui retomado os ouvidos e a voz ao user " + userNameToMuteDeaf);
        }

        private SocketGuild FindGuild(string guildName) {
            if(guildName == null)
                return _client.GetGuild(_defaultGuildId);
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

        private SocketGuildUser FindUser(SocketGuild guild , string userName) {
            foreach (var u in guild.Users)
            {
                if (u.Username.Equals(userName))
                    return u;
            }
            return null;
        }

        private SocketGuildChannel FindChannel(SocketGuild guild, string channelName) {
            foreach (var c in guild.Channels) {
                if (c.Name.Equals(channelName))
                    return c;
            }
            return null;
        }
    }
}
